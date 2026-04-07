using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;

namespace supershop.Inventory
{
    public partial class Add_Sales : Form
    {
        string saleType = "";

        /*
            Author :    Mohammad Tuaha
            Email:      citkar@live.Com
         * Advance POS 
         * http://codecanyon.net/item/advance-point-of-sale-system-pos/6317175
         * 
        */

        public Add_Sales()
        {
            InitializeComponent();
            lblUserID.Text = UserInfo.UserName;
           
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public static class itemid
        {
            public static string itemcode;
        }
        

        #region Databind  /////////////////////////////////////////
        public void customerComboboxDataBind()
        {
            string sqlCmd = "select DISTINCT Name   from tbl_customer where peopleType = 'Customer'";
            DataAccess.ExecuteSQL(sqlCmd);
            DataTable dt5 = DataAccess.GetDataTable(sqlCmd);
            CmbCustomer.DataSource = dt5;
            CmbCustomer.DisplayMember = "Name";
            CmbCustomer.Text = "Guest";

         //   CmbBiller.DataSource = dt5;
          //  CmbBiller.DisplayMember = "Name";            
        }

        public void BillerComboBoxDataBind()
        {
            string sqlCmd = "select DISTINCT Name   from tbl_customer where peopleType = 'Biller'";
            DataAccess.ExecuteSQL(sqlCmd);
            DataTable dt5 = DataAccess.GetDataTable(sqlCmd);
            CmbBiller.DataSource = dt5;
            CmbBiller.DisplayMember = "Name";
        }


        //Page Load
        private void Add_Sales_Load(object sender, EventArgs e)
        {
            try
            {
                customerComboboxDataBind();
                BillerComboBoxDataBind();

                ItemList_with_images(txtSearchItem.Text,lst_items);

                txtVATRate.Text = vatdisvalue.vat;
            //    txtDiscountRate.Text = vatdisvalue.dis;

                dtSalesDate.Format = DateTimePickerFormat.Custom;
                dtSalesDate.CustomFormat = "yyyy-MM-dd";

                this.dgrvSalesItemList.Columns.Add("itm", "Items Name");
                this.dgrvSalesItemList.Columns.Add("Am", "Price");
                this.dgrvSalesItemList.Columns.Add("Qty", "Qty");
                this.dgrvSalesItemList.Columns.Add("Total", "Total");
                this.dgrvSalesItemList.Columns.Add("ID", "ID");
                this.dgrvSalesItemList.Columns.Add("disamt", "Disamt");     // 5. new in 8.1 version
                this.dgrvSalesItemList.Columns.Add("taxamt", "taxamt");     // 6. new in 8.1 version
                this.dgrvSalesItemList.Columns.Add("dis", "Dis");           // 7. new in 8.1 version
                this.dgrvSalesItemList.Columns.Add("taxapply", "Tax");      // 8. new in 8.1 version

                ////Hide fields
                dgrvSalesItemList.Columns[4].Visible = false; // ID             // new in 8.1 version
                dgrvSalesItemList.Columns[5].Visible = false; // Disamt         // new in 8.1 version
                dgrvSalesItemList.Columns[6].Visible = false; // taxamt         // new in 8.1 version
                dgrvSalesItemList.Columns[7].Visible = false; // Discount rate  // new in 8.1 version

                //Font size of columns and aligmnet  // add in from version 8.3
                dgrvSalesItemList.Columns["itm"].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgrvSalesItemList.Columns["taxapply"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                 
                //Add  + button 
                DataGridViewButtonColumn inc = new DataGridViewButtonColumn();
                dgrvSalesItemList.Columns.Add(inc);
                inc.HeaderText = "Increase";
                inc.Text = "+";
                inc.ToolTipText = "Increase Item Qty";
                inc.Name = "inc";
                inc.UseColumnTextForButtonValue = true;

                //Add X Button
                DataGridViewButtonColumn del = new DataGridViewButtonColumn();
                dgrvSalesItemList.Columns.Add(del);
                del.HeaderText = "Del";
                del.Text = "X";
                del.ToolTipText = "Delete this Item";
                del.Name = "del";
                del.UseColumnTextForButtonValue = true;

               // this.dgrvSalesItemList.Rows[0].Cells[2].Value = "1";
                dgrvSalesItemList.Columns[0].ReadOnly = true;
                dgrvSalesItemList.Columns[1].ReadOnly = true;
                dgrvSalesItemList.Columns[2].ReadOnly = false;
                dgrvSalesItemList.Columns[3].ReadOnly = true;
                dgrvSalesItemList.Columns[4].ReadOnly = true;
                dgrvSalesItemList.Columns[5].ReadOnly = true;
                dgrvSalesItemList.Columns[6].ReadOnly = true;
                dgrvSalesItemList.Columns[7].ReadOnly = true;
                dgrvSalesItemList.Columns[8].ReadOnly = true;

                //Qty column row color
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.ForeColor = Color.Black;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.BackColor = Color.Silver;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.SelectionForeColor = Color.Black;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.SelectionBackColor = Color.Silver;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);

                // Item name Column width 
                DataGridViewColumn ColName = dgrvSalesItemList.Columns[0];
                ColName.Width = 151; ;

                //Load Invoice No / Receipt No for current transaction
                string sql = "select sales_id from sales_payment order by sales_id desc";
                DataTable dt = DataAccess.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    double id = Convert.ToDouble(dt.Rows[0].ItemArray[0].ToString()) + 1;
                    txtinvoiceNo.Text = Convert.ToString(id);
                }
                else
                {
                    double id = 1;                     
                    txtinvoiceNo.Text = Convert.ToString(id); 
                }
                txtBarcodeReaderBox.Text = UserInfo.invoiceNo;        
            }
            catch
            {
            }
            
        }

