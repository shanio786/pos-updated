using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace supershop
{
    public partial class Home : Form
    {
        Login lfrm = new Login();
        public Home()
        {
            InitializeComponent();
            tsmIUserName.Text = UserInfo.UserName;
            toolStripStatusLabel7.Text = "User: " + UserInfo.UserName;
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

        private void Home_Load(object sender, EventArgs e)
        {
            //SalesRegister go = new SalesRegister();
            //go.MdiParent = this;
            //go.Show();
            SalesRegisterQC go = new SalesRegisterQC();
            go.MdiParent = this;
            go.Show();

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

        private void purchaseProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Item go = new Add_Item(); 
            go.MdiParent = this;
            go.Show();
        }

        private void productListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Stock_List"] == null)
            {
                Stock_List go = new Stock_List();
                go.MdiParent = this;
                go.Show();
            }
            else
            {
                MessageBox.Show("Stock windows is already open");
            }
           
        }

        private void importItemsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Import_Items go = new Import_Items();
            go.MdiParent = this;
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

        private void salesReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //salesreport go = new salesreport(); 
            Report.ReportDialog go = new Report.ReportDialog();
           // go.MdiParent = this;
            go.ShowDialog();
        }
       

        private void systemConfigurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Config go = new Config();          
            go.ShowDialog();
        }

        private void userRegistrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User_mgt.User_regi go = new User_mgt.User_regi();
            go.MdiParent = this;
            go.Show();
        }
 
        private void manageUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User_mgt.Manage_user go = new User_mgt.Manage_user();
            go.MdiParent = this;
            go.Show();
        }

        private void salesRegisterBarcodeScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesRegister go = new SalesRegister();
            go.MdiParent = this;
            go.Show();

            statusStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
        }

        private void salesRegistertoolStripStatus_Click(object sender, EventArgs e)
        {
            SalesRegister go = new SalesRegister();
            go.MdiParent = this;
            go.Show(); 
        }  

        private void aboutSoftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_soft go = new About_soft();
            go.MdiParent = this;
            go.Show();
        }

        private void Home_FormClosing(object sender, FormClosingEventArgs e)
        {
            //DialogResult result = MessageBox.Show("Do you want to Close this System ?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (result == DialogResult.No)
            //{
            //    e.Cancel = true;
            //}
        }

        private void DateTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // DateTime today = DateTime.Today;
                DatetimertoolStripMenu.Text = DateTime.Now.ToString("hh:mm:ss tt");
            }
            catch
            {
            }
        } 


        private void Home_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void helpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start("\POS_Document\index.html");
            //webbrowser.Navigate("File location.html")
            //System.Diagnostics.Process.Start(Microsoft.SqlServer.Server.MapPath("~/HtmlFileFolderNameInSolution/") + "HtmlFileName.htm");
            HelpPage go = new HelpPage();
            go.MdiParent = this;
            go.Show();
        }

        private void helplinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = Application.StartupPath + @"\POS_Document\index.html";
            System.Diagnostics.Process.Start(path);
        }


        private void toolStripMenuItem3Mini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;   //Minimized 
        }


        //// Normal and Maximized
        private void menuStrip1_MouseDoubleClick(object sender, MouseEventArgs e)
        {

            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Maximized;
            }
            else if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        //Minimized 
        private void MinimizertoolStripMenu_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;   
        }

        //Restore
        private void RestoretoolStripMenu_Click(object sender, EventArgs e)
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

       

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            toolStripMenuOn_statusMenu.Visible = true;
            statusStrip1.Visible = false;
        }

        private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            statusStrip1.Visible = true;
            toolStripMenuOn_statusMenu.Visible = false;
        }
      

        private void overviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Overview go = new Overview();
            go.MdiParent = this;
            go.Show();
        }

        private void saleChartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Sale_chart go = new Sale_chart();
            go.MdiParent = this;
            go.Show();
        }

        private void importDBBackupToolStripMenuItem_Click(object sender, EventArgs e)   // import and delete 
        {           
            Database_import go = new Database_import();
            go.MdiParent = this;
            go.Show();          

        }    

        private void returnProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Return_product go = new Return_product();
            go.MdiParent = this;
            go.Show();
        }

        private void generalLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.LedgerReport go = new Report.LedgerReport();
            go.MdiParent = this;
            go.Show();
        }

        private void dueListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DueList go = new DueList();
            go.MdiParent = this;
            go.Show();
        }

        private void todaySaleToolStripMenuItem_Click(object sender, EventArgs e)
        { 
            Report.ShortCutReport go = new Report.ShortCutReport();
            go.DTtoday = DateTime.Now.ToString("yyyy-MM-dd");
            go.ReportName = "Daily Report";
            go.MdiParent = this;
            go.Show();
        }

        private void last30DaysSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime dtd = DateTime.Now;
            dtd = dtd.AddDays(-30);

            Report.ShortCutReport go = new Report.ShortCutReport();
            go.last30salesStartDate = dtd.ToString("yyyy-MM-dd"); 
            go.last30salesENDDate = DateTime.Now.ToString("yyyy-MM-dd");
            go.ReportName = "Last 30 Days Report";
            go.MdiParent = this;
            go.Show();
        }

        

        private void addNewCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.AddNewCustomer go = new Customer.AddNewCustomer();
            go.MdiParent = this;
            go.Show();
        }

        private void CustomersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.CustomerDetails go = new Customer.CustomerDetails();
            go.MdiParent = this;
            go.Show();
        }

        private void addSaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Inventory.Add_Sales go = new Inventory.Add_Sales();
            go.MdiParent = this;
            go.Show();
        }

        private void createBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (Application.OpenForms["createBarcode"] == null)
            //{
            //    CreateBarcode go = new CreateBarcode();
            //    go.MdiParent = this;
            //    go.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Form is already open");
            //}
            BarCode.newBarcode_Generator  go = new BarCode.newBarcode_Generator();
            go.MdiParent = this;
            go.Show();
         
        }

        private void userProfileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User_mgt.UserProfile go = new User_mgt.UserProfile(tsmIUserName.Text);
            go.MdiParent = this;
            go.Show();
        }

        private void storeCreditRewardsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.RewardsManagerReport go = new Customer.RewardsManagerReport();
            go.MdiParent = this;
            go.Show();
        }

        private void generalLedgerReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.LedgerReport go = new Report.LedgerReport();
            go.MdiParent = this;
            go.Show();
        }


        // Truncate table / remove data from transaction table
        private void dataResetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Reset your Data?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Data_Manage.DataReset go = new Data_Manage.DataReset();                
                go.ShowDialog();
            }

        }

        private void stockDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Items.StockDetails go = new Items.StockDetails();
            go.MdiParent = this;
            go.Show();

        }

        private void salesReportDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            salesreport go = new salesreport();
            go.MdiParent = this;
            go.Show();
        }

        private void profitLossReportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.PLdialog go = new Report.PLdialog();
           // go.MdiParent = this;
            go.ShowDialog();
        }


        //Only Save
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

        private void saveBackupAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "psoBackup_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".db";
             saveFileDialog1.ShowDialog();
        }


        //Save backup As
        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {
            try
            {                
                string sourceFileName = "psodb.db";
                string destFileFileName = "posBackup_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss") + ".db";
                string sourcePath = Application.StartupPath;
                
                // Get file name and dest path
                string targetPath = saveFileDialog1.FileName;

             //   string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);  

                //// Use Path class to manipulate file and directory paths. 
                string sourceFile = System.IO.Path.Combine(sourcePath, sourceFileName);
                string destFile = System.IO.Path.Combine(targetPath, destFileFileName);



                // To copy a folder's contents to a new location: 
                // Create a new target folder, if necessary. 
                if (!System.IO.Directory.Exists(targetPath))
                {
                    System.IO.Directory.CreateDirectory(targetPath);

                }

               System.IO.File.Copy(sourceFile, destFile, true);

               
              //File.Copy(sourceFile, destFileFileName);
         
            }
            catch
            {
            }

        }

        private void categoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            Items.Categories go = new Items.Categories();
            go.MdiParent = this;
            go.Show();
        }

        private void expensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Expenses.ExpensesList go = new Expenses.ExpensesList();
            go.MdiParent = this;
            go.Show();
        }

        private void topSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.TopSales go = new Report.TopSales();
            go.MdiParent = this;
            go.Show();
        }

        private void barcodeCreatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarCode.BarcodeCreator go = new BarCode.BarcodeCreator();
            go.MdiParent = this;
            go.Show();
        }

        private void backColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (ColorDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    menuStrip1.BackColor = ColorDialog1.Color;
            //    statusStrip1.BackColor = ColorDialog1.Color;
            //}
        }

        private void foreColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ColorDialog foreColor = new ColorDialog();
            if (foreColor.ShowDialog() == DialogResult.OK)
            {
                menuStrip1.ForeColor = foreColor.Color;
                statusStrip1.ForeColor = foreColor.Color;
            }
        }

        private void fontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //if (fontDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    menuStrip1.Font = fontDialog1.Font;                 
            //}
        }

        private void kitchenDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.Kitchen_display go = new Report.Kitchen_display();
            go.ShowDialog();
        }

        private void barcodeMachineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BarCode.Barcode_machine go = new BarCode.Barcode_machine();
            go.MdiParent = this;
            go.Show();
        }

        private void purchaseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["Purchase_History"] == null)
            {
                Items.Purchase_History go = new Items.Purchase_History();
                go.MdiParent = this;
                go.Show();
            }
            else
            {
                MessageBox.Show(" Purchase History windows is already open");
            }
           
        }

        private void workSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            User_mgt.WorkSheet go = new User_mgt.WorkSheet();
            go.MdiParent = this;
            go.Show();
        }

        private void quickSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesRegisterQC go = new SalesRegisterQC();
            go.MdiParent = this;
            go.Show();
        }
    }
}
