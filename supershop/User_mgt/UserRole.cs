using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace supershop
{
    public partial class UserRole : Form
    {
        public UserRole(string uid)
        {
            InitializeComponent();
            lbluid.Text = uid;
        }

        public void Databind()
        {

                string sql = "SELECT  * FROM  MNU_USERROLE where UID = '" + lbluid.Text + "'";
                DataAccess.ExecuteSQL(sql);
                DataTable dt1 = DataAccess.GetDataTable(sql);           
                if (dt1.Rows.Count > 0) // if Exist 
                {
                    string sql4 = "SELECT  * FROM  vw_pagerole where UID = '" + lbluid.Text + "' ";
                    DataAccess.ExecuteSQL(sql4);
                    DataTable dt4 = DataAccess.GetDataTable(sql4);
                    dataGridView1.DataSource = dt4;
                    this.dataGridView1.Refresh();
                    this.dataGridView1.Parent.Refresh();

                }
                else
                {

                    string sql3 = "SELECT  * FROM  vw_newpagerole ";
                    DataAccess.ExecuteSQL(sql3);
                    DataTable dt3 = DataAccess.GetDataTable(sql3);
                    dataGridView1.DataSource = dt3;
                }       
           

        }

        private void UserRole_Load(object sender, EventArgs e)
        {
            Databind();

            //DataGridViewCheckBoxColumn chk = new DataGridViewCheckBoxColumn();
            //dataGridView1.Columns.Add(chk);
            //chk.HeaderText = "btn";
            //chk.Name = "btn";
            //chk.TrueValue = true;
            //chk.AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells;
            //chk.FlatStyle = FlatStyle.Standard;
            //chk.CellTemplate = new DataGridViewCheckBoxCell(false);
            //chk.CellTemplate.Style.BackColor = Color.White;

         //  lbluid.Text = dataGridView1.Rows[0].Cells[4].Value.ToString();

          
            //foreach (DataGridViewRow row in dataGridView1.Rows)
            //{
            //    if (Convert.ToBoolean(row.Cells[0].Value) == true)
            //    {
            //        lbluid.Text = "sd";
            //    }
            //}
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.ColumnIndex == dataGridView1.Columns[0].Index)
            //{
            //    dataGridView1.EndEdit();  //Stop editing of cell.

            //    if ((bool)dataGridView1.Rows[e.RowIndex].Cells[0].Value)
            //    {
            //         MessageBox.Show("The Value is Checked", "Status", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }
            //}  
        }

   

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            for (int k = 0; k <= dataGridView1.RowCount - 1; k++)
            {
                this.dataGridView1[0, k].Value = this.ChkAll.Checked;
            }
            this.dataGridView1.EndEdit();

  
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            int rows = dataGridView1.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                if (dataGridView1.Rows[i].Cells[5].Value.ToString() == "1")
                {
                    dataGridView1.Rows[i].Cells[0].Value = true;
                }
            }         
          }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.EndEdit();  //Stop editing of cell.
            int rows = dataGridView1.Rows.Count;
            for (int i = 0; i < rows; i++)
            {
                    string FRM_CODE = dataGridView1.Rows[i].Cells[3].Value.ToString();
                    if (dataGridView1.Rows[i].Cells[5].Value.ToString() == "0" && dataGridView1.Rows[i].Cells[4].Value.ToString() == "")
                    {
                       // if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                      //  {                       
                            string sql1 = "insert into MNU_USERROLE (UID,FRM_CODE,status)  values ('" + lbluid.Text + "', '" + FRM_CODE + "', '1' )";
                            DataAccess.ExecuteSQL(sql1);
                            lblmsg.Text = "Item Successfully saved";
                            // Databind();
                      //  }
          
                    }
                    else
                    {   

                        if (Convert.ToBoolean(dataGridView1.Rows[i].Cells[0].Value) == true)
                        {                     
                            string sql = "update MNU_USERROLE set status= '1'     where FRM_CODE = '" + FRM_CODE + "' and uid   = '" + lbluid.Text + "'";
                            DataAccess.ExecuteSQL(sql);
                            lblmsg.Text = "Updated";
                            this.dataGridView1.EndEdit();
                            this.dataGridView1.Refresh();
                            dataGridView1.ResetBindings();
                         
                        }
                        else
                        {                            
                            string sql = "update MNU_USERROLE set status= '0'     where FRM_CODE = '" + FRM_CODE + "' and uid   = '" + lbluid.Text + "'";
                            DataAccess.ExecuteSQL(sql);
                            lblmsg.Text = "Updated";
                           // Databind();
                        }
           
                    }          

            }
            this.Hide();
            UserRole mkc = new UserRole(lbluid.Text);
            mkc.ShowDialog();
            
        }
 
    }
}
