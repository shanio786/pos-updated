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
    public partial class SalesMan_Home : Form
    {
        public SalesMan_Home()
        {
            InitializeComponent();
            toolStripMenuItemUser.Text = UserInfo.UserName; 
            toolStripStatusLabel2.Text = UserInfo.UserName; 
            userProfileToolStripMenuItem.Text = " User Profile ( " + UserInfo.UserName +")";
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

        private void purchaseProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Add_Item go = new Add_Item(); 
            go.MdiParent = this;
            go.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Do you want to Sign out?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Login go = new Login();
                go.Show();
                this.Close();
            }
        }       

        private void SalesMan_Home_Load(object sender, EventArgs e)
        {
            //About_soft go = new About_soft();
            //go.MdiParent = this;
            //go.Show();         
           
        }

        private void aboutSoftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About_soft go = new About_soft();
            go.MdiParent = this;
            go.Show();
        }

        private void toolStripStatusLabel6_Click(object sender, EventArgs e)
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
      

        private void timer2_Tick(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            toolStripMenuItem2.Text = "System is Running " + DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void toolStripMenuItem3Mini_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;   //Minimized 
        }

        private void xToolStripMenuItem_Click_1(object sender, EventArgs e)
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

        private void toolStripMenuItem3_Click_1(object sender, EventArgs e)
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

        private void menuStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void statusStrip1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void saleRegisterBarcodeScannerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesRegister go = new SalesRegister();
            go.MdiParent = this;
            go.Show();
           // statusStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
        }

        private void saveAsDatabaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime today = DateTime.Today;
                string fileName = "psodb.db";
                string fileName2 = "posBackup_" + DateTime.Now.ToString("yyyy-MM-dd_hh-mm-ss" + ".db");
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

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip1.Dock = System.Windows.Forms.DockStyle.Top;
        }

        private void toolStripStatusLabel5_Click(object sender, EventArgs e)
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

        private void toolStripMenuItemUser_Click(object sender, EventArgs e)
        {
            User_mgt.UserProfile go = new User_mgt.UserProfile(toolStripMenuItemUser.Text);
            go.MdiParent = this;
            go.Show(); 
        }

        private void createBarcodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //BarcodeRDLC go = new BarcodeRDLC();
            //go.MdiParent = this;
            //go.Show();

            BarCode.Barcode_machine go = new BarCode.Barcode_machine();
            go.MdiParent = this;
            go.Show();
        }

        private void toolStripStatusLblAddSale_Click(object sender, EventArgs e)
        {
            if (Application.OpenForms["createBarcode"] == null)
            {
                Inventory.Add_Sales go = new Inventory.Add_Sales();
                go.MdiParent = this;
                go.Show();
            }
            else
            {
                MessageBox.Show("Form is already open");
            }
            
        }

        private void addCustomerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Customer.AddNewCustomer go = new Customer.AddNewCustomer();
            go.MdiParent = this;
            go.Show();
        }

        private void kitchenDisplayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Report.Kitchen_display go = new Report.Kitchen_display();
            go.ShowDialog();
        }

        private void quickSalesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SalesRegisterQC go = new SalesRegisterQC();
            go.MdiParent = this;
            go.Show();
        }

        private void dueListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DueList go = new DueList();
            go.MdiParent = this;
            go.Show();
        }
    }
}
