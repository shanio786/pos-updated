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
    public partial class CustomerForm : Form
    {
        sellomat_Entities _context = new sellomat_Entities();

        public CustomerForm()
        {
            InitializeComponent();
        }
        public void LoadList()
        {
            var a = _context.tbl_customerdetails.ToList();
            dataGridView1.DataSource = a;
        }
        public void ResetFields()
        {
            CustomerNametextBox.Text = "";
            ContacttextBox.Text = "";
            EmailtextBox.Text = "";
            StoreNametextBox.Text = "";
            AddresstextBox.Text = "";
            PricetextBox.Text = "";
            LicenseCodetextBox.Text = "";
        }
        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadList();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ResetFields();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            var toBeDeleted = Convert.ToInt32(dataGridView1.SelectedCells[0].Value.ToString());
            var a = _context.tbl_customerdetails.Single(c => c.Id == toBeDeleted);
            _context.tbl_customerdetails.Remove(a);
            _context.SaveChanges();
            LoadList();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            tbl_customerdetails customerdetails = new tbl_customerdetails();
            customerdetails.Contact = ContacttextBox.Text;
            customerdetails.CustomerName = CustomerNametextBox.Text;
            customerdetails.EmailAddress = EmailtextBox.Text;
            customerdetails.LicenseCode = LicenseCodetextBox.Text;
            customerdetails.PermanentAddress = AddresstextBox.Text;
            customerdetails.Price = Convert.ToInt32(PricetextBox.Text);
            customerdetails.StoreName = StoreNametextBox.Text;
            _context.tbl_customerdetails.Add(customerdetails);
            _context.SaveChanges();
            MessageBox.Show("Customer added successfully!");
            LoadList();
            ResetFields();
        }
    }
}
