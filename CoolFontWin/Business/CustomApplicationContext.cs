using System;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Deployment;
using System.Deployment.Application;
using System.ComponentModel;
using log4net;

using CFW.Forms;

namespace CFW.Business
{
    public class CustomApplicationContext : ApplicationContext
    {
        private static readonly ILog log =
                LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string DefaultTooltip = "Pocket Strafe Companion";
        private static readonly string CurrentInstallLocation = Assembly.GetExecutingAssembly().Location;

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
            set { Ver = value; }
        }

        private NotifyIconController Cfw;

        public CustomApplicationContext()
        {
            InitializeContext();
            Cfw = new NotifyIconController(NotifyIcon);
            Cfw.StartServices();
        }

        private System.ComponentModel.IContainer Components;
        private NotifyIcon NotifyIcon;

        private void InitializeContext()
        {
            if (Properties.Settings.Default.FirstInstall)
            {
                log.Info("First launch after fresh install");
                log.Info("Install location " + CurrentInstallLocation);

                ShowSuccessfulInstallForm();
                
                
                Properties.Settings.Default.FirstInstall = false;
            }

            if (Properties.Settings.Default.JustUpdated)
            {
                log.Info("First launch with latest version.");
                log.Info("Install location " + CurrentInstallLocation);

                ShowUpdateNotesForm();

                try
                {
                    ForceFirewallWindow();
                }
                catch (Exception e)
                {
                    log.Error("Unable to open temp TCP socket because: " + e.Message);
                    log.Info("Windows Firewall should prompt on the next startup.");
                }

                Properties.Settings.Default.JustUpdated = false;

            }

            Properties.Settings.Default.Save();

            Components = new System.ComponentModel.Container();
            NotifyIcon = new NotifyIcon(Components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.tray_icon,
                Text = CustomApplicationContext.DefaultTooltip,
                Visible = true
            };
            NotifyIcon.ContextMenuStrip.Renderer = new CustomContextMenuRenderer();
            NotifyIcon.ContextMenuStrip.ShowItemToolTips = true;

            NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            NotifyIcon.MouseUp += NotifyIcon_MouseUp;
            
        }

        private void ForceFirewallWindow()
        {
            log.Info("Opening, closing TCP socket so that Windows Firewall prompt appears...");

            System.Net.IPAddress ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0];
            log.Info("Address: " + ipAddress.ToString());
            System.Net.IPEndPoint ipLocalEndPoint = new System.Net.IPEndPoint(ipAddress, 12345);

