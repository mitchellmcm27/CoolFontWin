using ReactiveUI;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using CFW.Business;
using System.Windows.Forms;

namespace CFW.ViewModel
{
    class ToolbarViewModel : ReactiveObject
    {

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        readonly ObservableAsPropertyHelper<bool> _UpdateAvailable;
        public bool UpdateAvailable
        {
            get { return _UpdateAvailable.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _UpdateIcon;
        public string UpdateIcon
        {
            get { return _UpdateIcon.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _UpdateToolTip;
        public string UpdateToolTip
        {
            get { return _UpdateToolTip.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _LightDarkIcon;
        public string LightDarkIcon
        {
            get { return _LightDarkIcon.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _IpAddress;
        public string IpAddress
        {
            get { return _IpAddress.Value; }
        }

        private readonly AppCastUpdater Updater;
        private readonly PocketStrafeDeviceManager DeviceManager;
        private readonly DNSNetworkService DnsServer;

        private ObservableAsPropertyHelper<SimulatorMode> _Mode;
        private SimulatorMode Mode { get { return (_Mode.Value); } }

        public ToolbarViewModel(PocketStrafe ps)
        {
            Updater = ps.AppCastUpdater;
            DnsServer = ps.DnsServer;
            DeviceManager = ps.DeviceManager;

            // IP address
            this.WhenAnyValue(x => x.DnsServer.Address, x => x.DnsServer.Port, (addr, p) => string.Format(addr + " : " + p.ToString()))
                .Where((addr, p) => addr.Length > 1 && p != 0)
                .ToProperty(this, x => x.IpAddress, out _IpAddress);

            // Updater
            this.WhenAnyValue(x => x.Updater.UpdateAvailable)
                .ToProperty(this, x => x.UpdateAvailable, out _UpdateAvailable);

            this.WhenAnyValue(x => x.Updater.UpdateAvailable, x => x.Updater.UpdateOnShutdown,
                    (available, shutdown) => shutdown ? "AlarmCheck" : available ? "Download" : "Minus")
                .ToProperty(this, x => x.UpdateIcon, out _UpdateIcon);

            this.WhenAnyValue(x => x.Updater.UpdateAvailable, x => x.Updater.UpdateOnShutdown,
                (available, shutdown) => shutdown ? "Update when closed" : available ? "Update is available" : "Up to date")
                .ToProperty(this, x => x.UpdateToolTip, out _UpdateToolTip);

            DownloadUpdate = ReactiveCommand.CreateFromTask(DownloadUpdateImpl);

            FlipX = ReactiveCommand.CreateFromTask(async _ => await Task.Run(() => DeviceManager.FlipAxis(OutputDeviceAxis.AxisX)));
            FlipY = ReactiveCommand.CreateFromTask(async _ => await Task.Run(() => DeviceManager.FlipAxis(OutputDeviceAxis.AxisY)));
            VJoyConfig = ReactiveCommand.CreateFromTask(VJoyConfigImpl);
            VJoyMonitor = ReactiveCommand.CreateFromTask(VJoyMonitorImpl);
            ViewLogFile = ReactiveCommand.CreateFromTask(ViewLogFileImpl);

        }

        public ReactiveCommand FlipX { get; set; }
        public ReactiveCommand FlipY { get; set; }
        public ReactiveCommand VJoyConfig { get; set; }
        public ReactiveCommand VJoyMonitor { get; set; }
        public ReactiveCommand ViewLogFile { get; set; }

        public ReactiveCommand ToggleLightDark { get; set; }
        public ReactiveCommand DownloadUpdate { get; set; }

        private async Task DownloadUpdateImpl()
        {
            if (Updater.UpdateOnShutdown)
            {
                Updater.UpdateOnShutdown = false;
            }
            else
            {
                await Task.Run(() => Updater.DownloadUpdate());
            }
        }               

        private async Task ViewLogFileImpl()
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                log.Info(path + "\\PocketStrafe\\Log.txt");
                await Task.Run(()=>System.Diagnostics.Process.Start(path + "\\PocketStrafe\\Log.txt"));
            }
            catch (Exception ex)
            {
                log.Error("Error opening Log.txt: " + ex);
            }
        }
    
        private async Task VJoyConfigImpl()
        {
            string fname = "vJoyConf.exe";
            string path = System.IO.Path.Combine("Program Files", "vJoy");
            try
            {
                await Task.Run(() => System.Diagnostics.Process.Start(System.IO.Path.Combine(Properties.Settings.Default.VJoyDir, fname)));
            }
            catch
            {
                string foundPath = FileManager.FindAndLaunch(path, fname);
                if (foundPath.Equals(string.Empty))
                {
                    ShowVJoyNotFoundMessageBox(fname, "Configure vJoy");
                }
                else
                {
                    log.Info("Found " + fname + " in " + foundPath + ". Saving in default setting for later use.");
                    Properties.Settings.Default.VJoyDir = foundPath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private async Task VJoyMonitorImpl()
        {
            string fname = "JoyMonitor.exe";
            string path = System.IO.Path.Combine("Program Files", "vJoy");
            try
            {
                await Task.Run(() => System.Diagnostics.Process.Start(System.IO.Path.Combine(Properties.Settings.Default.VJoyDir, fname)));
            }
            catch
            {
                string foundPath = FileManager.FindAndLaunch(path, fname);
                if (foundPath.Equals(string.Empty))
                {
                    ShowVJoyNotFoundMessageBox(fname, "Monitor vJoy");
                }
                else
                {
                    log.Info("Found " + fname + " in " + foundPath + ". Saving in default setting for later use.");
                    Properties.Settings.Default.VJoyDir = foundPath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void ShowVJoyNotFoundMessageBox(string fname, string description)
        {
            string title = "vJoy installation not found!";
            string message = String.Format("If vJoy is installed, you can launch the '{0}' app for Windows, or just browse to the vJoy install location manually. \n \n Browse to vJoy folder manually?", description);
            System.Windows.MessageBoxResult clicked = System.Windows.MessageBox.Show(message, title, System.Windows.MessageBoxButton.OKCancel, System.Windows.MessageBoxImage.Information);
            if (clicked == System.Windows.MessageBoxResult.OK)
            {
                VJoyOK_Click();
            }
        }

        private void VJoyOK_Click()
        {
            // Ookii: drop-in replacement for default dialog
            Ookii.Dialogs.VistaFolderBrowserDialog folderBrowser = new Ookii.Dialogs.VistaFolderBrowserDialog();
            folderBrowser.Description = "Select vJoy Folder (usually C:\\Program Files\\vJoy)";
            folderBrowser.UseDescriptionForTitle = true;
            folderBrowser.ShowNewFolderButton = false;

            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                Properties.Settings.Default.VJoyDir = folderBrowser.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }
    }
}
