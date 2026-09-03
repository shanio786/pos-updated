using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Drawing.Printing;
 

namespace supershop 
{
    public partial class SalesRegister : Form
    {
        private int m_currentPageIndex;
        string saleType = "";

        /// <summary>
        ///  Author : Tuaha Mohammad 
        ///  Email:   citkar@live.com        ///  
        ///Web:         http://codecanyon.net/user/dynamicsoft/portfolio
        ///Item Link:   http://codecanyon.net/item/advance-point-of-sale-system-pos/6317175
        /// </summary>
        /// <param name="aa"></param>
        /// Developed by DynamicSoft
        // Actual size = 1188, 679

        public SalesRegister()
        {
            InitializeComponent();
            lbluser.Text = UserInfo.UserName;             
            this.tabPageSR_Payment.Parent = null; //Hide payment tab
           // tabSRcontrol.TabPages.Remove(tabPageSR_Payment);
            txtBarcodeReaderBox.Focus();
           

            formFunctionPointer += new functioncall(Replicate); // Coin and papernotes
            currency_Shortcuts1.CoinandNotesFunctionPointer = formFunctionPointer;

            numformFunctionPointer += new numvaluefunctioncall(NumaricKeypad);
            currency_Shortcuts1.NumaricKeypad = numformFunctionPointer;
        }
        public void LoadTotalDiscount()
        {
            // storeconfig.disrate (column 7) - was reading column 6 (vatrate) by mistake
            DataTable dt1 = DataAccess.GetDataTable("select disrate from storeconfig");
            if (dt1.Rows.Count > 0)
                txtDiscountRate.Text = dt1.Rows[0][0].ToString();
        }
        public void LoadTotalTax()
        {
            DataTable dt1 = DataAccess.GetDataTable("select vatrate from storeconfig");
            if (dt1.Rows.Count > 0)
                txtVATRate.Text = dt1.Rows[0][0].ToString();
        }
        #region Databind
        //Default Form Load 
        private void SalesRegister_Load(object sender, EventArgs e)
        {
            LoadTotalDiscount();
            LoadTotalTax();
            try
            {
                CategoryList_with_images();  

                //Load Vat rate
                txtVATRate.Text = vatdisvalue.vat;


                this.dgrvSalesItemList.Columns.Add("itm", "Items Name");
                this.dgrvSalesItemList.Columns.Add("Am", "Price");
                this.dgrvSalesItemList.Columns.Add("Qty", "Qty");
                this.dgrvSalesItemList.Columns.Add("Total", "Total");             
                this.dgrvSalesItemList.Columns.Add("ID", "ID");
                this.dgrvSalesItemList.Columns.Add("disamt", "Disamt");     // 5. new in 8.1 version
                this.dgrvSalesItemList.Columns.Add("taxamt", "taxamt");     // 6. new in 8.1 version
                this.dgrvSalesItemList.Columns.Add("dis", "Dis");           // 7. new in 8.1 version
                this.dgrvSalesItemList.Columns.Add("taxapply", "Tax");      // 8. new in 8.1 version
                this.dgrvSalesItemList.Columns.Add("kitdisplay", "KD");      // 8. new in 8.3.1 version
             //   this.dgrvSalesItemList.Columns.Add("stockQty", "SQ");      // 8. new in 8.3.1 version

                //Hide fields
                dgrvSalesItemList.Columns[4].Visible = false; // ID             // new in 8.1 version
                dgrvSalesItemList.Columns[5].Visible = false; // Disamt         // new in 8.1 version
                dgrvSalesItemList.Columns[6].Visible = false; // taxamt         // new in 8.1 version
                dgrvSalesItemList.Columns[7].Visible = false; // Discount rate  // new in 8.1 version
                dgrvSalesItemList.Columns[9].Visible = false; // kitdisplay    // new in 8.3.1 version

                //Font size of columns and aligmnet  // add in from version 8.3
                dgrvSalesItemList.Columns["itm"].DefaultCellStyle.Font = new Font("Microsoft Sans Serif", 9);
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgrvSalesItemList.Columns["taxapply"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
               

               ///// dataGridView1.Rows.Add(1);         
               
                DataGridViewButtonColumn inc = new DataGridViewButtonColumn();
                dgrvSalesItemList.Columns.Add(inc);
                inc.HeaderText = "Inc";
                inc.Text = "+";
                inc.Name = "inc";
                inc.ToolTipText = "Increase Item Qty";
                inc.UseColumnTextForButtonValue = true;

                DataGridViewButtonColumn minus = new DataGridViewButtonColumn();
                dgrvSalesItemList.Columns.Add(minus);
                minus.HeaderText = "Dec";
                minus.Text = "-";
                minus.Name = "minus";
                minus.ToolTipText = "minus Item Qty";
                minus.UseColumnTextForButtonValue = true;

                DataGridViewButtonColumn del = new DataGridViewButtonColumn();
                dgrvSalesItemList.Columns.Add(del);
                del.HeaderText = "Del";
                del.Text = "x";
                del.Name = "del";
                del.ToolTipText = "Delete this Item";
                del.UseColumnTextForButtonValue = true;
                

               // this.dgrvSalesItemList.Rows[0].Cells[2].Value = "1";
               //  dgrvSalesItemList.ReadOnly = true;
                dgrvSalesItemList.Columns[0].ReadOnly = true;
                dgrvSalesItemList.Columns[1].ReadOnly = false;  // Price
                dgrvSalesItemList.Columns[2].ReadOnly = false;
                dgrvSalesItemList.Columns[3].ReadOnly = true;
                dgrvSalesItemList.Columns[4].ReadOnly = true;
                dgrvSalesItemList.Columns[5].ReadOnly = true;
                dgrvSalesItemList.Columns[6].ReadOnly = true;
                dgrvSalesItemList.Columns[7].ReadOnly = true;
                dgrvSalesItemList.Columns[8].ReadOnly = true;
                dgrvSalesItemList.Columns[9].ReadOnly = true;

                //Qty column row color
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.ForeColor = Color.Black;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.BackColor = Color.Silver;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.SelectionForeColor = Color.Black;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.SelectionBackColor = Color.Silver;
                dgrvSalesItemList.Columns["Qty"].DefaultCellStyle.Font = new Font(DataGridView.DefaultFont, FontStyle.Bold);

                

                //  Column width
                dgrvSalesItemList.Columns["itm"].Width = 200;
                dgrvSalesItemList.Columns["Del"].Width = 11;
                dgrvSalesItemList.Columns["inc"].Width = 35;
                dgrvSalesItemList.Columns["minus"].Width = 35;
               // dgrvSalesItemList.Columns["stockQty"].Width = 5; 
               // dgrvSalesItemList.Rows[0].Cells[2].Style.BackColor = Color.Red;
               // DataGridViewColumn ColQty = dgrvSalesItemList.Columns[2];
               // ColQty.Width = 45;


                //Load Invoice No / Receipt No for display (the real number is taken inside SaveSale)
                ShowNextInvoiceNo();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        //Show Products list (filtered by product name / id / category)
        public void ItemList_with_images(string value, ListView _lst_items)
        {
            flowLayoutPanelItemList.Controls.Clear();
            try
            {
                string sql = " select * from vw_itemdisplay_sr where product_quantity >= 1 " +
                             " and ( product_name like @q + '%' OR product_id like @q + '%' OR category like @q + '%' ) ";

                DataTable dt = DataAccess.GetDataTable(sql, DataAccess.P("@q", value));
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
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        //Product filter by Product Name or Product ID
        private void txtSearchItem_TextChanged(object sender, EventArgs e)
        {
            ItemList_with_images(txtSearchItem.Text, lst_items);
        }


        //Click add to cart
        protected void b_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            txtBarcodeReaderBox.Text = b.Tag.ToString();
        }

        //// BarCode or keyboard input  items code  || add to cart
        private void txtBarcodeReaderBox_TextChanged(object sender, EventArgs e)
        {
                try
                {
                    dgrvSalesItemList.Visible = true;
                    // Default tax rate 
                    double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                    //- new in 8.1 version // Default Product QTY is 1
                    string sql = "SELECT  product_name as Name , retail_price as Price , 1.00  as QTY, (retail_price * 1.00 ) * 1.00  as 'Total' ,  " +
                            " (((retail_price * 1.00 ) * discount) / 100.00) as 'dis amt' , " +
                            " CASE     " +
                            " WHEN taxapply = 1 THEN   (((retail_price * 1.00 )  - (((retail_price * 1.00 ) * discount) / 100.00))  * @taxrate ) / 100.00   " +
                            " ELSE 0.00  " +
                            " END 'taxamt' , product_id as ID , discount , taxapply, status, product_quantity  " +
                            " FROM  purchase  where product_id = @id  and product_quantity >= 1 ";
                    DataTable dt = DataAccess.GetDataTable(sql,
                        DataAccess.P("@taxrate", Convert.ToDecimal(Taxrate)),
                        DataAccess.P("@id", txtBarcodeReaderBox.Text));

                    string ItemsName    = dt.Rows[0].ItemArray[0].ToString();
                    double Rprice       = Convert.ToDouble(dt.Rows[0].ItemArray[1].ToString());
                    double Qty          = Convert.ToDouble(dt.Rows[0].ItemArray[2].ToString());
                    double Total        = Convert.ToDouble(dt.Rows[0].ItemArray[3].ToString()) * Qty;
                    string Itemid       = dt.Rows[0].ItemArray[6].ToString();
                    double Disamt       = Convert.ToDouble(dt.Rows[0].ItemArray[4].ToString());       //  Total Discount amount of this item
                    double Taxamt       = Convert.ToDouble(dt.Rows[0].ItemArray[5].ToString());       //  Total Tax amount  of this item
                    double Dis          = Convert.ToDouble(dt.Rows[0].ItemArray[7].ToString());       //  Discount Rate
                    double Taxapply     = Convert.ToDouble(dt.Rows[0].ItemArray[8].ToString());       //  VAT/TAX/TPS/TVQ apply or not
                    int kitchendisplay  = Convert.ToInt32(dt.Rows[0].ItemArray[9].ToString());        //  kitchen display 3= show 1= not display in kitchen 
                    double Stockqty     = Convert.ToDouble(dt.Rows[0].ItemArray[10].ToString());        //   

                    //Add to Item list
                   // long i = 1;
                    int n = Finditem(ItemsName);
                    if (n == -1)  //If new item
                    {
                        dgrvSalesItemList.Rows.Add(ItemsName, Rprice, Qty, Rprice, Itemid, Disamt, Taxamt, Dis, Taxapply, kitchendisplay);
                    }
                    else  // if same item Quantity increase by 1 
                    {
                        //// if given Qty > stock qty { Stcok exceed from stock  }                      
                        if (Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[2].Value) >= Stockqty)
                        {
                            MessageBox.Show("Quantity Exceed from Stcok Qty"); 
                        }
                        else
                        {
                            //  dgrvSalesItemList.Rows[n].Cells[0].Value = ItemsName;
                            // dgrvSalesItemList.Rows[n].Cells[1].Value = Rprice;
                            int QtyInc = Convert.ToInt32(dgrvSalesItemList.Rows[n].Cells[2].Value);
                            dgrvSalesItemList.Rows[n].Cells[2].Value = (QtyInc + 1);  //Qty Increase
                            dgrvSalesItemList.Rows[n].Cells[3].Value = Rprice * (QtyInc + 1);   // Total price
                            //  dgrvSalesItemList.Rows[n].Cells[4].Value = Itemid;                     

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

                            // dgrvSalesItemList.Rows[n].Cells[7].Value = Dis; // Discount rate
                            //  dgrvSalesItemList.Rows[n].Cells[8].Value = Taxapply;  //Tax apply
                            //  dgrvSalesItemList.Rows[n].Cells[9].Value = kitchendisplay;
                        }
            

                    }                  
                   

                    //Hide fields
                    dgrvSalesItemList.Columns[4].Visible = false; // ID             // new in 8.1 version
                    dgrvSalesItemList.Columns[5].Visible = false; // Disamt         // new in 8.1 version
                    dgrvSalesItemList.Columns[6].Visible = false; // taxamt         // new in 8.1 version
                    dgrvSalesItemList.Columns[7].Visible = false; // Discount rate  // new in 8.1 version
                    dgrvSalesItemList.Columns[9].Visible = false; // kitdisplay    // new in 8.3.1 version

                    txtBarcodeReaderBox.Text = "";
                    txtBarcodeReaderBox.Focus();

                    btnSuspend.Enabled = true;
                    btnPayment.Enabled = true;
                    btnSalesCredit.Enabled = true;
                    btnPrintDirect.Enabled = true;

                    DiscountCalculation();
                    vatcal();
                    //txtDiscountRate.Text = "0";
                    LoadTotalDiscount();
                    LoadTotalTax();
                    // lbloveralldiscount.Text = "0";

                    if (dt.Rows.Count > 0)
                    {
                        lblNotFound.Visible = false;
                    }

                    else
                    {
                        lblNotFound.Visible = true;
                    }
                }

                catch (Exception exLog) { Logger.Error(exLog); }
        }

        // Check duplicate item 
        public int Finditem(string item)
        {
            int k = -1;
            if (dgrvSalesItemList.Rows.Count > 0)
            {
                foreach (DataGridViewRow row in dgrvSalesItemList.Rows)
                {
                    if (row.Cells[4].Value.ToString().Equals(item))
                    {
                        k = row.Index;
                        break;
                    }
                }
            }
            return k;
        }
        #endregion


        #region Category Databind and click event  | Product filter by Category
        //Show Category    -- Add new from 8.3.2
        public void CategoryList_with_images()
        {
            string img_directory = Application.StartupPath + @"\ITEMIMAGE\";
            try
            {
                string sql = "select   DISTINCT  category   from purchase where product_quantity >= 1";
                DataTable dt = DataAccess.GetDataTable(sql);

                int currentImage = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataReader = dt.Rows[i];

                    Button category = new Button();
                    // Image i4 = Image.FromFile(img_directory + dataReader["name"]);
                    category.Tag = dataReader["category"];
                    category.Click += new EventHandler(category_Click);
                  

                    ImageList il = new ImageList();
                    il.ColorDepth = ColorDepth.Depth32Bit;
                    il.TransparentColor = Color.Blue;
                    il.ImageSize = new Size(115, 49);
                    il.Images.Add(Image.FromFile(img_directory + "category.png"));


                    category.Image = il.Images[0];
                    category.Margin = new Padding(3, 3, 3, 3);

                    category.Size = new Size(122, 49);
                    category.Text.PadRight(1);

                    // category.Text += " " + dataReader["product_name"];
                    category.Text += dataReader["category"].ToString();


                    category.Font = new Font("Times New Roman", 14, FontStyle.Regular, GraphicsUnit.Point);
                    category.TextAlign = ContentAlignment.MiddleCenter;
                    category.TextImageRelation = TextImageRelation.Overlay;
                    currentImage++;

                }
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        //Filter Product by category   -- Add new from 8.3.2
        protected void category_Click(object sender, EventArgs e)
        {
            Button category = sender as Button;
            string s;
            s = " ID: ";
            s += category.Tag;
            s += "\n Name: ";
            s += category.Name.ToString();

            //   txtBarcodeReaderBox.Text = category.Tag.ToString();
            //ItemList_with_images(category.Tag.ToString());
        }
        #endregion


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

            double payable = sum + Convert.ToDouble(lblTotalVAT.Text);
            payable = Math.Round(payable,2);
            lblTotalPayable.Text = payable.ToString();
            lblTotalpayableAmtPY.Text = payable.ToString();

            lblTotalDisCount.Text = DisCount.ToString();
            lbloveralldiscount.Text = DisCount.ToString();
           // btnPayment.Text = "Pay = " + payable.ToString();

            tabPageSR_Counter.Text = "Terminal (" + dgrvSalesItemList.RowCount.ToString() + ")";
            tabPageSR_Payment.Text = "Payment (" + payable.ToString() + ")";
        }

        //VAT amount sum calculation - Change in 8.1 version
        public void vatcal()
        {
            //Subtotal = total - (discount + Globaldiscount)
            double Subtotal = Convert.ToDouble(lblsubtotal.Text);            
            //double Subtotal = Convert.ToDouble(lbloveralldiscount.Text)  ;

            //VAT amount
            double VAT = 0.00;
            for (int i = 0; i < dgrvSalesItemList.Rows.Count; ++i)
            {
                VAT += Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[6].Value);
            }

            VAT = Math.Round(VAT, 2);
            lblTotalVAT.Text = VAT.ToString();

            double payable = Subtotal + VAT;
            payable = Math.Round(payable, 2);
            lblTotalPayable.Text = payable.ToString();
            lblTotalpayableAmtPY.Text = payable.ToString();
           // btnPayment.Text = "Pay = " + payable.ToString();
	    
	      ///////Pole shows Price value  | if you have pole device please UnComment   below code
		//SerialPort sp = new SerialPort();
		//sp.PortName = "COM1";  ////Insert your pole Device Port Name E.g. COM4  -- you can find  from pole device manual  
		//sp.BaudRate = 9600;     // Pole Bound Rate 
		//sp.Parity = Parity.None;
		//sp.DataBits = 8;   // Data Bits
		//sp.StopBits = StopBits.One;
		//sp.Open();                 
		//sp.WriteLine(lblTotalPayable.Text);

		//sp.Close();
		//sp.Dispose();
		//sp = null;
        }
        
        // Sales item   Increase , Decrease and Delete Options
        private void dgrvSalesItemList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
             
                // Delete items From Gridview
                if (e.ColumnIndex == dgrvSalesItemList.Columns["del"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow row2 in dgrvSalesItemList.SelectedRows)
                    {
                      //  DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                      //  if (result == DialogResult.Yes)
                      //  {
                            if (!row2.IsNewRow)
                                dgrvSalesItemList.Rows.Remove(row2);
                            DiscountCalculation();
                            vatcal();
                        //txtDiscountRate.Text = "0";
                        LoadTotalDiscount();
                        LoadTotalTax();
                        txtBarcodeReaderBox.Focus();
                        // lbloveralldiscount.Text = "0";
                        // }
                    } 
                }

                // Increase Item Quantity
                if (e.ColumnIndex == dgrvSalesItemList.Columns["inc"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow row in dgrvSalesItemList.SelectedRows)
                    {
                        //  dgrvSalesItemList.Rows[0][0].Convert.ToDouble(row.Cells[10].Value))Convert.ToString(row.Cells[4].Value.ToString() // Convert.ToString(dgrvSalesItemList.Rows[e.RowIndex].Cells[4].Value.ToString()
                        if (Convert.ToDouble(row.Cells[2].Value) >= CheckStockQty(Convert.ToString(dgrvSalesItemList.Rows[e.RowIndex].Cells[4].Value.ToString())))
                        {
                            MessageBox.Show("You don't have sufficient item Quantity \n\n Your  Item Quantity is greater than Stock Qty");
                            row.Cells[2].Value = CheckStockQty(Convert.ToString(dgrvSalesItemList.Rows[e.RowIndex].Cells[4].Value.ToString()));

                            double qtySum = Convert.ToDouble(row.Cells[2].Value);
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
                        }
                        else
                        {
                            //// Increase by 1
                            double qtySum = Convert.ToDouble(row.Cells[2].Value) + 1;
                            row.Cells[2].Value = qtySum;

                            double qty      = Convert.ToDouble(row.Cells[2].Value);
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
                        

                          } 
                            DiscountCalculation();
                            vatcal();
                            //txtDiscountRate.Text = "0";
                            LoadTotalDiscount();
                            LoadTotalTax();
                    }
                }

                // Decrease Item Quantity  -- Add new from 8.3.2
                if (e.ColumnIndex == dgrvSalesItemList.Columns["minus"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow row in dgrvSalesItemList.SelectedRows)
                    {
                        if (Convert.ToDouble(row.Cells[2].Value)  >  1)
                        {
                            //// Decrease by 1 
                            double qtySum = Convert.ToDouble(row.Cells[2].Value) - 1;
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
                            //txtDiscountRate.Text = "0";  
                            LoadTotalDiscount();
                            LoadTotalTax();
                        }
                   
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
                        // Total Price
                       // double totalPrice = Convert.ToDouble(row.Cells[2].Value) * Convert.ToDouble(row.Cells[1].Value);
                       // row.Cells[3].Value = totalPrice;
                        if (Convert.ToDouble(row.Cells[2].Value) > CheckStockQty(Convert.ToString(dgrvSalesItemList.Rows[e.RowIndex].Cells[4].Value.ToString())))
                        {
                            MessageBox.Show("You don't have sufficient item Quantity \n\n Your  Item Quantity is greater than Stock Qty");
                            row.Cells[2].Value = CheckStockQty(Convert.ToString(dgrvSalesItemList.Rows[e.RowIndex].Cells[4].Value.ToString()));

                            double qty = Convert.ToDouble(row.Cells[2].Value);
                            double Rprice = Convert.ToDouble(row.Cells[1].Value);
                            double disrate = Convert.ToDouble(row.Cells[7].Value);
                            double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                            //// show total price   Qty  * Rprice
                            double totalPrice = qty * Rprice;
                            row.Cells[3].Value = totalPrice;

                            if (Convert.ToDouble(row.Cells[7].Value) != 0)  // IF discount is not zero then apply discount
                            {
                                double Disamt = (((Rprice * qty) * disrate) / 100.00);      // Total Discount amount of this item
                                row.Cells[5].Value = Disamt;
                            }

                            if (Convert.ToDouble(row.Cells[8].Value) != 0)  // IF tax is not zero then apply tax
                            {
                                double Taxamt = ((((Rprice * qty) - (((Rprice * qty) * disrate) / 100.00)) * Taxrate) / 100.00); // Total Tax amount  of this item
                                row.Cells[6].Value = Taxamt;
                            }
                        }
                        else
                        {
                            double qty = Convert.ToDouble(row.Cells[2].Value);
                            double Rprice = Convert.ToDouble(row.Cells[1].Value);
                            double disrate = Convert.ToDouble(row.Cells[7].Value);
                            double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                            //// show total price   Qty  * Rprice
                            double totalPrice = qty * Rprice;
                            row.Cells[3].Value = totalPrice;

                            if (Convert.ToDouble(row.Cells[7].Value) != 0)  // IF discount is not zero then apply discount
                            {
                                double Disamt = (((Rprice * qty) * disrate) / 100.00);      // Total Discount amount of this item
                                row.Cells[5].Value = Disamt;
                            }

                            if (Convert.ToDouble(row.Cells[8].Value) != 0)  // IF tax is not zero then apply tax
                            {
                                double Taxamt = ((((Rprice * qty) - (((Rprice * qty) * disrate) / 100.00)) * Taxrate) / 100.00); // Total Tax amount  of this item
                                row.Cells[6].Value = Taxamt;
                            }
                        }

                        DiscountCalculation();
                        vatcal();
                        //txtDiscountRate.Text = "0";
                        LoadTotalDiscount();
                        LoadTotalTax();

                    }
                }
            }
            catch (Exception exLog) { Logger.Error(exLog); }

        }


