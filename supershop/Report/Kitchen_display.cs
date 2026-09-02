using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace supershop.Report
{
    public partial class Kitchen_display : Form
    {
        public Kitchen_display()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //Show pending kitchen items as buttons with the product image
        public void ItemList_with_images()
        {
            flowLayoutPanelUserList.Controls.Clear();
            string img_directory = Path.Combine(Application.StartupPath, "ITEMIMAGE");
            try
            {
                string sql = " SELECT si.item_id as ID, si.sales_id as 'ReceiptNo', si.itemName as 'ItemName', sp.comment as 'Note', " +
                             "  si.Qty, si.Total, si.sales_time as 'Date', si.itemcode, p.imagename, sp.emp_id, " +
                             "  CASE WHEN si.status = 3 THEN 'Pending' WHEN si.status = 1 THEN 'Served' END 'Status' " +
                             "  FROM sales_item si " +
                             "  left join sales_payment sp ON si.sales_id = sp.sales_id " +
                             "  left join purchase p ON p.product_id = si.itemcode " +
                             "  where si.status = 3 and si.Qty <> 0 " +
                             "  order by si.item_id asc ";
                DataTable dt = DataAccess.GetDataTable(sql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataReader = dt.Rows[i];

                    Button b = new Button();
                    b.Tag = dataReader["ReceiptNo"];
                    b.Click += new EventHandler(b_Click);

                    toolTip1.ToolTipTitle = "Click to Order Ready";
                    toolTip1.SetToolTip(b, "Press click to serve complete");

                    // product image (skipped when the file is missing)
                    string imageName = Convert.ToString(dataReader["imagename"]);
                    string imagePath = Path.Combine(img_directory, imageName);
                    if (imageName != "" && File.Exists(imagePath))
                    {
                        try
                        {
                            ImageList il = new ImageList();
                            il.ColorDepth = ColorDepth.Depth32Bit;
                            il.TransparentColor = Color.Transparent;
                            il.ImageSize = new Size(96, 96);
                            il.Images.Add(Image.FromFile(imagePath));   // ImageList copies it when the handle is created
                            b.Image = il.Images[0];
                        }
                        catch (Exception exImg) { Logger.Error(exImg); }
                    }

                    b.Margin = new Padding(3, 3, 3, 3);
                    b.Size = new Size(200, 300);

                    b.Text = " ========================= ";
                    b.Text += "\n Order # " + dataReader["ReceiptNo"];
                    b.Text += "\n Staff: " + dataReader["emp_id"];
                    b.Text += "\n Date: " + dataReader["Date"];
                    b.Text += "\n ========================= ";
                    b.Text += "\n " + Convert.ToString(dataReader["ItemName"]);
                    b.Text += "\n Qty: " + dataReader["Qty"];
                    b.Text += "\n Note: " + dataReader["Note"];

                    b.Font = new Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point);
                    b.TextAlign = ContentAlignment.MiddleLeft;
                    b.TextImageRelation = TextImageRelation.ImageAboveText;
                    flowLayoutPanelUserList.Controls.Add(b);
                }
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        //Click to Served
        protected void b_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if (b == null || b.Tag == null)
                return;

            Report.KD_dialog go = new Report.KD_dialog(b.Tag.ToString());
            go.ShowDialog();
        }

        public void kitchen_displayDataload()
        {
            string sql = " SELECT si.item_id as ID, si.sales_id as 'Receipt No', si.itemName as 'Item Name', sp.comment as 'Note', si.Qty, si.Total, si.sales_time as 'Date', " +
                         "  CASE WHEN si.status = 3 THEN 'Pending' WHEN si.status = 1 THEN 'Served' END 'Status' " +
                         "  FROM sales_item si " +
                         "  left join sales_payment sp ON si.sales_id = sp.sales_id " +
                         "  where si.status = 3 " +
                         "  order by si.sales_id desc ";
            DataTable dt1 = DataAccess.GetDataTable(sql);
            dtgridKitchenWaitingList.DataSource = dt1;
        }

        private void Kitchen_display_Load(object sender, EventArgs e)
        {
            try
            {
                ItemList_with_images();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not load the kitchen display."); }
        }

        // Mark the clicked line (column 0 = sales_item.item_id) as served
        private void dtgridKitchenWaitingList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0)
                    return;

                DataGridViewRow row = dtgridKitchenWaitingList.Rows[e.RowIndex];
                long itemId;
                if (!long.TryParse(Convert.ToString(row.Cells[0].Value), out itemId))
                    return;

                DataAccess.ExecuteSQL("update sales_item set status = 1 where item_id = @id", DataAccess.P("@id", itemId));
                kitchen_displayDataload();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not update the order."); }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                ItemList_with_images();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
    }
}
