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

        // Mark every line of the order as served (status 1)
        private void btnCompleteOrder_Click(object sender, EventArgs e)
        {
            try
            {
                long orderNo;
                if (!long.TryParse(lblOrder.Text.Trim(), out orderNo))
                {
                    MessageBox.Show("Invalid order number: " + lblOrder.Text);
                    return;
                }

                DataAccess.ExecuteSQL("update sales_item set status = 1 where sales_id = @id", DataAccess.P("@id", orderNo));
                MessageBox.Show("Order completed \n Wait 10 s for Refresh Display ");
                this.Hide();
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not complete the order."); }
        }
    }
}
