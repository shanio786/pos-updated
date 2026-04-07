using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.AdminPanelForms
{
    public partial class AdminLoginForm : Form
    {
        public AdminLoginForm()
        {
            InitializeComponent();
            PasswordtextBox.MaxLength = 20;
            PasswordtextBox.PasswordChar = '*';
        }

        private void Loginbutton_Click(object sender, EventArgs e)
        {
            try
            {
                if (UsernametextBox.Text == "admin" && PasswordtextBox.Text == "admin")
                {
                    AdminPanel ap = new AdminPanel();
                    ap.Show();
                }
                else
                {
                    MessageBox.Show("Invalid Username or Password!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(""+ex);
            }
        }
    }
}