        //Show Products image
        public void ItemList_with_images(string value, ListView _lst_items)
        {
            //flowLayoutPanelItemList.Controls.Clear();
            string img_directory = Application.StartupPath + @"\ITEMIMAGE\";
            string[] files = Directory.GetFiles(img_directory, "*.png *.jpg");
            try
            {
                string sql = " select  *  from  vw_itemdisplay_sr    where  ( product_name like '" + value + "%' and product_quantity >= 1) " +
                " OR ( product_id like '" + value + "%'  and product_quantity >= 1) " +
                " OR (category like '" + value + "%' and   product_quantity >= 1)  ";
                //" ORDER BY RANDOM() LIMIT 12 "; // Sqlite  //View vw_itemdisplay_sr   purchase
                // " ORDER BY RAND() LIMIT 12 "; // MySQL
                //  " ORDER BY NEWID() "; // SQL server and use -- top 12 after select  

                DataAccess.ExecuteSQL(sql);
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
                            lst.SubItems.Add(dr["retail_price"].ToString());
                            lst.SubItems.Add(dr["discount"].ToString());
                            lst.SubItems.Add(dr["category"].ToString());
                            lst.SubItems.Add(dr["supplier"].ToString());

                        }
                        _lst_items.Items.Add(lst);
                        //int y = dgv_items.Rows.Add();
                        //dgv_items.Rows[y].Cells[0].Value = dr["product_id"].ToString();
                        //if (dr["taxapply"].ToString() == "1")
                        //{
                        //    dgv_items.Rows[y].Cells[1].Value = "YES";
                        //}
                        //else
                        //{
                        //    dgv_items.Rows[y].Cells[1].Value = "NO";
                        //}

                        //dgv_items.Rows[y].Cells[2].Value = dr["product_name"].ToString();
                        //dgv_items.Rows[y].Cells[3].Value = dr["product_quantity"].ToString();
                        //dgv_items.Rows[y].Cells[4].Value = dr["retail_price"].ToString();
                        //dgv_items.Rows[y].Cells[5].Value = dr["discount"].ToString();
                        //dgv_items.Rows[y].Cells[6].Value = dr["category"].ToString();
                        //dgv_items.Rows[y].Cells[7].Value = dr["supplier"].ToString();



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


        //Click add to cart
        protected void b_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string s;
            s = " ID: ";
            s += b.Tag;
            s += "\n Name: ";
            s += b.Name.ToString();

            txtBarcodeReaderBox.Text = b.Tag.ToString();       

        }

