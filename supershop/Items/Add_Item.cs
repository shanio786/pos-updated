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
            string sql = "select product_id, product_name, product_quantity, cost_price, retail_price, category, " +
                         " supplier, imagename, discount, Shopid, taxapply, status " +
                         " from purchase where product_id = @id";
            DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@id", lblItemcode.Text));
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("Item not found: " + lblItemcode.Text);
                return;
            }
            DataRow r = dt1.Rows[0];

            txtProductCode.Text = r["product_id"].ToString();
            txtProductName.Text = r["product_name"].ToString();
            txtProductQty.Text  = r["product_quantity"].ToString();
            txtCostPrice.Text   = r["cost_price"].ToString();
            txtSalesPrice.Text  = r["retail_price"].ToString();
            ComboCategory.Text  = r["category"].ToString();
            cmbSupplier.Text    = r["supplier"].ToString();
            lblimagename.Text   = r["imagename"].ToString();

            string path = Application.StartupPath + @"\ITEMIMAGE\" + r["imagename"].ToString();
            picItemimage.ImageLocation = path;
            picItemimage.InitialImage.Dispose();

            txtdiscount.Text = r["discount"].ToString();
            cmboShopid.SelectedValue = r["Shopid"].ToString();

            chktaxapply.Checked = (r["taxapply"].ToString() == "1");
            chkkitchenDisplay.Checked = (r["status"].ToString() == "3");  // 3 = show on kitchen display
        }

        public void Bindshopbranch()
        {
            string sql5 = "select BranchName, Shopid from tbl_terminalLocation";
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            cmboShopid.DataSource = dt5;
            cmboShopid.DisplayMember = "Branchname";
            cmboShopid.ValueMember = "Shopid";
        }

        private void Add_Item_Load(object sender, EventArgs e)
        {
            try
            {
                lnkStocklist.Visible = (UserInfo.usertype == "1");

                dtpurchaseDate.Format = DateTimePickerFormat.Custom;
                dtpurchaseDate.CustomFormat = "yyyy-MM-dd";

                //Supplier Info
                string sqlCust = "select DISTINCT * from tbl_customer where PeopleType = 'Supplier'";
                DataTable dtCust = DataAccess.GetDataTable(sqlCust);
                cmbSupplier.DataSource = dtCust;
                cmbSupplier.DisplayMember = "Name";
                cmbSupplier.Text = "Unknown";

                //Category list
                string sqlcate = "select DISTINCT category_name from tbl_category";
                DataTable dtcate = DataAccess.GetDataTable(sqlcate);
                ComboCategory.DataSource = dtcate;
                ComboCategory.DisplayMember = "category_name";

                Bindshopbranch();

                //Update mode when an item code was passed in
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
            if (txtProductCode.Text == "")
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
                    decimal quan = Convert.ToDecimal(txtProductQty.Text);
                    decimal cprice = Convert.ToDecimal(txtCostPrice.Text);
                    decimal sprice = Convert.ToDecimal(txtSalesPrice.Text);
                    decimal ctotalpri = quan * cprice;
                    decimal rtotalpri = quan * sprice;
                    decimal discount = Convert.ToDecimal(txtdiscount.Text);
                    string category = ComboCategory.Text;
                    string supplier = cmbSupplier.Text;
                    string shopid = Convert.ToString(cmboShopid.SelectedValue);

                    int taxapply = chktaxapply.Checked ? 1 : 0;                      // 1 = Tax apply
                    int kitchenDisplaythisitem = chkkitchenDisplay.Checked ? 3 : 1;  // 3 = show on kitchen display, 1 = not

                    if (lblItemcode.Text == "-")  //New Insert / New Entry
                    {
                        string imageName = pid + lblFileExtension.Text;
                        string purchaseDate = DateTime.Now.ToString("yyyy-MM-dd");

                        // The product row and its first purchase-history row succeed or fail together
                        DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                        {
                            tx.Execute(" insert into purchase (product_id, product_name, product_quantity, cost_price, retail_price, total_cost_price, " +
                                       " total_retail_price, category, supplier, imagename, discount, taxapply, Shopid, status) " +
                                       " values (@pid, @pname, @qty, @cprice, @sprice, @ctotal, @rtotal, @category, @supplier, @image, " +
                                       " @discount, @taxapply, @shopid, @status)",
                                DataAccess.P("@pid", pid),
                                DataAccess.P("@pname", pname),
                                DataAccess.P("@qty", quan),
                                DataAccess.P("@cprice", cprice),
                                DataAccess.P("@sprice", sprice),
                                DataAccess.P("@ctotal", ctotalpri),
                                DataAccess.P("@rtotal", rtotalpri),
                                DataAccess.P("@category", category),
                                DataAccess.P("@supplier", supplier),
                                DataAccess.P("@image", imageName),
                                DataAccess.P("@discount", discount),
                                DataAccess.P("@taxapply", taxapply),
                                DataAccess.P("@shopid", shopid),
                                DataAccess.P("@status", kitchenDisplaythisitem));

                            insertpurchasehistory(tx, "NEW", quan, purchaseDate);
                        });

                        SaveItemImage(imageName, null);

                        MessageBox.Show("Item hase been saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                        DataAccess.ExecuteSQL(" update purchase set product_name = @pname, product_quantity = @qty, cost_price = @cprice, " +
                                              " retail_price = @sprice, total_cost_price = @ctotal, total_retail_price = @rtotal, " +
                                              " category = @category, supplier = @supplier, imagename = @image, discount = @discount, " +
                                              " taxapply = @taxapply, Shopid = @shopid, status = @status " +
                                              " where product_id = @pid",
                            DataAccess.P("@pname", pname),
                            DataAccess.P("@qty", quan),
                            DataAccess.P("@cprice", cprice),
                            DataAccess.P("@sprice", sprice),
                            DataAccess.P("@ctotal", ctotalpri),
                            DataAccess.P("@rtotal", rtotalpri),
                            DataAccess.P("@category", category),
                            DataAccess.P("@supplier", supplier),
                            DataAccess.P("@image", imageName),
                            DataAccess.P("@discount", discount),
                            DataAccess.P("@taxapply", taxapply),
                            DataAccess.P("@shopid", shopid),
                            DataAccess.P("@status", kitchenDisplaythisitem),
                            DataAccess.P("@pid", lblItemcode.Text));

                        if (lblFileExtension.Text != "item.png") // a new image was selected
                        {
                            picItemimage.InitialImage.Dispose();
                            SaveItemImage(imageName, lblimagename.Text);
                        }

                        MessageBox.Show("Successfully Data Updated!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save item (the item code may already exist)");
                }
            }
        }

        // Saves the picture shown on the form as ITEMIMAGE\<imageName>, removing the previous file when given
        private void SaveItemImage(string imageName, string oldImageName)
        {
            string path = Application.StartupPath + @"\ITEMIMAGE\";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            if (!string.IsNullOrEmpty(oldImageName))
                File.Delete(path + oldImageName);
            File.Delete(path + imageName);
            if (picItemimage.Image != null)
                picItemimage.Image.Save(path + imageName, System.Drawing.Imaging.ImageFormat.Png);
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

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = ".jpg";
            openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg| PNG files (*.png)|*.png";

            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
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
                    MessageBox.Show("You are Not able to Delete", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        DataAccess.ExecuteSQL("delete from purchase where product_id = @pid", DataAccess.P("@pid", lblItemcode.Text));

                        picItemimage.InitialImage.Dispose();
                        string path = Application.StartupPath + @"\ITEMIMAGE\";
                        File.Delete(path + lblimagename.Text);
                        MessageBox.Show("Successfully Data Delete !", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

                        Stock_List go = new Stock_List();
                        go.MdiParent = this.ParentForm;
                        go.Show();
                        this.Close();
                        ClearForm();
                    }
                    catch (Exception exp)
                    {
                        Logger.Show(exp, "Could not delete item");
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
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        #endregion

        //Check item code verfication
        private void txtProductCode_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dtitemcode = DataAccess.GetDataTable("select product_id from purchase where product_id = @id",
                    DataAccess.P("@id", txtProductCode.Text));
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
        // Adds one purchase-history row inside the caller's transaction
        private void insertpurchasehistory(DataAccess.DbTransaction tx, string ptype, decimal pQty, string pdate)
        {
            tx.Execute(" insert into tbl_purchase_history (product_id, product_name, product_quantity, cost_price, retail_price, category, " +
                       " supplier, purchase_date, Shopid, ptype) " +
                       " values (@pid, @pname, @qty, @cprice, @sprice, @category, @supplier, @pdate, @shopid, @ptype)",
                DataAccess.P("@pid", txtProductCode.Text),
                DataAccess.P("@pname", txtProductName.Text),
                DataAccess.P("@qty", pQty),
                DataAccess.P("@cprice", Convert.ToDecimal(txtCostPrice.Text)),
                DataAccess.P("@sprice", Convert.ToDecimal(txtSalesPrice.Text)),
                DataAccess.P("@category", ComboCategory.Text),
                DataAccess.P("@supplier", cmbSupplier.Text),
                DataAccess.P("@pdate", pdate),
                DataAccess.P("@shopid", Convert.ToString(cmboShopid.SelectedValue)),
                DataAccess.P("@ptype", ptype));
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
                    decimal newQty = Convert.ToDecimal(txtNewpQty.Text);
                    decimal stockQty = Convert.ToDecimal(txtProductQty.Text) + newQty;
                    decimal salesPrice = Convert.ToDecimal(txtSalesPrice.Text);
                    string pid = txtProductCode.Text;
                    string pdate = dtpurchaseDate.Text;

                    // History row and the stock update commit together
                    DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                    {
                        insertpurchasehistory(tx, "OLD", newQty, pdate);
                        tx.Execute("update purchase set product_quantity = @qty, retail_price = @sprice where product_id = @pid",
                            DataAccess.P("@qty", stockQty),
                            DataAccess.P("@sprice", salesPrice),
                            DataAccess.P("@pid", pid));
                    });

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
                        btnPurchaseHistory.Enabled = false;
                    }
                }
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not save purchase history"); }
        }

        #endregion
    }
}
