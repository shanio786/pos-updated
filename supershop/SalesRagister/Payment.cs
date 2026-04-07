using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace supershop
{
    public partial class Payment : Form
    {
        public Payment(object dataSource,string total,string subtotal,  string TotalAmount , string discount , string vat , string DiscountRate , string VatRate , string invoiceNo , string totalitems )
        {
            InitializeComponent();
            dgrvSalesItemList.DataSource = dataSource;
            lblTotalPayable.Text = TotalAmount;
            lblTotal.Text   = total; 
            lblsubtotal.Text = subtotal;
            lblTotalPayable.Text = TotalAmount;
            lblTotalDisCount.Text  = discount; 
            lblTotalVAT.Text  = vat;
            txtDiscountRate.Text = DiscountRate;
            txtVATRate.Text = VatRate;
            txtPaidAmount.Text = TotalAmount;
            txtInvoice.Text = invoiceNo;
            lblTotalItems.Text = totalitems;
            lbluser.Text = UserInfo.UserName;
            txtPaidAmount.Focus();           
        }


        private void Payment_Load(object sender, EventArgs e)
        {
            dtSalesDate.Format = DateTimePickerFormat.Custom;
            dtSalesDate.CustomFormat = "yyyy-MM-dd";
            try
            {
                //Customer Info
                string sqlCust = "select   DISTINCT  *   from tbl_customer where PeopleType = 'Customer'";
                DataAccess.ExecuteSQL(sqlCust);
                DataTable dtCust = DataAccess.GetDataTable(sqlCust);
                ComboCustID.DataSource = dtCust;
                ComboCustID.DisplayMember = "Name";
                ComboCustID.Text = "Guest";
                
                //this.dgrvSalesItemList.Columns.Add("itm", "Items Name");
                //this.dgrvSalesItemList.Columns.Add("Am", "Price");
                //this.dgrvSalesItemList.Columns.Add("Qty", "Qty");
                //this.dgrvSalesItemList.Columns.Add("Total", "Total");
                //this.dgrvSalesItemList.Columns.Add("ID", "ID");
            }
            catch
            {
            }
        }

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
                    }
                    if (Convert.ToDouble(txtPaidAmount.Text) <= Convert.ToDouble(lblTotalPayable.Text))
                    {
                        double changeAmt = Convert.ToDouble(lblTotalPayable.Text) - Convert.ToDouble(txtPaidAmount.Text);
                        changeAmt = Math.Round(changeAmt, 2);
                        txtDueAmount.Text = changeAmt.ToString();
                        txtChangeAmount.Text = "0";
                    }

                }
                catch //(Exception exp)
                {
                    // MessageBox.Show(exp.Message);
                }

            }
        }

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
            catch
            {
            } 
        }

        #region Data save
        /// //// Add sales item  ////////////Store into sales_item table //////////
        public bool sales_item()
        {             
            int rows = dgrvSalesItemList.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                string SalesDate = dtSalesDate.Text;
                string trno     = txtInvoice.Text;                 
                string itemid   = dgrvSalesItemList.Rows[i].Cells[4].Value.ToString();
                string itNam    = dgrvSalesItemList.Rows[i].Cells[0].Value.ToString();
                double qty      = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[2].Value.ToString());
                double Rprice   = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[1].Value.ToString());
                double total    = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[3].Value.ToString());
                double dis      = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[7].Value.ToString()); //discount rate
                double taxapply = Convert.ToDouble(dgrvSalesItemList.Rows[i].Cells[8].Value.ToString());
                int kitchendisplay = Convert.ToInt32(dgrvSalesItemList.Rows[i].Cells[9].Value.ToString());

               
                
                // =================================Start=====  Profit calculation =============== Start ========= 
                // Discount_amount = (Retail_price * discount) / 100                    -- 49 * 3 / 100 = 1.47
                // Retail_priceAfterDiscount = Retail_price - Discount_amount           -- 49 - 1.47 = 47.53
                // Profit = (Retail_priceAfterDiscount * QTY )   - (cost_price * qty);  ---( 47.53 * 1 ) - ( 45 * 1) = 2.53

                string sqlprofit = "Select cost_price , discount  from  purchase  where product_id  = '" + itemid + "'";
                DataAccess.ExecuteSQL(sqlprofit);
                DataTable dt1 = DataAccess.GetDataTable(sqlprofit);

                double cost_price   = Convert.ToDouble(dt1.Rows[0].ItemArray[0].ToString());
                double discount     = Convert.ToDouble(dt1.Rows[0].ItemArray[1].ToString());

                double Discount_amount = (Rprice * discount) / 100.00;
                double Retail_priceAfterDiscount = Rprice - Discount_amount;
                double Profit = Math.Round((Retail_priceAfterDiscount  - cost_price),2); // old calculation (Retail_priceAfterDiscount * qty) - (cost_price * qty);
                // =================================Start=====  Profit calculation =============== Start =========  


                string sql1 = " insert into sales_item (sales_id,itemName,Qty,RetailsPrice,Total, profit,sales_time, itemcode , discount, taxapply, status) " + 
                              " values ('" + trno + "', '" + itNam + "', '" + qty + "', '" + Rprice + "', '" +  total + "', '" + Profit + "', " +
                              " '" + SalesDate + "','" + itemid + "','" + dis + "','" + taxapply + "','" + kitchendisplay + "')";
                DataAccess.ExecuteSQL(sql1);

                //update quantity Decrease from Stock Qty |  purchase Table
                if (txtInvoice.Text == "")
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
                    double product_quantity = Convert.ToDouble(dtUqty.Rows[0].ItemArray[0].ToString())  -  qtyupdate ;

                    string sql =    " update purchase set " +
                                    " product_quantity = '" + product_quantity + "' " +
                                    " where product_id = '" + itemids + "' ";
                    DataAccess.ExecuteSQL(sql);
                }

            }
            return true;        

        }

        /// //// Payment items Add  ///////////Store into Sales_payment table //////// 
        public void payment_item()
        {
            string trno             = txtInvoice.Text;
            string payamount        = lblTotalPayable.Text;
            string changeamount     = txtChangeAmount.Text;
            string due              = txtDueAmount.Text;
            string vat              = lblTotalVAT.Text;
            string DiscountTotal    = lblTotalDisCount.Text;
            string Comment          = ComboCustID.Text + "  " + txtCustName.Text;
            string overalldisRate   = txtDiscountRate.Text;
            string vatRate         = txtVATRate.Text;
            
            string sql1 =   " insert into sales_payment (sales_id, payment_type,payment_amount,change_amount,due_amount, dis, vat, " +
                            " sales_time,c_id,emp_id,comment, TrxType, Shopid , ovdisrate , vaterate ) " + 
                            "  values ('" + txtInvoice.Text + "','" + CombPayby.Text + "', '" + payamount + "', '" + changeamount + "', " +
                            " '" + due + "', '" + DiscountTotal + "', '" + vat + "', '" + dtSalesDate.Text + "', '" + lblCustID.Text + "', " +
                            "  '" + UserInfo.UserName + "','" + Comment + "','POS','" + UserInfo.Shopid + "' , '" + overalldisRate + "' , '" + vatRate + "' )";
            DataAccess.ExecuteSQL(sql1);
        }

        private void btnCompleteSalesAndPrint_Click(object sender, EventArgs e)
        {
            //DialogResult result = MessageBox.Show("Do you want to Complete Sale and Print?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

           // if (result == DialogResult.Yes)
         //   {
                if (txtPaidAmount.Text == "00" || txtPaidAmount.Text == "0" || txtPaidAmount.Text == string.Empty)
                {
                    MessageBox.Show("Sorry ! You don't have enough product in Item cart \n  Please Add to cart", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                }
                //  else if (Convert.ToInt32(txtInvoice.Text) >= 53)
                // {
                //   MessageBox.Show("Sorry ! Demo version has limited transaction \n Please buy it \n contact at : citkar@live.com \n https://goo.gl/Hs7XsD", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                //  }
                else
                {
                    try
                    {
                        //Save payment info into sales_payment table
                        payment_item();

                        ///// save single item 
                        sales_item();

                        btnCompleteSalesAndPrint.Enabled = false;
                        btnSaveOnly.Enabled = false;

                        ///// // Open Print Invoice
                        parameter.autoprintid = "1";
                        POSPrintRpt go = new POSPrintRpt(txtInvoice.Text);
                        go.ShowDialog();

                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            //}
        }
        
        private void btnSaveOnly_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Complete this transaction?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                if (txtPaidAmount.Text == "00" || txtPaidAmount.Text == "0" || txtPaidAmount.Text == string.Empty)
                {
                    MessageBox.Show("Sorry ! You don't have enough product in Item cart \n  Please Add to cart", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                }
                //else if (Convert.ToInt32(txtInvoice.Text) >= 53)
                //{
                //    MessageBox.Show("Sorry ! Demo version has limited transaction \n Please buy it \n contact at : citkar@live.com \n https://goo.gl/Hs7XsD", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                //}
                else
                {
                    try
                    {

                        //Save payment info into sales_payment table
                        payment_item();

                        ///// save single item 
                        sales_item();


                        btnCompleteSalesAndPrint.Enabled = false;
                        btnSaveOnly.Text = "Done";
                        btnSaveOnly.Enabled = false;
                        //MessageBox.Show("Successfully Done");

                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show(exp.Message);
                    }
                }
            }
        }
        #endregion

        private void ComboCustID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = "Select ID from  tbl_customer  where Name  = '" + ComboCustID.Text + "'";
                DataAccess.ExecuteSQL(sqlCmd);
                DataTable dt1 = DataAccess.GetDataTable(sqlCmd);
                lblCustID.Text = dt1.Rows[0].ItemArray[0].ToString();
            }
            catch
            {
            }
        }

        //Invoice Id Auto increment
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                string sql = "select  sales_id  from sales_payment order by sales_id desc";
                DataTable dt = DataAccess.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    double id = Convert.ToDouble(dt.Rows[0].ItemArray[0].ToString()) + 1;
                    txtInvoice.Text = Convert.ToString(Convert.ToInt32(id));
                    //
                }
                else
                {
                    double id = 1;
                    txtInvoice.Text = Convert.ToString(Convert.ToInt32(id));
                  //  prgressbar();
                }
            }
            catch
            {

            }
        }

         

       
                       
    }
}
