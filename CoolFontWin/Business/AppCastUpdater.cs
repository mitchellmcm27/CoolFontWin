using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using AutoUpdaterDotNET;
using ReactiveUI;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CFW.Business
{
    public class AppCastUpdater : ReactiveObject
    {
        private static readonly ILog log =
                LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private bool _UpdateAvailable;
        public bool UpdateAvailable
        {
            get { return _UpdateAvailable; }
            set { this.RaiseAndSetIfChanged(ref _UpdateAvailable, value); }
        }

        private bool _UpdateProblem;
        public bool UpdateProblem
        {
            get { return _UpdateProblem; }
            set { this.RaiseAndSetIfChanged(ref _UpdateProblem, value); }
        }

        private string _AppCastPath = "";

        public AppCastUpdater(string appCastPath)
        {
            _AppCastPath = appCastPath;
            AutoUpdater.CheckForUpdateEvent += AutoUpdaterOnCheckForUpdateEvent;
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            Start();
        }

        public void Start()
        {
            log.Info("Start checking for update...");
            AutoUpdater.Start(_AppCastPath);
        }

        private void AutoUpdaterOnCheckForUpdateEvent(UpdateInfoEventArgs args)
        {
            
            log.Info("Update Check completed.");

            if (args == null)
            {
                log.Info("Returned null.");
                UpdateAvailable = false;
                return; // no update or failed
            }

            log.Info("Current version: " + args.CurrentVersion);
            log.Info("Installed version: " + args.InstalledVersion);

            if (args.IsUpdateAvailable)
            {
                UpdateAvailable = true;
            }
            else
            {
                UpdateAvailable = false;
            }
        }

        public void DownloadUpdate()
        {
            if (!UpdateAvailable) return;
            AutoUpdater.CheckForUpdateEvent -= AutoUpdaterOnCheckForUpdateEvent;
            try
            {
                AutoUpdater.Start(_AppCastPath);
                
                //You can use Download Update dialog used by AutoUpdater.NET to download the update.
                // AutoUpdater.DownloadUpdate();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, exception.GetType().ToString(), MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }   
        }
        
    }
}