        //Barcode Reader
        private void txtBarcodeReaderBox_TextChanged(object sender, EventArgs e)
        {
            if (txtBarcodeReaderBox.Text == "")
            {
                //  MessageBox.Show("Please Insert Product id : ");
                //textBox1.Focus();
            }
            else
            {
                try
                {
                    // Default tax rate 
                    double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                    //- new in 8.1 version // Default Product QTY is 1
                    string sql = "SELECT  product_name as Name , retail_price as Price , 1.00  as QTY, (retail_price * 1.00 ) * 1.00  as 'Total' ,  " +
                            " (((retail_price * 1.00 ) * discount) / 100.00) as 'dis amt' , " +
                            " CASE     " +
                            " WHEN taxapply = 1 THEN   (((retail_price * 1.00 )  - (((retail_price * 1.00 ) * discount) / 100.00))  * " + Taxrate + " ) / 100.00   " +
                            " ELSE '0.00'  " +
                            " END 'tax amt' , product_id as ID , discount , taxapply " +
                            " FROM  purchase  where product_id = '" + txtBarcodeReaderBox.Text + "'  and product_quantity >= 1 ";
                    DataAccess.ExecuteSQL(sql);
                    DataTable dt = DataAccess.GetDataTable(sql);

                    string ItemsName = dt.Rows[0].ItemArray[0].ToString();
                    double Rprice = Convert.ToDouble(dt.Rows[0].ItemArray[1].ToString());
                    double Qty = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                    double Total = Convert.ToDouble(dt.Rows[0].ItemArray[3].ToString()) * Qty;
                    string Itemid = dt.Rows[0].ItemArray[6].ToString();
                    double Disamt = Convert.ToDouble(dt.Rows[0].ItemArray[4].ToString());       // Total Discount amount of this item
                    double Taxamt = Convert.ToDouble(dt.Rows[0].ItemArray[5].ToString());       // Total Tax amount  of this item
                    double Dis = Convert.ToDouble(dt.Rows[0].ItemArray[7].ToString());          // Discount Rate
                    double Taxapply = Convert.ToDouble(dt.Rows[0].ItemArray[8].ToString());     //  VAT/TAX/TPS/TVQ apply or not

                    //Add to Item list
                   // dgrvSalesItemList.Rows.Add(ItemsName, Rprice, Qty, Rprice, Itemid, Disamt, Taxamt, Dis, Taxapply);

                    //Add to Item list             
                    int n = Finditem(ItemsName);
                    if (n == -1)  //If new item
                    {
                        dgrvSalesItemList.Rows.Add(ItemsName, Rprice, Qty, Rprice, Itemid, Disamt, Taxamt, Dis, Taxapply);
                         
                    }
                    else  // if same item Quantity increase by 1 
                    {
                        int QtyInc = Convert.ToInt32(dgrvSalesItemList.Rows[n].Cells[2].Value);
                        dgrvSalesItemList.Rows[n].Cells[2].Value = (QtyInc + 1);  //Qty Increase
                        dgrvSalesItemList.Rows[n].Cells[3].Value = Rprice * (QtyInc + 1);   // Total price                                             

                        double qty = Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[2].Value);
                        double disrate = Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[7].Value);

                        if (disrate != 0)  // if discount has
                        {
                            double DisamtInc = (((Rprice * qty) * disrate) / 100.00);      // Total Discount amount of this item
                            dgrvSalesItemList.Rows[n].Cells[5].Value = DisamtInc;
                        }

                        if (Taxapply != 0)   // If apply  tax 
                        {
                            // Total Tax amount  of this item  (Rprice - disamount) * taxRate / 100
                            double TaxamtInc = ((((Rprice * qty) - (((Rprice * qty) * disrate) / 100.00)) * Taxrate) / 100.00);
                            dgrvSalesItemList.Rows[n].Cells[6].Value = TaxamtInc;
                        }

                        //dgrvSalesItemList.Rows[n].Cells[0].Value = ItemsName;
                        //dgrvSalesItemList.Rows[n].Cells[1].Value = Rprice;
                        //int QtyInc = Convert.ToInt32(dgrvSalesItemList.Rows[n].Cells[2].Value);
                        //dgrvSalesItemList.Rows[n].Cells[2].Value = (QtyInc + 1);  //Qty
                        //dgrvSalesItemList.Rows[n].Cells[3].Value = Rprice * (QtyInc + 1);   // Total price
                        //dgrvSalesItemList.Rows[n].Cells[4].Value = Itemid;
                        //dgrvSalesItemList.Rows[n].Cells[5].Value = Disamt;
                        //dgrvSalesItemList.Rows[n].Cells[6].Value = Taxamt;
                        //dgrvSalesItemList.Rows[n].Cells[7].Value = Dis;
                        //dgrvSalesItemList.Rows[n].Cells[8].Value = Taxapply;
                    }

                    itemid.itemcode = Itemid;
                    
                    // ClearForm();
                    txtBarcodeReaderBox.Text = "";
                    txtBarcodeReaderBox.Focus();

                    DiscountCalculation();
                    vatcal();
                    txtDiscountRate.Text = "0";


                    Properties.Settings.Default["SomeProperty"] = "Some Value";
                    Properties.Settings.Default.Save();  

                }

