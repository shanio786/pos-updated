using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.Inventory
{
    public partial class StockShortList : Form
    {
        public StockShortList()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
               this.Close();            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void StockShortList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }
        

        private void loadData()
        {
            string sql = "select product_id as 'Code/ID', product_name as 'Item Name' , product_quantity as 'Qty/Stock-Item' ," +
                "  retail_price as 'Sale Price' , category as 'Category' , supplier as 'Supplier'      from purchase";
            DataTable dt1 = DataAccess.GetDataTable(sql);
            dtgrdViewStockItem.DataSource = dt1;
            dtgrdViewStockItem.Columns[0].ReadOnly = false;
            dtgrdViewStockItem.Columns[1].ReadOnly = true;
            dtgrdViewStockItem.Columns[2].ReadOnly = true;
            dtgrdViewStockItem.Columns[3].ReadOnly = true;
            dtgrdViewStockItem.Columns[4].ReadOnly = true;
            dtgrdViewStockItem.Columns[5].ReadOnly = true;

            dtgrdViewStockItem.Columns[0].ToolTipText = " Click on Code/ID row it's automatically copied ";
            if (dtgrdViewStockItem.Rows.Count > 0)
                dtgrdViewStockItem.Rows[0].Cells[0].ToolTipText = " Click on Code/ID row it's automatically copied ";

            DataGridViewColumn ColName = dtgrdViewStockItem.Columns[1];
            ColName.Width = 151;
        }
        private void txtItemSearchBar_TextChanged(object sender, EventArgs e)
        {
            if (txtItemSearchBar.Text != "")
            {
                try
                {
                    string sql = "select product_id as 'Code/ID', product_name as 'Item Name' , product_quantity as 'Qty/Stock-Item' , " +
                                   "  cost_price as 'Cost Price' , retail_price as 'Sale Price' , category as 'Category' , supplier as 'Supplier'   " +
                                   "   from purchase where product_id like '%' + @q + '%' or " +
                                   "  product_name like '%' + @q + '%' or category like '%' + @q + '%' " +
                                   "  or supplier like '%' + @q + '%' ";
                    DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@q", txtItemSearchBar.Text));
                    dtgrdViewStockItem.DataSource = dt1;
                }
                catch (Exception exLog) { Logger.Error(exLog); }
            }
        }

        private void StockShortList_Load(object sender, EventArgs e)
        {
            try
            {
                loadData();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void dtgrdViewStockItem_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dtgrdViewStockItem.Rows[e.RowIndex];
                Clipboard.SetText(row.Cells[0].Value.ToString());   // product code copied to clipboard
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void dtgrdViewStockItem_KeyDown(object sender, KeyEventArgs e)
        {
        }
    }
}
