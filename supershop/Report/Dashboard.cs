using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace supershop.Report
{
    /// <summary>At-a-glance figures for the shop: today's sales, cash, low stock and more.</summary>
    public partial class Dashboard : Form
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Dashboard_Load(object sender, EventArgs e) { Refresh_(); }

        string Today { get { return DateTime.Now.ToString("yyyy-MM-dd"); } }
        string Month { get { return DateTime.Now.ToString("yyyy-MM"); } }
        string Shop { get { return UserInfo.Shopid ?? ""; } }

        decimal Q(string sql, params System.Data.SqlClient.SqlParameter[] p) { return DataAccess.GetDecimal(sql, p); }

        void Refresh_()
        {
            try
            {
                lblShop.Text = "Shop: " + Shop + "     " + DateTime.Now.ToString("dddd, dd MMM yyyy");

                decimal todaySales = Q("SELECT SUM(payment_amount) FROM sales_payment WHERE sales_time=@d AND (ISNULL(Shopid,'')=@s OR @s='')",
                    DataAccess.P("@d", Today), DataAccess.P("@s", Shop));
                decimal todayCash = Q("SELECT SUM(ISNULL(payment_amount,0)-ISNULL(due_amount,0)) FROM sales_payment WHERE sales_time=@d AND ISNULL(SaleType,'CashSale')='CashSale' AND (ISNULL(Shopid,'')=@s OR @s='')",
                    DataAccess.P("@d", Today), DataAccess.P("@s", Shop));
                decimal todayTxns = Q("SELECT COUNT(*) FROM sales_payment WHERE sales_time=@d AND (ISNULL(Shopid,'')=@s OR @s='')",
                    DataAccess.P("@d", Today), DataAccess.P("@s", Shop));
                decimal todayDue = Q("SELECT SUM(due_amount) FROM sales_payment WHERE sales_time=@d AND (ISNULL(Shopid,'')=@s OR @s='')",
                    DataAccess.P("@d", Today), DataAccess.P("@s", Shop));
                decimal monthSales = Q("SELECT SUM(payment_amount) FROM sales_payment WHERE sales_time LIKE @m + '%' AND (ISNULL(Shopid,'')=@s OR @s='')",
                    DataAccess.P("@m", Month), DataAccess.P("@s", Shop));
                decimal lowStock = Q("SELECT COUNT(*) FROM purchase WHERE ISNULL(product_quantity,0) <= @t AND ISNULL(status,1)<>0 AND (ISNULL(Shopid,'')=@s OR @s='')",
                    DataAccess.P("@t", supershop.Inventory.LowStock.Threshold), DataAccess.P("@s", Shop));

                Set(tileSales, todaySales.ToString("N2"));
                Set(tileCash, todayCash.ToString("N2"));
                Set(tileTxns, ((int)todayTxns).ToString());
                Set(tileDue, todayDue.ToString("N2"));
                Set(tileMonth, monthSales.ToString("N2"));
                Set(tileLow, ((int)lowStock).ToString());
                tileLow.Tag = (int)lowStock;
                lblLowNote.Visible = lowStock > 0;

                // top 5 items today
                DataTable top = DataAccess.GetDataTable(
                    "SELECT TOP 5 itemName AS [Item], SUM(Qty) AS [Qty], SUM(Total) AS [Sales] FROM sales_item " +
                    "WHERE sales_time=@d AND status<>2 GROUP BY itemName ORDER BY SUM(Total) DESC",
                    DataAccess.P("@d", Today));
                gridTop.DataSource = top;
            }
            catch (Exception ex) { Logger.Show(ex, "Could not load the dashboard."); }
        }

        static void Set(Panel tile, string value)
        {
            foreach (Control c in tile.Controls) if (c is Label && c.Name.EndsWith("Val")) c.Text = value;
        }

        private void btnRefresh_Click(object sender, EventArgs e) { Refresh_(); }

        private void lblLowNote_Click(object sender, EventArgs e)
        {
            try { supershop.Inventory.LowStock.AlertIfLow(); } catch (Exception ex) { Logger.Error(ex); }
        }
    }
}
