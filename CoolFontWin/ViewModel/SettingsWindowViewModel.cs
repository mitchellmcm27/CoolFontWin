using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CFW.Business;
using System.Windows.Input;

namespace CFW.ViewModel
{
    public class SettingsWindowViewModel : ObservableObject
    {
        // Expose model properties that the view can bind to
        // Raise propertychangedevent on set

        private readonly ObservableCollection<string> _modes = new ObservableCollection<string>(CFWMode.GetDescriptions());
        private ObservableCollection<int> _xboxDevices;       
        private ObservableCollection<int> _vJoyDevices;

        private int _currentMode;
        private bool _vJoyOutput = false;
        private int _currentXboxDevice = 0;
        private int _currentVJoyDevice = 0;

        private uint _previousDeviceID;
        private uint _currentDeviceID;

        public IEnumerable<string> Modes
        {
            get { return _modes; }
        }

        public int CurrentMode
        {
            get { return (int)Model.Mode; }
            set
            {
                if (Model.UpdateMode(value))
                {
                    _currentMode = value;
                    RaisePropertyChangedEvent("CurrentMode");
                }
            }
        }

        public bool SecondaryDevice
        {
            get { return Model.DeviceNames.Count > 1; }
            set
            {
                if (value)
                {
                    Model.AddService("Secondary");
                }
                else
                {
                    Model.RemoveLastService();
                }
                RaisePropertyChangedEvent("SecondaryDevice");
            }
        }

        public bool XboxDevice
        {
            get { return Model.InterceptXInputDevice; }
            set
            {
                Model.InterceptXInputDevice = value;
                RaisePropertyChangedEvent("XboxDevice");
                // should update xboxoutput somehow
            }
        }

        public bool KeyboardOutput
        {
            get { return Model.Mode==SimulatorMode.ModeWASD; }
            set
            {
                if (value)
                {
                    Model.UpdateMode((int)SimulatorMode.ModeWASD);
                    RaisePropertyChangedEvent("CoupledOutput");
                }
                RaisePropertyChangedEvent("KeyboardOutput");
            }
        }

        private bool _xboxOutput;
        public bool XboxOutput
        {
            get { return _xboxOutput; }
            set
            {
                if (value == _xboxOutput) return;
                _xboxOutput = value;
                RaisePropertyChangedEvent("XboxOutput");
                _currentDeviceID = (uint)_currentXboxDevice + 1000;
                if (_xboxOutput) AcquireDevice();
            }
        }

        public int CurrentXboxDevice
        {
            get
            {
                if (_currentXboxDevice == 0) _currentXboxDevice = XboxDevices.FirstOrDefault();
                return _currentXboxDevice;
            }
            set
            {
                _currentXboxDevice = value;
                RaisePropertyChangedEvent("CurrrentXBoxDevice");
                _xboxOutput = true;
                _currentDeviceID = (uint)(value + 1000);
                AcquireDevice();
                RaisePropertyChangedEvent("DecoupledOutput");
                RaisePropertyChangedEvent("CoupledOutput");
                RaisePropertyChangedEvent("KeyboardOutput");
            }
        }

        private bool _xboxOutputButtonIsEnabled;
        public bool XboxOutputButtonIsEnabled
        {
            get { return _xboxOutputButtonIsEnabled; }
            set
            {
                _xboxOutputButtonIsEnabled = value;
                RaisePropertyChangedEvent("XboxOutputButtonIsEnabled");
            }
        }

        public bool VJoyOutput
        {
            get { return _vJoyOutput; }
            set
            {
                if (value == _vJoyOutput) return;
                _vJoyOutput = value;
                RaisePropertyChangedEvent("VJoyOutput");
                _currentDeviceID = (uint)_currentVJoyDevice;
                if (_vJoyOutput) AcquireDevice();
            }
        }

        public int CurrentVJoyDevice
        {
            get
            {
                if (_currentVJoyDevice == 0) _currentVJoyDevice = VJoyDevices.FirstOrDefault();
                return _currentVJoyDevice;
            }
            set
            {
                _currentVJoyDevice = value;
                RaisePropertyChangedEvent("CurrrentVJoyDevice");
                if (!_vJoyOutput) VJoyOutput = true;
                else { _currentDeviceID = (uint)value; }
                RaisePropertyChangedEvent("DecoupledOutput");
                RaisePropertyChangedEvent("CoupledOutput");
                RaisePropertyChangedEvent("KeyboardOutput");
            }
        }

