using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
 

namespace supershop.Inventory
{
    public partial class InvoicePrint : Form
    {
        public InvoicePrint(string InvoiceNo)
        {
            InitializeComponent();
            lblInvoiceNo.Text = InvoiceNo;
        }

        #region Invoice Setup Printing

        DataGridViewPrinter MyDataGridViewPrinter;
        private bool SetupThePrinting()
        {
            DataTable dt1 = DataAccess.GetDataTable("select * from tbl_terminalLocation where Shopid = @shopid", DataAccess.P("@shopid", UserInfo.Shopid));

            DateTime dt = DateTime.Now;
            string printdate = dt.ToString("MMMM dd, yyyy    hh:mm:ss tt");
            string Companyname = dt1.Rows[0].ItemArray[1].ToString();
            string Location = dt1.Rows[0].ItemArray[3].ToString();
            string email = dt1.Rows[0].ItemArray[5].ToString();

            // // Biller Info
            DataTable dtSP = DataAccess.GetDataTable("select * from tbl_saleInfo where InvoiceNo = @id", DataAccess.P("@id", lblInvoiceNo.Text));
            string Bnam = "Bill To \n" + dtSP.Rows[0].ItemArray[3].ToString();

            string TitleText = Companyname + "\n" + Location + "." + "\n" + email + "\n" + printdate + "\n\n" + Bnam + "\n\n" + "Invoice No: " + lblInvoiceNo.Text + "\n\n";
           
            PrintDialog MyPrintDialog = new PrintDialog();
            MyPrintDialog.AllowCurrentPage = false;
            MyPrintDialog.AllowPrintToFile = false;
            MyPrintDialog.AllowSelection = false;
            MyPrintDialog.AllowSomePages = false;
            MyPrintDialog.PrintToFile = false;
            MyPrintDialog.ShowHelp = false;
            MyPrintDialog.ShowNetwork = false;


            if (MyPrintDialog.ShowDialog() != DialogResult.OK)
                return false;

            printDocument1.DocumentName = "Invoice";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            MyDataGridViewPrinter = new DataGridViewPrinter(datagrdSalesInvoice,
                 printDocument1, false, true, TitleText, new Font("Times New Roman", 13, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);
            return true;
        }

        #endregion Invoice Setup Printing
        
        //Cross Button
        private void lnkClose_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }

        #region Invoice DataBind
        public void InvoiceDataBind()
        {
            if (lblInvoiceNo.Text == "")
            {
                MessageBox.Show("Please add atleast one item");
            }
            else
            {
                try
                {
                    string InvoiceNo = lblInvoiceNo.Text;
                    DataTable dt1 = DataAccess.GetDataTable("select itemName as Items ,  RetailsPrice as Price , Qty  , Total   from sales_item where sales_id = @id",
                                                            DataAccess.P("@id", InvoiceNo));
                    datagrdSalesInvoice.DataSource = dt1;

                    //Total calculation
                    decimal totalAmount = DataAccess.GetDecimal("select SUM(Total) from sales_item where sales_id = @id", DataAccess.P("@id", InvoiceNo));

                    DataTable dt6 = DataAccess.GetDataTable("select * from sales_payment where sales_id = @id", DataAccess.P("@id", InvoiceNo));

                    //Invoice  Shippingfee
                    DataTable dtSaleinfo = DataAccess.GetDataTable("select ShippingFee from tbl_saleInfo where InvoiceNo = @id", DataAccess.P("@id", InvoiceNo));

                    // Header info
                    DataTable dtTitle = DataAccess.GetDataTable("select * from tbl_terminalLocation where Shopid = @shopid", DataAccess.P("@shopid", UserInfo.Shopid));
                    string Ph           = dtTitle.Rows[0].ItemArray[4].ToString();
                    string web          = dtTitle.Rows[0].ItemArray[6].ToString();

                    decimal discountAmount = 0m;
                    decimal.TryParse(dt6.Rows[0].ItemArray[5].ToString(), out discountAmount);

                    DataRow dr = dt1.NewRow();
                    dr[0] = "";
                    dt1.Rows.Add(dr);

                    DataRow Total = dt1.NewRow();
                    Total[0] = "Total Amount: ";
                    Total[3] = totalAmount;
                    dt1.Rows.Add(Total);

                    DataRow dis = dt1.NewRow();
                    dis[0] = "Discount Amount: ";
                    dis[3] = discountAmount;
                    dt1.Rows.Add(dis);

                    DataRow dotlineSubtotal = dt1.NewRow();
                    dotlineSubtotal[0] = "___________________________________________________________________";
                    dt1.Rows.Add(dotlineSubtotal);

                    /// Sub total = total - Discount
                    DataRow Subtotal = dt1.NewRow();
                    Subtotal[0] = "Sub total : ";
                    Subtotal[3] = totalAmount - discountAmount;
                    dt1.Rows.Add(Subtotal);

                    DataRow dr0 = dt1.NewRow();
                    dr0[0] = "Invoice Tax :";
                    dr0[3] = dt6.Rows[0].ItemArray[6].ToString();
                    dt1.Rows.Add(dr0);

                    DataRow dotline = dt1.NewRow();
                    dotline[0] = "___________________________________________________________________";
                    dt1.Rows.Add(dotline);

                    //Shipping Fee
                    DataRow dr20 = dt1.NewRow();
                    dr20[0] = "Shipping Fee :";
                    dr20[3] = dtSaleinfo.Rows[0].ItemArray[0].ToString();
                    dt1.Rows.Add(dr20);

                    DataRow dotline2 = dt1.NewRow();
                    dotline2[0] = "___________________________________________________________________";
                    dt1.Rows.Add(dotline2);

                    // Net Amount = Sub total + VAT
                    DataRow dr2 = dt1.NewRow();
                    dr2[0] = "Net Amount :  ";
                    dr2[3] = dt6.Rows[0].ItemArray[2].ToString();
                    dt1.Rows.Add(dr2);
                     

                    DataRow dr6 = dt1.NewRow();
                    dr6[0] = "\n\n";
                    dt1.Rows.Add(dr6);

                    DataRow dr7 = dt1.NewRow();
                    dr7[0] = "|||| ||| |||||||| This is computer generated invoice printed copy. | Contact: " + Ph ;
                    dt1.Rows.Add(dr7);

                    DataRow dr9 = dt1.NewRow();
                    dr9[0] = "|||| ||| |||||||| Web: " + web;
                    dt1.Rows.Add(dr9);
                }
                catch (Exception exLog) { Logger.Error(exLog); }
            }
        }
        #endregion

        private void InvoicePrint_Load(object sender, EventArgs e)
        {
            InvoiceDataBind();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (SetupThePrinting())
                {
                    PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
                    MyPrintPreviewDialog.Document = printDocument1;
                    MyPrintPreviewDialog.ShowDialog();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("Sorry\r\n You have to Check the Data " + exp.Message);
            }        
          
        }

    }
}
