using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.BarCode
{
    public partial class newBarcode_Generator : Form
    {
        DataTable dt;
        ReportDocument rd = new ReportDocument();

        public newBarcode_Generator()
        {
            InitializeComponent();
        }

        private void newBarcode_Generator_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt5 = DataAccess.GetDataTable("select product_id from purchase");
                cmbitems.DataSource = dt5;
                cmbitems.DisplayMember = "product_id";

                dt = new DataTable();
                dt.TableName = "BarDS";
                dt.Columns.Add("product_id", typeof(string));
                dt.Columns.Add("product_name", typeof(string));
                dt.Columns.Add("retail_price", typeof(string));
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void bntSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = DataAccess.GetDataTable("select product_name, product_id, retail_price from purchase where product_id = @id",
                    DataAccess.P("@id", cmbitems.Text));
                if (dt1.Rows.Count == 0)
                {
                    MessageBox.Show("Item not found");
                    return;
                }
                string pname = dt1.Rows[0].ItemArray[0].ToString();
                string price = dt1.Rows[0].ItemArray[2].ToString();
                for (int i = 1; i <= int.Parse(txtQuantity.Text); i++)
                {
                    dt.Rows.Add(pname, cmbitems.Text, price);
                    string aa = Application.StartupPath;
                    aa = aa + "\\BarCode\\NewBarcodeGen.rpt";
                    rd.Load(aa);
                    rd.SetDataSource(dt);
                    crystalReportViewer1.ReportSource = rd;
                }
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
    }
}
