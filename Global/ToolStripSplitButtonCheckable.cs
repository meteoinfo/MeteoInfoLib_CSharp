using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MeteoInfoC.Global
{
    /// <summary>
    /// Checkable ToolStripSplitButton
    /// </summary>
    public class ToolStripSplitButtonCheckable:ToolStripSplitButton
    {
        private bool _checked;
        private static ProfessionalColorTable _professionalColorTable;

        /// <summary>
        /// Get or set checked
        /// </summary>
        public bool Checked
        {
            get
            {
                return _checked;
            }
            set
            {
                _checked = value;
                Invalidate();
            }
        }


        private void RenderCheckedButtonFill(Graphics g, Rectangle bounds)
        {
            if ((bounds.Width == 0) || (bounds.Height == 0))
            {
                return;
            }

            if (!UseSystemColors)
            {
                using (Brush b = new LinearGradientBrush(bounds, ColorTable.ButtonCheckedGradientBegin, ColorTable.ButtonCheckedGradientEnd, LinearGradientMode.Vertical))
                {
                    g.FillRectangle(b, bounds);
                }
            }
            else
            {
                Color fillColor = ColorTable.ButtonCheckedHighlight;

                using (Brush b = new SolidBrush(fillColor))
                {
                    g.FillRectangle(b, bounds);
                }
            }
        }

        private bool UseSystemColors
        {
            get { return (ColorTable.UseSystemColors || !ToolStripManager.VisualStylesEnabled); }
        }


        private static ProfessionalColorTable ColorTable
        {
            get
            {
                if (_professionalColorTable == null)
                {
                    _professionalColorTable = new ProfessionalColorTable();
                }
                return _professionalColorTable;
            }
        }

        /// <summary>
        /// Override OnPaint method
        /// </summary>
        /// <param name="e">PaintEventArgs</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (_checked)
            {
                Graphics g = e.Graphics;
                Rectangle bounds = new Rectangle(Point.Empty, Size);

                RenderCheckedButtonFill(g, bounds);

                using (Pen p = new Pen(ColorTable.ButtonSelectedBorder))
                {
                    g.DrawRectangle(p, bounds.X, bounds.Y, bounds.Width - 1, bounds.Height - 1);
                }
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// Synchronize to default
        /// </summary>
        public void SyncToDefault()
        {
            Action<ToolStripSplitButton, ToolStripItem> synchanlder = (splitButton, item) =>
            {
                splitButton.DefaultItem = item;
                splitButton.Text = item.Text;
                splitButton.ToolTipText = item.ToolTipText;
                splitButton.Image = item.Image;
            };

            synchanlder(this, this.DropDownItems[0]);
            this.DropDownItemClicked += (sender, args) => synchanlder((ToolStripSplitButton)sender, args.ClickedItem);

        }

    }
}
