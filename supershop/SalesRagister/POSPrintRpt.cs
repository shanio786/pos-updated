using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace supershop
{
    public partial class POSPrintRpt : Form
    {
        public POSPrintRpt(string saleno)
        {
            InitializeComponent();
            toolsaleno.Text = saleno;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }
         
        private void POSPrintRpt_Load(object sender, EventArgs e)
        {
            try
            {
               string sql = " SELECT  sp.sales_id AS salesid, sp.payment_type AS paytype, sp.payment_amount AS Payamount, " +
                            " sp.change_amount AS charAmt, sp.due_amount AS due, sp.dis, sp.vat, sp.sales_time AS s_time,  " +
                            " sp.c_id AS custID, sp.emp_id AS empID, sp.comment AS Note, sp.TrxType, si.sales_id,si.item_id,  " +
                            " si.itemName, si.Qty, si.RetailsPrice, si.Total,si.profit, si.sales_time , sp.Shopid, tl.*,c.* ,  " +
                            " CASE     " +
							" WHEN si.taxapply = 1 THEN 'TX'  " +
							" ELSE ''  " +
							" END 'TaxApply'  " +
                            " FROM            sales_payment sp " +
                            " INNER JOIN   sales_item si " +
                            " ON sp.sales_id  = si.sales_id " +
                            " INNER JOIN tbl_terminalLocation tl " +
                            " ON sp.Shopid  = tl.Shopid " +
                            " INNER JOIN tbl_customer c " +
                            " ON  sp.c_id  = c.ID " +
                            " Where sp.sales_id  = '" + toolsaleno.Text + "'  ";         
                DataAccess.ExecuteSQL(sql);
                DataTable dt = DataAccess.GetDataTable(sql);

                ReportDataSource reportDSDetail = new ReportDataSource("POSPRINTDataSet", dt);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(reportDSDetail);
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
                //this.reportViewer1.ZoomPercent = 80;
                this.reportViewer1.RefreshReport();
                
                // Code added at February 06, 2021
                timerpregress.Interval = 100;
                timerpregress.Start();
                this.reportViewer1.PrintDialog();
                // Code added at February 06, 2021

                if (parameter.autoprintid == "1")
                {
                    timerpregress.Interval = 100;
                    timerpregress.Start();
                    this.reportViewer1.PrintDialog();
                }
                else
                {
                    timerpregress.Stop();
                    toolstrpProgressCount.Visible = false;
                    toolStripProgressBar1.Visible = false;
                    btnstopPrint.Visible = false;
                }
              
            }
            catch
            {
            }
        }

        private void btnPrintDialog_Click(object sender, EventArgs e)
        {
            this.reportViewer1.PrintDialog();
        }

        //Auto open printpreview 
        public void prgressbar()
        {

            toolStripProgressBar1.Increment(5);
            toolstrpProgressCount.Text = " " + toolStripProgressBar1.Value.ToString() + "%";
            if (toolStripProgressBar1.Value == toolStripProgressBar1.Maximum)
            {
                timerpregress.Stop();
                this.reportViewer1.PrintDialog();
                timerpregress.Stop();
            }
        }
        private void timerpregress_Tick(object sender, EventArgs e)
        {
            try
            {
                prgressbar();
            }
            catch
            {
            }
        }

        private void btnstopPrint_Click(object sender, EventArgs e)
        {
            if (btnstopPrint.Text != "START")
            {
                timerpregress.Stop();
                btnstopPrint.Text = "START";
            }
            else
            { 
                btnstopPrint.Text = "STOP";
                timerpregress.Start();
            }
           
        }
    }
}
