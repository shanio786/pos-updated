using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace supershop.Inventory
{
    /// <summary>
    /// Low-stock reminder. Shows products at or below the reorder level so the
    /// shop can restock in time. Threshold comes from app.config
    /// (LowStockThreshold, default 10).
    /// </summary>
    public static class LowStock
    {
        public static int Threshold
        {
            get
            {
                try { int t; return int.TryParse(ConfigurationManager.AppSettings["LowStockThreshold"], out t) && t >= 0 ? t : 10; }
                catch { return 10; }
            }
        }

        public static DataTable GetLowItems()
        {
            return DataAccess.GetDataTable(
                " SELECT product_id AS [Code], product_name AS [Item], product_quantity AS [In stock], " +
                "        retail_price AS [Price], category AS [Category], supplier AS [Supplier] " +
                " FROM purchase WHERE ISNULL(product_quantity,0) <= @t AND ISNULL(status,1) <> 0 " +
                "   AND (ISNULL(Shopid,'') = @s OR @s = '') " +
                " ORDER BY product_quantity ASC",
                DataAccess.P("@t", Threshold), DataAccess.P("@s", UserInfo.Shopid ?? ""));
        }

        /// <summary>If any item is low, opens the reorder list. Never throws.</summary>
        public static void AlertIfLow()
        {
            try
            {
                DataTable dt = GetLowItems();
                if (dt.Rows.Count == 0) return;
                Report.FastReport.ShowReport(
                    "Low Stock – " + dt.Rows.Count + " item(s) to reorder (at or below " + Threshold + ")",
                    dt);
            }
            catch (Exception ex) { Logger.Error("LowStock", ex); }
        }
    }
}
