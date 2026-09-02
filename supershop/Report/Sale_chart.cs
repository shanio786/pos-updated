using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace supershop
{
    public partial class Sale_chart : Form
    {
        public Sale_chart()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Sale_chart_Load(object sender, EventArgs e)
        {
            dtyearmonth.Format = DateTimePickerFormat.Custom;
            dtyearmonth.CustomFormat = "yyyy-MM";
            LoadCharts(DateTime.Now.ToString("yyyy-MM"));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadCharts(dtyearmonth.Value.ToString("yyyy-MM"));
        }

        /// <summary>Daily sales and profit for one month ('yyyy-MM'); sold and partly-returned lines only.</summary>
        void LoadCharts(string yearMonth)
        {
            try
            {
                string sql5 = "select sales_time, SUM(total) as Total from sales_item " +
                              " where sales_time like @ym and (status = 1 or status = 3) GROUP BY sales_time order by sales_time";
                DataTable dt5 = DataAccess.GetDataTable(sql5, DataAccess.P("@ym", "%" + yearMonth + "%"));
                chartBarSale.DataSource = dt5;
                chartBarSale.Visible = true;
                chartBarSale.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
                chartBarSale.Series["Sale"].XValueMember = "sales_time";
                chartBarSale.Series["Sale"].YValueMembers = "Total";
                chartBarSale.DataBind();

                string sql2 = "select sales_time, SUM(total) as Total, SUM(profit * Qty) as Profit from sales_item " +
                              " where sales_time like @ym and (status = 1 or status = 3) GROUP BY sales_time order by sales_time";
                DataTable dt2 = DataAccess.GetDataTable(sql2, DataAccess.P("@ym", "%" + yearMonth + "%"));
                chartBarSalesProfitCom.DataSource = dt2;
                chartBarSalesProfitCom.Visible = true;
                chartBarSalesProfitCom.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
                chartBarSalesProfitCom.Series["Sale"].XValueMember = "sales_time";
                chartBarSalesProfitCom.Series["Sale"].YValueMembers = "Total";
                chartBarSalesProfitCom.Series["Profit"].XValueMember = "sales_time";
                chartBarSalesProfitCom.Series["Profit"].YValueMembers = "Profit";
                chartBarSalesProfitCom.DataBind();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the sales chart."); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                chartBarSale.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                chartBarSale.Printing.PrintPreview();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                chartBarSalesProfitCom.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                chartBarSalesProfitCom.Printing.PrintPreview();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Overview go = new Overview();
            go.MdiParent = this.ParentForm;
            go.Show();

            this.Hide();
        }
    }
}
