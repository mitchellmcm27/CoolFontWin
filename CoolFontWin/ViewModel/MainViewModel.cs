using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace CFW.ViewModel
{
    class MainViewModel : ReactiveObject
    {
        public InputSettingsViewModel InputSettingsVM { get; set; }
        public OutputSettingsViewModel OutputSettingsVM { get; set; }
        public SplashScreenViewModel SplashVM { get; set; }
        public ToolbarViewModel ToolbarVM { get; set; }

        public MainViewModel(Business.AppBootstrapper bs)
        {
            SplashVM = new SplashScreenViewModel(bs);
            InputSettingsVM = new InputSettingsViewModel(bs);
            OutputSettingsVM = new OutputSettingsViewModel(bs);
            ToolbarVM = new ToolbarViewModel(bs);
        }
    }
}
