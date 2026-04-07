using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;

namespace supershop.Customer
{
    public partial class AddNewCustomer : Form
    {
        public AddNewCustomer()
        {
            InitializeComponent();
        }

        public string CustID
        {
            set
            {
                lblCustID.Text = value;
                lnkCustomers.Visible = false;
                grboxCusthistory.Visible = true;
                lblcuthistorylabel.Visible = true;
                lbltoplabel.Visible = true;

            }
            get
            {
                return lblCustID.Text;

            }
        }

        public string CustName
        {
            set
            {
                txtCustomerName.Text = value;
                btnSave.Text = "Update";
            }
            get
            {
                return txtCustomerName.Text;
            }
        }

        public string CustPhone
        {
            set
            {
                txtPhone.Text = value;
                
            }
            get
            {
                return txtPhone.Text;
                
            }
        }

        public string City
        {
            set
            {
                txtCity.Text = value;
            }
            get
            {
                return txtCity.Text;
            }
        }


        public string Email
        {
            set
            {
                txtEmailAddress.Text = value;
            }
            get
            {
                return txtEmailAddress.Text;
            }
        }

        public string CustAddress
        {
            set
            {
                txtCustomerAddress.Text = value;                
            }
            get
            {
                return txtCustomerAddress.Text;
            }
        }

        public string PeopleType
        {
            set
            {
                
                CombPeopleType.Text = value;
            }
            get
            {
                return CombPeopleType.Text;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void clearform()
        {
            CombPeopleType.Text = string.Empty;
            txtCity.Text = string.Empty;
            txtCustomerName.Text = string.Empty;
            txtCustomerAddress.Text = string.Empty;
            txtPhone.Text = string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblCustID.Text == "-")
                {
                   // if (txtPeopleID.Text == "") { MessageBox.Show("Please Fill ID"); txtPeopleID.Focus(); } else
                    if (txtCustomerName.Text == "") { MessageBox.Show("Please Fill Name"); txtCustomerName.Focus(); }
                    else if (txtPhone.Text == "") { MessageBox.Show("Please Fill Phone"); txtPhone.Focus(); }
                    else if (CombPeopleType.Text == "") { MessageBox.Show("Please Fill People Type"); CombPeopleType.Focus(); }
                    else if (txtCity.Text == "") { MessageBox.Show("Please Fill City"); txtCity.Focus(); }
                    else if (ShopNametextBox.Text == "") { MessageBox.Show("Please Fill  Address"); ShopNametextBox.Focus(); }
                    else if (txtCustomerAddress.Text == "") { MessageBox.Show("Please Fill  Address"); txtCustomerAddress.Focus(); }
                    else
                    {                        
                            string sqlCmd = "insert into tbl_customer (Name, EmailAddress, Phone, address, City, PeopleType )  values ('" + txtCustomerName.Text + "', '" + txtEmailAddress.Text + "', '" + txtPhone.Text + "', '" + txtCustomerAddress.Text + "', '" + txtCity.Text + "', '" + CombPeopleType.Text + "')";
                            DataAccess.ExecuteSQL(sqlCmd);
                            MessageBox.Show("Successfully saved");
                            lblMsg.Text = "Successfully saved";
                            clearform();

                        try
                        {

                            //string sql = "update storeconfig set companyname= '" + ShopNametextBox.Text + "' " + "  where id = '" + 1 + "'";
                            //DataAccess.ExecuteSQL(sql);
                            //lblMsg.Text = "Configuation has been Saved";
                            //lblMsg.Visible = true;

                        }
                        catch (Exception exp)
                        {
                            MessageBox.Show("Sorry\r\n You have to Check the Data" + exp.Message);
                        }
                    }
                }
                else  //Update 
                {
                    string sqlUpdateCmd = "update tbl_customer set Name = '" + txtCustomerName.Text + "', EmailAddress= '" + txtEmailAddress.Text + "', address = '" + txtCustomerAddress.Text + "', Phone = '" + txtPhone.Text + "', City = '" + txtCity.Text + "' , PeopleType = '" + CombPeopleType.Text + "'   where ID = '" + lblCustID.Text + "'";
                    DataAccess.ExecuteSQL(sqlUpdateCmd);
                    MessageBox.Show("Successfully Updated");
                }
              
              
            }
            catch (Exception exp)
            {
                MessageBox.Show("Sorry\r\n this id already added \n\n " + exp.Message);
            }
                  
        }

        private void AddNewCustomer_FormClosed(object sender, FormClosedEventArgs e)
        {
            //Customer.CustomerDetails go = new Customer.CustomerDetails();
            //go.MdiParent = this.ParentForm;
            //go.Show();
        }

