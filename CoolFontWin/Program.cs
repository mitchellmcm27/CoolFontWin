using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Drawing;
using System.Reflection;
using MutexManager;
using CoolFont.IO;

namespace CoolFont
{
   static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            if (!SingleInstance.Start()) { return;  }  // Mutex not obtained so exit
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Console.WriteLine(args);
            try
            {
                var applicationContext = new CustomApplicationContext(args);
                Application.Run(applicationContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Program Terminated Unexpectedly", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            SingleInstance.Stop(); // Release mutex

        }
    }

    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly string IconFileName = "AppIcon.ico";
        private static readonly string DefaultTooltip = "Pocket Strafe Companion";
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
            JavaProc.Kill();
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
                Icon = new Icon(IconFileName),
                Text = DefaultTooltip,
                Visible = true     
            };
            /* http://www.99colors.net/dot-net-colors */
            notifyIcon.ContextMenuStrip.BackColor = Color.DarkSlateGray;
            notifyIcon.ContextMenuStrip.ForeColor = Color.LightSteelBlue; 
            notifyIcon.ContextMenuStrip.ShowImageMargin = false; // no images

            notifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            notifyIcon.MouseUp += notifyIcon_MouseUp;
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {  
            e.Cancel = false;
            cfw.BuildContextMenu(notifyIcon.ContextMenuStrip);
            notifyIcon.ContextMenuStrip.Items.Add(cfw.ToolStripMenuItemWithHandler("&Exit", exit_Click));          
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
