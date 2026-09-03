using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace supershop.BarCode
{
    /// <summary>
    /// Reads the special barcode an electronic weighing scale prints for a
    /// loose item, where the PRICE (or WEIGHT) is embedded inside the barcode.
    ///
    /// Standard in-store EAN-13 layout (all configurable in app.config):
    ///   [prefix][item digits][value digits][check]
    ///   e.g. prefix "2", item 5, value 5:  2 12345 01990 C
    ///        -> product whose code starts with "212345", value 01990.
    ///
    /// app.config &lt;appSettings&gt;:
    ///   WeightBarcode.Enabled   true|false     (default false)
    ///   WeightBarcode.Prefix    2              (leading digit(s) a scale uses)
    ///   WeightBarcode.ItemDigits 5
    ///   WeightBarcode.ValueDigits 5
    ///   WeightBarcode.Mode      price|weight   (what the value means)
    ///   WeightBarcode.PriceDivisor  100        (value 01990 -> 19.90)
    ///   WeightBarcode.WeightDivisor 1000       (value 01500 -> 1.500 kg)
    ///
    /// Off by default so ordinary barcodes are never misread.
    /// </summary>
    public static class WeightBarcode
    {
        static string Cfg(string k, string d)
        { try { string v = ConfigurationManager.AppSettings[k]; return string.IsNullOrEmpty(v) ? d : v; } catch { return d; } }
        static int Int(string k, int d) { int n; return int.TryParse(Cfg(k, d.ToString()), out n) ? n : d; }

        public static bool Enabled { get { return Cfg("WeightBarcode.Enabled", "false").Trim().ToLowerInvariant() == "true"; } }
        static string Prefix { get { return Cfg("WeightBarcode.Prefix", "2"); } }
        static int ItemDigits { get { return Int("WeightBarcode.ItemDigits", 5); } }
        static int ValueDigits { get { return Int("WeightBarcode.ValueDigits", 5); } }
        static bool PriceMode { get { return Cfg("WeightBarcode.Mode", "price").Trim().ToLowerInvariant() != "weight"; } }
        static decimal PriceDivisor { get { decimal d; return decimal.TryParse(Cfg("WeightBarcode.PriceDivisor", "100"), out d) && d != 0 ? d : 100; } }
        static decimal WeightDivisor { get { decimal d; return decimal.TryParse(Cfg("WeightBarcode.WeightDivisor", "1000"), out d) && d != 0 ? d : 1000; } }

        static bool AllDigits(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            foreach (char c in s) if (c < '0' || c > '9') return false;
            return true;
        }

        /// <summary>
        /// If the scanned text is a scale barcode for a known product, adds a cart
        /// line (with the weighed quantity / embedded price) and returns true.
        /// Otherwise returns false so the normal lookup runs.
        /// </summary>
        public static bool TryAdd(string scanned, DataGridView cart, double taxRatePercent)
        {
            try
            {
                if (!Enabled || cart == null) return false;
                scanned = (scanned ?? "").Trim();
                if (scanned.Length != 13 || !AllDigits(scanned)) return false;
                if (!scanned.StartsWith(Prefix)) return false;

                // if this exact code is a real product, let the normal path handle it
                if (DataAccess.GetDecimal("SELECT COUNT(*) FROM purchase WHERE product_id = @c", DataAccess.P("@c", scanned)) > 0)
                    return false;

                int headLen = Prefix.Length + ItemDigits;
                string head = scanned.Substring(0, headLen);            // prefix + item number
                string valueStr = scanned.Substring(headLen, Math.Min(ValueDigits, scanned.Length - headLen - 1));
                decimal rawValue; if (!decimal.TryParse(valueStr, out rawValue)) return false;

                // find the product whose code begins with the same prefix+item number
                DataTable p = DataAccess.GetDataTable(
                    "SELECT TOP 1 product_name, retail_price, ISNULL(discount,0) AS discount, ISNULL(taxapply,0) AS taxapply, " +
                    "       ISNULL(status,1) AS status, product_id, ISNULL(product_quantity,0) AS product_quantity " +
                    "FROM purchase WHERE LEFT(product_id, @n) = @head",
                    DataAccess.P("@n", headLen), DataAccess.P("@head", head));
                if (p.Rows.Count == 0)
                {
                    MessageBox.Show("Weighed item not found for code " + head + ".", "Scale barcode",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return true;   // handled (nothing to fall through to)
                }

                string name = p.Rows[0]["product_name"].ToString();
                decimal retail = Dec(p.Rows[0]["retail_price"]);
                decimal discount = Dec(p.Rows[0]["discount"]);
                int taxapply = (int)Dec(p.Rows[0]["taxapply"]);
                int kitchen = (int)Dec(p.Rows[0]["status"]);
                string code = p.Rows[0]["product_id"].ToString();

                decimal qty, lineTotal;
                if (PriceMode)
                {
                    lineTotal = Math.Round(rawValue / PriceDivisor, 2);           // the price the scale computed
                    qty = retail > 0 ? Math.Round(lineTotal / retail, 3) : 1;     // implied weight, for the receipt
                }
                else
                {
                    qty = Math.Round(rawValue / WeightDivisor, 3);               // weight in kg
                    lineTotal = Math.Round(qty * retail, 2);
                }

                decimal disAmt = Math.Round((retail * qty) * discount / 100m, 2);
                decimal taxAmt = taxapply != 0
                    ? Math.Round(((retail * qty) - disAmt) * (decimal)taxRatePercent / 100m, 2) : 0m;

                // (Name, Price, Qty, Total, Code, DisAmt, TaxAmt, DisRate, TaxApply, KitchenDisplay)
                cart.Rows.Add(name, retail, qty, lineTotal, code, disAmt, taxAmt, discount, taxapply, kitchen);
                return true;
            }
            catch (Exception ex)
            {
                Logger.Error("WeightBarcode", ex);
                return false;
            }
        }

        static decimal Dec(object o) { decimal d; return o != null && o != DBNull.Value && decimal.TryParse(o.ToString(), out d) ? d : 0m; }
    }
}