        private void AddNewCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                        string sql = "  select  sales_id as 'Invo_No' , sales_time as Date , payment_amount as Total , " +
                                    "   (payment_amount - due_amount) as 'Paid Amount' ,  payment_type as 'Payment Type' , " +
                                    "   due_amount as Due, emp_id as 'Sold by' ,    c_id  as Contact , Comment as 'Cust Name/Comment' " +
                                    "   from sales_payment   where c_id = '" + lblCustID.Text + "' order by  sales_id desc";
                        DataAccess.ExecuteSQL(sql);
                        DataTable dt1 = DataAccess.GetDataTable(sql);
                        dtgviewCusttrxHistory.DataSource = dt1;
            }
            catch
            {
            }

        }

        private void lnkCustomers_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Customer.CustomerDetails go = new Customer.CustomerDetails();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void dtgviewCusttrxHistory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow row = dtgviewCusttrxHistory.Rows[e.RowIndex];               
               
                //this.Hide();
                Customer.Due_payment_History go = new Customer.Due_payment_History(row.Cells["Contact"].Value.ToString(), row.Cells["Invo_No"].Value.ToString());
                go.MdiParent = this.ParentForm;
                go.ShowDialog();

            }
            catch
            {

            }
        }

        #region print section
        DataGridViewPrinter MyDataGridViewPrinter;

        private bool SetupThePrinting()
        {
            string sql3 = "select * from tbl_terminallocation where Shopid = '" + UserInfo.Shopid + "'";
            DataAccess.ExecuteSQL(sql3);
            DataTable dt1 = DataAccess.GetDataTable(sql3);
            DateTime dt = DateTime.Now;
            string printdate = dt.ToString("MMMM dd, yyyy    hh:mm:ss tt");
            string Companyname = dt1.Rows[0].ItemArray[1].ToString();
            string branchname = dt1.Rows[0].ItemArray[2].ToString();
            string Location = dt1.Rows[0].ItemArray[3].ToString();
            string phone = dt1.Rows[0].ItemArray[4].ToString();
            string email = dt1.Rows[0].ItemArray[5].ToString();
            string web = dt1.Rows[0].ItemArray[6].ToString();

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

            printDocument1.DocumentName = "CustomerReport_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".csv";
            printDocument1.PrinterSettings = MyPrintDialog.PrinterSettings;
            printDocument1.DefaultPageSettings = MyPrintDialog.PrinterSettings.DefaultPageSettings;
            printDocument1.DefaultPageSettings.Margins = new Margins(10, 10, 20, 20);

            //  if (MessageBox.Show("Do you want the report to be centered on the page",   "InvoiceManager - Center on Page", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            MyDataGridViewPrinter = new DataGridViewPrinter(dtgviewCusttrxHistory,
            printDocument1, true, true, Header + " Customer Report " + lblCustID.Text + "\n", new Font("Baskerville Old Face", 13, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);


            //else

            //    MyDataGridViewPrinter = new DataGridViewPrinter(dtgrdViewSalesReport,
            //    printDocument1, false, true, Header + "   Sales Report   \n", new Font("Times New Roman", 14, FontStyle.Regular, GraphicsUnit.Point), Color.Black, true);

            return true;
        }
       
        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.dtgviewCusttrxHistory.RowsDefaultCellStyle.BackColor = Color.White;
                this.dtgviewCusttrxHistory.AlternatingRowsDefaultCellStyle.BackColor = Color.White;

                if (SetupThePrinting())
                {
                    PrintPreviewDialog MyPrintPreviewDialog = new PrintPreviewDialog();
                    // MyPrintPreviewDialog.ClientSize = new System.Drawing.Size(990, 630);
                    MyPrintPreviewDialog.WindowState = FormWindowState.Maximized;
                    MyPrintPreviewDialog.PrintPreviewControl.Zoom = 1.0;
                    // MyPrintPreviewDialog.UseAntiAlias = true;
                    MyPrintPreviewDialog.Document = printDocument1;
                    MyPrintPreviewDialog.ShowDialog();
                }
            }
            catch (Exception exp)
            {
                MessageBox.Show("!!! Please Print Preview or Setup Print only for First time " + exp.Message);
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //Bitmap bm = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);

            //this.dataGridView1.DrawToBitmap(bm, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));

            //e.Graphics.DrawImage(bm, 0, 0);

            bool more = MyDataGridViewPrinter.DrawDataGridView(e.Graphics);
            if (more == true)
                e.HasMorePages = true;
        }
      
        #endregion
    }
}
