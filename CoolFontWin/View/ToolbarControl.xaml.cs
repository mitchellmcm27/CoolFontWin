using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CFW.View
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

        protected void StackPanel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                StackPanel sp = sender as StackPanel;
                ContextMenu contextMenu = sp.ContextMenu;
                contextMenu.PlacementTarget = sp;
                contextMenu.IsOpen = true;
            }

           
        }
    }
}
