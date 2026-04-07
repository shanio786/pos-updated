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

        //Show Kitchen item Products with images
        public void ItemList_with_images()
        {
            flowLayoutPanelUserList.Controls.Clear();
            string img_directory = Application.StartupPath + @"\ITEMIMAGE\";
            string[] files = Directory.GetFiles(img_directory, "*.png *.jpg");
            try
            {
                string sql = " SELECT  si.item_id as ID ,   si.sales_id as 'ReceiptNo' ,    si.itemName as 'ItemName' ,   sp.comment as 'Note', " +
                        "  si.Qty ,   si.Total ,   si.sales_time as 'Date', si.itemcode, p.imagename,  sp.emp_id , " +
                         "  CASE   " +
                         "  WHEN si.status = 3 THEN 'Pending' " +
                         "  WHEN si.status = 1 THEN 'Served'  " +
                         "  END 'Status' " +
                         "  FROM  sales_item si " +
                         "  left join  sales_payment sp " +
                         "  ON si.sales_id = sp.sales_id " +
                         "  left join purchase p " +
						 " ON p.product_id = si.itemcode " +
                         "  where si.status = 3   and  si.Qty != 0 " +
                         "  order by si.item_id asc ";
                DataAccess.ExecuteSQL(sql);
                DataTable dt = DataAccess.GetDataTable(sql);

                int currentImage = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dataReader = dt.Rows[i];

                    Button b = new Button();
                    b.Tag = dataReader["ReceiptNo"];
                    b.Click += new EventHandler(b_Click);
                     
                     
                    //b.Name = details;
                    toolTip1.ToolTipTitle = "Click to Order Ready";
                    toolTip1.SetToolTip(b, "Press click to serve complete");

                    ImageList il = new ImageList();
                    il.ColorDepth = ColorDepth.Depth32Bit;
                    il.TransparentColor = Color.Transparent;
                    il.ImageSize = new Size(96, 96);
                    il.Images.Add(Image.FromFile(img_directory + dataReader["imagename"]));


                    b.Image = il.Images[0];
                    b.Margin = new Padding(3, 3, 3, 3);

                    b.Size = new Size(200, 300);
                    b.Text.PadRight(4);

                    b.Text += " ========================= ";                    
                    b.Text += "\n Order # " + dataReader["ReceiptNo"];
                    b.Text += "\n Staff: " + dataReader["emp_id"];
                    b.Text += "\n Date: " + dataReader["Date"];
                    b.Text += "\n ========================= ";
                    b.Text += "\n " + dataReader["ItemName"].ToString();
                    b.Text += "\n Qty: " + dataReader["Qty"];
                   // b.Text += "\n Total: " + dataReader["Total"];
                    b.Text += "\n Note: " + dataReader["Note"];                   
                    
                     

                    b.Font = new Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Point);
                    b.TextAlign = ContentAlignment.MiddleLeft;
                    b.TextImageRelation = TextImageRelation.ImageAboveText;
                    flowLayoutPanelUserList.Controls.Add(b);
                    currentImage++;

                }
            }
            catch //(Exception)
            {

                //throw;
            }
        }
 
           //Click to Served
        protected void b_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
          //  string s = b.Tag.ToString();
            //s = b.Tag;
           // s += b.Tag;
            //s += "\n Name: ";
            //s += b.Name.ToString();
           // MessageBox.Show(b.Tag.ToString());

            Report.KD_dialog go = new Report.KD_dialog(b.Tag.ToString());
            go.ShowDialog();

            //string sql = " update sales_item set " +
            //               " status = 1 " +
            //               " where item_id  = '" + b.Tag.ToString() + "' ";
            //DataAccess.ExecuteSQL(sql);
            //DataTable dt1 = DataAccess.GetDataTable(sql);
            //ItemList_with_images();
        }

        public void kitchen_displayDataload()
        {
            string sql = " SELECT  si.item_id as ID ,   si.sales_id as 'Receipt No' ,    si.itemName as 'Item Name' ,   sp.comment as 'Note',  si.Qty ,   si.Total ,   si.sales_time as 'Date', " +
                     "  CASE   " +
                     "  WHEN si.status = 3 THEN 'Pending' " +
                     "  WHEN si.status = 1 THEN 'Served'  " +
                     "  END 'Status' " +
                     "  FROM  sales_item si " +
                     "  left join  sales_payment sp " +
					 "  ON si.sales_id = sp.sales_id " +
                     "  where si.status = 3 " +
                     "  order by si.sales_id desc ";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            dtgridKitchenWaitingList.DataSource = dt1;
          //  dtgridKitchenWaitingList.Columns[5].DefaultCellStyle.ForeColor = Color.DarkViolet;
        }

        private void Kitchen_display_Load(object sender, EventArgs e)
        {
            try
            {
                ItemList_with_images();
               
                //DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                //dtgridKitchenWaitingList.Columns.Add(btn);
                //btn.HeaderText = "Action";
                //btn.Text = "Served";
                //btn.Name = "btn";
                //btn.UseColumnTextForButtonValue = true;
                //kitchen_displayDataload();
            }
            catch
            {
            }
        }

        private void dtgridKitchenWaitingList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row2 in dtgridKitchenWaitingList.SelectedRows)
                {
                  //  DialogResult result = MessageBox.Show("Are you sure this item ready to Serve?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                   // if (result == DialogResult.Yes)
                   // {
                        DataGridViewRow row = dtgridKitchenWaitingList.Rows[e.RowIndex];
                        string item_id = row.Cells[1].Value.ToString();

                        string sql = " update sales_item set " +
                                    " status = 1 " +
                                    " where item_id  = '" + item_id  + "' ";
                        DataAccess.ExecuteSQL(sql);
                        DataTable dt1 = DataAccess.GetDataTable(sql);
                        kitchen_displayDataload();
                    //    MessageBox.Show("Item has been Served", "Yes or No", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button2); ;
                   // }
                } 
            }
            catch
            {

            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            { 
                ItemList_with_images();
            }
            catch
            {
            }
        }

      

    }
}
