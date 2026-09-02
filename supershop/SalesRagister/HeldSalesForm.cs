using System;
using System.Windows.Forms;

namespace supershop.SalesRagister
{
    /// <summary>Picker dialog for held sales. Set SelectedHoldId then check DialogResult.</summary>
    public partial class HeldSalesForm : Form
    {
        public long SelectedHoldId { get; private set; }

        public HeldSalesForm()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) { this.DialogResult = DialogResult.Cancel; this.Close(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void HeldSalesForm_Load(object sender, EventArgs e) { LoadList(); }

        void LoadList()
        {
            try { grid.DataSource = HeldSaleStore.List(); }
            catch (Exception ex) { Logger.Show(ex, "Could not load held sales."); }
        }

        long CurrentId()
        {
            if (grid.CurrentRow == null || grid.CurrentRow.Cells["Hold"].Value == null) return 0;
            return Convert.ToInt64(grid.CurrentRow.Cells["Hold"].Value);
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            long id = CurrentId();
            if (id == 0) { MessageBox.Show("Select a held sale."); return; }
            SelectedHoldId = id;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            long id = CurrentId();
            if (id == 0) return;
            if (MessageBox.Show("Delete this held sale?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;
            try { HeldSaleStore.Delete(id); LoadList(); }
            catch (Exception ex) { Logger.Show(ex, "Could not delete the held sale."); }
        }

        private void grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e) { btnResume_Click(sender, e); }
        private void btnCancel_Click(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
    }
}
