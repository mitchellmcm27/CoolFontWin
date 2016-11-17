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
using System.Collections.Generic;

namespace CFW.Business
{
    public class CFWApplicationContext : ApplicationContext
    {
        private static readonly ILog log =
                LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string DefaultTooltip = "Pocket Strafe Companion";
        private static readonly string CurrentInstallLocation = Assembly.GetExecutingAssembly().Location;

        private string _Ver;
        public string VersionDescription
        {
            get
            {
                Version version;
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
                    return version.ToString();
                }
                else
                {
                    return "Debug";
                }
            }
        }

        public SilentUpdater Updater { get; private set; }
        private NotifyIconController Cfw;
        private System.ComponentModel.IContainer Components;
        private NotifyIcon NotifyIcon;

        public CFWApplicationContext()
        {
            InitializeContext();

            Cfw = new NotifyIconController(NotifyIcon);         
            Cfw.StartServices();
            
            Updater = new SilentUpdater(); // checks immediately then starts 20 min timer
            Updater.Completed += Updater_Completed;
            Updater.PropertyChanged += Updater_PropertyChanged;

            NotifyIcon.Text = VersionItemString();
            if (ApplicationDeployment.IsNetworkDeployed && Properties.Settings.Default.FirstInstall)
            {
                log.Info("First launch after fresh install");
                log.Info("Install location " + CurrentInstallLocation);

                NotifyIcon.ShowBalloonTip(
                    30000,
                    "CoolFontWin successfully installed",
                    "Get more information at www.coolfont.co",
                    ToolTipIcon.Info);
                Properties.Settings.Default.FirstInstall = false;
                Properties.Settings.Default.Save();
            }
            else if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                log.Info("First launch with latest version.");
                log.Info("Install location " + CurrentInstallLocation);

                NotifyIcon.ShowBalloonTip(
                    30000,
                    "CoolFontWin updated",
                    "Get update notes at www.coolfont.co",
                    ToolTipIcon.Info);
            }
        }

        private void Updater_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void Updater_Completed(object sender, EventArgs e)
        {
            // ResourceSoundPlayer.TryToPlay(Properties.Resources.reverb_good);
            NotifyIcon.Text = VersionItemString();
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Process.Start("http://www.coolfont.co");
        }

        private void InitializeContext()
        {
            // if... 
            //ShowSuccessfulInstallForm();
            //ShowUpdateNotesForm();
            // save defaults

            try
            {
                ForceFirewallWindow();
            }
            catch (Exception e)
            {
                log.Error("Unable to open temp TCP socket because: " + e.Message);
                log.Info("Windows Firewall should prompt on the next startup.");
            }

            Components = new System.ComponentModel.Container();
            NotifyIcon = new NotifyIcon(Components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.tray_icon,
                Text = "CoolFontWin",
                Visible = true
            };
            
            NotifyIcon.ContextMenuStrip.Renderer = CustomRendererNormal;
            NotifyIcon.ContextMenuStrip.ShowItemToolTips = true;

            NotifyIcon.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
            NotifyIcon.MouseUp += NotifyIcon_MouseUp;

            NotifyIcon.BalloonTipClicked += new EventHandler(NotifyIcon_BalloonTipClicked);
        }

        /// <summary>
        /// Should force Windows Firewall prompt to show.
        /// </summary>
        private void ForceFirewallWindow()
        {
            log.Info("Opening, closing TCP socket so that Windows Firewall prompt appears...");

            // old way, get first IP address found
            //System.Net.IPAddress ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0];

            // new way, write function that avoids Hamachi network interfaces
            List<System.Net.IPAddress> localAddrs = DNSNetworkService.GetValidLocalAddresses();
            System.Net.IPAddress ipAddress = localAddrs.FirstOrDefault();

            log.Info("Address: " + ipAddress.ToString());

            System.Net.IPEndPoint ipLocalEndPoint = new System.Net.IPEndPoint(ipAddress, 12345);
            System.Net.Sockets.TcpListener t = new System.Net.Sockets.TcpListener(ipLocalEndPoint);
            t.Start();
            t.Stop();
        }

        /// <summary>
        /// Not used. Open and close TCP port instead.
        /// </summary>
        /// <param name="path"></param>
        private void AddFirewallRule(string path)
        {
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

        /// <summary>
        /// Not used. Open and close TCP port instead.
        /// </summary>
        /// <param name="path"></param>
        private void DeleteFirewallRule(string path)
        {
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

        private string VersionItemString()
        {
            return Updater.UpdateAvailable ? "Restart to apply update" : "CoolFontWin - " + VersionDescription;
        }

        private void VersionItem_Click(object sender, EventArgs e)
        {
            if (Updater.UpdateAvailable)
            {
                Application.Restart();
            }
        }

        private static CFWContextMenuRenderer CustomRendererVR = new CFWContextMenuRenderer(UIStyle.UIStyleVR);
        private static CFWContextMenuRenderer CustomRendererNormal = new CFWContextMenuRenderer(UIStyle.UIStyleNormal);

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;

            // TODO:
            // http://stackoverflow.com/questions/262280/how-can-i-know-if-a-process-is-running

            bool steamVRRunning =  Process.GetProcessesByName("VRServer").Length>0;
            bool oculusHomeRunning = Process.GetProcessesByName("OculusVR").Length>0; // correct process name?
            if (steamVRRunning || oculusHomeRunning)
            {
                NotifyIcon.ContextMenuStrip.Renderer = CustomRendererVR;
            }
            else
            {
                NotifyIcon.ContextMenuStrip.Renderer = CustomRendererNormal;
            }

            NotifyIcon.ContextMenuStrip.Items.Clear();

            ToolStripMenuItem versionItem = new ToolStripMenuItem(VersionItemString());
            if (Updater.UpdateAvailable)
            {
                versionItem.Enabled = true;
                versionItem.Click += VersionItem_Click;
                versionItem.Font = new System.Drawing.Font(versionItem.Font, (versionItem.Font.Style | System.Drawing.FontStyle.Bold));
                versionItem.Image = Properties.Resources.ic_refresh_blue_18dp;
            }
            else
            {
                versionItem.Enabled = false;
                versionItem.Image = Properties.Resources.ic_cloud_done_white_18dp;
                versionItem.Text = "Currently up-to-date";
            }

            NotifyIcon.ContextMenuStrip.Items.Add(versionItem);

            Cfw.AddToContextMenu(NotifyIcon.ContextMenuStrip);

            ToolStripMenuItem restartItem = Cfw.ToolStripMenuItemWithHandler("Restart", Restart_Click);
            ToolStripMenuItem quitItem = Cfw.ToolStripMenuItemWithHandler("&Quit", Exit_Click);
            quitItem.Image = Properties.Resources.ic_clear_orange_18dp;
            //quitItem.Tag = "alert"; // changes font color to orange

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
            {
                SuccessForm = new SuccessForm();
            }

            SuccessForm.Closed += SucessForm_Closed;
            SuccessForm.Show();
        }

        private void ShowUpdateNotesForm()
        {
            if (UpdateNotes == null)
            {
                UpdateNotes = new UpdateNotes();
            }

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
    }
}
