using System;
using System.Timers;
using System.Collections.Generic;
using SharpDX.XInput;
using log4net;
using ReactiveUI;
using System.Diagnostics;
using EasyHook;
using System.Reflection;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel;

namespace CFW.Business
{
    public enum Axis
    {
        AxisX,
        AxisY,
    }

    /// <summary>
    /// Thread-safe singleton class for managing connected and virtual devices.
    /// Updates vJoy device with data from socket, optionally including an XInput device.
    /// </summary>
    public class DeviceManager : ReactiveObject
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static readonly List<int> ValidDevIDList = new List<int>
        {
            0, // none
            1001, 1002, 1003, 1004, // vXbox
            1, 2, 3, 4 ,5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 // vJoy
        };

        static readonly object locker = new object();
        
        // Devices
        public VirtualDevice VDevice; // single vjoy device, combines multiple mobile device inputs
        private XInputDeviceManager XMgr;
        private Controller XDevice; // single xbox controller

        // timer for updating devices
        private Timer VDeviceUpdateTimer;
        private int TimerCount = 0;
        private static int MaxInterpolateCount;

        // the following properties allow access to the underlying devices:
        // joystick smoothing 
        public double SmoothingFactor
        {
            get
            {
                return VDevice.RCFilterStrength;
            }
            set
            {
                VDevice.RCFilterStrength = value;
            }
        }

        // get mode
        public SimulatorMode Mode
        {
            get
            {
                return VDevice.Mode;
            }
        }

        // get source of current mode
        public bool CurrentModeIsFromPhone
        {
            get
            {
                return VDevice.CurrentModeIsFromPhone;
            }
        }

