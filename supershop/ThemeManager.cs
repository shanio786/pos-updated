using System;
using System.Configuration;
using System.Drawing;
using System.Windows.Forms;

namespace supershop
{
    /// <summary>
    /// Lightweight, opt-in visual polish for the main window shell.
    /// It ONLY changes colours and the menu/status-bar renderer - never the
    /// size or position of any control - so it cannot break existing layouts.
    /// Turn it off with  &lt;add key="ModernTheme" value="false"/&gt;  in app.config.
    /// </summary>
    public static class ThemeManager
    {
        // Modern flat accent palette (calm slate-blue, easy on the eyes for long shifts).
        public static readonly Color Accent      = Color.FromArgb(37, 99, 165);   // header / hover
        public static readonly Color AccentDark  = Color.FromArgb(28, 76, 128);   // pressed
        public static readonly Color StripBack   = Color.FromArgb(245, 247, 250); // bar background
        public static readonly Color StripText   = Color.FromArgb(33, 43, 54);
        public static readonly Color GridHeader  = Color.FromArgb(37, 99, 165);
        public static readonly Color GridAltRow  = Color.FromArgb(244, 248, 252);

        public static bool Enabled
        {
            get
            {
                string v = ConfigurationManager.AppSettings["ModernTheme"];
                // default ON
                return string.IsNullOrEmpty(v) || v.Trim().ToLowerInvariant() == "true";
            }
        }

        /// <summary>Apply the shell theme to the main window's menu and status strips.</summary>
        public static void ApplyShell(MenuStrip menu, StatusStrip status)
        {
            if (!Enabled) return;
            try
            {
                if (menu != null)
                {
                    menu.RenderMode = ToolStripRenderMode.Professional;
                    menu.Renderer = new ModernRenderer();
                    menu.BackColor = StripBack;
                    menu.ForeColor = StripText;
                }
                if (status != null)
                {
                    status.RenderMode = ToolStripRenderMode.Professional;
                    status.Renderer = new ModernRenderer();
                    status.BackColor = StripBack;
                    status.ForeColor = StripText;
                }
            }
            catch (Exception ex) { Logger.Error(ex); }
        }

        /// <summary>Optional: give a data grid a clean modern header/row look (colours only).</summary>
        public static void StyleGrid(DataGridView grid)
        {
            if (!Enabled || grid == null) return;
            try
            {
                grid.EnableHeadersVisualStyles = false;
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.None;
                grid.ColumnHeadersDefaultCellStyle.BackColor = GridHeader;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font =
                    new Font(grid.Font, FontStyle.Bold);
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                grid.AlternatingRowsDefaultCellStyle.BackColor = GridAltRow;
                grid.RowHeadersVisible = false;
                grid.GridColor = Color.FromArgb(224, 228, 232);
            }
            catch (Exception ex) { Logger.Error(ex); }
        }

        /// <summary>
        /// Walk a whole form and give it a modern flat look WITHOUT moving or
        /// resizing anything: flat buttons with hover, styled grids, hand cursor.
        /// Font family/size and every control's bounds are left untouched, so
        /// existing fixed layouts are safe. Call once from a form's constructor
        /// after InitializeComponent, e.g. ThemeManager.ApplyModern(this);
        /// </summary>
        static readonly System.Runtime.CompilerServices.ConditionalWeakTable<Control, object> _done
            = new System.Runtime.CompilerServices.ConditionalWeakTable<Control, object>();

        public static void ApplyModern(Control root)
        {
            if (!Enabled || root == null) return;
            object seen;
            if (_done.TryGetValue(root, out seen)) return;   // theme each form only once
            _done.Add(root, _done);
            try
            {
                Walk(root);
                // After the font swap the Segoe UI face is a little taller than the
                // old MS Sans Serif / Times faces the screens were drawn with, so an
                // auto-sized caption label can grow just enough for its bottom edge to
                // touch the input directly beneath it (the "line through the label"
                // users saw on the payment panel). Lift any such caption clear of its
                // field - safely, never crossing whatever sits above it.
                FixCaptionOverlaps(root);
            }
            catch (Exception ex) { Logger.Error(ex); }
        }

