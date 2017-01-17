using System.Windows;
using System.Windows.Controls;


namespace CFW.View
{
    /// <summary>
    /// Interaction logic for UpdateControl.xaml
    /// </summary>
    public partial class UpdateControl : UserControl
    {
        public UpdateControl()
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
    }
}
