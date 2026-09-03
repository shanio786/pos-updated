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
            if (cbUserName.Text == "")
            {
                MessageBox.Show("Please Enter User Name", "User Name", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                string attDate = dtDate.Value.ToShortDateString();
                if (cbAttStatus.Text == "Absent")
                {
                    DataAccess.ExecuteSQL("insert into userattendence (Name, att_date, att_status, att_month, att_year) values (@n, @d, @s, @m, @y)",
                        DataAccess.P("@n", cbUserName.Text), DataAccess.P("@d", attDate), DataAccess.P("@s", cbAttStatus.Text),
                        DataAccess.P("@m", thisMonth), DataAccess.P("@y", thisYear));
                    MessageBox.Show("Record Save Successfully", "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    bindGrid();
                    return;
                }
                if (cbAttStatus.Text == "")
                {
                    MessageBox.Show("Please select attendance status", "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (button1.Text == "IN")
                {
                    DateTime dtIn = Convert.ToDateTime(dtInTime.Value.ToShortTimeString());
                    DataAccess.ExecuteSQL("insert into userattendence (Name, intime, att_date, att_status, att_month, att_year) values (@n, @in, @d, @s, @m, @y)",
                        DataAccess.P("@n", cbUserName.Text), DataAccess.P("@in", dtIn.ToShortTimeString()), DataAccess.P("@d", attDate),
                        DataAccess.P("@s", cbAttStatus.Text), DataAccess.P("@m", thisMonth), DataAccess.P("@y", thisYear));
                }
                else if (button1.Text == "OUT")
                {
                    DateTime dtOut = Convert.ToDateTime(dtOutTime.Value.ToShortTimeString());
                    DataAccess.ExecuteSQL("Update userattendence Set outtime = @out where Name = @n and att_date = @d",
                        DataAccess.P("@out", dtOut.ToShortTimeString()), DataAccess.P("@n", cbUserName.Text), DataAccess.P("@d", attDate));
                }
                else
                {
                    return;
                }
                MessageBox.Show("Record Save Successfully", "Attendance", MessageBoxButtons.OK, MessageBoxIcon.Information);

                bindGrid();
                cbUserName.Text = "";
                cbAttStatus.Text = "";
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not save the attendance record."); }
        }

        private void bindGrid()
        {
            string sql = " SELECT att_date as Date , Name AS [User Name], intime as [In Time],outtime as [Out Time],att_status as Status FROM userattendence " +
                        " where att_date = @d";
            DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@d", dtSearch.Value.ToShortDateString()));
            dataGridView1.DataSource = dt1;
        }
        private void bindGrid(string empName)
        {
            string sql = " SELECT att_date as Date , Name AS [User Name], intime as [In Time],outtime as [Out Time],att_status as Status FROM userattendence" +
                        " where att_date = @d and Name = @n";
            DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@d", dtSearch.Value.ToShortDateString()), DataAccess.P("@n", empName));
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
                string attDate = dtDate.Value.ToShortDateString();
                att_status = DataAccess.ExecuteSQLScaler(" SELECT intime FROM userattendence where att_date = @d and Name = @n",
                    DataAccess.P("@d", attDate), DataAccess.P("@n", cbUserName.Text));
                string att_status1 = DataAccess.ExecuteSQLScaler(" SELECT att_status FROM userattendence where att_date = @d and Name = @n",
                    DataAccess.P("@d", attDate), DataAccess.P("@n", cbUserName.Text));
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
                        " where att_date >= @f and att_date <= @t and Name = @n";
            DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@f", dtFrom.Value.ToShortDateString()),
                DataAccess.P("@t", dtTo.Value.ToShortDateString()), DataAccess.P("@n", cbEmpMnthly.Text));
            dataGridView1.DataSource = dt1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {

                string sql = "";
                string dFrom = dtFrom.Value.ToShortDateString();
                string dTo = dtTo.Value.ToShortDateString();
                
                sql = " SELECT Count(att_date) FROM userattendence where att_status = 'Present' and att_date >= @f and att_date <= @t and Name = @n";
                string presntDays = DataAccess.ExecuteSQLScaler(sql, DataAccess.P("@f", dFrom), DataAccess.P("@t", dTo), DataAccess.P("@n", cbEmpMnthly.Text));

                sql = " SELECT Count(att_date) FROM userattendence where att_status = 'Absent' and att_date >= @f and att_date <= @t and Name = @n";
                string absentDays = DataAccess.ExecuteSQLScaler(sql, DataAccess.P("@f", dFrom), DataAccess.P("@t", dTo), DataAccess.P("@n", cbEmpMnthly.Text));

                sql = " SELECT att_date ,Name, intime ,outtime ,att_status FROM userattendence where att_date >= @f and att_date <= @t and Name = @n";
DataTable ds = DataAccess.GetDataTable(
                    " SELECT att_date AS [Date], Name AS [User Name], intime AS [In Time], outtime AS [Out Time], att_status AS [Status] " +
                    " FROM userattendence WHERE att_date >= @f AND att_date <= @t AND Name = @n ORDER BY att_date",
                    DataAccess.P("@f", dFrom), DataAccess.P("@t", dTo), DataAccess.P("@n", cbEmpMnthly.Text));
                Report.FastReport.ShowReport(
                    "Attendance  -  " + cbEmpMnthly.Text + "  (" + dFrom + " to " + dTo + ")",
                    ds, "Present: " + presntDays, "Absent: " + absentDays);
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
