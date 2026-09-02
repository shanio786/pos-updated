using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace supershop.Report
{
    /// <summary>
    /// General ledger: rows come from vw_General_Ledger (columns Date, Sales, [Return]).
    /// Balance = SUM(Sales) - SUM([Return]) over the same date range as the rows shown.
    /// </summary>
    public partial class LedgerReport : Form
    {
        public LedgerReport()
        {
            InitializeComponent();
        }

        private void LedgerReport_Load(object sender, EventArgs e)
        {
            try
            {
                dtStartDate.Format = DateTimePickerFormat.Custom;
                dtStartDate.CustomFormat = "yyyy-MM-dd";

                dtEndDate.Format = DateTimePickerFormat.Custom;
                dtEndDate.CustomFormat = "yyyy-MM-dd";

                Databind();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the ledger report."); }
        }

        /// <summary>Whole ledger.</summary>
        public void Databind()
        {
            LoadLedger(null, null);
        }

        /// <summary>Ledger for the inclusive date range.</summary>
        public void ReportByDate(string StartDate, string EndDate)
        {
            try
            {
                LoadLedger(StartDate, EndDate);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the ledger report."); }
        }

        void LoadLedger(string startDate, string endDate)
        {
            bool byDate = !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate);
            string where = byDate ? " WHERE [Date] BETWEEN @from AND @to " : " ";

            DataTable rows = DataAccess.GetDataTable(
                "SELECT [Date], Sales, [Return] FROM vw_General_Ledger" + where + " ORDER BY [Date] DESC",
                byDate ? new SqlParameter[] { DataAccess.P("@from", startDate), DataAccess.P("@to", endDate) } : new SqlParameter[0]);

            DataTable sums = DataAccess.GetDataTable(
                "SELECT ISNULL(SUM(Sales), 0), ISNULL(SUM([Return]), 0) FROM vw_General_Ledger" + where,
                byDate ? new SqlParameter[] { DataAccess.P("@from", startDate), DataAccess.P("@to", endDate) } : new SqlParameter[0]);

            decimal totalSales = 0, totalReturn = 0;
            if (sums.Rows.Count > 0)
            {
                totalSales = ToDec(sums.Rows[0][0]);
                totalReturn = ToDec(sums.Rows[0][1]);
            }

            // Display copy with string columns so the footer lines can hold text.
            DataTable dt1 = ToStringTable(rows);

            DataRow dr = dt1.NewRow();
            dr[0] = "______________________________________________ ";
            dt1.Rows.Add(dr);

            DataRow Total = dt1.NewRow();
            Total[0] = "Total = ";
            Total[1] = Money(totalSales);
            Total[2] = Money(totalReturn);
            dt1.Rows.Add(Total);

            DataRow Balance = dt1.NewRow();
            Balance[0] = "Balance = ";
            Balance[1] = Money(totalSales - totalReturn);
            dt1.Rows.Add(Balance);

            if (byDate)
            {
                DataRow dr3 = dt1.NewRow();
                dr3[0] = "______________________________________________ ";
                dt1.Rows.Add(dr3);
            }

            dtGrdLedgerReport.DataSource = dt1;
        }

        static DataTable ToStringTable(DataTable source)
        {
            DataTable dt = new DataTable();
            foreach (DataColumn c in source.Columns)
                dt.Columns.Add(c.ColumnName, typeof(string));
            foreach (DataRow r in source.Rows)
            {
                DataRow n = dt.NewRow();
                for (int i = 0; i < source.Columns.Count; i++)
                {
                    object v = r[i];
                    if (v is DateTime) n[i] = ((DateTime)v).ToString("yyyy-MM-dd");
                    else if (v is decimal || v is double || v is float) n[i] = Money(Convert.ToDecimal(v));
                    else n[i] = Convert.ToString(v);
                }
                dt.Rows.Add(n);
            }
            return dt;
        }

        static decimal ToDec(object o)
        {
            if (o == null || o == DBNull.Value) return 0m;
            decimal d;
            return decimal.TryParse(Convert.ToString(o), out d) ? d : 0m;
        }

        static string Money(decimal d)
        {
            return d.ToString("0.00", CultureInfo.InvariantCulture);
        }

        /// //////////////  Print Part  Start

        DataGridViewPrinter MyDataGridViewPrinter;

        private bool SetupThePrinting()
        {
            DataTable dt1 = DataAccess.GetDataTable("SELECT companyname, companyaddress, companyphone FROM storeconfig");
            string s = DateTime.Now.ToString("MMMM dd, yyyy    hh:mm:ss tt");

            string sd = s + "\n";
            if (dt1.Rows.Count > 0)
                sd = Convert.ToString(dt1.Rows[0][0]) + "\n" + Convert.ToString(dt1.Rows[0][1]) + "." + "\n" + Convert.ToString(dt1.Rows[0][2]) + "\n" + s + "\n";

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

            printDocument1.DocumentName = "Ledger Report";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            MyDataGridViewPrinter = new DataGridViewPrinter(dtGrdLedgerReport,
                printDocument1, true, true, sd + " General Ledger Report \n", new Font("Baskerville Old Face", 15,
                FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);

            return true;
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtGrdLedgerReport.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtGrdLedgerReport.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

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

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            ReportByDate(dtStartDate.Value.ToString("yyyy-MM-dd"), dtEndDate.Value.ToString("yyyy-MM-dd"));
        }
    }
}
