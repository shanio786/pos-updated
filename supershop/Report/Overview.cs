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
    public partial class Overview : Form
    {
        public Overview()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Overview_Load(object sender, EventArgs e)
        {
            dtyearmonth.Format = DateTimePickerFormat.Custom;
            dtyearmonth.CustomFormat = "yyyy-MM";
            LoadCharts(DateTime.Now.ToString("yyyy-MM"));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            LoadCharts(dtyearmonth.Value.ToString("yyyy-MM"));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadCharts(dtyearmonth.Value.ToString("yyyy-MM"));
        }

        /// <summary>Daily profit / sales charts for one month ('yyyy-MM'); sold and partly-returned lines only.</summary>
        void LoadCharts(string yearMonth)
        {
            try
            {
                //Profit bar chart
                string sql5 = " select sales_time, SUM(profit * Qty) as Profit from sales_item " +
                              " where sales_time like @ym and (status = 1 or status = 3) GROUP BY sales_time order by sales_time";
                DataTable dt5 = DataAccess.GetDataTable(sql5, DataAccess.P("@ym", "%" + yearMonth + "%"));
                chartbarProfit.DataSource = dt5;
                chartbarProfit.Visible = true;
                chartbarProfit.ChartAreas[0].AxisX.LabelStyle.Angle = 45;
                chartbarProfit.Series["Profit"].XValueMember = "sales_time";
                chartbarProfit.Series["Profit"].YValueMembers = "Profit";
                chartbarProfit.DataBind();

                //Profit pie chart
                string sql2 = " select SUM(profit * Qty) as Profit, sales_time from sales_item " +
                              " where sales_time like @ym and (status = 1 or status = 3) GROUP BY sales_time order by sales_time";
                DataTable dt2 = DataAccess.GetDataTable(sql2, DataAccess.P("@ym", "%" + yearMonth + "%"));
                chartPieProfit.DataSource = dt2;
                chartPieProfit.Visible = true;
                chartPieProfit.Series["Profit"].XValueMember = "sales_time";
                chartPieProfit.Series["Profit"].YValueMembers = "Profit";
                chartPieProfit.DataBind();

                // Sales pie chart
                string sql3 = " select sales_time, SUM(total) as Total from sales_item " +
                              " where sales_time like @ym and (status = 1 or status = 3) GROUP BY sales_time order by sales_time";
                DataTable dt3 = DataAccess.GetDataTable(sql3, DataAccess.P("@ym", "%" + yearMonth + "%"));
                chartPieSales.DataSource = dt3;
                chartPieSales.Visible = true;
                chartPieSales.Series["Total"].XValueMember = "sales_time";
                chartPieSales.Series["Total"].YValueMembers = "Total";
                chartPieSales.DataBind();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the overview charts."); }
        }

        private void chart2_Click(object sender, EventArgs e)
        {
        }

        private void chart2_MouseLeave(object sender, EventArgs e)
        {
            chartPieProfit.Dock = DockStyle.None;
            label3.Visible = true;
        }

        private void chart2_MouseHover(object sender, EventArgs e)
        {
            chartPieProfit.Dock = DockStyle.Fill;
            label3.Visible = false;
        }

        private void chart1_MouseHover(object sender, EventArgs e)
        {
            chartbarProfit.Dock = DockStyle.None;
            chartPieProfit.Visible = false;
            chartPieSales.Visible = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
        }

        private void chart1_MouseLeave(object sender, EventArgs e)
        {
            chartbarProfit.Dock = DockStyle.None;
            chartPieProfit.Visible = true;
            chartPieSales.Visible = true;

            label1.Visible = true;
            label2.Visible = true;
            label3.Visible = true;
        }

        private void chart3_MouseHover(object sender, EventArgs e)
        {
            chartPieSales.Dock = DockStyle.Fill;
            chartPieProfit.Visible = false;
            chartbarProfit.Visible = false;
            label2.Visible = false;
        }

        private void chart3_MouseLeave(object sender, EventArgs e)
        {
            chartPieSales.Dock = DockStyle.None;
            chartPieProfit.Visible = true;
            chartbarProfit.Visible = true;
            label2.Visible = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                chartPieProfit.Dock = DockStyle.Fill;
                chartPieSales.Dock = DockStyle.Fill;
                chartbarProfit.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                chartPieProfit.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                chartPieSales.Printing.PrintDocument.DefaultPageSettings.Landscape = true;

                chartbarProfit.Printing.PrintPreview();
                chartPieProfit.Printing.PrintPreview();
                chartPieSales.Printing.PrintPreview();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Sale_chart go = new Sale_chart();
            go.MdiParent = this.ParentForm;
            go.Show();

            this.Hide();
        }
    }
}
