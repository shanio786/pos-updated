namespace supershop
{
    partial class Add_Item
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Add_Item));
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtCostPrice = new System.Windows.Forms.TextBox();
            this.txtProductQty = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtSalesPrice = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.ComboCategory = new System.Windows.Forms.ComboBox();
            this.cmbSupplier = new System.Windows.Forms.ComboBox();
            this.txtdiscount = new System.Windows.Forms.TextBox();
            this.cmboShopid = new System.Windows.Forms.ComboBox();
            this.chktaxapply = new System.Windows.Forms.CheckBox();
            this.lnkDelete = new System.Windows.Forms.LinkLabel();
            this.chkkitchenDisplay = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dtpurchaseDate = new System.Windows.Forms.DateTimePicker();
            this.txtNewpQty = new System.Windows.Forms.TextBox();
            this.btnPurchaseHistory = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.picItemimage = new System.Windows.Forms.PictureBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblimagename = new System.Windows.Forms.Label();
            this.lblFileExtension = new System.Windows.Forms.Label();
            this.lblItemcode = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblValidmsg = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpboxPurchasehistory = new System.Windows.Forms.GroupBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lnkSupplier = new System.Windows.Forms.LinkLabel();
            this.lnkcategories = new System.Windows.Forms.LinkLabel();
            this.lnkbulkitems = new System.Windows.Forms.LinkLabel();
            this.lnkStocklist = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picItemimage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpboxPurchasehistory.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.75F);
            this.label3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label3.Location = new System.Drawing.Point(221, 192);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Product Quantity (Qty) *";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label2.Location = new System.Drawing.Point(222, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "Product Name *";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 13.75F);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label1.Location = new System.Drawing.Point(219, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(138, 24);
            this.label1.TabIndex = 5;
            this.label1.Text = "Product Code *";
            this.toolTip1.SetToolTip(this.label1, "Bar Code or unique code");
            // 
            // txtProductCode
            // 
            this.txtProductCode.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtProductCode.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.RecentlyUsedList;
            this.txtProductCode.BackColor = System.Drawing.SystemColors.Info;
            this.txtProductCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtProductCode.Location = new System.Drawing.Point(225, 77);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(207, 24);
            this.txtProductCode.TabIndex = 0;
            this.toolTip1.SetToolTip(this.txtProductCode, "Add Number\r\nBarcode or Id number\r\n06900001372");
            this.txtProductCode.TextChanged += new System.EventHandler(this.txtProductCode_TextChanged);
            this.txtProductCode.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProductCode_KeyPress);
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(225, 126);
            this.txtProductName.Multiline = true;
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(207, 61);
            this.txtProductName.TabIndex = 1;
            this.toolTip1.SetToolTip(this.txtProductName, "Add item/Product Name");
            // 
            // txtCostPrice
            // 
            this.txtCostPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtCostPrice.Location = new System.Drawing.Point(224, 263);
            this.txtCostPrice.Name = "txtCostPrice";
            this.txtCostPrice.Size = new System.Drawing.Size(208, 24);
            this.txtCostPrice.TabIndex = 3;
            this.toolTip1.SetToolTip(this.txtCostPrice, "Add Cost Price [Number] \r\nExample: 120");
            this.txtCostPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCostPrice_KeyPress);
            // 
            // txtProductQty
            // 
            this.txtProductQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtProductQty.Location = new System.Drawing.Point(225, 215);
            this.txtProductQty.Name = "txtProductQty";
            this.txtProductQty.Size = new System.Drawing.Size(208, 24);
            this.txtProductQty.TabIndex = 2;
            this.toolTip1.SetToolTip(this.txtProductQty, "Add Number of Qty\r\nExample: 10\r\n\r\n::-:: you Can\'t add Letter \r\n       -> only num" +
        "ber can Accept");
            this.txtProductQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtProductQty_KeyPress);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.SystemColors.Control;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Purple;
            this.btnSave.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 12.25F);
            this.btnSave.Location = new System.Drawing.Point(475, 310);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(208, 27);
            this.btnSave.TabIndex = 11;
            this.btnSave.Text = "  Save";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnSave, "Submit to Store");
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtSalesPrice
            // 
            this.txtSalesPrice.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtSalesPrice.Location = new System.Drawing.Point(223, 311);
            this.txtSalesPrice.Name = "txtSalesPrice";
            this.txtSalesPrice.Size = new System.Drawing.Size(210, 24);
            this.txtSalesPrice.TabIndex = 4;
            this.toolTip1.SetToolTip(this.txtSalesPrice, "Add Sales Price\r\nExample: 125");
            this.txtSalesPrice.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSalesPrice_KeyPress);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label7.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label7.Location = new System.Drawing.Point(221, 243);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 16);
            this.label7.TabIndex = 29;
            this.label7.Text = "Cost Price *";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label8.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label8.Location = new System.Drawing.Point(220, 291);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 16);
            this.label8.TabIndex = 28;
            this.label8.Text = "Sale Price *";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label4.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label4.Location = new System.Drawing.Point(472, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 16);
            this.label4.TabIndex = 33;
            this.label4.Text = "Category *";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.ForeColor = System.Drawing.Color.Maroon;
            this.label5.Location = new System.Drawing.Point(472, 204);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 32;
            this.label5.Text = "Supplier *";
            // 
            // toolTip1
            // 
            this.toolTip1.AutomaticDelay = 800;
            this.toolTip1.AutoPopDelay = 32766;
            this.toolTip1.InitialDelay = 10;
            this.toolTip1.ReshowDelay = 10;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            // 
            // ComboCategory
            // 
            this.ComboCategory.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.ComboCategory.FormattingEnabled = true;
            this.ComboCategory.Items.AddRange(new object[] {
            "Beauty, Health & Grocery",
            "Books & Audible",
            "Cigarettes",
            "Computers",
            "Cosmetic",
            "Drinking water products",
            "Electronics",
            "Equipment",
            "Food",
            "Food service equipment",
            "Ice cream‎",
            "Leather",
            "Meat and poultry plant equipment",
            "Medicine",
            "Metal",
            "Milk",
            "Oil",
            "Outlet centers",
            "SoftDrink",
            "Software",
            "Sports & Outdoors",
            "Stationary equipment",
            "Ticket"});
            this.ComboCategory.Location = new System.Drawing.Point(475, 126);
            this.ComboCategory.Name = "ComboCategory";
            this.ComboCategory.Size = new System.Drawing.Size(208, 25);
            this.ComboCategory.Sorted = true;
            this.ComboCategory.TabIndex = 6;
            this.toolTip1.SetToolTip(this.ComboCategory, "Category list");
            // 
            // cmbSupplier
            // 
            this.cmbSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.cmbSupplier.FormattingEnabled = true;
            this.cmbSupplier.Location = new System.Drawing.Point(475, 219);
            this.cmbSupplier.Name = "cmbSupplier";
            this.cmbSupplier.Size = new System.Drawing.Size(208, 25);
            this.cmbSupplier.TabIndex = 8;
            this.toolTip1.SetToolTip(this.cmbSupplier, "Select Supplier name");
            // 
            // txtdiscount
            // 
            this.txtdiscount.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
            this.txtdiscount.Location = new System.Drawing.Point(475, 77);
            this.txtdiscount.Name = "txtdiscount";
            this.txtdiscount.Size = new System.Drawing.Size(136, 24);
            this.txtdiscount.TabIndex = 5;
            this.txtdiscount.Text = "0";
            this.toolTip1.SetToolTip(this.txtdiscount, "Add discount percentage\r\nExample: 5\r\nDon\'t insert %");
            this.txtdiscount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtdiscount_KeyPress);
            // 
            // cmboShopid
            // 
            this.cmboShopid.FormattingEnabled = true;
            this.cmboShopid.Location = new System.Drawing.Point(475, 174);
            this.cmboShopid.Name = "cmboShopid";
            this.cmboShopid.Size = new System.Drawing.Size(208, 21);
            this.cmboShopid.TabIndex = 7;
            this.toolTip1.SetToolTip(this.cmboShopid, "Assign Shop Location/ Branch");
            this.cmboShopid.Visible = false;
            // 
            // chktaxapply
            // 
            this.chktaxapply.AutoSize = true;
            this.chktaxapply.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.chktaxapply.Location = new System.Drawing.Point(631, 79);
            this.chktaxapply.Name = "chktaxapply";
            this.chktaxapply.Size = new System.Drawing.Size(51, 21);
            this.chktaxapply.TabIndex = 72;
            this.chktaxapply.Text = "Yes";
            this.toolTip1.SetToolTip(this.chktaxapply, "IF Tax Apply Checked it\r\nElse UnChecked.\r\n\r\nWhen you Checked, Tax will be apply f" +
        "or this item.\r\nYou can only change tax rate from settings page.");
            this.chktaxapply.UseVisualStyleBackColor = true;
            // 
            // lnkDelete
            // 
            this.lnkDelete.AutoSize = true;
            this.lnkDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.lnkDelete.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkDelete.LinkColor = System.Drawing.Color.Red;
            this.lnkDelete.Location = new System.Drawing.Point(14, 321);
            this.lnkDelete.Name = "lnkDelete";
            this.lnkDelete.Size = new System.Drawing.Size(48, 16);
            this.lnkDelete.TabIndex = 4;
            this.lnkDelete.TabStop = true;
            this.lnkDelete.Text = "Delete";
            this.toolTip1.SetToolTip(this.lnkDelete, "Delete this item from database");
            this.lnkDelete.Visible = false;
            this.lnkDelete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkDelete_LinkClicked);
            // 
            // chkkitchenDisplay
            // 
            this.chkkitchenDisplay.AutoSize = true;
            this.chkkitchenDisplay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.25F);
            this.chkkitchenDisplay.Location = new System.Drawing.Point(631, 256);
            this.chkkitchenDisplay.Name = "chkkitchenDisplay";
            this.chkkitchenDisplay.Size = new System.Drawing.Size(51, 21);
            this.chkkitchenDisplay.TabIndex = 73;
            this.chkkitchenDisplay.Text = "Yes";
            this.toolTip1.SetToolTip(this.chkkitchenDisplay, "Kitchen Display for \r\nRestaurent Item");
            this.chkkitchenDisplay.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label12.ForeColor = System.Drawing.Color.Maroon;
            this.label12.Location = new System.Drawing.Point(473, 261);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 74;
            this.label12.Text = "Kitchen Display";
            this.toolTip1.SetToolTip(this.label12, "Kitchen Display for Restaurent Item");
            // 
            // dtpurchaseDate
            // 
            this.dtpurchaseDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.dtpurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpurchaseDate.Location = new System.Drawing.Point(9, 38);
            this.dtpurchaseDate.Name = "dtpurchaseDate";
            this.dtpurchaseDate.Size = new System.Drawing.Size(130, 21);
            this.dtpurchaseDate.TabIndex = 91;
            this.toolTip1.SetToolTip(this.dtpurchaseDate, "Please Select  Date 2 \r\n \r\nExample:  2014-10-29\r\nyyyy-mm-dd");
            // 
            // txtNewpQty
            // 
            this.txtNewpQty.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.txtNewpQty.Location = new System.Drawing.Point(149, 40);
            this.txtNewpQty.Name = "txtNewpQty";
            this.txtNewpQty.Size = new System.Drawing.Size(150, 20);
            this.txtNewpQty.TabIndex = 93;
            this.toolTip1.SetToolTip(this.txtNewpQty, "Qty will be Auto update to current stock");
            this.txtNewpQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNewpQty_KeyPress);
            // 
            // btnPurchaseHistory
            // 
            this.btnPurchaseHistory.BackColor = System.Drawing.SystemColors.Control;
            this.btnPurchaseHistory.FlatAppearance.BorderColor = System.Drawing.SystemColors.Control;
            this.btnPurchaseHistory.FlatAppearance.BorderSize = 0;
            this.btnPurchaseHistory.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Purple;
            this.btnPurchaseHistory.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.btnPurchaseHistory.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnPurchaseHistory.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.btnPurchaseHistory.Location = new System.Drawing.Point(305, 34);
            this.btnPurchaseHistory.Name = "btnPurchaseHistory";
            this.btnPurchaseHistory.Size = new System.Drawing.Size(127, 30);
            this.btnPurchaseHistory.TabIndex = 95;
            this.btnPurchaseHistory.Text = "Purchase History";
            this.btnPurchaseHistory.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.toolTip1.SetToolTip(this.btnPurchaseHistory, "Submit to Purchase History");
            this.btnPurchaseHistory.UseVisualStyleBackColor = false;
            this.btnPurchaseHistory.Click += new System.EventHandler(this.btnPurchaseHistory_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(30, 241);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(173, 23);
            this.btnBrowse.TabIndex = 10;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // picItemimage
            // 
            this.picItemimage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.picItemimage.Image = ((System.Drawing.Image)(resources.GetObject("picItemimage.Image")));
            this.picItemimage.InitialImage = ((System.Drawing.Image)(resources.GetObject("picItemimage.InitialImage")));
            this.picItemimage.Location = new System.Drawing.Point(30, 57);
            this.picItemimage.Name = "picItemimage";
            this.picItemimage.Size = new System.Drawing.Size(173, 158);
            this.picItemimage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picItemimage.TabIndex = 37;
            this.picItemimage.TabStop = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // lblimagename
            // 
            this.lblimagename.AutoSize = true;
            this.lblimagename.Font = new System.Drawing.Font("Microsoft Sans Serif", 3.25F);
            this.lblimagename.Location = new System.Drawing.Point(34, 268);
            this.lblimagename.Name = "lblimagename";
            this.lblimagename.Size = new System.Drawing.Size(5, 6);
            this.lblimagename.TabIndex = 63;
            this.lblimagename.Text = "-";
            // 
            // lblFileExtension
            // 
            this.lblFileExtension.AutoSize = true;
            this.lblFileExtension.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F);
            this.lblFileExtension.Location = new System.Drawing.Point(167, 268);
            this.lblFileExtension.Name = "lblFileExtension";
            this.lblFileExtension.Size = new System.Drawing.Size(32, 7);
            this.lblFileExtension.TabIndex = 62;
            this.lblFileExtension.Text = "item.png";
            // 
            // lblItemcode
            // 
            this.lblItemcode.AutoSize = true;
            this.lblItemcode.Font = new System.Drawing.Font("Microsoft Sans Serif", 5.25F);
            this.lblItemcode.Location = new System.Drawing.Point(32, 45);
            this.lblItemcode.Name = "lblItemcode";
            this.lblItemcode.Size = new System.Drawing.Size(6, 7);
            this.lblItemcode.TabIndex = 64;
            this.lblItemcode.Text = "-";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(30, 218);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(170, 14);
            this.label6.TabIndex = 65;
            this.label6.Text = "Standard Size: 128 * 128 | .png";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.label9.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label9.Location = new System.Drawing.Point(472, 57);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 16);
            this.label9.TabIndex = 68;
            this.label9.Text = "Discount %";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(473, 156);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(117, 13);
            this.label10.TabIndex = 70;
            this.label10.Text = "Assign Shop Location *";
            this.label10.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.label11.ForeColor = System.Drawing.Color.DarkSlateBlue;
            this.label11.Location = new System.Drawing.Point(628, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(83, 26);
            this.label11.TabIndex = 71;
            this.label11.Text = "TPS/TVQ\r\nVAT/TAX/ Apply ";
            // 
            // lblValidmsg
            // 
            this.lblValidmsg.AutoSize = true;
            this.lblValidmsg.ForeColor = System.Drawing.Color.Red;
            this.lblValidmsg.Location = new System.Drawing.Point(248, 9);
            this.lblValidmsg.Name = "lblValidmsg";
            this.lblValidmsg.Size = new System.Drawing.Size(0, 13);
            this.lblValidmsg.TabIndex = 73;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpboxPurchasehistory);
            this.splitContainer1.Panel1.Controls.Add(this.label12);
            this.splitContainer1.Panel1.Controls.Add(this.chkkitchenDisplay);
            this.splitContainer1.Panel1.Controls.Add(this.picItemimage);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.chktaxapply);
            this.splitContainer1.Panel1.Controls.Add(this.txtProductCode);
            this.splitContainer1.Panel1.Controls.Add(this.label11);
            this.splitContainer1.Panel1.Controls.Add(this.txtProductName);
            this.splitContainer1.Panel1.Controls.Add(this.label10);
            this.splitContainer1.Panel1.Controls.Add(this.txtProductQty);
            this.splitContainer1.Panel1.Controls.Add(this.cmboShopid);
            this.splitContainer1.Panel1.Controls.Add(this.txtCostPrice);
            this.splitContainer1.Panel1.Controls.Add(this.label9);
            this.splitContainer1.Panel1.Controls.Add(this.btnSave);
            this.splitContainer1.Panel1.Controls.Add(this.txtdiscount);
            this.splitContainer1.Panel1.Controls.Add(this.txtSalesPrice);
            this.splitContainer1.Panel1.Controls.Add(this.label8);
            this.splitContainer1.Panel1.Controls.Add(this.label6);
            this.splitContainer1.Panel1.Controls.Add(this.label7);
            this.splitContainer1.Panel1.Controls.Add(this.lblItemcode);
            this.splitContainer1.Panel1.Controls.Add(this.label5);
            this.splitContainer1.Panel1.Controls.Add(this.lblimagename);
            this.splitContainer1.Panel1.Controls.Add(this.label4);
            this.splitContainer1.Panel1.Controls.Add(this.lblFileExtension);
            this.splitContainer1.Panel1.Controls.Add(this.btnBrowse);
            this.splitContainer1.Panel1.Controls.Add(this.ComboCategory);
            this.splitContainer1.Panel1.Controls.Add(this.cmbSupplier);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lnkSupplier);
            this.splitContainer1.Panel2.Controls.Add(this.lnkDelete);
            this.splitContainer1.Panel2.Controls.Add(this.lnkcategories);
            this.splitContainer1.Panel2.Controls.Add(this.lnkbulkitems);
            this.splitContainer1.Panel2.Controls.Add(this.lnkStocklist);
            this.splitContainer1.Size = new System.Drawing.Size(917, 477);
            this.splitContainer1.SplitterDistance = 730;
            this.splitContainer1.TabIndex = 76;
            // 
            // grpboxPurchasehistory
            // 
            this.grpboxPurchasehistory.Controls.Add(this.label15);
            this.grpboxPurchasehistory.Controls.Add(this.btnPurchaseHistory);
            this.grpboxPurchasehistory.Controls.Add(this.label14);
            this.grpboxPurchasehistory.Controls.Add(this.label13);
            this.grpboxPurchasehistory.Controls.Add(this.txtNewpQty);
            this.grpboxPurchasehistory.Controls.Add(this.dtpurchaseDate);
            this.grpboxPurchasehistory.Location = new System.Drawing.Point(223, 359);
            this.grpboxPurchasehistory.Name = "grpboxPurchasehistory";
            this.grpboxPurchasehistory.Size = new System.Drawing.Size(460, 95);
            this.grpboxPurchasehistory.TabIndex = 76;
            this.grpboxPurchasehistory.TabStop = false;
            this.grpboxPurchasehistory.Text = "Purchase History";
            this.grpboxPurchasehistory.Visible = false;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label15.Location = new System.Drawing.Point(6, 70);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(192, 12);
            this.label15.TabIndex = 96;
            this.label15.Text = "* Qty will be Auto update to your current stock";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.label14.Location = new System.Drawing.Point(148, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(151, 13);
            this.label14.TabIndex = 94;
            this.label14.Text = "New Purchase Quantity (Qty) *";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 22);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(103, 13);
            this.label13.TabIndex = 92;
            this.label13.Text = "New Purchase Date";
            // 
            // lnkSupplier
            // 
            this.lnkSupplier.AutoSize = true;
            this.lnkSupplier.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.lnkSupplier.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkSupplier.Location = new System.Drawing.Point(14, 40);
            this.lnkSupplier.Name = "lnkSupplier";
            this.lnkSupplier.Size = new System.Drawing.Size(65, 16);
            this.lnkSupplier.TabIndex = 5;
            this.lnkSupplier.TabStop = true;
            this.lnkSupplier.Text = "Suppliers";
            this.lnkSupplier.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSupplier_LinkClicked);
            // 
            // lnkcategories
            // 
            this.lnkcategories.AutoSize = true;
            this.lnkcategories.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.lnkcategories.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkcategories.Location = new System.Drawing.Point(14, 18);
            this.lnkcategories.Name = "lnkcategories";
            this.lnkcategories.Size = new System.Drawing.Size(74, 16);
            this.lnkcategories.TabIndex = 3;
            this.lnkcategories.TabStop = true;
            this.lnkcategories.Text = "Categories";
            this.lnkcategories.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkcategories_LinkClicked);
            // 
            // lnkbulkitems
            // 
            this.lnkbulkitems.AutoSize = true;
            this.lnkbulkitems.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.lnkbulkitems.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkbulkitems.Location = new System.Drawing.Point(14, 63);
            this.lnkbulkitems.Name = "lnkbulkitems";
            this.lnkbulkitems.Size = new System.Drawing.Size(109, 16);
            this.lnkbulkitems.TabIndex = 1;
            this.lnkbulkitems.TabStop = true;
            this.lnkbulkitems.Text = "Bulk items import";
            this.lnkbulkitems.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkbulkitems_LinkClicked);
            // 
            // lnkStocklist
            // 
            this.lnkStocklist.AutoSize = true;
            this.lnkStocklist.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F);
            this.lnkStocklist.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.lnkStocklist.Location = new System.Drawing.Point(14, 85);
            this.lnkStocklist.Name = "lnkStocklist";
            this.lnkStocklist.Size = new System.Drawing.Size(65, 16);
            this.lnkStocklist.TabIndex = 0;
            this.lnkStocklist.TabStop = true;
            this.lnkStocklist.Text = "Stock List";
            this.lnkStocklist.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkStocklist_LinkClicked);
            // 
            // Add_Item
            // 
            this.AcceptButton = this.btnSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(917, 477);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.lblValidmsg);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Add_Item";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add New item";
            this.Load += new System.EventHandler(this.Add_Item_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picItemimage)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpboxPurchasehistory.ResumeLayout(false);
            this.grpboxPurchasehistory.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.TextBox txtCostPrice;
        private System.Windows.Forms.TextBox txtProductQty;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtSalesPrice;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox ComboCategory;
        private System.Windows.Forms.ComboBox cmbSupplier;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.PictureBox picItemimage;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblimagename;
        private System.Windows.Forms.Label lblFileExtension;
        private System.Windows.Forms.Label lblItemcode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtdiscount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmboShopid;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chktaxapply;
        private System.Windows.Forms.Label lblValidmsg;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.LinkLabel lnkcategories;
        private System.Windows.Forms.LinkLabel lnkbulkitems;
        private System.Windows.Forms.LinkLabel lnkStocklist;
        private System.Windows.Forms.LinkLabel lnkDelete;
        private System.Windows.Forms.LinkLabel lnkSupplier;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox chkkitchenDisplay;
        private System.Windows.Forms.DateTimePicker dtpurchaseDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtNewpQty;
        private System.Windows.Forms.GroupBox grpboxPurchasehistory;
        private System.Windows.Forms.Button btnPurchaseHistory;
        private System.Windows.Forms.Label label15;
    }
}