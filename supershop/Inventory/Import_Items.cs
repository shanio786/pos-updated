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

                case ".csv":
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

        // One parsed spreadsheet row
        private class ImportRow
        {
            public string ProductId, ProductName, Category, Supplier, ImageName, ShopId;
            public decimal Quantity, CostPrice, RetailPrice, Discount;
            public int TaxApply, KitchenDisplay;
        }

        private static string CellText(DataGridViewRow row, int index)
        {
            object v = row.Cells[index].Value;
            return v == null ? "" : v.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            lblwaiting.Text = "Please Wait ...";
            try
            {
                // Parse every row first so a bad cell fails before anything is written
                List<ImportRow> items = new List<ImportRow>();
                foreach (DataGridViewRow gr in dtgridviewImportPreview.Rows)
                {
                    if (gr.IsNewRow) continue;
                    ImportRow r = new ImportRow();
                    r.ProductId      = CellText(gr, 0);
                    r.ProductName    = CellText(gr, 1);
                    r.Quantity       = Convert.ToDecimal(CellText(gr, 2));
                    r.CostPrice      = Convert.ToDecimal(CellText(gr, 3));
                    r.RetailPrice    = Convert.ToDecimal(CellText(gr, 4));
                    r.Category       = CellText(gr, 5);
                    r.Supplier       = CellText(gr, 6);
                    r.ImageName      = r.ProductId + ".png";
                    r.Discount       = Convert.ToDecimal(CellText(gr, 7));
                    r.TaxApply       = Convert.ToInt32(CellText(gr, 8));
                    r.ShopId         = CellText(gr, 9);
                    r.KitchenDisplay = Convert.ToInt32(CellText(gr, 10));
                    items.Add(r);
                }

                string pdate = DateTime.Now.ToString("yyyy-MM-dd");

                // Whole import is one unit of work: a duplicate id anywhere rolls back every row
                DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                {
                    foreach (ImportRow r in items)
                    {
                        tx.Execute(" insert into purchase (product_id, product_name, product_quantity, cost_price, retail_price, total_cost_price, " +
                                   " total_retail_price, category, supplier, imagename, discount, taxapply, Shopid, status) " +
                                   " values (@pid, @pname, @qty, @cprice, @sprice, @ctotal, @rtotal, @category, @supplier, @image, " +
                                   " @discount, @taxapply, @shopid, @status)",
                            DataAccess.P("@pid", r.ProductId),
                            DataAccess.P("@pname", r.ProductName),
                            DataAccess.P("@qty", r.Quantity),
                            DataAccess.P("@cprice", r.CostPrice),
                            DataAccess.P("@sprice", r.RetailPrice),
                            DataAccess.P("@ctotal", r.CostPrice * r.Quantity),
                            DataAccess.P("@rtotal", r.RetailPrice * r.Quantity),
                            DataAccess.P("@category", r.Category),
                            DataAccess.P("@supplier", r.Supplier),
                            DataAccess.P("@image", r.ImageName),
                            DataAccess.P("@discount", r.Discount),
                            DataAccess.P("@taxapply", r.TaxApply),
                            DataAccess.P("@shopid", r.ShopId),
                            DataAccess.P("@status", r.KitchenDisplay));

                        //Same time Purchase history insert
                        tx.Execute(" insert into tbl_purchase_history (product_id, product_name, product_quantity, cost_price, retail_price, category, " +
                                   " supplier, purchase_date, Shopid, ptype) " +
                                   " values (@pid, @pname, @qty, @cprice, @sprice, @category, @supplier, @pdate, @shopid, 'NEW')",
                            DataAccess.P("@pid", r.ProductId),
                            DataAccess.P("@pname", r.ProductName),
                            DataAccess.P("@qty", r.Quantity),
                            DataAccess.P("@cprice", r.CostPrice),
                            DataAccess.P("@sprice", r.RetailPrice),
                            DataAccess.P("@category", r.Category),
                            DataAccess.P("@supplier", r.Supplier),
                            DataAccess.P("@pdate", pdate),
                            DataAccess.P("@shopid", r.ShopId));
                    }
                });

                // Placeholder image for each imported item; a file problem must not undo the committed import
                try
                {
                    string path = Application.StartupPath + @"\ITEMIMAGE\";
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                    foreach (ImportRow r in items)
                    {
                        if (picItemimage.Image != null && !File.Exists(path + r.ImageName))
                            picItemimage.Image.Save(path + r.ImageName, System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
                catch (Exception exImg) { Logger.Error(exImg); }

                btnSave.Enabled = false;
                lblmsg.Text = "Successfully Added Bulk items and purchase history record";
                lblwaiting.Visible = false;
            }
            catch (Exception exp)
            {
                Logger.Show(exp, "Sorry\r\n this id already added \n Duplicate value");
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = "items.xls";
                p.Start();
                p.WaitForInputIdle();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
    }
}