        private bool _vJoyOutputButtonIsEnabled;
        public bool VJoyOutputButtonIsEnabled
        {
            get { return _vJoyOutputButtonIsEnabled; }
            set
            {
                _vJoyOutputButtonIsEnabled = value;
                RaisePropertyChangedEvent("VJoyOutputButtonIsEnabled");
            }
        }

        public bool CoupledOutput
        {
            get { return Model.Mode != SimulatorMode.ModeJoystickDecoupled; }
            set
            {
                if (value)
                {
                    Model.UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
                    RaisePropertyChangedEvent("CoupledOutput");
                }
            }
        }

        public bool DecoupledOutput
        {
            get { return Model.Mode == SimulatorMode.ModeJoystickDecoupled; }
            set
            {
                if (value)
                {
                    Model.UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
                    RaisePropertyChangedEvent("DecoupledOutput");
                }
            }
        }

        private bool _settingsContextMenuOpen = false;
        public bool SettingsContextMenuOpen
        {
            get { return _settingsContextMenuOpen; }
            set
            {
                _settingsContextMenuOpen = value;
                RaisePropertyChangedEvent("SettingsContextMenuOpen");
            }
        }

        public IEnumerable<int>XboxDevices
        {
            get { return _xboxDevices; }
        }

        public IEnumerable<int> VJoyDevices
        {
            get { return _vJoyDevices; }
        }

        private readonly BusinessModel Model;
        public SettingsWindowViewModel(BusinessModel model)
        {
            Model = model;
            _xboxDevices = new ObservableCollection<int>(Model.CurrentDevices.Where(x => x > 1000 && x < 1005).Select(x => x - 1000));
            _xboxOutputButtonIsEnabled = _xboxDevices.Count > 0;
            _vJoyDevices = new ObservableCollection<int>(Model.CurrentDevices.Where(x => x > 0 && x < 17));
            _vJoyOutputButtonIsEnabled = _vJoyDevices.Count > 0;
            KeyboardOutput = Model.Mode == SimulatorMode.ModeWASD;
        }
        
        // public Commands return ICommand using DelegateCommand class
        // and are backed by private methods

        public ICommand XboxOutputCommand
        {
            get { return new DelegateCommand(AcquireDevice); }
        }

        public ICommand VJoyOutputCommand
        {
            get { return new DelegateCommand(AcquireDevice); }
        }

        public ICommand UnplugAllXboxCommand
        {
            get { return new DelegateCommand(UnplugAllXbox); }
        }

        public ICommand SettingsMenuCommand
        {
            get { return new DelegateCommand(SettingsMenu); }
        }

        private async void AcquireDevice()
        {
            bool res = await Model.AcquireVDevAsync(_currentDeviceID);
            if (!res)
            {
                _currentDeviceID = _previousDeviceID;
                //Model.AcquireVDev(_previousDeviceID);
            }
             
            RaisePropertyChangedEvent("DecoupledOutput");
            RaisePropertyChangedEvent("CoupledOutput");
            RaisePropertyChangedEvent("KeyboardOutput");

            _previousDeviceID = _currentDeviceID;
        }

        private async void UnplugAllXbox()
        {
            KeyboardOutput = true;
  
            for (int i = 1; i < 5; i++) _xboxDevices.Add(i);
            _currentXboxDevice = 0;
            RaisePropertyChangedEvent("CurrentXboxDevice");

            await Model.UnplugAllXboxAsync(silent:true);
            _xboxDevices.Clear();
            foreach (var item in Model.CurrentDevices.Where(x => x > 1000 && x < 1005).Select(x => x - 1000)) _xboxDevices.Add(item);
            _currentXboxDevice = 0;
            RaisePropertyChangedEvent("CurrentXboxDevice");

            XboxOutputButtonIsEnabled = _xboxDevices.Count > 0;
        }

        private void SettingsMenu()
        {
            SettingsContextMenuOpen = !SettingsContextMenuOpen;
        }
    }
}
