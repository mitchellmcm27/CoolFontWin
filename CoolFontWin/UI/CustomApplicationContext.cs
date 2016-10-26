using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using System.Diagnostics;
using System.Deployment;
using System.Deployment.Application;
using CoolFont.UI;


namespace CoolFont.AppWinForms
{
    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly string IconFileName = "tray-icon.ico";
        private static readonly string DefaultTooltip = "Pocket Strafe Companion";
        private CoolFontWin Cfw;

        string _Ver;
        public string Ver
        {
            get
            {
                Version version;
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    _Ver = version.ToString();
                }
                else
                {
                    _Ver = "Debug";
                }
                return _Ver;
            }
        }

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
            ToolStripMenuItem versionItem = new ToolStripMenuItem("Current version: " + Ver);
            versionItem.Enabled = false;

            NotifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] { new ToolStripSeparator(), quitItem, versionItem });

            AddDebugMenuItems(); // called only if DEBUG is defined
        }

        [Conditional("DEBUG")]
        private void AddDebugMenuItems()
        {
            ToolStripMenuItem graphItem = Cfw.ToolStripMenuItemWithHandler("Show graph (debug only)", ShowGraphFormItem_Clicked);

            NotifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] { new ToolStripSeparator(), graphItem });
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
        
        #region updating

        public void CheckForUpdates()
        {
            InstallUpdateSyncWithInfo();
        }

        private void InstallUpdateSyncWithInfo()
        {
            UpdateCheckInfo info = null;

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

                try
                {
                    info = ad.CheckForDetailedUpdate();

                }
                catch (DeploymentDownloadException dde)
                {
                    MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
                    return;
                }
                catch (InvalidDeploymentException ide)
                {
                    MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
                    return;
                }
                catch (InvalidOperationException ioe)
                {
                    MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
                    return;
                }

                if (info.UpdateAvailable)
                {
                    Boolean doUpdate = true;

                    if (!info.IsUpdateRequired)
                    {
                        DialogResult dr = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButtons.OKCancel);
                        if (!(DialogResult.OK == dr))
                        {
                            doUpdate = false;
                        }
                    }
                    else
                    {
                        // Display a message that the app MUST reboot. Display the minimum required version.
                        MessageBox.Show("This application has detected a mandatory update from your current " +
                            "version to version " + info.MinimumRequiredVersion.ToString() +
                            ". The application will now install the update and restart.",
                            "Update Available", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);
                    }

                    if (doUpdate)
                    {
                        try
                        {
                            ad.Update();
                            MessageBox.Show("The application has been upgraded, and will now restart.");
                            Application.Restart();
                        }
                        catch (DeploymentDownloadException dde)
                        {
                            MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
                            return;
                        }
                    }
                }
            }
        }
        #endregion
    
    }
}
