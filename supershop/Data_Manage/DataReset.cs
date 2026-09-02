using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.Data_Manage
{
    public partial class DataReset : Form
    {
        public DataReset()
        {
            InitializeComponent();
        }

        private void btntruncate_Click(object sender, EventArgs e)
        {
            try
            {
                 DialogResult result = MessageBox.Show("Do you want Reset Database ? \n you will be loss all Data", "YES or NO", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                 if (result == DialogResult.Yes)
                 {
                     if (rdbsqlite.Checked == true)
                     {
                         // MS SQL Server: DELETE keeps the tables, then reset the IDENTITY counters
                         // (equivalent of the old SQLite  "UPDATE SQLITE_SEQUENCE SET seq = 0")
                         string sql1 =  " DELETE FROM return_item;           DBCC CHECKIDENT ('return_item',          RESEED, 0); " +
                                        " DELETE FROM sales_item;            DBCC CHECKIDENT ('sales_item',           RESEED, 0); " +
                                        " DELETE FROM sales_payment; " +
                                        " DELETE FROM tbl_saleInfo;          DBCC CHECKIDENT ('tbl_saleInfo',         RESEED, 0); " +
                                        " DELETE FROM purchase; " +
                                        " DELETE FROM tbl_duepayment;        DBCC CHECKIDENT ('tbl_duepayment',       RESEED, 0); " +
                                        " DELETE FROM tbl_purchase_history;  DBCC CHECKIDENT ('tbl_purchase_history', RESEED, 0); " +
                                        " DELETE FROM tbl_workrecords;       DBCC CHECKIDENT ('tbl_workrecords',      RESEED, 0); ";

                         DataAccess.ExecuteSQL(sql1);
                         MessageBox.Show("Successfully truncated !!! ", "Succeed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     }
                     else
                     {
                         string sql1 =  " TRUNCATE TABLE return_item ; " + 
                                        " TRUNCATE TABLE sales_item ; " +
                                        " TRUNCATE TABLE sales_payment; " +
                                        " TRUNCATE TABLE tbl_saleInfo; " +
                                        " TRUNCATE TABLE purchase; " +
                                        " TRUNCATE TABLE tbl_duepayment; " +                                        
	                                    " TRUNCATE TABLE tbl_purchase_history; " +
	                                    " TRUNCATE TABLE tbl_workrecords; ";
                         DataAccess.ExecuteSQL(sql1);                          
                         MessageBox.Show("Successfully truncated !!! ", "Succeed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     }
                 }                        
            }
            catch (Exception exLog) { Logger.Error(exLog); }
              
        }
    }
}
