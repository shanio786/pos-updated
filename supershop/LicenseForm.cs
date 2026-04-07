using supershop.AdminPanelForms;
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
    public partial class LicenseForm : Form
    {
        sellomat_Entities _context = new sellomat_Entities();
        LicenseModel lic = new LicenseModel();
        //public string MyVar { get { return CodetextBox.Text; } }
        //public long CodeText { get; set; }
        //LicenseKeyModule lic = new LicenseKeyModule();
        public LicenseForm()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            var CodeText = Convert.ToInt32(CodetextBox.Text);
            try
            {
                var a = _context.tbl_userlicense.Single(c => c.Id == CodeText);
                var dtm = a.ToDate;
                //LicenseKeyModule lkm = new LicenseKeyModule();
                //DateTime dtm = new DateTime(2020, 12, 30);
                var b = DateTime.Now;
                if (dtm <= b)
                {
                    MessageBox.Show("License Expired");
                    LicenseForm lf = new LicenseForm();
                    lf.Show();
                    this.Close();
                }
                else
                {
                    if (a.Status == true)
                    {
                        //lic.id = 1;
                        //lic.Licensekey = "WADFC";
                        //lic.Licensekey2 = "DQCRG";
                        //lic.Licensekey3 = "HM64M";
                        //lic.Licensekey4 = "6GJRK";
                        //lic.Licensekey5 = "8K83T";
                        //lic.Date = "24/11/2020";

                        string k1, k2, k3, k4, k5;
                        k1 = textBox1.Text;
                        k2 = textBox2.Text;
                        k3 = textBox3.Text;
                        k4 = textBox4.Text;
                        k5 = textBox5.Text;

                        if (k1 == a.LicenseKey1 && k2 == a.LicenseKey2 && k3 == a.LicenseKey3 && k4 == a.LicenseKey4 && k5 == a.LicenseKey5)
                        {
                            MessageBox.Show("License Activated");
                            a.Status = false;
                            Login log = new Login();
                            this.Hide();
                            log.Show();
                        }
                        else
                        {
                            MessageBox.Show("Invalid License Key !");
                        }
                    }
                    else
                    {
                        MessageBox.Show("License Key not activated !");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("" + ex);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == textBox1.MaxLength)
            {
                textBox2.Focus();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == textBox2.MaxLength)
            {
                textBox3.Focus();
            }
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.Length == textBox3.MaxLength)
            {
                textBox4.Focus();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (textBox4.Text.Length == textBox4.MaxLength)
            {
                textBox5.Focus();
            }
        }

        private void btnclose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CodetextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar);
        }
    }
}
