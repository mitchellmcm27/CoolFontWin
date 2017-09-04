using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace PocketStrafe.ViewModel
{
    internal class SplashScreenViewModel : ReactiveObject
    {
        private readonly ObservableAsPropertyHelper<string> _StatusText;

        public string StatusText
        {
            get { return _StatusText.Value; }
        }

        private PocketStrafeBootStrapper ps;

        public SplashScreenViewModel(PocketStrafeBootStrapper bs)
        {
            ps = bs;
            this.WhenAnyValue(x => x.ps.Status)
                .Throttle(TimeSpan.FromSeconds(1))
                .ToProperty(this, x => x.StatusText, out _StatusText);
        }
    }
}