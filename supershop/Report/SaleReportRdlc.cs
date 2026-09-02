using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace supershop.Report
{
    public partial class SaleReportRdlc : Form
    {
        public SaleReportRdlc()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SaleReportRdlc_Load(object sender, EventArgs e)
        {
            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            try
            {
                string emp = ReportValue.emp ?? "";
                string terminal = ReportValue.Terminal ?? "";

                // Report header: date range plus the optional employee / terminal filters
                string paravalue = ReportValue.StartDate + "  To  " + ReportValue.EndDate;
                if (emp != "" && terminal != "")
                    paravalue = "Report by : " + paravalue + " and " + emp + " - " + terminal;
                else if (emp != "")
                    paravalue = "Report by : " + paravalue + " and " + emp;
                else if (terminal != "")
                    paravalue = "Report by : " + paravalue + " and " + terminal;

                ReportParameter parReportParam1 = new ReportParameter("Dates", paravalue);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { parReportParam1 });

                // Empty employee / terminal means "all"
                string sql = "select sc.*, sp.payment_amount AS Payamount, sp.due_amount AS due, " +
                             " sp.dis, sp.vat, sp.sales_time AS sales_time, sp.emp_id AS empID, sp.sales_id AS salesid " +
                             " from sales_payment sp, storeconfig sc " +
                             " where sp.sales_time BETWEEN @from AND @to " +
                             (emp == "" ? "" : " AND sp.emp_id = @emp ") +
                             (terminal == "" ? "" : " AND sp.Shopid = @shop ") +
                             " order by sp.sales_time";
                DataTable dt = DataAccess.GetDataTable(sql,
                    DataAccess.P("@from", ReportValue.StartDate),
                    DataAccess.P("@to", ReportValue.EndDate),
                    DataAccess.P("@emp", emp),
                    DataAccess.P("@shop", terminal));

                ReportDataSource reportDSDetail = new ReportDataSource("DataSet1", dt);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(reportDSDetail);

                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
                this.reportViewer1.RefreshReport();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the report."); }
        }
    }
}
