using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocketStrafe.ViewModel
{
    class SplashScreenViewModel : ReactiveObject
    {

        readonly ObservableAsPropertyHelper<string> _StatusText;
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
