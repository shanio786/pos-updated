using supershop.AdminPanelForms;
using supershop.Customer;
using supershop.User_mgt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Login log = new Login();
            log.Show();
        }

        private void generateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GenerateLicenseForm glf = new GenerateLicenseForm();
            glf.Show();
        }

        private void createNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User_regi ur = new User_regi();
            ur.Show();
        }

        private void addNewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CustomerForm cf = new CustomerForm();
            cf.Show();
        }

        private void listToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            CustomerDetails cd = new CustomerDetails();
            cd.Show();
        }
    }
}
