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
        public SettingsWindowViewModel SettingsVM { get; set; }
        public SplashScreenViewModel SplashVM { get; set; }

        public MainViewModel(Business.AppBootstrapper bs)
        {
            SplashVM = new SplashScreenViewModel(bs);
            SettingsVM = new SettingsWindowViewModel(bs);
        }
    }
}
