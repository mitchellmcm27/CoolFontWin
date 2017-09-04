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
using CFW.VR;
using CFW.Business.Input;
using CFW.Business.Output;

namespace CFW.Business
{
    public enum OutputDeviceAxis
    {
        AxisX,
        AxisY,
    }

    /// <summary>
    /// Thread-safe singleton class for managing connected and virtual devices.
    /// Updates vJoy device with data from socket, optionally including an XInput device.
    /// </summary>
    public class PocketStrafeDeviceManager : ReactiveObject
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
        private XInputDeviceManager XMgr;
        private Controller XDevice; // single xbox controller
        private List<PocketStrafeMobileDevice> MobileDevices; // phones running PocketStrafe
        public IPocketStrafeOutputDevice OutputDevice; // user-selected output (virtual) device implementing IPocketStrafeOutputDevice interface
        private VDevOutputDevice VDev;
        private KeyboardOutputDevice Keyboard;

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
                return OutputDevice.RCFilterStrength;
            }
            set
            {
                OutputDevice.RCFilterStrength = value;
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
                return VDev.DriverEnabled;
            }
        }

        public bool VJoyDeviceConnected
        {
            get
            {
                return VDev.VDevAcquired;
            }
        }

        private TimeSpan UpdateInterval;

        public PocketStrafeDeviceManager()
        {
            UpdateInterval = TimeSpan.FromSeconds(1 / 60.0);
            XMgr = new XInputDeviceManager();
            InitializeTimer();
            VDev = new VDevOutputDevice();
            Keyboard = new KeyboardOutputDevice();
            OutputDevice = VDev;
            MobileDevices = new List<PocketStrafeMobileDevice> { new PocketStrafeMobileDevice(), new PocketStrafeMobileDevice() };
            log.Info("Get enabled devices...");
            VDev.GetEnabledDevices();
        }

        public void GetNewOutputDevice(OutputDeviceType type, uint id)
        {
            switch (type)
            {
                case OutputDeviceType.Keyboard:
                    DisconnectOutputDevice();
                    OutputDevice = Keyboard;
                    OutputDevice.Connect(id);
                    break;
                case OutputDeviceType.vJoy:
                    DisconnectOutputDevice();
                    OutputDevice = VDev;
                    OutputDevice.Connect(id);
                    break;
                case OutputDeviceType.vXbox:
                    DisconnectOutputDevice();
                    OutputDevice = VDev;
                    OutputDevice.Connect(); 
                    break;
                default:
                    break;
            }
        }

        public void SetKeybind(string key)
        {
            if (OutputDevice.Type == OutputDeviceType.Keyboard)
            {
                Keyboard.SetKeybind(key);
            }
        }

        /// <summary>
        /// Allows XInput devices to be re-plugged and acquired.
        /// </summary>
        /// <returns>Returns boolean indicating if a controller was acquired.</returns>
        public bool AcquireXInputDevice()
        {
            bool reacquire = false;
            if (OutputDevice.Type == OutputDeviceType.vXbox)
            {
                OutputDevice.Disconnect();
                reacquire = true;
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
                InterceptXInputDevice = true;
            }
            else
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                xDeviceAcquired = false;
                InterceptXInputDevice = true;
                InterceptXInputDevice = false;
            }

            if (reacquire)
            {
                OutputDevice.Connect();
            }
            return xDeviceAcquired;
        }

        public void RelinquishXInputDevice()
        {
            InterceptXInputDevice = false;
            ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
        }


        public void DisconnectOutputDevice()
        {
            OutputDevice.Disconnect();
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
        /// Invert an axis on the virutal device.
        /// </summary>
        /// <param name="axis">A valid Axis enum item.</param>
        public void FlipAxis(OutputDeviceAxis axis)
        {
            switch (axis)
            {
                case OutputDeviceAxis.AxisX:
                    OutputDevice.SignX = -OutputDevice.SignX;
                    break;
                case OutputDeviceAxis.AxisY:
                    OutputDevice.SignY = -OutputDevice.SignY;
                    break;
            }
        }

        public void PauseOutput(bool pause)
        {

        }

        // Executes when the timer elapses.
        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimerCount++;
            UpdateOutputDevice();
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
                PocketStrafeInput input = PocketStrafeData.GetData(data);
                MobileDevices[input.deviceNumber].SetState(input);
            }
        }

        private void UpdateOutputDevice()
        {
            // Called by a Timer at a fixed interval

            // Combine all mobile device data into a single input
            OutputDevice.AddInput(CombineMobileDevices(MobileDevices));

            // Xbox controller handling
            if (InterceptXInputDevice)
            {
                if (XDevice.IsConnected)
                {
                    OutputDevice.AddController(XDevice.GetState());
                }
                else
                {
                    log.Debug("Xbox controller was expected but not found.");
                    log.Debug("Relinquishing XInput device and setting InterceptXInputDevice to false.");
                    RelinquishXInputDevice();
                    XInputDeviceConnected = false;
                }
            }
            OutputDevice.Update();
        }

        /// <summary>
        /// Combine all Ready MobileDevices into a single input and get ready for vJoy
        /// </summary>
        public PocketStrafeInput CombineMobileDevices(List<PocketStrafeMobileDevice> devices)
        {
            double avgPOV = 0;
            int avgCount = 0;
            double[] valsf = new double[IndexOf.ValCount];
            PocketStrafeInput combined = new PocketStrafeInput();

            // Add vals and buttons from Ready devices
            combined.buttons = 0;

            for (int i = 0; i < MobileDevices.Count; i++)
            {
                if (!MobileDevices[i].Ready) continue;

                combined.speed += MobileDevices[i].State.speed;
                combined.buttons = combined.buttons | MobileDevices[i].State.buttons; // bitmask

                // rolling average of POV, no need to know beforehand how many devices are Ready
                if (MobileDevices[i].State.validPOV)
                {
                    avgCount++;
                    avgPOV = avgPOV * (avgCount - 1) / avgCount + MobileDevices[i].State.POV / avgCount;
                }
            }
            combined.POV = avgPOV;
            return combined;
        }


        // Injecting

        public PSInterface Iface;
        private static System.Runtime.Remoting.Channels.Ipc.IpcServerChannel IpcServer;
        public bool InjectControllerIntoProcess(string proc)
        {
            if (!Valve.VR.OpenVR.IsHmdPresent())
            {
                throw new Exception("No HMD was found.");
            }

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
                Iface.ButtonType = PStrafeButtonType.Press;
                log.Info("    Interface ButtonType: " + Iface.ButtonType);
                Iface.Hand = PStrafeHand.Left;
                log.Info("    Interface Hand: " + Iface.Hand);


                log.Info("  Subscribe to interface events:");
                log.Info("    UserIsRunning event");
                this.WhenAnyValue(x => x.OutputDevice.UserIsRunning)
                    .Do(x => Iface.UserIsRunning = x)
                    .Subscribe();

                var p = Process.GetProcessesByName(proc)[0];
                var injectDll = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(typeof(PocketStrafe).Assembly.Location), "Inject.dll");
                log.Info("  Injecting " + injectDll + " into " + p.ProcessName + " " + "(" + p.Id + ")");

                RemoteHooking.Inject(
                    p.Id,
                    InjectionOptions.DoNotRequireStrongName,
                    injectDll,
                    injectDll,
                    channelName);

                for (int i = 0; i < 20; i++)
                {
                    if (Iface.Installed)
                    {
                        ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                        log.Info("Successfully injected!");
                        return true;
                    }
                    System.Threading.Thread.Sleep(300);
                }
                throw new TimeoutException("Timed out. Make sure Steam VR is running.");
            }
            catch (Exception e)
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                log.Error("  EasyHook Error: " + e.Message);
                log.Error(e);
                return false;
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

        public void ReleaseHooks()
        {
            Iface.Cleanup();
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
            try
            {
                bool success = IsWow64Process(process.Handle, out isWow64);
                if (!success)
                    return false;
                return !isWow64;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);


        public void ForceUnplugAllXboxControllers()
        {
            if (OutputDevice.Type == OutputDeviceType.vJoy || OutputDevice.Type == OutputDeviceType.vXbox)
            {
                ((VDevOutputDevice)OutputDevice).ForceUnplugAllXboxControllers();
            }
        }

        public void Dispose()
        {
            if (OutputDevice.Type == OutputDeviceType.vJoy)
            { 
                Properties.Settings.Default.VJoyID = (int)((VDevOutputDevice)OutputDevice).Id;
                Properties.Settings.Default.Save();
            }
            DisconnectOutputDevice();
        }
    }
}
