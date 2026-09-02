using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace supershop.SalesRagister
{
    /// <summary>
    /// Collects a split payment (Cash + Card + Mobile) for one sale.
    /// Open with the payable amount; on OK read Tenders and TotalPaid.
    /// </summary>
    public partial class SplitPaymentForm : Form
    {
        readonly decimal _payable;

        public List<Tender> Tenders { get; private set; }
        public decimal TotalPaid { get; private set; }
        public decimal ChangeAmount { get; private set; }

        public SplitPaymentForm(decimal payable)
        {
            InitializeComponent();
            _payable = payable;
            Tenders = new List<Tender>();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) { this.DialogResult = DialogResult.Cancel; this.Close(); }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SplitPaymentForm_Load(object sender, EventArgs e)
        {
            lblPayable.Text = _payable.ToString("0.00");
            Recalc();
        }

        static decimal P(string s) { decimal d; return decimal.TryParse(s, out d) ? d : 0m; }

        void Recalc()
        {
            decimal paid = P(txtCash.Text) + P(txtCard.Text) + P(txtMobile.Text);
            decimal remaining = _payable - paid;
            lblPaid.Text = paid.ToString("0.00");
            lblRemaining.Text = remaining.ToString("0.00");
        }

        private void amount_TextChanged(object sender, EventArgs e) { Recalc(); }

        private void btnOK_Click(object sender, EventArgs e)
        {
            decimal cash = P(txtCash.Text), card = P(txtCard.Text), mobile = P(txtMobile.Text);
            decimal paid = cash + card + mobile;
            if (paid < _payable)
            {
                MessageBox.Show("The amounts entered are less than the payable amount.", "Split payment",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Tenders = new List<Tender>();
            if (cash > 0)   Tenders.Add(new Tender("Cash", cash));
            if (card > 0)   Tenders.Add(new Tender("Card", card));
            if (mobile > 0) Tenders.Add(new Tender("Mobile", mobile));
            TotalPaid = paid;
            ChangeAmount = paid - _payable;           // change is given from cash
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e) { this.DialogResult = DialogResult.Cancel; this.Close(); }
    }
}
