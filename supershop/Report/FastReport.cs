using System;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace supershop.Report
{
    /// <summary>
    /// Reusable fast report window: shows a DataTable in a grid and prints/exports
    /// instantly using GDI+ (no Crystal / ReportViewer engine to load).
    ///
    ///   FastReport.Show("Sales Report", dataTable);                 // simple
    ///   FastReport.Show("Sales Report", dataTable, summaryLines);   // with a totals footer
    /// </summary>
    public partial class FastReport : Form
    {
        DataGridViewPrinter _printer;
        string _title;

        public FastReport()
        {
            InitializeComponent();
        }

        public static void ShowReport(string title, DataTable data, params string[] summary)
        {
            FastReport f = new FastReport();
            f.Populate(title, data, summary);
            f.Show();
        }

        void Populate(string title, DataTable data, string[] summary)
        {
            _title = title;
            this.Text = title;
            lblTitle.Text = title;
            grid.DataSource = data;
            grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            lblSummary.Text = (summary == null) ? "" : string.Join("      ", summary);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        string HeaderText()
        {
            // company header from the current shop's terminal, then the report title
            string header = _title;
            try
            {
                DataTable t = DataAccess.GetDataTable(
                    "SELECT TOP 1 CompanyName, Branchname, Location, Phone FROM tbl_terminalLocation WHERE ISNULL(Shopid,'') = @s ORDER BY ID",
                    DataAccess.P("@s", UserInfo.Shopid));
                if (t.Rows.Count > 0)
                    header = t.Rows[0]["CompanyName"] + "\n" + t.Rows[0]["Branchname"] + "  -  " + t.Rows[0]["Location"] +
                             "\nTel: " + t.Rows[0]["Phone"] + "\n" + _title + "\n" +
                             DateTime.Now.ToString("dd MMM yyyy  hh:mm tt") + "\n";
            }
            catch (Exception ex) { Logger.Error("FastReport header", ex); }
            if (!string.IsNullOrEmpty(lblSummary.Text)) header += "\n" + lblSummary.Text + "\n";
            return header;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                using (PrintDialog pd = new PrintDialog())
                {
                    if (pd.ShowDialog() != DialogResult.OK) return;
                    printDocument1.PrinterSettings = pd.PrinterSettings;
                    printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);
                    _printer = new DataGridViewPrinter(grid, printDocument1, true, true, HeaderText(),
                        new Font("Segoe UI", 10, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);
                    using (PrintPreviewDialog pv = new PrintPreviewDialog())
                    {
                        pv.Document = printDocument1;
                        pv.WindowState = FormWindowState.Maximized;
                        pv.ShowDialog();
                    }
                }
            }
            catch (Exception ex) { Logger.Show(ex, "Could not print the report."); }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (_printer.DrawDataGridView(e.Graphics)) e.HasMorePages = true;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.Filter = "CSV file (*.csv)|*.csv";
                    dlg.FileName = _title.Replace(" ", "_") + "_" + DateTime.Now.ToString("yyyy-MM-dd_HHmm") + ".csv";
                    if (dlg.ShowDialog() != DialogResult.OK) return;
                    File.WriteAllText(dlg.FileName, ToCsv(), Encoding.UTF8);
                    MessageBox.Show("Saved: " + dlg.FileName, "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex) { Logger.Show(ex, "Could not export the report."); }
        }

        string ToCsv()
        {
            StringBuilder sb = new StringBuilder();
            for (int c = 0; c < grid.Columns.Count; c++)
            {
                if (c > 0) sb.Append(',');
                sb.Append(Csv(grid.Columns[c].HeaderText));
            }
            sb.AppendLine();
            foreach (DataGridViewRow r in grid.Rows)
            {
                if (r.IsNewRow) continue;
                for (int c = 0; c < grid.Columns.Count; c++)
                {
                    if (c > 0) sb.Append(',');
                    sb.Append(Csv(r.Cells[c].Value == null ? "" : r.Cells[c].Value.ToString()));
                }
                sb.AppendLine();
            }
            if (!string.IsNullOrEmpty(lblSummary.Text)) { sb.AppendLine(); sb.AppendLine(Csv(lblSummary.Text)); }
            return sb.ToString();
        }

        static string Csv(string v)
        {
            if (v == null) v = "";
            if (v.IndexOfAny(new[] { ',', '"', '\n', '\r' }) >= 0) v = "\"" + v.Replace("\"", "\"\"") + "\"";
            return v;
        }

        private void btnClose_Click(object sender, EventArgs e) { this.Close(); }
    }
}