        //Suspend Order/ Cancel transaction
        private void btnSuspend_Click(object sender, EventArgs e)
        {
            try
            {
                dgrvSalesItemList.Rows.Clear();
                dgrvSalesItemList.Visible = false;
                // lblTotalItems.Text = "0";
                //txtDiscountRate.Text = "0";
                LoadTotalDiscount();
                LoadTotalTax();
                lbloveralldiscount.Text = "0";
                DiscountCalculation();
                vatcal();
                btnSalesCredit.Enabled = false;
                btnPayment.Enabled = false;
                tabPageSR_Counter.Text = "Terminal";
                this.tabPageSR_Payment.Parent = null; //Hide payment tab
                txtBarcodeReaderBox.Focus();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        
        // Auto Invoice.No Shows 
        private void btnHold_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgrvSalesItemList.Rows.Count == 0)
                {
                    MessageBox.Show("The cart is empty.");
                    return;
                }
                string label = SalesRagister.HeldSaleStore.Prompt(
                    "Name for this held sale (customer / table):", "Hold sale", txtCustName.Text);
                if (label == null) return;   // cancelled
                SalesRagister.HeldSaleStore.Hold(label, lblCustID.Text, dgrvSalesItemList);
                btnSuspend_Click(sender, e);   // clear the counter for the next customer
                MessageBox.Show("Sale held. Use Resume to bring it back.", "Hold", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not hold the sale.");
            }
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            try
            {
                using (SalesRagister.HeldSalesForm f = new SalesRagister.HeldSalesForm())
                {
                    if (f.ShowDialog() != DialogResult.OK) return;
                    long holdId = f.SelectedHoldId;
                    DataTable items = SalesRagister.HeldSaleStore.Items(holdId);

                    dgrvSalesItemList.Rows.Clear();
                    foreach (DataRow r in items.Rows)
                    {
                        // (Name, Price, Qty, Total, Code, DisAmt, TaxAmt, DisRate, TaxApply, KitchenDisplay)
                        dgrvSalesItemList.Rows.Add(
                            r["itemName"], r["RetailsPrice"], r["Qty"], r["Total"], r["itemcode"],
                            r["disamt"], r["taxamt"], r["disrate"], r["taxapply"], r["kitchendisplay"]);
                    }
                    dgrvSalesItemList.Visible = true;
                    string cust = SalesRagister.HeldSaleStore.CustId(holdId);
                    if (!string.IsNullOrEmpty(cust)) lblCustID.Text = cust;
                    SalesRagister.HeldSaleStore.Delete(holdId);   // it is now live on the counter

                    tabPageSR_Counter.Text = "Terminal";
                    LoadTotalDiscount();
                    LoadTotalTax();
                    DiscountCalculation();
                    vatcal();
                    btnSalesCredit.Enabled = true;
                    btnPayment.Enabled = true;
                    btnSuspend.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not resume the held sale.");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ShowNextInvoiceNo();
        }

        // Next invoice number for display only (the real number is taken inside SaveSale)
        private void ShowNextInvoiceNo()
        {
            try
            {
                decimal id = DataAccess.GetDecimal("SELECT ISNULL(MAX(sales_id),0)+1 FROM sales_payment");
                txtInvoice.Text = Convert.ToString(Convert.ToInt64(id));
            }
            catch (Exception exLog) { Logger.Error(exLog); } 
        }

        //  Discount
        // Flat (fixed rupee) counter discount, in addition to the per-item percentage
        // discounts. Overwrites any percentage counter discount currently applied.
        private void btnFlatDisc_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgrvSalesItemList.Rows.Count == 0) { MessageBox.Show("Please add at least one item."); return; }
                double total = ToDouble(lblTotal.Text);
                double itemDisc = ToDouble(lblTotalDisCount.Text);
                double maxFlat = System.Math.Round(total - itemDisc, 2);
                string input = SalesRagister.HeldSaleStore.Prompt(
                    "Flat discount amount in Rs (max " + maxFlat.ToString("0.00") + "):", "Flat discount", "0");
                if (input == null) return;
                double flat;
                if (!double.TryParse(input, out flat) || flat < 0) { MessageBox.Show("Enter a valid amount."); return; }
                if (flat > maxFlat) flat = maxFlat;
                txtDiscountRate.Text = "0";   // clear any % counter discount (fires a recompute)
                double overall = System.Math.Round(itemDisc + flat, 2);
                double subtotalAfter = System.Math.Round(total - overall, 2);
                double payable = System.Math.Round(subtotalAfter + ToDouble(lblTotalVAT.Text), 2);
                lbloveralldiscount.Text = overall.ToString();
                lblsubtotal.Text = subtotalAfter.ToString();
                lblTotalPayable.Text = payable.ToString();
            }
            catch (Exception ex) { Logger.Show(ex, "Could not apply the flat discount."); }
        }

