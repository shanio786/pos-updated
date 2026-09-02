using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
 

namespace supershop
{ 
    public partial class Stock_List : Form
    {
        public Stock_List()
        {
            InitializeComponent();
        }

        #region Data bind
        //Show Products image
        public void ItemList_with_images(string value, ListView _lst_items)
        {
            //flowLayoutPanelItemList.Controls.Clear();
            string img_directory = Application.StartupPath + @"\ITEMIMAGE\";
            string[] files = Directory.GetFiles(img_directory, "*.png *.jpg");
            try
            {
                string sql = " select  *  from  vw_itemdisplay_sr    where  ( product_name like '" + value + "%') " +
                " OR ( product_id like '" + value + "%' ) " +
                " OR (category like '" + value + "%' )  ";
                DataTable dm = DataAccess.GetDataTable(sql);
                lblRows.Text =  "Total Rows " + dm.Rows.Count.ToString() + " Found";  

                DataTable dt = DataAccess.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    _lst_items.Items.Clear();

                    foreach (DataRow dr in dt.Rows)
                    {
                        ListViewItem lst = new ListViewItem(dr["product_id"].ToString());
                        {
                            if (dr["taxapply"].ToString() == "1")
                            {
                                lst.SubItems.Add("YES");
                            }
                            else
                            {
                                lst.SubItems.Add("NO");
                            }

                            lst.SubItems.Add(dr["product_name"].ToString());
                            lst.SubItems.Add(dr["product_quantity"].ToString());
                            lst.SubItems.Add(dr["cost_price"].ToString());
                            lst.SubItems.Add(dr["retail_price"].ToString());
                            lst.SubItems.Add(dr["discount"].ToString());
                            lst.SubItems.Add(dr["category"].ToString());
                            lst.SubItems.Add(dr["supplier"].ToString());

                        }
                        _lst_items.Items.Add(lst);

                    }

                    if (_lst_items.Items.Count > 0)
                    {
                        _lst_items.Visible = true;
                    }
                    else
                    {
                        _lst_items.Visible = false;

                    }
                }
                else
                {
                    _lst_items.Visible = false;
                    _lst_items.Items.Clear();
                }
                //int currentImage = 0;

                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    DataRow dataReader = dt.Rows[i];

                //    Button b = new Button();
                //    //Image i = Image.FromFile(img_directory + dataReader["name"]);
                //    b.Tag = dataReader["product_id"];
                //    b.Click += new EventHandler(b_Click);

                //    string taxapply;
                //    if (dataReader["taxapply"].ToString() == "1")
                //    {
                //        taxapply = "YES";
                //    }
                //    else
                //    {
                //        taxapply = "NO";
                //    }

                //    string details = dataReader["product_id"] +
                //     "\n Name: " + dataReader["product_name"].ToString() +
                //  //   "\n Buy price: " + dataReader["cost_price"].ToString() +
                //     "\n Stock Qty: " + dataReader["product_quantity"].ToString() +
                //     "\n Retail price: " + dataReader["retail_price"].ToString() +
                //     "\n Discount: " + dataReader["discount"].ToString() +
                //     "\n Category: " + dataReader["category"].ToString() +
                //     "\n Supplier: " + dataReader["supplier"].ToString() +
                //   //  "\n Branch: "  + dataReader["Shopid"].ToString() +
                //     "\n Tax Apply: " + taxapply;
                //    b.Name = details;                 
                //     toolTip2.ToolTipTitle = "Item Details";  // If you want to Show tooltip please uncomment
                //     toolTip2.SetToolTip(b, details);          //Umncomment

                //    ImageList il = new ImageList();
                //    il.ColorDepth = ColorDepth.Depth32Bit;
                //    il.TransparentColor = Color.Transparent;
                //    il.ImageSize = new Size(55, 45);
                //    il.Images.Add(Image.FromFile(img_directory + dataReader["imagename"]));


                //    b.Image = il.Images[0];
                //    b.Margin = new Padding(3, 3, 3, 3);

                //    b.Size = new Size(87, 60);
                //    b.Text.PadRight(4);

                //  //  b.Text += " " + dataReader["product_id"] + "\n ";
                //  //   b.Text +=  dataReader["product_name"].ToString();
                //  //  b.Text += "\n Buy: " + dataReader["cost_price"];
                ////    b.Text += "\n Stock: " + dataReader["product_quantity"];
                //  //  b.Text += "\n R.Price: " + dataReader["retail_price"];
                ////    b.Text += "\n Dis: " + dataReader["discount"] + "% ";   //"Tax: " + taxapply;

                //    b.Font = new Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point);
                //    b.TextAlign = ContentAlignment.TopLeft;
                //    b.TextImageRelation = TextImageRelation.ImageAboveText;
                //  //  b.FlatStyle = FlatStyle.Flat;
                //    b.FlatAppearance.BorderSize = 0;
                //    flowLayoutPanelItemList.Controls.Add(b);
                //    currentImage++;

                //}
            }
            catch //(Exception)
            {

                //throw;
            }
        }
        //Go to Item Details page
        protected void b_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string s;
            s = b.Tag.ToString();

