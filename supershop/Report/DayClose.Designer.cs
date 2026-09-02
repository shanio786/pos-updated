namespace supershop.Report
{
    partial class DayClose
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dtDate = new System.Windows.Forms.DateTimePicker();
            this.lblShop = new System.Windows.Forms.Label();
            this.lblOpeningCap = new System.Windows.Forms.Label();
            this.txtOpening = new System.Windows.Forms.TextBox();
            this.btnCalculate = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.colLabel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblExpectedCap = new System.Windows.Forms.Label();
            this.lblExpected = new System.Windows.Forms.Label();
            this.lblCountedCap = new System.Windows.Forms.Label();
            this.txtCounted = new System.Windows.Forms.TextBox();
            this.lblDiffCap = new System.Windows.Forms.Label();
            this.lblDifference = new System.Windows.Forms.Label();
            this.lblNoteCap = new System.Windows.Forms.Label();
            this.txtNote = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.SuspendLayout();
            // dtDate
            this.dtDate.Location = new System.Drawing.Point(20, 20);
            this.dtDate.Name = "dtDate";
            this.dtDate.Size = new System.Drawing.Size(160, 23);
            this.dtDate.TabIndex = 0;
            this.dtDate.ValueChanged += new System.EventHandler(this.dtDate_ValueChanged);
            // lblShop
            this.lblShop.AutoSize = true;
            this.lblShop.Location = new System.Drawing.Point(200, 24);
            this.lblShop.Name = "lblShop";
            this.lblShop.Size = new System.Drawing.Size(40, 15);
            this.lblShop.TabIndex = 1;
            this.lblShop.Text = "Shop:";
            // lblOpeningCap
            this.lblOpeningCap.AutoSize = true;
            this.lblOpeningCap.Location = new System.Drawing.Point(380, 24);
            this.lblOpeningCap.Name = "lblOpeningCap";
            this.lblOpeningCap.Size = new System.Drawing.Size(80, 15);
            this.lblOpeningCap.TabIndex = 2;
            this.lblOpeningCap.Text = "Opening cash";
            // txtOpening
            this.txtOpening.Location = new System.Drawing.Point(470, 20);
            this.txtOpening.Name = "txtOpening";
            this.txtOpening.Size = new System.Drawing.Size(100, 23);
            this.txtOpening.TabIndex = 3;
            this.txtOpening.TextChanged += new System.EventHandler(this.txtOpening_TextChanged);
            // btnCalculate
            this.btnCalculate.Location = new System.Drawing.Point(590, 18);
            this.btnCalculate.Name = "btnCalculate";
            this.btnCalculate.Size = new System.Drawing.Size(100, 27);
            this.btnCalculate.TabIndex = 4;
            this.btnCalculate.Text = "Recalculate";
            this.btnCalculate.UseVisualStyleBackColor = true;
            this.btnCalculate.Click += new System.EventHandler(this.btnCalculate_Click);
            // grid
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLabel, this.colValue, this.colAmount});
            this.grid.Location = new System.Drawing.Point(20, 60);
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.Size = new System.Drawing.Size(670, 300);
            this.grid.TabIndex = 5;
            // colLabel
            this.colLabel.HeaderText = "Item";
            this.colLabel.Name = "colLabel";
            this.colLabel.Width = 320;
            // colValue
            this.colValue.HeaderText = "Detail";
            this.colValue.Name = "colValue";
            this.colValue.Width = 160;
            // colAmount
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.Width = 150;
            // lblExpectedCap
            this.lblExpectedCap.AutoSize = true;
            this.lblExpectedCap.Location = new System.Drawing.Point(20, 375);
            this.lblExpectedCap.Name = "lblExpectedCap";
            this.lblExpectedCap.Size = new System.Drawing.Size(130, 15);
            this.lblExpectedCap.TabIndex = 6;
            this.lblExpectedCap.Text = "Expected cash:";
            // lblExpected
            this.lblExpected.AutoSize = true;
            this.lblExpected.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblExpected.Location = new System.Drawing.Point(155, 373);
            this.lblExpected.Name = "lblExpected";
            this.lblExpected.Size = new System.Drawing.Size(30, 20);
            this.lblExpected.TabIndex = 7;
            this.lblExpected.Text = "0.00";
            // lblCountedCap
            this.lblCountedCap.AutoSize = true;
            this.lblCountedCap.Location = new System.Drawing.Point(20, 410);
            this.lblCountedCap.Name = "lblCountedCap";
            this.lblCountedCap.Size = new System.Drawing.Size(120, 15);
            this.lblCountedCap.TabIndex = 8;
            this.lblCountedCap.Text = "Counted cash:";
            // txtCounted
            this.txtCounted.Location = new System.Drawing.Point(155, 407);
            this.txtCounted.Name = "txtCounted";
            this.txtCounted.Size = new System.Drawing.Size(120, 23);
            this.txtCounted.TabIndex = 9;
            this.txtCounted.TextChanged += new System.EventHandler(this.txtCounted_TextChanged);
            // lblDiffCap
            this.lblDiffCap.AutoSize = true;
            this.lblDiffCap.Location = new System.Drawing.Point(300, 410);
            this.lblDiffCap.Name = "lblDiffCap";
            this.lblDiffCap.Size = new System.Drawing.Size(70, 15);
            this.lblDiffCap.TabIndex = 10;
            this.lblDiffCap.Text = "Difference:";
            // lblDifference
            this.lblDifference.AutoSize = true;
            this.lblDifference.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblDifference.Location = new System.Drawing.Point(375, 408);
            this.lblDifference.Name = "lblDifference";
            this.lblDifference.Size = new System.Drawing.Size(30, 20);
            this.lblDifference.TabIndex = 11;
            this.lblDifference.Text = "0.00";
            // lblNoteCap
            this.lblNoteCap.AutoSize = true;
            this.lblNoteCap.Location = new System.Drawing.Point(20, 445);
            this.lblNoteCap.Name = "lblNoteCap";
            this.lblNoteCap.Size = new System.Drawing.Size(40, 15);
            this.lblNoteCap.TabIndex = 12;
            this.lblNoteCap.Text = "Note:";
            // txtNote
            this.txtNote.Location = new System.Drawing.Point(155, 442);
            this.txtNote.Name = "txtNote";
            this.txtNote.Size = new System.Drawing.Size(415, 23);
            this.txtNote.TabIndex = 13;
            // btnSave
            this.btnSave.Location = new System.Drawing.Point(470, 405);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 28);
            this.btnSave.TabIndex = 14;
            this.btnSave.Text = "Save && Close";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // btnPrint
            this.btnPrint.Location = new System.Drawing.Point(590, 405);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 28);
            this.btnPrint.TabIndex = 15;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // printDocument1
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // DayClose
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(710, 485);
            this.Controls.Add(this.dtDate);
            this.Controls.Add(this.lblShop);
            this.Controls.Add(this.lblOpeningCap);
            this.Controls.Add(this.txtOpening);
            this.Controls.Add(this.btnCalculate);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.lblExpectedCap);
            this.Controls.Add(this.lblExpected);
            this.Controls.Add(this.lblCountedCap);
            this.Controls.Add(this.txtCounted);
            this.Controls.Add(this.lblDiffCap);
            this.Controls.Add(this.lblDifference);
            this.Controls.Add(this.lblNoteCap);
            this.Controls.Add(this.txtNote);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnPrint);
            this.Name = "DayClose";
            this.Text = "Day Close / Z-Report";
            this.Load += new System.EventHandler(this.DayClose_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.DateTimePicker dtDate;
        private System.Windows.Forms.Label lblShop;
        private System.Windows.Forms.Label lblOpeningCap;
        private System.Windows.Forms.TextBox txtOpening;
        private System.Windows.Forms.Button btnCalculate;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLabel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
        private System.Windows.Forms.Label lblExpectedCap;
        private System.Windows.Forms.Label lblExpected;
        private System.Windows.Forms.Label lblCountedCap;
        private System.Windows.Forms.TextBox txtCounted;
        private System.Windows.Forms.Label lblDiffCap;
        private System.Windows.Forms.Label lblDifference;
        private System.Windows.Forms.Label lblNoteCap;
        private System.Windows.Forms.TextBox txtNote;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnPrint;
        private System.Drawing.Printing.PrintDocument printDocument1;
    }
}
