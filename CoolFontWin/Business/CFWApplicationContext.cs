using System;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Deployment.Application;
using System.ComponentModel;
using System.Collections.Generic;
using log4net;

using CFW.ViewModel;


namespace CFW.Business
{
    public class CFWApplicationContext : ApplicationContext
    {
        private static readonly ILog log =
                LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string VersionString = "PocketStrafe";
        private static readonly string DefaultTooltip = "PocketStrafe PC";
        private static readonly string CurrentInstallLocation = Assembly.GetExecutingAssembly().Location;

        private string _Ver;
        public string Ver
        {
            get { return _Ver; }
            set
            {
                _Ver = value;
            }
        }

        public SilentUpdater Updater { get; private set; }
        private NotifyIconViewModel NotifyIconViewModel;
        private System.ComponentModel.IContainer Components;
        private NotifyIcon NotifyIcon;
        private ScpVBus scpInstaller = new ScpVBus();

        private UDPServer UdpServer;
        private DNSNetworkService DnsServer;
        private DeviceManager DeviceManager;

        public CFWApplicationContext()
        {

            Updater = new SilentUpdater(); // checks immediately then starts 20 min timer
            Updater.Completed += Updater_Completed;
            Updater.PropertyChanged += Updater_PropertyChanged;

            // Install ScpVBus every time application is launched
            // Must be installed synchronously
            // Uninstall it on exit (see region below)
            // scpInstaller.Install();

            DeviceManager = new DeviceManager();
            UdpServer = new UDPServer(DeviceManager);

            InitializeContext();

            // Get number of expected mobile device inputs from Default
            List<string> names = Properties.Settings.Default.ConnectedDevices.Cast<string>().ToList();
            DeviceManager.MobileDevicesCount = names.Count;
            UdpServer.Start(Properties.Settings.Default.LastPort);
            int port = UdpServer.Port;
            Properties.Settings.Default.LastPort = port;
            Properties.Settings.Default.Save();
            
            // publish 1 network service for each device
            DnsServer = new DNSNetworkService(port, DeviceManager);
            for (int i = 0; i < names.Count; i++)
            {
                DnsServer.Publish(port, names[i]);
            }

            NotifyIconViewModel = new NotifyIconViewModel(DeviceManager, DnsServer);
            NotifyIcon.Text = GetVersionItemString();
            NotifyIcon.Visible = true;

            log.Info("Load settings window...");
            LoadSettingsWindow();

            log.Info("Get enabled devices...");
            DeviceManager.VDevice.GetEnabledDevices();

            log.Info("Show settings window...");
            ShowSettingsWindow();
            Properties.Settings.Default.FirstInstall = false;
            Properties.Settings.Default.Save();

            // Show tooltips if deployed via ClickOnce
            if (ApplicationDeployment.IsNetworkDeployed && Properties.Settings.Default.FirstInstall)
            {
                log.Info("First launch after fresh install");
                log.Info("Install location " + CurrentInstallLocation);

                NotifyIcon.ShowBalloonTip(
                    30000,
                    "PocketStrafe PC successfully installed",
                    "Get more information at www.pocketstrafe.com",
                    ToolTipIcon.Info);
            }

            else if (ApplicationDeployment.IsNetworkDeployed && ApplicationDeployment.CurrentDeployment.IsFirstRun)
            {
                log.Info("First launch with latest version.");
                log.Info("Install location " + CurrentInstallLocation);

                NotifyIcon.ShowBalloonTip(
                    30000,
                    "PocketStrafe PC updated",
                    "Get update notes at www.pocketstrafe.com",
                    ToolTipIcon.Info);
            }

            try
            {
                ForceFirewallWindow();
            }
            catch (Exception e)
            {
                log.Error("Unable to open temp TCP socket because: " + e.Message);
                log.Info("Windows Firewall should prompt on the next startup.");
            }
        }

        private void Updater_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void Updater_Completed(object sender, EventArgs e)
        {
            // ResourceSoundPlayer.TryToPlay(Properties.Resources.reverb_good);
            Ver = GetVersionItemString();
            NotifyIcon.Text = Ver;

            if (Updater.UpdateAvailable)
            {
                NotifyIcon.Icon = Properties.Resources.tray_icon_notification;
            }
            else
            {
                NotifyIcon.Icon = Properties.Resources.tray_icon;
            }
        }

        private void NotifyIcon_BalloonTipClicked(object sender, EventArgs e)
        {
            Process.Start("http://www.pocketstrafe.com");
        }

