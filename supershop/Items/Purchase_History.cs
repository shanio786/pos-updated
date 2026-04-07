using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace supershop.Items
{
    public partial class Purchase_History : Form
    {

        public Purchase_History()
        {
            InitializeComponent();
 

            dtStartDate.Format = DateTimePickerFormat.Custom;
            dtStartDate.CustomFormat = "yyyy-MM-dd";

            dtEndDate.Format = DateTimePickerFormat.Custom;
            dtEndDate.CustomFormat = "yyyy-MM-dd";

            dtStartDate.Text = dtStartDate.Value.AddDays(-15).ToShortDateString();
            dtEndDate.Text   = dtEndDate.Value.AddDays(+15).ToShortDateString();
            string dtstart = DateTime.Now.AddDays(-15).ToString();
            string  dtend = DateTime.Now.AddDays(+15).ToString();

            datagrdpurchasehistory.EnableHeadersVisualStyles = false;
            datagrdpurchasehistory.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            datagrdpurchasehistory.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            datagrdpurchasehistory.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            databind(dtstart, dtend);
        }
      

        public void databind(string dtstartvl,string dtendvl)
        {
            try
            {
                string sql = " select id as ID, purchase_date as 'Date',  product_name as 'Name', cost_price as 'Price', product_quantity as 'Qty', " +
                         "  ((cost_price * product_quantity) * 1.00) as 'Total',  category as 'Category' , supplier , Shopid , ptype as 'Status'  " +
                         "  from tbl_purchase_history  where purchase_date   BETWEEN '" + dtstartvl + "' AND    '" + dtendvl + "' " +
                         " and status = 1 Order  by id desc";
                DataAccess.ExecuteSQL(sql);
                DataTable dt1 = DataAccess.GetDataTable(sql);
                datagrdpurchasehistory.DataSource = dt1;

                string sql3 = " select SUM(cost_price * product_quantity)   " +
                               "  from tbl_purchase_history where purchase_date   BETWEEN '" + dtstartvl + "' AND    '" + dtendvl + "' and status = 1 ";
                DataAccess.ExecuteSQL(sql3);
                DataTable dt3 = DataAccess.GetDataTable(sql3);

                DataRow dr = dt1.NewRow();
                dr[1] = " ";
                dt1.Rows.Add(dr);

                DataRow dr2 = dt1.NewRow();
                dr2[2] = "Total ";
                dr2[5] = Convert.ToDouble(dt3.Rows[0].ItemArray[0].ToString()) ;
                dt1.Rows.Add(dr2);
                
                DataRow dr7 = dt1.NewRow();
                dr7[1] = " ";
                dt1.Rows.Add(dr7);


                DataRow rep = dt1.NewRow();
                rep[1] = "Purchase Report ";                                 
                dt1.Rows.Add(rep);

                DataRow repdt = dt1.NewRow();              
                repdt[1] = "From : " + dtStartDate.Text;               
                dt1.Rows.Add(repdt);

                DataRow repdt2 = dt1.NewRow(); 
                repdt2[1] = "To : " + dtEndDate.Text; 
                dt1.Rows.Add(repdt2);
            }
            catch
            {
            }
         
        }

        //Filter Date to Date
        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            databind(dtStartDate.Text , dtEndDate.Text);
        }

        #region Printing
        DataGridViewPrinter MyDataGridViewPrinter;

        private bool SetupThePrinting()
        {
            string sql3 = "select * from tbl_terminalLocation where Shopid = '" + UserInfo.Shopid + "'";
            DataAccess.ExecuteSQL(sql3);
            DataTable dt1 = DataAccess.GetDataTable(sql3);

            DateTime dt = DateTime.Now;
            string printdate = dt.ToString("MMMM dd, yyyy    hh:mm:ss tt");
            string Companyname = dt1.Rows[0].ItemArray[1].ToString();
            string branchname = dt1.Rows[0].ItemArray[2].ToString();
            string Location = dt1.Rows[0].ItemArray[3].ToString();
            string phone = dt1.Rows[0].ItemArray[4].ToString();
            string email = dt1.Rows[0].ItemArray[5].ToString();
            string web = dt1.Rows[0].ItemArray[6].ToString();

            string Header = Companyname + "\n" + Location + "." + "\n" + email + "\n" + branchname + " ph: " + phone + "\n" + printdate + "\n";

            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;


            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                return false;

            printDocument1.DocumentName = "Purchase Report";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            //if (MessageBox.Show("Do you want the report to be centered on the page",
            //    "InvoiceManager - Center on Page", MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question) == DialogResult.Yes)
            MyDataGridViewPrinter = new DataGridViewPrinter(datagrdpurchasehistory,
            printDocument1, true, true, Header + " Purchase Report \n", new Font("Baskerville Old Face", 14,
            FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);


            return true;
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.datagrdpurchasehistory.RowsDefaultCellStyle.BackColor = Color.White;
                this.datagrdpurchasehistory.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

                if (SetupThePrinting())
                {
                    PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
                    MyPrintPreviewDialog.WindowState = FormWindowState.Maximized;
                    MyPrintPreviewDialog.PrintPreviewControl.Zoom = 1.0;
                    MyPrintPreviewDialog.Document = printDocument1;
                    MyPrintPreviewDialog.ShowDialog();                   
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("!!! Please Print Preview or Setup Print only for First time " + exp.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }
        #endregion

        private void datagrdpurchasehistory_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch
            {

            }
        }

        private void datagrdpurchasehistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = datagrdpurchasehistory.Rows[e.RowIndex];
                if (row.Cells[0].Value.ToString() != string.Empty)
                {
                    DialogResult result = MessageBox.Show(" Do you want to delete ?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                    if (result == DialogResult.Yes)
                    {
                        string sql = " update tbl_purchase_history set status = '2' where id ='" + row.Cells[0].Value.ToString() + "'";
                        DataAccess.ExecuteSQL(sql);
                        databind(dtStartDate.Text, dtEndDate.Text);
                    }
                }                    
                
            }
            catch // (Exception exp)
            {
                // MessageBox.Show("Sorry" + exp.Message);
            }
        }

    }
}
