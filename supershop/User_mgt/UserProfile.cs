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
    public partial class UserProfile : Form
    {
        public UserProfile(string UName)
        {
            InitializeComponent();
            lblUserName.Text = UName;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        } 
        

        ///// Load Method
        public void loadData()
        {
            // The stored password is a hash and is never shown; an empty box means "keep the current password".
            DataTable dt1 = DataAccess.GetDataTable(
                "select id, Name, Father_name, Address, Email, Contact, DOB, Username, position, imagename from usermgt where Username = @u",
                DataAccess.P("@u", lblUserName.Text));
            if (dt1.Rows.Count == 0)
            {
                MessageBox.Show("User '" + lblUserName.Text + "' was not found.", "User profile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            DataRow r = dt1.Rows[0];

            txtuid.Text = r["id"].ToString();
            txtUserFullName.Text = r["Name"].ToString();
            txtFatherName.Text = r["Father_name"].ToString();
            txtAddress.Text = r["Address"].ToString();
            txtEmailaddress.Text = r["Email"].ToString();
            txtContact.Text = r["Contact"].ToString();
            try { dtDOB.Value = Convert.ToDateTime(r["DOB"].ToString()); } catch (Exception) { }
            txtUsername.Text = r["Username"].ToString();
            textUserPass.Text = "";
            label9.Text = "Password (leave blank to keep)";
            rdbtnUserRole.Text = r["position"].ToString();
            lblimagename.Text = r["imagename"].ToString();
            lblBranch.Text = UserInfo.Shopid;
            PicUserPhoto.ImageLocation = Application.StartupPath + @"\IMAGE\" + r["imagename"];
        }

        //Load event | user info 
        private void UserProfile_Load(object sender, EventArgs e)
        {
            loadData();
        }

        //Browse Picture Dialog
        private void btnChangePic_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            //openFileDialog1.InitialDirectory = @"C:\";
            //openFileDialog1.Title = "Browse Text Files";

            openFileDialog1.CheckFileExists = true;
            openFileDialog1.CheckPathExists = true;

            openFileDialog1.DefaultExt = "jpg";
           // openFileDialog1.Filter = "GIF files (*.gif)|*.gif| jpg files (*.jpg)|*.jpg| PNG files (*.png)|*.png| All files (*.*)|*.*";
             openFileDialog1.Filter = "jpg files (*.jpg)|*.jpg";

            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;

           // openFileDialog1.ReadOnlyChecked = true;
           // openFileDialog1.ShowReadOnly = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // textBox1.Text = openFileDialog1.FileName;
                PicUserPhoto.ImageLocation = openFileDialog1.FileName;
                lblFileExtension.Text = Path.GetExtension(openFileDialog1.FileName);
            }
        }

        //Update user info
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtUserFullName.Text == "")
            {
                MessageBox.Show("Please Add User full Name", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtUserFullName.Focus();
            }
            else if (txtFatherName.Text == "")
            {
                MessageBox.Show("Please fill fathers name", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtFatherName.Focus();
            }
            else if (txtAddress.Text == "")
            {
                MessageBox.Show("Please Add Address", "Fill Field", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtAddress.Focus();
            }
            else if (txtContact.Text == "")
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
            else
            {
                try
                {
                    bool pictureChanged = lblFileExtension.Text != "user.png";
                    string imageName = pictureChanged ? txtuid.Text + lblFileExtension.Text : lblimagename.Text;

                    // An empty password box means "keep the current password"; otherwise store a hash.
                    string passwordSet = textUserPass.Text == "" ? "" : ", password = @p ";
                    string sql = "UPDATE usermgt set Name = @name, Father_name = @father, Address = @addr, Email = @email, Contact = @contact, " +
                                 " DOB = @dob, Username = @newuser, imagename = @img " + passwordSet +
                                 " where Username = @u";
                    DataAccess.ExecuteSQL(sql,
                        DataAccess.P("@name", txtUserFullName.Text),
                        DataAccess.P("@father", txtFatherName.Text),
                        DataAccess.P("@addr", txtAddress.Text),
                        DataAccess.P("@email", txtEmailaddress.Text),
                        DataAccess.P("@contact", txtContact.Text),
                        DataAccess.P("@dob", dtDOB.Value.ToString("yyyy-MM-dd")),
                        DataAccess.P("@newuser", txtUsername.Text),
                        DataAccess.P("@img", imageName),
                        DataAccess.P("@p", textUserPass.Text == "" ? null : PasswordHasher.Hash(textUserPass.Text)),
                        DataAccess.P("@u", lblUserName.Text));
                    lblUserName.Text = txtUsername.Text;
                    MessageBox.Show("Successfully Data Updated!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Save the new picture under the user's image name
                    if (pictureChanged && PicUserPhoto.Image != null)
                    {
                        string path = Application.StartupPath + @"\IMAGE\";
                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);
                        System.IO.File.Delete(path + imageName);
                        PicUserPhoto.Image.Save(path + imageName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }

                    loadData();
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save the profile.");
                }
            }
        }

        private void lnkWorkingHours_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            UserInfo.usernamWK = txtUsername.Text;
           // this.Hide();
            User_mgt.WorkRecords go = new User_mgt.WorkRecords();
           // go.MdiParent = this.ParentForm;
            go.ShowDialog();
        }

    }
}
