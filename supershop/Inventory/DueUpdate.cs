using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace supershop
{
    public partial class DueUpdate : Form
    {
        public DueUpdate()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region Data get from Duelist
        public string due     
        {
            set{lbDueAmount.Text = value;} get{ return lbDueAmount.Text;}
        }
        public string Salesid
        {
            set { lbsalesid.Text = value; }       get { return lbsalesid.Text; }
        }
        public string salesdate
        {
            set { lbdate.Text = value; }           get { return lbdate.Text; }
        }
        public string totalamount
        {
            set { lbtotalamt.Text = value; }       get { return lbtotalamt.Text; }
        }
        public string paidamount
        {
            set { lbpaidamt.Text = value; }         get { return lbpaidamt.Text; }
        }
        public string contact
        {
            set { lbcontact.Text = value; }         get { return lbcontact.Text; }
        }
        #endregion
      
        private void DueUpdate_Load(object sender, EventArgs e)
        {
            dtReceiveDate.Format = DateTimePickerFormat.Custom;
            dtReceiveDate.CustomFormat = "yyyy-MM-dd";
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            
            DueList go = new DueList();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void DueUpdate_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        #region Request submit
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtReceive.Text == "" )
            {
                MessageBox.Show("You are Not able to Update", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    decimal received = Convert.ToDecimal(txtReceive.Text);
                    decimal due = Convert.ToDecimal(lbDueAmount.Text);
                    if (received <= due)
                    {
                        decimal remainingDue = due - received;
                        string salesId = lbsalesid.Text;
                        string receiveDate = dtReceiveDate.Text;        // 'yyyy-MM-dd'
                        decimal totalAmt = Convert.ToDecimal(lbtotalamt.Text);
                        string custId = lbcontact.Text;

                        // Update the invoice due and write the payment history together
                        DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                        {
                            tx.Execute("UPDATE sales_payment set due_amount = @due where sales_id = @id",
                                       DataAccess.P("@due", remainingDue),
                                       DataAccess.P("@id", salesId));

                            tx.Execute(" insert into tbl_duepayment (receivedate, sales_id, totalamt, dueamt, receiveamt, custid, emp_id, Shopid) " +
                                       " values (@rdate, @id, @total, @due, @received, @custid, @emp, @shopid)",
                                       DataAccess.P("@rdate", receiveDate),
                                       DataAccess.P("@id", salesId),
                                       DataAccess.P("@total", totalAmt),
                                       DataAccess.P("@due", remainingDue),
                                       DataAccess.P("@received", received),
                                       DataAccess.P("@custid", custId),
                                       DataAccess.P("@emp", UserInfo.UserName),
                                       DataAccess.P("@shopid", UserInfo.Shopid));
                        });

                        MessageBox.Show("Successfully Data Updated!", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtReceive.Text = string.Empty;

                        this.Hide();
                        DueList go = new DueList();
                        go.MdiParent = this.ParentForm;
                        go.Show();
                    }
                    else
                    {
                        MessageBox.Show("You are Not able to Update \n\n Excced Due amount ", "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
                catch (Exception exLog) { Logger.Show(exLog, "Could not save the due payment. Nothing has been changed."); }
            }
        }

        #endregion
    }
}
