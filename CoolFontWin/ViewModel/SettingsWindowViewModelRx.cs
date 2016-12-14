using CFW.Business;
using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CFW.ViewModel
{
    public class SettingsWindowViewModelRx : ReactiveObject
    {

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Expose model properties that the view can bind to
        // Raise propertychangedevent on set

        private readonly List<string> _modes = new List<string>(CFWMode.GetDescriptions());

        readonly ObservableAsPropertyHelper<uint> _CurrentDeviceID;
        private uint CurrentDeviceID
        {
            get { return _CurrentDeviceID.Value; }
        }

        public IEnumerable<string> Modes
        {
            get { return _modes; }
        }

        readonly ObservableAsPropertyHelper<bool> _SecondaryDevice;
        public bool SecondaryDevice
        {
            get { return _SecondaryDevice.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _XboxLedImage;
        public string XboxLedImage
        {
            get { return _XboxLedImage.Value; }
        }

        bool _XboxController;
        public bool XboxController
        {
            get { return _XboxController; }
            set
            {
                this.RaiseAndSetIfChanged(ref _XboxController, value);
            }
        }

        readonly ObservableAsPropertyHelper<bool> _KeyboardOutput;
        public bool KeyboardOutput
        {
            get { return _KeyboardOutput.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _XboxOutput;
        public bool XboxOutput
        {
            get { return _XboxOutput.Value; }
        } 

        bool _XboxOutputButtonIsEnabled;
        public bool XboxOutputButtonIsEnabled
        {
            get { return _XboxOutputButtonIsEnabled; }
            set { this.RaiseAndSetIfChanged(ref _XboxOutputButtonIsEnabled, value); }
        }

        readonly ObservableAsPropertyHelper<bool> _VJoyOutput;
        public bool VJoyOutput
        {
            get { return _VJoyOutput.Value; }
        }

        private int _CurrentVJoyDevice;
        public int CurrentVJoyDevice
        {
            get { return _CurrentVJoyDevice; }
            set
            {
                this.RaiseAndSetIfChanged(ref _CurrentVJoyDevice, value);
                AcquireAndUpdateVJoyDevice(value);
            }
        }

        bool _vJoyOutputButtonIsEnabled;
        public bool VJoyOutputButtonIsEnabled
        {
            get { return _vJoyOutputButtonIsEnabled; }
            set { this.RaiseAndSetIfChanged(ref _vJoyOutputButtonIsEnabled, value); }
        }

        readonly ObservableAsPropertyHelper<string> _CoupledText;
        public string CoupledText
        {
            get { return _CoupledText.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _CoupledOutput;
        public bool CoupledOutput
        {
            get { return _CoupledOutput.Value; }
        }

        List<int> _VJoyDevices;
        public List<int> VJoyDevices
        {
            get { return _VJoyDevices; }
            set { this.RaiseAndSetIfChanged(ref _VJoyDevices, value); }
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

        Visibility _VJoyVisibility;
        public Visibility VJoyVisibility
        {
            get { return _VJoyVisibility; }
            set { this.RaiseAndSetIfChanged(ref _VJoyVisibility, value); }
        }

        Visibility _XboxVisibility;
        public Visibility XboxVisibility
        {
            get { return _XboxVisibility; }
            set { this.RaiseAndSetIfChanged(ref _XboxVisibility, value); }
        }

        private readonly DeviceManager DeviceHub;
        private readonly DNSNetworkService DnsServer;

        public SettingsWindowViewModelRx(DeviceManager d, DNSNetworkService s)
        {
            DeviceHub = d;
            DnsServer = s;

            XboxController = DeviceHub.InterceptXInputDevice;
            XboxOutputButtonIsEnabled = DeviceHub.EnabledVJoyDevicesList.Where(x => x > 1000 && x < 1005).Count() > 0;
            VJoyDevices = new List<int>(DeviceHub.EnabledVJoyDevicesList.Where(x => x > 0 && x < 17));
            VJoyOutputButtonIsEnabled = VJoyDevices.Count > 0;

            // Commands 
            KeyboardMode = ReactiveCommand.CreateFromTask(async _ =>
            {
                await Task.Run(() => DeviceHub.RelinquishCurrentDevice(silent: true));
                await UpdateMode((int)SimulatorMode.ModeWASD);
            });

            VJoyMode = ReactiveCommand.CreateFromTask<int>(async (id) =>
            {
                await Task.Run(() => DeviceHub.AcquireVDev((uint)id));
                if (CoupledOutput)
                {
                    log.Info("Update mode to coupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
                }
                else
                {
                    log.Info("Update mode to decoupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
                }
            });

            XboxMode = ReactiveCommand.CreateFromTask(async _ =>
            {
                await Task.Run(() => DeviceHub.AcquireVDev(0));
                if (CoupledOutput)
                {
                    log.Info("Update mode to coupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
                }
                else
                {
                    log.Info("Update mode to decoupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
                }
            });

            AcquireDevice = ReactiveCommand.CreateFromTask(async _ => await Task.Run(() => DeviceHub.AcquireVDev(CurrentDeviceID)));

            // depends on checkbox state (bool parameter)
            AddRemoveSecondaryDevice = ReactiveCommand.Create<bool>(notAdded =>
            {
                if (notAdded) DnsServer.AddService("Secondary");
                else DnsServer.RemoveLastService();
            });

            PlayPause = ReactiveCommand.CreateFromTask(PlayPauseImpl);
            CoupledDecoupled = ReactiveCommand.CreateFromTask(CoupledDecoupledImpl);

            // Responding to model changes
            // Secondary device DNS service
            this.WhenAnyValue(x => x.DnsServer.DeviceNames, x => x.Count() > 1)
                .ToProperty(this, x => x.SecondaryDevice, out _SecondaryDevice);

            // Current vDevice ID
            this.WhenAnyValue(x => x.DeviceHub.VDevice.Id)
                .ToProperty(this, x => x.CurrentDeviceID, out _CurrentDeviceID);

            // Mode
            this.WhenAnyValue(x => x.DeviceHub.VDevice.Mode, m => m == SimulatorMode.ModePaused)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ToProperty(this, x => x.IsPaused, out _IsPaused);

            this.WhenAnyValue(x => x.IsPaused, x => x ? "Resume" : "Pause")
                .ToProperty(this, x => x.PauseButtonText, out _PauseButtonText);

            this.WhenAnyValue(x => x.IsPaused, x => x ? "Play" : "Pause") // Google material icon names
                .ToProperty(this, x => x.PauseButtonIcon, out _PauseButtonIcon);

            this.WhenAnyValue(x => x.DeviceHub.VDevice.Mode, m => m == SimulatorMode.ModeWASD)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ToProperty(this, x => x.KeyboardOutput, out _KeyboardOutput);

            this.WhenAnyValue(x => x.DeviceHub.VDevice.Mode, x => x.DeviceHub.VDevice.Id, (m, id) =>
                (m == SimulatorMode.ModeJoystickCoupled || m == SimulatorMode.ModeJoystickDecoupled) && id > 1000)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ToProperty(this, x => x.XboxOutput, out _XboxOutput);

            this.WhenAnyValue(x => x.DeviceHub.VDevice.Mode, x => x.DeviceHub.VDevice.Id, (m, id) =>
                (m == SimulatorMode.ModeJoystickCoupled || m == SimulatorMode.ModeJoystickDecoupled) && id < 17)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .ToProperty(this, x => x.VJoyOutput, out _VJoyOutput);

            this.WhenAnyValue(x => x.DeviceHub.VDevice.Mode, m => m == SimulatorMode.ModeJoystickCoupled || m == SimulatorMode.ModeWASD)
                //.Throttle(TimeSpan.FromMilliseconds(200))
                .ToProperty(this, x => x.CoupledOutput, out _CoupledOutput);

            this.WhenAnyValue(x => x.CoupledOutput, x => x ? "Coupled" : "Decoupled")
                .ToProperty(this, x => x.CoupledText, out _CoupledText);

            // Xbox controller LED image
            this.WhenAnyValue(x => x.DeviceHub.VDevice.Id)
                .Throttle(TimeSpan.FromMilliseconds(200))
                .Select(x => XboxLedImagePath((int)x))
                .ToProperty(this, x => x.XboxLedImage, out _XboxLedImage);      
        }

        public ReactiveCommand KeyboardMode { get; set; }
        public ReactiveCommand VJoyMode { get; set; }
        public ReactiveCommand XboxMode { get; set; }
        public ReactiveCommand AcquireDevice { get; set; }
        public ReactiveCommand AddRemoveSecondaryDevice { get; set; }
        public ReactiveCommand PlayPause { get; set; }
        public ReactiveCommand CoupledDecoupled { get; set; }


        // public Commands return ICommand using DelegateCommand class
        // and are backed by private methods

        public ICommand InterceptXInputDeviceCommand
        {
            get { return new AwaitableDelegateCommand(IntercpetXInputDeviceAsync); }
        }

        public ICommand UnplugAllXboxCommand
        {
            get { return new AwaitableDelegateCommand(UnplugAllXboxAsync); }
        }

        public ICommand JoyCplCommand
        {
            get { return new DelegateCommand(() => Process.Start("joy.cpl")); }
        }

        private async Task CoupledDecoupledImpl()
        {
            if (KeyboardOutput) return;
            if (CoupledOutput)
            {
                await UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
            }
            else
            {
                await UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
            }
        }

        private async Task IntercpetXInputDeviceAsync()
        {
            await Task.Run(() => DeviceHub.InterceptXInputDevice = _XboxController);
        }

        private async Task UpdateMode(int mode)
        {
            await Task.Run(() => DeviceHub.TryMode(mode));
        }

        private async Task UnplugAllXboxAsync()
        {
            await Task.Run(()=> DeviceHub.ForceUnplugAllXboxControllers(silent: true));
            await UpdateMode((int)SimulatorMode.ModeWASD);
        }

        private int previousMode;
        private async Task PlayPauseImpl()
        {
            if (!IsPaused)
            {
                if (DeviceHub.Mode != SimulatorMode.ModePaused) previousMode = (int)DeviceHub.Mode;
                await UpdateMode((int)SimulatorMode.ModePaused);
            }
            else await UpdateMode(previousMode);
        }   

        // Helper methods

        private string XboxLedImagePath(int id)
        {
            switch (id)
            {
                default:
                    return "/CoolFontWin;component/Resources/ic_xbox_off_blue_18dp.png";
                case 1001:
                    return "/CoolFontWin;component/Resources/ic_xbox_1p_blue_18dp.png";
                case 1002:
                    return "/CoolFontWin;component/Resources/ic_xbox_2p_blue_18dp.png";
                case 1003:
                    return "/CoolFontWin;component/Resources/ic_xbox_3p_blue_18dp.png";
                case 1004:
                    return "/CoolFontWin;component/Resources/ic_xbox_4p_blue_18dp.png";
            }
        }

        private async Task AcquireAndUpdateVJoyDevice(int id)
        {
            await Task.Run(()=>DeviceHub.AcquireVDev((uint)id));
            await Task.Run(()=>DeviceHub.TryMode(CoupledOutput ? (int)SimulatorMode.ModeJoystickCoupled : (int)SimulatorMode.ModeJoystickDecoupled));
        }

        // Notifications
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {

            if (e.PropertyName == "InterceptXInputDevice")
            {
                XboxController = DeviceHub.InterceptXInputDevice;
            }

            else if (e.PropertyName == "CurrentDevices")
            {
                _VJoyDevices = new List<int>(DeviceHub.EnabledVJoyDevicesList.Where(x => x > 0 && x < 17));

                XboxOutputButtonIsEnabled = DeviceHub.EnabledVJoyDevicesList.Where(x => x > 1000 && x < 1005).Count() > 0;
                VJoyOutputButtonIsEnabled = VJoyDevices.Count > 0;
            }
        }
    }
}
