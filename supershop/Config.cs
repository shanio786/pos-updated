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
            string sqlterminallist = "select Shopid as 'ID', Branchname, Location, Phone, " +
                                     " VAT as 'TAX %', Dis as 'Discount %' from tbl_terminallocation";
            DataTable dtterminallist = DataAccess.GetDataTable(sqlterminallist);
            dtgrdViewTerminallist.DataSource = dtterminallist;
        }

        private void Config_Load(object sender, EventArgs e)
        {
            try
            {
                //Bind store info
                string sql3 = "select * from storeconfig";
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

                bool hasTerminal = (lblShopID.Text != "-");
                btnAddnew.Visible = hasTerminal;
                lnkDelete.Visible = hasTerminal;
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        private void bntSave_Click(object sender, EventArgs e)
        {
            if (txtCompanyName.Text == "" || txtCompanyAddress.Text == "" || txtPhone.Text == "" || txtVATRate.Text == "" || txtDiscountRate.Text == "")
            {
                MessageBox.Show("You are Not able to Update", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                try
                {
                    decimal vatrate = Convert.ToDecimal(txtVATRate.Text);
                    decimal disrate = Convert.ToDecimal(txtDiscountRate.Text);

                    // Store info and the main terminal row are kept in step, so both updates commit together
                    DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
                    {
                        tx.Execute("update storeconfig set companyname = @name, companyaddress = @address, companyphone = @phone, " +
                                   " vatno = @vatno, web = @web, vatrate = @vatrate, disrate = @disrate, footermsg = @footer " +
                                   " where id = @id",
                            DataAccess.P("@name", txtCompanyName.Text),
                            DataAccess.P("@address", txtCompanyAddress.Text),
                            DataAccess.P("@phone", txtPhone.Text),
                            DataAccess.P("@vatno", txtVatRegiNo.Text),
                            DataAccess.P("@web", txtWebSite.Text),
                            DataAccess.P("@vatrate", vatrate),
                            DataAccess.P("@disrate", disrate),
                            DataAccess.P("@footer", txtFootermsg.Text),
                            DataAccess.P("@id", Convert.ToInt32(lblid.Text)));

                        tx.Execute("update tbl_terminallocation set Branchname = @name, Location = @address, Phone = @phone, " +
                                   " VAT = @vatrate, Web = @web, Dis = @disrate, VATRegiNo = @vatno, Footermsg = @footer, " +
                                   " CompanyName = @name where Shopid = @shopid",
                            DataAccess.P("@name", txtCompanyName.Text),
                            DataAccess.P("@address", txtCompanyAddress.Text),
                            DataAccess.P("@phone", txtPhone.Text),
                            DataAccess.P("@vatrate", vatrate),
                            DataAccess.P("@web", txtWebSite.Text),
                            DataAccess.P("@disrate", disrate),
                            DataAccess.P("@vatno", txtVatRegiNo.Text),
                            DataAccess.P("@footer", txtFootermsg.Text),
                            DataAccess.P("@shopid", UserInfo.Shopid));
                    });

                    lblmsg.Text = "Configuation has been Saved";
                    lblmsg.Visible = true;
                }
                catch (Exception exp)
                {
                    Logger.Show(exp, "Could not save configuration");
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
            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        // Click terminal list and move to add and update
        private void dtgrdViewTerminallist_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex < 0) return;
                DataGridViewRow row = dtgrdViewTerminallist.Rows[e.RowIndex];
                string terminalid = Convert.ToString(row.Cells[0].Value);

                string sqlterminallist = "select Shopid, Branchname, Location, Phone, Email, " +
                                         " Web, VAT, Dis, VATRegiNo, Footermsg from tbl_terminalLocation " +
                                         " where Shopid = @shopid";
                DataTable dtterminallist = DataAccess.GetDataTable(sqlterminallist, DataAccess.P("@shopid", terminalid));
                if (dtterminallist.Rows.Count == 0) return;

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
            catch (Exception exLog) { Logger.Error(exLog); }
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
                    decimal vat = Convert.ToDecimal(txtTrVAT.Text);
                    decimal dis = Convert.ToDecimal(txtTrDis.Text);

                    //Add new Terminal Info
                    if (lblShopID.Text == "-")
                    {
                        string Shopid = txtterminalname.Text.Substring(0, 2) + txtTrVATregino.Text.Substring(0, 2);
                        DataAccess.ExecuteSQL(" insert into tbl_terminallocation (Shopid, CompanyName, Branchname, Location, Phone, Email, Web, VAT, Dis, VATRegiNo, Footermsg) " +
                                              " values (@shopid, @company, @branch, @location, @phone, @email, @web, @vat, @dis, @vatno, @footer)",
                            DataAccess.P("@shopid", Shopid),
                            DataAccess.P("@company", txtCompanyName.Text),
                            DataAccess.P("@branch", txtterminalname.Text),
                            DataAccess.P("@location", txtTerminaladdress.Text),
                            DataAccess.P("@phone", txtTerminalPhone.Text),
                            DataAccess.P("@email", txtTremail.Text),
                            DataAccess.P("@web", txtTrweb.Text),
                            DataAccess.P("@vat", vat),
                            DataAccess.P("@dis", dis),
                            DataAccess.P("@vatno", txtTrVATregino.Text),
                            DataAccess.P("@footer", txtTrFootermsg.Text));
                        lbltrmsg.Text = "Submitted a new Terminal";
                        lbltrmsg.Visible = true;
                        terminallist();
                        tabControl1.SelectedTab = tabterminallist;
                    }
                    else // Update selected
                    {
                        DataAccess.ExecuteSQL("update tbl_terminallocation set Branchname = @branch, Location = @location, Email = @email, " +
                                              " Phone = @phone, VAT = @vat, Web = @web, Dis = @dis, VATRegiNo = @vatno, Footermsg = @footer, " +
                                              " CompanyName = @company where Shopid = @shopid",
                            DataAccess.P("@branch", txtterminalname.Text),
                            DataAccess.P("@location", txtTerminaladdress.Text),
                            DataAccess.P("@email", txtTremail.Text),
                            DataAccess.P("@phone", txtTerminalPhone.Text),
                            DataAccess.P("@vat", vat),
                            DataAccess.P("@web", txtTrweb.Text),
                            DataAccess.P("@dis", dis),
                            DataAccess.P("@vatno", txtTrVATregino.Text),
                            DataAccess.P("@footer", txtTrFootermsg.Text),
                            DataAccess.P("@company", txtCompanyName.Text),
                            DataAccess.P("@shopid", lblShopID.Text));
                        lbltrmsg.Text = "Terminal info has been Saved";
                        lbltrmsg.Visible = true;
                        terminallist();
                        tabControl1.SelectedTab = tabterminallist;
                    }
                }
            }
            catch (Exception exLog) { Logger.Show(exLog, "Could not save terminal info"); }
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
            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
            }
            catch (Exception exLog) { Logger.Error(exLog); }
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
                    MessageBox.Show("You are Not able to Delete", "Button3 Title", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    try
                    {
                        DataAccess.ExecuteSQL("delete from tbl_terminalLocation where Shopid = @shopid", DataAccess.P("@shopid", lblShopID.Text));
                        MessageBox.Show("successfully Data Delete !", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        terminallist();
                        tabControl1.SelectedTab = tabterminallist;
                    }
                    catch (Exception exp)
                    {
                        Logger.Show(exp, "Could not delete terminal");
                    }
                }
            }
        }
    }
}
