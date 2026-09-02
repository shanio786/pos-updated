using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace supershop.Customer
{
    public partial class RewardsManagerReport : Form
    {
        public RewardsManagerReport()
        {
            InitializeComponent();
        }

        private void RewardsManagerReport_Load(object sender, EventArgs e)
        {
            try
            {
                Databind();

                dtStartDate.Format = DateTimePickerFormat.Custom;
                dtStartDate.CustomFormat = "yyyy-MM-dd";

                dtEndDate.Format = DateTimePickerFormat.Custom;
                dtEndDate.CustomFormat = "yyyy-MM-dd";
            }
            catch (Exception exLog) { Logger.Error(exLog); }

        }

        /// <summary>Appends a blank row and a "Store Credit Balance" total row to the grid table.</summary>
        private static void AddTotalRow(DataTable dt1, decimal creditSum)
        {
            DataRow dr = dt1.NewRow();
            dr[1] = " ";
            dt1.Rows.Add(dr);

            DataRow CreditTotal = dt1.NewRow();
            CreditTotal[3] = "Store Credit Balance";
            CreditTotal[5] = creditSum;
            dt1.Rows.Add(CreditTotal);
        }

        public void Databind()
        {
            DataTable dt1 = DataAccess.GetDataTable("SELECT * from vw_custcreditreport    order by TrxID  desc");
            dtGrdvCustomerDetails.DataSource = dt1;

            decimal creditSum = DataAccess.GetDecimal("SELECT ISNULL(Sum(Credit), 0) as CreditSum from vw_custcreditreport ");
            AddTotalRow(dt1, creditSum);
        }

        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string where = " where Name like '%' + @q + '%' or cast(CustID as varchar(20)) like '%' + @q + '%' or OrderID like '%' + @q + '%' ";

                DataTable dt1 = DataAccess.GetDataTable("Select * from  vw_custcreditreport " + where + " order by TrxID  desc",
                                                        DataAccess.P("@q", txtCustomerSearch.Text));
                dtGrdvCustomerDetails.DataSource = dt1;

                decimal creditSum = DataAccess.GetDecimal("SELECT ISNULL(Sum(Credit), 0) as CreditSum from vw_custcreditreport " + where,
                                                          DataAccess.P("@q", txtCustomerSearch.Text));
                AddTotalRow(dt1, creditSum);
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            ReportByDate(dtStartDate.Text,dtEndDate.Text);
        }


        public void ReportByDate(string StartDate, string EndDate)
        {
            try
            {
                // Date is stored as 'yyyy-MM-dd' text
                DataTable dt1 = DataAccess.GetDataTable("Select * from  vw_custcreditreport  where  Date BETWEEN @from AND @to    Order  by  TrxID desc",
                                                        DataAccess.P("@from", StartDate), DataAccess.P("@to", EndDate));
                dtGrdvCustomerDetails.DataSource = dt1;

                decimal creditSum = DataAccess.GetDecimal("SELECT ISNULL(Sum(Credit), 0) as CreditSum from vw_custcreditreport   where  Date BETWEEN @from AND @to",
                                                          DataAccess.P("@from", StartDate), DataAccess.P("@to", EndDate));
                AddTotalRow(dt1, creditSum);
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void btnAddNewCustLink_Click(object sender, EventArgs e)
        {  
            Customer.AddCredit go = new Customer.AddCredit();
            go.MdiParent = this.ParentForm;
            go.Show();
            this.Close();          
        }


        #region Print
        DataGridViewPrinter MyDataGridViewPrinter;

        private bool SetupThePrinting()
        {
            string sql3 = "select * from storeconfig";
            DataTable dt1 = DataAccess.GetDataTable(sql3);
            DateTime dt = DateTime.Now;
            string s = dt.ToString("MMMM dd, yyyy    hh:mm:ss tt");

            string sd = dt1.Rows[0].ItemArray[1].ToString() + "\n" + dt1.Rows[0].ItemArray[2].ToString() + "." + "\n" + dt1.Rows[0].ItemArray[3].ToString() + "\n" + s + "\n";

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

            printDocument1.DocumentName = "Credit Report";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            if (MessageBox.Show("Do you want the report to be centered on the page",
                "InvoiceManager - Center on Page", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
                MyDataGridViewPrinter = new DataGridViewPrinter(dtGrdvCustomerDetails,
                printDocument1, true, true, sd + " Store Credit Report \n", new Font("Baskerville Old Face", 13,
                FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);
            else
                MyDataGridViewPrinter = new DataGridViewPrinter(dtGrdvCustomerDetails,
                printDocument1, false, true, sd + "   Store Credit Report   \n", new Font("Times New Roman", 14, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);

            return true;
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtGrdvCustomerDetails.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtGrdvCustomerDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

                if (SetupThePrinting())
                {
                    PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
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
    }
}
