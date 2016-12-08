using System;
using System.Windows.Forms;
using System.Linq;
using System.Reflection;
using System.Diagnostics;
using System.Deployment;
using System.Deployment.Application;
using System.ComponentModel;
using System.Collections.Generic;
using System.Threading;
using Ookii.Dialogs;
using log4net;

using CFW.Forms;


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
        private View.SettingsWindow SettingsView;
        
        public CFWApplicationContext()
        {
            
            Updater = new SilentUpdater(); // checks immediately then starts 20 min timer
            Updater.Completed += Updater_Completed;
            Updater.PropertyChanged += Updater_PropertyChanged;

            // Install ScpVBus every time application is launched
            // Uninstall it on exit (see region below)
            if (!ScpVBus.Install() && Properties.Settings.Default.ShowScpVbusDialog)
            {
                ShowScpVbusDialog();
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

            

            InitializeContext();
            Cfw = new NotifyIconController(NotifyIcon);
            Cfw.StartServices();

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
            Process.Start("http://www.coolfont.co");
        }

        private void InitializeContext()
        {
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

            ToolStripMenuItem versionItem = new ToolStripMenuItem(VersionItemString());
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

            NotifyIcon.ContextMenuStrip.Items.Add(versionItem);

            // Add VDevice handling items
            Cfw.AddToContextMenu(NotifyIcon.ContextMenuStrip);

            ToolStripMenuItem restartItem = Cfw.ToolStripMenuItemWithHandler("Restart", Restart_Click);
            ToolStripMenuItem quitItem = Cfw.ToolStripMenuItemWithHandler("Quit CoolFontWin", Exit_Click);
            quitItem.Image = Properties.Resources.ic_close_orange_18dp;
            quitItem.ImageScaling = ToolStripItemImageScaling.None;

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

        private SuccessForm SuccessForm;
        private UpdateNotes UpdateNotes;
        private View.SettingsWindow SettingsWindow;
        private ViewModel.Presenter Presenter;

        private void ShowSettingsWindow()
        {
            if (SettingsWindow == null)
            {
                SettingsWindow = new View.SettingsWindow();
                Presenter = new ViewModel.Presenter(Cfw);
                SettingsWindow.DataContext = Presenter;
                SettingsWindow.Closed += (o, i) => SettingsWindow = null;
            }              
            SettingsWindow.Show();
        }

        private void ShowSuccessfulInstallForm()
        {
            if (SuccessForm == null)
            {
                SuccessForm = new SuccessForm();
            }

            SuccessForm.Closed += (o, i) => SuccessForm = null;
            SuccessForm.Show();
        }

        private void ShowUpdateNotesForm()
        {
            if (UpdateNotes == null)
            {
                UpdateNotes = new UpdateNotes();
            }

            UpdateNotes.Closed += (o, i) => UpdateNotes = null;
            UpdateNotes.Show();
        }

        private void ShowScpVbusDialog()
        {
            var taskDialog = new TaskDialog();
            taskDialog.Width = 200;
            taskDialog.AllowDialogCancellation = true;

            taskDialog.WindowTitle = "CoolFontWin - An important component was not installed";
            taskDialog.MainIcon = TaskDialogIcon.Warning;

            taskDialog.MainInstruction = "ScpVBus failed to install";
            taskDialog.Content = "Xbox controller emulation requires ScpVBus.\n";
            taskDialog.Content += "You can download and install it, or continue using only keyboard/joystick emulation.";

            taskDialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;
            var customButton = new TaskDialogButton(ButtonType.Custom);
            customButton.CommandLinkNote = "github.com/shauleiz/ScpVBus";
            customButton.Text = "ScpVBus download page";
            customButton.Default = true;
            taskDialog.Buttons.Add(customButton);
            taskDialog.Buttons.Add(new TaskDialogButton(ButtonType.Close));

            taskDialog.ExpandFooterArea = true;
            taskDialog.ExpandedControlText = "Installation tips";
            taskDialog.ExpandedInformation = "1.  Download ScpVbus-x64.zip and extract it anywhere\n2.  Follow the directions on the website to install\n3.  Restart CoolFontWin";
            taskDialog.VerificationText = "Don't show this warning again";

            new Thread(() =>
            {
                try
                {
                    TaskDialogButton res = taskDialog.Show(); // Windows Vista and later
                    if (res != null && res.ButtonType == ButtonType.Custom)
                    {
                        Process.Start("https://github.com/shauleiz/ScpVBus/releases/tag/v1.7.1.2");
                    }

                    if (taskDialog.IsVerificationChecked)
                    {
                        Properties.Settings.Default.ShowScpVbusDialog = false;
                        Properties.Settings.Default.Save();
                    }
                }
                catch (Exception e)
                {
                    log.Warn("ScpVBus install dialog not shown, probably because operating system was earlier than Windows Vista.");
                    log.Warn(e.Message);
                    return;
                }
            }).Start();
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
            Cfw.Dispose();      
            Dispose(true);
            base.ExitThreadCore();
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            NotifyIcon.Visible = false;
            ScpVBus.Uninstall();
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
