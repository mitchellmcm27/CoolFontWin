using ReactiveUI;

namespace PocketStrafe.ViewModel
{
    internal class MainViewModel : ReactiveObject
    {
        public InputSettingsViewModel InputSettingsVM { get; set; }
        public OutputSettingsViewModel OutputSettingsVM { get; set; }
        public SplashScreenViewModel SplashVM { get; set; }
        public ToolbarViewModel ToolbarVM { get; set; }

        public MainViewModel(PocketStrafeBootStrapper ps)
        {
            SplashVM = new SplashScreenViewModel(ps);
            InputSettingsVM = new InputSettingsViewModel(ps);
            OutputSettingsVM = new OutputSettingsViewModel(ps);
            ToolbarVM = new ToolbarViewModel(ps);
        }
    }
}