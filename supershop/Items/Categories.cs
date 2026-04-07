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
    public partial class Categories : Form
    {
        public Categories()
        {
            InitializeComponent();
        }

        private void lnkAddcategory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Items.Add_Category go = new Items.Add_Category();
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

        public void categorybind()
        {
            string sql = " select ID, category_name as 'Category' from tbl_category ";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            datagridcategories.DataSource = dt1;
        }

        private void Categories_Load(object sender, EventArgs e)
        {
            try
            {         
              
                categorybind();  
                DataGridViewButtonColumn Edit = new DataGridViewButtonColumn();
                datagridcategories.Columns.Add(Edit);
                Edit.HeaderText = "Edit";
                Edit.Text = "Edit";
                Edit.Name = "Edit";
                Edit.ToolTipText = "Edit this category";
                Edit.UseColumnTextForButtonValue = true;

                DataGridViewButtonColumn del = new DataGridViewButtonColumn();
                datagridcategories.Columns.Add(del);
                del.HeaderText = "Del";
                del.Text = "X";
                del.Name = "del";
                del.ToolTipText = "Delete this category";
                del.UseColumnTextForButtonValue = true;

                DataGridViewColumn ColID = datagridcategories.Columns[0];
                ColID.Width = 31;
                DataGridViewColumn ColName = datagridcategories.Columns[1];
                ColName.Width = 220;
            }
            catch
            {
            }
        }

        private void datagridcategories_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // Delete category  
                if (e.ColumnIndex == datagridcategories.Columns["del"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow rowdel in datagridcategories.SelectedRows)
                    {
                        DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.Yes)
                        {

                            string sqldel = " delete from tbl_category     where ID = '" + rowdel.Cells[2].Value.ToString() + "'";
                            DataAccess.ExecuteSQL(sqldel);                           
                            MessageBox.Show("Deleted");
                            categorybind();
                        }
                    }
                }

                // Delete items From Gridview
                if (e.ColumnIndex == datagridcategories.Columns["Edit"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow row in datagridcategories.SelectedRows)
                    {
                        this.Hide();
                        Items.Add_Category mkc = new Items.Add_Category();
                        mkc.categoryID = row.Cells[2].Value.ToString();
                        mkc.categoryName = row.Cells[3].Value.ToString();
                        mkc.MdiParent = this.ParentForm;
                        mkc.Show();
                    }
                }           

            }
            catch  
            {
               
            }
        }
    }
}
