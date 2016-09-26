using System;
using System.Windows.Forms;
using System.Drawing;
using System.Reflection;
using CoolFont.UI;


namespace CoolFont
{
    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly string iconFileName = "tray-icon.ico";
        private static readonly string defaultTooltip = "Pocket Strafe Companion";
        private CoolFontWin cfw;

        public CustomApplicationContext(string[] args)
        {
            InitializeContext();
            cfw = new CoolFontWin(notifyIcon, args);
            cfw.StartService();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
            {
                components.Dispose();
            }
        }

        protected override void ExitThreadCore()
        {
            notifyIcon.Visible = false;
            Dispose(true);
            base.ExitThreadCore();
        }

        private void exit_Click(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it     
            cfw.KillOpenProcesses();
            ExitThread();
            Environment.Exit(0);
        }

        private System.ComponentModel.IContainer components;
        private NotifyIcon notifyIcon;

        private void InitializeContext()
        {
            components = new System.ComponentModel.Container();
            notifyIcon = new NotifyIcon(components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = new Icon(CustomApplicationContext.iconFileName),
                Text = CustomApplicationContext.defaultTooltip,
                Visible = true
            };
            notifyIcon.ContextMenuStrip.Renderer = new CustomContextMenuRenderer();

            /* Use custom renderer instead
            notifyIcon.ContextMenuStrip.BackColor = Color.White;//Ivory;
            notifyIcon.ContextMenuStrip.ForeColor = Color.DarkSlateGray;
            notifyIcon.ContextMenuStrip.ShowImageMargin = false; // no images
            notifyIcon.ContextMenuStrip.DropShadowEnabled = false;
            */
            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.MouseUp += notifyIcon_MouseUp;
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            cfw.BuildContextMenu(notifyIcon.ContextMenuStrip);

            ToolStripMenuItem quitItem = cfw.ToolStripMenuItemWithHandler("&Quit", exit_Click);
            quitItem.ForeColor = Color.Crimson;
            notifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] {
                    new ToolStripSeparator(), quitItem });
        }

        private void notifyIcon_MouseUp(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);

                mi.Invoke(notifyIcon, null);
            }
            else if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(notifyIcon, null);
            }
        }

    }
}
