using System.Windows;
using System.Windows.Shell;

namespace CFW.View
{
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
        }

        protected void Window_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            DragMove();
        }
    }
}
