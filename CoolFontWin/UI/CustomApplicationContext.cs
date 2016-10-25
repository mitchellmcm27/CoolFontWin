using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using CoolFont.UI;


namespace CoolFont.AppWinForms
{
    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly string IconFileName = "tray-icon.ico";
        private static readonly string DefaultTooltip = "Pocket Strafe Companion";
        private CoolFontWin Cfw;

        public CustomApplicationContext(string[] args)
        {
            handler = new ConsoleEventDelegate(ConsoleEventCallback);
            SetConsoleCtrlHandler(handler, true);
            InitializeContext();
            Cfw = new CoolFontWin(NotifyIcon, args);
            Cfw.StartService();
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
                new ToolStripItem[] { new ToolStripSeparator(), quitItem});

            AddDebugMenuItems(); // called only if DEBUG is defined
        }

        [Conditional("DEBUG")]
        private void AddDebugMenuItems()
        {
            ToolStripMenuItem graphItem = Cfw.ToolStripMenuItemWithHandler("Show graph (debug only)", ShowGraphFormItem_Clicked);
            NotifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] { new ToolStripSeparator(), graphItem});
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

        #region child forms

        private GraphForm GraphForm;

        private void ShowGraphForm()
        {
            if (GraphForm == null)
            {
                GraphForm = new GraphForm { Cfw = this.Cfw };
                GraphForm.Closed += GraphForm_Closed; // avoid reshowing a disposed form
                GraphForm.Show();
            }
            else { GraphForm.Activate(); }
        }

        private void ShowGraphFormItem_Clicked(object sender, EventArgs e) { ShowGraphForm(); }

        private void GraphForm_Closed(object sender, EventArgs e) { GraphForm = null; }

        #endregion

        #region exit handling

        bool ConsoleEventCallback(int eventType)
        {
            if (eventType == 2)
            {
                ExitThread();
            }
            return false;
        }

        static ConsoleEventDelegate handler;   // Keeps it from getting garbage collected
                                               
        private delegate bool ConsoleEventDelegate(int eventType);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool SetConsoleCtrlHandler(ConsoleEventDelegate callback, bool add);

        protected override void Dispose(bool disposing) 
        {
            if (disposing && Components != null)
            {
                Components.Dispose();
            }
        }

        protected override void ExitThreadCore()
        {
            //if (GraphForm != null) { GraphForm.Close(); }
            Cfw.KillOpenProcesses();
            NotifyIcon.Visible = false;
            Dispose(true);
            base.ExitThreadCore();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it     
            ExitThread();       
            Environment.Exit(0);
        }
        #endregion
    }
}
