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
using System.IO;
using System.Drawing.Printing;

namespace supershop.Report
{
    /// <summary>
    /// Profit / loss summary for the period ReportValue.StartDate .. ReportValue.EndDate
    /// (both inclusive, 'yyyy-MM-dd').  Every figure is read with DataAccess.GetDecimal
    /// so an empty period simply shows zeros instead of crashing on a NULL SUM.
    ///
    /// Formulas (see ProfitLossReport_Load):
    ///   Gross item sales     = SUM(sales_item.Total)            status &lt;&gt; 2   (before discount)
    ///   Total discount       = SUM(sales_payment.dis)                          (item + counter discount)
    ///   Sales after discount = gross item sales - total discount
    ///   Returns              = SUM(return_item.Total) - SUM(disamt) + SUM(vatamt)
    ///   Sales after returns  = sales after discount - returns
    ///   TAX collected        = SUM(sales_payment.vat)
    ///   Due (unpaid)         = SUM(sales_payment.due_amount)
    ///   Received             = SUM(sales_payment.payment_amount) - due - returns
    ///   Cost of goods sold   = SUM((RetailsPrice * (1 - discount/100) - profit) * Qty)   status &lt;&gt; 2
    ///   Gross profit         = SUM(profit * Qty)                                        status &lt;&gt; 2
    ///   Counter discount     = total discount - SUM(RetailsPrice * Qty * discount/100)  (item discounts are already inside profit)
    ///   Expenses             = SUM(tbl_expense.Amount)
    ///   Net profit           = gross profit - counter discount - expenses
    ///   Cash in hand         = received - expenses
    /// </summary>
    public partial class ProfitLossReport : Form
    {
        public ProfitLossReport()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        DataGridViewPrinter MyDataGridViewPrinter;

