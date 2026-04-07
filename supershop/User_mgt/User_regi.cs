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
            string sql3 = "select * from usermgt where id = '" + Uid + "'";
            DataAccess.ExecuteSQL(sql3);
            DataTable dt1 = DataAccess.GetDataTable(sql3);

          //  lblUid.Text = dt1.Rows[0].ItemArray[0].ToString();
            txtUserFullName.Text = dt1.Rows[0].ItemArray[1].ToString();
            txtFatherName.Text = dt1.Rows[0].ItemArray[2].ToString();
            txtAddress.Text = dt1.Rows[0].ItemArray[3].ToString();
            txtEmailaddress.Text = dt1.Rows[0].ItemArray[4].ToString();
            txtContact.Text = dt1.Rows[0].ItemArray[5].ToString();
            dtDOB.Value = Convert.ToDateTime(dt1.Rows[0].ItemArray[6].ToString());
            txtUsername.Text = dt1.Rows[0].ItemArray[7].ToString();
            textUserPass.Text = dt1.Rows[0].ItemArray[8].ToString();             
            lblimagename.Text = dt1.Rows[0].ItemArray[11].ToString();
           

            string path = Application.StartupPath + @"\IMAGE\" + dt1.Rows[0].ItemArray[11].ToString() + "";
            picUserimage.ImageLocation = path;
            picUserimage.InitialImage.Dispose();

            if (dt1.Rows[0].ItemArray[9].ToString() == "1")
            {
                rdbtnAdmin.Checked = true;
            }
            else if (dt1.Rows[0].ItemArray[9].ToString() == "2")
            {
                rdbtnManager.Checked = true;
            }
            else if (dt1.Rows[0].ItemArray[9].ToString() == "3")
            {
                rdbtnSalesMan.Checked = true;
            }
            else if (dt1.Rows[0].ItemArray[9].ToString() == "0")
            {
                rdbtnblock.Checked = true;
            }
            else
            {
               // rdbtnInactive.Checked = true;
            }         
            cmboShopid.SelectedValue = dt1.Rows[0].ItemArray[12].ToString();
            txtBasicSalary.Text = dt1.Rows[0].ItemArray[14].ToString();
            try
            {
                dtJoning.Value = Convert.ToDateTime(dt1.Rows[0].ItemArray[15].ToString());
            }
            catch (Exception) { dtJoning.Text = ""; }
            try
            {
                dtInTime.Value = Convert.ToDateTime(dt1.Rows[0].ItemArray[16].ToString());
            }
            catch (Exception) { dtInTime.Text = ""; }
            try
            {
                dtOutTime.Value = Convert.ToDateTime(dt1.Rows[0].ItemArray[17].ToString());
            }
            catch (Exception) { dtOutTime.Text = ""; }
            ShopNametextBox.Text = dt1.Rows[0].ItemArray[18].ToString();

        }

        // Load UID No
        private void showincrement()
        {
            string sql = "select id from usermgt order by id desc";
            DataTable dt = DataAccess.GetDataTable(sql);
            if (dt.Rows.Count > 0)
            {
                int id = Convert.ToInt32(dt.Rows[0].ItemArray[0].ToString()) + 1;
                txtUid.Text = id.ToString();
            }
            else
            {
                int id = 1;
                txtUid.Text = id.ToString();
            }
        }

        public void Bindshopbranch()
        {
            string sql5 = "select   BranchName , Shopid from tbl_terminallocation";
            DataAccess.ExecuteSQL(sql5);
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
            catch
            {
            }


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
            else if (textUserPass.Text == "")
            {
                MessageBox.Show("Please Add  Password", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textUserPass.Focus();
            }
            else
            {
               try
                {                     
                    
                    int flag;
                    if (rdbtnAdmin.Checked)
                    {
                        flag = 1;
                    }
                    else if (rdbtnManager.Checked)
                    {
                        flag = 2;
                    }
                    else if (rdbtnSalesMan.Checked)
                    {
                        flag = 3;
                    }
                    else if (rdbtnblock.Checked)
                    {
                        flag = 0;
                    }
                    else
                    {
                        flag = 0;
                    }
                    string posi;
                    if (rdbtnAdmin.Checked)
                    {
                        posi = "Admin";
                    }
                    else if (rdbtnManager.Checked)
                    {
                        posi = "Manager";
                    }
                    else if (rdbtnSalesMan.Checked)
                    {
                        posi = "Salesman";
                    }
                    else if (rdbtnblock.Checked)
                    {
                        posi = "Block";
                    }
                    else
                    {
                        posi = "0";
                    }

                    //New Insert / New Entry
                    if(lblUid.Text == "-")
                    {
                        string sql1 = "";
                        //string selno = txtUid.Text;
                        string imageName = txtUid.Text + lblFileExtension.Text; //System.IO.Path.GetFileName(openFileDialog1.FileName);
                          sql1 = "insert into usermgt (Name, Father_name, Address, Email , Contact, DOB , Username , password , usertype , position , imagename, Shopid, shopname,basic_salary,joning_date,in_time,out_time) " +
                                          "  values('" + txtUserFullName.Text + "', '" + txtFatherName.Text + "', '" + txtAddress.Text + "', '" + txtEmailaddress.Text + "', " +
                                          " '" + txtContact.Text + "',  '" + dtDOB.Text + "', '" + txtUsername.Text + "', '" + textUserPass.Text + "', " +
                                          " '" + flag + "', '" + posi + "' , '" + imageName + "' , '" + cmboShopid.SelectedValue + "' , '" + ShopNametextBox.Text + "','" + txtBasicSalary.Text + "','" + dtJoning.Value.ToShortDateString() + "','" + dtIn.ToShortTimeString() + "','" + dtOut.ToShortTimeString() + "')";
                            DataAccess.ExecuteSQL(sql1);
                       

                        /////////////////////picture upload  /////////////////
                        string path = Application.StartupPath + @"\IMAGE\";
                        System.IO.File.Delete(path + @"\" + imageName);
                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(Application.StartupPath + @"\IMAGE\");
                        string filename = path + @"\" + openFileDialog1.SafeFileName;
                        picUserimage.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                        System.IO.File.Move(path + @"\" + openFileDialog1.SafeFileName, path + @"\" + imageName);
                        MessageBox.Show("User hase been Created Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblEmailerrormsg.Visible = false;

                        User_mgt.Manage_user go = new User_mgt.Manage_user();
                        go.MdiParent = this.ParentForm;
                        go.Show();
                        this.Close(); 
                    }
                    else // Update info
                    {
                        string imageName;
                        if (lblFileExtension.Text == "user.png")
                        {
                            imageName = lblimagename.Text;  //Unchange pictures
                        }
                        else  //When change 
                        {
                            imageName = lblUid.Text + lblFileExtension.Text;
                        }                   
                       
                        string sql = "UPDATE usermgt set  Name = '" + txtUserFullName.Text + "', Father_name = '" + txtFatherName.Text + "', " +
                        " Address = '" + txtAddress.Text + "', Email = '" + txtEmailaddress.Text + "', Contact = '" + txtContact.Text + "', " +
                        " DOB = '" + dtDOB.Value.ToString("yyyy-MM-dd") + "' , Username= '" + txtUsername.Text + "', password = '" + textUserPass.Text + "',imagename = '" + imageName + "' ,    " +
                        " usertype    = '" + flag + "', position = '" + posi + "', Shopid = '" + cmboShopid.SelectedValue + "', "+
                        " basic_salary = '" + txtBasicSalary.Text + "', joning_date = '" + dtJoning.Value.ToShortDateString() + "', in_time = '" + dtIn.ToShortTimeString() + "', out_time = '" + dtOut.ToShortTimeString() + "'  "+
                        " where (id = '" + lblUid.Text + "' )"; 
                        DataAccess.ExecuteSQL(sql);        

    
                        /////////////////////////////////////////////Update image //////////////////////////////////////////////////////
                        if (lblFileExtension.Text != "user.png")
                        {
                            picUserimage.InitialImage.Dispose();
                            string path = Application.StartupPath + @"\IMAGE\";
                            System.IO.File.Delete(path + @"\" + lblimagename.Text);
                            if (!System.IO.Directory.Exists(path))
                                System.IO.Directory.CreateDirectory(Application.StartupPath + @"\IMAGE\");
                            string filename = path + @"\" + openFileDialog1.SafeFileName;
                            picUserimage.Image.Save(filename, System.Drawing.Imaging.ImageFormat.Png);
                            System.IO.File.Move(path + @"\" + openFileDialog1.SafeFileName, path + @"\" + imageName);
                        }
     
                        MessageBox.Show("Successfully Data Updated!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblEmailerrormsg.Visible = false;
                        loadData(lblUid.Text);
                    }
 
                }
                catch (Exception exp)
                {
                  //  MessageBox.Show("Sorry\r\n" + exp.Message);
                }
            }
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
                        string sql = "delete from usermgt where   (id = '" + lblUid.Text + "' )"; 
                        DataAccess.ExecuteSQL(sql);

                        picUserimage.InitialImage.Dispose();
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
                        MessageBox.Show("Sorry\r\n You have to Check the Data" + exp.Message);
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
            string sql5 = "select Shopid  from tbl_terminallocation where Branchname = '"+ cmboShopid .Text+ "' ";
            ShopNametextBox.Text =  DataAccess.ExecuteSQLScaler(sql5);
        }
    }
}
