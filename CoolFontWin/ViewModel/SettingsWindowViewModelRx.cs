using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CFW.Business;
using System.Windows.Input;
using System.ComponentModel;
using System.Threading;
using System.Windows.Data;
using System.Windows;
using log4net;
using System.Diagnostics;
using ReactiveUI;

namespace CFW.ViewModel
{
    public class SettingsWindowViewModelRx : ReactiveObject
    {

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Expose model properties that the view can bind to
        // Raise propertychangedevent on set

        private readonly List<string> _modes = new List<string>(CFWMode.GetDescriptions());
        private List<int> _xboxDevices;
        private List<int> _vJoyDevices;

        object lockObj = new object();


        private int _CurrentXboxDevice = 0;

        private uint _currentDeviceID;

        public IEnumerable<string> Modes
        {
            get { return _modes; }
        }

        bool _SecondaryDevice;
        public bool SecondaryDevice
        {
            get { return _SecondaryDevice; }
            set
            {
                this.RaiseAndSetIfChanged(ref _SecondaryDevice, value);
                if (value)
                {
                    Model.AddService("Secondary");
                }
                else
                {
                    Model.RemoveLastService();
                }
            }
        }

        string _XboxLedImage;
        public string XboxLedImage
        {
            get { return _XboxLedImage; }
            set { this.RaiseAndSetIfChanged(ref _XboxLedImage, value); }
        }

        bool _XboxDevice;
        public bool XboxDevice
        {
            get { return _XboxDevice; }
            set
            {
                this.RaiseAndSetIfChanged(ref _XboxDevice, value);
            }
        }

        bool _KeyboardOutput;
        public bool KeyboardOutput
        {
            get { return _KeyboardOutput; }
            set
            {
                this.RaiseAndSetIfChanged(ref _KeyboardOutput, value);
            }
        }

        bool _XboxOutput;
        public bool XboxOutput
        {
            get { return _XboxOutput; }
            set
            {
                if (_XboxOutput)
                {
                    _currentDeviceID = (uint)_CurrentXboxDevice + 1000;
                    XboxVisibility = Visibility.Visible;
                }
                else
                {
                    XboxVisibility = Visibility.Hidden;
                }
                this.RaiseAndSetIfChanged(ref _XboxOutput, value);

            }
        }

        public int CurrentXboxDevice
        {
            get
            {
                return _CurrentXboxDevice;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _CurrentXboxDevice, value);
                XboxLedImage = XboxLedImagePath(value);
                XboxOutput = true;
            }
        }

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

        bool _XboxOutputButtonIsEnabled;
        public bool XboxOutputButtonIsEnabled
        {
            get { return _XboxOutputButtonIsEnabled; }
            set { this.RaiseAndSetIfChanged(ref _XboxOutputButtonIsEnabled, value); }
        }

        bool _VJoyOutput = false;
        public bool VJoyOutput
        {
            get { return _VJoyOutput; }
            set
            {
                this.RaiseAndSetIfChanged(ref _VJoyOutput, value);
                if (_VJoyOutput)
                {
                    _currentDeviceID = (uint)_CurrentVJoyDevice;
                    VJoyVisibility = Visibility.Visible;
                }
                else
                {
                    VJoyVisibility = Visibility.Hidden;
                }
            }
        }

