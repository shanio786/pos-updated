using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
namespace supershop
{
	//////MS-SQL server  Edition
    class DataAccess
    {
 
        // MS SQL Server is the main (and only) database of this application.
        // The connection string is read from  Adv_POS.exe.config  (app.config while developing)
        //   <add name="supershop.Properties.Settings.APOSSQLConnectionString" ... />
        // so the server / database / login can be changed after install without recompiling.
        static string _ConnectionString = supershop.Properties.Settings.Default.APOSSQLConnectionString;

        // Examples:
        //   Windows authentication : Data Source=.\SQLEXPRESS;Initial Catalog=APOSDB;Integrated Security=True;
        //   SQL login              : Data Source=192.168.0.10,1433;Initial Catalog=APOSDB;User ID=posuser;Password=***;


        static SqlConnection _Connection = null;
        public SqlConnection conn;
        public static SqlConnection Connection
        {
            get
            {
                if (_Connection == null)
                {
                    _Connection = new SqlConnection(_ConnectionString);
                    _Connection.Open();

                    return _Connection;
                }
                else if (_Connection.State != System.Data.ConnectionState.Open)
                {
                    _Connection.Open();

                    return _Connection;
                }
                else
                {
                    return _Connection;
                }
            }
        }

        // this my created connection for showing report..no else used
        public SqlConnection OpenDBConn()
        {
            conn = new SqlConnection(_ConnectionString);
            conn.Open();
            return conn;
        }
        public static DataSet GetDataSet(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            SqlDataAdapter adp = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            adp.Fill(ds);
            Connection.Close();

            return ds;
        }

        public static DataTable GetDataTable(string sql)
        {
            Console.WriteLine(sql);
            DataSet ds = GetDataSet(sql);

            if (ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
			//https://skydrive.live.com/?cid=0331372fa6a33be3&resid=331372FA6A33BE3!386&id=331372FA6A33BE3%21386
        }

        public static int ExecuteSQL(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            return cmd.ExecuteNonQuery();
        }

        public static string ExecuteSQLScaler(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, Connection);
            return cmd.ExecuteScalar().ToString();
        }
    }

}
