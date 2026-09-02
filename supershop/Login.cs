using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Mail;
using System.Net;
using System.Management;
using System.Net.NetworkInformation;
using supershop.AdminPanelForms;

namespace supershop
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
            btnColse.Enabled = false;
          //  txtUserName.Text = "a";
           // txtPassword.Text = "a";
        }

        //Log in action 
        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text == "")
            {
                MessageBox.Show("Please insert User Name", "Not match", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUserName.Focus();
                return;
            }
            if (txtPassword.Text == "")
            {
                MessageBox.Show("Please  insert Password", "Not match", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return;
            }

            try
            {
                DataTable dt = DataAccess.GetDataTable(
                    "SELECT id, Username, password, usertype, Shopid FROM usermgt WHERE Username = @u",
                    DataAccess.P("@u", txtUserName.Text.Trim()));

                if (dt.Rows.Count == 0 || !PasswordHasher.Verify(txtPassword.Text, Convert.ToString(dt.Rows[0]["password"])))
                {
                    lblmsg.Visible = true;
                    lblmsg.Text = "Username or Password does not match";
                    return;
                }

                string username = Convert.ToString(dt.Rows[0]["Username"]);
                string stored   = Convert.ToString(dt.Rows[0]["password"]);
                string usertype = Convert.ToString(dt.Rows[0]["usertype"]);
                string shopid   = Convert.ToString(dt.Rows[0]["Shopid"]);

                // upgrade an old plain-text password to a hash on first successful login
                if (!PasswordHasher.IsHashed(stored))
                {
                    DataAccess.ExecuteSQL("UPDATE usermgt SET password = @p WHERE id = @id",
                        DataAccess.P("@p", PasswordHasher.Hash(txtPassword.Text)), DataAccess.P("@id", dt.Rows[0]["id"]));
                }

                if (usertype == "0") // Blocked user
                {
                    MessageBox.Show("\n This user (" + username + ") has been blocked. \n Please contact to administrator.", "Block - Inactive", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }

                UserInfo.Userid   = Convert.ToString(dt.Rows[0]["id"]);
                UserInfo.UserName = username;
                UserInfo.usertype = usertype; // 1 = admin, 2 = manager, 3 = salesman
                UserInfo.Shopid   = shopid;
                workRecords();

                Form home;
                if (usertype == "1")      home = new Home();
                else if (usertype == "2") home = new Manager_Home();
                else if (usertype == "3") home = new SalesMan_Home();
                else
                {
                    MessageBox.Show("Unknown user type '" + usertype + "'. Please contact the administrator.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return;
                }
                home.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                Logger.Show(ex, "Login failed because of a database error.");
            }
        }

        public void workRecords()
        {
            DateTime now = DateTime.Now;
            DataAccess.ExecuteSQL(
                " insert into tbl_workrecords (Username, datatype, logdate, logtime, logdatetime) values (@u, 'IN', @d, @t, @dt)",
                DataAccess.P("@u", UserInfo.UserName),
                DataAccess.P("@d", now.ToString("yyyy-MM-dd")),
                DataAccess.P("@t", now.ToString("HH:mm:ss")),
                DataAccess.P("@dt", now.ToString("yyyy-MM-dd HH:mm:ss")));
        }

        private void label3_Click(object sender, EventArgs e)
        {
            //this.Close();
          //  Application.Exit();
            Environment.Exit(0);
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            Login g = new Login();
            g.Show();
            this.Hide();
        }
        
        private void timer1_Tick(object sender, EventArgs e)
        {
            prg(); //pictureBox2.Visible = true;
        }

        public void prg()
        { 
            progressBar1.Increment(5);
            lblprgbarCount.Text = " " + progressBar1.Value.ToString() + "%";
            if (progressBar1.Value == progressBar1.Maximum)
            {
                timer1.Stop();
                // MessageBox.Show("Server has been connected");
                // this.Close();
                //timer1.Stop();
                btnColse.Enabled = true;
            }
        }

        private void Login_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void btnColse_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
