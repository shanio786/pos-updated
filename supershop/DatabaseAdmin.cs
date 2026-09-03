using System;
using System.Data.SqlClient;

namespace supershop
{
    /// <summary>
    /// Restore the database from a .bak file from inside the app.
    /// The restore runs on a connection to 'master' (you cannot restore the
    /// database you are currently using), puts APOSDB into single-user mode,
    /// restores with REPLACE, then returns it to multi-user.
    /// </summary>
    public static class DatabaseAdmin
    {
        public static string DatabaseName
        {
            get
            {
                try
                {
                    var b = new SqlConnectionStringBuilder(DataAccess.ConnectionString);
                    return string.IsNullOrEmpty(b.InitialCatalog) ? "APOSDB" : b.InitialCatalog;
                }
                catch { return "APOSDB"; }
            }
        }

        static string MasterConnectionString()
        {
            var b = new SqlConnectionStringBuilder(DataAccess.ConnectionString);
            b.InitialCatalog = "master";
            b.ConnectTimeout = 30;
            return b.ConnectionString;
        }

        /// <summary>Restores the database from bakPath. Throws on failure with a clear message.</summary>
        public static void Restore(string bakPath)
        {
            string db = DatabaseName;
            using (SqlConnection cn = new SqlConnection(MasterConnectionString()))
            {
                cn.Open();
                Exec(cn, "ALTER DATABASE [" + db + "] SET SINGLE_USER WITH ROLLBACK IMMEDIATE", 60);
                try
                {
                    using (SqlCommand cmd = new SqlCommand("RESTORE DATABASE [" + db + "] FROM DISK = @p WITH REPLACE, RECOVERY", cn))
                    {
                        cmd.Parameters.AddWithValue("@p", bakPath);
                        cmd.CommandTimeout = 0;   // a restore can take a while
                        cmd.ExecuteNonQuery();
                    }
                }
                finally
                {
                    try { Exec(cn, "ALTER DATABASE [" + db + "] SET MULTI_USER", 60); } catch (Exception ex) { Logger.Error("set multi_user", ex); }
                }
            }
        }

        static void Exec(SqlConnection cn, string sql, int timeout)
        {
            using (SqlCommand cmd = new SqlCommand(sql, cn)) { cmd.CommandTimeout = timeout; cmd.ExecuteNonQuery(); }
        }
    }
}
