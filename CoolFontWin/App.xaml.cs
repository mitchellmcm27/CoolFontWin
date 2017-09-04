using log4net;
using Microsoft.Shell;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms.Integration;

namespace PocketStrafe
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application, ISingleInstanceApp
    {
        private static readonly ILog log =
           LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool SignalExternalCommandLineArgs(IList<string> args)
        {
            // handle command line arguments of second instance, if necessary
            return true;
        }

        private const string Unique = "PocketStrafePC-1";

        [STAThread]
        public static void Main()
        {
            if (SingleInstance<App>.InitializeAsFirstInstance(Unique))
            {
                SplashScreen splashScreen = new SplashScreen("resources/unplugged-smallest.png");
                splashScreen.Show(true);
                var app = new App();
                app.InitializeComponent();
                app.Run();

                // Allow single instance code to perform cleanup operations
                SingleInstance<App>.Cleanup();
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            log.Info("===APP STARTUP===");
            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(UnhandledExceptionHandler);
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
            base.OnExit(e);
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs args)
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