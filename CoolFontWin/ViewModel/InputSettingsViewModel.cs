using log4net;
using ReactiveUI;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace PocketStrafe.ViewModel
{
    public class InputSettingsViewModel : ReactiveObject
    {
        private static readonly ILog log =
           LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ObservableAsPropertyHelper<bool> _BonjourNotInstalled;

        public bool BonjourNotInstalled
        {
            get { return _BonjourNotInstalled.Value; }
        }

        private readonly ObservableAsPropertyHelper<bool> _PrimaryDevice;

        public bool PrimaryDevice
        {
            get { return _PrimaryDevice.Value; }
        }

        private readonly ObservableAsPropertyHelper<bool> _SecondaryDevice;

        public bool SecondaryDevice
        {
            get { return _SecondaryDevice.Value; }
        }

        private readonly ObservableAsPropertyHelper<bool> _XboxController;

        public bool XboxController
        {
            get { return _XboxController.Value; }
        }

        private readonly ObservableAsPropertyHelper<string> _PauseButtonText;

        public string PauseButtonText
        {
            get { return _PauseButtonText.Value; }
        }

        private readonly ObservableAsPropertyHelper<string> _PauseButtonIcon;

        public string PauseButtonIcon
        {
            get { return _PauseButtonIcon.Value; }
        }

        private readonly ObservableAsPropertyHelper<bool> _IsPaused;

        public bool IsPaused
        {
            get { return _IsPaused.Value; }
            set
            {
                IsNotPaused = !value;
            }
        }

        private bool _IsNotPaused;

        public bool IsNotPaused
        {
            get { return _IsNotPaused; }
            set { this.RaiseAndSetIfChanged(ref _IsNotPaused, value); }
        }

        private readonly PocketStrafeDeviceManager DeviceManager;
        private readonly DNSNetworkService DnsServer;

        public InputSettingsViewModel(PocketStrafeBootStrapper ps)
        {
            DeviceManager = ps.DeviceManager;
            DnsServer = ps.DnsServer;

            // Primary device DNS service (implies that Bonjour wasn't installed)
            this.WhenAnyValue(x => x.DnsServer.BonjourInstalled, x => !x)
                .ToProperty(this, x => x.BonjourNotInstalled, out _BonjourNotInstalled);

            // Primary device DNS service
            this.WhenAnyValue(x => x.DnsServer.DeviceCount, x => x > 0)
                .ToProperty(this, x => x.PrimaryDevice, out _PrimaryDevice);

            // Secondary device DNS service
            this.WhenAnyValue(x => x.DnsServer.DeviceCount, x => x > 1)
                .ToProperty(this, x => x.SecondaryDevice, out _SecondaryDevice);

            // Device Manager
            // Xbox controller intercepted
            this.WhenAnyValue(x => x.DeviceManager.InterceptXInputDevice)
                .ToProperty(this, x => x.XboxController, out _XboxController);

            // Pausing
            this.WhenAnyValue(x => x.DeviceManager.IsPaused)
                .ToProperty(this, x => x.IsPaused, out _IsPaused);

            this.WhenAnyValue(x => x.IsPaused, x => x ? "Resume" : "Pause")
               .ToProperty(this, x => x.PauseButtonText, out _PauseButtonText);

            this.WhenAnyValue(x => x.IsPaused, x => x ? "Play" : "Pause")
                 .ToProperty(this, x => x.PauseButtonIcon, out _PauseButtonIcon);

            PlayPause = ReactiveCommand.CreateFromTask(PlayPauseImpl);

            InterceptXInputDevice = ReactiveCommand.CreateFromTask<bool>(async wasChecked =>
                {
                    if (wasChecked)
                    {
                        await Task.Run(() => DeviceManager.AcquireXInputDevice());
                    }
                    else DeviceManager.InterceptXInputDevice = false;
                });

            AddRemoveSecondaryDevice = ReactiveCommand.CreateFromTask(AddRemoveSecondaryDeviceImpl);
            AddRemoveSecondaryDevice.ThrownExceptions.Subscribe(ex => log.Error("AddRemoveSecondaryDevice\n" + ex));

            BonjourInfo = ReactiveCommand.CreateFromTask(_ => Task.Run(() => DnsServer.ShowBonjourDialog()));
        }

        public ReactiveCommand BonjourInfo { get; set; }
        public ReactiveCommand AddRemoveSecondaryDevice { get; set; }
        public ReactiveCommand InterceptXInputDevice { get; set; }

        private async Task AddRemoveSecondaryDeviceImpl()
        {
            if (!SecondaryDevice) await Task.Run(() => DnsServer.AddService("Secondary"));
            else await Task.Run(() => DnsServer.RemoveLastService());
        }

        public ReactiveCommand PlayPause { get; set; }

        private async Task PlayPauseImpl()
        {
            await Task.Run(() => DeviceManager.PauseOutput(!IsPaused));
        }
    }
}