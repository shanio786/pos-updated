using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace supershop.Items
{
    public partial class StockDetails : Form
    {
        public StockDetails()
        {
            InitializeComponent();
            
        }

        private void StockDetails_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = " select  product_id as 'Item Code' , product_name as 'Item Name' , product_quantity as 'Quantity',  " +
                             " cost_price as 'Buy Price' ,((cost_price * product_quantity) * 1.00) as 'Total', retail_price as 'Sales Price' ,   category as 'Category' , supplier as 'Supplier'   " +
                             " from purchase where product_quantity>=0 order by product_quantity";
                DataTable dt1 = DataAccess.GetDataTable(sql);
                datagridItemList.DataSource = dt1;
               lblRow.Text = "Total item :" + datagridItemList.Rows.Count.ToString();
               

                //~ double totalqty = 0;
                //~ for (int i = 0; i < datagridItemList.Rows.Count; ++i)
                //~ {
                //~ totalqty += Convert.ToDouble(datagridItemList.Rows[i].Cells[2].Value);
                //~ }
                //~ lblRow.Text  = totalqty.ToString();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            labeltotal.Text = "0";
            try
            {
                string sql = " select  product_id as 'Item Code' , product_name as 'Item Name' , product_quantity as 'Quantity',  " +
                             " cost_price as 'Buy Price' , retail_price as 'Sales Price' , ((cost_price * product_quantity) * 1.00) as 'Total',  category as 'Category' , supplier as 'Supplier'   " +
                             " from purchase where product_id like  '" + txtSearch.Text + "%' or product_name like '" + txtSearch.Text + "%'";
                DataTable dt1 = DataAccess.GetDataTable(sql);
                datagridItemList.DataSource = dt1;
                lblRow.Text = "Total item :" + datagridItemList.Rows.Count.ToString();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void datagridItemList_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(datagridItemList.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void datagridItemList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            foreach (DataGridViewRow Myrow in datagridItemList.Rows)
            {         
                // if less than equal 0 Red alter 
                if (Convert.ToDouble(Myrow.Cells[2].Value) <= 0) 
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Red;
                    Myrow.DefaultCellStyle.ForeColor = Color.Lime;
                }
                // if less than  10 yellow alarming 
                else if (Convert.ToDouble(Myrow.Cells[2].Value) < 7)
                {
                    Myrow.DefaultCellStyle.BackColor = Color.Yellow;
                    Myrow.DefaultCellStyle.ForeColor = Color.Black;
                }
                else
                {
                    Myrow.DefaultCellStyle.BackColor = Color.White;
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "StockDetails_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in datagridItemList.Columns)
            {
                csv += column.HeaderText + ',';
            }

            //Add new line.
            csv += "\r\n";

            //Adding the Rows
            foreach (DataGridViewRow row in datagridItemList.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //Add the Data rows.
                    csv += cell.Value.ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            //Exporting to CSV.           
            string fileName = "StockDetails_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
            string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location: 
            // Create a new target folder, if necessary. 
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);

            }

            // Get file name.
            string name = saveFileDialog1.FileName;
            File.WriteAllText(name, csv);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            labeltotal.Text = "0";

            for (int i = 0; i < datagridItemList.Rows.Count; i++)
            {
                labeltotal.Text = Convert.ToString(double.Parse(labeltotal.Text) + double.Parse(datagridItemList.Rows[i].Cells[5].Value.ToString()));
            }
        }

        private void txtSearchByCategory_TextChanged(object sender, EventArgs e)
        {
            labeltotal.Text = "0";
            try
            {
                string sql = " select  product_id as 'Item Code' , product_name as 'Item Name' , product_quantity as 'Quantity',  " +
                             " cost_price as 'Buy Price' , retail_price as 'Sales Price' , ((cost_price * product_quantity) * 1.00) as 'Total',  category as 'Category' , supplier as 'Supplier'   " +
                             " from purchase where category like  '" + txtSearchByCategory.Text + "%' or supplier like '" + txtSearchByCategory.Text + "%'";
                DataTable dt1 = DataAccess.GetDataTable(sql);
                datagridItemList.DataSource = dt1;
                lblRow.Text = "Total item :" + datagridItemList.Rows.Count.ToString();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void labeltotal_Click(object sender, EventArgs e)
        {

        }
    }
}
