namespace supershop.Report
{
    partial class Dashboard
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        System.Windows.Forms.Panel MakeTile(string caption, string name, System.Drawing.Color accent, int x, int y)
        {
            var t = new System.Windows.Forms.Panel();
            t.BackColor = System.Drawing.Color.White;
            t.Location = new System.Drawing.Point(x, y);
            t.Size = new System.Drawing.Size(210, 92);
            t.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            var bar = new System.Windows.Forms.Panel();
            bar.BackColor = accent; bar.Location = new System.Drawing.Point(0, 0); bar.Size = new System.Drawing.Size(6, 92);
            var cap = new System.Windows.Forms.Label();
            cap.Text = caption; cap.ForeColor = System.Drawing.Color.Gray;
            cap.Font = new System.Drawing.Font("Segoe UI", 9F);
            cap.Location = new System.Drawing.Point(18, 12); cap.AutoSize = true;
            var val = new System.Windows.Forms.Label();
            val.Name = name + "Val"; val.Text = "0";
            val.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold);
            val.ForeColor = System.Drawing.Color.FromArgb(30, 45, 60);
            val.Location = new System.Drawing.Point(16, 38); val.AutoSize = true;
            t.Controls.Add(val); t.Controls.Add(cap); t.Controls.Add(bar);
            t.Name = name;
            return t;
        }

        private void InitializeComponent()
        {
            this.pnlHead = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblShop = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.lblLowNote = new System.Windows.Forms.Label();
            this.lblTopCap = new System.Windows.Forms.Label();
            this.gridTop = new System.Windows.Forms.DataGridView();
            this.pnlHead.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTop)).BeginInit();
            this.SuspendLayout();
            // header
            this.pnlHead.BackColor = System.Drawing.Color.FromArgb(30, 45, 60);
            this.pnlHead.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHead.Height = 58;
            this.pnlHead.Controls.Add(this.lblTitle);
            this.pnlHead.Controls.Add(this.lblShop);
            this.pnlHead.Controls.Add(this.btnRefresh);
            this.pnlHead.Name = "pnlHead";
            this.lblTitle.Text = "Dashboard"; this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 15F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(14, 8); this.lblTitle.AutoSize = true;
            this.lblShop.Text = "Shop"; this.lblShop.ForeColor = System.Drawing.Color.Gainsboro;
            this.lblShop.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblShop.Location = new System.Drawing.Point(16, 38); this.lblShop.AutoSize = true;
            this.btnRefresh.Text = "Refresh"; this.btnRefresh.BackColor = System.Drawing.Color.White;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.Location = new System.Drawing.Point(560, 14); this.btnRefresh.Size = new System.Drawing.Size(96, 30);
            this.btnRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // tiles
            this.tileSales = MakeTile("Today's Sales", "tileSales", System.Drawing.Color.FromArgb(14,143,110), 16, 74);
            this.tileCash  = MakeTile("Today's Cash", "tileCash", System.Drawing.Color.FromArgb(43,110,163), 238, 74);
            this.tileTxns  = MakeTile("Transactions", "tileTxns", System.Drawing.Color.FromArgb(120,90,180), 460, 74);
            this.tileDue   = MakeTile("Today's Due", "tileDue", System.Drawing.Color.FromArgb(183,121,31), 16, 176);
            this.tileMonth = MakeTile("This Month", "tileMonth", System.Drawing.Color.FromArgb(14,143,110), 238, 176);
            this.tileLow   = MakeTile("Low Stock", "tileLow", System.Drawing.Color.FromArgb(192,73,47), 460, 176);
            // low note
            this.lblLowNote.Text = "Click to see items to reorder";
            this.lblLowNote.ForeColor = System.Drawing.Color.FromArgb(192,73,47);
            this.lblLowNote.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Underline);
            this.lblLowNote.Location = new System.Drawing.Point(466, 270); this.lblLowNote.AutoSize = true;
            this.lblLowNote.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblLowNote.Click += new System.EventHandler(this.lblLowNote_Click);
            // top items
            this.lblTopCap.Text = "Top selling today"; this.lblTopCap.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold);
            this.lblTopCap.Location = new System.Drawing.Point(16, 300); this.lblTopCap.AutoSize = true;
            this.gridTop.Location = new System.Drawing.Point(16, 324); this.gridTop.Size = new System.Drawing.Size(640, 170);
            this.gridTop.AllowUserToAddRows = false; this.gridTop.ReadOnly = true; this.gridTop.RowHeadersVisible = false;
            this.gridTop.BackgroundColor = System.Drawing.Color.White; this.gridTop.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.gridTop.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridTop.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Bottom;
            this.gridTop.Name = "gridTop";
            // form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 510);
            this.BackColor = System.Drawing.Color.FromArgb(244, 247, 250);
            this.Controls.Add(this.gridTop);
            this.Controls.Add(this.lblTopCap);
            this.Controls.Add(this.lblLowNote);
            this.Controls.Add(this.tileSales);
            this.Controls.Add(this.tileCash);
            this.Controls.Add(this.tileTxns);
            this.Controls.Add(this.tileDue);
            this.Controls.Add(this.tileMonth);
            this.Controls.Add(this.tileLow);
            this.Controls.Add(this.pnlHead);
            this.Name = "Dashboard";
            this.Text = "Dashboard";
            this.Load += new System.EventHandler(this.Dashboard_Load);
            this.pnlHead.ResumeLayout(false);
            this.pnlHead.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridTop)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlHead;
        private System.Windows.Forms.Label lblTitle, lblShop, lblLowNote, lblTopCap;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Panel tileSales, tileCash, tileTxns, tileDue, tileMonth, tileLow;
        private System.Windows.Forms.DataGridView gridTop;
    }
}
