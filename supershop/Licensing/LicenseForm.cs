using System;
using System.Windows.Forms;

namespace supershop.Licensing
{
    /// <summary>Activation screen: shows this PC's Machine ID and accepts a license key.</summary>
    public partial class LicenseForm : Form
    {
        public bool Activated { get; private set; }

        public LicenseForm()
        {
            InitializeComponent();
        }

        private void LicenseForm_Load(object sender, EventArgs e)
        {
            txtMachineId.Text = LicenseManager.GetMachineId();
            lblStatus.Text = "This copy is not activated. Send the Machine ID to your supplier to get a license key.";
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            try { Clipboard.SetText(txtMachineId.Text); lblStatus.Text = "Machine ID copied."; }
            catch (Exception ex) { Logger.Error("copy machine id", ex); }
        }

        private void btnActivate_Click(object sender, EventArgs e)
        {
            string reason;
            if (LicenseManager.Activate(txtKey.Text, out reason))
            {
                Activated = true;
                MessageBox.Show("Activated. Thank you!", "License", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblStatus.ForeColor = System.Drawing.Color.Firebrick;
                lblStatus.Text = reason;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
