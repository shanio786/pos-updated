using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.AdminPanelForms
{
    public partial class GenerateLicenseForm : Form
    {
        sellomat_Entities _context = new sellomat_Entities();
        private DataGridView LicenseDataGridView = new DataGridView();
        public GenerateLicenseForm()
        {
            InitializeComponent();
        }

        public void LoadList()
        {
            var a = _context.tbl_userlicense.ToList();
            dataGridView1.DataSource = a;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            LicensetextBox1.Text = "";
            LicensetextBox2.Text = "";
            LicensetextBox3.Text = "";
            LicensetextBox4.Text = "";
            LicensetextBox5.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                tbl_userlicense tul = new tbl_userlicense();
                tul.LicenseKey1 = LicensetextBox1.Text;
                tul.LicenseKey2 = LicensetextBox2.Text;
                tul.LicenseKey3 = LicensetextBox3.Text;
                tul.LicenseKey4 = LicensetextBox4.Text;
                tul.LicenseKey5 = LicensetextBox5.Text;
                tul.RegistrationDate = DateTime.Now;
                tul.FromDate = Convert.ToDateTime(FromdateTimePicker.Text);
                tul.ToDate = Convert.ToDateTime(TodateTimePicker.Text);
                tul.Status = true;
                _context.tbl_userlicense.Add(tul);
                _context.SaveChanges();
                MessageBox.Show("License Key Generated");
                LoadList();
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error: "+ex);
            }
            //LicenseKeyModule lkm =new LicenseKeyModule();
            //lkm.Id = 1;
            //lkm.LicenseKey1 = LicensetextBox1.Text;
            //lkm.LicenseKey2 = LicensetextBox2.Text;
            //lkm.LicenseKey3 = LicensetextBox3.Text;
            //lkm.LicenseKey4 = LicensetextBox4.Text;
            //lkm.LicenseKey5 = LicensetextBox5.Text;
            //lkm.RegistrationDate = DateTime.Now;

            //DateTime dtm = new DateTime(2020, 12, 14);
            //var a = DateTime.Now;
        }

        private void LicensetextBox1_TextChanged(object sender, EventArgs e)
        {
            if (LicensetextBox1.Text.Length == LicensetextBox1.MaxLength)
            {
                LicensetextBox2.Focus();
            }
        }

        private void LicensetextBox2_TextChanged(object sender, EventArgs e)
        {
            if (LicensetextBox2.Text.Length == LicensetextBox2.MaxLength)
            {
                LicensetextBox3.Focus();
            }
        }

        private void LicensetextBox3_TextChanged(object sender, EventArgs e)
        {
            if (LicensetextBox3.Text.Length == LicensetextBox3.MaxLength)
            {
                LicensetextBox4.Focus();
            }
        }

        private void LicensetextBox4_TextChanged(object sender, EventArgs e)
        {
            if (LicensetextBox4.Text.Length == LicensetextBox4.MaxLength)
            {
                LicensetextBox5.Focus();
            }
        }

        private void GenerateLicenseForm_Load(object sender, EventArgs e)
        {
            LoadList();
            //this.tbl_userlicenseTableAdapter.Fill(this.db_licenseDataSet.tbl_userlicense);
        }
    }
}
