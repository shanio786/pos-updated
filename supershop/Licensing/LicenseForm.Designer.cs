namespace supershop.Licensing
{
    partial class LicenseForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlHead = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSub = new System.Windows.Forms.Label();
            this.lblMidCap = new System.Windows.Forms.Label();
            this.txtMachineId = new System.Windows.Forms.TextBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.lblKeyCap = new System.Windows.Forms.Label();
            this.txtKey = new System.Windows.Forms.TextBox();
            this.btnActivate = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlHead.SuspendLayout();
            this.SuspendLayout();
            // pnlHead
            this.pnlHead.BackColor = System.Drawing.Color.FromArgb(45, 62, 80);
            this.pnlHead.Controls.Add(this.lblTitle);
            this.pnlHead.Controls.Add(this.lblSub);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Height = 70;
            this.pnlHead.Name = "pnlHead";
            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(18, 12);
            this.lblTitle.Text = "Activate Adv POS";
            this.lblTitle.Name = "lblTitle";
            // lblSub
            this.lblSub.AutoSize = true;
            this.lblSub.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblSub.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSub.Location = new System.Drawing.Point(20, 44);
            this.lblSub.Text = "One-time activation for this computer";
            this.lblSub.Name = "lblSub";
            // lblMidCap
            this.lblMidCap.AutoSize = true;
            this.lblMidCap.Location = new System.Drawing.Point(20, 92);
            this.lblMidCap.Text = "Your Machine ID:";
            this.lblMidCap.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblMidCap.Name = "lblMidCap";
            // txtMachineId
            this.txtMachineId.Location = new System.Drawing.Point(20, 114);
            this.txtMachineId.Size = new System.Drawing.Size(340, 27);
            this.txtMachineId.ReadOnly = true;
            this.txtMachineId.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.txtMachineId.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtMachineId.Name = "txtMachineId";
            // btnCopy
            this.btnCopy.Location = new System.Drawing.Point(368, 114);
            this.btnCopy.Size = new System.Drawing.Size(94, 28);
            this.btnCopy.Text = "Copy";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            this.btnCopy.Name = "btnCopy";
            // lblKeyCap
            this.lblKeyCap.AutoSize = true;
            this.lblKeyCap.Location = new System.Drawing.Point(20, 156);
            this.lblKeyCap.Text = "Paste your License Key:";
            this.lblKeyCap.Font = new System.Drawing.Font("Segoe UI Semibold", 9.5F, System.Drawing.FontStyle.Bold);
            this.lblKeyCap.Name = "lblKeyCap";
            // txtKey
            this.txtKey.Location = new System.Drawing.Point(20, 178);
            this.txtKey.Size = new System.Drawing.Size(442, 92);
            this.txtKey.Multiline = true;
            this.txtKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtKey.Font = new System.Drawing.Font("Consolas", 8.5F);
            this.txtKey.Name = "txtKey";
            // btnActivate
            this.btnActivate.Location = new System.Drawing.Point(20, 282);
            this.btnActivate.Size = new System.Drawing.Size(200, 40);
            this.btnActivate.Text = "Activate";
            this.btnActivate.BackColor = System.Drawing.Color.SeaGreen;
            this.btnActivate.ForeColor = System.Drawing.Color.White;
            this.btnActivate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnActivate.Font = new System.Drawing.Font("Segoe UI Semibold", 11F, System.Drawing.FontStyle.Bold);
            this.btnActivate.UseVisualStyleBackColor = false;
            this.btnActivate.Click += new System.EventHandler(this.btnActivate_Click);
            this.btnActivate.Name = "btnActivate";
            // btnExit
            this.btnExit.Location = new System.Drawing.Point(362, 282);
            this.btnExit.Size = new System.Drawing.Size(100, 40);
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            this.btnExit.Name = "btnExit";
            // lblStatus
            this.lblStatus.Location = new System.Drawing.Point(20, 332);
            this.lblStatus.Size = new System.Drawing.Size(442, 44);
            this.lblStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblStatus.Name = "lblStatus";
            // LicenseForm
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 386);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnActivate);
            this.Controls.Add(this.txtKey);
            this.Controls.Add(this.lblKeyCap);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.txtMachineId);
            this.Controls.Add(this.lblMidCap);
            this.Controls.Add(this.pnlHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Activate Adv POS";
            this.Load += new System.EventHandler(this.LicenseForm_Load);
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label lblTitle, lblSub, lblMidCap, lblKeyCap, lblStatus;
        private System.Windows.Forms.TextBox txtMachineId, txtKey;
        private System.Windows.Forms.Button btnCopy, btnActivate, btnExit;
    }
}
