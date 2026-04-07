using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Finisar.SQLite;
//using System.Data.SQLite;

namespace supershop 
{
	//////SQLite Edition
    class DataAccess
    {
	    
	    // Connection String for  SQlite Edition
	static string _ConnectionString = @"Data Source=psodb.db;Version=3;New=False;Compress=True";
							//Data Source=DemoT.db;Version=3;New=False;Compress=True;
	    
	// Use for Adv_POS.exe.config file -- you can change Database server info after compile/Debug 
	//   static string _ConnectionString = supershop.Properties.Settings.Default.psodbConnectionString1;

	//Its absolute Connection String for MS SQL Server 2008 - Upto
	//static string _ConnectionString = "Data Source=(local);Initial Catalog=APOSDB; User ID=aposdb;Password=aposdb!";
	    

	//This is Mysql Database Access  class -- leave empty if your Mysal does not has PASSWORD       
	// static string _ConnectionString = "server=localhost; database=aposmysqldb; uid=root; PASSWORD=";


	
  
        static SQLiteConnection _Connection = null;
        public static SQLiteConnection Connection
        {
            get
            {
                if (_Connection == null)
                {
                    _Connection = new SQLiteConnection(_ConnectionString);
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
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);

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
        }

        public static int ExecuteSQL(string sql)
        {
            SQLiteCommand cmd = new SQLiteCommand(sql, Connection);
            return cmd.ExecuteNonQuery();
        }
    }

}
