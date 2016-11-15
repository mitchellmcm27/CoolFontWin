using System;
using System.Drawing;
using System.Windows.Forms;

/* https://github.com/RobinPerris/DarkUI */

namespace CFW.Business
{
    public enum UIStyle
    {
        UIStyleNormal,
        UIStyleVR,
    }
    public class CFWContextMenuRenderer : ToolStripRenderer
    {
        #region Initialisation Region

        public Font Font;
        private UIStyle Style;

        public CFWContextMenuRenderer (UIStyle style)
        {
            switch(style)
            {
                case UIStyle.UIStyleNormal:
                    this.Font = new Font(Control.DefaultFont.Name, 9F);
                    break;
                case UIStyle.UIStyleVR:
                    this.Font = new Font(Control.DefaultFont.Name, 18F);
                    break;
            }
            this.Style = style;
        }

        protected override void Initialize(ToolStrip toolStrip)
        {
            base.Initialize(toolStrip);

            if (this.Style==UIStyle.UIStyleVR)
            {
                toolStrip.ImageScalingSize = new Size(32, 32);
            }
            else
            {
                toolStrip.ImageScalingSize = new Size(16, 16);
            }
            toolStrip.BackColor = Colors.FlatBlack;
            toolStrip.ForeColor = Colors.LightText;
            toolStrip.ShowItemToolTips = true;           
        }

        protected override void InitializeItem(ToolStripItem item)
        {
            base.InitializeItem(item);

            item.ForeColor = Colors.LightText;
            
            if (item.GetType() == typeof(ToolStripSeparator))
            {
                item.Margin = new Padding(0, 0, 0, 0);
            }
        }

        #endregion

        #region Render Region

        /** Change background color here */
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            var g = e.Graphics;
            using (var b = new SolidBrush(Colors.FlatBlack))
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

            using (var b = new SolidBrush(Colors.FlatBlack))
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

            e.Item.Font = this.Font;

            // comment these out if you don't want special text colors
            if (e.Item.Enabled && e.Item.Tag != null && e.Item.Tag.Equals("alert"))
            {
                e.Item.ForeColor = Colors.IconOrangeInnerCircle;
            }
            else if (e.Item.Enabled && e.Item.Font.Bold)
            {
                e.Item.ForeColor = Colors.IconBlue;
            }
            else if (e.Item.Enabled)
            {
                e.Item.ForeColor = Colors.LightText;
            }
            else
            {
                e.Item.ForeColor = Colors.DisabledText;
            }

            if (e.Item.Enabled)
            {
                // Normal item
                if (e.Item.Selected)
                {
                   // e.Item.ForeColor = Color.White; // Highlight mouse-over text
                    var rect = new Rectangle(0, 0, e.Item.Width, e.Item.Height);

                    using (var b = new SolidBrush(Colors.FlatBlackLight))
                    {
                        g.FillRectangle(b, rect); // fill or do not fill selected item bg
                    }
                }

                // Header item on open menu
                if (e.Item.GetType() == typeof(ToolStripMenuItem))
                {
                    if (((ToolStripMenuItem)e.Item).DropDown.Visible && e.Item.IsOnDropDown == false)
                    {
                        var rect = new Rectangle(0, 0, e.Item.Width - 0, e.Item.Height);

                        using (var b = new SolidBrush(Colors.FlatBlack))
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