        private void btnIncreaseDisCount_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblTotalPayable.Text == "")
                {
                    MessageBox.Show("Please Add at least One Item");
                }
                else
                {
                    double Discountvalue = Convert.ToDouble(txtDiscountRate.Text);
                    txtDiscountRate.Text = Discountvalue.ToString();
                    double subtotal = Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(lblTotalDisCount.Text); // total - item discount  100 - 5 = 95        
                    double totaldiscount = (subtotal * Discountvalue) / 100;  //Counter discount  // 95 * 5 /100 = 4.75  
                   // double totaldiscount = Convert.ToDouble(lblTotalDisCount.Text) + Discountvalue;   // Uncomment this line if you want to discount value and comment/delete above line
                    double disPlusOverallDiscount = totaldiscount + Convert.ToDouble(lblTotalDisCount.Text); // 4.75 + 5 = 9.75
                    disPlusOverallDiscount = Math.Round(disPlusOverallDiscount, 2);
                    lbloveralldiscount.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75

                    double subtotalafteroveralldiscount = subtotal - totaldiscount; // 95 - 4.75 = 90.25
                    subtotalafteroveralldiscount = Math.Round(subtotalafteroveralldiscount, 2);
                    lblsubtotal.Text = subtotalafteroveralldiscount.ToString();

                    double payable = subtotalafteroveralldiscount + Convert.ToDouble(lblTotalVAT.Text);
                    payable = Math.Round(payable, 2);
                    lblTotalPayable.Text = payable.ToString();

                  //  btnPayment.Text = "Pay = " + payable.ToString();

                }
            }
            catch
            {
                //txtDiscountRate.Text = "0";
                LoadTotalDiscount();
                LoadTotalTax();
            }
 
        }

        //Decrease Discount     new   8.1 version - Now not used
        private void btnDecreaseDiscount_Click(object sender, EventArgs e)
        {
            if (lblTotalPayable.Text == "")
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
                payable = Math.Round(payable, 2);
                lblTotalPayable.Text = payable.ToString();

              //  btnPayment.Text = "Pay = " + payable.ToString();

                //double Discountvalue = Convert.ToDouble(txtDiscountRate.Text) - 1;
                //txtDiscountRate.Text = Discountvalue.ToString();
                //DiscountCalculation();
                //vatcal();
            }
        }

        #region ////////////////  Submit request - New  ////////////////////////

        /// <summary>Numeric cell / column value as double (0 for NULL or empty).</summary>
        private static double ToDouble(object value)
        {
            if (value == null || value == DBNull.Value) return 0;
            string s = value.ToString().Trim();
            return s.Length == 0 ? 0 : Convert.ToDouble(s);
        }

        /// <summary>
        /// Saves the complete sale in ONE transaction: the sales_payment row, one
        /// sales_item row per cart line and the stock decrease in purchase.
        /// The invoice number is taken inside the transaction (safe when several
        /// terminals sell at the same time).  Returns the new invoice number.
        /// Payment type comes from CombPayby and customer from lblCustID (as before).
        /// </summary>
        public long SaveSale(string payamount, string changeamount, string dueamount, string salesdate, string Comment, System.Collections.Generic.List<SalesRagister.Tender> tenders)
        {
            saleType = "CashSale";
            string payby          = (tenders != null && tenders.Count > 0) ? (tenders.Count > 1 ? "Split" : tenders[0].Method) : CombPayby.Text;
            string vat            = lblTotalVAT.Text;
            string DiscountTotal  = lbloveralldiscount.Text; // Total discount = item wise discount + counter discount
            string custId         = lblCustID.Text;
            string overalldisRate = txtDiscountRate.Text;
            string vatRate        = txtVATRate.Text;

            long newId = 0;
            DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
            {
                long salesId = tx.NextSalesId();

                // 1. Payment header (sales_payment)
                tx.Execute(" insert into sales_payment (sales_id, payment_type, payment_amount, change_amount, due_amount, dis, vat, " +
                           " sales_time, c_id, emp_id, comment, TrxType, Shopid, ovdisrate, vaterate, SaleType) " +
                           " values (@sales_id, @payment_type, @payment_amount, @change_amount, @due_amount, @dis, @vat, " +
                           " @sales_time, @c_id, @emp_id, @comment, 'POS', @Shopid, @ovdisrate, @vaterate, @SaleType)",
                    DataAccess.P("@sales_id", salesId),
                    DataAccess.P("@payment_type", payby),
                    DataAccess.P("@payment_amount", payamount),
                    DataAccess.P("@change_amount", changeamount),
                    DataAccess.P("@due_amount", dueamount),
                    DataAccess.P("@dis", DiscountTotal),
                    DataAccess.P("@vat", vat),
                    DataAccess.P("@sales_time", salesdate),
                    DataAccess.P("@c_id", custId),
                    DataAccess.P("@emp_id", UserInfo.UserName),
                    DataAccess.P("@comment", Comment),
                    DataAccess.P("@Shopid", UserInfo.Shopid),
                    DataAccess.P("@ovdisrate", overalldisRate),
                    DataAccess.P("@vaterate", vatRate),
                    DataAccess.P("@SaleType", saleType));

                // 2. One sales_item row per cart line + stock decrease
                int rows = dgrvSalesItemList.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    string itemid   = dgrvSalesItemList.Rows[i].Cells[4].Value.ToString();
                    string itNam    = dgrvSalesItemList.Rows[i].Cells[0].Value.ToString();
                    double qty      = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[2].Value.ToString());
                    double Rprice   = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[1].Value.ToString());
                    double total    = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[3].Value.ToString());
                    double dis      = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[7].Value.ToString()); //discount rate
                    double taxapply = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[8].Value.ToString());
                    int kitchendisplay = Convert.ToInt32(dgrvSalesItemList.Rows[i].Cells[9].Value.ToString());

                    // Profit calculation
                    // Discount_amount = (Retail_price * discount) / 100          -- 49 * 3 / 100 = 1.47
                    // Retail_priceAfterDiscount = Retail_price - Discount_amount -- 49 - 1.47 = 47.53
                    // Profit = Retail_priceAfterDiscount - cost_price            -- 47.53 - 45 = 2.53
                    DataTable dt1 = tx.Query("select cost_price, discount from purchase where product_id = @id",
                                             DataAccess.P("@id", itemid));
                    if (dt1.Rows.Count == 0)
                        throw new Exception("Product " + itemid + " was not found in stock.");
                    double cost_price = ToDouble(dt1.Rows[0][0]);
                    double discount   = ToDouble(dt1.Rows[0][1]);

                    double Discount_amount = (Rprice * discount) / 100.00;
                    double Retail_priceAfterDiscount = Rprice - Discount_amount;
                    double Profit = Math.Round((Retail_priceAfterDiscount - cost_price), 2);

                    tx.Execute(" insert into sales_item (sales_id, itemName, Qty, RetailsPrice, Total, profit, sales_time, itemcode, discount, taxapply, status) " +
                               " values (@sales_id, @itemName, @Qty, @RetailsPrice, @Total, @profit, @sales_time, @itemcode, @discount, @taxapply, @status)",
                        DataAccess.P("@sales_id", salesId),
                        DataAccess.P("@itemName", itNam),
                        DataAccess.P("@Qty", qty),
                        DataAccess.P("@RetailsPrice", Rprice),
                        DataAccess.P("@Total", total),
                        DataAccess.P("@profit", Profit),
                        DataAccess.P("@sales_time", salesdate),
                        DataAccess.P("@itemcode", itemid),
                        DataAccess.P("@discount", dis),
                        DataAccess.P("@taxapply", taxapply.ToString()),
                        DataAccess.P("@status", kitchendisplay));

                    // Decrease stock quantity in purchase table
                    tx.Execute("update purchase set product_quantity = product_quantity - @qty where product_id = @id",
                        DataAccess.P("@qty", qty),
                        DataAccess.P("@id", itemid));
                }

                newId = salesId;
            });
            return newId;
        }
        #endregion

        //1. Direct sales and print Receipt
        private void btnPayment_Click(object sender, EventArgs e)
        {
            if (lblTotalPayable.Text == "00" || lblTotalPayable.Text == "0" || lblTotalPayable.Text == string.Empty)
            {
                MessageBox.Show("Sorry ! You don't have enough product in Item cart \n  Please Add to cart", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                try
                {
                    // Save payment + items + stock in one transaction
                    long newId = SaveSale(lblTotalPayable.Text, "0", "0", DateTime.Now.ToString("yyyy-MM-dd"), "Guest", null);
                    txtInvoice.Text = newId.ToString();

                    ///// // Open Print Invoice
                    parameter.autoprintid = "1";
                    ReceiptPrinter.Show(txtInvoice.Text);

                    dgrvSalesItemList.Rows.Clear();
                    DiscountCalculation();
                    vatcal();
                    this.tabPageSR_Payment.Parent = null; //Hide payment tab
                    btnCompleteSalesAndPrint.Enabled = false;
                    btnPayment.Enabled = false;
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save the sale.");
                }
            }

        }

        /////1.2 Open Payment Tab to receive amoount
        private void btnSalesCredit_Click(object sender, EventArgs e)
        {
            if (lblTotalPayable.Text == "00" || lblTotalPayable.Text == "0" || lblTotalPayable.Text == string.Empty)
            {
                MessageBox.Show("Sorry ! You don't have enough product in Item cart \n  Please Add to cart", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                //Open payment tab
                this.tabPageSR_Payment.Parent = this.tabSRcontrol; //show
                tabSRcontrol.SelectedTab = tabPageSR_Payment;

                DiscountCalculation();
                vatcal();  
            }
        }
 
        #region  Links
        // Call System Calculator
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // System.Diagnostics.Process.Start("Calc");
                SendKeys.SendWait(lblTotal.Text);
                Process p = new Process();
                p.StartInfo.FileName = "calc.exe";
                p.Start();
                p.WaitForInputIdle();

            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
       
        //--  new   8.1 version
        private void helplnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            parameter.helpid = "SR";
            HelpPage go = new HelpPage();
            go.MdiParent = this.ParentForm;
            go.Show();

            //SalesRagister.Currency_Shortcuts uc = new SalesRagister.Currency_Shortcuts();
            //uc.Dock = DockStyle.None;
            //panel1.Controls.Add(uc);
           // this.Controls.Add(uc);            
            
          //  tabControl1.SelectedTab = tabterminal;
        }

        ///ShortCut Keys
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Shift | Keys.C)) // Cash
            {
                btnPayment.PerformClick();  //Shift+P for Open Payment Page 
            }
            else if (keyData == (Keys.Control | Keys.Enter)) // Ctrl+Enter  // From vs 8.6 
            {
                btnPrintDirect.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.P)) // Ctrl+P   // From vs 8.6 
            {
                btnCompleteSalesAndPrint.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.S)) // Ctrl+S  
            {
                btnSaveOnly.PerformClick();
            }
            else if (keyData == (Keys.Shift | Keys.Delete)) // Shift + Del -> Suspen/clear all items
            {
                btnSuspend.PerformClick();
            }
            else if (keyData == (Keys.Control | Keys.B)) // Ctl + B Barcode Cursor
            {
                txtBarcodeReaderBox.Focus();
            }
            else if (keyData == (Keys.F4))  // Selected item delete
            {
                try
                {

                    foreach (DataGridViewRow row in dgrvSalesItemList.SelectedRows)
                    {
                        dgrvSalesItemList.Rows.RemoveAt(row.Index);
                    }
                    DiscountCalculation();
                    vatcal();
                    //txtDiscountRate.Text = "0";
                    LoadTotalDiscount();
                    LoadTotalTax();
                }
                catch (Exception exLog) { Logger.Error(exLog); }
            }
            else if (keyData == (Keys.F6)) // Increase item Qty
            {
                try
                {
                    double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                    int n = dgrvSalesItemList.CurrentCell.RowIndex;

                    double Rprice = Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[1].Value);
                    double Taxapply = Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[8].Value);

                    int QtyInc = Convert.ToInt32(dgrvSalesItemList.Rows[n].Cells[2].Value);
                    dgrvSalesItemList.Rows[n].Cells[2].Value = (QtyInc + 1);  //Qty Increase
                    dgrvSalesItemList.Rows[n].Cells[3].Value = Rprice * (QtyInc + 1);   // Total price
                    //  dgrvSalesItemList.Rows[n].Cells[4].Value = Itemid;                     



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
                    DiscountCalculation();
                    vatcal();
                }
                catch (Exception exLog) { Logger.Error(exLog); }
            }
            else if (keyData == (Keys.F7)) // Decrease item Qty
            {
                try
                {
                    int n = dgrvSalesItemList.CurrentCell.RowIndex;
                    if (Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[2].Value) > 1)
                    {
                        double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                        double Rprice = Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[1].Value);
                        double Taxapply = Convert.ToDouble(dgrvSalesItemList.Rows[n].Cells[8].Value);

                        int QtyInc = Convert.ToInt32(dgrvSalesItemList.Rows[n].Cells[2].Value);
                        dgrvSalesItemList.Rows[n].Cells[2].Value = (QtyInc - 1);  //Qty Increase
                        dgrvSalesItemList.Rows[n].Cells[3].Value = Rprice * (QtyInc - 1);   // Total price
                        //  dgrvSalesItemList.Rows[n].Cells[4].Value = Itemid;                     



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
                        DiscountCalculation();
                        vatcal();
                    }

                }
                catch (Exception exLog) { Logger.Error(exLog); }

            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion
             
        #region Text box Validatation 
        //Validation Overall Discount Rate
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
            catch (Exception exLog) { Logger.Error(exLog); } 
        }

        //Validation Paid amount
        private void txtPaidAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtPaidAmount.Text.ToString(), @"\.\d\d\d");

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
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        #endregion

        #region Payment receiver tab page calculation

        //paid amount Input Operation
        private void txtPaidAmount_TextChanged(object sender, EventArgs e)
        {
            if (lblTotalPayable.Text == "")
            {
                // MessageBox.Show("please insert Amount ");
            }
            else
            {
                try
                {
                    if (Convert.ToDouble(txtPaidAmount.Text) >= Convert.ToDouble(lblTotalPayable.Text))
                    {
                        double changeAmt = Convert.ToDouble(txtPaidAmount.Text) - Convert.ToDouble(lblTotalPayable.Text);
                        changeAmt = Math.Round(changeAmt, 2);
                        txtChangeAmount.Text = changeAmt.ToString();
                        txtDueAmount.Text = "0";
                        this.AcceptButton = btnPrintDirect;
                    }
                    if (Convert.ToDouble(txtPaidAmount.Text) <= Convert.ToDouble(lblTotalPayable.Text))
                    {
                        double changeAmt = Convert.ToDouble(lblTotalPayable.Text) - Convert.ToDouble(txtPaidAmount.Text);
                        changeAmt = Math.Round(changeAmt, 2);
                        txtDueAmount.Text = changeAmt.ToString();
                        txtChangeAmount.Text = "0";
                        this.AcceptButton = btnPrintDirect;
                    }

                }
                catch //(Exception exp)
                {
                    // MessageBox.Show(exp.Message);
                }

            }
        }


        // Sales cart page load
        private void tabPageSR_Counter_Enter(object sender, EventArgs e)
        {
            txtBarcodeReaderBox.Focus();
        }

        //Payment tab page load 
        private void tabPageSR_Payment_Enter(object sender, EventArgs e)
        {
            try
            {
                dtSalesDate.Format = DateTimePickerFormat.Custom;
                dtSalesDate.CustomFormat = "yyyy-MM-dd";


                //Customer Databind 
                string sqlCust = "select   DISTINCT  *   from tbl_customer where PeopleType = 'Customer'";
                DataTable dtCust = DataAccess.GetDataTable(sqlCust);
                ComboCustID.DataSource = dtCust;
                ComboCustID.DisplayMember = "Name";
                ComboCustID.Text = "Guest";

              // btnCompleteSalesAndPrint.Focus();
                btnPrintDirect.Focus();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
           
        }

        
        //2. Only save
        private void btnSaveOnly_Click(object sender, EventArgs e)
        {            
            if (txtPaidAmount.Text == "00" || txtPaidAmount.Text == "0" || txtPaidAmount.Text == string.Empty)
            {
                MessageBox.Show("Please insert paid amount. \n  If you want full due transaction \n Please insert 0.00 ", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                try
                {
                    // Save payment + items + stock in one transaction
                    long newId = SaveSale(lblTotalPayable.Text, txtChangeAmount.Text, txtDueAmount.Text, dtSalesDate.Text, txtCustName.Text, null);
                    txtInvoice.Text = newId.ToString();
                    MessageBox.Show("Successfully has been saved ");

                    //Clean Datagridview and Back to sales cart
                    dgrvSalesItemList.Rows.Clear();
                    DiscountCalculation();
                    vatcal();
                    this.tabPageSR_Payment.Parent = null; //Hide payment tab
                    tabSRcontrol.SelectedTab = tabPageSR_Counter;
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save the sale.");
                }
            }            
        }

        //3. Complete sale and Print Preview
        private void btnSplit_Click(object sender, EventArgs e)
        {
            decimal payable;
            if (!decimal.TryParse(lblTotalPayable.Text, out payable) || payable <= 0)
            {
                MessageBox.Show("Add items to the cart first.");
                return;
            }
            try
            {
                using (SalesRagister.SplitPaymentForm f = new SalesRagister.SplitPaymentForm(payable))
                {
                    if (f.ShowDialog() != DialogResult.OK) return;
                    long newId = SaveSale(lblTotalPayable.Text, f.ChangeAmount.ToString("0.00"), "0",
                                          dtSalesDate.Text, txtCustName.Text, f.Tenders);
                    txtInvoice.Text = newId.ToString();
                    parameter.autoprintid = "1";
                    ReceiptPrinter.Show(txtInvoice.Text);
                    dgrvSalesItemList.Rows.Clear();
                    DiscountCalculation();
                    vatcal();
                    this.tabPageSR_Payment.Parent = null;
                    tabSRcontrol.SelectedTab = tabPageSR_Counter;
                }
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not complete the split payment.");
            }
        }

        private void btnCompleteSalesAndPrint_Click(object sender, EventArgs e)
        {
            if (txtPaidAmount.Text == "00" || txtPaidAmount.Text == "0" || txtPaidAmount.Text == string.Empty)
            {
                MessageBox.Show("Please insert paid amount. \n  If you want full due transaction \n Please insert 0.00 ", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                try
                {
                    // Save payment + items + stock in one transaction
                    long newId = SaveSale(lblTotalPayable.Text, txtChangeAmount.Text, txtDueAmount.Text, dtSalesDate.Text, txtCustName.Text, null);
                    txtInvoice.Text = newId.ToString();

                    ///// // Open Print Invoice
                    parameter.autoprintid = "1";
                    ReceiptPrinter.Show(txtInvoice.Text);

                    //Clean Datagridview and Back to sales cart
                    dgrvSalesItemList.Rows.Clear();
                    DiscountCalculation();
                    vatcal();
                    this.tabPageSR_Payment.Parent = null; //Hide payment tab
                    tabSRcontrol.SelectedTab = tabPageSR_Counter;
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save the sale.");
                }
            }
        }


        //4 Comlete  sale and direct print
        private void btnPrintDirect_Click(object sender, EventArgs e)
        {
            if (txtPaidAmount.Text == "00" || txtPaidAmount.Text == "0" || txtPaidAmount.Text == string.Empty)
            {
                MessageBox.Show("Please insert paid amount. \n  If you want full due transaction \n Please insert 0.00 ", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                try
                {
                    // Save payment + items + stock in one transaction
                    long newId = SaveSale(lblTotalPayable.Text, "0", "0", DateTime.Now.ToString("yyyy-MM-dd"), "Guest", null);
                    txtInvoice.Text = newId.ToString();

                    ///// // Open Print Invoice
                    parameter.autoprintid = "1";
                    ReceiptPrinter.Show(txtInvoice.Text);

                    dgrvSalesItemList.Rows.Clear();
                    DiscountCalculation();
                    vatcal();
                    this.tabPageSR_Payment.Parent = null; //Hide payment tab
                    btnCompleteSalesAndPrint.Enabled = false;
                    btnPayment.Enabled = false;
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save the sale.");
                }
            }
        }

        //Back to Sales cart tab
        private void btnback_Click(object sender, EventArgs e)
        {  
            tabSRcontrol.SelectedTab = tabPageSR_Counter;
        }        

        //Customer filter
        private void ComboCustID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = "select ID from tbl_customer where Name = @name";
                DataTable dt1 = DataAccess.GetDataTable(sqlCmd, DataAccess.P("@name", ComboCustID.Text));
                if (dt1.Rows.Count > 0)
                    lblCustID.Text = dt1.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }


        #region //////////////////   Currency shortcuts value get
        public delegate void functioncall(string currencyvalue);
        public delegate void numvaluefunctioncall(string Numvalue);

        private event functioncall formFunctionPointer;
        private event numvaluefunctioncall numformFunctionPointer;

        private void Replicate(string currencyvalue)
        {
            if (currencyvalue == "XX") // All clear
            {
                txtPaidAmount.Text = "";
            }
            else if (currencyvalue == "BXC") //Backspace
            {
                if ((String.Compare(txtPaidAmount.Text, " ") < 0))
                {
                    txtPaidAmount.Text = txtPaidAmount.Text.Substring(0, txtPaidAmount.Text.Length - 1 + 1);
                }
                else
                {
                    txtPaidAmount.Text = txtPaidAmount.Text.Substring(0, txtPaidAmount.Text.Length - 1);
                }
                txtPaidAmount.Focus();
            }
            else
            {
                if (string.IsNullOrEmpty(txtPaidAmount.Text))
                {
                    txtPaidAmount.Text = "0.00";
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble(currencyvalue)).ToString();
                }
                else
                {
                    txtPaidAmount.Text = (Convert.ToDouble(txtPaidAmount.Text) + Convert.ToDouble(currencyvalue)).ToString();
                }
                txtPaidAmount.Focus();
            }
      
        }

        private void NumaricKeypad(string Numvalue)
        {
            txtPaidAmount.Text += Numvalue;
            txtPaidAmount.Focus();
        }
        #endregion

        #endregion

        public double CheckStockQty(string itemcode)
        {
            decimal totalstockQty = DataAccess.GetDecimal("SELECT product_quantity FROM purchase where product_id = @id",
                                                          DataAccess.P("@id", itemcode));
            return Convert.ToDouble(totalstockQty);
        }

        #region direct Print  // From vs 8.6

        private void PrintReceiptWithoutPrintDialog()
        {
            PrintDocument printDocument = new PrintDocument();
            printDocument.DocumentName = "Receipt_direct_" + txtInvoice.Text + "_" + DateTime.Now.ToString("yyyyMMddhhmmss");
            printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            printDocument.Print();
        }

        void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics graphic = e.Graphics;

            Font font = new Font("Courier New", 10);

            float fontHeight = font.GetHeight();

            int startX = 10;
            int startY = 10;
            int offset = 13;

            string sql = " SELECT  sp.sales_id AS salesid, sp.payment_type AS paytype, sp.payment_amount AS Payamount, " +
                            " sp.change_amount AS charAmt, sp.due_amount AS due, sp.dis, sp.vat, sp.sales_time AS s_time,  " +
                            " sp.c_id AS custID, sp.emp_id AS empID, sp.comment AS Note, sp.TrxType, si.sales_id,si.item_id,  " +
                            " si.itemName, si.Qty, si.RetailsPrice, si.Total,si.profit, si.sales_time , sp.Shopid, tl.*, c.* ,  " +
                            " CASE     " +
                            " WHEN si.taxapply = 1 THEN 'TX'  " +
                            " ELSE ''  " +
                            " END 'TaxApply'  " +
                            " FROM            sales_payment sp " +
                            " INNER JOIN   sales_item si " +
                            " ON sp.sales_id  = si.sales_id " +
                            " INNER JOIN tbl_terminalLocation tl " +
                            " ON sp.Shopid  = tl.Shopid " +
                            " INNER JOIN tbl_customer c " +
                            " ON  sp.c_id  = c.ID " +
                            " Where sp.sales_id  = @id ";
            DataTable dt = DataAccess.GetDataTable(sql, DataAccess.P("@id", txtInvoice.Text));

            string storename = dt.Rows[0]["companyname"].ToString(); //"Doglus Coffee Shop"
            string Address = dt.Rows[0]["location"].ToString(); ///// "34 Dandus street ON M7H R5T CA"
            string Phone = dt.Rows[0]["phone"].ToString();  //// "+1(416) 111 1234"
            string vatregino = dt.Rows[0]["vatregino"].ToString(); //// "803060284RT0003"
            string Salesid = "Invoice " + dt.Rows[0]["salesid"].ToString() + "-" + dt.Rows[0]["empID"].ToString();

            offset = offset + (int)fontHeight;
            graphic.DrawString(storename, new Font("Courier New", 18), new SolidBrush(Color.Black), startX, offset);

            offset = offset + (int)fontHeight + 9;
            RectangleF rectFaddr = new RectangleF(startX, offset, 180, 55);
            graphic.DrawString("".PadRight(3) + Address, new Font("Courier New", 10), new SolidBrush(Color.Black), rectFaddr);

            offset = offset + (int)fontHeight + 7;  // +Convert.ToInt32(rectFaddr.Height);
            graphic.DrawString("".PadRight(3) + "TEL:" + Phone, new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight + 4;
            graphic.DrawString("".PadRight(3) + "HST#:" + vatregino, new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight + 5;
            graphic.DrawString(DateTime.Now.ToString(), new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight + 3;
            graphic.DrawString(Salesid, new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight + 32;
            //graphic.DrawString("-----------------------", font, new SolidBrush(Color.Black), startX, startY + offset);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                string productdescription = dt.Rows[i]["itemname"].ToString(); // "Product-" + i.ToString();
                string productTotal = dt.Rows[i]["total"].ToString();  /// String.Format("{0:c}", "$20");
                string productQty = dt.Rows[i]["Qty"].ToString();
                string product1Line = productQty + "X " + productdescription + "  " + productTotal;

                // graphic.DrawString(product1Line, font, new SolidBrush(Color.Black), startX, startY + offset);
                //offset = offset + (int)fontHeight + 5;

                RectangleF rectProdline = new RectangleF(startX, offset, 215, 57);
                graphic.DrawString(product1Line, font, new SolidBrush(Color.Black), rectProdline);
                offset = offset + (int)fontHeight + 13;
            }

            decimal dis = Convert.ToDecimal(dt.Rows[0]["dis"].ToString());
            decimal TAX = Convert.ToDecimal(dt.Rows[0]["vat"].ToString());
            decimal Subtotal = Convert.ToDecimal(dt.Rows[0]["Payamount"].ToString()) - (TAX);
            string payment_amount = dt.Rows[0]["Payamount"].ToString();
            decimal Change = Convert.ToDecimal(dt.Rows[0]["charAmt"].ToString());
            decimal due = Convert.ToDecimal(dt.Rows[0]["due"].ToString());
            string Payment = dt.Rows[0]["paytype"].ToString();
            string footermsg = dt.Rows[0]["footermsg"].ToString();
            //if(footermsg.Length > 75)
            //{
            //    footermsg = footermsg.Substring(0, 28) + "\n" + footermsg.Substring(29, 30) + "\n" + footermsg.Substring(60, 14);  ///"THANK YOU & COME BACK"
            //}
            //else
            //{
            //    footermsg = "THANK YOU & COME BACK";
            //}

            graphic.DrawString("------------------------", font, new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight + 5;
            graphic.DrawString("Sub-Total ".PadRight(15) + String.Format("{0:c}", Subtotal.ToString()), font, new SolidBrush(Color.Black), startX, startY + offset);


            offset = offset + (int)fontHeight + 5;
            graphic.DrawString("TAX ".PadRight(15) + String.Format("{0:c}", TAX.ToString()), font, new SolidBrush(Color.Black), startX, startY + offset);

            //  offset = offset + (int)fontHeight + 5;
            // graphic.DrawString("Total ".PadRight(8) + String.Format("{0:c}", payment_amount), new Font("Courier New", 15, FontStyle.Bold), new SolidBrush(Color.Black), startX, startY + offset);

            offset = offset + (int)fontHeight + 15;
            RectangleF rectTotal = new RectangleF(startX, offset, 215, 127);
            graphic.DrawString("Total ".PadRight(8) + String.Format("{0:c}", payment_amount), new Font("Courier New", 14, FontStyle.Bold), new SolidBrush(Color.Black), rectTotal);

            if (Change > 0)
            {
                offset = offset + (int)fontHeight + 17;
                graphic.DrawString("Change  ".PadRight(15) + String.Format("{0:c}", Change.ToString()), new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);
            }

            if (due > 0)
            {
                offset = offset + (int)fontHeight + 17;
                graphic.DrawString("Due  ".PadRight(15) + String.Format("{0:c}", due.ToString()), new Font("Courier New", 10), new SolidBrush(Color.Black), startX, startY + offset);
            }


            if (dis > 0)
            {
                offset = offset + (int)fontHeight + 17;
                graphic.DrawString("Discount ".PadRight(15) + String.Format("{0:c}", dis.ToString()), font, new SolidBrush(Color.Black), startX, startY + offset);
            }
 
            offset = offset + (int)fontHeight + 5;
            graphic.DrawString("Payment ".PadRight(15) + String.Format("{0:N}", Payment), font, new SolidBrush(Color.Black), startX, startY + offset);


            offset = offset + (int)fontHeight + 13;
            RectangleF rectF1 = new RectangleF(startX, startY + offset, 210, 106);
            graphic.DrawString(footermsg, font, new SolidBrush(Color.Black), rectF1);

            //offset = offset + Convert.ToInt32(rectF1.Height) + 7;
            //graphic.DrawString(footermsg, font, new SolidBrush(Color.Black), startX, startY + offset);
            //////Logo Here Draw icon to screen.
            ////offset = offset + (int)fontHeight + 7;
            ////e.Graphics.DrawIcon(new Icon("Rockettheme-Ecommerce-Sale.ico"), startX, startY + offset);


        }

        #endregion

        private void DirectPrint()
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            m_currentPageIndex = 0;
            printDoc.Print();
        }

        private void txtRSDisc_TextChanged(object sender, EventArgs e)
        {
            try
            {

                Double res = Convert.ToDouble(txtRSDisc.Text) / Convert.ToDouble(lblTotal.Text) * 100;
                txtDiscountRate.Text = Convert.ToString(Math.Round(res, 2));
                //  lbloveralldiscount.Text = lblTotalDisCount.Text;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
            catch (Exception exLog) { Logger.Error(exLog); }
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
        private void lst_items_Click_1(object sender, EventArgs e)
        {
            try
            {
                // int selectionindex = lst_items.inde();
                string val1 = lst_items.SelectedItems[0].ToString();
                string a = GetNumbers(val1);



                txtBarcodeReaderBox.Text = a;

            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
    }
}