                catch
                {
                    //MessageBox.Show("sorry");
                }
            }
        }

        // Check duplicate item 
        public int Finditem(string item)
        {
            int k = -1;
            if (dgrvSalesItemList.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgrvSalesItemList.Rows)
                {
                    if (row.Cells[0].Value.ToString().Equals(item))
                    {
                        k = row.Index;
                        break;
                    }
                }
            }
            return k;
        }

        private void txtBarcodeReaderBox_Click(object sender, EventArgs e)
        {
            txtBarcodeReaderBox.Text = Clipboard.GetText();
        }

        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            ItemList_with_images(txtSearchItem.Text, lst_items);
        }

        #endregion

        //Item increase and decrease
        private void dgrvSalesItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {                 

                // Delete items From Gridview
                if (e.ColumnIndex == dgrvSalesItemList.Columns["del"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow row2 in dgrvSalesItemList.SelectedRows)
                    {
                       // DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        //if (result == DialogResult.Yes)
                        //{
                            if (!row2.IsNewRow)
                                dgrvSalesItemList.Rows.Remove(row2);
                            DiscountCalculation();
                            vatcal();
                            txtDiscountRate.Text = "0";
                      //  }

                    }
               }

                // Increase Item Quantity
                if (e.ColumnIndex == dgrvSalesItemList.Columns["inc"].Index && e.RowIndex >= 0)
                {

                    foreach (DataGridViewRow row in dgrvSalesItemList.SelectedRows)
                    {

                        //// Increase by 1
                        double qtySum = Convert.ToDouble(row.Cells[2].Value) + 1;
                        row.Cells[2].Value = qtySum;

                        double qty = Convert.ToDouble(row.Cells[2].Value);
                        double Rprice = Convert.ToDouble(row.Cells[1].Value);
                        double disrate = Convert.ToDouble(row.Cells[7].Value);
                        double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                        //// show total price   Qty  * Rprice
                        double totalPrice = qty * Rprice;
                        row.Cells[3].Value = totalPrice;

                        if (Convert.ToDouble(row.Cells[7].Value) != 0)
                        {
                            double Disamt = (((Rprice * qty) * disrate) / 100.00);      // Total Discount amount of this item
                            row.Cells[5].Value = Disamt;
                        }

                        if (Convert.ToDouble(row.Cells[8].Value) != 0)
                        {
                            double Taxamt = ((((Rprice * qty) - (((Rprice * qty) * disrate) / 100.00)) * Taxrate) / 100.00); // Total Tax amount  of this item
                            row.Cells[6].Value = Taxamt;
                        }                   

                        DiscountCalculation();
                        vatcal();
                        txtDiscountRate.Text = "0";

                    }
                }

            }
            catch //(Exception exp)
            {
                // MessageBox.Show("Sorry" + exp.Message);
            }
        }

        //Input Item Quantity
        private void dgrvSalesItemList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Increase Item Quantity with Edited cell
                if (e.ColumnIndex == dgrvSalesItemList.Columns["Qty"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow row in dgrvSalesItemList.SelectedRows)
                    {
                        double qty = Convert.ToDouble(row.Cells[2].Value);
                        double Rprice = Convert.ToDouble(row.Cells[1].Value);
                        double disrate = Convert.ToDouble(row.Cells[7].Value);
                        double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                        //// show total price   Qty  * Rprice
                        double totalPrice = qty * Rprice;
                        row.Cells[3].Value = totalPrice;

                        if (Convert.ToDouble(row.Cells[7].Value) != 0)
                        {
                            double Disamt = (((Rprice * qty) * disrate) / 100.00);      // Total Discount amount of this item
                            row.Cells[5].Value = Disamt;
                        }

                        if (Convert.ToDouble(row.Cells[8].Value) != 0)
                        {
                            double Taxamt = ((((Rprice * qty) - (((Rprice * qty) * disrate) / 100.00)) * Taxrate) / 100.00); // Total Tax amount  of this item
                            row.Cells[6].Value = Taxamt;
                        }



                        DiscountCalculation();
                        vatcal();
                        txtDiscountRate.Text = "0";

                    }
                }
            }
            catch
            {
            }

        }

        // Discount Calculation - Change in 8.1 version
        public void DiscountCalculation()
        {
            // // subtotal without dis vat sum 
            double totalsum = 0.00;
            for (int i = 0; i < dgrvSalesItemList.Rows.Count; ++i)
            {
                totalsum += Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[3].Value);
            }
            lblTotal.Text = Math.Round(totalsum, 2).ToString();
            lblTotalItems.Text = dgrvSalesItemList.RowCount.ToString();

            ////    Discount amount sum
            double total = Convert.ToDouble(totalsum.ToString());
            double DisCount = 0.00;
            for (int i = 0; i < dgrvSalesItemList.Rows.Count; ++i)
            {
                DisCount += Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[5].Value);
            }

            DisCount = Math.Round(DisCount, 2);
            double sum = total - DisCount;
            sum = Math.Round(sum, 2);
            lblsubtotal.Text = sum.ToString();

            double payable = sum + Convert.ToDouble(lblTotalVAT.Text) + Convert.ToDouble(txtShippingFee.Text);
            payable = Math.Round(payable, 2) + Convert.ToDouble(txtShippingFee.Text);  
            lblTotalPayable.Text = payable.ToString();

            lblTotalDisCount.Text = DisCount.ToString();
            lbloveralldiscount.Text = DisCount.ToString();

        }

        //VAT amount sum calculation - Change in 8.1 version
        public void vatcal()
        {            
            //Subtotal = total - discount
            double Subtotal = Convert.ToDouble(lblsubtotal.Text);
           
            double VAT = 0.00;
            for (int i = 0; i < dgrvSalesItemList.Rows.Count; ++i)
            {
                VAT += Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[6].Value);
            }

            VAT = Math.Round(VAT, 2);
            lblTotalVAT.Text = VAT.ToString();

            double payable = Subtotal + VAT;
            payable = Math.Round(payable, 2) + Convert.ToDouble(txtShippingFee.Text);
            lblTotalPayable.Text = payable.ToString();
        }
        

        // Tax and discount increase / decrease   ---------------  Start ------------
        private void btnDecreaseDiscount_Click(object sender, EventArgs e)
        {
            if (lblTotal.Text == "0")
            {
                MessageBox.Show("Please Add at least One Item");
            }
            else
            {
                double Discountvalue = Convert.ToDouble(txtDiscountRate.Text) - 1;
                txtDiscountRate.Text = Discountvalue.ToString();
                double subtotal = Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(lblTotalDisCount.Text); // total - item discount  100 - 5 = 95        
                double totaldiscount = (subtotal * Discountvalue) / 100;  //Counter discount  // 95 * 5 /100 = 4.75  
                double disPlusOverallDiscount = totaldiscount + Convert.ToDouble(lblTotalDisCount.Text); // 4.75 + 5 = 9.75
                disPlusOverallDiscount = Math.Round(disPlusOverallDiscount, 2);
                lbloveralldiscount.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75

                double subtotalafteroveralldiscount = subtotal - totaldiscount; // 95 - 4.75 = 90.25
                subtotalafteroveralldiscount = Math.Round(subtotalafteroveralldiscount, 2);
                lblsubtotal.Text = subtotalafteroveralldiscount.ToString();



                double payable = subtotalafteroveralldiscount + Convert.ToDouble(lblTotalVAT.Text);
                payable = Math.Round(payable, 2) + Convert.ToDouble(txtShippingFee.Text);  
                lblTotalPayable.Text = payable.ToString();
            } 
        }

        private void btnIncreaseDisCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblTotal.Text == "")
                {
                    MessageBox.Show("Please Add at least One Item");
                }
                else
                {
                    double Discountvalue = Convert.ToDouble(txtDiscountRate.Text) ;
                    txtDiscountRate.Text = Discountvalue.ToString();
                    double subtotal = Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(lblTotalDisCount.Text); // total - item discount  100 - 5 = 95        
                    double totaldiscount = (subtotal * Discountvalue) / 100;  //Counter discount  // 95 * 5 /100 = 4.75  
                    double disPlusOverallDiscount = totaldiscount + Convert.ToDouble(lblTotalDisCount.Text); // 4.75 + 5 = 9.75
                    disPlusOverallDiscount = Math.Round(disPlusOverallDiscount, 2);
                    lbloveralldiscount.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75

                    double subtotalafteroveralldiscount = subtotal - totaldiscount; // 95 - 4.75 = 90.25
                    subtotalafteroveralldiscount = Math.Round(subtotalafteroveralldiscount, 2);
                    lblsubtotal.Text = subtotalafteroveralldiscount.ToString();

                    double payable = subtotalafteroveralldiscount + Convert.ToDouble(lblTotalVAT.Text);
                    payable = Math.Round(payable, 2) + Convert.ToDouble(txtShippingFee.Text);
                    lblTotalPayable.Text = payable.ToString();

                    //  btnPayment.Text = "Pay      = " + payable.ToString();
                }
            }
            catch
            {
                txtDiscountRate.Text = "0";
            }

        }

        private void btnDeCreaseVAT_Click(object sender, EventArgs e)
        {
            if (lblTotal.Text == "0")
            {
                MessageBox.Show("Please Add at least One Item");
            }
            else
            {
                double vatvalue = Convert.ToDouble(txtVATRate.Text) - 1;
                txtVATRate.Text = vatvalue.ToString();
                vatcal();
            }
        }

        private void btnIncreaseVAT_Click(object sender, EventArgs e)
        {
            if (lblTotal.Text == "0")
            {
                MessageBox.Show("Please Add at least One Item");
            }
            else
            {
                double vatvalue = Convert.ToDouble(txtVATRate.Text) + 1;
                txtVATRate.Text = vatvalue.ToString();
                vatcal();
            }
        }
        // Tax and discount increase / decrease   ---------------  END  ---------------- END -------------------     
        

        private void txtShippingFee_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (txtShippingFee.Text == string.Empty)
                {
                    txtShippingFee.Text = "0";
                }
                else
                {
                    vatcal();
                }
               // DiscountCalculation();
              //  vatcal();
            }
            catch
            {
                txtShippingFee.Text = "0";
            }
        
        }
               

        private void timer_InvoiceNoRefresh_Tick(object sender, EventArgs e)
        {
            try
            {
                string sql = "select  sales_id  from sales_payment order by sales_id desc";
                DataTable dt = DataAccess.GetDataTable(sql);
              //  double id = Convert.ToDouble(dt.Rows[0].ItemArray[0].ToString()) + 1;
               // txtinvoiceNo.Text = Convert.ToString(id);
                if (dt.Rows.Count > 0)
                {
                    double id = Convert.ToDouble(dt.Rows[0].ItemArray[0].ToString()) + 1;
                    txtinvoiceNo.Text = Convert.ToString(id);
                }
                else
                {
                    double id = 1;
                    txtinvoiceNo.Text = Convert.ToString(id);
                }
            }
            catch
            {

            } 
        }

        #region Request to submit
        /// //// Add sales item  //////////////Store into sales_item table /////////
        public bool sales_item()
        {          
            int rows = dgrvSalesItemList.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                string SalesDate = dtSalesDate.Text;
                string trno = txtinvoiceNo.Text;
                string itemid = dgrvSalesItemList.Rows[i].Cells[4].Value.ToString();
                string itNam = dgrvSalesItemList.Rows[i].Cells[0].Value.ToString();
                double qty = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[2].Value.ToString());
                double Rprice = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[1].Value.ToString());
                double total = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[3].Value.ToString());
                double dis = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[7].Value.ToString());
                double taxapply = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[8].Value.ToString());


                // =================================Start=====  Profit calculation =============== Start ========= 
                // Discount_amount = (Retail_price * discount) / 100                    -- 49 * 3 / 100 = 1.47
                // Retail_priceAfterDiscount = Retail_price - Discount_amount           -- 49 - 1.47 = 47.53
                // Profit = (Retail_priceAfterDiscount * QTY )   - (cost_price * qty);  ---( 47.53 * 1 ) - ( 45 * 1) = 2.53

                string sqlprofit = "Select cost_price , discount from  purchase  where product_id  = '" + itemid + "'";
                DataAccess.ExecuteSQL(sqlprofit);
                DataTable dt1 = DataAccess.GetDataTable(sqlprofit);

                double cost_price = Convert.ToDouble(dt1.Rows[0].ItemArray[0].ToString());
                double discount = Convert.ToDouble(dt1.Rows[0].ItemArray[1].ToString());

                double Discount_amount = (Rprice * discount) / 100.00;
                double Retail_priceAfterDiscount = Rprice - Discount_amount;
                double Profit = Math.Round((Retail_priceAfterDiscount - cost_price), 2);
                // =================================Start=====  Profit calculation =============== Start ========= 

                string sql1 = " insert into sales_item (sales_id,itemName,Qty,RetailsPrice,Total, profit,sales_time, itemcode , discount, taxapply) " +
                              " values ('" + trno + "', '" + itNam + "', '" + qty + "', '" + Rprice + "', '" + total + "', '" + Profit + "', " +
                              " '" + SalesDate + "','" + itemid + "','" + dis + "','" + taxapply + "')";
                DataAccess.ExecuteSQL(sql1);               

                //update quantity Decrease from Stock Qty |  purchase Table
                if (txtinvoiceNo.Text == "")
                {
                    MessageBox.Show("please check sales no ");
                }
                else
                { 

                    string itemids = dgrvSalesItemList.Rows[i].Cells[4].Value.ToString();
                    double qtyupdate = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[2].Value.ToString());

                    // Update Quantity
                    string sqlupdateQty = "select product_quantity  from purchase where product_id = '" + itemids + "'";
                    DataAccess.ExecuteSQL(sqlupdateQty);
                    DataTable dtUqty = DataAccess.GetDataTable(sqlupdateQty);
                    double product_quantity = Convert.ToDouble(dtUqty.Rows[0].ItemArray[0].ToString()) - qtyupdate;

                    string sql =    " update purchase set " +
                                    " product_quantity = '" + product_quantity + "'  " + 
                                    " where product_id = '" + itemids + "' ";
                    DataAccess.ExecuteSQL(sql);
                }

            }
            return true;                   

        }

        /// //// Payment items Add  //////////////////////Store into Sales_payment table //////////////// 
        public void payment_item()
        {
            string trno = txtinvoiceNo.Text;


            string payamount = lblTotalPayable.Text;
            string changeamount = "0";
            string due = "0";
            string tax = lblTotalVAT.Text;
            string DiscountTotal = lbloveralldiscount.Text;  // lblTotalDisCount.Text;
            string Comment =  CmbCustomer.Text + "  " + txtNote.Text;
            string overalldisRate = txtDiscountRate.Text;
            string vatRate = txtVATRate.Text;


            string sql1 =   " insert into sales_payment (sales_id, payment_type, payment_amount, change_amount, due_amount, dis, vat, " +
                            " sales_time, c_id, emp_id, comment, TrxType, Shopid, ovdisrate , vaterate ) " +
                            " values ('"+ txtinvoiceNo.Text + "','Invoice', '" + payamount + "', '" + changeamount + "', " + 
                            " '" + due + "', '" + DiscountTotal + "', '" + tax + "', '"+ dtSalesDate.Text + "','" + lblCustID.Text + "', " +
                            " '" + UserInfo.UserName + "','" + Comment + "', 'Inventory', '" + UserInfo.Shopid + "', '" + overalldisRate + "' , '" + vatRate + "' )";
            DataAccess.ExecuteSQL(sql1);            
        }


        /// //// Add sales Info  /////////////tbl_salesInfo////// 
        public void SaleInfo()
        {
            string invoiceNo    = txtinvoiceNo.Text;
            string warehouseNo  = CmbWarehouse.Text;
            string Biller       = CmbBiller.Text;
            string customer     = lblCustID.Text; // CmbCustomer.Text;
            string note         = lblCustID.Text + "  " + txtNote.Text;
            string shippingFee  = txtShippingFee.Text.Trim();

            string sql1 = "insert into tbl_saleInfo (InvoiceNo, WarehouseNo, Biller, Customer, Note, DisRate, TaxRate, ShippingFee, SoldBy, Datetime)  values ('";
            sql1 += invoiceNo + "', '" + warehouseNo + "', '" + Biller + "', '" + customer + "', '" + note + "', '" + lblTotalDisCount.Text + "', '";
            sql1 += txtVATRate.Text + "','" + shippingFee + "', '" + lblUserID.Text + "','" + dtSalesDate.Text + "')";
            DataAccess.ExecuteSQL(sql1);
        }
        
        // Save and Complete Sales
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Complete Sale and Print?  ", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (lblTotal.Text == "0")
                {
                    MessageBox.Show("Sorry ! you have not enough product \n  Please Purchase product or Increase Product Quantity");
                }
                //else if (Convert.ToInt32(txtinvoiceNo.Text) >= 53)  //Please uncommet this section
                //{
                //    MessageBox.Show("Sorry ! Demo version has limited transaction \n Please buy it \n contact at : citkar@live.com", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                //}
                else if (CmbWarehouse.Text == string.Empty)
                {
                    MessageBox.Show("Please Select WareHouse .. ");
                    CmbWarehouse.Focus();
                }
                else
                {
                    try
                    {                     

                        //Save payment info into 'sales_payment' table
                        payment_item();

                        //Save Sales item into 'sales_item' table
                        sales_item();

                        //Save Sale info into 'tbl_saleInfo' table
                        SaleInfo();

                        // 5 % Rewards Point add to customer Account for total Payable amount 
                      //  AddCredit();

                       // Inventory.InvoicePrint go = new Inventory.InvoicePrint(txtinvoiceNo.Text);
                        View_Sales_invoice go = new View_Sales_invoice(txtinvoiceNo.Text);
                        go.ShowDialog();
                        
                        ShowSales_id_increment();
                        ClearForm2();
                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            }
        }

        #endregion

        private void ShowSales_id_increment()
        {
            try
            {
                string sql = "select  sales_id  from sales_payment order by sales_id desc";
                DataTable dt = DataAccess.GetDataTable(sql);     
                if (dt.Rows.Count > 0)
                {
                    double id = Convert.ToDouble(dt.Rows[0].ItemArray[0].ToString()) + 1;
                    txtinvoiceNo.Text = Convert.ToString(id);
                }
                else
                {
                    double id = 1;
                    txtinvoiceNo.Text = Convert.ToString(id);
                }
            }
            catch
            {

            } 
        }

        private void ClearForm2()
        {            
            Inventory.Add_Sales go = new Inventory.Add_Sales();
            go.MdiParent = this.ParentForm;
            go.Show();
            this.Close();

        }

        #region Page links
        private void lnkAddCust_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Customer.AddNewCustomer go = new Customer.AddNewCustomer();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void lblAddStockItem_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Inventory.StockShortList go = new Inventory.StockShortList();
            // go.MdiParent = this.ParentForm;
            go.ShowDialog();
        }
        #endregion

        //Add Customer Rewards
        public void AddCredit()
        {
            // 5 % Rewards Point add for total Payable amount 
            double CreditPoint = (Convert.ToDouble(lblTotalPayable.Text) * 5) / 100;
            string sqlCmd = "insert into tbl_custcredit (CustID, orderID, Date, Credit, Description)  values ('" + lblCustID.Text + "', '" + txtinvoiceNo.Text + "', '" + dtSalesDate.Text + "', '" + CreditPoint + "', 'Add Rewards')";
            DataAccess.ExecuteSQL(sqlCmd);
        }

        #region customer info
        private void CmbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerID();
        }

        public void CustomerID()
        {
            try
            {
                string sqlCmd = "Select ID from  tbl_customer  where Name  = '" + CmbCustomer.Text + "'";
                DataAccess.ExecuteSQL(sqlCmd);
                DataTable dt1 = DataAccess.GetDataTable(sqlCmd);
                lblCustID.Text = dt1.Rows[0].ItemArray[0].ToString();
            }
            catch
            {
            }

        }
        #endregion

        private void btnSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                dgrvSalesItemList.Rows.Clear();
                // lblTotalItems.Text = "0";
                txtDiscountRate.Text = "0";
                lbloveralldiscount.Text = "0";
                DiscountCalculation();
                vatcal();
            }
            catch
            {
            }
        }

        private void txtDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtDiscountRate.Text.ToString(), @"\.\d\d\d");

                if (e.KeyChar == '\b') // Always allow a Backspace
                    ignoreKeyPress = false;
                else if (matchString)
                    ignoreKeyPress = true;
                else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    ignoreKeyPress = true;
                else if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                    ignoreKeyPress = true;

                e.Handled = ignoreKeyPress;
                //using System.Text.RegularExpressions;
            }
            catch
            {
            } 
        }

        private void txtRSDisc_TextChanged(object sender, EventArgs e)
        {
            try
            {

                Double res = Convert.ToDouble(txtRSDisc.Text) / Convert.ToDouble(lblTotal.Text) * 100;
                txtDiscountRate.Text = Convert.ToString(Math.Round(res, 2));
                //  lbloveralldiscount.Text = lblTotalDisCount.Text;
            }
            catch (Exception) { }
        }

        private void txtSearchItem_KeyDown(object sender, KeyEventArgs e)
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
                    txtSearchItem.Focus();
                    txtSearchItem.SelectAll();
                }
            }
        }

        private void lst_items_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                txtBarcodeReaderBox.Text = lst_items.SelectedItems[0].SubItems[0].Text;
            // SetDiscount();
        }

        private void lst_items_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }
        private static string GetNumbers(string input)
        {
            return new string(input.Where(c => char.IsDigit(c)).ToArray());
        }
        private void lst_items_Click(object sender, EventArgs e)
        {
            try
            {
                // int selectionindex = lst_items.inde();
                string val1 = lst_items.SelectedItems[0].ToString();
                string a = GetNumbers(val1);

                txtBarcodeReaderBox.Text = a;

            }
            catch (Exception) { }
        }

        private void lst_items_Click_1(object sender, EventArgs e)
        {
            try
            {
                // int selectionindex = lst_items.inde();
                string val1 = lst_items.SelectedItems[0].ToString();
                string a = GetNumbers(val1);



                txtBarcodeReaderBox.Text = a;

            }
            catch (Exception) { }
        }
    }
}
