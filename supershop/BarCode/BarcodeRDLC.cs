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
    public partial class BarcodeRDLC : Form
    {
        public BarcodeRDLC()
        {
            InitializeComponent();
        }

        private void BarcodeRDLC_Load(object sender, EventArgs e)
        {
            try
            {
                //Product Category
                string sql5 = "select    product_id  from purchase ";
                DataAccess.ExecuteSQL(sql5);
                DataTable dt5 = DataAccess.GetDataTable(sql5);
                cmbitems.DataSource = dt5;
                cmbitems.DisplayMember = "product_id";

                string sql = "Select product_name, product_id, retail_price  from purchase  ";
               // string sql = " select  * from purchase a, sales_item b where a.product_id = '8940000000003' limit 11 ";
                DataAccess.ExecuteSQL(sql);
                DataTable dt = DataAccess.GetDataTable(sql);

                ReportDataSource reportDSDetail = new ReportDataSource("DataSet1", dt);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(reportDSDetail);
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
                //this.reportViewer1.ZoomPercent = 35;
                this.reportViewer1.RefreshReport();
            }
            catch
            {
            }

        }

        private void bntSearch_Click(object sender, EventArgs e)
        {
            try
            {

                string sql = " select  product_name, product_id, retail_price  " + 
                             " from purchase a, sales_item b where a.product_id = '" + cmbitems.Text + "' limit  " + txtQuantity.Text + " ";
                DataAccess.ExecuteSQL(sql);
                DataTable dt = DataAccess.GetDataTable(sql);

                ReportDataSource reportDSDetail = new ReportDataSource("DataSet1", dt);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(reportDSDetail);
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
               // this.reportViewer1.ZoomPercent = 35;
                this.reportViewer1.RefreshReport();
            }
            catch
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string sql = " select Top " + txtQuantity.Text + " product_name, product_id, retail_price "+
                              "  from purchase a, sales_item b where a.product_id = '" + cmbitems.Text + "'    ";
                DataAccess.ExecuteSQL(sql);
                DataTable dt = DataAccess.GetDataTable(sql);

                ReportDataSource reportDSDetail = new ReportDataSource("DataSet1", dt);
                this.reportViewer1.LocalReport.DataSources.Clear();
                this.reportViewer1.LocalReport.DataSources.Add(reportDSDetail);
                this.reportViewer1.LocalReport.Refresh();
                this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
                this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
               // this.reportViewer1.ZoomPercent = 35;
                this.reportViewer1.RefreshReport();
            }
            catch
            {
            }
        }

        private void btnlink_Click(object sender, EventArgs e)
        {
            //CreateBarcode go = new CreateBarcode();
            //go.MdiParent = this.ParentForm;
            //go.Show();
            BarCode.BarcodeCreator go = new BarCode.BarcodeCreator();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

 
    }
}