        private bool SetupThePrinting()
        {
            DataTable dt1 = DataAccess.GetDataTable(
                "SELECT CompanyName, Branchname, Location, Phone, Email, Web FROM tbl_terminalLocation WHERE Shopid = @shop",
                DataAccess.P("@shop", UserInfo.Shopid));

            string printdate = DateTime.Now.ToString("MMMM dd, yyyy    hh:mm:ss tt");
            string Header = printdate + "\n";
            if (dt1.Rows.Count > 0)
            {
                DataRow r = dt1.Rows[0];
                Header = Convert.ToString(r[0]) + "\n" + Convert.ToString(r[2]) + "." + "\n" + Convert.ToString(r[4]) + "\n" +
                         Convert.ToString(r[1]) + " ph: " + Convert.ToString(r[3]) + "\n" + printdate + "\n";
            }

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

            printDocument1.DocumentName = "Profit_Loss_Summary_Report";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            MyDataGridViewPrinter = new DataGridViewPrinter(dtgrdViewProfitLoss, printDocument1, true, true, Header + "\n",
                new Font("Baskerville Old Face", 13, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);

            return true;
        }

        /// <summary>Fresh @from/@to parameters for each query (a SqlParameter can only belong to one command).</summary>
        static SqlParameter[] DateParams()
        {
            return new SqlParameter[] {
                DataAccess.P("@from", ReportValue.StartDate),
                DataAccess.P("@to", ReportValue.EndDate)
            };
        }

        /// <summary>'yyyy-MM-dd' report date to DateTime (needed for the smalldatetime expense column).</summary>
        static DateTime ParseDate(string s)
        {
            DateTime d;
            if (DateTime.TryParseExact(s, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out d))
                return d;
            return DateTime.Parse(s);
        }

        static string Money(decimal d)
        {
            return d.ToString("0.00", CultureInfo.InvariantCulture);
        }

        void AddRow(string label, string value, string extra)
        {
            dtgrdViewProfitLoss.Rows.Add(new string[] { label, value, extra });
        }

        void AddRow(string label, decimal value)
        {
            AddRow(label, Money(value), " ");
        }

        void AddBlank()
        {
            AddRow(" ", " ", " ");
        }

        private void ProfitLossReport_Load(object sender, EventArgs e)
        {
            try
            {
                this.dtgrdViewProfitLoss.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtgrdViewProfitLoss.AlternatingRowsDefaultCellStyle.BackColor = Color.White;
                dtgrdViewProfitLoss.ColumnCount = 3;

                // ---- sales_item (returned lines, status = 2, are excluded) ----
                const string itemWhere = " FROM sales_item WHERE sales_time BETWEEN @from AND @to AND status <> 2 ";

                decimal grossSales = DataAccess.GetDecimal("SELECT SUM(Total)" + itemWhere, DateParams());
                decimal grossProfit = DataAccess.GetDecimal("SELECT SUM(profit * Qty)" + itemWhere, DateParams());
                decimal itemDiscount = DataAccess.GetDecimal("SELECT SUM(RetailsPrice * Qty * ISNULL(discount, 0) / 100.0)" + itemWhere, DateParams());
                decimal costOfGoods = DataAccess.GetDecimal("SELECT SUM((RetailsPrice * (1 - ISNULL(discount, 0) / 100.0) - profit) * Qty)" + itemWhere, DateParams());

                // ---- sales_payment (one row per invoice; payment_amount is already net of discount and includes tax) ----
                const string payWhere = " FROM sales_payment WHERE sales_time BETWEEN @from AND @to ";

                decimal paymentTotal = DataAccess.GetDecimal("SELECT SUM(payment_amount)" + payWhere, DateParams());
                decimal totalDiscount = DataAccess.GetDecimal("SELECT SUM(dis)" + payWhere, DateParams());
                decimal tax = DataAccess.GetDecimal("SELECT SUM(vat)" + payWhere, DateParams());
                decimal due = DataAccess.GetDecimal("SELECT SUM(due_amount)" + payWhere, DateParams());

                // ---- return_item: value refunded to customers ----
                decimal returns = DataAccess.GetDecimal(
                    "SELECT SUM(ISNULL(Total, 0) - ISNULL(disamt, 0) + ISNULL(vatamt, 0)) FROM return_item WHERE return_time BETWEEN @from AND @to",
                    DateParams());

                // ---- tbl_expense: [Date] is smalldatetime, so the end day is included with '< end + 1 day' ----
                decimal expenses = DataAccess.GetDecimal(
                    "SELECT SUM(Amount) FROM tbl_expense WHERE [Date] >= @fromDt AND [Date] < DATEADD(day, 1, @toDt)",
                    DataAccess.P("@fromDt", ParseDate(ReportValue.StartDate)),
                    DataAccess.P("@toDt", ParseDate(ReportValue.EndDate)));

                // ---- derived figures ----
                decimal salesAfterDiscount = grossSales - totalDiscount;
                decimal salesAfterReturns = salesAfterDiscount - returns;
                decimal received = paymentTotal - due - returns;
                decimal counterDiscount = totalDiscount - itemDiscount;
                decimal netProfit = grossProfit - counterDiscount - expenses;
                decimal cashInHand = received - expenses;

                // ---- report rows ----
                AddRow("  ", "Profit Loss Report", " ");
                AddRow("Date Between ", ReportValue.StartDate, ReportValue.EndDate);
                AddRow("_______________________", "__________________", "___________________");
                AddBlank();

                AddRow("Gross Item Sales (before discount) ", grossSales);
                AddRow("Total Discount ", totalDiscount);
                AddRow("Total Sales after discount ", salesAfterDiscount);
                AddRow("Total Return ", returns);
                AddRow("Total Sale After Return ", salesAfterReturns);
                AddRow("Total TAX ", tax);
                AddRow("Total Due Amount ", due);
                AddBlank();

                AddRow("Received From Customers ", received);
                AddBlank();

                AddRow("Cost of Goods Sold ", costOfGoods);
                AddRow("Gross Profit ", grossProfit);
                AddRow("Counter Discount ", counterDiscount);
                AddRow("Total Expenses ", expenses);
                AddBlank();

                AddRow("Net Profit ", "After discount and expenses ", Money(netProfit));
                AddBlank();

                AddRow("Cash In Hand ", cashInHand);
                AddBlank();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the profit / loss report."); }
        }

        // The grid is read-only; the figures are computed in ProfitLossReport_Load, so nothing to recalculate here.
        private void dtgrdViewProfitLoss_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        //save as
        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "ProfitLossReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                StringBuilder csv = new StringBuilder();

                foreach (DataGridViewColumn column in dtgrdViewProfitLoss.Columns)
                    csv.Append(column.HeaderText).Append(',');
                csv.Append("\r\n");

                foreach (DataGridViewRow row in dtgrdViewProfitLoss.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                        csv.Append(Convert.ToString(cell.Value).Replace(",", ";")).Append(',');
                    csv.Append("\r\n");
                }

                File.WriteAllText(saveFileDialog1.FileName, csv.ToString());
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not export the report."); }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtgrdViewProfitLoss.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtgrdViewProfitLoss.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

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
    }
}
