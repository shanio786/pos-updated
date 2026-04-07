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
                         string sql1 =  "DELETE FROM return_item; " + 
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'return_item'; " + 
                                        " DELETE FROM sales_item; " +  
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'sales_item'; " +
                                        " DELETE FROM sales_payment;" +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'sales_payment'; " +
                                        " DELETE FROM tbl_saleInfo;" +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_saleInfo'; " +
                                        " DELETE FROM purchase;" +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'purchase'; " +
                                        " DELETE FROM tbl_duepayment;" +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_duepayment'; " +
                                        " DELETE FROM tbl_purchase_history; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_purchase_history'; " +
                                        " DELETE FROM tbl_workrecords; " +
                                        " UPDATE SQLITE_SEQUENCE SET seq = 0 WHERE name = 'tbl_workrecords'; ";

                         DataTable dt1 = DataAccess.GetDataTable(sql1);                          
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
                         DataTable dt1 = DataAccess.GetDataTable(sql1);                          
                         MessageBox.Show("Successfully truncated !!! ", "Succeed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                     }
                 }                        
            }
            catch
            {
            }
              
        }
    }
}
