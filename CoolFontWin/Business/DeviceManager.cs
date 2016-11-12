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

        static readonly object locker = new object();

        // devices
        private VirtualDevice VDevice;
        private XInputDeviceManager XMgr;
        private Controller XDevice;

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
        public bool InterceptXInputDevice = false;
        public bool XInputDeviceConnected = false;
        public bool VJoyEnabled
        {
            get
            {
                return VDevice.vJoyEnabled;
            }
        }
        public bool VJoyDeviceConnected
        {
            get
            {
                return VDevice.vJoyAcquired;
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
            XMgr = new XInputDeviceManager();
            AcquireXInputDevice();

            VDevice = new VirtualDevice();

            AcquireDefaultVJoyDevice();

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
            }
            return XInputDeviceConnected;
        }

        /// <summary>
        /// Aquire vJoy device according to Default setting.
        /// </summary>
        /// <returns>Bool indicating whether device was acquired.</returns>
        public bool AcquireDefaultVJoyDevice()
        {
            return VDevice.TryVJoyDevice((uint)Properties.Settings.Default.VJoyID);
        }

        public bool AcquireVJoyDevice(uint id)
        {
            bool res = VDevice.TryVJoyDevice(id);
            return res;
        }

        public void RelinquishCurrentDevice()
        {
            VDevice.RelinquishCurrentDevice();
        }

        private void InitializeTimer()
        {
            VDeviceUpdateTimer = new Timer(16); // elapse every 1/60 sec, approx 16 ms
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
            return VDevice.ClickedMode(mode);
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
            PassDataToDevices(new byte[] { });
        }

        /// <summary>
        /// Pass data from socket to virtual device, and determine if interpolation should occur.
        /// </summary>
        /// <param name="data">Array of bytes representing UTF8 encoded string.</param>
        public void PassDataToDevices(byte[] data)
        {
            lock (locker)
            {
                // give data to virtual device
                if (VDevice.HandleNewData(data))
                {
                    // returns false if data is empty
                    VDevice.ShouldInterpolate = true;
                    TimerCount = 0;
                }

                // determine whether to keep interpolating
                if (TimerCount >= MaxInterpolateCount)
                {
                    VDevice.ShouldInterpolate = false;
                }

                // xbox controller handling
                if (InterceptXInputDevice)
                {
                    if (VDevice.Mode == SimulatorMode.ModeWASD)
                    {
                        // do not allow xbox controller in Keyboard mode
                        InterceptXInputDevice = false;
                    }
                    else
                    {
                        try
                        {
                            State state = XDevice.GetState();
                            VDevice.AddControllerState(state);
                        }
                        catch
                        {
                            // controller probably not connected
                            InterceptXInputDevice = false;
                            XInputDeviceConnected = false;
                            System.Media.SystemSounds.Beep.Play();
                        }
                    }
                }

                // update virtual device
                VDevice.FeedVJoy();
                VDevice.ResetValues();
            }
        }

        public void Dispose()
        {
            Properties.Settings.Default.VJoyID = (int)CurrentDeviceID;
            Properties.Settings.Default.Save();
            VDevice.Dispose();
        }
    }
}
