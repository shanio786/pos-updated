using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace supershop.Report
{
    /// <summary>
    /// End-of-day cash reconciliation (Z-Report).
    /// For the chosen date and the logged-in shop it works out how much cash
    /// should be in the drawer, lets the cashier enter what was counted, shows
    /// the difference and saves the close into tbl_dayclose.
    /// </summary>
    public partial class DayClose : Form
    {
        DataGridViewPrinter MyPrinter;

        public DayClose()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void DayClose_Load(object sender, EventArgs e)
        {
            dtDate.Format = DateTimePickerFormat.Custom;
            dtDate.CustomFormat = "yyyy-MM-dd";
            lblShop.Text = "Shop: " + UserInfo.Shopid;
            Calculate();
        }

        string Day { get { return dtDate.Value.ToString("yyyy-MM-dd"); } }

        private void Calculate()
        {
            try
            {
                string shop = UserInfo.Shopid;

                // opening cash = counted cash of the last close for this shop before today
                decimal opening = DataAccess.GetDecimal(
                    "SELECT TOP 1 counted_cash FROM tbl_dayclose WHERE Shopid = @s AND close_date < @d ORDER BY close_date DESC, id DESC",
                    DataAccess.P("@s", shop), DataAccess.P("@d", Day));
                if (txtOpening.Text.Trim().Length == 0)
                    txtOpening.Text = opening.ToString("0.00");
                decimal openingCash = ParseDecimal(txtOpening.Text);

                // cash actually taken at the counter = payable - still-due, for cash sales
                decimal cashSales = DataAccess.GetDecimal(
                    "SELECT SUM(ISNULL(payment_amount,0) - ISNULL(due_amount,0)) FROM sales_payment " +
                    "WHERE sales_time = @d AND ISNULL(Shopid,'') = @s AND ISNULL(SaleType,'CashSale') = 'CashSale'",
                    DataAccess.P("@d", Day), DataAccess.P("@s", shop));

                decimal otherSales = DataAccess.GetDecimal(
                    "SELECT SUM(ISNULL(payment_amount,0) - ISNULL(due_amount,0)) FROM sales_payment " +
                    "WHERE sales_time = @d AND ISNULL(Shopid,'') = @s AND ISNULL(SaleType,'CashSale') <> 'CashSale'",
                    DataAccess.P("@d", Day), DataAccess.P("@s", shop));

                decimal dueReceived = DataAccess.GetDecimal(
                    "SELECT SUM(ISNULL(receiveamt,0)) FROM tbl_duepayment WHERE receivedate = @d AND ISNULL(Shopid,'') = @s",
                    DataAccess.P("@d", Day), DataAccess.P("@s", shop));

                decimal returns = DataAccess.GetDecimal(
                    "SELECT SUM(ISNULL(Total,0) - ISNULL(disamt,0) + ISNULL(vatamt,0)) FROM return_item " +
                    "WHERE return_time = @d AND ISNULL(Shopid,'') = @s",
                    DataAccess.P("@d", Day), DataAccess.P("@s", shop));

                // expenses are company-wide (no Shopid on tbl_expense); count them once
                decimal expenses = DataAccess.GetDecimal(
                    "SELECT SUM(ISNULL(Amount,0)) FROM tbl_expense WHERE [Date] >= @d AND [Date] < DATEADD(day,1,@d)",
                    DataAccess.P("@d", dtDate.Value.Date));

                decimal expected = openingCash + cashSales + dueReceived - returns - expenses;

                grid.Rows.Clear();
                AddRow("Date", Day, "");
                AddRow("Shop", UserInfo.Shopid, "");
                AddRow("", "", "");
                AddRow("Opening cash", "", openingCash.ToString("0.00"));
                AddRow("Cash sales (net of due)", "", cashSales.ToString("0.00"));
                AddRow("Card / other sales", "", otherSales.ToString("0.00"));
                AddRow("Old due received", "", dueReceived.ToString("0.00"));
                AddRow("Less: returns refunded", "", "-" + returns.ToString("0.00"));
                AddRow("Less: expenses", "", "-" + expenses.ToString("0.00"));
                AddRow("", "", "");
                AddRow("Expected cash in drawer", "", expected.ToString("0.00"));

                lblExpected.Text = expected.ToString("0.00");
                UpdateDifference();
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not calculate the day close.");
            }
        }

        void AddRow(string label, string value, string amount)
        {
            grid.Rows.Add(label, value, amount);
        }

        static decimal ParseDecimal(string s)
        {
            decimal d;
            return decimal.TryParse(s, out d) ? d : 0m;
        }

        private void UpdateDifference()
        {
            decimal expected = ParseDecimal(lblExpected.Text);
            decimal counted = ParseDecimal(txtCounted.Text);
            decimal diff = counted - expected;
            lblDifference.Text = diff.ToString("0.00");
            lblDifference.ForeColor = diff == 0 ? Color.Green : (diff < 0 ? Color.Red : Color.DarkOrange);
        }

        private void txtCounted_TextChanged(object sender, EventArgs e) { UpdateDifference(); }
        private void txtOpening_TextChanged(object sender, EventArgs e) { }
        private void btnCalculate_Click(object sender, EventArgs e) { txtOpening.Text = ""; Calculate(); }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                decimal expected = ParseDecimal(lblExpected.Text);
                decimal counted = ParseDecimal(txtCounted.Text);
                DataAccess.ExecuteSQL(
                    "INSERT INTO tbl_dayclose (Shopid, close_date, opening_cash, cash_sales, card_sales, other_sales, " +
                    " returns_total, expenses_total, due_received, expected_cash, counted_cash, difference, closed_by, note) " +
                    "VALUES (@shop, @date, @opening, @cash, @card, 0, @ret, @exp, @due, @expected, @counted, @diff, @by, @note)",
                    DataAccess.P("@shop", UserInfo.Shopid),
                    DataAccess.P("@date", Day),
                    DataAccess.P("@opening", ParseDecimal(txtOpening.Text)),
                    DataAccess.P("@cash", ParseDecimal(GridAmount("Cash sales (net of due)"))),
                    DataAccess.P("@card", ParseDecimal(GridAmount("Card / other sales"))),
                    DataAccess.P("@ret", ParseDecimal(GridAmount("Less: returns refunded").Replace("-", ""))),
                    DataAccess.P("@exp", ParseDecimal(GridAmount("Less: expenses").Replace("-", ""))),
                    DataAccess.P("@due", ParseDecimal(GridAmount("Old due received"))),
                    DataAccess.P("@expected", expected),
                    DataAccess.P("@counted", counted),
                    DataAccess.P("@diff", counted - expected),
                    DataAccess.P("@by", UserInfo.UserName),
                    DataAccess.P("@note", txtNote.Text));
                MessageBox.Show("Day close saved.", "Day Close", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not save the day close.");
            }
        }

        string GridAmount(string label)
        {
            foreach (DataGridViewRow r in grid.Rows)
                if (r.Cells[0].Value != null && r.Cells[0].Value.ToString() == label)
                    return r.Cells[2].Value == null ? "0" : r.Cells[2].Value.ToString();
            return "0";
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (SetupPrint())
                {
                    PrintPreviewDialog dlg = new PrintPreviewDialog();
                    dlg.Document = printDocument1;
                    dlg.WindowState = FormWindowState.Maximized;
                    dlg.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not print the day close.");
            }
        }

        private bool SetupPrint()
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() != DialogResult.OK) return false;
            printDocument1.PrinterSettings = pd.PrinterSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
            string header = "Day Close / Z-Report\nShop: " + UserInfo.Shopid + "   Date: " + Day +
                            "\nExpected: " + lblExpected.Text + "   Counted: " + txtCounted.Text +
                            "   Difference: " + lblDifference.Text + "\n";
            MyPrinter = new DataGridViewPrinter(grid, printDocument1, true, true, header,
                new Font("Segoe UI", 11, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);
            return true;
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (MyPrinter.DrawDataGridView(e.Graphics)) e.HasMorePages = true;
        }

        private void dtDate_ValueChanged(object sender, EventArgs e) { txtOpening.Text = ""; Calculate(); }
    }
}
