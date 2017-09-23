using EasyHook;
using log4net;
using PocketStrafe.VR;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reactive.Linq;

namespace PocketStrafe.Output
{
    public class OpenVrInjectDevice : BaseOutputDevice, IPocketStrafeOutputDevice
    {
        private static readonly ILog log =
    LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public PSInterface Iface;
        private static System.Runtime.Remoting.Channels.Ipc.IpcServerChannel IpcServer;

        public OutputDeviceType Type { get { return OutputDeviceType.OpenVRInject; } }
        public uint Id { get { return 1; } }
        private List<int> _EnabledDevices = new List<int>() { 1 };
        public List<int> EnabledDevices { get { return _EnabledDevices; } }
        private string _InjectDll;
        

        private string _Keybind;

        public string Keybind
        {
            get { return _Keybind; }
            set { _Keybind = value; }
        }

        private bool _UserIsRunning;

        public bool UserIsRunning
        {
            get { return _UserIsRunning; }
            set { this.RaiseAndSetIfChanged(ref _UserIsRunning, value); }
        }

        private string _ChannelName = null;

        public OpenVrInjectDevice()
        {
            _InjectDll = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Inject.dll");
        }

        public void Connect(uint id)
        {
            Connect();
        }

        public void Connect()
        {
            _ChannelName = null;
            log.Info("Dll to inject: " + _InjectDll);
            try
            {
                log.Info("  Creating IPC server");
                IpcServer = RemoteHooking.IpcCreateServer<PSInterface>(
                    ref _ChannelName, System.Runtime.Remoting.WellKnownObjectMode.Singleton);

                log.Info("  Connect to IPC as client to get interface");
                Iface = RemoteHooking.IpcConnectClient<PSInterface>(_ChannelName);
                ;
                log.Info("  Set interface properties");
                Iface.RunButton = Valve.VR.EVRButtonId.k_EButton_Axis0;
                log.Info("    Interface RunButton: " + Iface.RunButton);
                Iface.ButtonType = PStrafeButtonType.Press;
                log.Info("    Interface ButtonType: " + Iface.ButtonType);
                Iface.Hand = PStrafeHand.Left;
                log.Info("    Interface Hand: " + Iface.Hand);

                log.Info("  Subscribe to interface events:");
                log.Info("    UserIsRunning event");
                this.WhenAnyValue(x => x.UserIsRunning)
                    .Do(x => Iface.UserIsRunning = x)
                    .Subscribe();
            }
            catch (Exception ex)
            {
                log.Error("  EasyHook Error: " + ex.Message);
                log.Error(ex);
                throw new PocketStrafeOutputDeviceException(ex.Message);
            }
        }

        public void SwapToDevice(int id)
        {
            // do nothing
        }

        public void Disconnect()
        {
            Iface.Cleanup();
        }

        public void AddInput(PocketStrafeInput input)
        {
            _State.Speed += input.speed;
        }

        public void AddController(SharpDX.XInput.State state)
        {
            // do nothing
        }

        private readonly double _ThreshRun = 0.1;
        private readonly double _ThreshWalk = 0.1;

        public void Update()
        {
            if (_State.Speed > _ThreshRun && !UserIsRunning)
            {
                UserIsRunning = true;
            }
            else if (_State.Speed <= _ThreshRun && UserIsRunning)
            {
                UserIsRunning = false;
            }

            ResetState();
        }

        public void InjectControllerIntoProcess(string proc)
        {
            if (!Valve.VR.OpenVR.IsHmdPresent())
            {
                throw new PocketStrafeOutputDeviceException("No HMD was found.");
            }
            try
            {
                log.Info("Inject PocketStrafe into " + proc + "...");
                var p = Process.GetProcessesByName(proc)[0];
                log.Info("  Injecting " + _InjectDll + " into " + p.ProcessName + " " + "(" + p.Id + ")");
                RemoteHooking.Inject(
                    p.Id,
                    InjectionOptions.DoNotRequireStrongName,
                    _InjectDll,
                    _InjectDll,
                    _ChannelName);

                for (int i = 0; i < 20; i++)
                {
                    if (Iface.Installed)
                    {
                        ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                        log.Info("Successfully injected!");
                        return;
                    }
                    System.Threading.Thread.Sleep(300);
                }
                throw new PocketStrafeOutputDeviceException("Connecting hook timed out. Make sure Steam VR is running.");
            }
            catch (Exception e)
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                log.Error("  EasyHook Error: " + e.Message);
                log.Error(e);
                throw new PocketStrafeOutputDeviceException(e.Message);
            }
        }

        public void ReceivedNewViveBindings(PStrafeButtonType touch, Valve.VR.EVRButtonId button, PStrafeHand hand)
        {
            if (Iface == null)
            {
                return;
            }

            try
            {
                var handNames = Enum.GetNames(typeof(PStrafeHand)).ToList();
                log.Info(touch);
                log.Info(hand);
                log.Info(button);

                Iface.ButtonType = touch;
                Iface.Hand = hand;
                Iface.RunButton = button;
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public List<string> GetProcessesWithModule(string dllName)
        {
            var thisName = Process.GetCurrentProcess().ProcessName;
            var found = new List<string>();
            var noPermission = new List<string>();
            var notFound = new List<string>();
            var modulesList = new List<string>();
            log.Info("Searching for processes that loaded " + dllName);

            foreach (var proc in Process.GetProcesses())
            {
                if (proc.MainWindowTitle.Length == 0 || proc.ProcessName.Equals(thisName))
                {
                    continue;
                }

                ProcessModuleCollection mods;
                try
                {
                    mods = proc.Modules;
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    noPermission.Add(proc.ProcessName);
                    continue;
                }
                foreach (ProcessModule mod in mods)
                {
                    modulesList.Add(mod.ModuleName);
                }

                if (modulesList.Contains(dllName))
                {
                    found.Add(proc.ProcessName);
                }
                else
                {
                    notFound.Add(proc.ProcessName);
                }
            }

            log.Info("  Found module:");
            found.OrderBy(x => x).ToList().ForEach(x => log.Info("    " + x));
            log.Info("  Module not found:");
            notFound.OrderBy(x => x).ToList().ForEach(x => log.Info("    " + x));
            log.Info("  Lacked permissions:");
            noPermission.OrderBy(x => x).ToList().ForEach(x => log.Info("    " + x));
            return found;
        }
    }
}