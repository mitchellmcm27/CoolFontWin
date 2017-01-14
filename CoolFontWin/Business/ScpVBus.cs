using System;
using log4net;
using System.Diagnostics;
using System.ComponentModel;
using Ookii.Dialogs;
using System.Threading;

namespace CFW.Business
{
    public class ScpVBus : INotifyPropertyChanged
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // devcon.exe exit codes
        // https://github.com/Microsoft/Windows-driver-samples/blob/master/setup/devcon/devcon.h

        private enum DevconExitCode
        {
            EXIT_OK = 0,
            EXIT_REBOOT = 1,
            EXIT_FAIL = 2,
            EXIT_USAGE = 3
        }

        private static readonly string exe = "scpvbus\\devcon.exe";
        private static readonly string installargs = "install scpvbus\\ScpVBus.inf Root\\ScpVBus";
        private static readonly string uninstallargs = "remove Root\\ScpVBus";
        private readonly Process proc = new Process();
        private bool _installSuccess;
        public bool InstallSuccess
        {
            get { return _installSuccess; }
            set
            {
                _installSuccess = value;
                OnPropertyChanged("InstallSuccess");
                if (_installSuccess) Installed = true;
            }
        }

        public bool Installed = false;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Install ScpVBus async
        /// </summary>
        /// <returns></returns>
        public void Install()
        {
            log.Info("Attempt to install ScpVBus...");
            ProcessStartInfo startInfo = new ProcessStartInfo(exe, installargs);
           // startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Verb = "runas";
            try
            {
                proc.StartInfo = startInfo;
                proc.EnableRaisingEvents = true;
                proc.Exited += Proc_Exited; 
                proc.Start();
                proc.WaitForExit();
            }
            catch (Exception e)
            {
                log.Warn("Failed to start devcon.exe to install ScpVBus: " + e.Message);
                InstallSuccess = false;
            }
        }

        private void Proc_Exited(object sender, EventArgs args)
        {
            
            DevconExitCode exitCode = (DevconExitCode)proc.ExitCode;
            log.Info("ScpVBus installation finished with code: " + exitCode.ToString());

            switch (exitCode)
            {
                case DevconExitCode.EXIT_OK:
                    log.Info("ScpVBus installation suceeded.");
                    InstallSuccess = true;
                    break;
                case DevconExitCode.EXIT_REBOOT:
                    log.Info("ScpVBus installation requires reboot.");
                    InstallSuccess = true;
                    break;
                case DevconExitCode.EXIT_FAIL:
                    log.Info("ScpVBus installation failed.");
                    InstallSuccess = false;
                    break;
                case DevconExitCode.EXIT_USAGE:
                    log.Info("Devcon.exe command received incorrect argument.");
                    InstallSuccess = false;
                    break;
                default:
                    log.Info("Devcon.exe returned an unkown exit code.");
                    InstallSuccess = false;
                    break;
            }

            if (!InstallSuccess)
            {
                ShowScpVbusDialog();
            }
        }

        /// <summary>
        /// Uninstall ScpVBus. Blocks so that app can close after it's done installing.
        /// </summary>
        /// <returns></returns>
        public bool Uninstall()
        {
            if (!Installed)
            {
                log.Info("Didn't install ScpVBus, so not uninstalling it.");
                return true;
            }

            log.Info("Attempt to uninstall ScpVBus...");
            ProcessStartInfo startInfo = new ProcessStartInfo(exe, uninstallargs);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Verb = "runas";
            try
            {
                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
                log.Info("Uninstalled.");
                Installed = false;
                return true;
                // do not wait to see if successful
            }
            catch (Exception e)
            {
                log.Warn("Failed to start devcon.exe to uninstall ScpVBus: " + e.Message);
                return false;
            }
        }

        public static void ShowScpVbusDialog()
        {
            var taskDialog = new TaskDialog();
            taskDialog.Width = 200;
            taskDialog.AllowDialogCancellation = true;

            taskDialog.WindowTitle = "An important component was not installed";
            taskDialog.MainIcon = TaskDialogIcon.Warning;

            taskDialog.MainInstruction = "ScpVBus failed to install";
            taskDialog.Content = "Xbox controller emulation requires ScpVBus.\n";
            taskDialog.Content += "ScpVBus is installed with PocketStrafe but it seems to have failed. Download and install ScpVBus yourself, or continue using only keyboard/joystick emulation.";

            taskDialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;
            var customButton = new TaskDialogButton(ButtonType.Custom);
            customButton.CommandLinkNote = "github.com/shauleiz/ScpVBus";
            customButton.Text = "ScpVBus download page";
            customButton.Default = true;
            taskDialog.Buttons.Add(customButton);
            taskDialog.Buttons.Add(new TaskDialogButton(ButtonType.Close));

            taskDialog.ExpandFooterArea = true;
            taskDialog.ExpandedControlText = "Installation tips";
            taskDialog.ExpandedInformation = "1.  Download ScpVbus-x64.zip and extract it anywhere\n2.  Follow the directions on the website to install\n3.  Restart app";
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
    }
}
