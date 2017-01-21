using AutoUpdaterDotNET;
using log4net;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace CFW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public MainWindow()
        {
            InitializeComponent();
        }

        protected void Window_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            DragMove();
        }

        private void Window_Closing(object sender, EventArgs e)
        {
            if (AutoUpdater.UpdateOnShutdown)
            {
                log.Info("Download update");
                AutoUpdater.DownloadUpdate();
                return; // Update will shut down the app
            }
        }
    }
}
