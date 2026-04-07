using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.User_mgt
{
    public partial class User_attendence : Form
    {
        string uid = "";
        string att_status = "";
        DateTime dt = DateTime.Today;
        string thisMonth, thisYear = "";
        public User_attendence()
        {
            InitializeComponent();
        }

        private void User_attendence_Load(object sender, EventArgs e)
        {
            thisMonth = dt.ToString("MMMM");
            thisYear = dt.ToString("yyyy");

            string sql5 = "select Name from usermgt";
            DataAccess.ExecuteSQL(sql5);
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            cbUserName.DataSource = dt5;
            cbUserName.DisplayMember = "Name";

            cbSearchName.DataSource = dt5;
            cbSearchName.DisplayMember = "Name";

            cbEmpMnthly.DataSource = dt5;
            cbEmpMnthly.DisplayMember = "Name";


            cbUserName.SelectedIndex = -1;
            cbSearchName.SelectedIndex = -1;
            cbEmpMnthly.SelectedIndex = -1;
            cbAttStatus.SelectedIndex = 0;


            bindGrid();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string sql1 = "";
            if (cbUserName.Text != "")
            {
                if (cbAttStatus.Text == "Absent")
                {
                    sql1 = "insert into userattendence (Name,att_date,att_status,att_month,att_year)" +
                                      "  values('" + cbUserName.Text + "','" + dtDate.Value.ToShortDateString() + "','" + cbAttStatus.Text + "','" + thisMonth + "','" + thisYear + "'" +
                                      ")";
                    DataAccess.ExecuteSQL(sql1);
                    MessageBox.Show("Record Save Successfully", "Pay Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bindGrid();

                }
                else
                {
                    if (cbUserName.Text != "" && cbAttStatus.Text != "")
                    {
                        if (button1.Text == "IN")
                        {
                            DateTime dtIn = Convert.ToDateTime(dtInTime.Value.ToShortTimeString());

                            sql1 = "insert into userattendence (Name, intime,att_date,att_status,att_month,att_year)" +
                                         "  values('" + cbUserName.Text + "', '" + dtIn.ToShortTimeString() + "','" + dtDate.Value.ToShortDateString() + "','" + cbAttStatus.Text + "','" + thisMonth + "','" + thisYear + "'" +
                                         ")";
                            //MessageBox.Show("IN Time Enter Successfully", "IN Time", MessageBoxButtons.OK);
                        }
                        else if (button1.Text == "OUT")
                        {
                            DateTime dtOut = Convert.ToDateTime(dtOutTime.Value.ToShortTimeString());

                            sql1 = "Update userattendence Set outtime = '" + dtOut.ToShortTimeString() + "' where Name = '" + cbUserName.Text + "' and att_date = '" + dtDate.Value.ToShortDateString() + "'";
                           // MessageBox.Show("Out Time Updated Successfully", "OUT Time", MessageBoxButtons.OK);
                        }
                        else
                        { }
                        DataAccess.ExecuteSQL(sql1);
                        MessageBox.Show("Record Save Successfully", "Pay Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        bindGrid();
                        cbUserName.Text = "";
                        cbAttStatus.Text = "";
                    }
                    else
                    {
                        MessageBox.Show("Please Enter User Name", "User Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        

        private void bindGrid()
        {
            string sql = " SELECT att_date as Date , Name AS [User Name], intime as [In Time],outtime as [Out Time],att_status as Status FROM userattendence " +
                        " where att_date = '"+ dtSearch.Value.ToShortDateString()+"'";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            dataGridView1.DataSource = dt1;
        }
        private void bindGrid(string empName)
        {
            string sql = " SELECT att_date as Date , Name AS [User Name], intime as [In Time],outtime as [Out Time],att_status as Status FROM userattendence" +
                        " where att_date = '" + dtSearch.Value.ToShortDateString() + "' and Name = '"+ empName + "'";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            dataGridView1.DataSource = dt1;
        }
      

        private void cbUserName_SelectionChangeCommitted(object sender, EventArgs e)
        {
            
          
           
        }

        private void cbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                att_status = "";
                string sql = " SELECT intime FROM userattendence " +
                         " where att_date = '" + dtDate.Value.ToShortDateString() + "' and Name = '" + cbUserName.Text + "'";
                att_status = DataAccess.ExecuteSQLScaler(sql);
                sql = " SELECT att_status FROM userattendence " +
                        " where att_date = '" + dtDate.Value.ToShortDateString() + "' and Name = '" + cbUserName.Text + "'";
                string att_status1 = DataAccess.ExecuteSQLScaler(sql);
                if (att_status1 == "Absent")
                {
                    button1.Text = "SAVE";
                    button1.Enabled = false;
                }
                else
                {
                    if (att_status != "")
                        button1.Text = "OUT";
                    else if (att_status == "")
                        button1.Text = "IN";
                    else { button1.Text = "SAVE"; }
                    button1.Enabled = true;
                }
            }
            catch (Exception) { button1.Text = "IN"; }
        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            bindGrid();
            
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            bindGrid(cbSearchName.Text);
        }

        private void cbEmpMnthly_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sql = " SELECT att_date as Date , Name AS [User Name], intime as [In Time],outtime as [Out Time],att_status as Status FROM userattendence" +
                        " where att_date >= '" + dtFrom.Value.ToShortDateString() + "' and att_date <= '" + dtTo.Value.ToShortDateString() + "' and Name = '" + cbEmpMnthly.Text + "'";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            dataGridView1.DataSource = dt1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                DataAccess DataAccess = new DataAccess();
                SqlConnection cnn;
                cnn = DataAccess.OpenDBConn();
                string sql = "";
                
                sql = " SELECT Count(att_date) FROM userattendence" +
                         " where att_status = 'Present' and att_date >= '" + dtFrom.Value.ToShortDateString() + "' and att_date <= '" + dtTo.Value.ToShortDateString() + "' and Name = '" + cbEmpMnthly.Text + "'";
                string presntDays =  DataAccess.ExecuteSQLScaler(sql);

                sql = " SELECT Count(att_date) FROM userattendence" +
                        " where att_status = 'Absent' and att_date >= '" + dtFrom.Value.ToShortDateString() + "' and att_date <= '" + dtTo.Value.ToShortDateString() + "' and Name = '" + cbEmpMnthly.Text + "'";
                string absentDays = DataAccess.ExecuteSQLScaler(sql);

                sql = " SELECT att_date ,Name, intime ,outtime ,att_status FROM userattendence" +
                       " where att_date >= '" + dtFrom.Value.ToShortDateString() + "' and att_date <= '" + dtTo.Value.ToShortDateString() + "' and Name = '" + cbEmpMnthly.Text + "'";

                SqlCommand cmd = new SqlCommand(sql,cnn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds, "DSAttandence");
                User_mgt.attendenceRPT exprpr = new User_mgt.attendenceRPT();
                exprpr.SetDataSource(ds.Tables[0]);
                ReportViwer rp = new ReportViwer();

                TextObject dtF = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["dtFrom"];
                dtF.Text = dtFrom.Value.ToShortDateString();
                TextObject dtT = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["dtTo"];
                dtT.Text = dtTo.Value.ToShortDateString();

                TextObject prsnt = (TextObject)exprpr.ReportDefinition.Sections["Section4"].ReportObjects["Text16"];
                prsnt.Text = presntDays;
                TextObject absnt = (TextObject)exprpr.ReportDefinition.Sections["Section4"].ReportObjects["Text19"];
                absnt.Text = absentDays;

                rp.Show();
                rp.crystalReportViewer1.ReportSource = exprpr;
                rp.crystalReportViewer1.Refresh();
            }
            catch (Exception)
            {
                MessageBox.Show(this, "No Record Found.", "Query Error !!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbAttStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbAttStatus.Text == "Absent")
            {
                button1.Text = "SAVE";
               
            }
            else
            {
           
            }
        }
    }
}
