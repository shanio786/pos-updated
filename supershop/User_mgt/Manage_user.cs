using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace supershop.User_mgt
{
    public partial class Manage_user : Form
    {
        public Manage_user()
        {
            InitializeComponent();
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
                this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }
       
        // Columns shown on the user tiles - the password column is deliberately never selected here.
        const string UserColumns = "select id, Name, Username, Contact, position, Email, Shopid, imagename from usermgt ";

        //Show User List with image
        public void list_images()
        {
            try
            {
                DataTable dt = DataAccess.GetDataTable(UserColumns + " order by id");
                ShowUsers(dt);
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        //Search User with Image
        private void txtsearchUser_TextChanged(object sender, EventArgs e)
        {
            flowLayoutPanelUserList.Controls.Clear();
            try
            {
                DataTable dt = DataAccess.GetDataTable(
                    UserColumns + " where Name like @q + '%' OR Username like @q + '%' OR Contact like @q + '%' OR position like @q + '%' order by id",
                    DataAccess.P("@q", txtsearchUser.Text));
                ShowUsers(dt);
            }
            catch (Exception exLog) { Logger.Error(exLog); }
        }

        // One tile (button) per user row; clicking a tile opens the user for editing.
        private void ShowUsers(DataTable dt)
        {
            string img_directory = Application.StartupPath + @"\IMAGE\";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow row = dt.Rows[i];

                Button b = new Button();
                b.Tag = row["id"];
                b.Click += new EventHandler(b_Click);
                b.Name = "user_" + row["id"];

                string imagePath = img_directory + row["imagename"];
                if (File.Exists(imagePath))
                {
                    try
                    {
                        ImageList il = new ImageList();
                        il.ColorDepth = ColorDepth.Depth32Bit;
                        il.TransparentColor = Color.Transparent;
                        il.ImageSize = new Size(150, 120);
                        il.Images.Add(Image.FromFile(imagePath));
                        b.Image = il.Images[0];
                    }
                    catch (Exception exImg) { Logger.Error(exImg); } // a bad picture must not hide the user
                }

                b.Margin = new Padding(4, 4, 4, 4);
                b.Size = new Size(330, 130);

                b.Text += "\n UID: " + row["Username"];
                b.Text += "\n Name: " + row["Name"];
                b.Text += "\n Contact: " + row["Contact"];
                b.Text += "\n Position: " + row["position"];
                b.Text += "\n " + row["Email"];
                b.Text += "\n " + row["Shopid"];

                b.Font = new Font("Times New Roman", 10, FontStyle.Regular, GraphicsUnit.Point);
                b.TextAlign = ContentAlignment.TopLeft;
                b.TextImageRelation = TextImageRelation.ImageBeforeText;
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                flowLayoutPanelUserList.Controls.Add(b);
            }
        }

        //Click add to cart
        protected void b_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            string s;
            s = b.Tag.ToString();

            this.Hide();
            User_mgt.User_regi go = new User_mgt.User_regi();
            go.Uid = s;
            go.MdiParent = this.ParentForm;
            go.Show();
        }
        

        private void Manage_user_Load(object sender, EventArgs e)
        {
            list_images();
        }

        // Link to   user registration
        private void btnCreateLink_Click(object sender, EventArgs e)
        {
            this.Hide();
            User_mgt.User_regi go = new User_mgt.User_regi();
            go.MdiParent = this.ParentForm;
            go.Show();           
            
        }

        private void Manage_user_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MoveForm.ReleaseCapture();
                MoveForm.SendMessage(Handle, MoveForm.WM_NCLBUTTONDOWN, MoveForm.HT_CAPTION, 0);
            }
        }

        private void lnkWorkingHours_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            User_mgt.WorkSheet go = new User_mgt.WorkSheet();
            go.MdiParent = this.ParentForm;
            go.Show();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            User_mgt.User_attendence ua = new User_mgt.User_attendence();
            ua.Show();
            ua.MdiParent = this;
        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new PayRoll().Show();
        }
    }
}
