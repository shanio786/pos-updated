using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;

namespace supershop
{
    public partial class Add_Item : Form
    {
        public Add_Item()
        {
            InitializeComponent();
        }

        // Get Item bar-code from Stock List form
        public string itemCode
        {
            set { lblItemcode.Text = value; }
            get { return lblItemcode.Text; }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region DataBind
        private void loadData()
        {
            string sql = "select product_id, product_name, product_quantity, cost_price , retail_price, category ,  " +
                        " supplier, imagename , discount, Shopid , taxapply , status " + 
                        " from purchase where product_id = '" + lblItemcode.Text + "'";
            DataTable dt1 = DataAccess.GetDataTable(sql);

            txtProductCode.Text     = dt1.Rows[0].ItemArray[0].ToString();
            txtProductName.Text     = dt1.Rows[0].ItemArray[1].ToString();
            txtProductQty.Text      = dt1.Rows[0].ItemArray[2].ToString();
            txtCostPrice.Text       = dt1.Rows[0].ItemArray[3].ToString();
            txtSalesPrice.Text      = dt1.Rows[0].ItemArray[4].ToString();
            ComboCategory.Text      = dt1.Rows[0].ItemArray[5].ToString();
            cmbSupplier.Text        = dt1.Rows[0].ItemArray[6].ToString();
            lblimagename.Text       = dt1.Rows[0].ItemArray[7].ToString();

            string path = Application.StartupPath + @"\ITEMIMAGE\" + dt1.Rows[0].ItemArray[7].ToString() + "";
            picItemimage.ImageLocation = path;
            picItemimage.InitialImage.Dispose();
            
            txtdiscount.Text = dt1.Rows[0].ItemArray[8].ToString();           
            cmboShopid.SelectedValue = dt1.Rows[0].ItemArray[9].ToString();
            
            if ( dt1.Rows[0].ItemArray[10].ToString() == "1")
            {
                chktaxapply.Checked = true;
            }
            else
            {
                chktaxapply.Checked = false;
            }

            if (dt1.Rows[0].ItemArray[11].ToString() == "3")  // 3 = show kitchen display 
            {
                chkkitchenDisplay.Checked = true;
            }
            else
            {
                chkkitchenDisplay.Checked = false;
            }
        }

        public void Bindshopbranch()
        {
            string sql5 = "select   BranchName , Shopid from tbl_terminalLocation";
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            cmboShopid.DataSource = dt5;
            cmboShopid.DisplayMember = "Branchname";
            cmboShopid.ValueMember = "Shopid";
           
        }

        private void Add_Item_Load(object sender, EventArgs e)
        {
            try
            {
                if (UserInfo.usertype == "1")
                {
                    lnkStocklist.Visible = true;
                }
                else
                {
                    lnkStocklist.Visible = false;
                }
                dtpurchaseDate.Format = DateTimePickerFormat.Custom;
                dtpurchaseDate.CustomFormat = "yyyy-MM-dd";

                //Supplier Info
                string sqlCust = "select   DISTINCT  *   from tbl_customer where PeopleType = 'Supplier'";
                DataTable dtCust = DataAccess.GetDataTable(sqlCust);
                cmbSupplier.DataSource = dtCust;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.Text = "Unknown";

                //Category list
                string sqlcate = "select DISTINCT   category_name from tbl_category";
                DataTable dtcate = DataAccess.GetDataTable(sqlcate);
                ComboCategory.DataSource = dtcate;
                ComboCategory.DisplayMember = "category_name";
                
                Bindshopbranch();
                //Update data | If user id has
                if (lblItemcode.Text != "-")
                {
                    loadData();
                    txtProductCode.ReadOnly = true;
                    btnSave.Text = "Update";
                    lnkDelete.Visible = true;
                    grpboxPurchasehistory.Visible = true;                    
                }             

            }
            catch (Exception exLog) { Logger.Error(exLog); }

        }
        #endregion

        #region Insert , Update and delete Item
        
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtProductCode.Text == "" )
            {
                MessageBox.Show("Please Insert Product Code/ Item Bar-code");
                txtProductCode.Focus();
            } 
            else if (txtProductName.Text == "")
            {
                MessageBox.Show("Please Insert  Product Name");
                txtProductName.Focus();
            }
            else if (txtdiscount.Text == "")
            {
                txtdiscount.Text = "0";
                txtdiscount.Focus();
            }
            else if (txtProductQty.Text == "")
            {
                MessageBox.Show("Please Insert Product Quantity");
                txtProductQty.Focus();
            }
            else if (txtCostPrice.Text == "")
            {
                MessageBox.Show("Please Insert Product Cost Price / Buy price ");
                txtCostPrice.Focus();
            }
  
