using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.Report
{
    public partial class KD_dialog : Form
    {
        public KD_dialog(string orderNo)
        {
            InitializeComponent();
            lblOrder.Text = orderNo;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }
        
        private void btnPrint_Click(object sender, EventArgs e)
        {
            POSPrintRpt mkc = new POSPrintRpt(lblOrder.Text);
            mkc.ShowDialog();
        }

        private void btnCompleteOrder_Click(object sender, EventArgs e)
        {
            string sql = " update sales_item set " +
                           " status = 1 " +
                           " where sales_id  = '" + lblOrder.Text + "' ";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            MessageBox.Show("Order completed \n Wait 10 s for Refresh Display ");
            this.Hide();
            
        }
    }
}
