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
using System.IO;

namespace supershop
{
    public partial class salesreport : Form
    {
        public salesreport()
        {
            InitializeComponent();
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

            printDocument1.DocumentName = "SalesReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);

            MyDataGridViewPrinter = new DataGridViewPrinter(dtgrdViewSalesReport,
                printDocument1, true, true, Header + " Sales Report \n", new Font("Baskerville Old Face", 13, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);

            return true;
        }

        static decimal ToDec(object o)
        {
            if (o == null || o == DBNull.Value) return 0m;
            decimal d;
            return decimal.TryParse(Convert.ToString(o), out d) ? d : 0m;
        }

        // Daily payment Report with retun item
        private void dtDailyPaymentReport_ValueChanged(object sender, EventArgs e)
        {
            if (dtDailyPaymentReport.Text != "")
                loadInitialData();
        }

        private void loadInitialData()
        {
            try
            {
                dtgrdViewSalesReport.Refresh();
                string day = dtDailyPaymentReport.Text;

                string sql = "select  sales_id as 'Rpt No' , sales_time as 'Date' , payment_amount as 'Total' , emp_id as 'Sold by',  dis as 'Dis' , " +
                             " vat as 'TAX' ,  payment_type as 'Pay type' ,  due_amount as 'Due', c_id as 'Cust ID' , Comment as 'Comments' " +
                             " from sales_payment where sales_time like @day order by sales_time";
                DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@day", "%" + day + "%"));
                dtgrdViewSalesReport.DataSource = dt1;
                dtgrdViewSalesReport.DefaultCellStyle.Font = new Font("Times New Roman", 8.5F);

                string sql3 = "select SUM(payment_amount), SUM(vat), SUM(due_amount), SUM(dis) from sales_payment where sales_time like @day";
                DataTable dt3 = DataAccess.GetDataTable(sql3, DataAccess.P("@day", "%" + day + "%"));
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
                discount[2] = dis;
                dt1.Rows.Add(discount);

                DataRow dr4 = dt1.NewRow();
                dr4[1] = "Total TAX ";
                dr4[2] = vat;
                dt1.Rows.Add(dr4);

                DataRow dr6 = dt1.NewRow();
                dr6[1] = " ";
                dt1.Rows.Add(dr6);

                //Payable amount
                DataRow dr5 = dt1.NewRow();
                dr5[1] = "Total Sales + TAX ";
                dr5[2] = total;
                dt1.Rows.Add(dr5);

                DataRow dr8 = dt1.NewRow();
                dr8[1] = "Total Due ";
                dr8[2] = due;
                dt1.Rows.Add(dr8);

                DataRow dr17 = dt1.NewRow();
                dr17[1] = " ";
                dt1.Rows.Add(dr17);

                DataRow dr7 = dt1.NewRow();
                dr7[1] = " ";
                dt1.Rows.Add(dr7);

                DataRow rep = dt1.NewRow();
                rep[1] = "Daily Report For ";
                rep[3] = day;
                dt1.Rows.Add(rep);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }

        public void Dateformat()
        {
            dtDailyPaymentReport.Format = DateTimePickerFormat.Custom;
            dtDailyPaymentReport.CustomFormat = "yyyy-MM-dd";

            dtStartDate.Format = DateTimePickerFormat.Custom;
            dtStartDate.CustomFormat = "yyyy-MM-dd";

            dtEndDate.Format = DateTimePickerFormat.Custom;
            dtEndDate.CustomFormat = "yyyy-MM-dd";

            dtSalesItemEND.Format = DateTimePickerFormat.Custom;
            dtSalesItemEND.CustomFormat = "yyyy-MM-dd";

            dtSalesItemStart.Format = DateTimePickerFormat.Custom;
            dtSalesItemStart.CustomFormat = "yyyy-MM-dd";

            dtReturnEndDate.Format = DateTimePickerFormat.Custom;
            dtReturnEndDate.CustomFormat = "yyyy-MM-dd";

            dtReturnStartDate.Format = DateTimePickerFormat.Custom;
            dtReturnStartDate.CustomFormat = "yyyy-MM-dd";
        }

        private void salesreport_Load(object sender, EventArgs e)
        {
            Dateformat();
            dtgrdViewSalesReport.EnableHeadersVisualStyles = false;
            dtgrdViewSalesReport.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dtgrdViewSalesReport.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dtgrdViewSalesReport.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;

            dtDailyPaymentReport_ValueChanged(sender, e);
        }

        // Search by Sales ID / Invoice No, item code or item name
        private void txtItemSearchBox_TextChanged(object sender, EventArgs e)
        {
            string text = txtItemSearchBox.Text.Trim();
            if (text == "")
            {
                loadInitialData();
                return;
            }

            try
            {
                // sales_id is bigint: only compare it when the search text is a number
                long id;
                bool isNumber = long.TryParse(text, out id);

                string sql = "select sales_id as 'Receipt No', sales_time as Date, item_id as 'Item ID', itemName as 'Item Name', " +
                             "RetailsPrice as 'Retails Price', Qty as 'QTY', Total as '-Total-', profit * Qty as 'Profit' " +
                             "from sales_item " +
                             "where (itemName like @name or itemcode = @code" + (isNumber ? " or sales_id = @id" : "") + ") " +
                             "and (status = 1 or status = 3) " +
                             "order by sales_time";

                DataTable dt1 = DataAccess.GetDataTable(sql,
                    DataAccess.P("@name", text + "%"),
                    DataAccess.P("@code", text),
                    DataAccess.P("@id", isNumber ? (object)id : (object)0L));
                dtgrdViewSalesReport.DataSource = dt1;
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtgrdViewSalesReport.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtgrdViewSalesReport.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

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

        // Sales items report, date to date (returned lines excluded)
        private void dtSalesItemStart_TextChanged(object sender, EventArgs e)
        {
            if (dtSalesItemEND.Text == "")
                return;

            try
            {
                string from = dtSalesItemStart.Text;
                string to = dtSalesItemEND.Text;

                dtgrdViewSalesReport.Columns.Clear();
                string sql = " select sales_time as 'Date', itemName as 'Name', RetailsPrice as 'Price', Qty, Total, " +
                             " ((profit * Qty) * 1.00) as 'Profit', discount as 'Dis Rate', sales_id as 'Rpt.No' " +
                             " from sales_item where sales_time BETWEEN @from AND @to and status <> 2 order by sales_time";
                DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@from", from), DataAccess.P("@to", to));
                dtgrdViewSalesReport.DataSource = dt1;
                dtgrdViewSalesReport.DefaultCellStyle.Font = new Font("Trebuchet MS", 12.0F);

                // aggregate query: no ORDER BY allowed without GROUP BY on SQL Server
                decimal profit = DataAccess.GetDecimal(
                    " select SUM(profit * Qty) from sales_item where sales_time BETWEEN @from AND @to and status <> 2",
                    DataAccess.P("@from", from), DataAccess.P("@to", to));

                DataRow dr = dt1.NewRow();
                dr[0] = " ";
                dt1.Rows.Add(dr);

                DataRow dr6 = dt1.NewRow();
                dr6[0] = " ";
                dt1.Rows.Add(dr6);

                //Total Profit
                DataRow dr5 = dt1.NewRow();
                dr5[0] = "Total Profit :";
                dr5[5] = profit;
                dt1.Rows.Add(dr5);

                DataRow dr7 = dt1.NewRow();
                dr7[0] = "______ ";
                dt1.Rows.Add(dr7);

                DataRow rep = dt1.NewRow();
                rep[0] = "Sales Report ";
                dt1.Rows.Add(rep);

                DataRow repdt = dt1.NewRow();
                repdt[0] = "From : " + from;
                repdt[1] = "To : " + to;
                dt1.Rows.Add(repdt);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        // Sales payment report, date to date, grouped by day
        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            if (dtEndDate.Text == "")
                return;

            try
            {
                string from = dtStartDate.Text;
                string to = dtEndDate.Text;

                dtgrdViewSalesReport.Refresh();
                string sql = "select sales_time as 'Date', SUM(payment_amount) as 'Total', SUM(dis) as 'Discount', SUM(vat) as 'TAX', " +
                             "SUM(due_amount) as 'Due' from sales_payment where sales_time BETWEEN @from AND @to " +
                             " GROUP BY sales_time order by sales_time";
                DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@from", from), DataAccess.P("@to", to));
                dtgrdViewSalesReport.DataSource = dt1;
                dtgrdViewSalesReport.DefaultCellStyle.Font = new Font("Times New Roman", 13.0F);

                string sql3 = "select SUM(payment_amount), SUM(vat), SUM(due_amount), SUM(dis) from sales_payment " +
                              " where sales_time BETWEEN @from AND @to";
                DataTable dt3 = DataAccess.GetDataTable(sql3, DataAccess.P("@from", from), DataAccess.P("@to", to));
                decimal total = 0, vat = 0, due = 0, dis = 0;
                if (dt3.Rows.Count > 0)
                {
                    total = ToDec(dt3.Rows[0][0]);
                    vat = ToDec(dt3.Rows[0][1]);
                    due = ToDec(dt3.Rows[0][2]);
                    dis = ToDec(dt3.Rows[0][3]);
                }

                DataRow dr = dt1.NewRow();
                dr[0] = " ";
                dt1.Rows.Add(dr);

                DataRow discount = dt1.NewRow();
                discount[0] = "Total Discount";
                discount[2] = dis;
                dt1.Rows.Add(discount);

                DataRow dr4 = dt1.NewRow();
                dr4[0] = "Total TAX ";
                dr4[3] = vat;
                dt1.Rows.Add(dr4);

                DataRow dr6 = dt1.NewRow();
                dr6[0] = " ";
                dt1.Rows.Add(dr6);

                DataRow dr5 = dt1.NewRow();
                dr5[0] = "Total Sales+TAX ";
                dr5[1] = total;
                dt1.Rows.Add(dr5);

                DataRow dr8 = dt1.NewRow();
                dr8[0] = "Total Due ";
                dr8[4] = due;
                dt1.Rows.Add(dr8);

                DataRow dr17 = dt1.NewRow();
                dr17[0] = " ";
                dt1.Rows.Add(dr17);

                DataRow dr7 = dt1.NewRow();
                dr7[0] = "_________________________________ ";
                dt1.Rows.Add(dr7);

                DataRow rep = dt1.NewRow();
                rep[0] = "Payment Report ";
                dt1.Rows.Add(rep);

                DataRow repdt = dt1.NewRow();
                repdt[0] = "From : " + from;
                dt1.Rows.Add(repdt);

                DataRow repdt2 = dt1.NewRow();
                repdt2[0] = "To : " + to;
                dt1.Rows.Add(repdt2);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        private void dataGridView1_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                e.Column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        // Return report, date to date
        private void dateTimePicker6Return_ValueChanged(object sender, EventArgs e)
        {
            if (dtReturnEndDate.Text == "")
                return;

            try
            {
                string from = dtReturnStartDate.Text;
                string to = dtReturnEndDate.Text;

                dtgrdViewSalesReport.Refresh();
                string sql = "select return_time as 'Date', itemName as 'itemName', RetailsPrice as 'Price', Qty, Total, custno as CustID, " +
                             " SoldInvoiceNo as 'Receipt No', emp as 'Return by' from return_item " +
                             " where return_time BETWEEN @from AND @to order by return_time";
                DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@from", from), DataAccess.P("@to", to));
                dtgrdViewSalesReport.DataSource = dt1;
                dtgrdViewSalesReport.DefaultCellStyle.Font = new Font("Times New Roman", 13.0F);

                string sql3 = "select SUM(Total), SUM(disamt), SUM(vatamt) from return_item where return_time BETWEEN @from AND @to";
                DataTable dt3 = DataAccess.GetDataTable(sql3, DataAccess.P("@from", from), DataAccess.P("@to", to));
                decimal total = 0, dis = 0, vat = 0;
                if (dt3.Rows.Count > 0)
                {
                    total = ToDec(dt3.Rows[0][0]);
                    dis = ToDec(dt3.Rows[0][1]);
                    vat = ToDec(dt3.Rows[0][2]);
                }

                DataRow dr = dt1.NewRow();
                dr[0] = " ";
                dt1.Rows.Add(dr);

                DataRow dr4 = dt1.NewRow();
                dr4[0] = "Total  :";
                dr4[4] = total;
                dt1.Rows.Add(dr4);

                DataRow dr5 = dt1.NewRow();
                dr5[0] = "Total Discount :";
                dr5[5] = dis;
                dt1.Rows.Add(dr5);

                DataRow drvat = dt1.NewRow();
                drvat[0] = "Total TAX :";
                drvat[5] = vat;
                dt1.Rows.Add(drvat);

                DataRow drtotalreturned = dt1.NewRow();
                drtotalreturned[0] = "Total Returned :";
                drtotalreturned[4] = total - dis + vat;
                dt1.Rows.Add(drtotalreturned);

                DataRow dr7 = dt1.NewRow();
                dr7[0] = " ";
                dt1.Rows.Add(dr7);

                DataRow rep = dt1.NewRow();
                rep[0] = "Return Report ";
                dt1.Rows.Add(rep);

                DataRow repdt = dt1.NewRow();
                repdt[0] = "From : " + from;
                repdt[1] = "To : " + to;
                dt1.Rows.Add(repdt);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DueList go = new DueList();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void btnGLprint_Click(object sender, EventArgs e)
        {
            Report.LedgerReport go = new Report.LedgerReport();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        /// <summary>Grid contents as CSV text (null cells become empty).</summary>
        string BuildCsv()
        {
            StringBuilder csv = new StringBuilder();

            foreach (DataGridViewColumn column in dtgrdViewSalesReport.Columns)
                csv.Append(column.HeaderText).Append(',');
            csv.Append("\r\n");

            foreach (DataGridViewRow row in dtgrdViewSalesReport.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                    csv.Append(Convert.ToString(cell.Value).Replace(",", ";")).Append(',');
                csv.Append("\r\n");
            }
            return csv.ToString();
        }

        // Export straight to the desktop
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                string fileName = "SalesReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
                string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string destFile = Path.Combine(targetPath, fileName);

                if (!Directory.Exists(targetPath))
                    Directory.CreateDirectory(targetPath);

                File.WriteAllText(destFile, BuildCsv());
                MessageBox.Show(" Successfully Exported !!! ", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not export the report."); }
        }

        private void btnSaveAs_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "SalesReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {
                File.WriteAllText(saveFileDialog1.FileName, BuildCsv());
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not export the report."); }
        }
    }
}
