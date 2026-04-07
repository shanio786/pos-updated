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
    public partial class Chart : Form
    {
        public Chart()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        } 

        private void Chart_Load(object sender, EventArgs e)
        {
            try
            {
                //Product Category
                string sqlcate = "select   DISTINCT  category   from purchase ";
                DataAccess.ExecuteSQL(sqlcate);
                DataTable dtcate = DataAccess.GetDataTable(sqlcate);
                combCategory.DataSource = dtcate;
                combCategory.DisplayMember = "category";

                string sql5 = "select * from purchase where category = '"+ combCategory.Text +"' ";
                DataAccess.ExecuteSQL(sql5);
                DataTable dt5 = DataAccess.GetDataTable(sql5);
                chart1.DataSource = dt5;
                chart1.Visible = true;
                chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;

                chart1.Series[0].LegendText = "Cost Price";
                chart1.Series[1].LegendText = "Retail Price";

                chart1.Series["Cost_price"].XValueMember = "product_name";
                chart1.Series["Cost_price"].YValueMembers = "cost_price";

                chart1.Series["Retail_price"].XValueMember = "product_name";
                chart1.Series["Retail_price"].YValueMembers = "retail_price";
                chart1.DataBind();
            }
            catch
            {
            }
           
        }

        private void printGrid_Click(object sender, EventArgs e)
        {
            
            try
            {

                chart1.Printing.PrintDocument.DefaultPageSettings.Landscape = true;
                // Print preview chart
                chart1.Printing.PrintPreview();               
            }
            catch
            {

            }
        }

        private void combCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql5 = "select * from purchase where category = '" + combCategory.Text + "' ";
            DataAccess.ExecuteSQL(sql5);
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            chart1.DataSource = dt5;
            chart1.Visible = true;
            chart1.ChartAreas[0].AxisX.LabelStyle.Angle = 45;

            chart1.Series[0].LegendText = "Cost Price";
            chart1.Series[1].LegendText = "Retail Price";

            chart1.Series["Cost_price"].XValueMember = "product_name";
            chart1.Series["Cost_price"].YValueMembers = "cost_price";

            chart1.Series["Retail_price"].XValueMember = "product_name";
            chart1.Series["Retail_price"].YValueMembers = "retail_price";
            chart1.DataBind();
        }
    }
}
