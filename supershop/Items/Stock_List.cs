using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;

namespace supershop
{
    public partial class Stock_List : Form
    {
        public Stock_List()
        {
            InitializeComponent();
        }

        #region Data bind
        // Fills the list view with items whose name / code / category starts with the given text
        public void ItemList_with_images(string value, ListView _lst_items)
        {
            try
            {
                string sql = " select * from vw_itemdisplay_sr " +
                             " where product_name like @q + '%' or product_id like @q + '%' or category like @q + '%'";
                DataTable dt = DataAccess.GetDataTable(sql, DataAccess.P("@q", value));
                lblRows.Text = "Total Rows " + dt.Rows.Count.ToString() + " Found";

                _lst_items.Items.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    ListViewItem lst = new ListViewItem(dr["product_id"].ToString());
                    lst.SubItems.Add(dr["taxapply"].ToString() == "1" ? "YES" : "NO");
                    lst.SubItems.Add(dr["product_name"].ToString());
                    lst.SubItems.Add(dr["product_quantity"].ToString());
                    lst.SubItems.Add(dr["cost_price"].ToString());
                    lst.SubItems.Add(dr["retail_price"].ToString());
                    lst.SubItems.Add(dr["discount"].ToString());
                    lst.SubItems.Add(dr["category"].ToString());
                    lst.SubItems.Add(dr["supplier"].ToString());
                    _lst_items.Items.Add(lst);
                }

                _lst_items.Visible = (_lst_items.Items.Count > 0);
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        //Go to Item Details page
        protected void b_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string s = b.Tag.ToString();

            this.Hide();
            Add_Item go = new Add_Item();
            go.itemCode = s;
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        //Product filter by Category
        private void combCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            ItemList_with_images(combCategory.Text, lst_items);
        }

        //Product filter by Product Name or Product ID
        private void txtItemSearchBar_TextChanged(object sender, EventArgs e)
        {
            ItemList_with_images(txtItemSearchBar.Text, lst_items);
        }

        private void detail_info_Load(object sender, EventArgs e)
        {
            try
            {
                //Product Category
                string sql5 = "select DISTINCT category from purchase";
                DataTable dt5 = DataAccess.GetDataTable(sql5);
                combCategory.DataSource = dt5;
                combCategory.DisplayMember = "category";
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        #endregion

        #region page links
        private void btnCreateBarcode_Click(object sender, EventArgs e)
        {
            BarCode.Barcode_machine go = new BarCode.Barcode_machine();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void btnChart_Click(object sender, EventArgs e)
        {
            Chart g = new Chart();
            g.MdiParent = this.ParentForm;
            g.Show();
        }

        private void picCloseEvent_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            this.Hide();
            Import_Items go = new Import_Items();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            this.Hide();
            Add_Item go = new Add_Item();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void bntStock_Click(object sender, EventArgs e)
        {
            this.Hide();
            Items.StockDetails go = new Items.StockDetails();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void btnpurchasehistory_Click(object sender, EventArgs e)
        {
            this.Hide();
            Items.Purchase_History go = new Items.Purchase_History();
            go.MdiParent = this.ParentForm;
            go.Show();
        }
        #endregion

        // toolbar
        private void lblMinimized_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;   //Minimized
        }

        private void detail_info_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void txtItemSearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (lst_items.Items.Count > 0)
                {
                    lst_items.Focus();
                    lst_items.Items[0].Selected = true;
                }
            }

            if (e.KeyCode == Keys.Up)
            {
                if (lst_items.Items.Count > 0 && lst_items.Items[0].Selected == true)
                {
                    txtItemSearchBar.Focus();
                    txtItemSearchBar.SelectAll();
                }
            }
        }

        private void lst_items_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13 && lst_items.SelectedItems.Count > 0)
                txtItemSearchBar.Text = lst_items.SelectedItems[0].SubItems[0].Text;
        }

        private void lst_items_Click(object sender, EventArgs e)
        {
            if (lst_items.SelectedItems.Count == 0) return;
            string s = lst_items.SelectedItems[0].SubItems[0].Text;

            this.Hide();
            Add_Item go = new Add_Item();
            go.itemCode = s;
            go.MdiParent = this.ParentForm;
            go.Show();
        }
    }
}
