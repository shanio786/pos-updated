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
                System.DateTime from = dtStartDate.Value.Date;
                System.DateTime to = dtEndDate.Value.Date;
                DataTable dt = DataAccess.GetDataTable(
                    " SELECT Category AS [Category], Note AS [Note], [Date], Amount AS [Amount] " +
                    " FROM tbl_expense WHERE [Date] >= @from AND [Date] < DATEADD(day,1,@to) " +
                    " ORDER BY Category, [Date]",
                    DataAccess.P("@from", from), DataAccess.P("@to", to));
                decimal total = DataAccess.GetDecimal(
                    "SELECT SUM(ISNULL(Amount,0)) FROM tbl_expense WHERE [Date] >= @from AND [Date] < DATEADD(day,1,@to)",
                    DataAccess.P("@from", from), DataAccess.P("@to", to));
                Report.FastReport.ShowReport(
                    "Expense Report  (" + from.ToString("yyyy-MM-dd") + " to " + to.ToString("yyyy-MM-dd") + ")",
                    dt, "Total expenses: " + total.ToString("0.00"));
            }
            catch (System.Exception ex)
            {
                Logger.Show(ex, "Could not build the expense report.");
            }
        }
    }
}