        int _CurrentVJoyDevice = 0;
        public int CurrentVJoyDevice
        {
            get
            {
                return (_CurrentVJoyDevice == 0) ? VJoyDevices.FirstOrDefault() : _CurrentVJoyDevice;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _CurrentVJoyDevice, value);
                XboxLedImage=XboxLedImagePath(value);
                VJoyOutput = true;
            }
        }

        bool _vJoyOutputButtonIsEnabled;
        public bool VJoyOutputButtonIsEnabled
        {
            get { return _vJoyOutputButtonIsEnabled; }
            set { this.RaiseAndSetIfChanged(ref _vJoyOutputButtonIsEnabled, value); }
        }

        string _CoupledText;
        public string CoupledText
        {
            get { return _CoupledText; }
            set { this.RaiseAndSetIfChanged(ref _CoupledText, value); }
        }

        bool _CoupledOutput;
        public bool CoupledOutput
        {
            get { return _CoupledOutput; }
            set
            {
                this.RaiseAndSetIfChanged(ref _CoupledOutput, value);
                CoupledText = value ? "Coupled" : "Decoupled";
            }
        }

        List<int> _VJoyDevices;
        public List<int> VJoyDevices
        {
            get { return _VJoyDevices; }
            set { this.RaiseAndSetIfChanged(ref _VJoyDevices, value); }
        }
    
        bool _IsPaused;
        public bool IsPaused
        {
            get { return _IsPaused; }
            set
            {
                this.RaiseAndSetIfChanged(ref _IsPaused, value);
                IsNotPaused = !value;
                PauseButtonText = value ? "Resume" : "Pause";
                PauseButtonIcon = value ? "Play" : "Pause"; // google material icon names
            }
        }

        bool _IsNotPaused;
        public bool IsNotPaused
        {
            get { return _IsNotPaused; }
            set { this.RaiseAndSetIfChanged(ref _IsNotPaused, value); }
        }

        string _PauseButtonText;
        public string PauseButtonText
        {
            get { return _PauseButtonText; }
            set { this.RaiseAndSetIfChanged(ref _PauseButtonText, value); }
        }

        string _PauseButtonIcon;
        public string PauseButtonIcon
        {
            get { return _PauseButtonIcon; }
            set { this.RaiseAndSetIfChanged(ref _PauseButtonIcon, value); }
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

        private readonly BusinessModel Model;
        public SettingsWindowViewModelRx(BusinessModel model)
        {
            Model = model;
            Model.PropertyChanged += Model_PropertyChanged;
            _XboxDevice = Model.InterceptXInputDevice;
            _xboxDevices = new List<int>(Model.CurrentDevices.Where(x => x > 1000 && x < 1005).Select(x => x - 1000));
            _XboxOutputButtonIsEnabled = _xboxDevices.Count > 0;
            _vJoyDevices = new List<int>(Model.CurrentDevices.Where(x => x > 0 && x < 17));
            _vJoyOutputButtonIsEnabled = _vJoyDevices.Count > 0;

            _IsPaused = Model.Mode == SimulatorMode.ModePaused;
            _KeyboardOutput = Model.Mode == SimulatorMode.ModeWASD && !_IsPaused;
            _XboxOutput = !_KeyboardOutput && Model.CurrentDeviceID > 1000 && !_IsPaused;
            _VJoyOutput = !_XboxOutput && !_KeyboardOutput && !_IsPaused;
            _CoupledOutput = Model.Mode == SimulatorMode.ModeJoystickCoupled && !_IsPaused;

        }

        // public Commands return ICommand using DelegateCommand class
        // and are backed by private methods

        public ICommand XboxOutputCommand
        {
            get { return AcquireDeviceAsyncCommand; }
        }

        public ICommand VJoyOutputCommand
        {
            get { return AcquireDeviceAsyncCommand; }
        }

        public ICommand KeyboardOutputCommand
        {
            get { return AcquireDeviceAsyncCommand; }
        }

        public ICommand CoupledDecoupledCommand
        {
            get { return new AwaitableDelegateCommand(UpdateCoupledCommand); }
        }

        public ICommand InterceptXInputDeviceCommand
        {
            get { return new AwaitableDelegateCommand(IntercpetXInputDeviceAsync); }
        }

        public ICommand UnplugAllXboxCommand
        {
            get { return new AwaitableDelegateCommand(UnplugAllXboxAsync); }
        }

        public ICommand CurrentVJoyDeviceChangedCommand
        {
            get
            {
                return AcquireDeviceAsyncCommand;
            }
        }

        private ICommand AcquireDeviceAsyncCommand
        {
            get { return new AwaitableDelegateCommand(AcquireDeviceAsync); }
        }

        public ICommand PlayPauseCommand
        {
            get { return new AwaitableDelegateCommand(PlayPause); }
        }

        public ICommand JoyCplCommand
        {
            get { return new DelegateCommand(() => Process.Start("joy.cpl")); }
        }

        private async Task UpdateCoupledCommand()
        {
            if (KeyboardOutput) return;
            if (CoupledOutput)
            {
                await UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
            }
            else
            {
                await UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
            }
        }

        private async Task AcquireDeviceAsync()
        {
            log.Info("AcquireDeviceAsync Task:");
            if (_KeyboardOutput)
            {
                log.Info("Keyboard output TRUE, update mode to keyboard");
                await UpdateMode((int)SimulatorMode.ModeWASD);
            }
            else
            {
                log.Info("Keyboard output FALSE, acquire vdev");
                await Model.AcquireVDevAsync(_currentDeviceID);

                if (_CoupledOutput)
                {
                    log.Info("Update mode to coupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
                }
                else
                {
                    log.Info("Update mode to decoupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
                }
            }

        }

        private int previousMode;
        private async Task PlayPause()
        {
            IsPaused = !IsPaused;
            if (_IsPaused)
            {
                previousMode = (int)Model.Mode;
                await UpdateMode((int)SimulatorMode.ModePaused);
            }
            else await UpdateMode(previousMode);
        }
        private async Task IntercpetXInputDeviceAsync()
        {
            await Task.Run(() => Model.InterceptXInputDevice = _XboxDevice);
        }

        private async Task UpdateMode(int mode)
        {
            await Model.UpdateModeAsync(mode);
        }

        private async Task UnplugAllXboxAsync()
        {
            CurrentXboxDevice = 0;
            await Model.UnplugAllXboxAsync(silent: true);
        }

     
        // Notifications
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DeviceNames")
            {
                SecondaryDevice = Model.DeviceNames.Count > 1;
            }
            else if (e.PropertyName == "InterceptXInputDevice")
            {
                XboxDevice = Model.InterceptXInputDevice;
            }
            else if (e.PropertyName == "Mode")
            {
                IsPaused = Model.Mode == SimulatorMode.ModePaused;
                KeyboardOutput = Model.Mode == SimulatorMode.ModeWASD && !_IsPaused;
                XboxOutput = !_KeyboardOutput && Model.CurrentDeviceID > 1000 && !_IsPaused;
                VJoyOutput = !_XboxOutput && !_KeyboardOutput && !_IsPaused;

                CoupledOutput = (KeyboardOutput || Model.Mode == SimulatorMode.ModeJoystickCoupled) && !_IsPaused;
            }
            else if (e.PropertyName == "CurrentDeviceID")
            {
                _currentDeviceID = Model.CurrentDeviceID;
                if (_currentDeviceID > 0 && _currentDeviceID < 17) CurrentVJoyDevice = (int)_currentDeviceID;
                else if (_currentDeviceID > 1000 && _currentDeviceID < 1005) CurrentXboxDevice = (int)_currentDeviceID - 1000;
            }
            else if (e.PropertyName == "CurrentDevices")
            {
                _xboxDevices = new List<int>(Model.CurrentDevices.Where(x => x > 1000 && x < 1005).Select(x => x - 1000));
                _VJoyDevices = new List<int>(Model.CurrentDevices.Where(x => x > 0 && x < 17));

                XboxOutputButtonIsEnabled = _xboxDevices.Count > 0;
                VJoyOutputButtonIsEnabled = _vJoyDevices.Count > 0;


            }
        }
    }
}