        private void InitializeContext()
        {
            Components = new System.ComponentModel.Container();
            NotifyIcon = new NotifyIcon(Components)
            {
                ContextMenuStrip = new ContextMenuStrip(),
                Icon = Properties.Resources.tray_icon,
                Text = "PocketStrafe PC",
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
            List<System.Net.IPAddress> localAddrs = DnsServer.GetValidLocalAddresses();
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
            log.Info("Authorize firewall via netsh command");

            string arguments = "advfirewall firewall add rule name=\"PocketStrafe PC\" dir=in action=allow program=\"" + path + "\" enable=yes";
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
            log.Info("Delete firewall rule via netsh command");

            string arguments = "advfirewall firewall delete rule name=\"PocketStrafe PC\" program=\"" + path + "\"";
            log.Info("netsh " + arguments);
            ProcessStartInfo procStartInfo = new ProcessStartInfo("netsh", arguments);
            procStartInfo.RedirectStandardOutput = false;
            procStartInfo.UseShellExecute = true;
            procStartInfo.CreateNoWindow = true;
            procStartInfo.Verb = "runas";
            Process.Start(procStartInfo);
        }

        private string GetVersionItemString()
        {
            string version;
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                version = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            else
            {
                version = "";
            }
            return Updater.UpdateAvailable ? "Restart to apply update" : "PocketStrafe PC " + version;
        }

        private void VersionItem_Click(object sender, EventArgs e)
        {
            if (Updater.UpdateAvailable)
            {
                Application.Restart();
            }
        }

        private static readonly CFWContextMenuRenderer CustomRendererVR = new CFWContextMenuRenderer(UIStyle.UIStyleVR);
        private static readonly CFWContextMenuRenderer CustomRendererNormal = new CFWContextMenuRenderer(UIStyle.UIStyleNormal);

        private bool SteamVRRunning
        {
            get { return Process.GetProcessesByName("VRServer").Length > 0; }
        }
        bool OculusHomeRunning
        {
            get { return Process.GetProcessesByName("OculusVR").Length > 0; } // correct process name?
        }

        private void ContextMenuStrip_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = false;
            if (SteamVRRunning || OculusHomeRunning)
            {
                NotifyIcon.ContextMenuStrip.Renderer = CustomRendererVR;
            }
            else
            {
                NotifyIcon.ContextMenuStrip.Renderer = CustomRendererNormal;
            }

            NotifyIcon.ContextMenuStrip.Items.Clear();

            ToolStripMenuItem versionItem = new ToolStripMenuItem(GetVersionItemString());
            if (Updater.UpdateAvailable)
            {
                versionItem.Enabled = true;
                versionItem.Click += VersionItem_Click;
                versionItem.Font = new System.Drawing.Font(versionItem.Font, (versionItem.Font.Style | System.Drawing.FontStyle.Bold));
                versionItem.Image = Properties.Resources.ic_refresh_blue_18dp;
                versionItem.ImageScaling = ToolStripItemImageScaling.None;
            }
            else
            {
                versionItem.Enabled = false;
                versionItem.Image = Properties.Resources.ic_cloud_done_white_18dp;
                versionItem.ImageScaling = ToolStripItemImageScaling.None;
                versionItem.Text = "Latest version";
            }

            if (!ApplicationDeployment.IsNetworkDeployed) versionItem.Visible = false;

            ToolStripMenuItem settingsItem = NotifyIconViewModel.ToolStripMenuItemWithHandler("&Configure", (o, i) => ShowSettingsWindow());
            settingsItem.Image = Properties.Resources.ic_settings_white_18dp;
            //settingsItem.ImageScaling = ToolStripItemImageScaling.None;

            NotifyIcon.ContextMenuStrip.Items.AddRange( new ToolStripItem[] {
                    versionItem,
                    new ToolStripSeparator(),
                    settingsItem,
            });

            // Add VDevice handling items
            NotifyIconViewModel.AddToContextMenu(NotifyIcon.ContextMenuStrip);

            ToolStripMenuItem restartItem = NotifyIconViewModel.ToolStripMenuItemWithHandler("Restart", Restart_Click);
            ToolStripMenuItem quitItem = NotifyIconViewModel.ToolStripMenuItemWithHandler("Quit PocketStrafe", Exit_Click);

            NotifyIcon.ContextMenuStrip.Items.AddRange(
                new ToolStripItem[] { new ToolStripSeparator(), quitItem });

         //   AddDebugMenuItems(); // called only if DEBUG is defined
        }

        private void NotifyIcon_MouseUp(Object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowSettingsWindow();
            }
            else if (e.Button == MouseButtons.Right)
            {
                MethodInfo mi = typeof(NotifyIcon).GetMethod("ShowContextMenu", BindingFlags.Instance | BindingFlags.NonPublic);
                mi.Invoke(NotifyIcon, null);
            }
        }

        #region child forms

        private View.SettingsWindow SettingsWindow;
        private SettingsWindowViewModel SettingsWindowViewModel;

        private void LoadSettingsWindow()
        {
            if (SettingsWindow == null)
            {
                SettingsWindow = new View.SettingsWindow();
                SettingsWindowViewModel = new SettingsWindowViewModel(DeviceManager, DnsServer);
                SettingsWindow.DataContext = SettingsWindowViewModel;
                SettingsWindow.Closed += (o, i) => SettingsWindow = null;
                ElementHost.EnableModelessKeyboardInterop(SettingsWindow);
            }     
        }

        private void ShowSettingsWindow()
        {
            if (SettingsWindow == null) LoadSettingsWindow();
            SettingsWindow.Show();
        }

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
            DeviceManager.Dispose();
            Dispose(true);
            base.ExitThreadCore();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            NotifyIcon.Visible = false;
            if (scpInstaller.Installed) scpInstaller.Uninstall();
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
