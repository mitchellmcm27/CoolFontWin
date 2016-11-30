using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.XInput;
using log4net;


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
    public sealed class DeviceManager : IDisposable
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<int> ValidDevIDList = 
            new List<int> { 0, // none
                            1001, 1002, 1003, 1004, // vXbox
                            1, 2, 3, 4 ,5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16 // vJoy
                           };

        static readonly object locker = new object();

        // Devices
        private VirtualDevice VDevice; // single vjoy device, combines multiple mobile device inputs
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
        private bool _InterceptXInputDevice = false;
        public bool InterceptXInputDevice
        {
            get
            {
                return _InterceptXInputDevice;
            }
            set
            {
                if (VDevice.Mode==SimulatorMode.ModeWASD)
                {
                    _InterceptXInputDevice = false;
                }
                else if (value==true)
                {
                    // acquire device
                    _InterceptXInputDevice = AcquireXInputDevice();
                }
                else
                {
                    // give up device
                    ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                    _InterceptXInputDevice = false;
                }
            }
        }
        
        public bool XInputDeviceConnected { get; private set; }

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

        public uint CurrentDeviceID
        {
            get
            {
                return VDevice.Id;
            }
        }

        public List<int> EnabledVJoyDevicesList
        {
            get
            {
                return VDevice.GetEnabledDevices();
            }
        }

        private TimeSpan UpdateInterval;

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
                lock (locker)
                { 
                    VDevice.DeviceList = new List<MobileDevice>(value); // reset device list
                    for (int i = 0; i < value; i++)
                    {
                        VDevice.DeviceList.Add(new MobileDevice());
                    }
                }
            }
        }
        private int _MobileDevicesCount;

        // Lazy instantiation of singleton class.
        // Executes only once because static.
        private static readonly Lazy<DeviceManager> lazy = 
            new Lazy<DeviceManager>(() => new DeviceManager());
        
        /// <summary>
        /// Public getter for the singleton instance.
        /// </summary>
        public static DeviceManager Instance
        {
            get
            { 
                return lazy.Value;
            }
        }

        // Private initialization for the singleton class.
        private DeviceManager()
        {
            UpdateInterval = TimeSpan.FromSeconds(1 / 60.0);
            XMgr = new XInputDeviceManager();
            //AcquireXInputDevice();

            VDevice = new VirtualDevice(UpdateInterval);

            AcquireDefaultVDev();

            InitializeTimer();
        }

        /// <summary>
        /// Allows XInput devices to be re-plugged and acquired.
        /// </summary>
        /// <returns>Returns boolean indicating if a controller was acquired.</returns>
        public bool AcquireXInputDevice()
        {
            XInputDeviceConnected = false;
            XDevice = XMgr.getController();

            if (XDevice != null && XDevice.IsConnected)
            {
                XInputDeviceConnected = true;
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                return true;
            }
            ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
            return false;
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
                if (id < 1000) ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
            }
            else
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
            }
            return res;
        }

        public void RelinquishCurrentDevice()
        {
            VDevice.RelinquishCurrentDevice();
            if (CurrentDeviceID < 1000) ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
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
                    log.Debug("Setting InterceptXInputDevice to FALSE");
                    InterceptXInputDevice = false;
                    XInputDeviceConnected = false;
                }
            }

            VDevice.FeedVDev();
        }

        public void ForceUnplugAllXboxControllers()
        {
            RelinquishCurrentDevice();
            VDevice.ForceUnplugAllXboxControllers();
        }

        public void Dispose()
        {
            RelinquishCurrentDevice();
            Properties.Settings.Default.VJoyID = (int)CurrentDeviceID;
            Properties.Settings.Default.Save(); 
        }
    }
}