            System.Net.Sockets.TcpListener t = new System.Net.Sockets.TcpListener(ipLocalEndPoint);
            t.Start();
            t.Stop();
        }

        private void AddFirewallRule(string path)
        {
            // Not used currently
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                return;
            }

            log.Info("Authorize firewall via netsh command");

            string arguments = "advfirewall firewall add rule name=\"CoolFontWin\" dir=in action=allow program=\"" + path + "\" enable=yes";
            log.Info("netsh " + arguments);
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", arguments);
            procStartInfo.RedirectStandardOutput = false;
            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.Verb = "runas";
            Process.Start(procStartInfo);
        }

        private void DeleteFirewallRule(string path)
        {
            // Not used currently
            if (!ApplicationDeployment.IsNetworkDeployed)
            {
                return;
            }

            log.Info("Delete firewall rule via netsh command");

            string arguments = "advfirewall firewall delete rule name=\"CoolFontWin\" program=\"" + path + "\"";
            log.Info("netsh " + arguments);
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", arguments);
            procStartInfo.RedirectStandardOutput = false;
            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.Verb = "runas";
            Process.Start(procStartInfo);
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

            e.Cancel = false;

            NotifyIcon.ContextMenuStrip.Items.Clear();

            ToolStripMenuItem versionItem = new ToolStripMenuItem("Current version: " + Ver);
            versionItem.Enabled = false;
            NotifyIcon.ContextMenuStrip.Items.Add(versionItem);

            Cfw.AddToContextMenu(NotifyIcon.ContextMenuStrip);

            ToolStripMenuItem restartItem = Cfw.ToolStripMenuItemWithHandler("Restart", Restart_Click);
            ToolStripMenuItem quitItem = Cfw.ToolStripMenuItemWithHandler("&Quit", Exit_Click);
            quitItem.Image = Properties.Resources.ic_clear_orange_18dp;
            quitItem.Tag = "alert"; // changes font color to orange

            ToolStripMenuItem logItem = Cfw.ToolStripMenuItemWithHandler("View log file", ViewLog_Click);
            logItem.Image = Properties.Resources.ic_open_in_browser_white_18dp;

            NotifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] { new ToolStripSeparator(), logItem, quitItem });

         //   AddDebugMenuItems(); // called only if DEBUG is defined
        }

        private void ViewLog_Click(Object sender, EventArgs e)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                log.Info(path + "\\CoolFontWin\\Log.txt");
                Process.Start(path + "\\CoolFontWin\\Log.txt");
            }
            catch (Exception ex)
            {
                log.Error("Error opening Log.txt: " + ex);
            }
        }

        // removed ZedGraph.dll reference
        /*
        [Conditional("DEBUG")]
        private void AddDebugMenuItems()
        {
            ToolStripMenuItem graphItem = Cfw.ToolStripMenuItemWithHandler("Show graph (debug only)", ShowGraphFormItem_Clicked);

            NotifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] { new ToolStripSeparator(), graphItem });
        }
        */

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

        private SuccessForm SuccessForm;
        private UpdateNotes UpdateNotes;

        private void ShowSuccessfulInstallForm()
        {
            if (SuccessForm == null)
                SuccessForm = new SuccessForm();
            SuccessForm.Closed += SucessForm_Closed;
            SuccessForm.Show();
        }

        private void ShowUpdateNotesForm()
        {
            if (UpdateNotes == null)
                UpdateNotes = new UpdateNotes();
            UpdateNotes.Closed += UpdateNotes_Closed;
            UpdateNotes.Show();
        }

        private void SucessForm_Closed(object sender, EventArgs e) { SuccessForm = null; }
        private void UpdateNotes_Closed(object sender, EventArgs e) { UpdateNotes = null; }

        #endregion

        #region exit handling

        protected override void Dispose(bool disposing) 
        {
            if (disposing && Components != null)
            {
                Components.Dispose();
            }
        }

        protected override void ExitThreadCore()
        {
            // If next launch will be a new version
            if (Properties.Settings.Default.JustUpdated)
            {
               // DeleteFirewallRule(CurrentInstallLocation);
            }

            Cfw.Dispose();
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

        private void Restart_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
        #endregion

        #region updating

        public void CheckForUpdates()
        {

            if (ApplicationDeployment.IsNetworkDeployed)
            {
                log.Info("CoolFontWin Version " + Ver);
                log.Info("Checking for updates...");
                ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
                ad.CheckForUpdateCompleted += new CheckForUpdateCompletedEventHandler(ad_CheckForUpdateCompleted);
                ad.CheckForUpdateProgressChanged += new DeploymentProgressChangedEventHandler(ad_CheckForUpdateProgressChanged);

                ad.CheckForUpdateAsync();
            }
        }

        void ad_CheckForUpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            
        }

        void ad_CheckForUpdateCompleted(object sender, CheckForUpdateCompletedEventArgs e)
        {
            log.Info("Done checking for updates.");
            
            if (e.Error != null)
            {
               log.Error("Could not retrieve new version of the application. Reason: \n" + e.Error.Message + "\nPlease report this error to the system administrator.");
                return;
            }
            else if (e.Cancelled == true)
            {
               log.Info("The update was cancelled.");
            }

            if (e.UpdateAvailable)
            {
                log.Info("Update available. Beginning... ");
                BeginUpdate();
            }
        }

        private void BeginUpdate()
        {
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.UpdateCompleted += new AsyncCompletedEventHandler(ad_UpdateCompleted);

            // Indicate progress in the application's status bar.
            ad.UpdateProgressChanged += new DeploymentProgressChangedEventHandler(ad_UpdateProgressChanged);
            ad.UpdateAsync();
        }

        void ad_UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {         
        }

        void ad_UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {     
            if (e.Cancelled)
            {
                log.Info("The update of the application's latest version was cancelled.");
                return;
            }
            else if (e.Error != null)
            {
                log.Error("Could not install the latest version of the application. Reason: \n" + e.Error.Message + "\nPlease report this error to the system administrator.");
                return;
            }

            log.Info("Update completed.");
            Properties.Settings.Default.JustUpdated = true;
            Properties.Settings.Default.Save();

            // update will apply on next startup                 
        }
        #endregion
    
    }
}
