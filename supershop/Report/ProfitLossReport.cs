using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;

namespace supershop.Report
{
    public partial class ProfitLossReport : Form
    {
        public ProfitLossReport()
        {
            InitializeComponent();
        }
        
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        DataGridViewPrinter MyDataGridViewPrinter;
        private bool SetupThePrinting()
        {
            string sql3 = "select * from tbl_terminalLocation where Shopid = '"+ UserInfo.Shopid +"'";
            DataTable dt1 = DataAccess.GetDataTable(sql3);
            DateTime dt = DateTime.Now;
            string printdate    = dt.ToString("MMMM dd, yyyy    hh:mm:ss tt");
            string Companyname  = dt1.Rows[0].ItemArray[1].ToString();
            string branchname   = dt1.Rows[0].ItemArray[2].ToString();
            string Location     = dt1.Rows[0].ItemArray[3].ToString();  
            string phone        = dt1.Rows[0].ItemArray[4].ToString();            
            string email        = dt1.Rows[0].ItemArray[5].ToString();
            string web          = dt1.Rows[0].ItemArray[6].ToString();

            string Header = Companyname + "\n" + Location + "." + "\n" + email + "\n" + branchname + " ph: " + phone + "\n" + printdate + "\n";

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

            printDocument1.DocumentName = "Profit_Loss_Summary_Report";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(40, 40, 40, 40);

            //if (MessageBox.Show("Do you want the report to be centered on the page",
            //    "InvoiceManager - Center on Page", MessageBoxButtons.YesNo,
            //    MessageBoxIcon.Question) == DialogResult.Yes)
            MyDataGridViewPrinter = new DataGridViewPrinter(dtgrdViewProfitLoss, printDocument1, true, true, Header + "\n", 
                new Font("Baskerville Old Face", 13, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);
             

            //else
            //    MyDataGridViewPrinter = new DataGridViewPrinter(dtgrdViewProfitLoss,
            //    printDocument1, false, true, Header  + "   Sales Report   \n", new Font("Times New Roman", 14, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);

            return true;
        }

        private void ProfitLossReport_Load(object sender, EventArgs e)
        {

            try
            {

                // dtgrdViewProfitLoss.Refresh(); //.Columns.Clear();
                this.dtgrdViewProfitLoss.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtgrdViewProfitLoss.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

                dtgrdViewProfitLoss.ColumnCount = 3;


                string sql3 = " select SUM(Total) ,  SUM(( profit * Qty)) as Profit , SUM(RetailsPrice) as SubTotal,SUM(discount) as discount    from sales_item  " +
                    " where sales_time   >='" + ReportValue.StartDate + "' AND  sales_time <='" + ReportValue.EndDate + "' ";
                DataTable dt3 = DataAccess.GetDataTable(sql3);
                string Totalsales = dt3.Rows[0].ItemArray[0].ToString();
                string grossprofit = dt3.Rows[0].ItemArray[1].ToString();
                string subTotal = dt3.Rows[0].ItemArray[2].ToString();
                string discountFromReturnedItem = dt3.Rows[0].ItemArray[3].ToString();




                string[] row = new string[] { "  ", "Profit Loss Report", " " };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { "Date Between ", ReportValue.StartDate.ToString(), ReportValue.EndDate };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { "_______________________", "__________________", "___________________" };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { " ", " ", " " };
                dtgrdViewProfitLoss.Rows.Add(row);


                string sqlPayment = " select SUM(payment_amount), SUM(dis), SUM(vat) , SUM(due_amount)  from sales_payment " +
                                  " where sales_time   >='" + ReportValue.StartDate + "' AND  sales_time <='" + ReportValue.EndDate + "' ";
                DataTable dtPayment = DataAccess.GetDataTable(sqlPayment);

                string totalpaidbycustomer = dtPayment.Rows[0].ItemArray[0].ToString(); // total paid by customer with vat
                string dis = dtPayment.Rows[0].ItemArray[1].ToString();
                string vat = dtPayment.Rows[0].ItemArray[2].ToString();
                string due = dtPayment.Rows[0].ItemArray[3].ToString();
                double salesminusdis = Convert.ToDouble(Totalsales) - Convert.ToDouble(dis);
                if (salesminusdis < 0)
                    salesminusdis = 0;
                Double grossprofitNew = Convert.ToDouble(grossprofit) - Convert.ToDouble(dis);
                string totalcost = (Convert.ToDouble(Totalsales) - Convert.ToDouble(grossprofitNew)).ToString();

                //int totallossamount = Convert.ToInt32(due) - Convert.ToInt32(totalcost);


                double totalreturnedamt = 0;
                double totalreturn = 0;
                double totaldis = 0;
                try
                {
                    string sqlReturn = " select SUM(Total) , SUM(disamt), SUM(vatamt)  from return_item " +
                        " where return_time   >='" + ReportValue.StartDate + "' AND  return_time <='" + ReportValue.EndDate + "' ";
                    DataTable dtReturn = DataAccess.GetDataTable(sqlReturn);
                    totalreturn = Convert.ToDouble(dtReturn.Rows[0].ItemArray[0].ToString());
                    totaldis = Convert.ToDouble(dtReturn.Rows[0].ItemArray[1].ToString());
                    double totalvat = Convert.ToDouble(dtReturn.Rows[0].ItemArray[2].ToString());
                    totalreturnedamt = (totalreturn) + totalvat- Convert.ToDouble(totaldis);
                }
                catch (Exception)
                {
                    totalreturnedamt = 0;
                }
                double totalSale = 0;
                if (totalreturn > 0)
                {
                    totalSale = Convert.ToDouble(Totalsales) + Convert.ToDouble(totalreturn);
                    row = new string[] { "Sub Total ", totalSale.ToString(), " " };
                    double disNew = 0;
                    if (discountFromReturnedItem != "")
                    {
                        disNew = Convert.ToDouble(dis) + Convert.ToDouble(totaldis);
                    }
                    else
                    {
                        disNew = Convert.ToDouble(dis) + totaldis;
                    }
                    dis = disNew.ToString();
                }
                else
                {
                    totalSale = Convert.ToDouble(Totalsales);
                    row = new string[] { "Sub Total ", totalSale.ToString(), " " };

                }
                dtgrdViewProfitLoss.Rows.Add(row);

                row = new string[] { "Total Discount ", dis, " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                double subTotalSaleAfterDis = Convert.ToDouble(totalSale) - Convert.ToDouble(dis);

                row = new string[] { "Total Sales after discount ", subTotalSaleAfterDis.ToString(), " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                row = new string[] { "Total Return ", totalreturnedamt.ToString(), " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                double totalSaleAfterReturn = Convert.ToDouble(subTotalSaleAfterDis) - Convert.ToDouble(totalreturnedamt);
                row = new string[] { "Total Sale After Return ", totalSaleAfterReturn.ToString(), " " };
                dtgrdViewProfitLoss.Rows.Add(row);





                row = new string[] { "Total TAX ", vat, " " };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { "Total Due Amount ", due, " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                //row = new string[] { "Total Loss Amount ", totallossamount.ToString(), " " };
                //dtgrdViewProfitLoss.Rows.Add(row);

                row = new string[] { " ", " ", " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                //  row = new string[] { "Total buy Cost ", totalcost, " " };
                //  dtgrdViewProfitLoss.Rows.Add(row);
                double totalPaidByCustomerFinal = Convert.ToDouble(Totalsales) - Convert.ToDouble(totalreturnedamt);

                row = new string[] { "Total Paid By Customer ", totalSaleAfterReturn.ToString(), " " };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { " ", " ", " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                double Netprofit = grossprofitNew;//Convert.ToDouble(totalSaleAfterReturn) - Convert.ToDouble(totalcost);
                row = new string[] { "Net profit ", "After discount ", Netprofit.ToString() };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { " ", " ", " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                row = new string[] { "Total Cost", totalcost.ToString(), "" };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { " ", " ", " " };
                dtgrdViewProfitLoss.Rows.Add(row);

                //Return  Start

                //// Return END

                //Expenses Start                
                string sqlExpenses = " select SUM(Amount)   from tbl_expense " +
                                 " where Date   >='" + ReportValue.StartDate + "' AND  Date <='" + ReportValue.EndDate + "' ";
                DataTable dtExpenses = DataAccess.GetDataTable(sqlExpenses);

                double totalExpenses = Convert.ToDouble(dtExpenses.Rows[0].ItemArray[0].ToString());
                row = new string[] { "Total Expenses ", totalExpenses.ToString(), " " };
                dtgrdViewProfitLoss.Rows.Add(row);
                // Expenses END

                double incash = (Convert.ToDouble(totalpaidbycustomer) - Convert.ToDouble(due)) - Convert.ToDouble(dis) - totalreturnedamt - totalExpenses;
                row = new string[] { " ", " ", " " };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { "In cash ", incash.ToString(), " " };
                dtgrdViewProfitLoss.Rows.Add(row);
                row = new string[] { " ", " ", " " };
                dtgrdViewProfitLoss.Rows.Add(row);

            }
            catch (Exception exLog) { Logger.Error(exLog); }

        }

        private void dtgrdViewProfitLoss_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //Calculate sum of Operation Cost and Other cost
                double sumofOperationCostNOthercost = Convert.ToDouble(dtgrdViewProfitLoss.Rows[10].Cells[1].Value) + Convert.ToDouble(dtgrdViewProfitLoss.Rows[11].Cells[1].Value);
                double grossprofit = Convert.ToDouble(dtgrdViewProfitLoss.Rows[8].Cells[2].Value);
                double Netprofit = grossprofit  - sumofOperationCostNOthercost;
                dtgrdViewProfitLoss.Rows[12].Cells[2].Value = sumofOperationCostNOthercost;
                dtgrdViewProfitLoss.Rows[14].Cells[2].Value = Netprofit;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        
        //save as
        private void btnExport_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "ProfitLossReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
            saveFileDialog1.ShowDialog();
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dtgrdViewProfitLoss.Columns)
            {
                csv += column.HeaderText + ',';
            }

            //Add new line.
            csv += "\r\n";

            //Adding the Rows
            foreach (DataGridViewRow row in dtgrdViewProfitLoss.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //Add the Data rows.
                    csv += cell.Value.ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            //Exporting to CSV.            
            string fileName = "ProfitLossReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
            string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string destFile = System.IO.Path.Combine(targetPath, fileName);

            // To copy a folder's contents to a new location: 
            // Create a new target folder, if necessary. 
            if (!System.IO.Directory.Exists(targetPath))
            {
                System.IO.Directory.CreateDirectory(targetPath);

            }

            // Get file name.
            string name = saveFileDialog1.FileName;
            File.WriteAllText(name, csv);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtgrdViewProfitLoss.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtgrdViewProfitLoss.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

                if (SetupThePrinting())
                {
                    PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
                    MyPrintPreviewDialog.WindowState = FormWindowState.Maximized;
                    MyPrintPreviewDialog.PrintPreviewControl.Zoom = 1.0;
                    MyPrintPreviewDialog.Document = printDocument1;
                    MyPrintPreviewDialog.ShowDialog();  
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("!!! Please Print Preview or Setup Print only for First time " + exp.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }

       
    }
}
