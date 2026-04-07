using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Finisar.SQLite;

namespace supershop
{
    public partial class DueList : Form
    {
        Double invoicetotal = 0;
        Double invoicereced = 0;
        Double invoicebal = 0;

        public DueList()
        {
            InitializeComponent();
        }

        private void DueList_Load(object sender, EventArgs e)
        {
            
                dateTimeDue.Format = DateTimePickerFormat.Custom;
            dateTimeDue.CustomFormat = "yyyy-MM-dd";

            datagridDueList.EnableHeadersVisualStyles = false;
            datagridDueList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            datagridDueList.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            datagridDueList.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;


            string sql = "select  sales_id as 'Invoice No' , sales_time as Date , payment_amount as Total , " + 
                         " (payment_amount - due_amount) as 'Paid Amount' ,  payment_type as 'Payment Type' , " + 
                         "  due_amount as Due, emp_id as 'Sold by' ,   C_id  as CustID , Comment as 'Cust Name/Comment' " + 
                         " from sales_payment where due_amount !='0'  ";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            datagridDueList.DataSource = dt1;
            datagridDueList.Columns[5].DefaultCellStyle.ForeColor = Color.DarkViolet;

            //this.datagridDueList.EnableHeadersVisualStyles = false;
            //this.datagridDueList.Columns[5].HeaderCell.Style.BackColor = Color.Red;


            DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
            datagridDueList.Columns.Add(btn);
            btn.HeaderText = "Receive";
            btn.Text = "+";
            btn.Name = "btn";
            btn.UseColumnTextForButtonValue = true;

            DataGridViewButtonColumn btnL = new DataGridViewButtonColumn();
            datagridDueList.Columns.Add(btnL);
            btnL.HeaderText = "Ledger";
            btnL.Text = "View Ledger";
            btnL.Name = "btn1";
            btnL.UseColumnTextForButtonValue = true;


        }
        private void invoice_total()
        {
            try
            {
                invoicetotal = 0;
                invoicereced = 0;
                invoicebal = 0;

                for (int i = 0; i < datagridDueList.Rows.Count; i++)
                {

                    double tp = Convert.ToDouble(datagridDueList[4, i].Value.ToString());
                    invoicetotal = invoicetotal + tp;

                    double tr = Convert.ToDouble(datagridDueList[5, i].Value.ToString());
                    invoicereced = invoicereced + tr;

                    double tb = Convert.ToDouble(datagridDueList[7, i].Value.ToString());
                    invoicebal = invoicebal + tb;
                }
                lblInvoiceTotal.Text = Convert.ToString(invoicetotal);
                lblinvoicereced.Text = Convert.ToString(invoicereced);
                lblinvoicebal.Text = Convert.ToString(invoicebal);

            }
            catch (Exception)
            {
                MessageBox.Show(this, "Some Feilds Are Missing.", "Processing Error !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void datagridDueList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = datagridDueList.Rows[e.RowIndex];

            if (datagridDueList.CurrentCell.ColumnIndex == 0)
            {
                try
                {

                    //DataGridViewRow row = datagridDueList.Rows[e.RowIndex];
                    DueUpdate mkc = new DueUpdate();

                    mkc.Salesid = row.Cells[2].Value.ToString();      //invoice no
                    mkc.salesdate = row.Cells[3].Value.ToString();    // sale date
                    mkc.totalamount = row.Cells[4].Value.ToString();  // total amount
                    mkc.paidamount = row.Cells[5].Value.ToString();   // paid amount
                    mkc.due = row.Cells[7].Value.ToString();          // due amount
                    mkc.contact = row.Cells[9].Value.ToString();      // cid
                    this.Hide();
                    mkc.MdiParent = this.ParentForm;
                    mkc.Show();

                }
                catch
                {

                }
            }
            else if (datagridDueList.CurrentCell.ColumnIndex == 1)
            {
                Customer.CustomerLedger cl = new Customer.CustomerLedger();
                cl.Salesid = row.Cells[2].Value.ToString();      //inv id
                cl.contact = row.Cells[9].Value.ToString();      //cust id
                cl.CustName = row.Cells[10].Value.ToString();      //cust name

                this.Hide();
                cl.MdiParent = this.ParentForm;
                cl.Show();
            }
         }
           

        private void txtsearch_TextChanged(object sender, EventArgs e)
        {
            string sql = "";
            string tString = txtsearch.Text;
            if (tString.Trim() == "")
            {
        
                sql = "select  sales_id as 'Invoice No' , sales_time as Date , payment_amount as Total ,  " +
       " (payment_amount - due_amount) as 'Paid Amount' , payment_type as 'Payment Type' , " +
       "  due_amount as Due, emp_id as 'Sold by' ,    C_id  as Contact , Comment as 'Cust Name/Comment' " +
       "  from sales_payment where  due_amount !='0'";
                DataAccess.ExecuteSQL(sql);
                DataTable dt2 = DataAccess.GetDataTable(sql);
                datagridDueList.DataSource = dt2;
                return;

            }
                if (!char.IsNumber(tString[0]))
                {
                    try
                    {
                        sql = "select  sales_id as 'Invoice No' , sales_time as Date , payment_amount as Total ,  " +
                                     " (payment_amount - due_amount) as 'Paid Amount' , payment_type as 'Payment Type' , " +
                                     "  due_amount as Due, emp_id as 'Sold by' ,    C_id  as Contact , Comment as 'Cust Name/Comment' " +
                                     "  from sales_payment where Comment like '" + txtsearch.Text + "%' and due_amount !='0'";               
                    }
                    catch
                    {
                    }
                }
                else
                {

                try
                {
                    sql = "select  sales_id as 'Invoice No' , sales_time as Date , payment_amount as Total ,  " +
                                " (payment_amount - due_amount) as 'Paid Amount' , payment_type as 'Payment Type' , " +
                                "  due_amount as Due, emp_id as 'Sold by' ,    C_id  as Contact , Comment as 'Cust Name/Comment' " +
                                "  from sales_payment where sales_id = '" + txtsearch.Text + "' and due_amount !='0'";

                }
                catch
                {
                }
            }

            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            datagridDueList.DataSource = dt1;
            invoice_total();
        }

        private void dateTimeDue_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                string sql = "select  sales_id as 'Invoice No' , sales_time as Date , payment_amount as Total , " + 
                            " (payment_amount - due_amount) as 'Paid Amount' ,  payment_type as 'Payment Type' ,  " + 
                            " due_amount as Due, emp_id as 'Sold by' ,    C_id  as Contact , Comment   " + 
                            " from sales_payment where sales_time = '" + dateTimeDue.Text + "' and due_amount !='0'  ";
                DataAccess.ExecuteSQL(sql);
                DataTable dt1 = DataAccess.GetDataTable(sql);
                datagridDueList.DataSource = dt1;
            }
            catch
            {
            }
        }

     
    }
}
