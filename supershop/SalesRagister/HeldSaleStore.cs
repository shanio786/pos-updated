using System;
using System.Data;
using System.Windows.Forms;

namespace supershop.SalesRagister
{
    /// <summary>
    /// Park (hold) a cart and bring it back later, so a busy counter can serve
    /// the next customer without losing the current basket.
    /// The cart grid is the one used by the sales screens
    /// (columns: 0 Name, 1 Price, 2 Qty, 3 Total, 4 Code, 5 DisAmt, 6 TaxAmt,
    ///           7 DisRate, 8 TaxApply, 9 KitchenDisplay).
    /// </summary>
    public static class HeldSaleStore
    {
        /// <summary>Small text-input dialog (replaces VisualBasic InputBox).</summary>
        public static string Prompt(string text, string title, string def)
        {
            using (Form f = new Form())
            using (Label lbl = new Label())
            using (TextBox box = new TextBox())
            using (Button ok = new Button())
            using (Button cancel = new Button())
            {
                f.Text = title;
                f.FormBorderStyle = FormBorderStyle.FixedDialog;
                f.StartPosition = FormStartPosition.CenterParent;
                f.MinimizeBox = false; f.MaximizeBox = false;
                f.ClientSize = new System.Drawing.Size(360, 120);
                lbl.SetBounds(12, 12, 336, 20); lbl.Text = text;
                box.SetBounds(12, 40, 336, 25); box.Text = def == null ? "" : def;
                ok.SetBounds(150, 78, 90, 30); ok.Text = "OK"; ok.DialogResult = DialogResult.OK;
                cancel.SetBounds(258, 78, 90, 30); cancel.Text = "Cancel"; cancel.DialogResult = DialogResult.Cancel;
                f.Controls.Add(lbl); f.Controls.Add(box); f.Controls.Add(ok); f.Controls.Add(cancel);
                f.AcceptButton = ok; f.CancelButton = cancel;
                return f.ShowDialog() == DialogResult.OK ? box.Text : null;
            }
        }

        /// <summary>Saves the cart as a held sale and returns its hold id.</summary>
        public static long Hold(string label, string custId, DataGridView cart)
        {
            if (cart == null || cart.Rows.Count == 0)
                throw new Exception("The cart is empty.");

            long holdId = 0;
            DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
            {
                object idObj = tx.Scalar(
                    "INSERT INTO tbl_held_sale (label, Shopid, emp_id, cust_id) OUTPUT INSERTED.hold_id " +
                    "VALUES (@label, @shop, @emp, @cust)",
                    DataAccess.P("@label", string.IsNullOrEmpty(label) ? DateTime.Now.ToString("HH:mm") : label),
                    DataAccess.P("@shop", UserInfo.Shopid),
                    DataAccess.P("@emp", UserInfo.UserName),
                    DataAccess.P("@cust", custId));
                long hid = Convert.ToInt64(idObj);

                foreach (DataGridViewRow r in cart.Rows)
                {
                    if (r.IsNewRow || r.Cells[0].Value == null) continue;
                    tx.Execute(
                        "INSERT INTO tbl_held_item (hold_id, itemcode, itemName, Qty, RetailsPrice, Total, disamt, taxamt, disrate, taxapply, kitchendisplay) " +
                        "VALUES (@h, @code, @name, @qty, @price, @total, @disamt, @taxamt, @disrate, @taxapply, @kd)",
                        DataAccess.P("@h", hid),
                        DataAccess.P("@code", Cell(r, 4)),
                        DataAccess.P("@name", Cell(r, 0)),
                        DataAccess.P("@qty", Cell(r, 2)),
                        DataAccess.P("@price", Cell(r, 1)),
                        DataAccess.P("@total", Cell(r, 3)),
                        DataAccess.P("@disamt", Cell(r, 5)),
                        DataAccess.P("@taxamt", Cell(r, 6)),
                        DataAccess.P("@disrate", Cell(r, 7)),
                        DataAccess.P("@taxapply", Cell(r, 8)),
                        DataAccess.P("@kd", Cell(r, 9)));
                }
                holdId = hid;
            });
            return holdId;
        }

        static string Cell(DataGridViewRow r, int i)
        {
            if (i >= r.Cells.Count || r.Cells[i].Value == null) return "";
            return r.Cells[i].Value.ToString();
        }

        /// <summary>List of held sales for the current shop (hold_id, label, created, items, total).</summary>
        public static DataTable List()
        {
            return DataAccess.GetDataTable(
                "SELECT h.hold_id AS [Hold], h.label AS [Name], h.created_at AS [Time], " +
                "       (SELECT COUNT(*) FROM tbl_held_item i WHERE i.hold_id = h.hold_id) AS [Items], " +
                "       (SELECT SUM(ISNULL(i.Total,0)) FROM tbl_held_item i WHERE i.hold_id = h.hold_id) AS [Total] " +
                "FROM tbl_held_sale h WHERE ISNULL(h.Shopid,'') = @shop ORDER BY h.created_at DESC",
                DataAccess.P("@shop", UserInfo.Shopid));
        }

        /// <summary>Rows of a held sale, ready to load back into the cart grid.</summary>
        public static DataTable Items(long holdId)
        {
            return DataAccess.GetDataTable(
                "SELECT itemName, RetailsPrice, Qty, Total, itemcode, disamt, taxamt, disrate, taxapply, kitchendisplay " +
                "FROM tbl_held_item WHERE hold_id = @h ORDER BY id",
                DataAccess.P("@h", holdId));
        }

        public static string CustId(long holdId)
        {
            return DataAccess.ExecuteSQLScaler("SELECT cust_id FROM tbl_held_sale WHERE hold_id = @h",
                DataAccess.P("@h", holdId));
        }

        /// <summary>Removes a held sale (after it is resumed or cancelled).</summary>
        public static void Delete(long holdId)
        {
            DataAccess.RunInTransaction(delegate(DataAccess.DbTransaction tx)
            {
                tx.Execute("DELETE FROM tbl_held_item WHERE hold_id = @h", DataAccess.P("@h", holdId));
                tx.Execute("DELETE FROM tbl_held_sale WHERE hold_id = @h", DataAccess.P("@h", holdId));
            });
        }
    }
}
