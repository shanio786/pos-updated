namespace supershop.AdminPanelForms
{
    partial class GenerateLicenseForm
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
            this.LicensetextBox1 = new System.Windows.Forms.TextBox();
            this.LicensetextBox2 = new System.Windows.Forms.TextBox();
            this.LicensetextBox3 = new System.Windows.Forms.TextBox();
            this.LicensetextBox4 = new System.Windows.Forms.TextBox();
            this.LicensetextBox5 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.tbluserlicenseBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.db_licenseDataSet = new supershop.db_licenseDataSet();
            this.FromdateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.TodateTimePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbl_userlicenseTableAdapter = new supershop.db_licenseDataSetTableAdapters.tbl_userlicenseTableAdapter();
            this.tbluserlicenseBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbluserlicenseBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.db_licenseDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbluserlicenseBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // LicensetextBox1
            // 
            this.LicensetextBox1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LicensetextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicensetextBox1.Location = new System.Drawing.Point(11, 58);
            this.LicensetextBox1.MaxLength = 5;
            this.LicensetextBox1.Multiline = true;
            this.LicensetextBox1.Name = "LicensetextBox1";
            this.LicensetextBox1.Size = new System.Drawing.Size(104, 26);
            this.LicensetextBox1.TabIndex = 0;
            this.LicensetextBox1.TextChanged += new System.EventHandler(this.LicensetextBox1_TextChanged);
            // 
            // LicensetextBox2
            // 
            this.LicensetextBox2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LicensetextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicensetextBox2.Location = new System.Drawing.Point(121, 58);
            this.LicensetextBox2.MaxLength = 5;
            this.LicensetextBox2.Multiline = true;
            this.LicensetextBox2.Name = "LicensetextBox2";
            this.LicensetextBox2.Size = new System.Drawing.Size(104, 26);
            this.LicensetextBox2.TabIndex = 1;
            this.LicensetextBox2.TextChanged += new System.EventHandler(this.LicensetextBox2_TextChanged);
            // 
            // LicensetextBox3
            // 
            this.LicensetextBox3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LicensetextBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicensetextBox3.Location = new System.Drawing.Point(231, 58);
            this.LicensetextBox3.MaxLength = 5;
            this.LicensetextBox3.Multiline = true;
            this.LicensetextBox3.Name = "LicensetextBox3";
            this.LicensetextBox3.Size = new System.Drawing.Size(104, 26);
            this.LicensetextBox3.TabIndex = 2;
            this.LicensetextBox3.TextChanged += new System.EventHandler(this.LicensetextBox3_TextChanged);
            // 
            // LicensetextBox4
            // 
            this.LicensetextBox4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LicensetextBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicensetextBox4.Location = new System.Drawing.Point(341, 58);
            this.LicensetextBox4.MaxLength = 5;
            this.LicensetextBox4.Multiline = true;
            this.LicensetextBox4.Name = "LicensetextBox4";
            this.LicensetextBox4.Size = new System.Drawing.Size(104, 26);
            this.LicensetextBox4.TabIndex = 3;
            this.LicensetextBox4.TextChanged += new System.EventHandler(this.LicensetextBox4_TextChanged);
            // 
            // LicensetextBox5
            // 
            this.LicensetextBox5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LicensetextBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LicensetextBox5.Location = new System.Drawing.Point(452, 58);
            this.LicensetextBox5.MaxLength = 5;
            this.LicensetextBox5.Multiline = true;
            this.LicensetextBox5.Name = "LicensetextBox5";
            this.LicensetextBox5.Size = new System.Drawing.Size(104, 26);
            this.LicensetextBox5.TabIndex = 4;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(452, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 33);
            this.button1.TabIndex = 5;
            this.button1.Text = "Generate";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(188, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "License Key Generator";
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.Color.DarkRed;
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.ForeColor = System.Drawing.SystemColors.Control;
            this.button3.Location = new System.Drawing.Point(12, 147);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(104, 33);
            this.button3.TabIndex = 8;
            this.button3.Text = "Clear";
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(13, 186);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(543, 268);
            this.dataGridView1.TabIndex = 10;
            // 
            // tbluserlicenseBindingSource
            // 
            this.tbluserlicenseBindingSource.DataMember = "tbl_userlicense";
            this.tbluserlicenseBindingSource.DataSource = this.db_licenseDataSet;
            // 
            // db_licenseDataSet
            // 
            this.db_licenseDataSet.DataSetName = "db_licenseDataSet";
            this.db_licenseDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // FromdateTimePicker
            // 
            this.FromdateTimePicker.Location = new System.Drawing.Point(12, 120);
            this.FromdateTimePicker.Name = "FromdateTimePicker";
            this.FromdateTimePicker.Size = new System.Drawing.Size(214, 20);
            this.FromdateTimePicker.TabIndex = 11;
            // 
            // TodateTimePicker
            // 
            this.TodateTimePicker.Location = new System.Drawing.Point(342, 120);
            this.TodateTimePicker.Name = "TodateTimePicker";
            this.TodateTimePicker.Size = new System.Drawing.Size(214, 20);
            this.TodateTimePicker.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 104);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 15);
            this.label2.TabIndex = 13;
            this.label2.Text = "From Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(339, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "To Date";
            // 
            // tbl_userlicenseTableAdapter
            // 
            this.tbl_userlicenseTableAdapter.ClearBeforeFill = true;
            // 
            // tbluserlicenseBindingSource1
            // 
            this.tbluserlicenseBindingSource1.DataMember = "tbl_userlicense";
            this.tbluserlicenseBindingSource1.DataSource = this.db_licenseDataSet;
            // 
            // GenerateLicenseForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Bisque;
            this.ClientSize = new System.Drawing.Size(568, 466);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TodateTimePicker);
            this.Controls.Add(this.FromdateTimePicker);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.LicensetextBox5);
            this.Controls.Add(this.LicensetextBox4);
            this.Controls.Add(this.LicensetextBox3);
            this.Controls.Add(this.LicensetextBox2);
            this.Controls.Add(this.LicensetextBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "GenerateLicenseForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GenerateLicenseForm";
            this.Load += new System.EventHandler(this.GenerateLicenseForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbluserlicenseBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.db_licenseDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbluserlicenseBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox LicensetextBox1;
        private System.Windows.Forms.TextBox LicensetextBox2;
        private System.Windows.Forms.TextBox LicensetextBox3;
        private System.Windows.Forms.TextBox LicensetextBox4;
        private System.Windows.Forms.TextBox LicensetextBox5;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DateTimePicker FromdateTimePicker;
        private System.Windows.Forms.DateTimePicker TodateTimePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private db_licenseDataSet db_licenseDataSet;
        private System.Windows.Forms.BindingSource tbluserlicenseBindingSource;
        private db_licenseDataSetTableAdapters.tbl_userlicenseTableAdapter tbl_userlicenseTableAdapter;
        private System.Windows.Forms.BindingSource tbluserlicenseBindingSource1;
    }
}