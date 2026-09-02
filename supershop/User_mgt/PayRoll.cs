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
    public partial class PayRoll : Form
    {
        DateTime dt = DateTime.Today;
        string thisMonth, thisYear = "";
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
                        " where pay_month = @m and pay_year = @y";
            DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text));
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
                txtbais.Text = DataAccess.ExecuteSQLScaler("select basic_salary from usermgt where Name = @n", DataAccess.P("@n", cbUserName.Text));
                if (txtbais.Text == "") txtbais.Text = "0";
                getdata();
                
            }
            catch (Exception exLog) { Logger.Error(exLog); }
           
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
                string sqlSal = " select basic_pay,paid_amount,deducations,net_amount,bouns,pay_status,bal_amount from tbl_payroll where user_name = @n and pay_month = @m and pay_year = @y";
                DataTable dtVat = DataAccess.GetDataTable(sqlSal, DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text));

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
            catch (Exception exLog) { Logger.Error(exLog); }
            try
            {
                txtLeaves.Text = DataAccess.GetDecimal(
                    "select Count(att_date) from userattendence where Name = @n and att_month = @m and att_year = @y and att_status = 'Absent'",
                    DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text)).ToString();
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
            catch (Exception exLog) { Logger.Error(exLog); }
           
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
            } catch (Exception exLog) { Logger.Error(exLog); }

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
            catch (Exception exLog) { Logger.Error(exLog); }
            try
            {
                Double netsal = Convert.ToDouble(txtTotalPay.Text) - Convert.ToDouble(dedcutValue);
                txtNetSal.Text = Convert.ToString(netsal);
            }
            catch (Exception exLog) { Logger.Error(exLog); }
            try
            {
                Double bal = Convert.ToDouble(txtbais.Text) - (Convert.ToDouble(txtAdvAmnt.Text) + Convert.ToDouble(txtPaidAmnt.Text));
                Double balamnt = bal + Convert.ToDouble(bounsValue)- Convert.ToDouble(dedcutValue);
                txtbalamnt.Text = Convert.ToString(balamnt);
            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
                catch (Exception exLog) { Logger.Error(exLog); }
            }
            else
            {
                try
                {
                    Double net = Convert.ToDouble(txtbais.Text) + Convert.ToDouble(txtBouns.Text);
                    txtTotalPay.Text = Convert.ToString(net);
                    
                }
                catch (Exception exLog) { Logger.Error(exLog); }
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
            catch (Exception exLog) { Logger.Error(exLog); }
            try
            {
                Double totalDeduct = Convert.ToDouble(dedcutValue) + Convert.ToDouble(txtDedcut.Text);
                dedcutValue = Convert.ToString(totalDeduct);

            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
                paystatus = DataAccess.ExecuteSQLScaler("select user_name from tbl_payroll where user_name = @n and pay_month = @m and pay_year = @y",
                    DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text));
                if (paystatus == "")
                    paystatus = "false";
            }
            catch (Exception) { paystatus = "false"; }
            // Everything belonging to one "pay" action (advance row + payroll row) is saved in one transaction.
            try
            {
                if (paystatus == "" || paystatus == "false")
                {
                    if (cbPayType.Text == "Advence")
                    {
                        DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                        {
                            tx.Execute(InsertAdvanceSql, AdvanceParams());
                            tx.Execute(InsertPayrollSql, PayrollInsertParams(advpaid, txtbalamnt.Text));
                        });
                    }
                    else if (cbPayType.Text == "Normal")
                    {
                        DataAccess.ExecuteSQL(InsertPayrollSql, PayrollInsertParams("Full Paid", "0"));
                    }
                    else
                    {
                        MessageBox.Show("Please select a pay type", "Not Submit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    MessageBox.Show("Record Save Successfully", "Pay Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        double totalpaid = Convert.ToDouble(txtPaidAmnt.Text) + Convert.ToDouble(txtNetSal.Text);
                        DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                        {
                            tx.Execute(InsertAdvanceSql, AdvanceParams());
                            tx.Execute("Update tbl_payroll SET net_amount = '0', deducations = @ded, paid_amount = @paid where user_name = @n and pay_month = @m and pay_year = @y",
                                DataAccess.P("@ded", txtDedcut.Text), DataAccess.P("@paid", totalpaid.ToString()),
                                DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text));
                        });
                    }
                    else if (cbPayType.Text == "Advence")
                    {
                        double totalpaid = Convert.ToDouble(txtPaidAmnt.Text) + Convert.ToDouble(txtAdvAmnt.Text) + Convert.ToDouble(txtBouns.Text) - Convert.ToDouble(txtDedcut.Text);
                        DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                        {
                            tx.Execute(InsertAdvanceSql, AdvanceParams());
                            tx.Execute("Update tbl_payroll SET pay_status = @status, deducations = @ded, bouns = @bonus, total_salary = @total, net_amount = @net, paid_amount = @paid, bal_amount = @bal " +
                                       " where user_name = @n and pay_month = @m and pay_year = @y",
                                DataAccess.P("@status", advpaid), DataAccess.P("@ded", dedcutValue), DataAccess.P("@bonus", bounsValue),
                                DataAccess.P("@total", txtTotalPay.Text), DataAccess.P("@net", txtNetSal.Text), DataAccess.P("@paid", totalpaid.ToString()),
                                DataAccess.P("@bal", txtbalamnt.Text),
                                DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text));
                        });
                        MessageBox.Show("Record Save Successfully", "Pay Submit", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception exSave)
            {
                Logger.Show(exSave, "Could not save the payroll record.");
                return;
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

        const string InsertAdvanceSql =
            "insert into tbl_adv_sal (user_name, adv_month, adv_year, adv_date, adv_amount, bal_amnt) values (@n, @m, @y, @d, @amount, @bal)";

        const string InsertPayrollSql =
            "insert into tbl_payroll (user_name, pay_month, pay_year, pay_date, basic_pay, leaves, deducations, net_amount, pay_status, paid_amount, bouns, total_salary, bal_amount) " +
            " values (@n, @m, @y, @d, @basic, @leaves, @ded, @net, @status, @paid, @bonus, @total, @bal)";

        // A SqlParameter can only belong to one command, so these build a fresh set every call.
        private SqlParameter[] AdvanceParams()
        {
            return new SqlParameter[] {
                DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text),
                DataAccess.P("@d", dtDate.Value.ToShortDateString()), DataAccess.P("@amount", txtAdvAmnt.Text), DataAccess.P("@bal", txtbalamnt.Text) };
        }

        private SqlParameter[] PayrollInsertParams(string status, string balance)
        {
            return new SqlParameter[] {
                DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text),
                DataAccess.P("@d", dtDate.Value.ToShortDateString()), DataAccess.P("@basic", txtbais.Text), DataAccess.P("@leaves", txtLeaves.Text),
                DataAccess.P("@ded", txtDedcut.Text), DataAccess.P("@net", txtNetSal.Text), DataAccess.P("@status", status),
                DataAccess.P("@paid", txtNetSal.Text), DataAccess.P("@bonus", txtBouns.Text), DataAccess.P("@total", txtTotalPay.Text),
                DataAccess.P("@bal", balance) };
        }

        private void txtTotalPay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Double net = Convert.ToDouble(txtTotalPay.Text) - Convert.ToDouble(txtDedcut.Text);
                txtNetSal.Text = Convert.ToString(net);

            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
                         " where user_name = @n and adv_month = @m and adv_year = @y";
                DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@n", cbUserName.Text), DataAccess.P("@m", cbmonth.Text), DataAccess.P("@y", txtYear.Text));
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