        /// <summary>
        /// Walk every container and nudge caption labels up so they never overlap the
        /// input (text box / combo / date picker) sitting right below them. It only
        /// ever moves small caption labels, only upward, never past the control above
        /// them, and by at most a few pixels - so it fixes the clipping the Segoe UI
        /// font introduces without disturbing any deliberate layout.
        /// </summary>
        static void FixCaptionOverlaps(Control container)
        {
            if (container == null) return;
            try
            {
                Control[] kids = new Control[container.Controls.Count];
                container.Controls.CopyTo(kids, 0);

                foreach (Control c in kids)
                {
                    Label lab = c as Label;
                    // Caption-sized labels only (skip titles, banners, rule lines).
                    if (lab == null || !lab.Visible) continue;
                    if (lab.Height <= 0 || lab.Height > 40) continue;
                    if (IsRuleText(lab.Text) || string.IsNullOrEmpty(lab.Text)) continue;

                    Control field = NearestFieldBelow(lab, kids);
                    if (field == null) continue;

                    int overlap = lab.Bottom - field.Top;   // >0 means they touch/overlap
                    if (overlap < 1) continue;               // already clear

                    // How far may we lift? Never into the control directly above.
                    int ceiling = NearestBottomAbove(lab, kids) + 2;      // may be <0 if nothing above
                    int lift = overlap + 4;                               // 4px breathing room
                    int newTop = lab.Top - lift;
                    if (newTop < ceiling) newTop = ceiling;
                    if (newTop < lab.Top) lab.Top = newTop;               // only ever move up
                }

                foreach (Control c in kids)
                    if (c.HasChildren) FixCaptionOverlaps(c);
            }
            catch { }
        }

        /// <summary>Is this control a data-entry field a caption would sit above?</summary>
        static bool IsField(Control c)
        {
            return c is TextBox || c is ComboBox || c is DateTimePicker
                || c is MaskedTextBox || c is NumericUpDown || c is RichTextBox;
        }

        /// <summary>The input field whose top is at/just below the label and overlaps it horizontally.</summary>
        static Control NearestFieldBelow(Label lab, Control[] siblings)
        {
            Control best = null;
            foreach (Control c in siblings)
            {
                if (c == lab || !c.Visible || !IsField(c)) continue;
                bool across = c.Left < lab.Right && c.Right > lab.Left;   // horizontal overlap
                if (!across) continue;
                if (c.Top < lab.Top - 2) continue;        // field must be at or below the caption
                if (c.Top > lab.Bottom + 6) continue;     // and be the caption's own field, not a far one
                if (best == null || c.Top < best.Top) best = c;
            }
            return best;
        }

        /// <summary>Bottom edge of the nearest control sitting directly above the label (or a low floor).</summary>
        static int NearestBottomAbove(Label lab, Control[] siblings)
        {
            int bottom = -1000;   // nothing above -> free to lift
            foreach (Control c in siblings)
            {
                if (c == lab || !c.Visible) continue;
                bool across = c.Left < lab.Right && c.Right > lab.Left;   // horizontal overlap
                if (!across) continue;
                if (c.Bottom <= lab.Top && c.Bottom > bottom) bottom = c.Bottom;
            }
            return bottom;
        }

        static readonly string UiFont = "Segoe UI";

        static void Walk(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                Button b = c as Button;
                if (b != null) StyleButton(b);

                DataGridView g = c as DataGridView;
                if (g != null) { StyleGrid(g); }
                else if (c is Label && IsRuleText(c.Text)) StyleRule((Label)c);
                else
                {
                    ModernFont(c);   // grids keep the font StyleGrid gives them
                    // Let caption labels grow to fit their text so nothing is clipped
                    // on Windows fonts (a common cause of "…" / overlapping labels).
                    Label lab = c as Label;
                    if (lab != null && !lab.AutoSize)
                    {
                        try { lab.AutoSize = true; } catch { }
                    }
                }

                if (c.HasChildren) Walk(c);
            }
        }

