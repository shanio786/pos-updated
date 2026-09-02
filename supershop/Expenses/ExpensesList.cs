using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop.Expenses
{
    public partial class ExpensesList : Form
    {
        public ExpensesList()
        {
            InitializeComponent();
        }

        private void lnkAddExpense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            Expenses.AddExpense go = new Expenses.AddExpense();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        const string SelectExpenses =
            " select ID, Date, ReferenceNo as 'Refer No', Category, Amount, Note, Createdby as 'Posted by', Attachment, fileextension from tbl_expense ";

        public void Expensebind()
        {
            DataTable dt1 = DataAccess.GetDataTable(SelectExpenses);
            datagridExpenses.DataSource = dt1;
            ShowTotals();
        }

        // Row count and amount total for whatever is currently in the grid
        private void ShowTotals()
        {
            lblRow.Text = datagridExpenses.RowCount.ToString() + " Records Found";

            // Looked up by column name: the button columns shift the numeric indexes after the first bind
            double sum = 0;
            foreach (DataGridViewRow row in datagridExpenses.Rows)
            {
                object v = row.Cells["Amount"].Value;
                if (v != null && v != DBNull.Value)
                    sum += Convert.ToDouble(v);
            }
            lblSum.Text = "Total amount: " + sum.ToString();
        }

        private void ExpensesList_Load(object sender, EventArgs e)
        {
            try
            {
                Expensebind();

                DataGridViewButtonColumn View = new DataGridViewButtonColumn();
                datagridExpenses.Columns.Add(View);
                View.HeaderText = "Attachment";
                View.Text = "View";
                View.Name = "View";
                View.ToolTipText = "View this attachment";
                View.UseColumnTextForButtonValue = true;

                DataGridViewButtonColumn del = new DataGridViewButtonColumn();
                datagridExpenses.Columns.Add(del);
                del.HeaderText = "Delete";
                del.Text = "X";
                del.Name = "del";
                del.ToolTipText = "Delete this category";
                del.UseColumnTextForButtonValue = true;

                DataGridViewColumn ColID = datagridExpenses.Columns[0];
                ColID.Width = 31;
                DataGridViewColumn ColName = datagridExpenses.Columns[5];
                ColName.Width = 230;
                datagridExpenses.RowTemplate.MinimumHeight = 35;

                datagridExpenses.Columns[7].Visible = false;
                datagridExpenses.Columns[8].Visible = false;
                txtSearch.Focus();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void datagridExpenses_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // View support document From Gridview
                if (e.ColumnIndex == datagridExpenses.Columns["View"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow row in datagridExpenses.SelectedRows)
                    {
                        Expenses.ViewDoc mkc = new Expenses.ViewDoc(row.Cells[9].Value.ToString(), row.Cells[10].Value.ToString());
                        mkc.ShowDialog();
                    }
                }
                // Delete expense
                if (e.ColumnIndex == datagridExpenses.Columns["del"].Index && e.RowIndex >= 0)
                {
                    foreach (DataGridViewRow rowdel in datagridExpenses.SelectedRows)
                    {
                        DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

                        if (result == DialogResult.Yes)
                        {
                            DataAccess.ExecuteSQL("delete from tbl_expense where ID = @id", DataAccess.P("@id", rowdel.Cells[2].Value));

                            if (rowdel.Cells[9].Value.ToString() != string.Empty)
                            {
                                string path = Application.StartupPath + @"\ExpenseAttachment\";
                                System.IO.File.Delete(path + @"\" + rowdel.Cells[9].Value.ToString());
                            }
                            MessageBox.Show("Deleted");
                            Expensebind();
                        }
                    }
                }
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not delete expense"); }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sql = SelectExpenses +
                             " where ReferenceNo like @q + '%' or Note like '%' + @q + '%' or Createdby like @q + '%'";
                DataTable dt1 = DataAccess.GetDataTable(sql, DataAccess.P("@q", txtSearch.Text));
                datagridExpenses.DataSource = dt1;
                ShowTotals();
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Expenses.ExpReportForm().ShowDialog();
        }
    }
}
