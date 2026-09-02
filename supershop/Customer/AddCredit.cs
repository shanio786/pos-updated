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
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            ComboCustID.DataSource = dt5;
            ComboCustID.DisplayMember = "Name";

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
                    DataAccess.ExecuteSQL("insert into tbl_custcredit (CustID, orderID, Date, Credit, Description) values (@custid, 'SyS', @date, @credit, @desc)",
                                          DataAccess.P("@custid", lblCustID.Text),
                                          DataAccess.P("@date", dtDate.Text),
                                          DataAccess.P("@credit", NumUDcredit.Value),
                                          DataAccess.P("@desc", txtDesCription.Text));
                    MessageBox.Show("Successfully Added Credit to " + lblCustID.Text);
                    txtDesCription.Text = string.Empty;
                }
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not save the credit."); }
        }

        private void ComboCustID_SelectedIndexChanged(object sender, EventArgs e)
        {
            CustomerID();
        }

        public void CustomerID()
        {
            try
            {
                DataTable dt1 = DataAccess.GetDataTable("Select ID from  tbl_customer  where Name  = @name", DataAccess.P("@name", ComboCustID.Text));
                if (dt1.Rows.Count > 0)
                    lblCustID.Text = dt1.Rows[0].ItemArray[0].ToString();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
    }
}
