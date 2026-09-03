using System;
using System.Data;
using System.Windows.Forms;

namespace supershop.Items
{
    /// <summary>
    /// Set a product's optional wholesale (2nd) price and a flat (Rs) discount
    /// per unit. Kept separate from Add Item so it stays simple.
    /// </summary>
    public partial class ProductPricing : Form
    {
        public ProductPricing()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape) this.Close();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void ProductPricing_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = DataAccess.GetDataTable("select product_id from purchase order by product_id");
                cmbProduct.DataSource = dt;
                cmbProduct.DisplayMember = "product_id";
                cmbProduct.SelectedIndex = -1;
            }
            catch (Exception ex) { Logger.Show(ex, "Could not load products."); }
        }

        private void btnLoad_Click(object sender, EventArgs e) { LoadProduct(); }
        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e) { LoadProduct(); }

        void LoadProduct()
        {
            try
            {
                if (string.IsNullOrEmpty(cmbProduct.Text)) return;
                DataTable dt = DataAccess.GetDataTable(
                    "select product_name, retail_price, ISNULL(wholesale_price,0) AS wholesale_price, ISNULL(disc_amount,0) AS disc_amount " +
                    "from purchase where product_id = @id", DataAccess.P("@id", cmbProduct.Text));
                if (dt.Rows.Count == 0) { lblName.Text = "Not found"; return; }
                lblName.Text = dt.Rows[0]["product_name"].ToString();
                lblRetail.Text = "Retail: " + Convert.ToDecimal(dt.Rows[0]["retail_price"]).ToString("0.00");
                txtWholesale.Text = Convert.ToDecimal(dt.Rows[0]["wholesale_price"]).ToString("0.00");
                txtFlatDisc.Text = Convert.ToDecimal(dt.Rows[0]["disc_amount"]).ToString("0.00");
            }
            catch (Exception ex) { Logger.Show(ex, "Could not load the product."); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(cmbProduct.Text)) { MessageBox.Show("Choose a product."); return; }
                decimal w, d;
                if (!decimal.TryParse(txtWholesale.Text, out w) || w < 0) w = 0;
                if (!decimal.TryParse(txtFlatDisc.Text, out d) || d < 0) d = 0;
                DataAccess.ExecuteSQL(
                    "update purchase set wholesale_price = @w, disc_amount = @d where product_id = @id",
                    DataAccess.P("@w", w), DataAccess.P("@d", d), DataAccess.P("@id", cmbProduct.Text));
                MessageBox.Show("Saved.", "Product pricing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { Logger.Show(ex, "Could not save the pricing."); }
        }
    }
}