        // which devices are connected and which should be updated
        private bool _InterceptXInputDevice;
        public bool InterceptXInputDevice
        {
            get
            {
                return _InterceptXInputDevice;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _InterceptXInputDevice, value);
            }
        }

        private bool _XInputDeviceConnected;
        public bool XInputDeviceConnected
        {
            get { return _XInputDeviceConnected; }
            private set { this.RaiseAndSetIfChanged(ref _XInputDeviceConnected, value); }
        }

        public bool VJoyEnabled
        {
            get
            {
                return VDevice.DriverEnabled;
            }
        }

        public bool VJoyDeviceConnected
        {
            get
            {
                return VDevice.VDevAcquired;
            }
        }

        private TimeSpan UpdateInterval;

        private int _MobileDevicesCount;

        /// <summary>
        /// Tells Virtual Device how many different input streams to expect.
        /// </summary>
        public int MobileDevicesCount
        {
            get
            {
                return _MobileDevicesCount;
            }
            set
            {
                _MobileDevicesCount = value;
                VDevice.DeviceList = new List<MobileDevice>(value); // reset device list
                for (int i = 0; i < value; i++)
                {
                    VDevice.DeviceList.Add(new MobileDevice());
                }
                VDevice.MaxDevices = _MobileDevicesCount;
            }
        }


        public DeviceManager()
        {
            UpdateInterval = TimeSpan.FromSeconds(1 / 60.0);
            XMgr = new XInputDeviceManager();
            VDevice = new VirtualDevice(UpdateInterval);   
            InitializeTimer();
        }

        /// <summary>
        /// Allows XInput devices to be re-plugged and acquired.
        /// </summary>
        /// <returns>Returns boolean indicating if a controller was acquired.</returns>
        public bool AcquireXInputDevice()
        {                 
            uint id = VDevice.Id;
            if (id > 1000)
            {
                RelinquishCurrentDevice(silent: true);
            }
            ForceUnplugAllXboxControllers();
            
            XInputDeviceConnected = false;
            bool xDeviceAcquired = false;

            XDevice = XMgr.getController();

            if (XDevice != null && XDevice.IsConnected)
            {
                XInputDeviceConnected = true;
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good, afterMilliseconds: 1000);
                xDeviceAcquired = true;
                
                if (id == 0)
                {
                    VDevice.AcquireUnusedVDev();
                }
                else
                {
                    AcquireVDev(id);
                }

                TryMode((int)SimulatorMode.ModeJoystickCoupled);
                InterceptXInputDevice = true;
            }
            else
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                xDeviceAcquired = false;
                InterceptXInputDevice = true;
                InterceptXInputDevice = false;
                AcquireVDev(id);
            }

            return xDeviceAcquired;
        }

        public void RelinquishXInputDevice()
        {
            InterceptXInputDevice = false;
            ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
        }

        /// <summary>
        /// Aquire vJoy device according to Default setting.
        /// </summary>
        /// <returns>Bool indicating whether device was acquired.</returns>
        public bool AcquireDefaultVDev()
        {
            return VDevice.SwapToVDev((uint)Properties.Settings.Default.VJoyID);
        }

        public bool AcquireVDev(uint id)
        {
            bool res = VDevice.SwapToVDev(id);
            if (res)
            {
                if (id < 1000 ) ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
            }
            else if (id!=0 && id !=1000)
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
            }
            return res;
        }

        public void RelinquishCurrentDevice(bool silent=false)
        {
            log.Info("Relinquish current device");
            if (VDevice.VDevType == DevType.vJoy && !silent) ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
            VDevice.RelinquishCurrentDevice();
        }

        private void InitializeTimer()
        {
            VDeviceUpdateTimer = new Timer(UpdateInterval.TotalMilliseconds); // elapse every 1/60 sec, approx 16 ms
            VDeviceUpdateTimer.Elapsed += new ElapsedEventHandler(TimerElapsed); //define a handler
            VDeviceUpdateTimer.Enabled = true; //enable the timer.
            VDeviceUpdateTimer.AutoReset = true;
            MaxInterpolateCount = (int)Math.Floor(1 / VDeviceUpdateTimer.Interval * 1000 / 2); // = approx 0.5 sec
            log.Info("Started timer to update VDevice with interval " + VDeviceUpdateTimer.Interval.ToString() + "ms, max of " + MaxInterpolateCount.ToString() + " times.");
        }

        /// <summary>
        /// Pass along selected mode to the virtual device, which will decide whether to switch.
        /// </summary>
        /// <param name="mode">A valid SimulatorMode cast as int.</param>
        /// <returns>Returns a bool indicating whether the mode switched.</returns>
        public bool TryMode(int mode)
        {
            if (mode == (int)SimulatorMode.ModeWASD) RelinquishCurrentDevice(silent: true);
            return VDevice.ClickedMode((SimulatorMode)mode);
        }

        /// <summary>
        /// Invert an axis on the virutal device.
        /// </summary>
        /// <param name="axis">A valid Axis enum item.</param>
        public void FlipAxis(Axis axis)
        {
            switch (axis)
            {
                case Axis.AxisX:
                    VDevice.signX = -VDevice.signX;
                    break;
                case Axis.AxisY:
                    VDevice.signY = -VDevice.signY;
                    break;
            }
        }

        // Executes when the timer elapses.
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimerCount++;
            UpdateVirtualDevice();
        }

        /// <summary>
        /// Pass data from socket to virtual device, and determine if interpolation should occur.
        /// </summary>
        /// <param name="data">Array of bytes representing UTF8 encoded string.</param>
        public void PassDataToDevices(byte[] data)
        {
            // Called by a socket thread whenver it rcvs data
            lock (locker)
            {
                if (!VDevice.HandleNewData(data))
                {
                    return;
                }
            }
        }

        public void UpdateVirtualDevice()
        {
            // Called by a Timer at a fixed interval

            // Combine all mobile device data into a single input 
            VDevice.CombineMobileDevices();

            // Xbox controller handling
            if (InterceptXInputDevice)
            {
                if (XDevice.IsConnected)
                {
                    VDevice.AddControllerState(XDevice.GetState());
                }
                else
                {
                    log.Debug("Xbox controller was expected but not found.");
                    log.Debug("Relinquishing XInput device and setting InterceptXInputDevice to false.");
                    RelinquishXInputDevice();
                    XInputDeviceConnected = false;
                }
            }

            VDevice.FeedVDev();
        }

        public PSInterface Iface;
        private static System.Runtime.Remoting.Channels.Ipc.IpcServerChannel IpcServer;
        public bool InjectControllerIntoProcess(string proc)
        {
            string channelName = null;
            try
            {
                log.Info("Inject PocketStrafe into " + proc + "...");

                //Config.Register("PocketStrafe", "PocketStrafeInterface.dll", "Inject.dll");

                log.Info("  Creating IPC server");
                IpcServer = RemoteHooking.IpcCreateServer<PSInterface>(
                    ref channelName, System.Runtime.Remoting.WellKnownObjectMode.Singleton);

                log.Info("  Connect to IPC as client to get interface");
                Iface = RemoteHooking.IpcConnectClient<PSInterface>(channelName);
                ;
                log.Info("  Set interface properties");
                Iface.RunButton = Valve.VR.EVRButtonId.k_EButton_Axis0;
                log.Info("    Interface RunButton: " + Iface.RunButton);
                Iface.ButtonType = Valve.VR.EVRButtonType.Press;
                log.Info("    Interface ButtonType: " + Iface.ButtonType);
                Iface.Hand = Valve.VR.EVRHand.Left;
                log.Info("    Interface Hand: " + Iface.Hand);


                log.Info("  Subscribe to interface events");
                log.Info("    UserIsRunning");
                this.WhenAnyValue(x => x.VDevice.UserIsRunning)
                    .Do(x => Iface.UserIsRunning = x)
                    .Subscribe();
   
                var p = Process.GetProcessesByName(proc)[0];
                var injectDll = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(PocketStrafe).Assembly.Location), "Inject.dll");
                log.Info("  Injecting " + injectDll + " into " + p.ProcessName + " " + p.Id);
                RemoteHooking.Inject(
                    p.Id,
                    InjectionOptions.DoNotRequireStrongName,
                    injectDll,
                    injectDll,
                    channelName);

                for (int i = 0; i < 10; i++)
                {
                    if (Iface.Installed)
                    {
                        ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                        log.Info("Successfully injected!");
                        return true;
                    }
                    System.Threading.Thread.Sleep(250);
                }
                throw new TimeoutException("Injecting process timed out.");
            }
            catch (Exception e)
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                log.Error("  EasyHook Error: " + e.Message);
                log.Error(e);
                return false;
            }

        }

        public void ReceivedNewViveBindings(Valve.VR.EVRButtonType touch, Valve.VR.EVRButtonId button, Valve.VR.EVRHand hand)
        {

            if (Iface == null)
            {
                throw new ArgumentNullException("IPC Interface hasn't been initialized yet.");
            }

            try
            {
                var handNames = Enum.GetNames(typeof(Valve.VR.EVRHand)).ToList();
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

        private static readonly HashSet<string> DllNames = new HashSet<string>
        {
            "openvr_api.dll",
            "ovrplugin.dll",
            "vrclient.dll",

        };

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

        public List<string> GetProcesses()
        {
            var thisName = Process.GetCurrentProcess().ProcessName;
            var list = new List<string>();
            foreach (var proc in Process.GetProcesses())
            {
                if (proc.MainWindowTitle.Length > 0 && !proc.ProcessName.Equals(thisName) && Is64Bit(proc))
                {
                    list.Add(proc.ProcessName);
                }
            }
            return list;
        }

        public static bool Is64Bit(Process process)
        {
            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86")
                return false;

            bool isWow64;
            if (!IsWow64Process(process.Handle, out isWow64))
                throw new Win32Exception();
            return !isWow64;
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
    

        public void ForceUnplugAllXboxControllers()
        {  
            VDevice.ForceUnplugAllXboxControllers();
        }

        public void Dispose()
        {
            RelinquishCurrentDevice(silent:true);
            Properties.Settings.Default.VJoyID = (int)VDevice.Id;
            Properties.Settings.Default.Save(); 
        }    
    }
}
