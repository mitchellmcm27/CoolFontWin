using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFW.Business;

namespace CFW.ViewModel
{
    public class InputSettingsViewModel : ReactiveObject
    {
        private static readonly ILog log =
           LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        readonly ObservableAsPropertyHelper<bool> _BonjourNotInstalled;
        public bool BonjourNotInstalled
        {
            get { return _BonjourNotInstalled.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _PrimaryDevice;
        public bool PrimaryDevice
        {
            get { return _PrimaryDevice.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _SecondaryDevice;
        public bool SecondaryDevice
        {
            get { return _SecondaryDevice.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _XboxController;
        public bool XboxController
        {
            get { return _XboxController.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _PauseButtonText;
        public string PauseButtonText
        {
            get { return _PauseButtonText.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _PauseButtonIcon;
        public string PauseButtonIcon
        {
            get { return _PauseButtonIcon.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _IsPaused;
        public bool IsPaused
        {
            get { return _IsPaused.Value; }
            set
            {
                IsNotPaused = !value;
            }
        }

        bool _IsNotPaused;
        public bool IsNotPaused
        {
            get { return _IsNotPaused; }
            set { this.RaiseAndSetIfChanged(ref _IsNotPaused, value); }
        }

        private readonly DeviceManager DeviceManager;
        private readonly DNSNetworkService DnsServer;

        public InputSettingsViewModel(AppBootstrapper bs)
        {
            DeviceManager = bs.DeviceManager;
            DnsServer = bs.DnsServer;

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

            this.WhenAnyValue(x => x.DeviceManager.VDevice.Mode, m => m == SimulatorMode.ModePaused)
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

        private int previousMode;
        private async Task PlayPauseImpl()
        {
            if (!IsPaused)
            {
                if (DeviceManager.Mode != SimulatorMode.ModePaused) previousMode = (int)DeviceManager.Mode;
                await UpdateMode((int)SimulatorMode.ModePaused);
            }
            else await UpdateMode(previousMode);
        }

        private async Task UpdateMode(int mode)
        {
            await Task.Run(() => DeviceManager.TryMode(mode));
        }


    }


}
