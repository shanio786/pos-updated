using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.Report
{
    public partial class ReportDialog : Form
    {
        public ReportDialog()
        {
            InitializeComponent();
          //  dtStartDate.Text = dtStartDate.Value.AddDays(-30).ToShortDateString();           
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        } 

        private void btnContinue_Click(object sender, EventArgs e)
        {
            try
            {
                ReportValue.StartDate = dtStartDate.Text;
                ReportValue.EndDate = dtEndDate.Text;
                ReportValue.emp = cmbEmp.Text;
                ReportValue.Terminal = cmboterminal.SelectedValue == null ? "" : cmboterminal.SelectedValue.ToString();

                // optional filters: empty employee / terminal means "all"
                string empFilter = ReportValue.emp == "" ? "" : " AND sp.emp_id = @emp ";
                string shopFilter = ReportValue.Terminal == "" ? "" : " AND sp.Shopid = @shop ";

                DataTable sales = DataAccess.GetDataTable(
                    " SELECT sp.payment_amount, sp.due_amount, sp.dis, sp.vat, sp.sales_time, sp.emp_id, sp.sales_id " +
                    " FROM sales_payment sp " +
                    " WHERE sp.sales_time BETWEEN @from AND @to " + empFilter + shopFilter +
                    " ORDER BY sp.sales_time, sp.sales_id",
                    Params());
                sales.TableName = "DS_SaleReport";

                decimal inCash = DataAccess.GetDecimal(
                    " SELECT SUM(sp.payment_amount - sp.dis - sp.vat) FROM sales_payment sp " +
                    " WHERE sp.SaleType = 'CashSale' AND sp.sales_time BETWEEN @from AND @to " + empFilter + shopFilter,
                    Params());

                decimal recvd = DataAccess.GetDecimal(
                    " SELECT SUM(sp.receiveamt) FROM tbl_duepayment sp " +
                    " WHERE sp.receivedate BETWEEN @from AND @to " + empFilter + shopFilter,
                    Params());

                decimal returnAmount = DataAccess.GetDecimal(
                    " SELECT SUM(sp.Total) FROM return_item sp " +
                    " WHERE sp.return_time BETWEEN @from AND @to " +
                    (ReportValue.emp == "" ? "" : " AND sp.emp = @emp ") + shopFilter,
                    Params());

// Nicely-named columns for the grid report
                if (sales.Columns.Contains("payment_amount")) sales.Columns["payment_amount"].ColumnName = "Total";
                if (sales.Columns.Contains("due_amount")) sales.Columns["due_amount"].ColumnName = "Due";
                if (sales.Columns.Contains("dis")) sales.Columns["dis"].ColumnName = "Discount";
                if (sales.Columns.Contains("vat")) sales.Columns["vat"].ColumnName = "Tax";
                if (sales.Columns.Contains("sales_time")) sales.Columns["sales_time"].ColumnName = "Date";
                if (sales.Columns.Contains("emp_id")) sales.Columns["emp_id"].ColumnName = "Sold by";
                if (sales.Columns.Contains("sales_id")) sales.Columns["sales_id"].ColumnName = "Invoice";

                Report.FastReport.ShowReport(
                    "Sales Report  (" + ReportValue.StartDate + " to " + ReportValue.EndDate + ")",
                    sales,
                    "Cash: " + inCash.ToString("0.00"),
                    "Due received: " + recvd.ToString("0.00"),
                    "Returns: " + returnAmount.ToString("0.00"));
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not build the sales report.");
            }
        }

        /// <summary>Fresh parameter set for each query (a SqlParameter can only belong to one command).</summary>
        static SqlParameter[] Params()
        {
            return new SqlParameter[] {
                DataAccess.P("@from", ReportValue.StartDate),
                DataAccess.P("@to", ReportValue.EndDate),
                DataAccess.P("@emp", ReportValue.emp),
                DataAccess.P("@shop", ReportValue.Terminal)
            };
        }

        private void button1_Click(object sender, EventArgs e)
        {
            salesreport go = new salesreport();
           // go.MdiParent = this;
            go.Show();
        }

        private void ReportDialog_Load(object sender, EventArgs e)
        {
            try
            {
                dtStartDate.Format = DateTimePickerFormat.Custom;
                dtStartDate.CustomFormat = "yyyy-MM-dd";
                dtEndDate.Format = DateTimePickerFormat.Custom;
                dtEndDate.CustomFormat = "yyyy-MM-dd";

                string sql5 = "   select     DISTINCT '' as Username    from usermgt  union all " +  
                                " select   DISTINCT  Username   from usermgt ";
                DataTable dt5 = DataAccess.GetDataTable(sql5);
                cmbEmp.DataSource = dt5;
                cmbEmp.DisplayMember = "Username";


                string sqltr = " select  DISTINCT '' as BranchName ,'' as Shopid from tbl_terminalLocation  union all" +
                               " select   BranchName , Shopid from tbl_terminalLocation   ";
                DataTable dttr = DataAccess.GetDataTable(sqltr);
                cmboterminal.DataSource = dttr;
                cmboterminal.DisplayMember = "BranchName";
                cmboterminal.ValueMember = "Shopid";
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbEmp.Text = "";
            cmboterminal.Text =  "";
            cmboterminal.SelectedValue = "";
        }

       
    }
}
