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
                DataTable dtCust = DataAccess.GetDataTable(sqlCust);
                ComboCustID.DataSource = dtCust;
                ComboCustID.DisplayMember = "Name";
                ComboCustID.Text = "Guest";
            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
            catch (Exception exLog) { Logger.Error(exLog); } 
        }

        #region Data save

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
        /// </summary>
        public long SaveSale()
        {
            string SalesDate      = dtSalesDate.Text;
            string payby          = CombPayby.Text;
            string payamount      = lblTotalPayable.Text;
            string changeamount   = txtChangeAmount.Text;
            string due            = txtDueAmount.Text;
            string vat            = lblTotalVAT.Text;
            string DiscountTotal  = lblTotalDisCount.Text;
            string custId         = lblCustID.Text;
            string Comment        = ComboCustID.Text + "  " + txtCustName.Text;
            string overalldisRate = txtDiscountRate.Text;
            string vatRate        = txtVATRate.Text;

            long newId = 0;
            DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
            {
                long salesId = tx.NextSalesId();

                // 1. Payment header  (sales_payment)
                tx.Execute(" insert into sales_payment (sales_id, payment_type, payment_amount, change_amount, due_amount, dis, vat, " +
                           " sales_time, c_id, emp_id, comment, TrxType, Shopid, ovdisrate, vaterate) " +
                           " values (@sales_id, @payment_type, @payment_amount, @change_amount, @due_amount, @dis, @vat, " +
                           " @sales_time, @c_id, @emp_id, @comment, 'POS', @Shopid, @ovdisrate, @vaterate)",
                    DataAccess.P("@sales_id", salesId),
                    DataAccess.P("@payment_type", payby),
                    DataAccess.P("@payment_amount", payamount),
                    DataAccess.P("@change_amount", changeamount),
                    DataAccess.P("@due_amount", due),
                    DataAccess.P("@dis", DiscountTotal),
                    DataAccess.P("@vat", vat),
                    DataAccess.P("@sales_time", SalesDate),
                    DataAccess.P("@c_id", custId),
                    DataAccess.P("@emp_id", UserInfo.UserName),
                    DataAccess.P("@comment", Comment),
                    DataAccess.P("@Shopid", UserInfo.Shopid),
                    DataAccess.P("@ovdisrate", overalldisRate),
                    DataAccess.P("@vaterate", vatRate));

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
                        DataAccess.P("@sales_time", SalesDate),
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

        private void btnCompleteSalesAndPrint_Click(object sender, EventArgs e)
        {
            if (txtPaidAmount.Text == "00" || txtPaidAmount.Text == "0" || txtPaidAmount.Text == string.Empty)
            {
                MessageBox.Show("Sorry ! You don't have enough product in Item cart \n  Please Add to cart", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            }
            else
            {
                try
                {
                    // Save payment + items + stock in one transaction
                    long newId = SaveSale();
                    txtInvoice.Text = newId.ToString();

                    btnCompleteSalesAndPrint.Enabled = false;
                    btnSaveOnly.Enabled = false;

                    // Open Print Invoice
                    parameter.autoprintid = "1";
                    ReceiptPrinter.Show(txtInvoice.Text);
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save the sale.");
                }
            }
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
                else
                {
                    try
                    {
                        // Save payment + items + stock in one transaction
                        long newId = SaveSale();
                        txtInvoice.Text = newId.ToString();

                        btnCompleteSalesAndPrint.Enabled = false;
                        btnSaveOnly.Text = "Done";
                        btnSaveOnly.Enabled = false;
                    }
                    catch (Exception exp)
                    {
                        Logger.Show(exp, "Could not save the sale.");
                    }
                }
            }
        }
        #endregion

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

        //Invoice Id Auto increment (display only - the real number is taken inside SaveSale)
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                decimal id = DataAccess.GetDecimal("SELECT ISNULL(MAX(sales_id),0)+1 FROM sales_payment");
                txtInvoice.Text = Convert.ToString(Convert.ToInt64(id));
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
    }
}
