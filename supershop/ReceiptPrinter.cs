using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace supershop
{
    /// <summary>
    /// Fast thermal-style receipt printing drawn directly with GDI+
    /// (no ReportViewer / Crystal engine to load), so printing is near-instant.
    ///
    /// Call ReceiptPrinter.Show(salesId).  Behaviour is controlled by app.config:
    ///   FastReceipt = true|false   (default true)   - use the fast printer
    ///   ReceiptWidthMm = 80                          - paper width in mm
    ///   ReceiptPreview = false|true                  - show a preview instead of printing
    /// When FastReceipt is false it falls back to the old POSPrintRpt (ReportViewer).
    /// </summary>
    public static class ReceiptPrinter
    {
        static string Cfg(string k, string d)
        {
            try { string v = ConfigurationManager.AppSettings[k]; return string.IsNullOrEmpty(v) ? d : v; }
            catch { return d; }
        }

        public static void Show(string salesId)
        {
            bool fast = Cfg("FastReceipt", "true").Trim().ToLowerInvariant() != "false";
            if (!fast)
            {
                POSPrintRpt r = new POSPrintRpt(salesId);
                r.ShowDialog();
                return;
            }
            try
            {
                new ReceiptDocument(salesId).Run();
            }
            catch (Exception ex)
            {
                Logger.Error("FastReceipt", ex);
                // fall back so a sale is never left without a receipt option
                POSPrintRpt r = new POSPrintRpt(salesId);
                r.ShowDialog();
            }
        }

        sealed class ReceiptDocument
        {
            readonly DataTable _dt;
            readonly DataTable _tenders;
            readonly float _widthMm;
            readonly bool _preview;
            int _row;   // current item row for pagination

            public ReceiptDocument(string salesId)
            {
                _dt = DataAccess.GetDataTable(
                    " SELECT sp.sales_id, sp.payment_amount, sp.change_amount, sp.due_amount, sp.dis, sp.vat, " +
                    "        sp.sales_time, sp.emp_id, sp.payment_type, sp.SaleType, " +
                    "        si.itemName, si.Qty, si.RetailsPrice, si.Total, si.taxapply, " +
                    "        tl.CompanyName, tl.Branchname, tl.Location, tl.Phone, tl.Email, tl.Web, tl.VATRegiNo, tl.Footermsg, " +
                    "        c.Name AS CustName, c.Phone AS CustPhone " +
                    " FROM sales_payment sp " +
                    " INNER JOIN sales_item si ON sp.sales_id = si.sales_id " +
                    " LEFT JOIN tbl_terminalLocation tl ON sp.Shopid = tl.Shopid " +
                    " LEFT JOIN tbl_customer c ON sp.c_id = c.ID " +
                    " WHERE sp.sales_id = @id ORDER BY si.item_id",
                    DataAccess.P("@id", salesId));
                _tenders = DataAccess.GetDataTable(
                    "SELECT method, amount FROM tbl_sale_tender WHERE sales_id = @id ORDER BY id",
                    DataAccess.P("@id", salesId));

                float mm; if (!float.TryParse(Cfg("ReceiptWidthMm", "80"), out mm)) mm = 80;
                _widthMm = mm;
                _preview = Cfg("ReceiptPreview", "false").Trim().ToLowerInvariant() == "true";
            }

            public void Run()
            {
                if (_dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data found for this receipt.", "Receipt", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                using (PrintDocument doc = new PrintDocument())
                {
                    // paper width in hundredths of an inch; long roll height
                    int widthHund = (int)Math.Round(_widthMm / 25.4 * 100);
                    try { doc.DefaultPageSettings.PaperSize = new PaperSize("Receipt", widthHund, 3276); } catch { }
                    doc.DefaultPageSettings.Margins = new Margins(6, 6, 6, 6);
                    doc.PrintPage += PrintPage;
                    _row = 0;

                    if (_preview)
                    {
                        using (PrintPreviewDialog pv = new PrintPreviewDialog())
                        {
                            pv.Document = doc;
                            pv.WindowState = FormWindowState.Maximized;
                            pv.ShowDialog();
                        }
                    }
                    else
                    {
                        doc.Print();   // straight to the default printer - instant
                    }
                }
            }

            string S(DataRow r, string col) { return r.Table.Columns.Contains(col) && r[col] != DBNull.Value ? r[col].ToString() : ""; }
            static decimal D(object o) { decimal d; return o != null && o != DBNull.Value && decimal.TryParse(o.ToString(), out d) ? d : 0m; }

            void PrintPage(object sender, PrintPageEventArgs e)
            {
                Graphics g = e.Graphics;
                g.PageUnit = GraphicsUnit.Point;
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                float left = 4;
                float right = _widthMm / 25.4f * 72f - 4;   // page width in points
                float width = right - left;
                float y = 4;

                using (Font big = new Font("Segoe UI", 10, FontStyle.Bold))
                using (Font norm = new Font("Consolas", 8, FontStyle.Regular))
                using (Font bold = new Font("Consolas", 8, FontStyle.Bold))
                {
                    DataRow h = _dt.Rows[0];

                    if (_row == 0)   // header only on first page
                    {
                        y = Center(g, S(h, "CompanyName"), big, left, width, y);
                        y = Center(g, S(h, "Branchname"), norm, left, width, y);
                        y = Center(g, S(h, "Location"), norm, left, width, y);
                        string ph = S(h, "Phone"); if (ph != "") y = Center(g, "Tel: " + ph, norm, left, width, y);
                        string web = S(h, "Web"); if (web != "") y = Center(g, web, norm, left, width, y);
                        string vat = S(h, "VATRegiNo"); if (vat != "") y = Center(g, "VAT: " + vat, norm, left, width, y);
                        y += 2; y = Line(g, left, right, y);

                        y = LeftRight(g, "Invoice: " + S(h, "sales_id"), S(h, "sales_time"), norm, left, right, y);
                        y = LeftRight(g, "Cashier: " + S(h, "emp_id"), S(h, "payment_type"), norm, left, right, y);
                        string cn = S(h, "CustName"); if (cn != "") y = LeftRight(g, "Customer: " + cn, S(h, "CustPhone"), norm, left, right, y);
                        y = Line(g, left, right, y);

                        y = Row3(g, "Item", "Qty", "Amount", bold, left, right, y);
                        y = Line(g, left, right, y);
                    }

                    // item lines (paginate if the page fills)
                    decimal subtotal = 0;
                    for (int i = 0; i < _dt.Rows.Count; i++) subtotal += D(_dt.Rows[i]["Total"]);

                    while (_row < _dt.Rows.Count)
                    {
                        if (y > e.PageBounds.Height - 90) { e.HasMorePages = true; return; }
                        DataRow r = _dt.Rows[_row];
                        string name = S(r, "itemName");
                        if (S(r, "taxapply") == "1") name += " *";
                        string qtyPrice = D(r["Qty"]).ToString("0.##") + " x " + D(r["RetailsPrice"]).ToString("0.00");
                        y = Row3(g, name, qtyPrice, D(r["Total"]).ToString("0.00"), norm, left, right, y);
                        _row++;
                    }

                    // totals
                    y = Line(g, left, right, y);
                    y = LeftRight(g, "Subtotal", subtotal.ToString("0.00"), norm, left, right, y);
                    decimal dis = D(h["dis"]); if (dis != 0) y = LeftRight(g, "Discount", "-" + dis.ToString("0.00"), norm, left, right, y);
                    decimal vatv = D(h["vat"]); if (vatv != 0) y = LeftRight(g, "Tax", vatv.ToString("0.00"), norm, left, right, y);
                    y = LeftRight(g, "TOTAL", D(h["payment_amount"]).ToString("0.00"), bold, left, right, y);

                    if (_tenders.Rows.Count > 0)
                        foreach (DataRow t in _tenders.Rows)
                            y = LeftRight(g, "  " + t["method"], D(t["amount"]).ToString("0.00"), norm, left, right, y);

                    decimal chg = D(h["change_amount"]); if (chg != 0) y = LeftRight(g, "Change", chg.ToString("0.00"), norm, left, right, y);
                    decimal due = D(h["due_amount"]); if (due != 0) y = LeftRight(g, "Due", due.ToString("0.00"), bold, left, right, y);

                    y = Line(g, left, right, y);
                    string foot = S(h, "Footermsg"); if (foot != "") y = Center(g, foot, norm, left, width, y);
                    y = Center(g, "* = taxable", norm, left, width, y);
                    y = Center(g, DateTime.Now.ToString("yyyy-MM-dd HH:mm"), norm, left, width, y);
                    e.HasMorePages = false;
                }
            }

            // The drawing helpers below use the (text, font, brush, x, y) overload with
            // manually measured alignment. This renders reliably on every GDI+ backend
            // (avoids RectangleF/StringFormat clipping quirks) and keeps amounts aligned.
            static readonly StringFormat NoWrap = new StringFormat(StringFormatFlags.NoWrap | StringFormatFlags.NoClip);

            float TextW(Graphics g, string t, Font f) { return g.MeasureString(t, f, PointF.Empty, NoWrap).Width; }
            float TextH(Graphics g, string t, Font f) { return g.MeasureString(t, f, PointF.Empty, NoWrap).Height; }

            float Center(Graphics g, string text, Font f, float left, float width, float y)
            {
                if (string.IsNullOrEmpty(text)) return y;
                float tw = TextW(g, text, f);
                float x = tw >= width ? left : left + (width - tw) / 2f;
                g.DrawString(text, f, Brushes.Black, x, y, NoWrap);
                return y + TextH(g, text, f);
            }

            float LeftRight(Graphics g, string l, string r, Font f, float left, float right, float y)
            {
                g.DrawString(l, f, Brushes.Black, left, y, NoWrap);
                g.DrawString(r, f, Brushes.Black, right - TextW(g, r, f), y, NoWrap);
                return y + TextH(g, l, f);
            }

            // name (wrapped if long) on the left, qty in the middle, amount right-aligned
            float Row3(Graphics g, string name, string mid, string amt, Font f, float left, float right, float y)
            {
                float width = right - left;
                float amtW = 55, midW = 62;
                float nameW = width - amtW - midW;
                float lineH = TextH(g, "X", f);
                // wrap the item name within nameW
                System.Collections.Generic.List<string> lines = Wrap(g, name, f, nameW);
                for (int i = 0; i < lines.Count; i++)
                    g.DrawString(lines[i], f, Brushes.Black, left, y + i * lineH, NoWrap);
                g.DrawString(mid, f, Brushes.Black, left + nameW + midW - TextW(g, mid, f), y, NoWrap);
                g.DrawString(amt, f, Brushes.Black, right - TextW(g, amt, f), y, NoWrap);
                return y + Math.Max(1, lines.Count) * lineH;
            }

            System.Collections.Generic.List<string> Wrap(Graphics g, string text, Font f, float maxW)
            {
                System.Collections.Generic.List<string> outp = new System.Collections.Generic.List<string>();
                if (string.IsNullOrEmpty(text)) { outp.Add(""); return outp; }
                string[] words = text.Split(' ');
                string cur = "";
                foreach (string w in words)
                {
                    string cand = cur.Length == 0 ? w : cur + " " + w;
                    if (TextW(g, cand, f) > maxW && cur.Length > 0) { outp.Add(cur); cur = w; }
                    else cur = cand;
                }
                if (cur.Length > 0) outp.Add(cur);
                return outp;
            }

            float Line(Graphics g, float left, float right, float y)
            {
                using (Pen p = new Pen(Color.Gray, 0.4f)) { g.DrawLine(p, left, y + 2, right, y + 2); }
                return y + 6;
            }
        }
    }
}
