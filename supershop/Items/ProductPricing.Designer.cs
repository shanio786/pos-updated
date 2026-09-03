namespace supershop.Items
{
    partial class ProductPricing
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
            this.lblProdCap = new System.Windows.Forms.Label();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblName = new System.Windows.Forms.Label();
            this.lblRetail = new System.Windows.Forms.Label();
            this.lblWCap = new System.Windows.Forms.Label();
            this.txtWholesale = new System.Windows.Forms.TextBox();
            this.lblDCap = new System.Windows.Forms.Label();
            this.txtFlatDisc = new System.Windows.Forms.TextBox();
            this.lblHint = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlHead.SuspendLayout();
            this.SuspendLayout();
            this.pnlHead.BackColor = System.Drawing.Color.FromArgb(30, 45, 60);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Height = 50;
            this.pnlHead.Controls.Add(this.lblTitle);
            this.pnlHead.Name = "pnlHead";
            this.lblTitle.Text = "Product Pricing"; this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 13F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(14, 10); this.lblTitle.AutoSize = true;
            this.lblProdCap.Text = "Product code"; this.lblProdCap.Location = new System.Drawing.Point(20, 70); this.lblProdCap.AutoSize = true;
            this.cmbProduct.Location = new System.Drawing.Point(140, 66); this.cmbProduct.Size = new System.Drawing.Size(230, 25);
            this.cmbProduct.Name = "cmbProduct"; this.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            this.btnLoad.Text = "Load"; this.btnLoad.Location = new System.Drawing.Point(378, 65); this.btnLoad.Size = new System.Drawing.Size(70, 27);
            this.btnLoad.Click += new System.EventHandler(this.btnLoad_Click);
            this.lblName.Text = ""; this.lblName.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(140, 100); this.lblName.AutoSize = true;
            this.lblRetail.Text = ""; this.lblRetail.ForeColor = System.Drawing.Color.Gray;
            this.lblRetail.Location = new System.Drawing.Point(140, 124); this.lblRetail.AutoSize = true;
            this.lblWCap.Text = "Wholesale price (Rs)"; this.lblWCap.Location = new System.Drawing.Point(20, 160); this.lblWCap.AutoSize = true;
            this.txtWholesale.Location = new System.Drawing.Point(220, 157); this.txtWholesale.Size = new System.Drawing.Size(150, 25); this.txtWholesale.Name = "txtWholesale";
            this.lblDCap.Text = "Flat discount per unit (Rs)"; this.lblDCap.Location = new System.Drawing.Point(20, 196); this.lblDCap.AutoSize = true;
            this.txtFlatDisc.Location = new System.Drawing.Point(220, 193); this.txtFlatDisc.Size = new System.Drawing.Size(150, 25); this.txtFlatDisc.Name = "txtFlatDisc";
            this.lblHint.Text = "Flat discount is applied per unit when the item is sold (0 = use the % discount)."; 
            this.lblHint.ForeColor = System.Drawing.Color.Gray; this.lblHint.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblHint.Location = new System.Drawing.Point(20, 226); this.lblHint.AutoSize = true;
            this.btnSave.Text = "Save"; this.btnSave.BackColor = System.Drawing.Color.SeaGreen; this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat; this.btnSave.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.btnSave.Location = new System.Drawing.Point(220, 258); this.btnSave.Size = new System.Drawing.Size(150, 36);
            this.btnSave.UseVisualStyleBackColor = false; this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(474, 320);
            this.Controls.Add(this.btnSave); this.Controls.Add(this.lblHint);
            this.Controls.Add(this.txtFlatDisc); this.Controls.Add(this.lblDCap);
            this.Controls.Add(this.txtWholesale); this.Controls.Add(this.lblWCap);
            this.Controls.Add(this.lblRetail); this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnLoad); this.Controls.Add(this.cmbProduct);
            this.Controls.Add(this.lblProdCap); this.Controls.Add(this.pnlHead);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false; this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "ProductPricing"; this.Text = "Product Pricing";
            this.Load += new System.EventHandler(this.ProductPricing_Load);
            this.pnlHead.ResumeLayout(false); this.pnlHead.PerformLayout();
            this.ResumeLayout(false); this.PerformLayout();
        }

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label lblTitle, lblProdCap, lblName, lblRetail, lblWCap, lblDCap, lblHint;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.Button btnLoad, btnSave;
        private System.Windows.Forms.TextBox txtWholesale, txtFlatDisc;
    }
}
