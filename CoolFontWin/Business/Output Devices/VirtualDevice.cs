using System;
using System.Diagnostics;
using WindowsInput;
using vGenWrap;
using log4net;
using System.Collections.Generic;
using ReactiveUI;
using System.ComponentModel;
using System.Windows.Forms;

namespace CFW.Business
{

    /// <summary>
    /// Gives indexes of Valsf[] of joystick axes values
    /// </summary>
    /// vel, X, Y, RX, RY, Z, RZ, POV, dY, dX
    public struct IndexOf
    {
        // Index of data after separating string by $
        public static readonly int DataMode = 0;
        public static readonly int DataValidPOV = 0;
        public static readonly int DataVals = 1;
        public static readonly int DataButtons = 2;
        public static readonly int DataPacketNumber = 3;
        public static readonly int DataDeviceNumber = 4;

        // Index of vals after separating DataVals by :
        public static readonly int ValVelocity = 0;
        public static readonly int ValX = 1;
        public static readonly int ValY = 2;
        public static readonly int ValRX = 3;
        public static readonly int ValRY = 4;
        public static readonly int ValZ = 5;
        public static readonly int ValRZ = 6;
        public static readonly int ValPOV = 7;
        public static readonly int ValMouseDY = 8;
        public static readonly int ValMouseDX = 9;
        public static readonly int ValCount = 10;
    };

    public struct DataModeCode
    {
        public static readonly int Paused = 0;
        public static readonly int Keyboard = 1;
        public static readonly int JoystickCoupled = 2;
        public static readonly int JoystickDecoupled = 3;
        public static readonly int NoGyro = 10;
        public static readonly int GyroOK = 11;
    }
    /// <summary>
    /// Emulates vJoy, Keyboard, and Mouse devices on Windows.
    /// </summary>
    public class VirtualDevice : ReactiveObject
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public class State
        {
            public double X;
            public double Y;
            public double Z;
            public double RX;
            public double RY;
            public double RZ;
            public double Slider0;
            public double Slider1;
            public double POV;
            public double DPad;

            public uint Buttons;
            public byte bDevice;
        }

        public enum wButtons
        {
            //  ButtonNone      = 0,
            ButtonUp = 1 << 0,  // 00000001 = 1
            ButtonDown = 1 << 1,  // 00000010 = 2
            ButtonLeft = 1 << 2,  // 00000100 = 4
            ButtonRight = 1 << 3,  // 8
            ButtonStart = 1 << 4,  // 16
            ButtonBack = 1 << 5,  // 32
            ButtonLAnalog = 1 << 6,  // 64
            ButtonRAnalog = 1 << 7,  // 128
            ButtonLTrigger = 1 << 8,  // 256
            ButtonRTrigger = 1 << 9,  // 512
            ButtonA = 1 << 12, // 4096
            ButtonB = 1 << 13, // 8192
            ButtonX = 1 << 14, // 16384
            ButtonY = 1 << 15, // 32768
            ButtonHome = 1 << 16, // 65536
            ButtonChooseL = 1 << 17, // 131072
            ButtonChooseR = 1 << 18, // 262144
};

        #region Init

        // private properties
        private static readonly double MaxAxis = 100.0;
        private static readonly double MinAxis = 0.0;
        private static readonly double MaxPov = 359.9;
        private static readonly double MinPov = 0.0;

        private vDev Joystick;
        private State iReport;
        private int ContPovNumber;
        private InputSimulator KbM;
        private MobileDevice CombinedDevice;
        private int HDev;
        private Lazy<OpenVrEmulator> OpenVrEmulator = new Lazy<OpenVrEmulator>();

        private bool LeftMouseButtonDown = false;
        private bool RightMoustButtonDown = false;

        private TimeSpan UpdateInterval;

