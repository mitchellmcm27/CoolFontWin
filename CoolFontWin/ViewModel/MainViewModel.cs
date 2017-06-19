using ReactiveUI;

namespace CFW.ViewModel
{
    class MainViewModel : ReactiveObject
    {
        public InputSettingsViewModel InputSettingsVM { get; set; }
        public OutputSettingsViewModel OutputSettingsVM { get; set; }
        public SplashScreenViewModel SplashVM { get; set; }
        public ToolbarViewModel ToolbarVM { get; set; }

        public MainViewModel(Business.PocketStrafe ps)
        {
            SplashVM = new SplashScreenViewModel(ps);
            InputSettingsVM = new InputSettingsViewModel(ps);
            OutputSettingsVM = new OutputSettingsViewModel(ps);
            ToolbarVM = new ToolbarViewModel(ps);
        }
    }
}
