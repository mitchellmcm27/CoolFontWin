using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Timers;
using log4net;

namespace CFW.Business
{
    public class SilentUpdater : INotifyPropertyChanged
    {
        private static readonly ILog log =
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ApplicationDeployment Deployment;
        private readonly Timer CheckTimer = new Timer(TimeSpan.FromMinutes(20).TotalMilliseconds);
        private bool processing;

        public event EventHandler<DeploymentProgressChangedEventArgs> ProgressChanged;
        public event EventHandler<EventArgs> Completed;
        public event PropertyChangedEventHandler PropertyChanged;

        private bool _UpdateAvailable;
        public bool UpdateAvailable
        {
            get
            {
                return _UpdateAvailable;
            }
            private set
            {
                _UpdateAvailable = value;
                OnPropertyChanged("UpdateAvailable");
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OnCompleted()
        {
            Completed?.Invoke(this, null);
        }

        private void OnProgressChanged(DeploymentProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }

        public SilentUpdater()
        {
            if (!ApplicationDeployment.IsNetworkDeployed) return;
 
            log.Info("Checking for updates...");
            Deployment = ApplicationDeployment.CurrentDeployment;
            Deployment.UpdateCompleted += UpdateCompleted;
            Deployment.UpdateProgressChanged += UpdateProgressChanged;
            CheckTimer.Elapsed += CheckForUpdate;

            CheckTimer.Start(); // check when timer fires
            CheckForUpdate(null, null); // check when initialized
        }

        private void CheckForUpdate(object sender, EventArgs eventargs)
        {
            {
                if (processing)
                    return;
                processing = true;
                try
                {
                    // bool: Persist update to disk?
                    // false: Apply update silently
                    // true: Show prompt and allow user to skip update (not desired)
                    if (Deployment.CheckForUpdate(false)) 
                        Deployment.UpdateAsync();
                    else
                        processing = false;
                }
                catch (Exception ex)
                {
                    log.Warn("Check for update failed. " + ex.Message);
                    processing = false;
                }
            };
        }
        private void BeginUpdate()
        {
            ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;
            ad.UpdateCompleted += new AsyncCompletedEventHandler(UpdateCompleted);

            // Indicate progress in the application's status bar.
            ad.UpdateProgressChanged += new DeploymentProgressChangedEventHandler(UpdateProgressChanged);
            ad.UpdateAsync();
        }

        void UpdateProgressChanged(object sender, DeploymentProgressChangedEventArgs e)
        {
            OnProgressChanged(e);
        }

        void UpdateCompleted(object sender, AsyncCompletedEventArgs e)
        {
            processing = false;
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

            log.Info("Update completed, will apply on next startup");
            UpdateAvailable = true;
            OnCompleted();
        }
    }
}
