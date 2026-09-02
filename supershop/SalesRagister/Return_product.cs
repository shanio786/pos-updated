using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Diagnostics;


namespace supershop
{
    public partial class Return_product : Form
    {
        bool upDateFlag = true;
        public Return_product()
        {
            InitializeComponent();
            lblEmpID.Text = UserInfo.UserName; 
        }
        private void ClearForm()
        {
            txtbarcodeinputer.Text = string.Empty;            
          
        }
         

        // Total VAT , and Discount Calculation
          private void total()
          {
              // // subtotal without dis vat sum 
              double totalsum = 0;
              for (int i = 0; i < dtgrdviewReturnItem.Rows.Count; ++i)
              {
                  totalsum += Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells["Total"].Value);
              }
              lblTotal.Text = totalsum.ToString();
              double total = Convert.ToDouble(totalsum.ToString());

              ////  Discount amount sum Calculation              
              double DisCount = 0.00;
              for (int i = 0; i < dtgrdviewReturnItem.Rows.Count; ++i)
              {
                  DisCount += Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells["disamt"].Value);
              }
              DisCount = Math.Round(DisCount, 2);
              lbldis.Text = DisCount.ToString();

              //Overall sold discount / counter discount calculation
               double Discountvalue = Convert.ToDouble(txtDiscountRate.Text) ;
              double subtotal = Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(lbldis.Text); // total - item discount  100 - 5 = 95        
              double totaldiscount = (subtotal * Discountvalue) / 100;  //Counter discount  // 95 * 5 /100 = 4.75  

              double disPlusOverallDiscount = totaldiscount + Convert.ToDouble(lbldis.Text); // 4.75 + 5 = 9.75
              disPlusOverallDiscount = Math.Round(disPlusOverallDiscount, 2);
              lbloveralldiscount.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75
                if (upDateFlag)
                {
                    lblOverallDiscountOrignal.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75

                }
            double subtotalafteroveralldiscount = subtotal - totaldiscount; // 95 - 4.75 = 90.25
              subtotalafteroveralldiscount = Math.Round(subtotalafteroveralldiscount, 2);
              lblsubtotal.Text = subtotalafteroveralldiscount.ToString();
              

