using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using CoolFont.UI;


namespace CoolFont
{
    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly string IconFileName = "tray-icon.ico";
        private static readonly string DefaultTooltip = "Pocket Strafe Companion";
        private CoolFontWin Cfw;

        public CustomApplicationContext(string[] args)
        {
            InitializeContext();
            Cfw = new CoolFontWin(NotifyIcon, args);
            Cfw.StartService();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Components != null)
            {
                Components.Dispose();
            }
        }

        protected override void ExitThreadCore()
        {
            NotifyIcon.Visible = false;
            Dispose(true);
            base.ExitThreadCore();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it     
            Cfw.KillOpenProcesses();
            ExitThread();
            Environment.Exit(0);
        }

        private System.ComponentModel.IContainer Components;
        private NotifyIcon NotifyIcon;

        private void InitializeContext()
        {
            Components = new System.ComponentModel.Container();
            NotifyIcon = new NotifyIcon(Components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon(CustomApplicationContext.IconFileName),
                Text = CustomApplicationContext.DefaultTooltip,
                Visible = true
            };
            NotifyIcon.ContextMenuStrip.Renderer = new CustomContextMenuRenderer();

            /* Use custom renderer instead
            notifyIcon.ContextMenuStrip.BackColor = Color.White;//Ivory;
            notifyIcon.ContextMenuStrip.ForeColor = Color.DarkSlateGray;
            notifyIcon.ContextMenuStrip.ShowImageMargin = false; // no images
            notifyIcon.ContextMenuStrip.DropShadowEnabled = false;
            */
            NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            NotifyIcon.MouseUp += NotifyIcon_MouseUp;
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            Cfw.BuildContextMenu(NotifyIcon.ContextMenuStrip);

            ToolStripMenuItem quitItem = Cfw.ToolStripMenuItemWithHandler("&Quit", Exit_Click);
            quitItem.ForeColor = Color.Crimson;
            NotifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] {
                    new ToolStripSeparator(), quitItem });
        }

        private void NotifyIcon_MouseUp(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);

                mi.Invoke(NotifyIcon, null);
            }
            else if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(NotifyIcon, null);
            }
        }

    }
}