        // public properties
        public DevType VDevType;
        private uint _Id;
        public uint Id // 1-16 for vJoy, 1001-1004 for vXbox
        {
            get
            {
                return _Id;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _Id, value);
                VDevType = value < 1001 ? DevType.vJoy : DevType.vXbox;
            }
        }

        private List<int> _EnabledDevices;
        public List<int> EnabledDevices
        {
            get { return _EnabledDevices; }
            set { this.RaiseAndSetIfChanged(ref _EnabledDevices, value); }
        }

        public int MaxDevices;
        public List<MobileDevice> DeviceList; // Mobile devices to expect

        private bool _UserIsRunning;
        public bool UserIsRunning
        {
            get { return _UserIsRunning; }
            set { this.RaiseAndSetIfChanged(ref _UserIsRunning, value); }
        }

        public bool DriverEnabled = false;
        public bool VDevAcquired = false;
        public bool CurrentModeIsFromPhone = false;

        public int signX = 1; // allows axis inversion
        public int signY = 1;

        SimulatorMode _Mode;
        public SimulatorMode Mode
        {
            get { return _Mode; }
            set { this.RaiseAndSetIfChanged(ref _Mode, value); }
        }

        private SimulatorMode PreviousMode;

        public double RCFilterStrength;

        private SendInputWrapper.ScanCodeShort _VirtualKeyCode;
        private TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));

        private string _Keybind;
        public string Keybind
        {
            get { return _Keybind; }
            set
            {
                this.RaiseAndSetIfChanged(ref _Keybind, value);
               
            }
        }

        public VirtualDevice(TimeSpan updateInterval)
        {

            Mode = SimulatorMode.ModeWASD;
            PreviousMode = Mode;

            UpdateInterval = updateInterval;

            // 0.05 good for mouse movement, 0.15 was a little too smooth
            // 0.05 probably good for VR, where you don't have to aim with the phone
            // 0.00 is good for when you have to aim slowly/precisely
            RCFilterStrength = 0.05;

            KbM = new InputSimulator();
            Joystick = new vDev();
            iReport = new State();
            CombinedDevice = new MobileDevice();            

            // DeviceManager will fill this list as needed
            DeviceList = new List<MobileDevice>();
            EnabledDevices = new List<int>();

            // Default boolean states
            DriverEnabled = false;
            VDevAcquired = false;
            HDev = 0;

            // Zero out iReport
            ResetValues();
            
            string key = "W";
            SetKeybind(key);
        }

        public void SetKeybind(string key)
        {
            var keybindOld = Keybind;
            try
            {
                Keybind = key.ToCharArray()[0].ToString().ToUpper();

                // http://www.pinvoke.net/default.aspx/user32/MapVirtualKey.html
                _VirtualKeyCode = (SendInputWrapper.ScanCodeShort)SendInputWrapper.MapVirtualKey((uint)(Keys)Enum.Parse(typeof(Keys), _Keybind, true), 0x00);
                log.Info("Changed keybind to " + Keybind);
            }
            catch(Exception e)
            {
                log.Debug("Unable to set keybind " + e.Message);
                Keybind = keybindOld;
            }
        }
        #endregion

        #region Input data handling
        /// <summary>
        /// Main method for handling raw data from socket.
        /// </summary>
        /// <param name="data">Byte array representing UTF8 string.</param>
        /// <returns>Bool indicating whether data was ingested.</returns>
        public bool HandleNewData(byte[] data)
        {

            // Handle empty data case
            if (data.Length == 0)
            {
                return false;
            }

            string rcvd = System.Text.Encoding.UTF8.GetString(data);

            // Split on $ for main categories
            string[] instring_sep = rcvd.Split('$');

            // We have to know which device we are talking to in order to do anything
            int deviceNumber = 0;
            if (instring_sep.Length > IndexOf.DataDeviceNumber)
            {
                deviceNumber = int.Parse(instring_sep[IndexOf.DataDeviceNumber]);
            }

            if (deviceNumber > MaxDevices - 1)
            {
                log.Error("Data was from extraneous device! Return false.");
                return false;
            }

            DeviceList[deviceNumber].PacketNumber = int.Parse(instring_sep[IndexOf.DataPacketNumber]);

            // packet number goes from 0 to 999 (MaxPacketNumber)
            // when packet number reaches 999, it resets to 0
            // we want to check if we received an outdated packet

            // if new packet # is smaller than previous 
            // and if it's not just the number resetting to 0
            // e.g. have 99, received 0  ->  99 -> false 
            // e.g. have 99, received 3  ->  95 -> false
            // e.g. have 99, received 98 ->   1 -> true
            // e.g. have 0, received 95  -> -95 -> true
            // e.g. have 10, received 98 -> -88 -> true

            // Not implemented on a per-device basis yet!

            /* 
            if (packetNumber < this.PacketNumber && this.PacketNumber - packetNumber < MaxPacketNumber/3) // received an out-dated packet
            {
                if (ShouldInterpolate) { InterpolateData(); }
                return false;
            }                
            */

            // this.PacketNumber = packetNumber;

            // Buttons
            DeviceList[deviceNumber].Buttons = int.Parse(instring_sep[IndexOf.DataButtons]);

            // Update MobileDevice with processed vals
            DeviceList[deviceNumber].Valsf = ParseVals(instring_sep[IndexOf.DataVals]);
            DeviceList[deviceNumber].ValidPOV = int.Parse(instring_sep[IndexOf.DataValidPOV]) != DataModeCode.NoGyro;
            DeviceList[deviceNumber].Count++;
            return true;
        }

        public bool ClickedMode(SimulatorMode mode)
        {
            if (!CheckMode(mode))
            {
                log.Debug("Selected mode not available. vJoy not enabled? " + Mode.ToString());
                return false;
            }

            NeutralizeCurrentVJoyDevice();

            Mode = mode;
            CurrentModeIsFromPhone = false;
            log.Info("Obtained mode from CFW menu: " + Mode.ToString());
            return true;
        }

        private bool CheckMode(SimulatorMode mode)
        {
            // Must have a vJoy device acquired if trying to switch to Joystick mode
            if (mode == SimulatorMode.ModeGamepad || mode == SimulatorMode.ModeJoystickCoupled || mode == SimulatorMode.ModeJoystickDecoupled)
            {
                if (VDevAcquired)
                {
                    return true;
                }
                else
                {
                    return AcquireUnusedVDev();
                }
         
            }
            return true;
        }

        private double[] ParseVals(string axes_string)
        {
            /* Given a string representation of ints, split it into ints */
            /* Return int array */

            string[] axes_sep = axes_string.Split(':');
            double[] parsed = new double[axes_sep.Length];

            for (int i = 0; i < axes_sep.Length; i++)
            {
                parsed[i] = Int32.Parse(axes_sep[i]);
            }
            return parsed;
        }

        #endregion

        #region Data preparation (Main Class Logic)

        /// <summary>
        /// Combine all Ready MobileDevices into a single input and get ready for vJoy
        /// </summary>
        public void CombineMobileDevices()
        {
            ResetValues();

            double avgPOV = 0;
            int avgCount = 0;
            double[] valsf = new double[IndexOf.ValCount];

            // Add vals and buttons from Ready devices
            CombinedDevice.Buttons = 0;

            for (int i = 0; i < DeviceList.Count; i++)
            {
                if (!DeviceList[i].Ready) continue;

                valsf[IndexOf.ValVelocity] += DeviceList[i].Valsf[IndexOf.ValVelocity];
                valsf[IndexOf.ValX] += DeviceList[i].Valsf[IndexOf.ValX];
                valsf[IndexOf.ValY] += DeviceList[i].Valsf[IndexOf.ValY];
                CombinedDevice.Buttons = CombinedDevice.Buttons | DeviceList[i].Buttons; // bitmask

                // rolling average of POV, no need to know beforehand how many devices are Ready
                if (DeviceList[i].ValidPOV)
                {
                    avgCount++;
                    avgPOV = avgPOV * (avgCount - 1) / avgCount + DeviceList[i].Valsf[IndexOf.ValPOV] / avgCount;
                }
            }

            valsf[IndexOf.ValPOV] = avgPOV;
            
            valsf = TranslateValuesForVDev(ProcessValues(valsf)); // converts units for specific device (e.g. vJoy)  

            // Filtering
            for (int i = 0; i < IndexOf.ValCount; i++)
            {
                if (i == IndexOf.ValPOV) { continue; } // do not filter POV because of angle problems

                valsf[i] = Algorithm.LowPassFilter(
                    valsf[i],                   // new data
                    CombinedDevice.Valsf[i],    // last data
                    RCFilterStrength,           // strength
                    UpdateInterval.TotalSeconds // delta-t in seconds
                    );
            }

            // Update CombinedDevice
            CombinedDevice.Valsf = valsf;

            // Add values to iReport for vJoy
            AddValues(CombinedDevice.Valsf);
            AddButtons(CombinedDevice.Buttons);
        }

        private double[] ProcessValues(double[] valsf)
        {

            /* Goal:
                * Convert ints to floats and put in sensible units (e.g. 0 to 1, -1 to 1)
                *
                *    ^ 0    . pi/6
                *    |     / 
                *    |    /
                *    |   /
                *    |  /
                *    | /
                *    ./______________> pi/2
                * 
            /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX */

            valsf[0] = valsf[0] / 1000.0; // 0 to 1

            valsf[7] = Algorithm.WrapAngle(-valsf[7] / 1000.0); // 0 to 360, do not Clamp


            if (Mode == SimulatorMode.ModeJoystickDecoupled)
            {
                // X and Y are determined by user direction and speed
                valsf[1] = Math.Sin(valsf[7] * Math.PI / 180) * valsf[0]; // -1 to 1 
                valsf[2] = Math.Cos(valsf[7] * Math.PI / 180) * valsf[0]; // -1 to 1 
            }
            else
            {
                /* X and Y are determined by device orientation
                    * Could be any angle, but used as linear joystick input
                    * Need to wrap, because the user could turn their device >360,
                    * which would be read as maximum on the joystick
                    * 
                    * -1 to 1 
                    */
                valsf[1] = Algorithm.WrapQ2toQ4(valsf[1] / 1000.0); // -180 to 180
                valsf[1] = valsf[1] / 180;
                valsf[2] = Algorithm.WrapQ2toQ4(valsf[2] / 1000.0);
                valsf[2] = valsf[2] / 180;
            }

            /* RX, RY, Z, and RZ determiend by device orientation
                * They are linear joystick input, so Clamp
                * 
                * -1 to 1
                */
            valsf[3] = Algorithm.WrapQ2toQ4(valsf[3] / 1000.0);
            valsf[3] = valsf[3] / 180;
            valsf[4] = Algorithm.WrapQ2toQ4(valsf[4] / 1000.0);
            valsf[4] = valsf[4] / 180;

            valsf[5] = Algorithm.WrapQ2toQ4(valsf[5] / 1000.0);
            valsf[5] = valsf[5] / 180;
            valsf[6] = Algorithm.WrapQ2toQ4(valsf[6] / 1000.0);
            valsf[6] = valsf[6] / 180;

            /* Mouse dY and dX
                * Could be any number but often around += 2
                */

            valsf[9] = 1 * valsf[9] / 1000.0; // not yet accounting for mouse sensitivity
            valsf[8] = 2 * valsf[8] / 1000.0;

            return valsf;
        }

        static private int MouseSens = 20;
        private double[] TranslateValuesForVDev(double[] valsf)
        {
            /* Goal: get data to the point where it can be added to the joystick device 
                * Specific to particular joystick
                * Final step before filtering and adding */

            // Use default valsf for keyboard, mouse modes
            if (Mode == SimulatorMode.ModeWASD ||
                Mode == SimulatorMode.ModeSteamVr || 
                Mode == SimulatorMode.ModeMouse ||
                Mode == SimulatorMode.ModePaused)
            {
                return valsf;
            }

            // Y axis in some modes
            valsf[0] = valsf[0] * MaxAxis / 2;

            // 3 axes
            if (Mode == SimulatorMode.ModeJoystickDecoupled)
            {
                valsf[1] = valsf[1] * MaxAxis / 2;
                valsf[2] = valsf[2] * MaxAxis / 2;
            }
            else
            {
                valsf[1] = valsf[1] * MaxAxis / 2 * 2; // max at half turn
                valsf[2] = valsf[2] * MaxAxis / 2 * 2;
            }
            valsf[3] = valsf[3] * MaxAxis / 2 * 2;
            valsf[4] = valsf[4] * MaxAxis / 2 * 2;

            // 2 triggers
            valsf[5] = -valsf[5] * MaxAxis * 2; // max out at a quarter turn
            valsf[6] = valsf[6] * MaxAxis * 2;

            // POV hat
            valsf[7] = valsf[7] / 360 * MaxPov;

            // Mouse movement
            valsf[8] = valsf[8] * VirtualDevice.MouseSens;
            valsf[9] = valsf[9] * VirtualDevice.MouseSens;

            return valsf;
        }

        private void InterpolateData()
        {
            /* given no new data, create some from previously received data */
            /* Could be more complex but right now just returns the last good values */
            AddValues(CombinedDevice.Valsf);
            AddButtons(CombinedDevice.Buttons);
        }

        public void ResetValues()
        {
            iReport.bDevice = (byte)Id;
            iReport.X = 0;
            iReport.Y = 0;
            iReport.Z = 0;
            iReport.RX = 0;
            iReport.RY = 0;
            iReport.RZ = 0;
            iReport.POV = -1;
            iReport.DPad = -1;

            iReport.Buttons = 0;
        }

        static private double ThreshRun = 0.1;
        static private double ThreshWalk = 0.1;
        private void AddValues(double[] valsf)
        { 
            // Simply update iReport with values
            switch (Mode)
            {
                case SimulatorMode.ModeWASD:
                    if (valsf[0] > VirtualDevice.ThreshRun && !UserIsRunning)
                    {
                        SendInputWrapper.KeyDown(_VirtualKeyCode);
                        //KbM.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                        UserIsRunning = true;
                    }
                    else if (valsf[0] <= VirtualDevice.ThreshRun && UserIsRunning)
                    {
                        SendInputWrapper.KeyUp(_VirtualKeyCode);
                        //KbM.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                        UserIsRunning = false;
                    }

                    if (false) // TODO: Implement jumping on iPhone
                    {
                        KbM.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                    }
                    break;
        
                case SimulatorMode.ModeSteamVr:
                    if (valsf[0] > VirtualDevice.ThreshRun && !UserIsRunning)
                    {
                        UserIsRunning = true;  
                    }
                    else if (valsf[0] <= VirtualDevice.ThreshRun && UserIsRunning)
                    {
                        UserIsRunning = false;
                    }
                    
                    break;
    
                case SimulatorMode.ModeJoystickCoupled:

                    /* no strafing */
                    iReport.Y += signY * valsf[IndexOf.ValVelocity];
                    iReport.POV = valsf[IndexOf.ValPOV];
                    break;

                case SimulatorMode.ModeOpenVrEmulator:
                case SimulatorMode.ModeJoystickDecoupled:

                    /* strafing but no turning*/
                    iReport.X += signX * valsf[IndexOf.ValX];
                    iReport.Y += signY * valsf[IndexOf.ValY];
                    iReport.POV = valsf[IndexOf.ValPOV];
                    break;

                case SimulatorMode.ModeJoystickTurn:

                    // still in testing
                    iReport.Y += signY * valsf[0];
                    iReport.POV = valsf[7];

                    KbM.Mouse.MoveMouseBy(-(int)valsf[9], // negative because device is not assumed upside down
                                            0); // dx, dy (pixels)
                    break;

                case SimulatorMode.ModeGamepad:

                    /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                    // Full gamepad simulation
                    // NOT FINISHED YET

                    iReport.X += signX * valsf[1];
                    iReport.Y += signY * valsf[2];
                    iReport.RX += valsf[3];
                    iReport.RY += -valsf[4];
                    iReport.Z += valsf[6];
                    iReport.RZ += valsf[5];
                    iReport.POV = valsf[7];
                    break;

                case SimulatorMode.ModeMouse:
                    /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                    // Control mouse on screen
                    KbM.Mouse.MoveMouseBy(-(int)valsf[9] * 5, // negative because device is not assumed upside down
                                            -(int)valsf[8] * 5); // dx, dy (pixels)
                    break;
            }
        }

        public void AddJoystickConstants()
        {
            // 50
            iReport.X += MaxAxis / 2;
            iReport.Y += MaxAxis / 2;
            iReport.RX += MaxAxis / 2;
            iReport.RY += MaxAxis / 2;

            // 0
            // iReport.Z += 0;
            // iReport.RZ += 0;
        }

        private void AddButtons(int buttonsDown)
        {
            switch (Mode)
            {
                case SimulatorMode.ModeJoystickCoupled:
                case SimulatorMode.ModeJoystickTurn:
                case SimulatorMode.ModeJoystickDecoupled:
                case SimulatorMode.ModeGamepad:
                    if ((buttonsDown & 32768) != 0) // Y button pressed on Phone
                    {
                        buttonsDown = (short.MinValue | buttonsDown & ~32768); // Y button pressed in terms of XInput
                    }

                    CombinedDevice.Buttons = CombinedDevice.Buttons | buttonsDown;
                    break;

                case SimulatorMode.ModeMouse:
                    if ((buttonsDown & 4096) != 0 & !LeftMouseButtonDown) // A button pressed on phone
                    {
                        KbM.Mouse.LeftButtonDown();
                        LeftMouseButtonDown = true;
                    }
                    if ((buttonsDown & 4096) == 0 & LeftMouseButtonDown)
                    {
                        KbM.Mouse.LeftButtonUp();
                        LeftMouseButtonDown = false;
                    }

                    if ((buttonsDown & 8192) != 0 & !RightMoustButtonDown) // B button pressed on phone
                    {
                        KbM.Mouse.RightButtonDown();
                        RightMoustButtonDown = true;
                    }
                    if ((buttonsDown & 8192) == 0 & RightMoustButtonDown)
                    {
                        KbM.Mouse.RightButtonUp();
                        RightMoustButtonDown = false;
                    }
                    break;
            }
            iReport.Buttons = (uint)CombinedDevice.Buttons;

        }

        private void NeutralizeCurrentVJoyDevice()
        {
            log.Info("Feeding vJoy device with neutral values.");
            ResetValues();
            FeedVDev();
            ResetValues();
        }

        public void AddControllerState(SharpDX.XInput.State state)
        {
            // -50 to 50
            iReport.X += state.Gamepad.LeftThumbX / 327.68 / 2;
            iReport.Y += state.Gamepad.LeftThumbY / 327.68 / 2;
            iReport.RX += state.Gamepad.RightThumbX / 327.68 / 2;
            iReport.RY += state.Gamepad.RightThumbY / 327.68 / 2;

            // 0 to 100
            iReport.Z += state.Gamepad.RightTrigger / 2.55; 
            iReport.RZ += state.Gamepad.LeftTrigger / 2.55; 

            iReport.Buttons = iReport.Buttons | (uint)state.Gamepad.Buttons;
        }

        public void FeedVDev()
        {
            if (Mode == SimulatorMode.ModeMouse || Mode == SimulatorMode.ModePaused || Mode == SimulatorMode.ModeWASD || Mode==SimulatorMode.ModeSteamVr)
            {
                return;
            }

            // vJoy joysticks are generally neutral at 50% values, this function takes care of that.
            AddJoystickConstants();

            // clamp values to min/max
            iReport.X = Algorithm.Clamp(iReport.X, MinAxis, MaxAxis);
            iReport.Y = Algorithm.Clamp(iReport.Y, MinAxis, MaxAxis);
            iReport.RX = Algorithm.Clamp(iReport.RX, MinAxis, MaxAxis);
            iReport.RY = Algorithm.Clamp(iReport.RY, MinAxis, MaxAxis);
            iReport.Z = Algorithm.Clamp(iReport.Z, 0, 255);
            iReport.RZ = Algorithm.Clamp(iReport.RZ, 0, 255);

            if (Mode==SimulatorMode.ModeOpenVrEmulator) {
                OpenVrEmulator.Value.Update(iReport);
             }

            Joystick.SetDevAxis(HDev, 1, iReport.X);
            Joystick.SetDevAxis(HDev, 2, iReport.Y);
            Joystick.SetDevAxis(HDev, 3, iReport.Z);
            Joystick.SetDevAxis(HDev, 4, iReport.RX);
            Joystick.SetDevAxis(HDev, 5, iReport.RY);
            Joystick.SetDevAxis(HDev, 6, iReport.RZ);

            Joystick.SetDevButton(HDev, 1, ((wButtons)iReport.Buttons & wButtons.ButtonA) !=0);
            Joystick.SetDevButton(HDev, 2, ((wButtons)iReport.Buttons & wButtons.ButtonB) != 0);
            Joystick.SetDevButton(HDev, 3, ((wButtons)iReport.Buttons & wButtons.ButtonX) != 0);
            Joystick.SetDevButton(HDev, 4, ((wButtons)iReport.Buttons & wButtons.ButtonY) != 0);
            Joystick.SetDevButton(HDev, 5, ((wButtons)iReport.Buttons & wButtons.ButtonLTrigger) != 0);
            Joystick.SetDevButton(HDev, 6, ((wButtons)iReport.Buttons & wButtons.ButtonRTrigger) != 0);
            Joystick.SetDevButton(HDev, 7, ((wButtons)iReport.Buttons & wButtons.ButtonBack) != 0);
            Joystick.SetDevButton(HDev, 8, ((wButtons)iReport.Buttons & wButtons.ButtonStart) != 0);
            Joystick.SetDevButton(HDev, 9, ((wButtons)iReport.Buttons & wButtons.ButtonLAnalog) != 0);
            Joystick.SetDevButton(HDev, 10, ((wButtons)iReport.Buttons & wButtons.ButtonRAnalog) != 0);

            if (this.VDevType == DevType.vJoy)
            {
                Joystick.SetDevPov(this.HDev, 1, iReport.POV);
            }
            else
            {
                double val = -1;
                if (((wButtons)iReport.Buttons & wButtons.ButtonUp)!=0)
                {
                    if (((wButtons)iReport.Buttons & wButtons.ButtonRight) != 0)
                    {
                        val = 45;
                    }
                    else { val = 0; }
                }
                else if (((wButtons)iReport.Buttons & wButtons.ButtonRight) != 0)
                {
                    if (((wButtons)iReport.Buttons & wButtons.ButtonDown) != 0)
                    {
                        val = 135;
                    }
                    else { val = 90;}
                }
                else if (((wButtons)iReport.Buttons & wButtons.ButtonDown) != 0)
                {
                    if (((wButtons)iReport.Buttons & wButtons.ButtonLeft) != 0)
                    {
                        val = 225;
                    }
                    else{ val = 180; }
                }
                else if (((wButtons)iReport.Buttons & wButtons.ButtonLeft) != 0)
                {
                    if (((wButtons)iReport.Buttons & wButtons.ButtonUp) != 0)
                    {
                        val = 315;
                    }
                    else { val = 270; }
                }

                    Joystick.SetDevPov(this.HDev, 1, val);
            }
        }

        #endregion

        #region vJoy helper methods
        /// <summary>
        /// Tries to acquire given vJoy device, relinquishing current device if necessary.
        /// </summary>
        /// <param name="id">vJoy device ID (1-16)</param>
        /// <returns>Boolean indicating if device was acquired.</returns>
        public bool SwapToVDev(uint id)
        {

            DevType devType;
            if (id > 1000)
            {
                devType = DevType.vXbox;
                id -= 1000;
            }
            else
            {
                devType = DevType.vJoy;
            }

            log.Info("Will try to acquire " + (devType==DevType.vJoy ? "vJoy":"xBox") + " device " + id.ToString() + " and return result.");
            if (VDevAcquired)
            {
                log.Info("First, relinquishing " + (VDevType == DevType.vJoy ? "vJoy" : "xBox") + " device " + Id.ToString());
                Joystick.ResetAll();
                Joystick.RelinquishDev(HDev);
                VDevAcquired = false;
            }

        
            if (devType == DevType.vJoy && (id < 1 || id > 16) // vjoy
                ||
                devType == DevType.vXbox && (id < 1|| id > 4)) // xbox
            {
                log.Debug("SwapToVJoyDevice: Device index " + id + " was invalid. Returning false.");
                return false;
            }

            if (!IsDriverEnabled(devType))
            {
                DriverEnabled = false;
                log.Debug("Correc driver not enabled. I could try to enable it in the future. Returning false.");
                return false;
            }

            DriverEnabled = true;

            if (AcquireDevice(id, devType))
            {
                log.Info("Successfully acquired " + (devType == DevType.vJoy ? "vJoy" : "xBox") + " device " + id + ". Returning true.");
                this.VDevType = devType;
                this.Id = devType==DevType.vXbox ? id+1000 : id;
                this.VDevAcquired = true;
                GetJoystickProperties(id);
                ResetValues();
                Joystick.ResetAll();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loop through vJoy devices, find the first disabled device. Enable, config, and acquire it.
        /// </summary>
        /// <returns>Bool indicating if device was found, enabled, created, and acquired. </returns>
        public bool AcquireUnusedVDev()
        {
            log.Info("Will acquire first available vXbox device");
            VDevAcquired = false;

            // find a disabled device
            for (uint i = 1; i <= 4; i++)
            {
                if (AcquireDevice(i, DevType.vXbox))
                {
                    log.Info("  Acquired device " + (i+1000).ToString());
                    this.Id = i+1000;
                    VDevAcquired = true;
                    GetJoystickProperties(i);
                    ResetValues();
                    Joystick.ResetAll();
                    return true;
                } 
            }
            return false;
        }

        private bool IsDriverEnabled(DevType devType)
        {
            switch (devType)
            {
                case DevType.vJoy:
                    log.Info("vJoy Version: " + Joystick.GetvJoyVersion());
                    if (!Joystick.vJoyEnabled())
                    {
                        log.Info("  vJoy driver not enabled: Failed Getting vJoy attributes.");
                        return false;
                    }          
                    break;

                case DevType.vXbox:
                    if (!Joystick.isVBusExist())
                    {
                        log.Info("ScpVBus driver not installed!");
                        return false;
                    }
                    else
                    {
                        byte nSlots = 0;
                        Joystick.GetNumEmptyBusSlots(ref nSlots);
                        log.Info("ScpVBus enabled with " + nSlots.ToString() + " empty bus slots.");
                    }
                    break;
            }
            return true;
        }

        private bool AcquireDevice(uint id, DevType devType)
        {
            if (devType == DevType.vJoy && (id < 1 || id > 16) // vjoy
                ||
                devType == DevType.vXbox && (id < 1 || id > 4)) // xbox
            {
                log.Debug("AcquireVJoyDevice: Device index " + id + " was invalid. Returning false.");
                return false;
            }

            bool owned = false;
            bool free = false;
            bool exist = false;
            Joystick.isDevOwned((uint)id, devType, ref owned);
            Joystick.isDevFree((uint)id, devType, ref free);
            Joystick.isDevExist((uint)id, devType, ref exist);

            //Test if DLL matches the driver
            short DllVer = 0, DrvVer = 0;
            DrvVer = Joystick.GetvJoyVersion();
            log.Info("vJoy version " + DrvVer.ToString());

            // Acquire the target (sets hDev)
            Joystick.AcquireDev(id, devType, ref this.HDev);
            if (owned || (free && HDev == 0))
            {
                log.Info(String.Format("Failed to acquire " + (devType==DevType.vXbox?"xBox":"vJoy") + " device number {0}.", id));
                return false;
            }
            else
            {
                log.Info(String.Format("Acquired: " + (devType == DevType.vXbox ? "xBox" : "vJoy") + " device number {0}.", id));
            }


            bool AxisX = false;
            Joystick.isAxisExist(HDev, 1, ref AxisX);
            bool AxisY = false;
            Joystick.isAxisExist(HDev, 2, ref AxisY);
            bool AxisZ = false;
            Joystick.isAxisExist(HDev, 3, ref AxisZ);
            bool AxisRX = false;
            Joystick.isAxisExist(HDev, 4, ref AxisRX);
            bool AxisRY = false;
            Joystick.isAxisExist(HDev, 5, ref AxisRY);
            bool AxisRZ = false;
            Joystick.isAxisExist(HDev, 6, ref AxisRZ);
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            uint nBtn = 0;
            Joystick.GetDevButtonN(HDev, ref nBtn);
            uint nHat = 0;
            Joystick.GetDevHatN(HDev, ref nHat);

            int DiscPovNumber = Joystick.GetVJDDiscPovNumber(id);


            // Print results
            log.Info(String.Format("Device {0} capabilities:", id));
            log.Info(String.Format("  Number of buttons\t\t{0}", nBtn));
            log.Info(String.Format("  Number of Hats\t{0}", nHat));
            log.Info(String.Format("  Number of Descrete POVs\t\t{0}", DiscPovNumber));
            log.Info(String.Format("  Axis X\t\t{0}", AxisX ? "Yes" : "No"));
            log.Info(String.Format("  Axis Y\t\t{0}", AxisY ? "Yes" : "No"));
            log.Info(String.Format("  Axis Z\t\t{0}", AxisZ ? "Yes" : "No"));
            log.Info(String.Format("  Axis Rx\t\t{0}", AxisRX ? "Yes" : "No"));
            log.Info(String.Format("  Axis Ry\t\t{0}", AxisRY ? "Yes" : "No"));
            log.Info(String.Format("  Axis Rz\t\t{0}", AxisRZ ? "Yes" : "No"));

            if (devType==DevType.vXbox)
            {
                log.Info("Checking for valid vXbox controller");
                log.Info("  Checking axes...");
                if (AxisX && AxisY && AxisZ && AxisRX && AxisRY && AxisRZ)
                {
                    log.Info("     All axes found.");
                }
                else
                {
                    log.Info("    Required axes not found, returning false");
                    return false;
                }

                log.Info("  Checking buttons...");
                if (nBtn>=10)
                {
                    log.Info("    All buttons found.");
                }
                else
                {
                    log.Info("    Buttons not found, returning false");
                    return false;
                }
            }
            return true;
        }

        private void GetJoystickProperties(UInt32 id)
        {
            // Get max range of joysticks
            // Neutral position is max/2
            ContPovNumber = Joystick.GetVJDContPovNumber(id);
        }

        public void GetEnabledDevices()
        {
            Joystick = new vDev();
            log.Info("Get virtual devices able to be acquired...");
            List<int> enabledDevs = new List<int>();

            log.Info("Check drivers enabled: ");
            IsDriverEnabled(DevType.vJoy);
            IsDriverEnabled(DevType.vXbox);

            bool owned=false;
            bool exist=false;
            bool free=false;

            // loop through possible vJoy devices
            for (int i = 1; i <= 16; i++)
            {
                Joystick.isDevOwned((uint)i, DevType.vJoy, ref owned);
                Joystick.isDevFree((uint)i, DevType.vJoy, ref free);
                Joystick.isDevExist((uint)i, DevType.vJoy, ref exist);

                if (free || owned)
                {
                    log.Info("Found vJoy device " + i.ToString());
                    enabledDevs.Add(i);
                }
            }

            // loop through possible Xbox devices
            for (int i = 1; i <= 4; i++)
            {
                Joystick.isDevOwned((uint)i, DevType.vXbox, ref owned);
                Joystick.isDevFree((uint)i, DevType.vXbox, ref free);
                Joystick.isDevExist((uint)i, DevType.vXbox, ref exist);

                if (free || owned)
                {
                    log.Info("Found vXbox device " + i.ToString());
                    enabledDevs.Add(i+1000);
                }
            }

            EnabledDevices = enabledDevs;
        }

        public bool EnableDefaultVJoyDevice(uint id)
        {
            /* Enable and Configure a vJoy device by calling 2 external processes
                * Requires path to the vJoy dll directory */
            string fname = "vJoyConfig.exe";
            string path = FileManager.FirstOcurrenceOfFile(Properties.Settings.Default.VJoyDir, fname);

            if (path.Length < 1)
            {
                return false;
            }

            string enableArgs = "enable on";
            string configArgs = String.Format("{0} -f -a x y rx ry z rz -b 17 -p 1", id);
            string createArgs = String.Format("{0}", id);

            ProcessStartInfo[] infos = new ProcessStartInfo[]
            {
                /* 
                    * leads to a bug at the moment
                    * Not safe with an Xbox controller plugged in
                    * For now, rely on the vJoy Config GUI 
                    */

                new ProcessStartInfo(path, enableArgs),
                new ProcessStartInfo(path, configArgs),
                new ProcessStartInfo(path, createArgs),
            };

            Process vJoyConfigProc;
            foreach (ProcessStartInfo info in infos)
            {
                //Vista or higher check
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    info.Verb = "runas";
                }

                info.UseShellExecute = true;
                info.RedirectStandardError = false;
                info.RedirectStandardOutput = false;
                info.CreateNoWindow = true;
                vJoyConfigProc = Process.Start(info);
                vJoyConfigProc.WaitForExit();
            }
            return true;
        }

        public void RelinquishCurrentDevice()
        {
            Joystick.ResetAll();
            Joystick.RelinquishDev(HDev);
            Id = 0;
            HDev = 0;
            VDevAcquired = false;
        }

        public void DeleteVJoyDevice(uint id)
        {
            string fname = "vJoyConfig.exe";
            string path = FileManager.FirstOcurrenceOfFile(Properties.Settings.Default.VJoyDir, fname);

            if (path.Length < 1)
            {
                return;
            }

            string deleteArgs = String.Format("-d {0}", id);

            ProcessStartInfo[] infos = new ProcessStartInfo[]
            {
            new ProcessStartInfo(path, deleteArgs),
            };

            Process vJoyConfigProc;
            foreach (ProcessStartInfo info in infos)
            {
                //Vista or higher check
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    info.Verb = "runas";
                }

                info.UseShellExecute = true;
                info.RedirectStandardError = false;
                info.RedirectStandardOutput = false;
                info.CreateNoWindow = true;
                vJoyConfigProc = Process.Start(info);
                vJoyConfigProc.WaitForExit();
            }
        }

        public void ForceUnplugAllXboxControllers()
        {

            log.Info("Unplugging all vXbox controllers.");
            for (uint i=1; i <=4; i++)
            {
                Joystick.UnPlugForce(i);
            }
            GetEnabledDevices();
        }
        #endregion
    }
    
}
