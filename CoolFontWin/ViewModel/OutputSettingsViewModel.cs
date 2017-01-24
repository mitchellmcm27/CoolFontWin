﻿using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using CFW.Business;
using System.Diagnostics;
using System.Windows;
using Ookii.Dialogs;
using System.Reactive;

namespace CFW.ViewModel
{
    public class OutputSettingsViewModel : ReactiveObject
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Expose model properties that the view can bind to
        // Raise propertychangedevent on set

        // Input devices
        readonly ObservableAsPropertyHelper<uint> _CurrentDeviceID;
        private uint CurrentDeviceID
        {
            get { return _CurrentDeviceID.Value; }
        }

        readonly ObservableAsPropertyHelper<string> _XboxLedImage;
        public string XboxLedImage
        {
            get { return _XboxLedImage.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _KeyboardOutput;
        public bool KeyboardOutput
        {
            get { return _KeyboardOutput.Value; }
        }

        private string _Keybind;
        public string Keybind
        {
            get { return _Keybind; }
            set
            {
                value = value.ToUpper();
                this.RaiseAndSetIfChanged(ref _Keybind, value);
            }
        }

        private bool _KeybindChanged;
        public bool KeybindChanged
        {
            get { return _KeybindChanged; }
            set { this.RaiseAndSetIfChanged(ref _KeybindChanged, value); }
        }

        readonly ObservableAsPropertyHelper<bool> _XboxOutput;
        public bool XboxOutput
        {
            get { return _XboxOutput.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _XboxOutputButtonIsEnabled;
        public bool XboxDevicesExist
        {
            get { return _XboxOutputButtonIsEnabled.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _VJoyOutput;
        public bool VJoyOutput
        {
            get { return _VJoyOutput.Value; }
        }

        private int? _CurrentVJoyDevice;
        public int? CurrentVJoyDevice // nullable
        {
            get { return _CurrentVJoyDevice; }
            set
            {
                this.RaiseAndSetIfChanged(ref _CurrentVJoyDevice, value);
            }
        }

        private bool _VJoyDeviceChanged;
        public bool VJoyDeviceChanged
        {
            get { return _VJoyDeviceChanged; }
            set { this.RaiseAndSetIfChanged(ref _VJoyDeviceChanged, value); }
        }

        readonly ObservableAsPropertyHelper<bool> _vJoyOutputButtonIsEnabled;
        public bool VJoyDevicesExist
        {
            get { return _vJoyOutputButtonIsEnabled.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _NoVJoyDevices;
        public bool NoVJoyDevices
        {
            get { return _NoVJoyDevices.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _NoXboxDevices;
        public bool NoXboxDevices
        {
            get { return _NoXboxDevices.Value; }
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

        readonly ObservableAsPropertyHelper<List<int>> _VJoyDevices;
        public List<int> VJoyDevices
        {
            get { return _VJoyDevices.Value; }
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

        // Vive controller
        private ReactiveList<string> _Procs;
        public ReactiveList<string> RunningProcs { get; set; }
        public string SelectedProc { get; set; }

        private readonly DeviceManager DeviceManager;
        private readonly DNSNetworkService DnsServer;
        private readonly ScpVBus ScpVBus;

        private ObservableAsPropertyHelper<SimulatorMode> _Mode;
        private SimulatorMode Mode { get { return (_Mode.Value); } }

        public OutputSettingsViewModel(AppBootstrapper bs)
        {
            DeviceManager = bs.DeviceManager;
            DnsServer = bs.DnsServer;
            ScpVBus = bs.ScpVBus;

            this.WhenAnyValue(x => x.ScpVBus.InstallSuccess)
                .Where(x => x)
                .Subscribe(x => 
                {
                    log.Info("ScpVBis Installation Succeeded");
                    ShowRestartMessage();
                });

            // Current vDevice ID
            this.WhenAnyValue(x => x.DeviceManager.VDevice.Id)
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ToProperty(this, x => x.CurrentDeviceID, out _CurrentDeviceID);

            // Keybind
            this.WhenAnyValue(x => x.DeviceManager.VDevice.Keybind)
                .Do(x => Keybind = x)
                .Subscribe();

            // Cascade down Mode and Current Device ID
            this.WhenAnyValue(x => x.DeviceManager.VDevice.Mode)
                .ToProperty(this, x => x.Mode, out _Mode);

            this.WhenAnyValue(x => x.Mode, m => m == SimulatorMode.ModeWASD)
                .ToProperty(this, x => x.KeyboardOutput, out _KeyboardOutput);

            this.WhenAnyValue(x => x.Mode, x => x.CurrentDeviceID, (m, id) =>
                (m == SimulatorMode.ModeJoystickCoupled || m == SimulatorMode.ModeJoystickDecoupled) && id > 1000 && id < 1005)
                .ToProperty(this, x => x.XboxOutput, out _XboxOutput);

            this.WhenAnyValue(x => x.Mode, x => x.CurrentDeviceID, (m, id) =>
                (m == SimulatorMode.ModeJoystickCoupled || m == SimulatorMode.ModeJoystickDecoupled) && id < 17 && id > 0)
                .ToProperty(this, x => x.VJoyOutput, out _VJoyOutput);

            this.WhenAnyValue(x => x.Mode, m => m == SimulatorMode.ModeJoystickCoupled || m == SimulatorMode.ModeWASD)
                .ToProperty(this, x => x.CoupledOutput, out _CoupledOutput);

            this.WhenAnyValue(x => x.CoupledOutput, x => x ? "Coupled" : "Decoupled")
                .ToProperty(this, x => x.CoupledText, out _CoupledText);

            // Filter list of enabled devices to VJoyDevices list
            this.WhenAnyValue(x => x.DeviceManager.VDevice.EnabledDevices, x => x.Where(y => y > 0 && y < 17).ToList())
                 .ToProperty(this, x => x.VJoyDevices, out _VJoyDevices);

            // Set *DevicesExist property based on *Devices lists having values
            this.WhenAnyValue(x => x.VJoyDevices, x => x.Count > 0)
                .ToProperty(this, x => x.VJoyDevicesExist, out _vJoyOutputButtonIsEnabled);

            this.WhenAnyValue(x => x.DeviceManager.VDevice.EnabledDevices, x => x.Where(y => y > 1000 && y < 1005).Count() > 0)
                .ToProperty(this, x => x.XboxDevicesExist, out _XboxOutputButtonIsEnabled);

            // No*Devices is the inverse of *DevicesExist
            this.WhenAnyValue(x => x.VJoyDevicesExist, x => !x)
                .ToProperty(this, x => x.NoVJoyDevices, out _NoVJoyDevices);

            this.WhenAnyValue(x => x.XboxDevicesExist, x => !x)
                .ToProperty(this, x => x.NoXboxDevices, out _NoXboxDevices);

            // When the user changes VJoy ID, but hasn't acquired it yet, set VJoyDevice changed to true
            this.WhenAnyValue(x => x.CurrentVJoyDevice)
                .Skip(2)
                .Do(_ => VJoyDeviceChanged = true)
                .Subscribe();

            // When CFW changes the current device ID to a vJoy id (1-16), set VJoyDeviceChanged to false
            this.WhenAnyValue(x => x.CurrentDeviceID, x => x > 0 && x < 17)
                .Do(_ => VJoyDeviceChanged = false)
                .Subscribe();

            // Handling user changing Keybind in a similar way
            this.WhenAnyValue(x => x.Keybind)
                .Skip(1) // skip initial value set by model
                .Do(x => KeybindChanged = !string.IsNullOrWhiteSpace(x))
                .Subscribe();

            // Xbox controller LED image
            this.WhenAnyValue(x => x.CurrentDeviceID)
                .Select(x => XboxLedImagePath((int)x))
                .ToProperty(this, x => x.XboxLedImage, out _XboxLedImage);

            // Commands
            //
            KeyboardMode = ReactiveCommand.CreateFromTask(async _ =>
            {
                await Task.Run(() => DeviceManager.RelinquishCurrentDevice(silent: true));
                await UpdateMode((int)SimulatorMode.ModeWASD);
            });

            ChangeKeybind = ReactiveCommand.CreateFromTask(ChangeKeybindImpl);

            VJoyMode = ReactiveCommand.CreateFromTask<int>(async (id) =>
            {
                await Task.Run(() => DeviceManager.AcquireVDev((uint)id));
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

            AcquireVJoyDevice = ReactiveCommand.CreateFromTask(AcquireVJoyDeviceImpl);

            XboxMode = ReactiveCommand.CreateFromTask(async _ =>
            {
                await Task.Run(() => DeviceManager.AcquireVDev(0));
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

            InterceptXInputDevice = ReactiveCommand.CreateFromTask<bool>(async wasChecked =>
            {
                if (wasChecked)
                {
                    await Task.Run(() => DeviceManager.AcquireXInputDevice());
                }
                else DeviceManager.InterceptXInputDevice = false;
            });

            AcquireDevice = ReactiveCommand.CreateFromTask(async _ => await Task.Run(() => DeviceManager.AcquireVDev(CurrentDeviceID)));

            CoupledDecoupled = ReactiveCommand.CreateFromTask(CoupledDecoupledImpl);

            VJoyInfo = ReactiveCommand.CreateFromTask(_ => Task.Run(() => VJoyInfoDialog.ShowVJoyInfoDialog()));
            VXboxInfo = ReactiveCommand.CreateFromTask(_ => Task.Run(() => ShowScpVbusDialog()));
            VXboxInfo.ThrownExceptions.Subscribe(ex => log.Error(ex.Message));

            JoyCplCommand = ReactiveCommand.Create(() => Process.Start("joy.cpl"));
            UnplugAllXboxCommand = ReactiveCommand.CreateFromTask(UnplugAllXboxImpl);

            _Procs = new ReactiveList<string>();
            RunningProcs = new ReactiveList<string>();
            RefreshProcs = ReactiveCommand.Create(() =>  
            {
                RunningProcs.Clear();
                foreach (Process proc in Process.GetProcesses())
                {
                    if (proc.MainWindowTitle.Length>0) RunningProcs.Add(proc.ProcessName);
                }
            });
            RefreshProcs.ThrownExceptions.Subscribe(ex => log.Error("RefreshProcs: " + ex.Message));

            int start = 0;
            Observable
                .Generate(start, x => x < 1, x=>x+1, x => Unit.Default)
                .InvokeCommand(this, x => x.RefreshProcs);

            InjectProc = ReactiveCommand.CreateFromTask(InjectProcImpl);
        }

        public ReactiveCommand KeyboardMode { get; set; }
        public ReactiveCommand VJoyMode { get; set; }
        public ReactiveCommand XboxMode { get; set; }
        public ReactiveCommand AcquireDevice { get; set; }
        public ReactiveCommand AddRemoveSecondaryDevice { get; set; }
        public ReactiveCommand PlayPause { get; set; }
        public ReactiveCommand CoupledDecoupled { get; set; }
        public ReactiveCommand InterceptXInputDevice { get; set; }
        public ReactiveCommand StartKeybind { get; set; }
        public ReactiveCommand ChangeKeybind { get; set; }
        public ReactiveCommand JoyCplCommand { get; set; }
        public ReactiveCommand UnplugAllXboxCommand { get; set; }
        public ReactiveCommand AcquireVJoyDevice { get; set; }

        public ReactiveCommand VJoyInfo { get; set; }
        public ReactiveCommand VXboxInfo { get; set; }

        public ReactiveCommand RefreshProcs { get; set; }
        public ReactiveCommand InjectProc { get; set; }

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

        private async Task ChangeKeybindImpl()
        {
            await Task.Run(() => DeviceManager.VDevice.SetKeybind(Keybind));
            KeybindChanged = false;
        }

        private async Task UpdateMode(int mode)
        {
            await Task.Run(() => DeviceManager.TryMode(mode));
        }

        private async Task UnplugAllXboxImpl()
        {
            await Task.Run(() => DeviceManager.ForceUnplugAllXboxControllers());
            await UpdateMode((int)SimulatorMode.ModeWASD);
        }

        private async Task AcquireVJoyDeviceImpl()
        {
            uint id = (uint)(CurrentVJoyDevice ?? VJoyDevices.FirstOrDefault());

            await Task.Run(() => DeviceManager.AcquireVDev(id));
            await Task.Run(() => DeviceManager.TryMode(CoupledOutput ? (int)SimulatorMode.ModeJoystickCoupled : (int)SimulatorMode.ModeJoystickDecoupled));
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

        public async Task ShowScpVbusDialog()
        {
            var taskDialog = new TaskDialog();
            taskDialog.Width = 200;
            taskDialog.AllowDialogCancellation = true;

            taskDialog.WindowTitle = "An important component was not installed";
            taskDialog.MainIcon = TaskDialogIcon.Shield;

            taskDialog.MainInstruction = "ScpVBus failed to install";
            taskDialog.Content = "Xbox controller emulation requires ScpVBus.\n";
            taskDialog.Content += "ScpVBus is installed with PocketStrafe but it seems to have failed. You can try again here, or continue using only keyboard/joystick emulation.";

            taskDialog.ButtonStyle = TaskDialogButtonStyle.Standard;

            var customButton = new TaskDialogButton(ButtonType.Custom);
            customButton.CommandLinkNote = "Virtual Xbox controller driver";
            customButton.Text = "Install ScpVBus";
            customButton.Default = true;

            taskDialog.Buttons.Add(customButton);
            taskDialog.Buttons.Add(new TaskDialogButton(ButtonType.Close));

            await Task.Run(()=>
            {
                TaskDialogButton res = taskDialog.Show(); // Windows Vista and later
                if (res != null && res.ButtonType == ButtonType.Custom)
                {
                    ScpVBus.Install();
                }
            });
        }

        private async Task InjectProcImpl()
        {
            await Task.Run(() => DeviceManager.InjectControllerIntoProcess(this.SelectedProc + ".exe"));
        }

        public void ShowRestartMessage()
        {
            MessageBox.Show("Restart PocketStrafe PC to use vXbox","Success!",MessageBoxButton.OK,MessageBoxImage.Information);
        }


    }
}