using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using log4net;
using System.Threading;

namespace CFW
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log =
           LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Mutex mutex = null;

        protected override void OnStartup(StartupEventArgs e)
        {
            log.Info("===APP STARTUP===");

            mutex = AcquireMutex();
            if (mutex == null)
            {
                log.Warn("Application was already running.");
                Application.Current.Shutdown();
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);
  
            StartAsync();
            base.OnStartup(e);
        }

        Business.AppBootstrapper bs;
        private async void StartAsync()
        {
            bs = new Business.AppBootstrapper();
            var main = new MainWindow();
            var splash = new View.SplashScreen();
            main.DataContext = new ViewModel.MainViewModel(bs);
            main.splashControl.Content = splash;
            main.contentControl.Visibility = Visibility.Collapsed;
            
            main.Show();

            await Task.Run(()=>bs.Start());
            var settings = new View.SettingsWindow();
            main.contentControl.Content = settings;

            main.splashControl.Visibility = Visibility.Collapsed;
            main.contentControl.Visibility = Visibility.Visible;          
        }

        protected override void OnExit(ExitEventArgs e)
        {
            log.Info("===APP SHUTDOWN===");
            mutex.ReleaseMutex();
            base.OnExit(e);
        }

        private static Mutex AcquireMutex()
        {
            Mutex appGlobalMutex = new Mutex(false, "mutex");
            if (!appGlobalMutex.WaitOne(3000))
            {
                return null;
            }
            return appGlobalMutex;
        }

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            log.Fatal("Unhandled Exception: " + e.Message);
            log.Fatal(String.Format("Runtime terminating: {0}", args.IsTerminating));

            MessageBoxResult res = MessageBox.Show(
                "Would you like to send a report to CoolFont?",
                "Program Terminated Unexpectedly",
                MessageBoxButton.YesNo, 
                MessageBoxImage.Error);

            if (res == MessageBoxResult.Yes || res == MessageBoxResult.OK)
            {
                Business.LogFileManager.EmailLogFile();

            }

        }
    }
}