            else if (txtSalesPrice.Text == "")
            {
                MessageBox.Show("Please Insert Product  Sales Price");
                txtSalesPrice.Focus();
            }
            else if (ComboCategory.Text == "")
            {
                MessageBox.Show("Please Insert Product Category");
                ComboCategory.Focus();
            }
            else if (cmboShopid.Text == "")
            {
                MessageBox.Show("Please Select Branch name ");
                cmboShopid.Focus();
            }
            else if (cmbSupplier.Text == "")
            {
                MessageBox.Show("Please Select Supplier Name");
                cmbSupplier.Focus();
            }
            else
            {
                try
                {
                    string pid = txtProductCode.Text;
                    string pname = txtProductName.Text;
                    double quan = Convert.ToDouble(txtProductQty.Text);
                    double cprice = Convert.ToDouble(txtCostPrice.Text);
                    double sprice = Convert.ToDouble(txtSalesPrice.Text);

                    double ctotalpri = quan * cprice;
                    double rtotalpri = quan * sprice;
                    double discount = Convert.ToDouble(txtdiscount.Text);

                    int taxapply;
                    if (chktaxapply.Checked)
                    {
                        taxapply = 1;  //1 = Tax apply
                    }
                    else
                    {
                        taxapply = 0; // 0 = Tax not apply
                    }

                    int kitchenDisplaythisitem;
                    if (chkkitchenDisplay.Checked)
                    {
                        kitchenDisplaythisitem = 3; // 3 = It's show display on kitchen display
                    }
                    else
                    {
                        kitchenDisplaythisitem = 1; // 1 = it's not show on ditcken display.
                    }
                     //New Insert / New Entry
                    if (lblItemcode.Text == "-")
                    {
                        string imageName = pid + lblFileExtension.Text;
                        string sql1 = " insert into purchase (product_id, product_name, product_quantity, cost_price, retail_price, total_cost_price, " +
                                        " total_retail_price, category, supplier , imagename, discount, taxapply, Shopid , status) " +
                                        " values ('" + pid + "', '" + pname + "', '" + quan + "', '" + cprice + "', '" + sprice + "', '" + ctotalpri + "', " +
                                        " '" + rtotalpri + "', '" + ComboCategory.Text + "', '" + cmbSupplier.Text + "' , '" + imageName + "', " +
                                        " '" + discount + "' , '" + taxapply + "' , '" + cmboShopid.SelectedValue + "' , '"+ kitchenDisplaythisitem +"')";
                        DataAccess.ExecuteSQL(sql1);
                        
                        //Add to purchase history - New item history
                        insertpurchasehistory("NEW", quan, DateTime.Now.ToString("yyyy-MM-dd"));
                        
                        //picture upload  /////////////////
                      //  if (openFileDialog1.FileName != string.Empty)
                       // {
                            string path = Application.StartupPath + @"\ITEMIMAGE\";
                            System.IO.File.Delete(path + @"\" + imageName);
                            if (!System.IO.Directory.Exists(path))
                                System.IO.Directory.CreateDirectory(Application.StartupPath + @"\ITEMIMAGE\");
                            string filename = path + @"\" + openFileDialog1.SafeFileName;
                            picItemimage.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                            System.IO.File.Move(path + @"\" + openFileDialog1.SafeFileName, path + @"\" + imageName);
                     //   }

                        MessageBox.Show("Item hase been saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (UserInfo.usertype == "1")
                        {
                            //Stock_List go = new Stock_List();
                            //go.MdiParent = this.ParentForm;
                            //go.Show();
                            //this.Close();
                        }
                        else
                        {
                           // btnItemLink.Visible = false;
                        }
               
                        
                        ClearForm();
                    }
                    else  //Update
                    {
                        
                        string imageName;
                        if (lblFileExtension.Text == "item.png") //if not select image
                        {
                            imageName = lblimagename.Text;
                        }
                        else  // select image
                        {
                            imageName = lblItemcode.Text + lblFileExtension.Text;
                        }

                        string sql =" update purchase set product_name = '" + txtProductName.Text + "', product_quantity= '" + txtProductQty.Text + "', " + 
                                    " cost_price = '" + txtCostPrice.Text + "', retail_price= '" + txtSalesPrice.Text + "', total_cost_price = '" + ctotalpri + "', " +
                                    " total_retail_price= '" + rtotalpri + "', category = '" + ComboCategory.Text + "', supplier = '" + cmbSupplier.Text + "',  " +
                                    " imagename = '" + imageName + "' , discount   = '" + discount + "' , taxapply = '" + taxapply + "' , " +
                                    " Shopid = '" + cmboShopid.SelectedValue + "' , status =  '" + kitchenDisplaythisitem + "' " + 
                                    " where product_id = '" + lblItemcode.Text + "'";
                        DataAccess.ExecuteSQL(sql);

                        /////////////////////////////////////////////Update image //////////////////////////////////////////////////////
                          if (lblFileExtension.Text != "item.png") // if select image
                          {
                              picItemimage.InitialImage.Dispose();
                              string path = Application.StartupPath + @"\ITEMIMAGE\";                                                 
                              System.IO.File.Delete(path + @"\" + lblimagename.Text);
                              if (!System.IO.Directory.Exists(path))
                                  System.IO.Directory.CreateDirectory(Application.StartupPath + @"\ITEMIMAGE\");
                              string filename = path + @"\" + openFileDialog1.SafeFileName;
                              picItemimage.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                              System.IO.File.Move(path + @"\" + openFileDialog1.SafeFileName, path + @"\" + imageName);
                          }


                        MessageBox.Show("Successfully Data Updated!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //loadData();    
                        if (UserInfo.usertype == "1")
                        {
                            //Stock_List go = new Stock_List();
                            //go.MdiParent = this.ParentForm;
                            //go.Show();
                            //this.Close();
                        }
                        else
                        {
                            // btnItemLink.Visible = false;
                        }
                    }


                }
                catch (Exception exp)
                {
                    MessageBox.Show("Sorry\r\n this id already added \n" + exp.Message);
                }
            }
            //this.Hide();

        }

        private void ClearForm()
        {
            txtProductCode.Text = string.Empty;
            txtProductName.Text = string.Empty;
            txtProductQty.Text = string.Empty;
            txtCostPrice.Text = string.Empty;
            txtSalesPrice.Text = string.Empty;
        }
              
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //  openFileDialog1.InitialDirectory = @"C:\";
            //  openFileDialog1.Title = "Browse Text Files";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = ".jpg";
            // openFileDialog1.Filter = "GIF files (*.gif)|*.gif| jpg files (*.jpg)|*.jpg| PNG files (*.png)|*.png| All files (*.*)|*.*";
            openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg| PNG files (*.png)|*.png";

            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            //openFileDialog1.ReadOnlyChecked = true;
            //openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // textBox1.Text = openFileDialog1.FileName;
                picItemimage.ImageLocation = openFileDialog1.FileName;
                lblFileExtension.Text = Path.GetExtension(openFileDialog1.FileName);
            }
        }

        private void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {

                if (lblItemcode.Text == "-")
                {
                    // MessageBox.Show("You are Not able to Update");
                    MessageBox.Show("You are Not able to Delete", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        string sql = "delete from purchase where product_id ='" + lblItemcode.Text + "'";
                        DataAccess.ExecuteSQL(sql);

                        picItemimage.InitialImage.Dispose();
                        string path = Application.StartupPath + @"\ITEMIMAGE\";
                        System.IO.File.Delete(path + @"\" + lblimagename.Text);
                        MessageBox.Show("Successfully Data Delete !", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        Stock_List go = new Stock_List();
                        go.MdiParent = this.ParentForm;
                        go.Show();
                        this.Close();
                        ClearForm();

                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Sorry\r\n You have to Check the Data" + exp.Message);
                    }
                }
            }
        }
        
        #endregion

        #region   Accept Decimal Value Validation
        private void txtProductCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtProductCode.Text.ToString(), @"\");

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

        private void txtProductQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtProductQty.Text.ToString(), @"\.\d\d\d");

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

        //Purchase history Qty
        private void txtNewpQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtNewpQty.Text.ToString(), @"\.\d\d\d");

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

        private void txtCostPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtCostPrice.Text.ToString(), @"\.\d\d\d");

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

        private void txtSalesPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtSalesPrice.Text.ToString(), @"\.\d\d\d");

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

        private void txtdiscount_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtdiscount.Text.ToString(), @"\.\d\d\d");

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
        

        //Check item code verfication
        private void txtProductCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sqlitemcode = " select  product_id from   purchase where product_id = '" + txtProductCode.Text + "' ";
                DataTable dtitemcode = DataAccess.GetDataTable(sqlitemcode);
                if (dtitemcode.Rows.Count > 0)
                {
                    lblValidmsg.ForeColor = System.Drawing.Color.Red;
                    lblValidmsg.Text = "Duplicate item code";                    
                    if (lblItemcode.Text == "-")
                    {
                        MessageBox.Show("Warning: Duplicate item code \n Item code already used for another product", "Warning ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                }
                else
                {
                    lblValidmsg.ForeColor = System.Drawing.Color.Black;
                    lblValidmsg.Text = "Valid code";                 

                }
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        #region Page links
        private void lnkbulkitems_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Import_Items go = new Import_Items();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void lnkStocklist_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Stock_List go = new Stock_List();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void lnkcategories_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Items.Categories go = new Items.Categories();
            go.MdiParent = this.ParentForm;
            go.Show();
        }
      
        private void lnkSupplier_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            parameter.peopleid = "SUP";
            Customer.CustomerDetails go = new Customer.CustomerDetails();
            go.MdiParent = this.ParentForm;
            go.Show();
        }
               
        #endregion
       
        #region Purchase history
        public void insertpurchasehistory(string ptype, double pQty, string pdate)
        {
            string pid = txtProductCode.Text;
            string pname = txtProductName.Text;           
            double cprice = Convert.ToDouble(txtCostPrice.Text);
            double sprice = Convert.ToDouble(txtSalesPrice.Text);
             

            string sql1 = " insert into tbl_purchase_history (product_id, product_name, product_quantity, cost_price, retail_price, category, " +
                            " supplier, purchase_date, Shopid, ptype ) " +
                            " values ('" + pid + "', '" + pname + "', '" + pQty + "', '" + cprice + "', '" + sprice + "', '" + ComboCategory.Text + "', " +
                            "  '" + cmbSupplier.Text + "', '"+ pdate +"' ,'" + cmboShopid.SelectedValue + "', '"+ ptype +"' )";
            DataAccess.ExecuteSQL(sql1);
        }

        public void updatestockqty()
        {
            string pid      = txtProductCode.Text;
            double StockQty =  Convert.ToDouble(txtProductQty.Text) + Convert.ToDouble(txtNewpQty.Text) ;
            string sql = " update purchase set " +
                                    " product_quantity = '" + StockQty + "', " +
                                    " retail_price = '" + txtSalesPrice.Text + "' " +
                                    " where product_id = '" + pid + "' ";
            DataAccess.ExecuteSQL(sql);
        }

        private void btnPurchaseHistory_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewpQty.Text == "")
                {
                    MessageBox.Show("Please Insert Purchase Quantity");
                    txtNewpQty.Focus();
                }
                else
                {
                    insertpurchasehistory("OLD", Convert.ToDouble(txtNewpQty.Text), dtpurchaseDate.Text);
                    updatestockqty();

                    DialogResult result = MessageBox.Show("Purchase history hase been saved Successfully. \n\n Do you want to see Purchase history?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);

                    if (result == DialogResult.Yes)
                    {
                        this.Hide();
                        Items.Purchase_History go = new Items.Purchase_History();
                        go.MdiParent = this.ParentForm;
                        go.Show();
                    }
                    else
                    {
                        // MessageBox.Show("", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnPurchaseHistory.Enabled = false;
                    }
                  
                }

            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        #endregion


    }
}
