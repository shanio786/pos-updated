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
    public partial class SalesDetails : Form
    {
        public SalesDetails(string ReceiptNo, double Discount, double subtotal, double vat, double PayAmt)
        {
            InitializeComponent();
            lblReceiptNo.Text   = ReceiptNo;
            lblDisCount.Text    = Convert.ToString(Discount);
            lblVat.Text         = Convert.ToString(vat);
            lblSubTotal.Text    = subtotal.ToString();
            lblTotal.Text       = Convert.ToString(PayAmt);
        }

        //Escape Closing 
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Mouse Moving 
        private void MouseDown_Class_mouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        public void Databind()
        {
            string sqlCmd = "select  Sales_id as 'Receipt No' , sales_time as Date , itemName as 'Item Name' , " +
                            " RetailsPrice  as Price , Qty,  Total , Profit * Qty as 'Profit'     " + 
                            " from sales_item  Where sales_id = '" + lblReceiptNo.Text + "' ";               
            DataAccess.ExecuteSQL(sqlCmd);
            DataTable dt = DataAccess.GetDataTable(sqlCmd);
            datagrdSalesDetails.DataSource = dt;
        }

        private void SalesDetails_Load(object sender, EventArgs e)
        {
            Databind();
        }

        //Cross Button
        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void btnPrintReceipt_Click(object sender, EventArgs e)
        {
            parameter.autoprintid = "2";
            POSPrintRpt mkc = new POSPrintRpt(lblReceiptNo.Text);
            mkc.ShowDialog();

            //PrintPage mkc = new PrintPage();
            //mkc.saleno      = lblReceiptNo.Text;
            //mkc.vat         = "";
            //mkc.dis         = "";
            //mkc.paidamt     = lblTotal.Text;
            //mkc.subtotal    = lblSubTotal.Text;
            //mkc.ShowDialog();
        }

    }
}
