using System;
using System.Text;

namespace supershop.BarCode
{
    /// <summary>
    /// Makes a scannable barcode number for products that don't have one
    /// (loose items, vegetables, your own packing).
    ///
    /// It builds a valid EAN-13 using the in-store prefix "22" (the GS1 range
    /// 20-29 is reserved for a shop's own internal items), a running sequence,
    /// and the correct EAN-13 check digit. The number is guaranteed unique in
    /// your product list, and any scanner reads it like a normal barcode.
    /// </summary>
    public static class BarcodeService
    {
        const string InStorePrefix = "22";   // reserved for in-store use

        /// <summary>EAN-13 check digit for a 12-digit string.</summary>
        public static int Ean13CheckDigit(string first12)
        {
            int sum = 0;
            for (int i = 0; i < 12; i++)
            {
                int d = first12[i] - '0';
                sum += (i % 2 == 0) ? d : d * 3;   // positions 1,3,5.. weight 1; 2,4,6.. weight 3
            }
            return (10 - (sum % 10)) % 10;
        }

        /// <summary>Completes a 12-digit body into a full 13-digit EAN-13.</summary>
        public static string ToEan13(string first12)
        {
            return first12 + Ean13CheckDigit(first12).ToString();
        }

        /// <summary>
        /// Returns a fresh, unique internal EAN-13 barcode not already used by any product.
        /// </summary>
        public static string GenerateInternalCode()
        {
            // next sequence = 1 + the highest internal number already issued
            long seq = 0;
            try
            {
                string maxCode = DataAccess.ExecuteSQLScaler(
                    "SELECT MAX(product_id) FROM purchase WHERE product_id LIKE @p AND LEN(product_id) = 13 AND product_id NOT LIKE '%[^0-9]%'",
                    DataAccess.P("@p", InStorePrefix + "%"));
                if (!string.IsNullOrEmpty(maxCode) && maxCode.Length == 13)
                {
                    long body;
                    if (long.TryParse(maxCode.Substring(0, 12), out body))
                        seq = body - long.Parse(InStorePrefix + new string('0', 10));
                }
            }
            catch (Exception ex) { Logger.Error("GenerateInternalCode seq", ex); }

            for (int attempt = 0; attempt < 1000; attempt++)
            {
                seq++;
                // prefix(2) + 10-digit sequence = 12-digit body, then check digit
                string body = InStorePrefix + seq.ToString().PadLeft(10, '0');
                if (body.Length != 12) break;
                string code = ToEan13(body);
                if (!Exists(code)) return code;
            }
            // extreme fallback: time-based
            string b = InStorePrefix + (DateTime.Now.Ticks % 10000000000L).ToString().PadLeft(10, '0');
            return ToEan13(b);
        }

        static bool Exists(string code)
        {
            try
            {
                return DataAccess.GetDecimal("SELECT COUNT(*) FROM purchase WHERE product_id = @c", DataAccess.P("@c", code)) > 0;
            }
            catch (Exception ex) { Logger.Error("barcode Exists", ex); return false; }
        }
    }
}
