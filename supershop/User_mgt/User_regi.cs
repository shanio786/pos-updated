using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace supershop.User_mgt
{
    public partial class User_regi : Form
    {
        public User_regi()
        {
            InitializeComponent();             
        }

        // Get User ID from ManagerUser form
        public string Uid
        {
            set { lblUid.Text = value; }
            get { return lblUid.Text; }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        } 
        
        //Auto Generate Password
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            string myPWD = PWDGenerator.GeneratePWD();
            textUserPass.Text = myPWD;
        }

        // Load User Info for Update 
        public void loadData(string Uid)
        {
            // The stored password is a hash and is never shown; an empty box means "keep the current password".
            DataTable dt1 = DataAccess.GetDataTable(
                "select id, Name, Father_name, Address, Email, Contact, DOB, Username, usertype, imagename, Shopid, " +
                " basic_salary, joning_date, in_time, out_time, shopname from usermgt where id = @id",
                DataAccess.P("@id", Uid));
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("User not found.", "User", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRow r = dt1.Rows[0];

            txtUserFullName.Text = r["Name"].ToString();
            txtFatherName.Text = r["Father_name"].ToString();
            txtAddress.Text = r["Address"].ToString();
            txtEmailaddress.Text = r["Email"].ToString();
            txtContact.Text = r["Contact"].ToString();
            try { dtDOB.Value = Convert.ToDateTime(r["DOB"].ToString()); } catch (Exception) { }
            txtUsername.Text = r["Username"].ToString();
            textUserPass.Text = "";
            label7.Text = "Password (leave blank to keep)";
            lblimagename.Text = r["imagename"].ToString();

            string path = Application.StartupPath + @"\IMAGE\" + r["imagename"];
            picUserimage.ImageLocation = path;
            if (picUserimage.InitialImage != null) picUserimage.InitialImage.Dispose();

            string usertype = r["usertype"].ToString();
            if (usertype == "1")
                rdbtnAdmin.Checked = true;
            else if (usertype == "2")
                rdbtnManager.Checked = true;
            else if (usertype == "3")
                rdbtnSalesMan.Checked = true;
            else if (usertype == "0")
                rdbtnblock.Checked = true;

            cmboShopid.SelectedValue = r["Shopid"].ToString();
            txtBasicSalary.Text = r["basic_salary"].ToString();
            try { dtJoning.Value = Convert.ToDateTime(r["joning_date"].ToString()); }
            catch (Exception) { dtJoning.Text = ""; }
            try { dtInTime.Value = Convert.ToDateTime(r["in_time"].ToString()); }
            catch (Exception) { dtInTime.Text = ""; }
            try { dtOutTime.Value = Convert.ToDateTime(r["out_time"].ToString()); }
            catch (Exception) { dtOutTime.Text = ""; }
            ShopNametextBox.Text = r["shopname"].ToString();
        }

        // Next UID No (display only - the id column is an identity)
        private void showincrement()
        {
            decimal next = DataAccess.GetDecimal("select ISNULL(MAX(id), 0) + 1 from usermgt");
            txtUid.Text = next.ToString();
        }

        public void Bindshopbranch()
        {
            string sql5 = "select   BranchName , Shopid from tbl_terminallocation";
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            cmboShopid.DataSource = dt5;
            cmboShopid.DisplayMember = "BranchName";
            cmboShopid.ValueMember = "Shopid";
        }

        private void User_regi_Load(object sender, EventArgs e)
        {
            try
            { 
               

                dtDOB.Format = DateTimePickerFormat.Custom;
                dtDOB.CustomFormat = "yyyy-MM-dd";
                Bindshopbranch();
                //Update data | If user id has
                if (lblUid.Text != "-")
                {
                    loadData(lblUid.Text);
                    txtUsername.Enabled = false;
                    btnSave.Enabled = true;
                    btnSave.Text = "Update";
                    lnkDelete.Visible = true;
                }
                else
                {                   
                    showincrement();
                    lnkAddnew.Visible = false;
                    lnkDelete.Visible = false;
                }

            }
            catch (Exception exLog) { Logger.Error(exLog); }


        }

  
        private void ClearForm()
        {
            txtUserFullName.Text = string.Empty;
            txtFatherName.Text = string.Empty;
            txtAddress.Text = string.Empty;
            txtContact.Text = string.Empty;
            txtUsername.Text = string.Empty;
            textUserPass.Text = string.Empty;
            txtEmailaddress.Text = string.Empty;
            dtDOB.Text = string.Empty;
            
        }
        
        // Save if not UID | Update if UID present
        private void btnSave_Click(object sender, EventArgs e)
        {
            DateTime dtIn = Convert.ToDateTime(dtInTime.Value.ToShortTimeString());
            DateTime dtOut = Convert.ToDateTime(dtOutTime.Value.ToShortTimeString());

            if (txtUserFullName.Text == "" )
            {
                MessageBox.Show("Please Add User full Name", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserFullName.Focus();
            }
            else if (txtFatherName.Text == "" )
            {
                MessageBox.Show("Please fill fathers name", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFatherName.Focus();
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please Add Address", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAddress.Focus();
            }
            else if (txtContact.Text == ""  )
            {
                MessageBox.Show("Please Add Contact Number", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtContact.Focus();
            }
            else if (txtUsername.Text == "")
            {
                MessageBox.Show("Please Add Username \n Username should be unique", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUsername.Focus();
            }
            else if (txtEmailaddress.Text == "")
            {
                MessageBox.Show("Please Add  Email address", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtEmailaddress.Focus();
            }
            else if (lblUid.Text == "-" && textUserPass.Text == "")
            {
                // a password is only mandatory for a new user; when updating, empty = unchanged
                MessageBox.Show("Please Add  Password", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textUserPass.Focus();
            }
            else
            {
                try
                {
                    int flag;
                    string posi;
                    if (rdbtnAdmin.Checked) { flag = 1; posi = "Admin"; }
                    else if (rdbtnManager.Checked) { flag = 2; posi = "Manager"; }
                    else if (rdbtnSalesMan.Checked) { flag = 3; posi = "Salesman"; }
                    else if (rdbtnblock.Checked) { flag = 0; posi = "Block"; }
                    else { flag = 0; posi = "0"; }

                    //New Insert / New Entry
                    if (lblUid.Text == "-")
                    {
                        string imageName = txtUid.Text + lblFileExtension.Text;
                        string sql1 = "insert into usermgt (Name, Father_name, Address, Email, Contact, DOB, Username, password, usertype, position, imagename, Shopid, shopname, basic_salary, joning_date, in_time, out_time) " +
                                      " values (@name, @father, @addr, @email, @contact, @dob, @user, @p, @type, @posi, @img, @shopid, @shopname, @salary, @joined, @in, @out)";
                        DataAccess.ExecuteSQL(sql1,
                            DataAccess.P("@name", txtUserFullName.Text),
                            DataAccess.P("@father", txtFatherName.Text),
                            DataAccess.P("@addr", txtAddress.Text),
                            DataAccess.P("@email", txtEmailaddress.Text),
                            DataAccess.P("@contact", txtContact.Text),
                            DataAccess.P("@dob", dtDOB.Text),
                            DataAccess.P("@user", txtUsername.Text),
                            DataAccess.P("@p", PasswordHasher.Hash(textUserPass.Text)),
                            DataAccess.P("@type", flag.ToString()),
                            DataAccess.P("@posi", posi),
                            DataAccess.P("@img", imageName),
                            DataAccess.P("@shopid", cmboShopid.SelectedValue),
                            DataAccess.P("@shopname", ShopNametextBox.Text),
                            DataAccess.P("@salary", txtBasicSalary.Text),
                            DataAccess.P("@joined", dtJoning.Value.ToShortDateString()),
                            DataAccess.P("@in", dtIn.ToShortTimeString()),
                            DataAccess.P("@out", dtOut.ToShortTimeString()));

                        SavePicture(imageName, null);
                        MessageBox.Show("User has been Created Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblEmailerrormsg.Visible = false;

                        User_mgt.Manage_user go = new User_mgt.Manage_user();
                        go.MdiParent = this.ParentForm;
                        go.Show();
                        this.Close();
                    }
                    else // Update info
                    {
                        bool pictureChanged = lblFileExtension.Text != "user.png";
                        string imageName = pictureChanged ? lblUid.Text + lblFileExtension.Text : lblimagename.Text;

                        // An empty password box means "keep the current password"; otherwise store a hash.
                        string passwordSet = textUserPass.Text == "" ? "" : ", password = @p ";
                        string sql = "UPDATE usermgt set Name = @name, Father_name = @father, Address = @addr, Email = @email, Contact = @contact, " +
                                     " DOB = @dob, Username = @user, imagename = @img, usertype = @type, position = @posi, Shopid = @shopid, " +
                                     " basic_salary = @salary, joning_date = @joined, in_time = @in, out_time = @out " + passwordSet +
                                     " where id = @id";
                        DataAccess.ExecuteSQL(sql,
                            DataAccess.P("@name", txtUserFullName.Text),
                            DataAccess.P("@father", txtFatherName.Text),
                            DataAccess.P("@addr", txtAddress.Text),
                            DataAccess.P("@email", txtEmailaddress.Text),
                            DataAccess.P("@contact", txtContact.Text),
                            DataAccess.P("@dob", dtDOB.Value.ToString("yyyy-MM-dd")),
                            DataAccess.P("@user", txtUsername.Text),
                            DataAccess.P("@img", imageName),
                            DataAccess.P("@type", flag.ToString()),
                            DataAccess.P("@posi", posi),
                            DataAccess.P("@shopid", cmboShopid.SelectedValue),
                            DataAccess.P("@salary", txtBasicSalary.Text),
                            DataAccess.P("@joined", dtJoning.Value.ToShortDateString()),
                            DataAccess.P("@in", dtIn.ToShortTimeString()),
                            DataAccess.P("@out", dtOut.ToShortTimeString()),
                            DataAccess.P("@p", textUserPass.Text == "" ? null : PasswordHasher.Hash(textUserPass.Text)),
                            DataAccess.P("@id", lblUid.Text));

                        if (pictureChanged)
                            SavePicture(imageName, lblimagename.Text);

                        MessageBox.Show("Successfully Data Updated!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblEmailerrormsg.Visible = false;
                        loadData(lblUid.Text);
                    }
                }
                catch (Exception exp) { Logger.Show(exp, "Could not save the user."); }
            }
        }

        // Writes the picture box image to \IMAGE\<imageName>, removing the old file first.
        private void SavePicture(string imageName, string oldImageName)
        {
            if (picUserimage.Image == null) return;
            string path = Application.StartupPath + @"\IMAGE\";
            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);
            if (!string.IsNullOrEmpty(oldImageName))
                System.IO.File.Delete(path + oldImageName);
            System.IO.File.Delete(path + imageName);
            picUserimage.Image.Save(path + imageName, System.Drawing.Imaging.ImageFormat.Png);
        }

        // Reset  
        private void btnReset_Click(object sender, EventArgs e)
        {
            ClearForm();
            
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
                picUserimage.ImageLocation = openFileDialog1.FileName;
                lblFileExtension.Text = Path.GetExtension(openFileDialog1.FileName);
            }
        }

        private void User_regi_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void txtEmailaddress_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            if (txtEmailaddress.Text.Length > 0 && txtEmailaddress.Text.Trim().Length != 0)
            {
                if (!rEmail.IsMatch(txtEmailaddress.Text.Trim()))
                {
                    lblEmailerrormsg.Visible = true;
                    lblEmailerrormsg.Text = "Invalid Email address";
                    txtEmailaddress.SelectAll();
                    // e.Cancel = true;
                   
                }
                else
                {
                    btnSave.Enabled = true;
                    lblEmailerrormsg.Visible = false;
                }
            }
           
        }

        private void lnkManageusers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            User_mgt.Manage_user go = new User_mgt.Manage_user();
            go.MdiParent = this.ParentForm;
            go.Show();
            this.Close();
        }

        private void lnkCustomers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
            Customer.CustomerDetails go = new Customer.CustomerDetails();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void lnkAddnew_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            User_mgt.User_regi go = new User_mgt.User_regi();
            go.MdiParent = this.ParentForm;
            go.Show();
           
        }
        
        //// Delete user
        private void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {

                if (lblUid.Text == "-")
                {
                    // MessageBox.Show("You are Not able to Update");
                    MessageBox.Show("You are Not able to Delete", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        DataAccess.ExecuteSQL("delete from usermgt where id = @id", DataAccess.P("@id", lblUid.Text));

                        if (picUserimage.InitialImage != null) picUserimage.InitialImage.Dispose();
                        string path = Application.StartupPath + @"\IMAGE\";
                        System.IO.File.Delete(path + @"\" + lblimagename.Text);

                        MessageBox.Show("User has been Deleted", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);                  
                        User_mgt.Manage_user go = new User_mgt.Manage_user();
                        go.MdiParent = this.ParentForm;
                        go.Show();
                        this.Close(); 
                        ClearForm();

                    }
                    catch (Exception exp)
                    {
                        Logger.Show(exp, "Could not delete the user.");
                    }
                }
            }
        }

        private void lnkWorkingHours_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserInfo.usernamWK = txtUsername.Text;
            //this.Hide();
            User_mgt.WorkRecords go = new User_mgt.WorkRecords();
          //  go.MdiParent = this.ParentForm;
            go.ShowDialog();
        }

        private void cmboShopid_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShopNametextBox.Text = DataAccess.ExecuteSQLScaler("select Shopid from tbl_terminallocation where Branchname = @b",
                DataAccess.P("@b", cmboShopid.Text));
        }
    }
}
