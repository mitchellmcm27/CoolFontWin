using System;
using System.Timers;
using System.Collections.Generic;
using SharpDX.XInput;
using log4net;
using ReactiveUI;
using PocketStrafe.Input;
using PocketStrafe.Output;

namespace PocketStrafe
{
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
        private IPocketStrafeOutputDevice _OutputDevice;
        public IPocketStrafeOutputDevice OutputDevice // user-selected output (virtual) device implementing IPocketStrafeOutputDevice interface
        {
            get { return _OutputDevice; }
            set { this.RaiseAndSetIfChanged(ref _OutputDevice, value); }
        }
        public VDevOutputDevice VDev;
        public KeyboardOutputDevice Keyboard;
        public OpenVrInjectDevice Inject;

        // timer for updating devices
        private Timer VDeviceUpdateTimer;
        private int TimerCount = 0;
        private static int MaxInterpolateCount;

        // the following properties allow access to the underlying devices:
        // joystick smoothing 

        // 0.05 good for mouse movement, 0.15 was a little too smooth
        // 0.05 probably good for VR, where you don't have to aim with the phone
        // 0.00 is good for when you have to aim slowly/precisely
        private double _RCFilterStrength = 0.05;
        public double SmoothingFactor
        {
            get
            {
                return _RCFilterStrength;
            }
            set
            {
                _RCFilterStrength = value;
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
        private double UpdateIntervalSeconds;

        private bool _IsPaused;
        public bool IsPaused
        {
            get { return _IsPaused; }
            set { this.RaiseAndSetIfChanged(ref _IsPaused, value); }
        }

        public PocketStrafeDeviceManager()
        {
            UpdateInterval = TimeSpan.FromSeconds(1 / 60.0);
            UpdateIntervalSeconds = UpdateInterval.TotalSeconds;
            XMgr = new XInputDeviceManager();
            VDev = new VDevOutputDevice();
            Keyboard = new KeyboardOutputDevice();
            Inject = new OpenVrInjectDevice();
            MobileDevices = new List<PocketStrafeMobileDevice> { new PocketStrafeMobileDevice(), new PocketStrafeMobileDevice() };
            InitializeTimer();
            IsPaused = true;
            OutputDevice = null;
        }


        public void PauseOutput(bool pause)
        {
            if (pause) Stop(); 
            else Start();
        }

        public void Start()
        {
            log.Info("Get enabled devices...");
            VDev.GetEnabledDevices();
            if(OutputDevice == null) OutputDevice = Keyboard;
            VDeviceUpdateTimer.Start();
            IsPaused = false;
        }

        public void Stop()
        {
            VDeviceUpdateTimer.Stop();
            IsPaused = true;
        }

        /// <summary>
        /// Disconnect current output device and connect a new one of given type and (optional) ID.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="id"></param>
        public void GetNewOutputDevice(OutputDeviceType type, uint id = 0)
        {
            DisconnectOutputDevice();
            switch (type)
            {
                case OutputDeviceType.Keyboard:
                    Keyboard.Connect();
                    OutputDevice = Keyboard;
                    break;
                case OutputDeviceType.vJoy:
                    VDev.Connect(id);
                    OutputDevice = VDev;
                    break;
                case OutputDeviceType.vXbox:
                    VDev.Connect();
                    OutputDevice = VDev;
                    break;
                case OutputDeviceType.OpenVRInject:
                    Inject.Connect();
                    OutputDevice = Inject;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Sets the keybind to be used for running forward in keyboard output.
        /// </summary>
        /// <param name="key"></param>
        public void SetKeybind(string key)
        {
            Keyboard.SetKeybind(key);
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
            VDeviceUpdateTimer.Enabled = false; //enable the timer.
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
            OutputDevice.AddInput(Smooth(CombineMobileDevices(MobileDevices)));

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
            double[] valsf = new double[PocketStrafePacketIndex.Count];
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

        private PocketStrafeInput _LastInput;
        private PocketStrafeInput Smooth(PocketStrafeInput input)
        {
            input.speed = Algorithm.LowPassFilter(
                    input.speed,                   // new data
                    _LastInput.speed,    // last data
                    _RCFilterStrength,           // strength
                    UpdateIntervalSeconds // delta-t in seconds
            );
            input.POV = Algorithm.UnwrapAngle(input.POV, _LastInput.POV);
            input.POV = Algorithm.LowPassFilter(
                    input.POV,
                    _LastInput.POV,
                    _RCFilterStrength,
                    UpdateIntervalSeconds
            );
            input.POV = Algorithm.WrapAngle(input.POV);
            _LastInput = input;
            return input;
        }

        public void ForceUnplugAllXboxControllers()
        {
            VDev.ForceUnplugAllXboxControllers();
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
