using System.Windows.Controls;
using CFW.ViewModel;
using System.Windows;

namespace CFW.View
{
    /// <summary>
    /// Interaction logic for OutputSettingsControl.xaml
    /// </summary>
    public partial class OutputSettingsControl : UserControl
    {
        public OutputSettingsControl()
        {
            InitializeComponent();
        }

        public void Keybind_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = string.Empty;
        }

        private void ProgressBar_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
        {

        }
    }
}
