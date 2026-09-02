namespace supershop.SalesRagister
{
    partial class SplitPaymentForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblPayableCap = new System.Windows.Forms.Label();
            this.lblPayable = new System.Windows.Forms.Label();
            this.lblCashCap = new System.Windows.Forms.Label();
            this.txtCash = new System.Windows.Forms.TextBox();
            this.lblCardCap = new System.Windows.Forms.Label();
            this.txtCard = new System.Windows.Forms.TextBox();
            this.lblMobileCap = new System.Windows.Forms.Label();
            this.txtMobile = new System.Windows.Forms.TextBox();
            this.lblPaidCap = new System.Windows.Forms.Label();
            this.lblPaid = new System.Windows.Forms.Label();
            this.lblRemainingCap = new System.Windows.Forms.Label();
            this.lblRemaining = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // captions and fields
            this.lblPayableCap.AutoSize = true; this.lblPayableCap.Location = new System.Drawing.Point(20, 20);
            this.lblPayableCap.Text = "Payable:"; this.lblPayableCap.Name = "lblPayableCap";
            this.lblPayable.AutoSize = true; this.lblPayable.Location = new System.Drawing.Point(140, 20);
            this.lblPayable.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblPayable.Text = "0.00"; this.lblPayable.Name = "lblPayable";
            this.lblCashCap.AutoSize = true; this.lblCashCap.Location = new System.Drawing.Point(20, 60);
            this.lblCashCap.Text = "Cash:"; this.lblCashCap.Name = "lblCashCap";
            this.txtCash.Location = new System.Drawing.Point(140, 57); this.txtCash.Size = new System.Drawing.Size(120, 23);
            this.txtCash.Name = "txtCash"; this.txtCash.TextChanged += new System.EventHandler(this.amount_TextChanged);
            this.lblCardCap.AutoSize = true; this.lblCardCap.Location = new System.Drawing.Point(20, 95);
            this.lblCardCap.Text = "Card:"; this.lblCardCap.Name = "lblCardCap";
            this.txtCard.Location = new System.Drawing.Point(140, 92); this.txtCard.Size = new System.Drawing.Size(120, 23);
            this.txtCard.Name = "txtCard"; this.txtCard.TextChanged += new System.EventHandler(this.amount_TextChanged);
            this.lblMobileCap.AutoSize = true; this.lblMobileCap.Location = new System.Drawing.Point(20, 130);
            this.lblMobileCap.Text = "Mobile / Wallet:"; this.lblMobileCap.Name = "lblMobileCap";
            this.txtMobile.Location = new System.Drawing.Point(140, 127); this.txtMobile.Size = new System.Drawing.Size(120, 23);
            this.txtMobile.Name = "txtMobile"; this.txtMobile.TextChanged += new System.EventHandler(this.amount_TextChanged);
            this.lblPaidCap.AutoSize = true; this.lblPaidCap.Location = new System.Drawing.Point(20, 170);
            this.lblPaidCap.Text = "Paid:"; this.lblPaidCap.Name = "lblPaidCap";
            this.lblPaid.AutoSize = true; this.lblPaid.Location = new System.Drawing.Point(140, 170);
            this.lblPaid.Text = "0.00"; this.lblPaid.Name = "lblPaid";
            this.lblRemainingCap.AutoSize = true; this.lblRemainingCap.Location = new System.Drawing.Point(20, 195);
            this.lblRemainingCap.Text = "Remaining:"; this.lblRemainingCap.Name = "lblRemainingCap";
            this.lblRemaining.AutoSize = true; this.lblRemaining.Location = new System.Drawing.Point(140, 195);
            this.lblRemaining.Text = "0.00"; this.lblRemaining.Name = "lblRemaining";
            // buttons
            this.btnOK.Location = new System.Drawing.Point(20, 230); this.btnOK.Size = new System.Drawing.Size(110, 34);
            this.btnOK.Text = "OK"; this.btnOK.Name = "btnOK"; this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            this.btnCancel.Location = new System.Drawing.Point(150, 230); this.btnCancel.Size = new System.Drawing.Size(110, 34);
            this.btnCancel.Text = "Cancel"; this.btnCancel.Name = "btnCancel"; this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 285);
            this.Controls.Add(this.lblPayableCap); this.Controls.Add(this.lblPayable);
            this.Controls.Add(this.lblCashCap); this.Controls.Add(this.txtCash);
            this.Controls.Add(this.lblCardCap); this.Controls.Add(this.txtCard);
            this.Controls.Add(this.lblMobileCap); this.Controls.Add(this.txtMobile);
            this.Controls.Add(this.lblPaidCap); this.Controls.Add(this.lblPaid);
            this.Controls.Add(this.lblRemainingCap); this.Controls.Add(this.lblRemaining);
            this.Controls.Add(this.btnOK); this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Name = "SplitPaymentForm"; this.Text = "Split Payment";
            this.Load += new System.EventHandler(this.SplitPaymentForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblPayableCap, lblPayable, lblCashCap, lblCardCap, lblMobileCap;
        private System.Windows.Forms.Label lblPaidCap, lblPaid, lblRemainingCap, lblRemaining;
        private System.Windows.Forms.TextBox txtCash, txtCard, txtMobile;
        private System.Windows.Forms.Button btnOK, btnCancel;
    }
}
