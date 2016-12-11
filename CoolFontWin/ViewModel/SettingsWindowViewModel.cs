﻿using System;
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

namespace CFW.ViewModel
{
    public class SettingsWindowViewModel : ObservableObject
    {

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Expose model properties that the view can bind to
        // Raise propertychangedevent on set

        private readonly List<string> _modes = new List<string>(CFWMode.GetDescriptions());
        private List<int> _xboxDevices;       
        private List<int> _vJoyDevices;

        object lockObj = new object();

        private bool _vJoyOutput = false;
        private int _currentXboxDevice = 0;
        private int _currentVJoyDevice = 0;

        private uint _previousDeviceID;
        private uint _currentDeviceID;

        public IEnumerable<string> Modes
        {
            get { return _modes; }
        }

        private bool _secondaryDevice;
        public bool SecondaryDevice
        {
            get { return _secondaryDevice; }
            set
            {
                _secondaryDevice = value;
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

        private bool _xboxDevice;
        public bool XboxDevice
        {
            get { return _xboxDevice; }
            set
            {
                _xboxDevice = value;
                RaisePropertyChangedEvent("XboxDevice");
            }
        }

        private bool _keyboardOutput;
        public bool KeyboardOutput
        {
            get { return _keyboardOutput; }
            set
            {
                _keyboardOutput = value;
                RaisePropertyChangedEvent("KeyboardOutput");
            }
        }

        private bool _xboxOutput;
        public bool XboxOutput
        {
            get { return _xboxOutput; }
            set
            {
                _xboxOutput = value;
                if (_xboxOutput)
                {
                    _currentDeviceID = (uint)_currentXboxDevice+1000;
                }
                RaisePropertyChangedEvent("XboxOutput");

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
                XboxOutput = true;
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
                _vJoyOutput = value;
                if (_vJoyOutput)
                {
                    _currentDeviceID = (uint)_currentVJoyDevice;
                }
                RaisePropertyChangedEvent("VJoyOutput");
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
                VJoyOutput = true;
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

        private bool _coupledOutput;
        public bool CoupledOutput
        {
            get { return _coupledOutput; }
            set
            {
                _coupledOutput = value;
                if (value)
                {
                    UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
                }
            }
        }

        private bool _decoupledOutput;
        public bool DecoupledOutput
        {
            get { return _decoupledOutput; }
            set
            {
                _decoupledOutput = value;
                if (value)
                {
                    Model.UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
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

        private bool _isPaused;
        public string PauseButtonText
        {
            get { return _isPaused ? "Resume" : "Pause"; }
        }

        public string PauseButtonIcon
        {
            get { return _isPaused ? "Play" : "Pause"; }
        }

        private readonly BusinessModel Model;
        public SettingsWindowViewModel(BusinessModel model)
        {
            Model = model;
            Model.PropertyChanged += Model_PropertyChanged;
            _xboxDevice = Model.InterceptXInputDevice;
            _xboxDevices = new List<int>(Model.CurrentDevices.Where(x => x > 1000 && x < 1005).Select(x => x - 1000));
            _xboxOutputButtonIsEnabled = _xboxDevices.Count > 0;
            _vJoyDevices = new List<int>(Model.CurrentDevices.Where(x => x > 0 && x < 17));
            _vJoyOutputButtonIsEnabled = _vJoyDevices.Count > 0;

            _isPaused = Model.Mode == SimulatorMode.ModePaused;
            _keyboardOutput = Model.Mode == SimulatorMode.ModeWASD && !_isPaused;
            _xboxOutput = !_keyboardOutput && Model.CurrentDeviceID > 1000 && !_isPaused;
            _vJoyOutput = !_xboxOutput && !_keyboardOutput && !_isPaused;
            _coupledOutput = Model.Mode == SimulatorMode.ModeJoystickCoupled && !_isPaused;
            _decoupledOutput = Model.Mode == SimulatorMode.ModeJoystickDecoupled && !_isPaused;

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

        public ICommand InterceptXInputDeviceCommand
        {
            get { return new AwaitableDelegateCommand(IntercpetXInputDeviceAsync); }
        }

        public ICommand UnplugAllXboxCommand
        {
            get { return new AwaitableDelegateCommand(UnplugAllXboxAsync); }
        }

        public ICommand SettingsMenuCommand
        {
            get { return new DelegateCommand(SettingsMenu); }
        }

        public ICommand CurrentXboxDeviceChangedCommand
        {
            get
            {
                return AcquireDeviceAsyncCommand_NotCodeTriggered;
            }
        }

        public ICommand CurrentVJoyDeviceChangedCommand
        {
            get
            {
                return AcquireDeviceAsyncCommand_NotCodeTriggered;
            }
        }

        private ICommand AcquireDeviceAsyncCommand
        {
            get { return new AwaitableDelegateCommand(AcquireDeviceAsync); }
        }

        private ICommand AcquireDeviceAsyncCommand_NotCodeTriggered
        {
            get { return new AwaitableDelegateCommand(AcquireDeviceAsync_NotCodeTriggered); }
        }

        public ICommand PlayPauseCommand
        {
            get { return new AwaitableDelegateCommand(PlayPause); }
        }

        public ICommand JoyCplCommand
        {
            get { return new DelegateCommand(() => Process.Start("joy.cpl")); }
        }

        bool _codeTriggered = false;
        private async Task AcquireDeviceAsync_NotCodeTriggered()
        {
            if (!_codeTriggered) await AcquireDeviceAsync();            
        }

        private async Task AcquireDeviceAsync()
        {
            log.Info("AcquireDeviceAsync Task:");
            if (_keyboardOutput)
            {
                log.Info("Keyboard output TRUE, update mode to keyboard");
                await UpdateMode((int)SimulatorMode.ModeWASD);
            }
            else
            {
                log.Info("Keyboard output FALSE, acquire vdev");
                await Model.AcquireVDevAsync(_currentDeviceID);

                if (_decoupledOutput)
                {
                    log.Info("Update mode to decoupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickDecoupled);
                }
                else
                { 
                    log.Info("Update mode to coupled");
                    await UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
                }

            }
   
            
        }

        private int previousMode;
        private async Task PlayPause()
        {
            _isPaused = !_isPaused;
            RaisePropertyChangedEvent("PauseButtonText");
            RaisePropertyChangedEvent("PauseButtonIcon");
            if (_isPaused)
            {
                previousMode = (int)Model.Mode;
                await UpdateMode((int)SimulatorMode.ModePaused);
            }
            else await UpdateMode(previousMode);
        }
        private async Task IntercpetXInputDeviceAsync()
        {
            await Task.Run(() => Model.InterceptXInputDevice = _xboxDevice);
        }

        private async Task UpdateMode(int mode)
        {
            await Model.UpdateModeAsync(mode);
        }

        private async Task UnplugAllXboxAsync()
        {
            _codeTriggered = true;
            await Model.UnplugAllXboxAsync(silent:true);
            _codeTriggered = false;
        }

        private void SettingsMenu()
        {
            SettingsContextMenuOpen = !SettingsContextMenuOpen;
        }    

        // Notifications
        private void Model_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DeviceNames")
            {   
                _secondaryDevice = Model.DeviceNames.Count > 1;
                RaisePropertyChangedEvent("SecondaryDevice");
            }
            else if (e.PropertyName == "InterceptXInputDevice")
            {
                XboxDevice = Model.InterceptXInputDevice;
            }
            else if (e.PropertyName == "Mode")
            {
                _isPaused = Model.Mode == SimulatorMode.ModePaused;
                _keyboardOutput = Model.Mode == SimulatorMode.ModeWASD && !_isPaused;
                _xboxOutput = !_keyboardOutput && Model.CurrentDeviceID > 1000 && !_isPaused; 
                _vJoyOutput = !_xboxOutput && !_keyboardOutput && !_isPaused; 

                _coupledOutput = Model.Mode == SimulatorMode.ModeJoystickCoupled && !_isPaused;
                _decoupledOutput = Model.Mode == SimulatorMode.ModeJoystickDecoupled && !_isPaused;
 
                RaisePropertyChangedEvent("VJoyOutput");
                RaisePropertyChangedEvent("XboxOutput");
                RaisePropertyChangedEvent("KeyboardOutput");
                RaisePropertyChangedEvent("CoupledOutput");
                RaisePropertyChangedEvent("DecoupledOutput");
                RaisePropertyChangedEvent("PauseButtonText");
                RaisePropertyChangedEvent("PauseButtonIcon");
            }
            else if (e.PropertyName == "CurrentDeviceID")
            {
                _currentDeviceID = Model.CurrentDeviceID;
                if (_currentDeviceID > 0 && _currentDeviceID < 17) CurrentVJoyDevice = (int)_currentDeviceID;
                else if (_currentDeviceID > 1000 && _currentDeviceID < 1005) CurrentXboxDevice = (int)_currentDeviceID-1000;
            }
            else if (e.PropertyName == "CurrentDevices")
            {
                _xboxDevices = new List<int>(Model.CurrentDevices.Where(x => x > 1000 && x < 1005).Select(x => x - 1000));
                _vJoyDevices = new List<int>(Model.CurrentDevices.Where(x => x > 0 && x < 17));

                _xboxOutputButtonIsEnabled = _xboxDevices.Count > 0;
                RaisePropertyChangedEvent("XboxOutputButtonIsEnabled");

                _vJoyOutputButtonIsEnabled = _vJoyDevices.Count > 0;
                RaisePropertyChangedEvent("VJoyOutputButtonIsEnabled");

                RaisePropertyChangedEvent("XboxDevices");
                RaisePropertyChangedEvent("VJoyDevices");         
            }
        }
    }
}
