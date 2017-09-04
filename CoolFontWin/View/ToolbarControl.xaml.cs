using System.Windows;
using System.Windows.Controls;

namespace PocketStrafe.View
{
    /// <summary>
    /// Interaction logic for UpdateControl.xaml
    /// </summary>
    public partial class ToolbarControl : UserControl
    {
        public ToolbarControl()
        {
            InitializeComponent();
        }

        protected void Close_Click(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.Close();
        }

        protected void Minimize_Click(object sender, RoutedEventArgs e)
        {
            var myWindow = Window.GetWindow(this);
            myWindow.WindowState = WindowState.Minimized;
        }

        protected void Window_MouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.DragMove();
        }
    }
}