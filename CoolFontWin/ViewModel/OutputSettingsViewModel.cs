using log4net;
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
using System.Reactive.Concurrency;
using System.Threading;
using CFW.Business.Extensions;

namespace CFW.ViewModel
{
    public class OutputSettingsViewModel : ReactiveObject
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        // Expose model properties that the view can bind to
        // Raise propertychangedevent on set

        private ObservableAsPropertyHelper<SimulatorMode> _Mode;
        private SimulatorMode Mode { get { return (_Mode.Value); } }

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

        string _Keybind;
        public string Keybind
        {
            get { return _Keybind; }
            set
            {
                value = value.ToUpper();
                this.RaiseAndSetIfChanged(ref _Keybind, value);
            }
        }

        bool _KeybindChanged;
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

        readonly ObservableAsPropertyHelper<bool> _XboxDevicesExist;
        public bool XboxDevicesExist
        {
            get { return _XboxDevicesExist.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _VJoyOutput;
        public bool VJoyOutput
        {
            get { return _VJoyOutput.Value; }
        }

        readonly ObservableAsPropertyHelper<bool> _VrOutput;
        public bool VrOutput
        {
            get { return _VrOutput.Value; }
        }

        int? _CurrentVJoyDevice;
        public int? CurrentVJoyDevice // nullable
        {
            get { return _CurrentVJoyDevice; }
            set
            {
                this.RaiseAndSetIfChanged(ref _CurrentVJoyDevice, value);
            }
        }

        bool _VJoyDeviceChanged;
        public bool VJoyDeviceChanged
        {
            get { return _VJoyDeviceChanged; }
            set { this.RaiseAndSetIfChanged(ref _VJoyDeviceChanged, value); }
        }

        readonly ObservableAsPropertyHelper<bool> _vJoyDevicesExist;
        public bool VJoyDevicesExist
        {
            get { return _vJoyDevicesExist.Value; }
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

        static readonly List<string> _SupportedVrSystems = new List<string> { "SteamVR" };
        public List<string> SupportedVrSystems { get { return _SupportedVrSystems; } }

        int _SelectedVrSystemIndex = 0;
        public int SelectedVrSystemIndex
        {
            get { return _SelectedVrSystemIndex; }
            set { this.RaiseAndSetIfChanged(ref _SelectedVrSystemIndex, value); }
        }
        
        public ReactiveList<string> RunningProcs { get; set; }
        public ReactiveList<string> InjectedProcs { get; set; }
        string _SelectedProc;
        public string SelectedProc
        {
            get { return _SelectedProc; }
            set { this.RaiseAndSetIfChanged(ref _SelectedProc, value); }
        }

        readonly ObservableAsPropertyHelper<bool> _ProcsRefreshing;
        public bool ProcsRefreshing
        {
            get { return _ProcsRefreshing.Value; }
        }
        readonly ObservableAsPropertyHelper<bool> _ProcsDoneRefreshing;
        public bool ProcsDoneRefreshing
        {
            get { return _ProcsDoneRefreshing.Value; }
        }

        bool _Injected;
        public bool Injected
        {
            get { return _Injected; }
            set { this.RaiseAndSetIfChanged(ref _Injected, value); }
        }

        bool _InjectedIntoSelected;
        public bool InjectedIntoSelected
        {
            get { return _InjectedIntoSelected; }
            set { this.RaiseAndSetIfChanged(ref _InjectedIntoSelected, value); }
        }
        List<string> _ControllerHand = Enum.GetNames(typeof(Valve.VR.EVRHand)).ToList();
        public List<string> ControllerHand { get { return _ControllerHand; } }

        List<string> _ControllerTouch = Enum.GetNames(typeof(Valve.VR.EVRButtonType)).ToList();
        public List<string> ControllerTouch { get { return _ControllerTouch; } }

        int _SelectedControllerTouchIndex;
        public int SelectedControllerTouchIndex
        {
            get { return _SelectedControllerTouchIndex; }
            set { this.RaiseAndSetIfChanged(ref _SelectedControllerTouchIndex, value); }
        }

        int _SelectedControllerHandIndex;
        public int SelectedControllerHandIndex
        {
            get { return _SelectedControllerHandIndex; }
            set { this.RaiseAndSetIfChanged(ref _SelectedControllerHandIndex, value); }
        }

        // must keep these 2 lists in sync
        List<Valve.VR.EVRButtonId> _ViveControllerButtonId = new List<Valve.VR.EVRButtonId> {
            Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad,
            Valve.VR.EVRButtonId.k_EButton_Grip,
            Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger
        };
        List<string> _ViveControllerButton = new List<string> { "Touchpad", "Grip", "Trigger" };
        public  List<string> ViveControllerButton { get { return _ViveControllerButton; } }

        int _SelectedViveControllerButtonIndex;
        public int SelectedViveControllerButtonIndex
        {
            get { return _SelectedViveControllerButtonIndex; }
            set { this.RaiseAndSetIfChanged(ref _SelectedViveControllerButtonIndex, value); }
        }

        bool _ViveBindingsChanged;
        public bool ViveBindingsChanged
        {
            get { return _ViveBindingsChanged; }
            set { this.RaiseAndSetIfChanged(ref _ViveBindingsChanged, value); }
        }


        //OVRPlugin.dll
        string _HookedDllName = "openvr_api.dll";
        public string HookedDllName
        {
            get { return _HookedDllName; }
            set { this.RaiseAndSetIfChanged(ref _HookedDllName, value); }
        }

        string _HookedFnName = "GetControllerStateWithPose";
        public string HookedFnName
        {
            get { return _HookedFnName; }
            set { this.RaiseAndSetIfChanged(ref _HookedFnName, value); }
        }


        // Constructor

        private readonly DeviceManager DeviceManager;
        private readonly DNSNetworkService DnsServer;
        private readonly ScpVBus ScpVBus;

        public OutputSettingsViewModel(PocketStrafe ps)
        {
            DeviceManager = ps.DeviceManager;
            DnsServer = ps.DnsServer;
            ScpVBus = ps.ScpVBus;

            this.WhenAnyValue(x => x.ScpVBus.InstallSuccess)
                .Where(x => x)
                .Subscribe(x => 
                {
                    log.Info("ScpVBis Installation Succeeded");
                    ShowRestartMessage();
                });

            // Current vDevice ID
            this.WhenAnyValue(x => x.DeviceManager.VDevice.Id)
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

            this.WhenAnyValue(x => x.Mode, m => m == SimulatorMode.ModeSteamVr)
                .ToProperty(this, x => x.VrOutput, out _VrOutput);

            this.WhenAnyValue(x => x.CoupledOutput, x => x ? "Coupled" : "Decoupled")
                .ToProperty(this, x => x.CoupledText, out _CoupledText);

            // Filter list of enabled devices to VJoyDevices list
            this.WhenAnyValue(x => x.DeviceManager.VDevice.EnabledDevices, x => x.Where(y => y > 0 && y < 17).ToList())
                 .ToProperty(this, x => x.VJoyDevices, out _VJoyDevices);

            // Set *DevicesExist property based on *Devices lists having values
            this.WhenAnyValue(x => x.VJoyDevices, x => x.Count > 0)
                .ToProperty(this, x => x.VJoyDevicesExist, out _vJoyDevicesExist);

            this.WhenAnyValue(x => x.DeviceManager.VDevice.EnabledDevices, x => x.Where(y => y > 1000 && y < 1005).Count() > 0)
                .ToProperty(this, x => x.XboxDevicesExist, out _XboxDevicesExist);

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

            VrMode = ReactiveCommand.CreateFromTask(async _ =>
            {
                await Task.Run(() => DeviceManager.RelinquishCurrentDevice(silent: true));
                await UpdateMode((int)SimulatorMode.ModeSteamVr);
            });

            AcquireDevice = ReactiveCommand.CreateFromTask(async _ => 
            {
                await Task.Run(() => DeviceManager.AcquireVDev(CurrentDeviceID));
            });
            CoupledDecoupled = ReactiveCommand.CreateFromTask(CoupledDecoupledImpl);
            VJoyInfo = ReactiveCommand.CreateFromTask(async _ => await Task.Run(() => VJoyInfoDialog.ShowVJoyInfoDialog()));
            VXboxInfo = ReactiveCommand.CreateFromTask(async _ => await Task.Run(() => ShowScpVbusDialog()));
            VXboxInfo.ThrownExceptions.Subscribe(ex => log.Error(ex.Message));

            SteamVRInfo = ReactiveCommand.CreateFromTask(async _ => await Task.Run(() => ShowSteamVrDialog()));

            JoyCplCommand = ReactiveCommand.CreateFromTask(async _=> await Task.Run(()=>Process.Start("joy.cpl")));
            UnplugAllXboxCommand = ReactiveCommand.CreateFromTask(UnplugAllXboxImpl);

            RunningProcs = new ReactiveList<string>();

            RefreshProcs = ReactiveCommand.CreateFromTask(async () =>
            {
                RunningProcs.Clear();
                var procs = await Task.Run(() =>
                    {
                        return DeviceManager.GetProcesses().Distinct().OrderBy(x => x);
                    });
                foreach (string proc in procs)
                {
                    RunningProcs.Add(proc);
                }
            });
            RefreshProcs.ThrownExceptions
                .Subscribe(ex => log.Error("RefreshProcs: " + ex));

            RefreshProcs.IsExecuting
                .Throttle(TimeSpan.FromMilliseconds(250))
                .ToProperty(this, x => x.ProcsRefreshing, out _ProcsRefreshing);

            this.WhenAnyValue(x => x.ProcsRefreshing, x => !x)
                .ToProperty(this, x => x.ProcsDoneRefreshing, out _ProcsDoneRefreshing);

            Observable.Return(Unit.Default)
                .InvokeCommand(RefreshProcs);

            InjectedProcs = new ReactiveList<string>();
            InjectedProcs.ItemsAdded
                .Select(_ => InjectedProcs.Contains(SelectedProc))
                .Do(x => InjectedIntoSelected = x)
                .Subscribe();

            var canInject = this
                .WhenAnyValue(x => x.SelectedProc, x => !string.IsNullOrEmpty(x));
            
            InjectProc = ReactiveCommand.CreateFromTask(InjectProcImpl, canInject);

            InjectProc.ThrownExceptions
                .Do(ex => MessageBox.Show("Error ", ex.Message, MessageBoxButton.OK))
                .Subscribe();

            this.WhenAnyValue(
                x => x.SelectedControllerTouchIndex,
                x => x.SelectedViveControllerButtonIndex, 
                x => x.SelectedControllerHandIndex, 
                x => x.Injected,
                    (touch, but, hand, injected) => injected)
                .Do(x => ViveBindingsChanged = x)
                .Subscribe();

            UpdateHookInterface = ReactiveCommand.CreateFromTask(UpdateHookInterfaceImpl);
            UpdateHookInterface.ThrownExceptions
                .Do(_ => ViveBindingsChanged = true)
                .Subscribe(ex => log.Debug("UpdateHookInterface error: " + ex));

        }

        public ReactiveCommand KeyboardMode { get; set; }
        public ReactiveCommand VJoyMode { get; set; }
        public ReactiveCommand XboxMode { get; set; }
        public ReactiveCommand VrMode { get; set; }
        public ReactiveCommand AcquireDevice { get; set; }
        public ReactiveCommand AddRemoveSecondaryDevice { get; set; }
        public ReactiveCommand PlayPause { get; set; }
        public ReactiveCommand CoupledDecoupled { get; set; }
        public ReactiveCommand StartKeybind { get; set; }
        public ReactiveCommand ChangeKeybind { get; set; }
        public ReactiveCommand JoyCplCommand { get; set; }
        public ReactiveCommand UnplugAllXboxCommand { get; set; }
        public ReactiveCommand AcquireVJoyDevice { get; set; }

        public ReactiveCommand VJoyInfo { get; set; }
        public ReactiveCommand VXboxInfo { get; set; }
        public ReactiveCommand SteamVRInfo { get; set; }

        public ReactiveCommand RefreshProcs { get; set; }
        public ReactiveCommand InjectProc { get; set; }
        public ReactiveCommand UpdateHookInterface { get; set; }
        public ReactiveCommand InjectAndUpdate { get; set; }

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

        private async Task InjectProcImpl()
        {
            bool success = await Task.Run(() => DeviceManager.InjectControllerIntoProcess(this.SelectedProc));
            Injected = success;
            if (Injected)
            {
                InjectedProcs.Add(SelectedProc);
                await UpdateHookInterfaceImpl();
            }
        }

        private async Task UpdateHookInterfaceImpl()
        {
            if (_ViveControllerButtonId[this.SelectedViveControllerButtonIndex] != Valve.VR.EVRButtonId.k_EButton_SteamVR_Touchpad)
            {
                this.SelectedControllerTouchIndex = (int)Valve.VR.EVRButtonType.Press;
            }
            await Task.Run(() => DeviceManager.ReceivedNewViveBindings(
                (Valve.VR.EVRButtonType)this.SelectedControllerTouchIndex,
                _ViveControllerButtonId[this.SelectedViveControllerButtonIndex], 
                (Valve.VR.EVRHand)this.SelectedControllerHandIndex));
            ViveBindingsChanged = false;
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

        private void ShowSteamVrDialog()
        {
            string text = "This feature is highly experimental, doesn't work with many games yet, and could get flagged by VAC.";
            text += "\nUse at your own risk!";
            text += "\n\n1. Start your game, make sure controllers have synced";
            text += "\n2. Select the game in PocketStrafe (refresh if needed)";
            text += "\n3. Setup the run forward binding (touchpad, trigger, or grip)";
            text += "\n4. Click Inject";
            MessageBox.Show(text, "SteamVR Output (Beta feature)", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowRestartMessage()
        {
            MessageBox.Show("Restart PocketStrafe PC to use vXbox","Success!",MessageBoxButton.OK,MessageBoxImage.Information);
        }


    }
}
