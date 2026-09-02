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
            try
            {
                ReportValue.StartDate = dtStartDate.Value.ToString("yyyy-MM-dd");
                ReportValue.EndDate = dtEndDate.Value.ToString("yyyy-MM-dd");

                DataTable dt = new DataTable();
                dt.Columns.Add("Category", typeof(string));
                dt.Columns.Add("Note", typeof(string));
                dt.Columns.Add("Date", typeof(DateTime));
                dt.Columns.Add("Amount", typeof(double));
                dt.Columns.Add("exp_amnt", typeof(double));
                dt.Columns.Add("exp_tamnt", typeof(double));
                dt.Columns.Add("exp_cate_Heading", typeof(string));
                dt.Columns.Add("exp_amnt_Heading", typeof(string));

                // one query for everything: rows ordered by category, with the category total on every row
                DataTable rows = DataAccess.GetDataTable(
                    " SELECT e.Category, e.Note, e.[Date], e.Amount, " +
                    "        SUM(e.Amount) OVER (PARTITION BY e.Category) AS CategoryTotal " +
                    " FROM tbl_expense e " +
                    " WHERE e.[Date] >= @from AND e.[Date] < DATEADD(day, 1, @to) " +
                    " ORDER BY e.Category, e.[Date]",
                    DataAccess.P("@from", dtStartDate.Value.Date), DataAccess.P("@to", dtEndDate.Value.Date));

                string lastCategory = null;
                foreach (DataRow src in rows.Rows)
                {
                    DataRow r = dt.NewRow();
                    string category = Convert.ToString(src["Category"]);
                    r["Note"] = Convert.ToString(src["Note"]);
                    r["Date"] = src["Date"] == DBNull.Value ? (object)DBNull.Value : Convert.ToDateTime(src["Date"]);
                    r["Amount"] = src["Amount"] == DBNull.Value ? 0.0 : Convert.ToDouble(src["Amount"]);
                    if (category != lastCategory)
                    {
                        r["Category"] = category;
                        r["exp_tamnt"] = src["CategoryTotal"] == DBNull.Value ? 0.0 : Convert.ToDouble(src["CategoryTotal"]);
                        r["exp_cate_Heading"] = "Expense Type";
                        r["exp_amnt_Heading"] = "Expense amount";
                        lastCategory = category;
                    }
                    dt.Rows.Add(r);
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
            catch (Exception ex)
            {
                Logger.Show(ex, "Could not build the expense report.");
            }
        }
    }
}
