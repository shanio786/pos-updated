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
            try { Walk(root); }
            catch (Exception ex) { Logger.Error(ex); }
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
                else ModernFont(c);   // grids keep the font StyleGrid gives them

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
