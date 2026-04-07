using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.Items
{
    public partial class Add_Category : Form
    {

        public Add_Category()
        {
            InitializeComponent();
        }
	
	protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        public string categoryID
        {
            set { lblID.Text = value;} 
            get{return lblID.Text;}
        }
        public string categoryName
        {
            set { txtCategoryName.Text = value; btnSave.Text = "Update"; }
            get { return txtCategoryName.Text; }
        }


        private void lnkCategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                 if (txtCategoryName.Text == "") 
                 { 
                     MessageBox.Show("Please Fill  Category Name"); 
                     txtCategoryName.Focus(); 
                 }
                else
                 {
                     if (lblID.Text == "-")
                     {
                         string sqlCmd = " insert into tbl_category (category_name)  values ('" + txtCategoryName.Text + "'  )";
                         DataAccess.ExecuteSQL(sqlCmd);
                         txtCategoryName.Text = "";
                         lblMsg.Visible = true;
                         lblMsg.Text = "Successfully saved";

                     }
                     else  //Update 
                     {
                         string sqlUpdateCmd = " update tbl_category set category_name = '" + txtCategoryName.Text + "'   where ID = '" + lblID.Text + "'";
                         DataAccess.ExecuteSQL(sqlUpdateCmd);
                         // lblMsg.Visible = true;
                         // lblMsg.Text = "Successfully Updated";
                         this.Hide();
                         Items.Categories mkc = new Items.Categories();
                         mkc.MdiParent = this.ParentForm;
                         mkc.Show();
                     }
                 }         


            }
            catch (Exception exp)
            {
                MessageBox.Show("Sorry\r\n this id already added \n\n " + exp.Message);
            }
        }
    }
}
