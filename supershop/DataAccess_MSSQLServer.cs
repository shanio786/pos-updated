using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace supershop
{
    /// <summary>
    /// MS SQL Server data access for Adv_POS.
    ///
    /// * One short-lived connection per call (ADO.NET pooling makes this the
    ///   fastest and safest pattern; nothing is shared between forms/threads).
    /// * Every method accepts optional SqlParameters:
    ///       DataAccess.GetDataTable("select * from usermgt where Username = @u", DataAccess.P("@u", name));
    /// * RunInTransaction() runs several statements atomically (sale + items + stock).
    ///
    /// The connection string comes from Adv_POS.exe.config
    /// (supershop.Properties.Settings.APOSSQLConnectionString).
    /// </summary>
    public static class DataAccess
    {
        static string _connectionString;

        public static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(_connectionString))
                    _connectionString = supershop.Properties.Settings.Default.APOSSQLConnectionString;
                return _connectionString;
            }
            set { _connectionString = value; }
        }

        /// <summary>Opens and returns a new connection. Caller must dispose it.</summary>
        public static SqlConnection OpenConnection()
        {
            SqlConnection cn = new SqlConnection(ConnectionString);
            cn.Open();
            return cn;
        }

        /// <summary>Shorthand to build a parameter: DataAccess.P("@name", value)</summary>
        public static SqlParameter P(string name, object value)
        {
            return new SqlParameter(name, value ?? DBNull.Value);
        }

        static SqlCommand BuildCommand(string sql, SqlConnection cn, SqlTransaction tx, SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(sql, cn);
            cmd.CommandTimeout = 60;
            if (tx != null) cmd.Transaction = tx;
            if (parameters != null)
            {
                foreach (SqlParameter p in parameters)
                {
                    if (p == null) continue;
                    if (p.Value == null) p.Value = DBNull.Value;
                    cmd.Parameters.Add(p);
                }
            }
            return cmd;
        }

        public static DataSet GetDataSet(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = BuildCommand(sql, cn, null, parameters))
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                DataSet ds = new DataSet();
                adp.Fill(ds);
                return ds;
            }
        }

        /// <summary>Runs a SELECT and returns the first result table (never null).</summary>
        public static DataTable GetDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = BuildCommand(sql, cn, null, parameters))
            using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
            {
                DataTable dt = new DataTable();
                adp.Fill(dt);
                return dt;
            }
        }

        /// <summary>Runs INSERT / UPDATE / DELETE and returns the affected row count.</summary>
        public static int ExecuteSQL(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = BuildCommand(sql, cn, null, parameters))
            {
                return cmd.ExecuteNonQuery();
            }
        }

        /// <summary>Returns the first column of the first row, or null.</summary>
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection cn = OpenConnection())
            using (SqlCommand cmd = BuildCommand(sql, cn, null, parameters))
            {
                object o = cmd.ExecuteScalar();
                return o == DBNull.Value ? null : o;
            }
        }

        /// <summary>Scalar as string ("" when NULL / no rows).</summary>
        public static string ExecuteSQLScaler(string sql, params SqlParameter[] parameters)
        {
            object o = ExecuteScalar(sql, parameters);
            return o == null ? "" : o.ToString();
        }

        /// <summary>Scalar as decimal (0 when NULL / no rows / not numeric).</summary>
        public static decimal GetDecimal(string sql, params SqlParameter[] parameters)
        {
            object o = ExecuteScalar(sql, parameters);
            if (o == null) return 0m;
            decimal d;
            return decimal.TryParse(Convert.ToString(o), out d) ? d : 0m;
        }

        /// <summary>
        /// Runs the given work inside one SQL transaction. Any exception rolls
        /// everything back and is re-thrown to the caller.
        /// </summary>
        public static void RunInTransaction(Action<DbTransaction> work)
        {
            using (SqlConnection cn = OpenConnection())
            using (SqlTransaction tx = cn.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                DbTransaction scope = new DbTransaction(cn, tx);
                try
                {
                    work(scope);
                    tx.Commit();
                }
                catch
                {
                    try { tx.Rollback(); } catch { }
                    throw;
                }
            }
        }

        /// <summary>Statements executed inside RunInTransaction().</summary>
        public sealed class DbTransaction
        {
            readonly SqlConnection _cn;
            readonly SqlTransaction _tx;

            internal DbTransaction(SqlConnection cn, SqlTransaction tx) { _cn = cn; _tx = tx; }

            public int Execute(string sql, params SqlParameter[] parameters)
            {
                using (SqlCommand cmd = BuildCommand(sql, _cn, _tx, parameters))
                    return cmd.ExecuteNonQuery();
            }

            public object Scalar(string sql, params SqlParameter[] parameters)
            {
                using (SqlCommand cmd = BuildCommand(sql, _cn, _tx, parameters))
                {
                    object o = cmd.ExecuteScalar();
                    return o == DBNull.Value ? null : o;
                }
            }

            public decimal ScalarDecimal(string sql, params SqlParameter[] parameters)
            {
                object o = Scalar(sql, parameters);
                if (o == null) return 0m;
                decimal d;
                return decimal.TryParse(Convert.ToString(o), out d) ? d : 0m;
            }

            public DataTable Query(string sql, params SqlParameter[] parameters)
            {
                using (SqlCommand cmd = BuildCommand(sql, _cn, _tx, parameters))
                using (SqlDataAdapter adp = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adp.Fill(dt);
                    return dt;
                }
            }

            /// <summary>
            /// Next invoice number, safe for several terminals at once: the
            /// UPDLOCK/HOLDLOCK keeps a second sale waiting until this one commits.
            /// </summary>
            public long NextSalesId()
            {
                object o = Scalar("SELECT ISNULL(MAX(sales_id), 0) + 1 FROM sales_payment WITH (UPDLOCK, HOLDLOCK)");
                return Convert.ToInt64(o);
            }
        }

        /// <summary>Quick connectivity test used by the login screen.</summary>
        public static bool TestConnection(out string error)
        {
            try
            {
                using (SqlConnection cn = OpenConnection()) { }
                error = null;
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
