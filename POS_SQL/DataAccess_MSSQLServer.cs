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
 
	// Use for Adv_POS.exe.config file   you can change Database server info after compile/Debug 
	//  static string _ConnectionString = supershop.Properties.Settings.Default.APOSSQLConnectionString;

	//Its absolute Connection String for MS SQL Server 2008 - Upto
	static string _ConnectionString = "Data Source=(local);Initial Catalog=APOSDB; User ID=sa;Password=sapass123!";
	    
					//"Data Source= (local) /or .\\SQLEXPRESS or your DB IP address or your SQL server name; for External database use only single dot  .\SQLEXPRESS or Data Source=.       --//(only dot)
					//Initial Catalog= Database Name; 
					//User ID= DB User ID;
					//Password= DB user password";
	    
	    //If your MSSQL server have window authentication (MSSQL server 2008 open without Password) please use this one 
	//static string _ConnectionString = "Data Source=(local); Initial Catalog=APOSDB; "; 

	// Connection String for  SQlite Edition
	//static string _ConnectionString = @"Data Source=psodb.db;Version=3;New=False;Compress=True";
							//Data Source=DemoT.db;Version=3;New=False;Compress=True;
	    
	//This is Mysql Database Access  class -- leave empty if your Mysal does not has PASSWORD       
	// static string _ConnectionString = "server=localhost; database=aposmysqldb; uid=root; PASSWORD=";

       static SqlConnection _Connection = null;
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
    }

}
