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
    public class Presenter : ObservableObject
    {
        // Expose model properties that the view can bind to
        // Raise propertychangedevent on set
    
        private readonly ObservableCollection<string> _modes = new ObservableCollection<string>(CFWMode.GetDescriptions());
        private ObservableCollection<int> _xboxDevices
        {
            get { return new ObservableCollection<int>(Model.CurrentDevices.Where(x => x>1000 && x<1005).Select(x=>x-1000)); }
        }
        private ObservableCollection<int> _vJoyDevices
        {
            get { return new ObservableCollection<int>(Model.CurrentDevices.Where(x => x>0 && x<17)); }
        }

        private int _currentMode;
        private bool _xboxOutput=false;
        private bool _vJoyOutput=false;
        private int _currentXboxDevice=0;
        private int _currentVJoyDevice=0;

        private uint _currentDeviceID;
        public uint CurrentDeviceID
        {
            get { return _currentDeviceID; }
            set
            {
                if (Model.SharedDeviceManager.AcquireVDev(value)) _currentDeviceID = value;
            }
        }
    
        public IEnumerable<string> Modes
        {
            get { return _modes; }
        }

        public int CurrentMode
        {
            get { return (int)Model.SharedDeviceManager.Mode; }
            set
            {
                if (DeviceManager.Instance.TryMode(value)) 
                { 
                    _currentMode = value;
                    RaisePropertyChangedEvent("CurrentMode");
                }
                
            }
        }

        public bool SecondaryDevice
        {
            get { return Model.DeviceNames.Count>1; }
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
            get { return Model.SharedDeviceManager.InterceptXInputDevice; }
            set
            {
                Model.SharedDeviceManager.InterceptXInputDevice = value;
                RaisePropertyChangedEvent("XboxDevice");
            }
        }

        public bool XboxOutput
        {
            get { return _xboxOutput; }
            set
            {
                if (value == _xboxOutput) return;
                _xboxOutput = value;
                RaisePropertyChangedEvent("XboxOutput");
                _currentDeviceID = (uint)_currentXboxDevice+1000;
                if (_xboxOutput) AcquireDevice();
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
                CurrentDeviceID = (uint)value;        
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
                if (!_xboxOutput) XboxOutput = true;
                CurrentDeviceID = (uint)(value+1000);
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
        public Presenter(BusinessModel model)
        {
            Model = model;
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
        private void AcquireDevice()
        {
            Model.SharedDeviceManager.AcquireVDev(_currentDeviceID);
        }

        private void UnplugAllXbox()
        {
            Model.SharedDeviceManager.ForceUnplugAllXboxControllers(silent:true);
        }

        private void SettingsMenu()
        {
            SettingsContextMenuOpen = !SettingsContextMenuOpen;
        }
    }
}
