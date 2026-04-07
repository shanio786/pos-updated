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
            string sql = "select receivedate as [Receive Date], totalamt as [Total Amount] ,dueamt as [Due Amount] ,receiveamt as [Receive Amount] from tbl_duepayment where sales_id = '" + lbsalesid.Text+"' and custid = '"+lbcontact.Text+"'";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            datagridDueList.DataSource = dt1;
            //datagridDueList.Columns[5].DefaultCellStyle.ForeColor = Color.DarkViolet;
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
