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
    public partial class CustomerDetails : Form
    {
        public CustomerDetails()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }


        #region Databind
        private void CustomerDetails_Load(object sender, EventArgs e)
        {
            try
            {
                dtGrdvCustomerDetails.EnableHeadersVisualStyles = false;
                dtGrdvCustomerDetails.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dtGrdvCustomerDetails.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
                dtGrdvCustomerDetails.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                Databind();

                //Edit Button
                DataGridViewButtonColumn btn = new DataGridViewButtonColumn();
                dtGrdvCustomerDetails.Columns.Add(btn);
                btn.HeaderText = "Edit";
                btn.Text = "Edit";
                btn.Name = "btn";
                btn.UseColumnTextForButtonValue = true;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
          
        }

        public void Databind()
        {
            if (parameter.peopleid == "SUP")
            {
                string sqlCmd = "Select * from  customercredit   where PeopleType  = 'Supplier' and id != '10000009' "; //From view combination of tbl_customer and custcredit
                DataTable dt1 = DataAccess.GetDataTable(sqlCmd);
                dtGrdvCustomerDetails.DataSource = dt1;
            }
            else
            {
                string sqlCmd = "Select * from  customercredit where id != '10000009'"; //From view combination of tbl_customer and custcredit
                DataTable dt1 = DataAccess.GetDataTable(sqlCmd);
                dtGrdvCustomerDetails.DataSource = dt1;
            }

        }
        #endregion

        private void dtGrdvCustomerDetails_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dtGrdvCustomerDetails.Rows[e.RowIndex];

                Customer.AddNewCustomer mkc = new Customer.AddNewCustomer();
                mkc.CustID      = row.Cells[1].Value.ToString();
                mkc.CustName    = row.Cells[2].Value.ToString();
                mkc.CustPhone   = row.Cells[3].Value.ToString();
                mkc.City        = row.Cells[6].Value.ToString();
                mkc.Email       = row.Cells[5].Value.ToString();
                mkc.CustAddress = row.Cells[4].Value.ToString();
                mkc.PeopleType  = row.Cells[7].Value.ToString();                 
                mkc.ShowDialog();

            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        #region Data serach
        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sqlCmd = " Select * from  customercredit " +
                                " where Name like '%' + @q + '%' or " +
                                " cast(ID as varchar(20)) like '%' + @q + '%' or " +
                                " Mobile like '%' + @q + '%' or " +
                                " City like '%' + @q + '%' or " +
                                " EmailAddress like '%' + @q + '%'";
                DataTable dt1 = DataAccess.GetDataTable(sqlCmd, DataAccess.P("@q", txtCustomerSearch.Text));
                dtGrdvCustomerDetails.DataSource = dt1;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void CombPeopleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CombPeopleType.Text == "All")
                {
                    DataTable dt1 = DataAccess.GetDataTable("Select * from  customercredit");
                    dtGrdvCustomerDetails.DataSource = dt1;
                }
                else
                {
                    DataTable dt1 = DataAccess.GetDataTable("Select * from  customercredit  where PeopleType = @type", DataAccess.P("@type", CombPeopleType.Text));
                    dtGrdvCustomerDetails.DataSource = dt1;
                }
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        #endregion


        #region Page link
        private void btnAddNewCustLink_Click(object sender, EventArgs e)
        {
            Customer.AddNewCustomer go = new Customer.AddNewCustomer();
            go.MdiParent = this.ParentForm;
            go.Show();
            this.Close();
        }

        private void btnStoreCreditRewards_Click(object sender, EventArgs e)
        {
            Customer.RewardsManagerReport go = new Customer.RewardsManagerReport();
            go.MdiParent = this.ParentForm;
            go.Show();
        }
        #endregion

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Delete all records?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            { 
                try
                {
                    // 10000009 is the built-in "Guest" customer and must stay
                    DataAccess.ExecuteSQL("delete from tbl_customer where id != 10000009");

                    MessageBox.Show("Has been Deleted", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Databind();
                }
                catch (Exception exLog) { Logger.Show(exLog, "Could not delete the customers."); }
            }
        }
    }
}
