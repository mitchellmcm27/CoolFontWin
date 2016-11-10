using System;
using System.Drawing;
using System.Windows.Forms;

/* https://github.com/RobinPerris/DarkUI */

namespace CFW.Business
{
    public class CustomContextMenuRenderer : ToolStripRenderer
    {
        #region Initialisation Region

        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);

            toolStrip.BackColor = Colors.GreyBackground;
            toolStrip.ForeColor = Colors.LightText;
            toolStrip.ShowItemToolTips = true;
        }

        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);

            item.ForeColor = Colors.LightText;

            if (item.GetType() == typeof(ToolStripSeparator))
            {
                item.Margin = new Padding(0, 0, 0, 1);
            }
        }

        #endregion

        #region Render Region

        /** Change background color here */
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            var g = e.Graphics;
            using (var b = new SolidBrush(Colors.DarkBlueBackground))
            {
                g.FillRectangle(b, e.AffectedBounds);
            }
        }

        /** Change border color here */
        protected override void OnRenderImageMargin(ToolStripRenderEventArgs e)
        {
            var g = e.Graphics;

            var rect = new Rectangle(0, 0, e.ToolStrip.Width - 1, e.ToolStrip.Height - 1);

            using (var p = new Pen(Color.Transparent)) // Flat look
            {
                g.DrawRectangle(p, rect);
            }
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            var g = e.Graphics;

            var rect = new Rectangle(e.ImageRectangle.Left - 2, e.ImageRectangle.Top - 2,
                                            e.ImageRectangle.Width + 4, e.ImageRectangle.Height + 4);

            using (var b = new SolidBrush(Colors.LightBorder))
            {
                g.FillRectangle(b, rect);
            }

            using (var p = new Pen(Colors.BlueHighlight))
            {
                var modRect = new Rectangle(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
                g.DrawRectangle(p, modRect);
            }
        }

        /** Change separator color here */
        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            var g = e.Graphics;

            var rect = new Rectangle(1, 3, e.Item.Width, 1);

            using (var b = new SolidBrush(Colors.DarkBorder))
            {
                g.FillRectangle(b, rect);
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = Colors.LightText;
            e.ArrowRectangle = new Rectangle(new Point(e.ArrowRectangle.Left, e.ArrowRectangle.Top - 1), e.ArrowRectangle.Size);

            base.OnRenderArrow(e);
        }

        /** Change text color while hovered here */
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            var g = e.Graphics;

            e.Item.ForeColor = e.Item.Enabled ? Colors.LightText : Colors.DisabledText;

            if (e.Item.Enabled)
            {
                // Normal item
                if (e.Item.Selected)
                {
                    e.Item.ForeColor = Colors.LightText; // Highlight mouse-over text
                    var rect = new Rectangle(2, 0, e.Item.Width - 3, e.Item.Height);

                    using (var b = new SolidBrush(Colors.BlueSelection))
                    {
                        g.FillRectangle(b, rect); // fill or do not fill selected item bg
                    }
                }

                // Header item on open menu
                if (e.Item.GetType() == typeof(ToolStripMenuItem))
                {
                    if (((ToolStripMenuItem)e.Item).DropDown.Visible && e.Item.IsOnDropDown == false)
                    {
                        var rect = new Rectangle(2, 0, e.Item.Width - 3, e.Item.Height);

                        using (var b = new SolidBrush(Colors.GreySelection))
                        {
                            g.FillRectangle(b, rect);
                        }
                    }
                }
            }
        }

        #endregion
    }
}


