using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.User_mgt
{
    public partial class PayRoll : Form
    {
        DateTime dt = DateTime.Today;
        string thisMonth, thisYear = "";
        string netamount = "";
        string sql5 = "";
        string bounsValue = "";
        string dedcutValue = "";

        string preValTP = "";
        string preValNet = "";

        public PayRoll()
        {
            InitializeComponent();
        }


        private void PayRoll_Load(object sender, EventArgs e)
        {
            thisMonth = dt.ToString("MMMM");
            thisYear = dt.ToString("yyyy");
            txtYear.Text = thisYear;
            cbmonth.Text = thisMonth;
            string sql5 = "select Name from usermgt";
            DataAccess.ExecuteSQL(sql5);
            DataTable dt5 = DataAccess.GetDataTable(sql5);
            cbUserName.DataSource = dt5;
            cbUserName.DisplayMember = "Name";

             cbUserName.SelectedIndex = -1;
            cbPayType.SelectedIndex = -1;

            bindGrid();
            getclear();
        }

        private void getclear()
        {
            txtbais.Text = "0";
            txtAdvAmnt.Text = "";
            txtPaidAmnt.Text = "0";
            
            txtLeaves.Text = "0";
            txtNetSal.Text = "0";
            txtDedcut.Text = "0";
            txtBouns.Text = "0";
            txtbalamnt.Text = "0";
            txtTotalPay.Text = "0";
            txtpaystatus.Text = "...";
            dedcutValue = "0";
            bounsValue = "0";


        }
        private void bindGrid()
        {
            string sql = " SELECT user_name as Name , pay_month AS [Pay Month], pay_year as [Pay Year],pay_date as [Pay Date],leaves as [Leaves],basic_pay as [Basic Salary],bouns as [Bouns],total_salary as [Total Salary], deducations as [Deducations],net_amount as [Net Amount],paid_amount as [Paid Amount],bal_amount as [Balance],pay_status as [Status] FROM tbl_payroll" +
                        " where pay_month = '" + cbmonth.Text + "' and pay_year = '"+txtYear.Text+"'";
            DataAccess.ExecuteSQL(sql);
            DataTable dt1 = DataAccess.GetDataTable(sql);
            dataGridView1.DataSource = dt1;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPayType.Text != "Advence")
            {
                groupBox2.Visible = false;
            }
            else
            {
                groupBox2.Visible = true;
            }
        }

        private void cbUserName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                getclear();
                sql5 = "select basic_salary from usermgt where Name = '" + cbUserName.Text + "'";
                txtbais.Text = DataAccess.ExecuteSQLScaler(sql5);
                getdata();
                
            }
            catch (Exception) { }
           
        }

        private void cbmonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            getdata();

           
            bindGrid();

        }
        private void getdata()
        {
            txtDedcut.Text = "";
            bounsValue = "0";
            dedcutValue = "0";
            try
            {
                string sqlSal = " select basic_pay,paid_amount,deducations,net_amount,bouns,pay_status,bal_amount from tbl_payroll where (user_name = '" + cbUserName.Text + "')and(pay_month = '" + cbmonth.Text + "')and(pay_year = '" + txtYear.Text + "')"; // 
                DataAccess.ExecuteSQL(sqlSal);
                DataTable dtVat = DataAccess.GetDataTable(sqlSal);
                // txtbais.Text = dtVat.Rows[0].ItemArray[0].ToString();

                try { txtPaidAmnt.Text = dtVat.Rows[0].ItemArray[1].ToString(); } catch (Exception) { txtPaidAmnt.Text = "0";}
                try { dedcutValue = dtVat.Rows[0].ItemArray[2].ToString();} catch (Exception) { txtDedcut.Text = "0"; }
                try { txtNetSal.Text = dtVat.Rows[0].ItemArray[3].ToString();} catch (Exception) { txtNetSal.Text = "0"; }
                try { bounsValue = dtVat.Rows[0].ItemArray[4].ToString();} catch (Exception) {txtBouns.Text = "0"; }
                try { txtpaystatus.Text = dtVat.Rows[0].ItemArray[5].ToString();} catch (Exception) { txtpaystatus.Text = "...."; }
                try { txtbalamnt.Text = dtVat.Rows[0].ItemArray[6].ToString();} catch (Exception) { txtbalamnt.Text = "0"; }

                txtBouns.Text = bounsValue;
                txtDedcut.Text = dedcutValue;

                if (txtPaidAmnt.Text == "")
                    txtPaidAmnt.Text = "0";
                if (txtDedcut.Text == "")
                    txtDedcut.Text = "0";
                if (txtNetSal.Text == "")
                    txtNetSal.Text = "0";
                if (txtBouns.Text == "")
                    txtBouns.Text = "0";

                Double net = Convert.ToDouble(txtbais.Text) + Convert.ToDouble(txtBouns.Text);
                txtTotalPay.Text = Convert.ToString(net);
                Double net1 = Convert.ToDouble(txtTotalPay.Text) - Convert.ToDouble(txtDedcut.Text);
                txtNetSal.Text = Convert.ToString(net1);
                if(txtpaystatus.Text == "....")
                { txtbalamnt.Text = txtNetSal.Text; }

            }
            catch (Exception) { }
            try
            {
                sql5 = "select Count(att_date) from userattendence where (Name = '" + cbUserName.Text + "')and(att_month = '" + cbmonth.Text + "')and(att_year = '" + txtYear.Text + "')and(att_status = 'Absent')";
                txtLeaves.Text = DataAccess.ExecuteSQLScaler(sql5);
                if (txtLeaves.Text == "")
                    txtLeaves.Text = "0";
            }
            catch (Exception) { txtLeaves.Text = "0"; }
           
        }
        private void txtDedcut_TextChanged(object sender, EventArgs e)
        {

            try
            {
                if (cbPayType.Text == "Advence")
                {
                    Double net = Convert.ToDouble(txtTotalPay.Text)- Convert.ToDouble(txtDedcut.Text);
                   // net = net + Convert.ToDouble(txtBouns.Text);
                    txtNetSal.Text = Convert.ToString(net);
                }
                else
                {
                    Double net = Convert.ToDouble(txtTotalPay.Text) - Convert.ToDouble(txtDedcut.Text);
                   // net = net + Convert.ToDouble(txtBouns.Text);
                    txtNetSal.Text = Convert.ToString(net);
                }
            }
            catch (Exception) { }
           
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

       

        private void txtAdvAmnt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtDedcut.Text = "0";
                txtBouns.Text = "0";
            } catch (Exception) { }

            if (bounsValue == "")
                bounsValue = "0";
            if (dedcutValue == "")
                dedcutValue = "0";
            try
            {
                Double net = Convert.ToDouble(txtAdvAmnt.Text) + Convert.ToDouble(txtBouns.Text)+ Convert.ToDouble(txtPaidAmnt.Text);
                if (dedcutValue != "" || dedcutValue != "0")
                    net = net + Convert.ToDouble(dedcutValue);
                    txtTotalPay.Text = Convert.ToString(net);
            }
            catch (Exception) { }
            try
            {
                Double netsal = Convert.ToDouble(txtTotalPay.Text) - Convert.ToDouble(dedcutValue);
                txtNetSal.Text = Convert.ToString(netsal);
            }
            catch (Exception) { }
            try
            {
                Double bal = Convert.ToDouble(txtbais.Text) - (Convert.ToDouble(txtAdvAmnt.Text) + Convert.ToDouble(txtPaidAmnt.Text));
                Double balamnt = bal + Convert.ToDouble(bounsValue)- Convert.ToDouble(dedcutValue);
                txtbalamnt.Text = Convert.ToString(balamnt);
            }
            catch (Exception) { }
        }

        private void txtBalAmnt_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (cbPayType.Text == "Advence")
            {
                try
                {
                    Double net = Convert.ToDouble(txtAdvAmnt.Text) + Convert.ToDouble(txtBouns.Text)+ Convert.ToDouble(txtPaidAmnt.Text) ;
                    txtTotalPay.Text = Convert.ToString(net);
                    
                }
                catch (Exception)
                { }
            }
            else
            {
                try
                {
                    Double net = Convert.ToDouble(txtbais.Text) + Convert.ToDouble(txtBouns.Text);
                    txtTotalPay.Text = Convert.ToString(net);
                    
                }
                catch (Exception)
                { }
            }
           
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Double totalBouns = Convert.ToDouble(bounsValue) + Convert.ToDouble(txtBouns.Text);
                bounsValue = Convert.ToString(totalBouns);

            }
            catch (Exception)
            { }
            try
            {
                Double totalDeduct = Convert.ToDouble(dedcutValue) + Convert.ToDouble(txtDedcut.Text);
                dedcutValue = Convert.ToString(totalDeduct);

            }
            catch (Exception)
            { }
            string paystatus = "";
            string advpaid = "";
            if (Convert.ToDouble(txtbalamnt.Text) == 0)
                advpaid = "Advance Full Paid";
            else if (Convert.ToDouble(txtbalamnt.Text) >= 0)
                advpaid = "Advance Partial Paid";
            else
            {
                MessageBox.Show("Amount Greater", "Can Not Process", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            try
            {
                sql5 = "select user_name from tbl_payroll where (user_name = '" + cbUserName.Text + "')and(pay_month = '" + cbmonth.Text + "')and(pay_year = '" + txtYear.Text + "')";
                paystatus = DataAccess.ExecuteSQLScaler(sql5);
                if (paystatus == "")
                    paystatus = "false";
            }
            catch (Exception) { paystatus = "false"; }
            if (paystatus == "" || paystatus == "false")
            {
                try
                {
                    if (cbPayType.Text == "Advence")
                    {
                       
                        sql5 = "insert into tbl_adv_sal (user_name, adv_month,adv_year,adv_date,adv_amount,bal_amnt)" +
                                          "  values('" + cbUserName.Text + "', '" + cbmonth.Text + "','" + txtYear.Text + "','" + dtDate.Value.ToShortDateString() + "','" + txtAdvAmnt.Text + "','"+txtbalamnt.Text+"'" +
                                          ")";
                        DataAccess.ExecuteSQL(sql5);

                        sql5 = "insert into tbl_payroll (user_name,pay_month,pay_year,pay_date,basic_pay,leaves,deducations,net_amount,pay_status,paid_amount,bouns,total_salary,bal_amount)" +
                                        "  values('" + cbUserName.Text + "', '" + cbmonth.Text + "','" + txtYear.Text + "','" + dtDate.Value.ToShortDateString() + "','" + txtbais.Text + "','" + txtLeaves.Text + "','" + txtDedcut.Text + "','" + txtNetSal.Text + "','" + advpaid + "','" + txtNetSal.Text + "','" + txtBouns.Text + "','" + txtTotalPay.Text + "','" + txtbalamnt.Text + "'" +
                                        ")";
                        //Double net = Convert.ToDouble(txtbais.Text) + Convert.ToDouble(txtpa.Text);
                        //txtTotalPay.Text = Convert.ToString(net);
                    }
                    else if (cbPayType.Text == "Normal")
                    {
                        sql5 = "insert into tbl_payroll (user_name,pay_month,pay_year,pay_date,basic_pay,leaves,deducations,net_amount,pay_status,paid_amount,bouns,bal_amount,total_salary)" +
                                                               "  values('" + cbUserName.Text + "', '" + cbmonth.Text + "','" + txtYear.Text + "','" + dtDate.Value.ToShortDateString() + "','" + txtbais.Text + "','" + txtLeaves.Text + "','" + txtDedcut.Text + "','" + txtNetSal.Text + "','Full Paid','" + txtNetSal.Text + "','" + txtBouns.Text + "','0','" + txtTotalPay.Text + "'" +
                                                               ")";
                    }
                    DataAccess.ExecuteSQL(sql5);
                    MessageBox.Show("Record Save Successfully", "Pay Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception)
                {
                    MessageBox.Show("Record Not Save", "Not Submit", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else
            {
                if (cbPayType.Text == "Normal")
                {
                    MessageBox.Show("Record Already Saved Of This Month", "Not Submit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (cbPayType.Text == "Due Balance")
                {
                    sql5 = "insert into tbl_adv_sal (user_name, adv_month,adv_year,adv_date,adv_amount,bal_amnt)" +
                                      "  values('" + cbUserName.Text + "', '" + cbmonth.Text + "','" + txtYear.Text + "','" + dtDate.Value.ToShortDateString() + "','" + txtAdvAmnt.Text + "','"+txtbalamnt.Text+"'" +
                                      ")";
                    DataAccess.ExecuteSQL(sql5);
                    double totalpaid = Convert.ToDouble(txtPaidAmnt.Text) + Convert.ToDouble(txtNetSal.Text);
                    sql5 = "Update tbl_payroll SET pay_status = 'Full Paid',deducations = '" + dedcutValue + "',bouns = '" + bounsValue + "',total_salary = '" + txtTotalPay.Text + "',net_amount = '" + txtNetSal.Text + "',paid_amount = '" + totalpaid + "',bal_amount = '0'  where user_name = '" + cbUserName.Text + "' and pay_month = '" + cbmonth.Text + "' and pay_year = '" + txtYear.Text + "'";

                    sql5 = "Update tbl_payroll SET net_amount = '0',deducations = '" + txtDedcut.Text + "',paid_amount = '" + totalpaid + "'  where user_name = '" + cbUserName.Text + "' and pay_month = '" + cbmonth.Text + "' and pay_year = '" + txtYear.Text + "'";
                    DataAccess.ExecuteSQL(sql5);
                }
                else if (cbPayType.Text == "Advence")
                {
                    sql5 = "insert into tbl_adv_sal (user_name, adv_month,adv_year,adv_date,adv_amount,bal_amnt)" +
                                      "  values('" + cbUserName.Text + "', '" + cbmonth.Text + "','" + txtYear.Text + "','" + dtDate.Value.ToShortDateString() + "','" + txtAdvAmnt.Text + "','"+txtbalamnt.Text+"'" +
                                      ")";
                    DataAccess.ExecuteSQL(sql5);
                    double totalpaid = Convert.ToDouble(txtPaidAmnt.Text) + Convert.ToDouble(txtAdvAmnt.Text) + Convert.ToDouble(txtBouns.Text) - Convert.ToDouble(txtDedcut.Text);
                    sql5 = "Update tbl_payroll SET pay_status = '"+ advpaid + "',deducations = '" + dedcutValue + "',bouns = '" + bounsValue + "',total_salary = '" + txtTotalPay.Text+"',net_amount = '" + txtNetSal.Text + "',paid_amount = '" + totalpaid + "',bal_amount = '"+txtbalamnt.Text+"'  where user_name = '" + cbUserName.Text + "' and pay_month = '" + cbmonth.Text + "' and pay_year = '" + txtYear.Text + "'";
                    DataAccess.ExecuteSQL(sql5);
                    MessageBox.Show("Record Save Successfully", "Pay Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else { }
            }
            bindGrid();
            preValTP = txtTotalPay.Text;
            preValNet = txtNetSal.Text;

            getdata();
            DialogResult result = MessageBox.Show("Are You Want Report", "PayRoll Report", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                getReport();
            }
            else
            {
                // Do something  
            }
            getclear();
            
        }

        private void txtTotalPay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Double net = Convert.ToDouble(txtTotalPay.Text) - Convert.ToDouble(txtDedcut.Text);
                txtNetSal.Text = Convert.ToString(net);

            }
            catch (Exception)
            { }
        }

        private void txtNetSal_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel1.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                panel1.Visible = true;
                string sql = " SELECT adv_date as [Date],adv_amount as [Amount],bal_amnt as [Balance Amount] FROM tbl_adv_sal" +
                         " where user_name = '" + cbUserName.Text + "' and adv_month = '" + cbmonth.Text + "' and adv_year = '" + txtYear.Text + "'";
                DataAccess.ExecuteSQL(sql);
                DataTable dt1 = DataAccess.GetDataTable(sql);
                dataGridView2.DataSource = dt1;
            }
            catch(Exception)
            { MessageBox.Show("Some Feilds are Missing"); }
        }

        private void getReport()
        {
            try
            {
               

                User_mgt.PayRollReport exprpr = new User_mgt.PayRollReport();
               
                ReportViwer rp = new ReportViwer();

                TextObject empName = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["empName"];
                empName.Text = cbUserName.Text;

                TextObject payMonth = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["payMonth"];
                payMonth.Text = cbmonth.Text;

                TextObject payYear = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["payYear"];
                payYear.Text = txtYear.Text;

                TextObject payDate = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["payDate"];
                payDate.Text = dtDate.Value.ToShortDateString();

                TextObject payBasic = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["payBasic"];
                payBasic.Text = txtbais.Text;

                TextObject Bouns = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["Bouns"];
                Bouns.Text = txtBouns.Text;

                TextObject Leaves = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["Leaves"];
                Leaves.Text = txtLeaves.Text;

                TextObject deducations = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["deducations"];
                deducations.Text = txtDedcut.Text;

                TextObject PaidAmnt = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["PaidAmnt"];
                PaidAmnt.Text = txtPaidAmnt.Text;

                TextObject netPay = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["netPay"];
                netPay.Text = preValNet;

                TextObject balPay = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["txtbal"];
                balPay.Text = txtbalamnt.Text;

                TextObject tp = (TextObject)exprpr.ReportDefinition.Sections["Section2"].ReportObjects["txttp"];
                tp.Text = preValTP;
                TextObject ss = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["payStatus"];
                ss.Text = txtpaystatus.Text;

                rp.Show();
                rp.crystalReportViewer1.ReportSource = exprpr;
                rp.crystalReportViewer1.Refresh();


            }
            catch (Exception)
            {
                MessageBox.Show(this, "No Record Found.", "Query Error !!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            preValNet = "";
            preValTP = "";
        }
    }
}
