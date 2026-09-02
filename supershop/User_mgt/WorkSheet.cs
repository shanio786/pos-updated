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
    public partial class WorkSheet : Form
    {
        public WorkSheet()
        {
            InitializeComponent();
        }

        #region Databind
        public void Databind(string dtstart, string dtend)
        {
            DataTable dt1 = DataAccess.GetDataTable(
                " SELECT * FROM vw_workrecords where [Date] BETWEEN @s AND @e order by [Date]",
                DataAccess.P("@s", dtstart), DataAccess.P("@e", dtend));
            dtgrdWorkingHoursList.DataSource = dt1;
        }

        private void WorkSheet_Load(object sender, EventArgs e)
        {
            try
            {
                Databind(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd"), DateTime.Now.ToString("yyyy-MM-dd"));              
            }
            catch (Exception exLog) { Logger.Error(exLog); } 
        }

        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                Databind(dtStartDate.Text, dtEndDate.Text); 
              
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                DataTable dt1 = DataAccess.GetDataTable(
                    " SELECT * FROM vw_workrecords where username like @q + '%' order by [Date]",
                    DataAccess.P("@q", txtSearch.Text));
                dtgrdWorkingHoursList.DataSource = dt1;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        #endregion

        #region Export to CSV
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            //Build the CSV file data as a Comma separated string.
            string csv = string.Empty;

            //Add the Header row for CSV file.
            foreach (DataGridViewColumn column in dtgrdWorkingHoursList.Columns)
            {
                csv += column.HeaderText + ',';
            }

            //Add new line.
            csv += "\r\n";

            //Adding the Rows
            foreach (DataGridViewRow row in dtgrdWorkingHoursList.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    //Add the Data rows.
                    csv += cell.Value.ToString().Replace(",", ";") + ',';
                }

                //Add new line.
                csv += "\r\n";
            }

            // Get file name.
            string name = saveFileDialog1.FileName;
            System.IO.File.WriteAllText(name, csv);
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                //Exporting to xls.     
                saveFileDialog1.FileName = "WorkedHours_" + UserInfo.usernamWK + "_" + DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".csv";
                saveFileDialog1.ShowDialog();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }
        #endregion
    }
}
