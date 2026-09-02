using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace supershop.Report
{
    public partial class ShortCutReport : Form
    {
        public ShortCutReport()
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

            printDocument1.DocumentName = "Sales Report";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            MyDataGridViewPrinter = new DataGridViewPrinter(datagrdReportDetails,
                printDocument1, true, true, Header + " Sales Report \n", new Font("Baskerville Old Face", 13,
                FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);

            return true;
        }

        private void ShortCutReport_Load(object sender, EventArgs e)
        {
            dtStartDate.Format = DateTimePickerFormat.Custom;
            dtStartDate.CustomFormat = "yyyy-MM-dd";

            dtEndDate.Format = DateTimePickerFormat.Custom;
            dtEndDate.CustomFormat = "yyyy-MM-dd";

            datagrdReportDetails.EnableHeadersVisualStyles = false;
            datagrdReportDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            datagrdReportDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            datagrdReportDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            LoadDefaultReport();
        }

        /// <summary>Daily report when no end date was given (lblENDdate = "0"), otherwise the date range report.</summary>
        void LoadDefaultReport()
        {
            if (lblENDdate.Text == "0")
                dailyReport();
            else
                Last30daysReport(lblStartDate.Text, lblENDdate.Text);
        }

        public string ReportName
        {
            set { lblReportName.Text = value; }
            get { return lblReportName.Text; }
        }

        public string DTtoday
        {
            set { lblStartDate.Text = value; }
            get { return lblStartDate.Text; }
        }

        public string last30salesStartDate
        {
            set { lblStartDate.Text = value; }
            get { return lblStartDate.Text; }
        }

        public string last30salesENDDate
        {
            set { lblENDdate.Text = value; }
            get { return lblENDdate.Text; }
        }

        const string PaymentColumns =
            "select sales_id as 'Recipt No', sales_time as Date, payment_amount as Total, emp_id as 'Sold by', " +
            " dis as Discount, vat as TAX, payment_type as 'Payment Type', due_amount as Due, Comment as Comments " +
            " from sales_payment ";

        const string PaymentSums = "select SUM(payment_amount), SUM(vat), SUM(due_amount), SUM(dis) from sales_payment ";

        static decimal ToDec(object o)
        {
            if (o == null || o == DBNull.Value) return 0m;
            decimal d;
            return decimal.TryParse(Convert.ToString(o), out d) ? d : 0m;
        }

        /// <summary>Loads the invoice list and its totals, then appends the summary lines.</summary>
        void ShowPaymentReport(string where, SqlParameter[] listParams, SqlParameter[] sumParams, string from, string to)
        {
            DataTable dt1 = DataAccess.GetDataTable(PaymentColumns + where + " order by sales_time", listParams);
            datagrdReportDetails.DataSource = dt1;

            DataTable dt3 = DataAccess.GetDataTable(PaymentSums + where, sumParams);
            decimal total = 0, vat = 0, due = 0, dis = 0;
            if (dt3.Rows.Count > 0)
            {
                total = ToDec(dt3.Rows[0][0]);
                vat = ToDec(dt3.Rows[0][1]);
                due = ToDec(dt3.Rows[0][2]);
                dis = ToDec(dt3.Rows[0][3]);
            }

            DataRow dr = dt1.NewRow();
            dr[1] = " ";
            dt1.Rows.Add(dr);

            // Sub total = total payable - TAX
            DataRow dr2 = dt1.NewRow();
            dr2[1] = "Sub Total";
            dr2[2] = total - vat;
            dt1.Rows.Add(dr2);

            DataRow discount = dt1.NewRow();
            discount[1] = "Total Discount";
            discount[4] = dis;
            dt1.Rows.Add(discount);

            DataRow dr4 = dt1.NewRow();
            dr4[1] = "Total TAX ";
            dr4[5] = vat;
            dt1.Rows.Add(dr4);

            DataRow dr6 = dt1.NewRow();
            dr6[1] = " ";
            dt1.Rows.Add(dr6);

            DataRow dr5 = dt1.NewRow();
            dr5[1] = "Total Sales+TAX ";
            dr5[2] = total;
            dt1.Rows.Add(dr5);

            DataRow dr8 = dt1.NewRow();
            dr8[1] = "Total Due ";
            dr8[5] = due;
            dt1.Rows.Add(dr8);

            DataRow dr17 = dt1.NewRow();
            dr17[1] = " ";
            dt1.Rows.Add(dr17);

            DataRow dr7 = dt1.NewRow();
            dr7[1] = " ";
            dt1.Rows.Add(dr7);

            DataRow rep = dt1.NewRow();
            rep[1] = "Payment Report ";
            dt1.Rows.Add(rep);

            DataRow repdt = dt1.NewRow();
            repdt[1] = "From : " + from;
            dt1.Rows.Add(repdt);

            DataRow repdt2 = dt1.NewRow();
            repdt2[1] = "To : " + to;
            dt1.Rows.Add(repdt2);
        }

        /// <summary>All invoices of the day in lblStartDate.</summary>
        public void dailyReport()
        {
            string day = lblStartDate.Text;
            if (day == "")
                return;

            try
            {
                ShowPaymentReport(" where sales_time like @day ",
                    new SqlParameter[] { DataAccess.P("@day", "%" + day + "%") },
                    new SqlParameter[] { DataAccess.P("@day", "%" + day + "%") },
                    day, day);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        /// <summary>All invoices between the two dates (inclusive).</summary>
        public void Last30daysReport(string startDate, string endDate)
        {
            if (lblStartDate.Text == "")
                return;

            try
            {
                ShowPaymentReport(" where sales_time BETWEEN @from AND @to ",
                    new SqlParameter[] { DataAccess.P("@from", startDate), DataAccess.P("@to", endDate) },
                    new SqlParameter[] { DataAccess.P("@from", startDate), DataAccess.P("@to", endDate) },
                    startDate, endDate);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        private void btnPrintReport_Click(object sender, EventArgs e)
        {
            try
            {
                this.datagrdReportDetails.RowsDefaultCellStyle.BackColor = Color.White;
                this.datagrdReportDetails.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }

        // Open the invoice / sale details of the clicked row
        private void datagrdReportDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (lblENDdate.Text != "0" || e.RowIndex < 0)
                    return;

                DataGridViewRow row = datagrdReportDetails.Rows[e.RowIndex];
                string id = Convert.ToString(row.Cells[0].Value);
                if (id == "")
                    return;   // summary line, not an invoice

                double Payamt = Convert.ToDouble(row.Cells[2].Value);
                double vat = Convert.ToDouble(row.Cells[5].Value);
                double subtotal = Payamt - vat;
                double dis = Convert.ToDouble(row.Cells[4].Value);

                if (Convert.ToString(row.Cells[6].Value) == "Invoice")
                {
                    View_Sales_invoice go = new View_Sales_invoice(id);
                    go.ShowDialog();
                }
                else
                {
                    SalesDetails go = new SalesDetails(id, dis, subtotal, vat, Payamt);
                    go.ShowDialog();
                }
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            Last30daysReport(dtStartDate.Value.ToString("yyyy-MM-dd"), dtEndDate.Value.ToString("yyyy-MM-dd"));
        }

        // Search one invoice by number
        private void txtInvoice_TextChanged(object sender, EventArgs e)
        {
            string text = txtInvoice.Text.Trim();
            if (text == "")
            {
                LoadDefaultReport();
                return;
            }

            long id;
            if (!long.TryParse(text, out id))
                return;   // sales_id is bigint

            try
            {
                ShowPaymentReport(" where sales_id = @id ",
                    new SqlParameter[] { DataAccess.P("@id", id) },
                    new SqlParameter[] { DataAccess.P("@id", id) },
                    lblStartDate.Text, lblStartDate.Text);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        private void helplnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            parameter.helpid = "INV";
            HelpPage go = new HelpPage();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void datagrdReportDetails_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
    }
}