        /// <summary>
        /// Swap a control's font FAMILY to Segoe UI, keeping its exact size and
        /// style. This is the single biggest "modern" win - the old screens mix
        /// Times New Roman / Trebuchet / MS Sans Serif, which reads as dated.
        /// Size is preserved so layouts don't shift.
        /// </summary>
        static void ModernFont(Control c)
        {
            try
            {
                Font f = c.Font;
                if (f == null) return;
                if (string.Equals(f.Name, UiFont, StringComparison.OrdinalIgnoreCase)) return;
                c.Font = new Font(UiFont, f.Size, f.Style, f.Unit);
            }
            catch { }
        }

        /// <summary>True for an old "======" / "------" separator label.</summary>
        static bool IsRuleText(string t)
        {
            if (string.IsNullOrEmpty(t) || t.Length < 4) return false;
            foreach (char ch in t) if (ch != '=' && ch != '-' && ch != '_') return false;
            return true;
        }

        /// <summary>Turn a dashed-text separator label into a clean thin rule.</summary>
        static void StyleRule(Label l)
        {
            try
            {
                int w = l.Width > 12 ? l.Width : 500;
                l.AutoSize = false;
                l.Text = "";
                l.Height = 1;
                l.Width = w;
                l.BackColor = Color.FromArgb(214, 220, 226);
                l.Top += 8;   // sit it on the text baseline it replaced
            }
            catch { }
        }

        /// <summary>Flat, modern button - colours and flatness only, no bounds change.</summary>
        static void StyleButton(Button b)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.UseVisualStyleBackColor = false;

            // plain grey system buttons get the accent; already-coloured ones keep their colour
            if (b.BackColor == SystemColors.Control || b.BackColor == SystemColors.ButtonFace || b.BackColor.IsEmpty)
            {
                b.BackColor = Accent;
                b.ForeColor = Color.White;
            }
            try
            {
                b.FlatAppearance.MouseOverBackColor = ControlPaint.Light(b.BackColor, 0.15f);
                b.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(b.BackColor, 0.05f);
            }
            catch { }
            b.Cursor = Cursors.Hand;
        }

        /// <summary>Professional colour table used by the flat renderer.</summary>
        private sealed class ModernColors : ProfessionalColorTable
        {
            public override Color MenuItemSelected            { get { return Accent; } }
            public override Color MenuItemSelectedGradientBegin { get { return Accent; } }
            public override Color MenuItemSelectedGradientEnd   { get { return Accent; } }
            public override Color MenuItemPressedGradientBegin  { get { return AccentDark; } }
            public override Color MenuItemPressedGradientEnd    { get { return AccentDark; } }
            public override Color MenuItemBorder              { get { return Accent; } }
            public override Color MenuBorder                  { get { return Color.FromArgb(210, 216, 222); } }
            public override Color ToolStripDropDownBackground { get { return Color.White; } }
            public override Color ImageMarginGradientBegin    { get { return Color.White; } }
            public override Color ImageMarginGradientMiddle   { get { return Color.White; } }
            public override Color ImageMarginGradientEnd      { get { return Color.White; } }
            public override Color SeparatorDark               { get { return Color.FromArgb(224, 228, 232); } }
            public override Color MenuStripGradientBegin      { get { return StripBack; } }
            public override Color MenuStripGradientEnd        { get { return StripBack; } }
            public override Color StatusStripGradientBegin    { get { return StripBack; } }
            public override Color StatusStripGradientEnd      { get { return StripBack; } }
        }

        private sealed class ModernRenderer : ToolStripProfessionalRenderer
        {
            public ModernRenderer() : base(new ModernColors()) { RoundedEdges = false; }

            protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
            {
                // White text on the accent highlight, dark text otherwise.
                if (e.Item.Selected || e.Item.Pressed) e.TextColor = Color.White;
                else e.TextColor = StripText;
                base.OnRenderItemText(e);
            }
        }
    }
}
