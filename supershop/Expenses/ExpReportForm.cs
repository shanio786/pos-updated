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

namespace supershop.Expenses
{
    public partial class ExpReportForm : Form
    {
        

        public ExpReportForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string[] aa = new string[100];

            DataAccess DataAccess = new DataAccess();
            SqlConnection cnn;
            cnn = DataAccess.OpenDBConn();



             try
            {

                ReportValue.StartDate = dtStartDate.Value.ToShortDateString(); // dtStartDate.Value.ToShortDateString();
                ReportValue.EndDate = dtEndDate.Value.ToShortDateString();
                int count = 0;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                ds.Tables.Add("DSExpense");
                dt.Columns.Add("Category", System.Type.GetType("System.String"));
                dt.Columns.Add("Note", System.Type.GetType("System.String"));
                dt.Columns.Add("Date", System.Type.GetType("System.DateTime"));
                dt.Columns.Add("Amount", System.Type.GetType("System.Double"));
                dt.Columns.Add("exp_amnt", System.Type.GetType("System.Double"));
                dt.Columns.Add("exp_tamnt", System.Type.GetType("System.Double"));

                dt.Columns.Add("exp_cate_Heading", System.Type.GetType("System.String"));
                dt.Columns.Add("exp_amnt_Heading", System.Type.GetType("System.String"));

                DataRow r;
                SqlCommand cmddpt = new SqlCommand("select DISTINCT(Category) from tbl_expense", cnn);
                cmddpt.ExecuteNonQuery();
                SqlDataReader datardr = cmddpt.ExecuteReader();
                while (datardr.Read())
                {
                    aa[count] = datardr[0].ToString();
                    count++;
                }
                datardr.Dispose();
                for (int i = 0; i < count; i++)
                {
                    int cc = 0;
                    double exp_amount = 0;
                    try
                    {
                        SqlCommand cmd_amnt = new SqlCommand("select SUM(Amount) from tbl_expense where (Date >= '" + ReportValue.StartDate + "')AND(Date <= '" + ReportValue.EndDate + "')AND(Category = '" + aa[i] + "')", cnn);
                        exp_amount = Convert.ToDouble(cmd_amnt.ExecuteScalar().ToString());
                    }
                    catch (Exception)
                    { }


                    SqlCommand cmdqp1 = new SqlCommand("select Note,Date,Amount from tbl_expense where (Date >= '" + ReportValue.StartDate + "')AND(Date <= '" + ReportValue.EndDate + "')AND(Category = '" + aa[i] + "')", cnn);
                    cmdqp1.ExecuteNonQuery();
                    SqlDataReader drreadr11 = cmdqp1.ExecuteReader();
                    while (drreadr11.Read())
                    {
                        r = dt.NewRow();
                        r["Note"] = drreadr11[0].ToString();
                        r["Date"] = drreadr11[1].ToString();
                        r["Amount"] = drreadr11[2].ToString();


                        if (cc == 0)
                        {
                            r["Category"] = aa[i];
                            r["exp_tamnt"] = exp_amount;
                            r["exp_cate_Heading"] = "Expense Type";
                            r["exp_amnt_Heading"] = "Expense amount";

                        }
                        dt.Rows.Add(r);
                        cc++;
                    }


                    drreadr11.Dispose();



                }

                Expenses.Expense_List exprpr = new Expenses.Expense_List();

                exprpr.SetDataSource(dt);

                ReportViwer rf = new ReportViwer();
                TextObject dtFrom = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["dtFrom"];
                dtFrom.Text = ReportValue.StartDate;

                TextObject dtTo = (TextObject)exprpr.ReportDefinition.Sections["Section1"].ReportObjects["dtTo"];
                dtTo.Text = ReportValue.EndDate;
                rf.Show();
                rf.crystalReportViewer1.ReportSource = exprpr;
                rf.crystalReportViewer1.Refresh();
            }
            catch
            {
            }
        }
    }
}
