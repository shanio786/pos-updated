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
