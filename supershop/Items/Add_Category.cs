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
            set { lblID.Text = value; }
            get { return lblID.Text; }
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
                        DataAccess.ExecuteSQL("insert into tbl_category (category_name) values (@name)",
                            DataAccess.P("@name", txtCategoryName.Text));
                        txtCategoryName.Text = "";
                        lblMsg.Visible = true;
                        lblMsg.Text = "Successfully saved";
                    }
                    else  //Update 
                    {
                        DataAccess.ExecuteSQL("update tbl_category set category_name = @name where ID = @id",
                            DataAccess.P("@name", txtCategoryName.Text),
                            DataAccess.P("@id", Convert.ToInt64(lblID.Text)));
                        this.Hide();
                        Items.Categories mkc = new Items.Categories();
                        mkc.MdiParent = this.ParentForm;
                        mkc.Show();
                    }
                }
            }
            catch (Exception exp)
            {
                Logger.Show(exp, "Could not save category");
            }
        }
    }
}
