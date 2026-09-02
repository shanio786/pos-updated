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
                DataTable dt5 = DataAccess.GetDataTable("select product_id from purchase");
                cmbitems.DataSource = dt5;
                cmbitems.DisplayMember = "product_id";

                ShowReport(DataAccess.GetDataTable("select product_name, product_id, retail_price from purchase"));
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void ShowReport(DataTable dt)
        {
            ReportDataSource reportDSDetail = new ReportDataSource("DataSet1", dt);
            this.reportViewer1.LocalReport.DataSources.Clear();
            this.reportViewer1.LocalReport.DataSources.Add(reportDSDetail);
            this.reportViewer1.LocalReport.Refresh();
            this.reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            this.reportViewer1.ZoomMode = ZoomMode.PageWidth;
            this.reportViewer1.RefreshReport();
        }

        // One report row per label: TOP (@n) over a cross join repeats the product @n times
        // (SQL Server has no LIMIT; sys.all_objects is just a row source that always has enough rows)
        private void LoadLabels()
        {
            int qty;
            if (!int.TryParse(txtQuantity.Text, out qty) || qty < 1)
            {
                MessageBox.Show("Please enter how many labels to print");
                txtQuantity.Focus();
                return;
            }
            string sql = " select top (@n) a.product_name, a.product_id, a.retail_price " +
                         " from purchase a cross join sys.all_objects n where a.product_id = @id";
            ShowReport(DataAccess.GetDataTable(sql, DataAccess.P("@n", qty), DataAccess.P("@id", cmbitems.Text)));
        }

        private void bntSearch_Click(object sender, EventArgs e)
        {
            try
            {
                LoadLabels();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                LoadLabels();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void btnlink_Click(object sender, EventArgs e)
        {
            BarCode.BarcodeCreator go = new BarCode.BarcodeCreator();
            go.MdiParent = this.ParentForm;
            go.Show();
        }
    }
}