            this.Hide();
            Add_Item go = new Add_Item();
            go.itemCode = s;
            go.MdiParent = this.ParentForm;
            go.Show();
     

        }

        //Product filter by Category
        private void combCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemList_with_images(combCategory.Text, lst_items);
        }

        //Product filter by Product Name or Product ID
        private void txtItemSearchBar_TextChanged(object sender, EventArgs e)
        {
         
             ItemList_with_images(txtItemSearchBar.Text, lst_items);
            
        } 
 
        private void detail_info_Load(object sender, EventArgs e)
        {         
            
            try
            {
                //Product Category
                string sql5 = "select   DISTINCT  category   from purchase ";
                DataTable dt5 = DataAccess.GetDataTable(sql5);
                combCategory.DataSource = dt5;
                combCategory.DisplayMember = "category";

                //ItemList_with_images("");
                
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        #endregion

        #region page links
        private void btnCreateBarcode_Click(object sender, EventArgs e)
        { 

            BarCode.Barcode_machine go = new BarCode.Barcode_machine();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            Chart g = new Chart();
            g.MdiParent = this.ParentForm;
            g.Show();
        }

        private void picCloseEvent_Click(object sender, EventArgs e)
        {
            this.Close();
        }
            
        private void btnImport_Click(object sender, EventArgs e)
        {
            this.Hide();
            Import_Items go = new Import_Items();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            this.Hide();
            Add_Item go = new Add_Item();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void bntStock_Click(object sender, EventArgs e)
        {
            this.Hide();
            Items.StockDetails go = new Items.StockDetails();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void btnpurchasehistory_Click(object sender, EventArgs e)
        {
            this.Hide();
            Items.Purchase_History go = new Items.Purchase_History();
            go.MdiParent = this.ParentForm;
            go.Show();
        }
        #endregion


        // toolbar 
        private void lblMinimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;   //Minimized              
        }

        private void detail_info_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void txtItemSearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (lst_items.Items.Count > 0)
                {
                    lst_items.Focus();
                    lst_items.Items[0].Selected = true;
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (lst_items.Items[0].Selected == true)
                {
                    txtItemSearchBar.Focus();
                    txtItemSearchBar.SelectAll();
                }
            }
        }

        private void lst_items_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                txtItemSearchBar.Text = lst_items.SelectedItems[0].SubItems[0].Text;
            // SetDiscount();
        }

        private void lst_items_Click(object sender, EventArgs e)
        {
            //Button b = sender as Button;
            string s;
            s = lst_items.SelectedItems[0].SubItems[0].Text;

            this.Hide();
            Add_Item go = new Add_Item();
            go.itemCode = s;
            go.MdiParent = this.ParentForm;
            go.Show();
        }
    }
}
