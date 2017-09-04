using log4net;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;

namespace PocketStrafe
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

        private PocketStrafeBootStrapper ps;

        private async void StartAsync()
        {
            var main = new MainWindow();
            main.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            main.splashControl.Content = new View.Splash();
            ps = new PocketStrafeBootStrapper();
            main.DataContext = new ViewModel.MainViewModel(ps);
            main.contentControl.Visibility = Visibility.Hidden;
            var settings = new View.SettingsView();
            main.contentControl.Content = settings;
            main.Show();

            await Task.Run(() => ps.Start());
            main.splashControl.Visibility = Visibility.Collapsed;
            main.contentControl.Visibility = Visibility.Visible;
            ElementHost.EnableModelessKeyboardInterop(main);
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

        private static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            log.Fatal("Unhandled Exception: " + e);
            log.Fatal(String.Format("Runtime terminating: {0}", args.IsTerminating));

            MessageBoxResult res = MessageBox.Show(
                "Would you like to send a report to Cool Font?",
                "Program Terminated Unexpectedly",
                MessageBoxButton.YesNo,
                MessageBoxImage.Error);

            if (res == MessageBoxResult.Yes || res == MessageBoxResult.OK)
            {
                LogFileManager.EmailLogFile();
            }
        }
    }
}