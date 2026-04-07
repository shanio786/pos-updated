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
    public partial class AddCredit : Form
    {
        public AddCredit()
        {
            InitializeComponent();
        }
 

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void AddCredit_FormClosed(object sender, FormClosedEventArgs e)
        {
            Customer.RewardsManagerReport go = new Customer.RewardsManagerReport();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void AddCredit_Load(object sender, EventArgs e)
        {
            dtDate.Format = DateTimePickerFormat.Custom;
            dtDate.CustomFormat = "yyyy-MM-dd";

            string sql5 = "select   DISTINCT  *   from tbl_customer where PeopleType = 'Customer'";
            DataAccess.ExecuteSQL(sql5);
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            ComboCustID.DataSource = dt5;
            ComboCustID.DisplayMember = "Name";
            
           // lblCustID.Text = dt5.Rows[0].ItemArray[0].ToString();
            CustomerID();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDesCription.Text == "")
                {
                    MessageBox.Show("Please Write Description");
                }
                else
                {
                    string sqlCmd = "insert into tbl_custcredit (CustID, orderID, Date, Credit, Description)  values ('" + lblCustID.Text + "', 'SyS', '" + dtDate.Text + "', '" + NumUDcredit.Text + "', '" + txtDesCription.Text + "')";
                    DataAccess.ExecuteSQL(sqlCmd);
                    MessageBox.Show("Successfully Added Credit to " + lblCustID.Text);
                    txtDesCription.Text = string.Empty;
                }

            }
            catch (Exception exp)
            {
                MessageBox.Show("Sorry\r\n this id already added \n\n " + exp.Message);
            }
        }

        private void ComboCustID_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                CustomerID();
            }
            catch // (Exception exp)
            {
               // MessageBox.Show(exp.Message);
            }
        }

        public void CustomerID()
        {
            string sqlCmd = "Select ID from  tbl_customer  where Name  = '" + ComboCustID.Text + "'";
            DataAccess.ExecuteSQL(sqlCmd);
            DataTable dt1 = DataAccess.GetDataTable(sqlCmd);

            lblCustID.Text = dt1.Rows[0].ItemArray[0].ToString();
        }
                 
                  
                 
    }
}
