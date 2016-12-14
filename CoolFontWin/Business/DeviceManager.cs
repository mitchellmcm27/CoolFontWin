﻿using System;
using System.Timers;
using System.Collections.Generic;
using SharpDX.XInput;
using log4net;
using System.ComponentModel;
using ReactiveUI;

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
    public sealed class DeviceManager : ReactiveObject, IDisposable
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
                    VDevice.MaxDevices = _MobileDevicesCount;
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
            VDevice = new VirtualDevice(UpdateInterval);
            InitializeTimer();
        }

        /// <summary>
        /// Allows XInput devices to be re-plugged and acquired.
        /// </summary>
        /// <returns>Returns boolean indicating if a controller was acquired.</returns>
        public bool AcquireXInputDevice()
        {
            InterceptXInputDevice = true;
                 
            uint id = VDevice.Id;
            ForceUnplugAllXboxControllers(silent: true);
            
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
            }
            else
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad, afterMilliseconds: 1000);
                xDeviceAcquired = false;
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

        public void ForceUnplugAllXboxControllers(bool silent = false)
        {
            RelinquishCurrentDevice(silent);
            VDevice.ForceUnplugAllXboxControllers();
        }

        public void Dispose()
        {
            RelinquishCurrentDevice();
            Properties.Settings.Default.VJoyID = (int)VDevice.Id;
            Properties.Settings.Default.Save(); 
        }    
    }
}
