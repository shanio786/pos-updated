using System;
using System.Data;
using System.Windows.Forms;

namespace supershop.BarCode
{
    /// <summary>
    /// Pick a product, set how many labels, and print them fast (Code 128,
    /// drawn directly). Also generates a barcode for a product that has none.
    /// </summary>
    public partial class newBarcode_Generator : Form
    {
        public newBarcode_Generator()
        {
            InitializeComponent();
        }

        private void newBarcode_Generator_Load(object sender, EventArgs e)
        {
            try
            {
                DataTable dt5 = DataAccess.GetDataTable("select product_id from purchase order by product_id");
                cmbitems.DataSource = dt5;
                cmbitems.DisplayMember = "product_id";
            }
            catch (Exception ex) { Logger.Error("barcode load", ex); }
        }

        LabelPrinter.Label Current(out int qty)
        {
            qty = 1;
            DataTable dt1 = DataAccess.GetDataTable(
                "select product_name, product_id, retail_price from purchase where product_id = @id",
                DataAccess.P("@id", cmbitems.Text));
            if (dt1.Rows.Count == 0) { MessageBox.Show("Item not found."); return null; }
            int q;
            if (!int.TryParse(txtQuantity.Text, out q) || q < 1) q = 1;
            qty = q;
            string name = dt1.Rows[0]["product_name"].ToString();
            string code = dt1.Rows[0]["product_id"].ToString();
            string price = dt1.Rows[0]["retail_price"].ToString();
            return new LabelPrinter.Label(name, code, price);
        }

        // Search/Preview
        private void bntSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int qty; LabelPrinter.Label lab = Current(out qty);
                if (lab != null) LabelPrinter.Print(lab, qty, true);   // preview
            }
            catch (Exception ex) { Logger.Show(ex, "Could not preview the labels."); }
        }
    }
}
