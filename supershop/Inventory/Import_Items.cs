using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
using System.IO;
using System.Diagnostics;

namespace supershop
{
    public partial class Import_Items : Form
    {
        private string Excel03ConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
       // private string Excel07ConString = "Provider=Microsoft.ACE.OLEDB.8.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
        private string Excel10ConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR={1}'";
       
        public Import_Items()
        {
            InitializeComponent();
            btnSave.Enabled = false;
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            string filePath = openFileDialog1.FileName;
            string extension = Path.GetExtension(filePath);
            string header = rbHeaderYes.Checked ? "YES" : "NO";
            string conStr, sheetName;

            conStr = string.Empty;
            switch (extension)
            {

                case ".xls": //Excel 97-03
                    conStr = string.Format(Excel03ConString, filePath, header);
                    break;

                case ".xlsx": //Excel 07
                    conStr = string.Format(Excel10ConString, filePath, header);
                    break;

                case ".csv": //Excel 07
                    conStr = string.Format(Excel10ConString, filePath, header);
                    break;
            }

            //Get the name of the First Sheet.
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    cmd.Connection = con;
                    con.Open();
                    DataTable dtExcelSchema = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                    con.Close();
                }
            }

            //Read Data from the First Sheet.
            using (OleDbConnection con = new OleDbConnection(conStr))
            {
                using (OleDbCommand cmd = new OleDbCommand())
                {
                    using (OleDbDataAdapter oda = new OleDbDataAdapter())
                    {
                        DataTable dt = new DataTable();
                        cmd.CommandText = "SELECT * From [" + sheetName + "]";
                        cmd.Connection = con;
                        con.Open();
                        oda.SelectCommand = cmd;
                        oda.Fill(dt);
                        con.Close();

                        //Populate DataGridView.
                        dtgridviewImportPreview.DataSource = dt;
                        btnSave.Enabled = true;
                    }
                }
            }
        }

        private void btnImportPreview_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            lblRows.Text = "Total ID = " + dtgridviewImportPreview.RowCount.ToString();
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblwaiting.Text = "Please Wait ...";
            try
            {
                
                int rows = dtgridviewImportPreview.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    string product_id           = dtgridviewImportPreview.Rows[i].Cells[0].Value.ToString();
                    string product_name         = dtgridviewImportPreview.Rows[i].Cells[1].Value.ToString();
                    double product_quantity     = Convert.ToDouble(dtgridviewImportPreview.Rows[i].Cells[2].Value.ToString());
                    double cost_price           = Convert.ToDouble(dtgridviewImportPreview.Rows[i].Cells[3].Value.ToString());
                    double retail_price         = Convert.ToDouble(dtgridviewImportPreview.Rows[i].Cells[4].Value.ToString());
                    double total_cost_price     = cost_price * product_quantity;
                    double total_retail_price   = retail_price * product_quantity;
                    string category             = dtgridviewImportPreview.Rows[i].Cells[5].Value.ToString();
                    string supplier             = dtgridviewImportPreview.Rows[i].Cells[6].Value.ToString();
                    string imagename            = product_id + ".png"; // dtgridviewImportPreview.Rows[i].Cells[7].Value.ToString();
                    double discount             = Convert.ToDouble(dtgridviewImportPreview.Rows[i].Cells[7].Value.ToString());
                    int taxapply                = Convert.ToInt32(dtgridviewImportPreview.Rows[i].Cells[8].Value.ToString());
                    string Shopid               = dtgridviewImportPreview.Rows[i].Cells[9].Value.ToString();
                    int kitchendisplay          = Convert.ToInt32(dtgridviewImportPreview.Rows[i].Cells[10].Value.ToString()); 
                 

                    string sqlCmd = " insert into purchase (product_id , product_name , product_quantity , cost_price , retail_price , total_cost_price , " +
                                    " total_retail_price , category , supplier , imagename, discount, taxapply, Shopid, status ) " +
                                    "  values ('" + product_id + "' , '" + product_name + "', '" + product_quantity + "', '" + cost_price + "' , '" + retail_price + "' , " +
                                    "  '" + total_cost_price + "' , '" + total_retail_price + "' , '" + category + "', '" + supplier + "', '" + imagename + "' , " +
                                    " '" + discount + "' , '" + taxapply + "' , '" + Shopid + "', '" + kitchendisplay + "')";
                    DataAccess.ExecuteSQL(sqlCmd);

                    //Same time Purchase history insert
                    insertpurchasehistory(product_id, product_name, product_quantity, cost_price, retail_price, category, supplier, Shopid);
        

                    //Serial image upload
                    string path = Application.StartupPath + @"\ITEMIMAGE\";
                    string filename = path + @"\" + picItemimage.Image;
                    picItemimage.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                    System.IO.File.Move(path + @"\" + picItemimage.Image , path + @"\" + imagename);

                  ///  MessageBox.Show("Successfully Added ");
                    btnSave.Enabled = false;
                    lblmsg.Text = "Successfully Added Bulk items and purchase history record";
                    lblwaiting.Visible = false;
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Sorry\r\n this id already added \n Duplicate value \n  " + exp.Message);
            }
             

        }

        public void insertpurchasehistory(string pid, string pname, double pQty, double cprice, double sprice, string category, string supplier, string shopid)
        {
            string pdate =   DateTime.Now.ToString("yyyy-MM-dd");
            string sql1 = " insert into tbl_purchase_history (product_id, product_name, product_quantity, cost_price, retail_price, category, " +
                            " supplier, purchase_date, Shopid, ptype ) " +
                            " values ('" + pid + "', '" + pname + "', '" + pQty + "', '" + cprice + "', '" + sprice + "', '" + category + "', " +
                            "  '" + supplier + "', '" + pdate + "' ,'" + shopid + "', 'NEW' )";
            DataAccess.ExecuteSQL(sql1);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // System.Diagnostics.Process.Start("Calc");
               // SendKeys.SendWait(lblTotal.Text);
                Process p = new Process();
                p.StartInfo.FileName = "items.xls";
                p.Start();
                p.WaitForInputIdle();

            }
            catch
            {
            }
        }
    }
}
