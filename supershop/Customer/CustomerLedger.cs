using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.Customer
{
    public partial class CustomerLedger : Form
    {
        public CustomerLedger()
        {
            InitializeComponent();
        }
        public string contact
        {
            set { lbcontact.Text = value; }
            get { return lbcontact.Text; }
        }
        public string Salesid
        {
            set { lbsalesid.Text = value; }
            get { return lbsalesid.Text; }
        }
        public string CustName
        {
            set { lbcname.Text = value; }
            get { return lbcname.Text; }
        }
        private void CustomerLedger_Load(object sender, EventArgs e)
        {
            try
            {
                string sql = "select receivedate as [Receive Date], totalamt as [Total Amount] ,dueamt as [Due Amount] ,receiveamt as [Receive Amount] " +
                             " from tbl_duepayment where sales_id = @id and custid = @custid";
                DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@id", lbsalesid.Text), DataAccess.P("@custid", lbcontact.Text));
                datagridDueList.DataSource = dt1;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();

            DueList go = new DueList();
            go.MdiParent = this.ParentForm;
            go.Show();
        }
    }
}
