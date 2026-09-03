using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace supershop.BarCode
{
    /// <summary>
    /// Fast barcode label printing, drawn directly with GDI+ (Code 128) - no
    /// Crystal report to load. Prints shelf/product labels with the item name,
    /// a scannable barcode, the code and the price. Works for any product code.
    ///
    ///   LabelPrinter.Print(new LabelPrinter.Label("Grapes (lb)", "2200000000018", "1.99"), copies, preview);
    /// </summary>
    public static class LabelPrinter
    {
        public sealed class Label
        {
            public string Name, Code, Price;
            public Label(string name, string code, string price) { Name = name; Code = code; Price = price; }
        }

        public static void Print(Label label, int copies, bool preview)
        {
            if (label == null || string.IsNullOrEmpty(label.Code)) { MessageBox.Show("Nothing to print."); return; }
            if (copies < 1) copies = 1;
            int printed = 0;
            using (PrintDocument doc = new PrintDocument())
            {
                doc.DefaultPageSettings.Margins = new Margins(20, 20, 20, 20);
                doc.PrintPage += delegate(object s, PrintPageEventArgs e)
                {
                    Graphics g = e.Graphics;
                    g.PageUnit = GraphicsUnit.Point;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;   // crisp bars
                    // label box in points; lay out a grid across the page
                    float labW = 150, labH = 90, gap = 10;
                    float x0 = e.MarginBounds.Left / 1.333f, y0 = e.MarginBounds.Top / 1.333f;
                    float pageW = e.PageBounds.Width, pageH = e.PageBounds.Height;
                    int cols = Math.Max(1, (int)((pageW - 2 * x0) / (labW + gap)));
                    float x = x0, y = y0;
                    int onPage = 0;
                    while (printed < copies)
                    {
                        DrawLabel(g, label, x, y, labW, labH);
                        printed++; onPage++;
                        x += labW + gap;
                        if (x + labW > pageW - x0) { x = x0; y += labH + gap; }
                        if (y + labH > pageH - y0 && printed < copies) { e.HasMorePages = true; return; }
                    }
                    e.HasMorePages = false;
                };
                try
                {
                    if (preview)
                        using (PrintPreviewDialog pv = new PrintPreviewDialog()) { pv.Document = doc; pv.WindowState = FormWindowState.Maximized; pv.ShowDialog(); }
                    else
                        doc.Print();
                }
                catch (Exception ex) { Logger.Show(ex, "Could not print labels."); }
            }
        }

        static void DrawLabel(Graphics g, Label label, float x, float y, float w, float h)
        {
            using (Font nameF = new Font("Segoe UI", 8, FontStyle.Bold))
            using (Font small = new Font("Consolas", 7, FontStyle.Regular))
            using (Font priceF = new Font("Segoe UI", 10, FontStyle.Bold))
            {
                var nf = new StringFormat { Alignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap };
                // name
                g.DrawString(Trim(label.Name, 22), nameF, Brushes.Black, new RectangleF(x, y + 2, w, 14), nf);
                // barcode
                float bcTop = y + 18, bcH = h - 44;
                DrawCode128(g, label.Code, x + 8, bcTop, w - 16, bcH);
                // human readable code
                g.DrawString(label.Code, small, Brushes.Black, new RectangleF(x, y + h - 26, w, 12), nf);
                // price
                if (!string.IsNullOrEmpty(label.Price))
                    g.DrawString("Rs " + label.Price, priceF, Brushes.Black, new RectangleF(x, y + h - 15, w, 14), nf);
            }
        }

        static string Trim(string s, int n) { s = s ?? ""; return s.Length <= n ? s : s.Substring(0, n); }

        // ---- Code 128 (subset B) ----
        static readonly int[][] Patterns = BuildPatterns();

        static void DrawCode128(Graphics g, string data, float x, float y, float w, float h)
        {
            List<int> codes = Encode128B(data);
            // total modules
            int modules = 0;
            foreach (int c in codes) modules += 11;
            modules += 13; // stop pattern (7 elements = 13 modules) already? add quiet zones
            int totalModules = 0;
            foreach (int c in codes) { foreach (int el in Patterns[c]) totalModules += el; }
            foreach (int el in Patterns[106]) totalModules += el; // stop
            int quiet = 10;
            float unit = w / (totalModules + 2 * quiet);
            float bx = x + quiet * unit;
            // draw each pattern: bar,space,bar,... starting with bar
            List<int> all = new List<int>(codes); all.Add(106); // stop
            foreach (int c in all)
            {
                bool bar = true;
                foreach (int el in Patterns[c])
                {
                    float ew = el * unit;
                    if (bar) g.FillRectangle(Brushes.Black, bx, y, ew + 0.2f, h);
                    bx += ew;
                    bar = !bar;
                }
            }
        }

        static List<int> Encode128B(string data)
        {
            List<int> vals = new List<int>();
            vals.Add(104); // Start B
            long sum = 104;
            int pos = 1;
            foreach (char ch in data)
            {
                int v = ch - 32;                // Code B value for printable ASCII
                if (v < 0 || v > 94) v = '0' - 32;
                vals.Add(v);
                sum += (long)v * pos;
                pos++;
            }
            vals.Add((int)(sum % 103));          // checksum
            return vals;
        }

        // Widths for Code 128 values 0..106 (6 elements each; 106 = stop, 7 elements)
        static int[][] BuildPatterns()
        {
            string[] w = {
            "212222","222122","222221","121223","121322","131222","122213","122312","132212","221213",
            "221312","231212","112232","122132","122231","113222","123122","123221","223211","221132",
            "221231","213212","223112","312131","311222","321122","321221","312212","322112","322211",
            "212123","212321","232121","111323","131123","131321","112313","132113","132311","211313",
            "231113","231311","112133","112331","132131","113123","113321","133121","313121","211331",
            "231131","213113","213311","213131","311123","311321","331121","312113","312311","332111",
            "314111","221411","431111","111224","111422","121124","121421","141122","141221","112214",
            "112412","122114","122411","142112","142211","241211","221114","413111","241112","134111",
            "111242","121142","121241","114212","124112","124211","411212","421112","421211","212141",
            "214121","412121","111143","111341","131141","114113","114311","411113","411311","113141",
            "114131","311141","411131","211412","211214","211232","2331112" };
            int[][] p = new int[w.Length][];
            for (int i = 0; i < w.Length; i++)
            {
                int[] a = new int[w[i].Length];
                for (int j = 0; j < w[i].Length; j++) a[j] = w[i][j] - '0';
                p[i] = a;
            }
            return p;
        }
    }
}
