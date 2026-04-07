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
    public partial class Manager_Home : Form
    {
        public Manager_Home()
        {
            InitializeComponent();
            toolStripMenuItemUserName.Text = UserInfo.UserName;
            toolStripStatusLabel7.Text = UserInfo.UserName;
            userProfileToolStripMenuItem.Text = " User Profile ( " + UserInfo.UserName + ")";
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                // this.Close();
                // Normal and Maximized
                if (this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Maximized;
                }
                else if (this.WindowState == FormWindowState.Maximized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
            return base.ProcessCmdKey(ref msg, keyData);
        } 
        

        private void sellProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesRegister go = new SalesRegister();
            go.MdiParent = this;
            go.Show();
        }    

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            salesreport go = new salesreport();
            go.MdiParent = this;
            go.Show();
        }

        private void productListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stock_List go = new Stock_List();
            go.MdiParent = this;
            go.Show();
        }

        private void purchaseProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Item go = new Add_Item(); go.MdiParent = this;
            go.Show();
        }
      

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Sign out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                workRecords();
                Login go = new Login();
                go.Show();
                this.Close();
            }
        }

        public void workRecords()
        {
            string logdate = DateTime.Now.ToString("yyyy-MM-dd");
            string logtime = DateTime.Now.ToString("HH:mm:ss");
            string logdatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            string sqlLogIn = " insert into tbl_workrecords (Username, datatype, logdate, logtime, logdatetime) " +
                                 " values ('" + UserInfo.UserName + "' , 'OUT' , '" + logdate + "' , " +
                                  " '" + logtime + "' , '" + logdatetime + "'  )";
            DataAccess.ExecuteSQL(sqlLogIn);
        }

        private void Manager_Home_Load(object sender, EventArgs e)
        {
            //About_soft go = new About_soft();
            //go.MdiParent = this;
            //go.Show();

            try
            {
                //Bind store info 
                string sql3 = "select * from storeconfig";
                DataAccess.ExecuteSQL(sql3);
                DataTable dt1 = DataAccess.GetDataTable(sql3);

                companynamelabel.Text = dt1.Rows[0].ItemArray[1].ToString();

            }
            catch
            {
            }
        }

        private void aboutSoftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_soft go = new About_soft();
            go.MdiParent = this;
            go.Show();
        }

 

        private void timer1_Tick(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            toolStripMenuItem2.Text = "System is Running  " + DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void saveAsDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;
                string fileName = "psodb.db";
                string fileName2 = "posBackup_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss");
                string sourcePath = Application.StartupPath; //Application.StartupPath + @"\FinalImage\";
                string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop); // @"C:\Users\Public\TestFolder\SubDir";

                // Use Path class to manipulate file and directory paths. 
                string sourceFile = System.IO.Path.Combine(sourcePath, fileName);
                string destFile = System.IO.Path.Combine(targetPath, fileName2);

                // To copy a folder's contents to a new location: 
                // Create a new target folder, if necessary. 
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);

                }

                System.IO.File.Copy(sourceFile, destFile, true);

                //  File.SetAttributes(destFile, File.GetAttributes(destFile) | (FileAttributes.Archive | FileAttributes.ReadOnly));                

                MessageBox.Show("Your Backup is Created !!! ... \n " +
                                 "Please check  your Desktop And \n Keep --posBackup-- File In your Secure folder. " +
                                 "\n You should try to keep  the File  " + 
                                 "\n If File is not Appear Please Show hidden files; From the Folder Option  ", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch
            {
            }
        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Dock = System.Windows.Forms.DockStyle.Top;
        }

        private void dockStyleLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Dock = System.Windows.Forms.DockStyle.Left;
        }

        private void dockStyleRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Dock = System.Windows.Forms.DockStyle.Right;
        }

        private void dockStyleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
        }

        private void Manager_Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult result = MessageBox.Show("Do you want to Sign out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (result == DialogResult.Yes)
            //{
            //    // Application.Exit();
            //    Login go = new Login();
            //    go.Show();
            //    this.Close();
            //}
        }

        private void Manager_Home_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }
        

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;   //Minimized 
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            // Normal and Maximized
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private void toolStripStatusLabel4_Click(object sender, EventArgs e)
        {
            Return_product go = new Return_product();
            go.MdiParent = this;
            go.Show();
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HelpPage go = new HelpPage();
            go.MdiParent = this;
            go.Show();
        }

        private void createBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarCode.Barcode_machine go = new BarCode.Barcode_machine();
            go.MdiParent = this;
            go.Show();
            //BarcodeRDLC go = new BarcodeRDLC();
            //    go.MdiParent = this;
            //    go.Show();
         
        }

        private void toolStripMenuItem19_Click(object sender, EventArgs e)
        {
            Inventory.Add_Sales go = new Inventory.Add_Sales();
            go.MdiParent = this;
            go.Show();
        }

        private void toolStripMenuItemUserName_Click(object sender, EventArgs e)
        {
            User_mgt.UserProfile go = new User_mgt.UserProfile(toolStripMenuItemUserName.Text);
            go.MdiParent = this;
            go.Show();
        }

        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.AddNewCustomer go = new Customer.AddNewCustomer();
            go.MdiParent = this;
            go.Show();
        }

        private void customerReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.CustomerDetails go = new Customer.CustomerDetails();
            go.MdiParent = this;
            go.Show();
        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Items.Categories go = new Items.Categories();
            go.MdiParent = this;
            go.Show();
        }

        private void kitchenDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.Kitchen_display go = new Report.Kitchen_display();
            go.ShowDialog();
        }

        private void purchaseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Items.Purchase_History go = new Items.Purchase_History();
            go.MdiParent = this;
            go.Show();
        }

        private void toolStripMenuQuickSale_Click(object sender, EventArgs e)
        {
            SalesRegisterQC go = new SalesRegisterQC();
            go.MdiParent = this;
            go.Show();
        }      

        
    }
}
