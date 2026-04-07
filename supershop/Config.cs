using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace supershop
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
           // txtTrVAT.CharacterCasing = CharacterCasing.Upper;
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        //bind terminal
        public void terminallist()
        {
            string sqlterminallist = "select Shopid as 'ID', Branchname	 ,Location ,Phone ,  " + 
                                     " VAT as 'TAX %' ,Dis as 'Discount %'    from tbl_terminallocation";
            DataAccess.ExecuteSQL(sqlterminallist);
            DataTable dtterminallist = DataAccess.GetDataTable(sqlterminallist);
            dtgrdViewTerminallist.DataSource = dtterminallist;
        }

        private void Config_Load(object sender, EventArgs e)
        {
            try
            {
                //Bind store info 
                string sql3 = "select * from storeconfig";
                DataAccess.ExecuteSQL(sql3);
                DataTable dt1 = DataAccess.GetDataTable(sql3);

                txtCompanyName.Text = dt1.Rows[0].ItemArray[1].ToString();
                txtCompanyAddress.Text = dt1.Rows[0].ItemArray[2].ToString();
                txtPhone.Text = dt1.Rows[0].ItemArray[3].ToString();
                txtVatRegiNo.Text = dt1.Rows[0].ItemArray[4].ToString();
                txtWebSite.Text = dt1.Rows[0].ItemArray[5].ToString();
                lblid.Text = dt1.Rows[0].ItemArray[0].ToString();
                txtVATRate.Text = dt1.Rows[0].ItemArray[6].ToString();
                txtDiscountRate.Text = dt1.Rows[0].ItemArray[7].ToString();
                txtFootermsg.Text = dt1.Rows[0].ItemArray[8].ToString();

                txtTrweb.Text = dt1.Rows[0].ItemArray[5].ToString();               
                txtTrFootermsg.Text = dt1.Rows[0].ItemArray[8].ToString();
                terminallist();
                dtgrdViewTerminallist.Columns[1].SortMode = DataGridViewColumnSortMode.NotSortable;

                if (lblShopID.Text == "-")
                {
                    btnAddnew.Visible = false ;
                    lnkDelete.Visible = false;
                }
                else
                {
                    btnAddnew.Visible = true;
                    lnkDelete.Visible = true;
                }
            }
            catch
            {
            }
          
        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            if (txtCompanyName.Text == "" || txtCompanyAddress.Text == "" || txtPhone.Text == "" || txtVATRate.Text == "" || txtDiscountRate.Text == "")
            {
                // MessageBox.Show("You are Not able to Update");
                MessageBox.Show("You are Not able to Update", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    
                    string sql = "update storeconfig set companyname= '" + txtCompanyName.Text + "', companyaddress = '" + txtCompanyAddress.Text + "', " + 
                        " companyphone = '" + txtPhone.Text + "', vatno = '" + txtVatRegiNo.Text + "' , web = '" + txtWebSite.Text + "' ,    " +
                         " vatrate = '" + txtVATRate.Text + "', disrate = '" + txtDiscountRate.Text + "' , footermsg = '" + txtFootermsg.Text + "'   " +
                        "  where id = '" + lblid.Text + "'";
                    DataAccess.ExecuteSQL(sql);

                    string sql2 = "update tbl_terminallocation set Branchname = '" + txtCompanyName.Text + "', Location = '" + txtCompanyAddress.Text + "', " +
                        "Phone = '" + txtPhone.Text + "', VAT = '" + txtVATRate.Text + "' , Web = '" + txtWebSite.Text + "' ,    " +
                        " Dis = '" + txtDiscountRate.Text + "', VATRegiNo = '" + txtVatRegiNo.Text + "' , Footermsg = '" + txtFootermsg.Text + "'   " +
                        " , CompanyName = '" + txtCompanyName.Text + "'  where Shopid = 'MTQC02' ";
                    DataAccess.ExecuteSQL(sql2);

                    lblmsg.Text = "Configuation has been Saved";
                    lblmsg.Visible = true;

                }
                catch (Exception exp)
                {
                    MessageBox.Show("Sorry\r\n You have to Check the Data" + exp.Message);
                }
            }
        }

        private void groupBox1_MouseHover(object sender, EventArgs e)
        {
            lblmsg.Visible = false;
        }

        private void txtVATRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtVATRate.Text.ToString(), @"\.\d\d\d");

                if (e.KeyChar == '\b') // Always allow a Backspace
                    ignoreKeyPress = false;
                else if (matchString)
                    ignoreKeyPress = true;
                else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    ignoreKeyPress = true;
                else if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                    ignoreKeyPress = true;

                e.Handled = ignoreKeyPress;
                //using System.Text.RegularExpressions;
            }
            catch
            {
            } 
        }

        private void txtDiscountRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtDiscountRate.Text.ToString(), @"\.\d\d\d");

                if (e.KeyChar == '\b') // Always allow a Backspace
                    ignoreKeyPress = false;
                else if (matchString)
                    ignoreKeyPress = true;
                else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    ignoreKeyPress = true;
                else if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                    ignoreKeyPress = true;

                e.Handled = ignoreKeyPress;
                //using System.Text.RegularExpressions;
            }
            catch
            {
            } 
        }

        // Click terminal list and move to add and update
        private void dtgrdViewTerminallist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow row = dtgrdViewTerminallist.Rows[e.RowIndex];
                string terminalid = row.Cells[0].Value.ToString();


                string sqlterminallist = "select Shopid  , Branchname , Location ,Phone , Email , " +
                                         " Web, VAT , Dis , VATRegiNo , Footermsg    from tbl_terminalLocation " +
                                         " where Shopid = '" + terminalid + "' ";
                DataAccess.ExecuteSQL(sqlterminallist);
                DataTable dtterminallist = DataAccess.GetDataTable(sqlterminallist);

                lblShopID.Text          = dtterminallist.Rows[0].ItemArray[0].ToString(); 
                txtterminalname.Text    = dtterminallist.Rows[0].ItemArray[1].ToString();
                txtTerminaladdress.Text = dtterminallist.Rows[0].ItemArray[2].ToString();
                txtTerminalPhone.Text   = dtterminallist.Rows[0].ItemArray[3].ToString();
                txtTremail.Text         = dtterminallist.Rows[0].ItemArray[4].ToString();
                txtTrweb.Text           = dtterminallist.Rows[0].ItemArray[5].ToString();
                txtTrVAT.Text           = dtterminallist.Rows[0].ItemArray[6].ToString();
                txtTrDis.Text           = dtterminallist.Rows[0].ItemArray[7].ToString();
                txtTrVATregino.Text     = dtterminallist.Rows[0].ItemArray[8].ToString();
                txtTrFootermsg.Text     = dtterminallist.Rows[0].ItemArray[9].ToString();
                tabControl1.SelectedTab = tabterminal;
                btnAddnew.Visible = true;
                lnkDelete.Visible = true;
                lbltrmsg.Visible = false;
            }
            catch
            {
            }
       
                       
        }
      
        private void bntTrSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtterminalname.Text == "" || txtTerminaladdress.Text == "" || txtTerminalPhone.Text == "" || txtTrVAT.Text == "" || txtTrDis.Text == "")
                {                     
                    MessageBox.Show("Please fill Terminal info", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    //Add new Terminal Info
                    if (lblShopID.Text == "-")
                    {
                        string Shopid = txtterminalname.Text.Substring(0, 2) + txtTrVATregino.Text.Substring(0, 2);
                        string sqlinsert = " insert into tbl_terminallocation  (Shopid, CompanyName, Branchname , Location ,Phone , Email ,  Web, VAT , Dis , VATRegiNo , Footermsg ) " +
                                           " values ('" + Shopid + "' , '" + txtCompanyName.Text + "' , '" + txtterminalname.Text + "' , " +
                                            " '" + txtTerminaladdress.Text + "' , '" + txtTerminalPhone.Text + "' , '" + txtTremail.Text + "' ," + 
                                            " '" + txtTrweb.Text + "',  '" + txtTrVAT.Text + "', " +
                                            " '" + txtTrDis.Text + "' , '" + txtTrVATregino.Text + "',  '" + txtTrFootermsg.Text + "')";
                        DataAccess.ExecuteSQL(sqlinsert);
                        lbltrmsg.Text = "Submitted a new Terminal";
                        lbltrmsg.Visible = true;
                        terminallist();
                        tabControl1.SelectedTab = tabterminallist;
                    }
                    else // Update selected 
                    {
                        string sql = "update tbl_terminallocation set Branchname = '" + txtterminalname.Text + "', Location = '" + txtTerminaladdress.Text + "', " +
                        " Email = '" + txtTremail.Text + "' , Phone = '" + txtTerminalPhone.Text + "', VAT = '" + txtTrVAT.Text + "' , Web = '" + txtTrweb.Text + "' ,    " +
                        " Dis = '" + txtTrDis.Text + "', VATRegiNo = '" + txtTrVATregino.Text + "' , Footermsg = '" + txtTrFootermsg.Text + "'   " +
                        " , CompanyName = '" + txtCompanyName.Text + "'  where Shopid = '" + lblShopID.Text + "' ";
                        DataAccess.ExecuteSQL(sql);
                        lbltrmsg.Text = "Terminal info has been Saved";
                        lbltrmsg.Visible = true;
                        terminallist();
                        tabControl1.SelectedTab = tabterminallist;
                    }
                }
             
            }
            catch
            {
            }
        }

        // Prevent String value
        private void txtTrVAT_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtTrVAT.Text.ToString(), @"\.\d\d\d");

                if (e.KeyChar == '\b') // Always allow a Backspace
                    ignoreKeyPress = false;
                else if (matchString)
                    ignoreKeyPress = true;
                else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    ignoreKeyPress = true;
                else if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                    ignoreKeyPress = true;

                e.Handled = ignoreKeyPress;
                //using System.Text.RegularExpressions;
            }
            catch
            {
            } 
        }

        // Prevent String value
        private void txtTrDis_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                bool ignoreKeyPress = false;

                bool matchString = Regex.IsMatch(txtTrDis.Text.ToString(), @"\.\d\d\d");

                if (e.KeyChar == '\b') // Always allow a Backspace
                    ignoreKeyPress = false;
                else if (matchString)
                    ignoreKeyPress = true;
                else if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
                    ignoreKeyPress = true;
                else if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
                    ignoreKeyPress = true;

                e.Handled = ignoreKeyPress;
                //using System.Text.RegularExpressions;
            }
            catch
            {
            } 
        }

        private void btnAddnew_Click(object sender, EventArgs e)
        {
            txtterminalname.Text = string.Empty;
            txtTerminaladdress.Text = string.Empty;
            txtVatRegiNo.Text = string.Empty;
            txtTrweb.Text = txtWebSite.Text;
            txtTrFootermsg.Text = txtFootermsg.Text;
            lblShopID.Text = "-";
        }

        private void helplnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            parameter.helpid = "config";  
            HelpPage go = new HelpPage();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void lnkDelete_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Delete?", "Yes or No", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {

                if (lblShopID.Text == "-")
                {
                    // MessageBox.Show("You are Not able to Update");
                    MessageBox.Show("You are Not able to Delete", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        string sql = "delete from tbl_terminalLocation where Shopid ='" + lblShopID.Text + "'";
                        DataAccess.ExecuteSQL(sql);
                        MessageBox.Show("successfully Data Delete !", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        terminallist();
                        tabControl1.SelectedTab = tabterminallist;
                    

                    }
                    catch (Exception exp)
                    {
                        MessageBox.Show("Sorry\r\n You have to Check the Data" + exp.Message);
                    }
                }
            }
        }
 
    }
}
