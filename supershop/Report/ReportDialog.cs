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
                ReportValue.StartDate = dtStartDate.Text; // dtStartDate.Value.ToShortDateString();
                ReportValue.EndDate = dtEndDate.Text; // dtEndDate.Value.ToShortDateString();
                ReportValue.emp = cmbEmp.Text;
                ReportValue.Terminal = cmboterminal.SelectedValue.ToString();
                decimal recvd = 0;
                decimal inCash = 0;
                decimal returnAmount = 0;

                string query = "";
                DataSet ds = new DataSet();

                if (ReportValue.emp == "" && ReportValue.Terminal == "")   //Report by Every transaction -  Only Date to Date 
                {
                    query = @"select  payment_amount,due_amount,
                     dis, vat, sales_time ,emp_id , sales_id
                     from sales_payment 
                     where sales_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "' Order  by sales_time";

                    SqlCommand cmd = new SqlCommand(query, DataAccess.Connection);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds, "DS_SaleReport");
                    query = @"select  SUM(payment_amount-dis-vat) from sales_payment
                     where SaleType = 'CashSale' And sales_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "'";

                    try
                    {
                        inCash = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { inCash = 0; }
                    if (Convert.ToString(inCash) == "")
                    {
                        inCash = 0;
                    }

                    query = @"select  SUM(receiveamt) from tbl_duepayment
                     where receivedate BETWEEN '" + ReportValue.StartDate + "' AND '" + ReportValue.EndDate + "'";
                    try
                    {
                        recvd = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { recvd = 0; }
                    if (Convert.ToString(recvd) == "")
                    {
                        recvd = 0;
                    }


                    query = @"select  SUM(Total) from return_item
                    where  return_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "'";

                    try
                    {
                        returnAmount = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { returnAmount = 0; }
                    if (Convert.ToString(returnAmount) == "")
                    {
                        returnAmount = 0;
                    }
                }
                else if (ReportValue.emp != "" && ReportValue.Terminal == "")   //Report by Every transaction -  Employee with date to date 
                {
                    query = @"select  payment_amount,due_amount,
                     dis, vat, sales_time ,emp_id , sales_id
                     from sales_payment 
                     where sales_time BETWEEN 
                     '" + ReportValue.StartDate + "' AND '" + ReportValue.EndDate + "' AND sales_payment.emp_id = '" + ReportValue.emp + "' Order  by sales_time";

                    SqlCommand cmd = new SqlCommand(query, DataAccess.Connection);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds, "DS_SaleReport");
                    query = @"select  SUM(payment_amount-dis-vat) from sales_payment
                     where SaleType = 'CashSale' And sales_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "' AND sales_payment.emp_id = '" + ReportValue.emp + "' ";

                    try
                    {
                        inCash = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { inCash = 0; }
                    if (Convert.ToString(inCash) == "")
                    {
                        inCash = 0;
                    }

                    query = @"select  SUM(receiveamt) from tbl_duepayment
                     where receivedate BETWEEN '" + ReportValue.StartDate + "' AND '" + ReportValue.EndDate + "' AND emp_id = '" + ReportValue.emp + "' ";
                    try
                    {
                        recvd = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { recvd = 0; }
                    if (Convert.ToString(recvd) == "")
                    {
                        recvd = 0;
                    }

                    query = @"select  SUM(Total) from return_item
                    where  return_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "' AND emp = '" + ReportValue.emp + "' ";

                    try
                    {
                        returnAmount = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { returnAmount = 0; }
                    if (Convert.ToString(returnAmount) == "")
                    {
                        returnAmount = 0;
                    }
                }
                else if (ReportValue.emp == "" && ReportValue.Terminal != "")     //Report by Every transaction -    Terminal with date to date
                {
                    query = @"select  payment_amount,due_amount,
                     dis, vat, sales_time ,emp_id , sales_id
                     from sales_payment 
                     where sales_time BETWEEN 
                     '" + ReportValue.StartDate + "' AND '" + ReportValue.EndDate + "' AND sales_payment.Shopid = '" + ReportValue.Terminal + "'  Order  by sales_time";

                    SqlCommand cmd = new SqlCommand(query, DataAccess.Connection);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds, "DS_SaleReport");
                    query = @"select  SUM(payment_amount-dis-vat) from sales_payment
                     where SaleType = 'CashSale' And sales_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "' AND  sales_payment.Shopid = '" + ReportValue.Terminal + "'";

                    try
                    {
                        inCash = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { inCash = 0; }
                    if (Convert.ToString(inCash) == "")
                    {
                        inCash = 0;
                    }

                    query = @"select  SUM(receiveamt) from tbl_duepayment
                     where receivedate BETWEEN '" + ReportValue.StartDate + "' AND '" + ReportValue.EndDate + "' AND Shopid = '" + ReportValue.Terminal + "'";
                    try
                    {
                        recvd = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { recvd = 0; }
                    if (Convert.ToString(recvd) == "")
                    {
                        recvd = 0;
                    }

                    query = @"select  SUM(Total) from return_item
                    where  return_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "' AND Shopid = '" + ReportValue.Terminal + "'";

                    try
                    {
                        returnAmount = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { returnAmount = 0; }
                    if (Convert.ToString(returnAmount) == "")
                    {
                        returnAmount = 0;
                    }
                }
                else if (ReportValue.emp != "" && ReportValue.Terminal != "")   //Report by Every transaction -  Employee and  Terminal with date to date  -- All
                {
                    query = @"select  payment_amount,due_amount,
                     dis, vat, sales_time ,emp_id , sales_id
                     from sales_payment 
                     where sales_time BETWEEN 
                     '" + ReportValue.StartDate + "' AND '" + ReportValue.EndDate + "'AND sales_payment.emp_id = '" + ReportValue.emp + "' AND sales_payment.Shopid = '" + ReportValue.Terminal + "'  Order  by sales_time";

                    SqlCommand cmd = new SqlCommand(query, DataAccess.Connection);
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds, "DS_SaleReport");
                    query = @"select  SUM(payment_amount-dis-vat) from sales_payment
                     where SaleType = 'CashSale' And sales_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "' AND sales_payment.emp_id = '" + ReportValue.emp + "' AND sales_payment.Shopid = '" + ReportValue.Terminal + "'";

                    try
                    {
                        inCash = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { inCash = 0; }
                    if (Convert.ToString(inCash) == "")
                    {
                        inCash = 0;
                    }

                    query = @"select  SUM(receiveamt) from tbl_duepayment
                     where receivedate BETWEEN '" + ReportValue.StartDate + "' AND '" + ReportValue.EndDate + "' AND emp_id = '" + ReportValue.emp + "' AND Shopid = '" + ReportValue.Terminal + "'";
                    try
                    {
                        recvd = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { recvd = 0; }
                    if (Convert.ToString(recvd) == "")
                    {
                        recvd = 0;
                    }

                    query = @"select  SUM(Total) from return_item
                    where  return_time BETWEEN '" + ReportValue.StartDate + "' AND    '" + ReportValue.EndDate + "' AND emp = '" + ReportValue.emp + "' AND Shopid = '" + ReportValue.Terminal + "'";

                    try
                    {
                        returnAmount = Convert.ToDecimal(DataAccess.ExecuteSQLScaler(query));
                    }
                    catch { returnAmount = 0; }
                    if (Convert.ToString(returnAmount) == "")
                    {
                        returnAmount = 0;
                    }
                }
                Report.SaleReport exprpr = new Report.SaleReport();
                exprpr.SetDataSource(ds.Tables[0]);
                exprpr.DataDefinition.FormulaFields["due_recv"].Text = "" + recvd + "";
                exprpr.DataDefinition.FormulaFields["incash2"].Text = "" + inCash + "";
                exprpr.DataDefinition.FormulaFields["return"].Text = "" + returnAmount + "";


                ReportViwer rp = new ReportViwer();


                TextObject dtFrom = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["dtFrom"];
                dtFrom.Text = ReportValue.StartDate;

                TextObject dtTo = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["dtTo"];
                dtTo.Text = ReportValue.StartDate;

                rp.Show();
                rp.crystalReportViewer1.ReportSource = exprpr;
                rp.crystalReportViewer1.Refresh();
                //Report.SaleReportRdlc go = new Report.SaleReportRdlc();
                //go.ShowDialog();		    
            }
            catch
            {
            }
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
                DataAccess.ExecuteSQL(sql5);
                DataTable dt5 = DataAccess.GetDataTable(sql5);
                cmbEmp.DataSource = dt5;
                cmbEmp.DisplayMember = "Username";


                string sqltr = " select  DISTINCT '' as BranchName ,'' as Shopid from tbl_terminalLocation  union all" +
                               " select   BranchName , Shopid from tbl_terminalLocation   ";
                DataAccess.ExecuteSQL(sqltr);
                DataTable dttr = DataAccess.GetDataTable(sqltr);
                cmboterminal.DataSource = dttr;
                cmboterminal.DisplayMember = "BranchName";
                cmboterminal.ValueMember = "Shopid";
            }
            catch
            {
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cmbEmp.Text = "";
            cmboterminal.Text =  "";
            cmboterminal.SelectedValue = "";
        }

       
    }
}