              ////VAT Calculation              
              double VAT = 0.00;
              for (int i = 0; i < dtgrdviewReturnItem.Rows.Count; ++i)
              {
                  VAT += Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells["taxamt"].Value);
              }
              VAT = Math.Round(VAT, 2);
              lblvat.Text = VAT.ToString();

             // double Subtotal = total - DisCount;
              double sum = subtotalafteroveralldiscount + VAT;
              sum = Math.Round(sum, 2);            
              lblTotalReturn.Text = sum.ToString();        
              txtReturnAmount.Text = lblTotalReturn.Text;
          }
               

          private void Return_product_Load(object sender, EventArgs e)
          {
              try
              {
                  dtReturnDate.Format = DateTimePickerFormat.Custom;
                  dtReturnDate.CustomFormat = "yyyy-MM-dd";

                  txtVATRate.Text = vatdisvalue.vat;
                  //txtDisRate.Text = vatdisvalue.dis;
                  // // Add new Colunm header Name
                  // this.dtgrdviewReturnItem.Columns.Add("itm", "Items Name");
                  // this.dtgrdviewReturnItem.Columns.Add("Am", "Price");
                  // this.dtgrdviewReturnItem.Columns.Add("Qty", "Quantity");
                  // this.dtgrdviewReturnItem.Columns.Add("Total", "Total");
                  // this.dtgrdviewReturnItem.Columns.Add("ID", "ID");
                   

                  DataGridViewButtonColumn del = new DataGridViewButtonColumn();
                  dtgrdviewReturnItem.Columns.Add(del);
                  del.HeaderText = "Del";
                  del.Text = "X";
                  del.Name = "del";
                  del.ToolTipText = "Delete item";
                  del.UseColumnTextForButtonValue = true;


                  DataGridViewButtonColumn minus = new DataGridViewButtonColumn();
                  dtgrdviewReturnItem.Columns.Add(minus);
                  minus.HeaderText = "Dec";
                  minus.Text = "-";
                  minus.Name = "minus";
                  minus.ToolTipText = "minus Item Qty";
                  minus.UseColumnTextForButtonValue = true;


                  


                  //Customer Info
                  string sqlCust = "select   DISTINCT  *   from tbl_customer where PeopleType = 'Customer'";
                  DataTable dtCust = DataAccess.GetDataTable(sqlCust);
                  ComboCustID.DataSource = dtCust;
                  ComboCustID.DisplayMember = "Name";
                   ComboCustID.ValueMember = "ID";
                 //  ComboCustID.Text = "Guest";
              }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message);
              }

          }

      
          private void disDecrease_Click(object sender, EventArgs e)  // // vat decrease
          {             
              if (txtReturnAmount.Text == "")
              {
                  MessageBox.Show("Please Add at least One Item");
              }
              else
              {
                  double Discountvalue = Convert.ToDouble(txtDiscountRate.Text) - 1;
                  txtDiscountRate.Text = Discountvalue.ToString();
                  double subtotal = Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(lbldis.Text); // total - item discount  100 - 5 = 95        
                  double totaldiscount = (subtotal * Discountvalue) / 100;  //Counter discount  // 95 * 5 /100 = 4.75  
                  double disPlusOverallDiscount = totaldiscount + Convert.ToDouble(lbldis.Text); // 4.75 + 5 = 9.75
                  disPlusOverallDiscount = Math.Round(disPlusOverallDiscount, 2);
                  lbloveralldiscount.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75

                  double subtotalafteroveralldiscount = subtotal - totaldiscount; // 95 - 4.75 = 90.25
                  subtotalafteroveralldiscount = Math.Round(subtotalafteroveralldiscount, 2);
                  lblsubtotal.Text = subtotalafteroveralldiscount.ToString();

                  double payable = subtotalafteroveralldiscount + Convert.ToDouble(lblvat.Text);
                  payable = Math.Round(payable, 2);
                  lblTotalReturn.Text = payable.ToString();
                   
                  txtReturnAmount.Text = lblTotalReturn.Text;
              }
          }

          private void disIncreasebtn_Click(object sender, EventArgs e)   // Discount Increase 
          {              
              if (txtReturnAmount.Text == "")
              {
                  MessageBox.Show("Please Add at least One Item");
              }
              else
              {
                  double Discountvalue = Convert.ToDouble(txtDiscountRate.Text) + 1;
                  txtDiscountRate.Text = Discountvalue.ToString();
                  double subtotal = Convert.ToDouble(lblTotal.Text) - Convert.ToDouble(lbldis.Text); // total - item discount  100 - 5 = 95        
                  double totaldiscount = (subtotal * Discountvalue) / 100;  //Counter discount  // 95 * 5 /100 = 4.75  

                  double disPlusOverallDiscount = totaldiscount + Convert.ToDouble(lbldis.Text); // 4.75 + 5 = 9.75
                  disPlusOverallDiscount = Math.Round(disPlusOverallDiscount, 2);
                  lbloveralldiscount.Text = disPlusOverallDiscount.ToString();  // Overall discount 9.75

                  double subtotalafteroveralldiscount = subtotal - totaldiscount; // 95 - 4.75 = 90.25
                  subtotalafteroveralldiscount = Math.Round(subtotalafteroveralldiscount, 2);
                  lblsubtotal.Text = subtotalafteroveralldiscount.ToString();

                  double payable = subtotalafteroveralldiscount + Convert.ToDouble(lblvat.Text);
                  payable = Math.Round(payable, 2);
                  lblTotalReturn.Text = payable.ToString();

                  txtReturnAmount.Text = lblTotalReturn.Text;
                  //double vatvalue = Convert.ToDouble(txtVATRate.Text) + 1;
                  //txtVATRate.Text = vatvalue.ToString();
                //  total();
              }
          }



        /// //// Add/insert Return items  //////////////////////
        public void Return_item()
        {
            int rows = dtgrdviewReturnItem.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                //ItemName, RetailsPrice, Qty, Total , item_id
                string itemName = dtgrdviewReturnItem.Rows[i].Cells["ItemName"].Value.ToString();
                double RetailsPrice = Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells["RetailsPrice"].Value.ToString());
                double Qty = Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells["Qty"].Value.ToString());
                double Total = Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells["Total"].Value.ToString());
                double disamt = Convert.ToDouble(lbloveralldiscount.Text); // Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells[5].Value.ToString());
                double vatamt = Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells["taxamt"].Value.ToString());
                string itemcode = dtgrdviewReturnItem.Rows[i].Cells["itemcode"].Value.ToString(); //itemcode
                string SoldID = dtgrdviewReturnItem.Rows[i].Cells["item_id"].Value.ToString();  //Single sales item id
                string return_time = dtReturnDate.Text;
                string InvoiceNo = txtInvoiceNo.Text;
                string emp = lblEmpID.Text;
               
                double itemDisForReturn =  Convert.ToDouble(dtgrdviewReturnItem.Rows[i].Cells[5].Value.ToString());
                double disRate = Convert.ToDouble(txtDiscountRate.Text);
                double discountPerItem = (disRate * Total) / 100;

                disamt = Convert.ToDouble(lblOverallDiscountOrignal.Text) - Convert.ToDouble(lbloveralldiscount.Text);
                
                //if (Convert.ToDouble(txtNewDiscountOnReturn.Text) > 0 && txtNewDiscountOnReturn.Text != "")
                //{
                //    disamt = Convert.ToDouble(txtNewDiscountOnReturn.Text);
                //}
                string sql1 = " insert into return_item (item_id, itemName, Qty, RetailsPrice, Total, return_time, custno, emp, SoldInvoiceNo, Comment, disamt , vatamt,Shopid) " +
                                " values ('" + itemcode + "', '" + itemName + "', '" + Qty + "', '" + RetailsPrice + "' , '" + Total + "', '" + return_time + "',   " +
                                " '" + lblCustID.Text + "', '" + emp + "' , '" + InvoiceNo + "', '" + txtComment.Text + "', '" + discountPerItem + "', '" + vatamt + "','"+UserInfo.Shopid+"')";               
                  DataAccess.ExecuteSQL(sql1);
 


                  // Update Quantity | Increase Quantity to Purchase table 
                  string sqlupdateQty = "select product_quantity  from purchase where product_id = '" + itemcode + "'";
                  DataTable dtUqty = DataAccess.GetDataTable(sqlupdateQty);
                  double product_quantity = Convert.ToDouble(dtUqty.Rows[0].ItemArray[0].ToString()) + Qty;

                  string sql = " update purchase set " +
                                  " product_quantity = '" + product_quantity + "'  where product_id = '" + itemcode + "' ";
                  DataAccess.ExecuteSQL(sql);

                string sqlSalePsyment = " update sales_payment set " +
                          " dis = '" + disamt + "'  where sales_id = '" + InvoiceNo + "' ";
                DataAccess.ExecuteSQL(sqlSalePsyment);

                // Decrease Sales Item into Sales_item table
                //Update sales_item Qty , status . Status 1 = Sold 2 = Sold item has been returned
                string sqlSalesQTY = " select Qty from sales_item  where item_id = '" + SoldID + "' ";
                  DataTable dtSalesQTY = DataAccess.GetDataTable(sqlSalesQTY);
                  double SalesQTY = Convert.ToDouble(dtSalesQTY.Rows[0].ItemArray[0].ToString()) - Qty;
                  double totalsale = SalesQTY * RetailsPrice;
                 
                  string sqlSIstatus =  " update sales_item set " +
                                     //   " Qty = (select Qty from sales_item  where item_id = '" + SoldID + "' ) - '" + Qty + "' , " +
                                     //   " Total  = ((select Qty from sales_item  where item_id = '" + SoldID + "' )  - '" + Qty + "' )  * RetailsPrice   " + 
                                        " Qty = '" + SalesQTY + "' , Total  = '" + totalsale + "',discount='" + lbloveralldiscount.Text + "'    " +
                                        "    where item_id = '" + SoldID + "' ";
                  DataAccess.ExecuteSQL(sqlSIstatus);
              }
     
                            
          }

          private void ClearForm2()
          {
              Return_product go = new Return_product();
              go.MdiParent = this.ParentForm;
              go.Show();
              this.Close();
          }

          private void ReturnSave_Click(object sender, EventArgs e)
          {
              DialogResult result = MessageBox.Show("Do you want to Complete Return ?\n\n -To change Qty edit Qty cell ", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

              if (result == DialogResult.Yes)
              {
                  if (txtReturnAmount.Text == "" || txtInvoiceNo.Text == ""  || lblTotalReturn.Text == "0")
                  {
                      MessageBox.Show("Please Insert  Product and Sold item Invoice / Receipt No ");                  
                  }
                  else
                  {
                     try
                      {
                          Return_item();
                        //  SubtractCredit();
                          MessageBox.Show("Successfully Returned Items  \n   ....... ", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);                         
                          ClearForm2();                

                      }
                      catch (Exception exp)
                      {                         
                         MessageBox.Show(exp.Message);
                      }
                  }
              }
          }
        
          private void ComboCustID_SelectedIndexChanged(object sender, EventArgs e)
          {
              CustomerID();
          }

          public void CustomerID()
          {
              try
              {
                  string sqlCmd = "Select ID from  tbl_customer  where Name  = '" + ComboCustID.Text + "'";
                  DataTable dt1 = DataAccess.GetDataTable(sqlCmd);

                  lblCustID.Text = dt1.Rows[0].ItemArray[0].ToString();
              }
              catch (Exception exLog) { Logger.Error(exLog); }
          }

          public void salePaymentinfo()
          {
              try
              {
                  string sqlCmd =   " Select  sales_id , change_amount , due_amount , dis, vat , sales_time , " +
                                    " c_id, emp_id , comment , TrxType, ShopId , payment_type , payment_amount, ovdisrate, vaterate " +
                                    "  from  sales_payment  where sales_id  = '" + txtbarcodeinputer.Text + "'";
                  DataTable dt = DataAccess.GetDataTable(sqlCmd);
                  for (int i = 0; i < dt.Rows.Count; i++)
                  {

                      DataRow dataReader = dt.Rows[i];
                      txtDiscountRate.Text = dataReader["ovdisrate"].ToString();
                      txtVATRate.Text = dataReader["vaterate"].ToString(); 

                      lblDue.Text       = dataReader["due_amount"].ToString();
                      lblChange.Text    = dataReader["change_amount"].ToString();
                      lblsalestime.Text = dataReader["sales_time"].ToString();
                      lbltrxType.Text   = dataReader["TrxType"].ToString();
                      lblShopid.Text    = dataReader["ShopId"].ToString();
                      lblNote.Text      = dataReader["comment"].ToString();
                     // double Ovdiscount = Convert.ToDouble(dataReader["dis"].ToString());
                     // lbloveralldiscount.Text =  Math.Round(Ovdiscount, 2).ToString();
                      lblCustID.Text            = dataReader["c_id"].ToString();
                      lblSalesby.Text           = dataReader["emp_id"].ToString();
                      lblpaytype.Text           = dataReader["payment_type"].ToString();
                      double Paid               = Convert.ToDouble(dataReader["payment_amount"].ToString()) - Convert.ToDouble(dataReader["due_amount"].ToString());
                      lblPaidAmount.Text        = Paid.ToString();

                      ComboCustID.SelectedValue = dataReader["c_id"].ToString(); 
                       
                      
                  }
                
              }
              catch (Exception exLog) { Logger.Error(exLog); }
          }
 

          private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
          {
              try
              {
                  // Delete items From Gridview
                  if (e.ColumnIndex == dtgrdviewReturnItem.Columns["del"].Index && e.RowIndex >= 0)
                  {
                      foreach (DataGridViewRow row2 in dtgrdviewReturnItem.SelectedRows)
                      {
                          DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                          if (result == DialogResult.Yes)
                          {
                            upDateFlag = false;
                              if (!row2.IsNewRow)
                                  dtgrdviewReturnItem.Rows.Remove(row2);
                              total();                           
                          }
                      }                    
                  }

                  // Decrease Item Quantity  -- Add new from 8.3.2
                  if (e.ColumnIndex == dtgrdviewReturnItem.Columns["minus"].Index && e.RowIndex >= 0)
                  {
                      foreach (DataGridViewRow row in dtgrdviewReturnItem.SelectedRows)
                      {
                          if (Convert.ToDouble(row.Cells["Qty"].Value) > 1)
                          {
                              //// Decrease by 1 
                              double qtySum = Convert.ToDouble(row.Cells["Qty"].Value) - 1;
                              row.Cells["Qty"].Value = qtySum;

                              double qty = Convert.ToDouble(row.Cells["Qty"].Value);
                              double Rprice = Convert.ToDouble(row.Cells["RetailsPrice"].Value);
                              double disrate = Convert.ToDouble(row.Cells["discount"].Value);
                              double Taxrate = Convert.ToDouble(vatdisvalue.vat);

                              //// show total price   Qty  * Rprice
                              double totalPrice = qty * Rprice;
                              row.Cells["Total"].Value = totalPrice;

                              if (Convert.ToDouble(row.Cells["discount"].Value) != 0)
                              {
                                  double Disamt = (((Rprice * qty) * disrate) / 100.00);      // Total Discount amount of this item
                                  row.Cells["disamt"].Value = Disamt;
                              }

                              if (Convert.ToDouble(row.Cells["Tax"].Value) != 0)
                              {
                                  double Taxamt = ((((Rprice * qty) - (((Rprice * qty) * disrate) / 100.00)) * Taxrate) / 100.00); // Total Tax amount  of this item
                                  row.Cells["taxamt"].Value = Taxamt; 
                              }

                              total(); 

                               
                          }

                      }
                  }

                

              }
              catch //(Exception exp)
              {
                  // MessageBox.Show("Sorry" + exp.Message);
              }
          }

          private void dtgrdviewReturnItem_CellEndEdit(object sender, DataGridViewCellEventArgs e)
          {
              try
              {
                  // Increase Item Quantity with Edited cell
                  if (e.ColumnIndex == dtgrdviewReturnItem.Columns["Qty"].Index && e.RowIndex >= 0)
                  {
                      foreach (DataGridViewRow row in dtgrdviewReturnItem.SelectedRows)
                      {
                          //// Total Price = Qty * RetailsPrice
                          //double totalPrice = Convert.ToDouble(row.Cells[3].Value) * Convert.ToDouble(row.Cells[2].Value);
                          //row.Cells[4].Value = totalPrice;


                          double qty        = Convert.ToDouble(row.Cells["Qty"].Value);
                          double Rprice     = Convert.ToDouble(row.Cells["RetailsPrice"].Value);
                          double disrate    = Convert.ToDouble(row.Cells["discount"].Value);
                          double Taxrate    = Convert.ToDouble(txtVATRate.Text); // Convert.ToDouble(vatdisvalue.vat);

                          //// show total price   Qty  * Rprice
                          double totalPrice = qty * Rprice;
                          row.Cells["Total"].Value = totalPrice;

                          if (Convert.ToDouble(row.Cells["discount"].Value) != 0) // IF discount is not zero then apply discount
                          {
                              double Disamt = (((Rprice * qty) * disrate) / 100.00);      // Total Discount amount of this item
                              row.Cells["disamt"].Value = Disamt;
                          }

                          if (Convert.ToDouble(row.Cells["Tax"].Value) != 0)  // IF tax is not zero then apply tax
                          {
                              double Taxamt = ((((Rprice * qty) - (((Rprice * qty) * disrate) / 100.00)) * Taxrate) / 100.00); // Total Tax amount  of this item
                              row.Cells["taxamt"].Value = Taxamt;
                          }

                          total();                     

                      }
                  }
              }
              catch (Exception exLog) { Logger.Error(exLog); }
          }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            
                try
                {
                    double Taxrate = Convert.ToDouble(txtVATRate.Text); // Convert.ToDouble(vatdisvalue.vat);

                    // Items rows
                    // string sqlitems = " select ItemName, RetailsPrice, Qty, Total , itemcode from sales_item where sales_id = '" + txtbarcodeinputer.Text + "' ";
                    string sqlitems = " Select ItemName, RetailsPrice, Qty, Total , (((RetailsPrice * Qty ) * discount) / 100.00) as 'disamt' ,  " +
                    " CASE     " +
                    " WHEN taxapply = 1 THEN   ((((RetailsPrice * Qty )  - (((RetailsPrice * Qty ) * discount) / 100.00)) * " + Taxrate + " ) / 100.00 )  " +
                    " ELSE '0.00'  " +
                    " END 'taxamt', discount , taxapply as 'Tax' , itemcode, item_id " +
                    " FROM sales_item where (sales_id = '" + txtbarcodeinputer.Text + "' and status = 1 and Qty != 0) " +
                    " or (sales_id = '" + txtbarcodeinputer.Text + "' and status = 3  and Qty != 0)";
                    DataTable dtItems = DataAccess.GetDataTable(sqlitems);
                    dtgrdviewReturnItem.DataSource = dtItems;


                    ////Hide fields                 
                    dtgrdviewReturnItem.Columns["disamt"].Visible = false; // Disamt         // new in 8.1 version 5
                    dtgrdviewReturnItem.Columns["taxamt"].Visible = false; // taxamt         // new in 8.1 version 6
                    dtgrdviewReturnItem.Columns["discount"].Visible = false; // Discount rate  // new in 8.1 version 7
                    dtgrdviewReturnItem.Columns["itemcode"].Visible = false; // itemcode             // new in 8.1 version 9
                    dtgrdviewReturnItem.Columns["item_id"].Visible = false; // sold_item_ID    item_id          // new in 8.3 version 10


                    dtgrdviewReturnItem.Columns["del"].Width = 35;
                    dtgrdviewReturnItem.Columns["minus"].Width = 35;
                    dtgrdviewReturnItem.Columns["ItemName"].Width = 220;
                    dtgrdviewReturnItem.Columns["Tax"].Width = 40;

                    salePaymentinfo();
                    total();
                    txtInvoiceNo.Text = txtbarcodeinputer.Text;
                    //  lbloveralldiscount.Text   = "0";
                    // txtDiscountRate.Text      = "0";


                }
                catch
                {
                    lblCustID.Text = "10000009";
                    lblTotalReturn.Text = "0";
                    txtReturnAmount.Text = "0";
                    lbldis.Text = "0";
                    lblvat.Text = "0";
                    txtComment.Text = "0";
                    CmbPayType.Text = " ";
                }
            
        }
          //Suspen trx 
          private void Suspen_Click(object sender, EventArgs e)
          {
              try
              {
                  dtgrdviewReturnItem.Rows.Clear();
                  total();
              }
              catch (Exception exLog) { Logger.Error(exLog); }
          }

          //Call system Calculator
          private void button11_Click(object sender, EventArgs e)
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

        private void txtNewDiscountOnReturn_KeyDown(object sender, KeyEventArgs e)
        {
           //sNewDisEnterFlag = true;
        }

        private void txtNewDiscountOnReturn_TextChanged(object sender, EventArgs e)
        {
            
               
        }
    }
}
