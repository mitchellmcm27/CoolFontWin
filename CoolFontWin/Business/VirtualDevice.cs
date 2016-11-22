using System;
using System.Diagnostics;
using System.ComponentModel;
using SharpDX.XInput;
using WindowsInput;
using vJoyInterfaceWrap;

using log4net;
using System.Collections.Generic;

namespace CFW.Business
{
    public enum SimulatorMode
    {
        // Controls how the character moves in-game
        [Description("Pause")]
        ModePaused = 0,

        [Description("Keyboard")]
        ModeWASD, // Use KB to run forward, mouse to turn

        [Description("Joystick Coupled")]
        ModeJoystickCoupled, // Use vJoy/XOutput to move character through game (strafe only, no turning). VR MODE.

        [Description("Joystick Decoupled")]
        ModeJoystickDecoupled, // phone direction decides which direction the character strafes (no turning)

        [Description("Joystick+Mouse")]
        ModeJoystickTurn, //TODO: Move character forward and turn L/R using joystick. Difficult.

        [Description("Mouse")]
        ModeMouse, // tilt the phone L/R U/D to move the mouse pointer

        [Description("Gamepad")]
        ModeGamepad, // fully functional gamepad similar to Xbox controller

        ModeCountDebug,
        ModeCountRelease = 4,
        ModeDefault = ModeWASD,
    };

    /// <summary>
    /// Gives indexes of Valsf[] of joystick axes values
    /// </summary>
    /// vel, X, Y, RX, RY, Z, RZ, POV, dY, dX
    public static class IndexOf
    {
        // Index of data after separating string by $
        public static readonly int DataMode = 0;
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

    /// <summary>
    /// Emulates vJoy, Keyboard, and Mouse devices on Windows.
    /// </summary>
    public class VirtualDevice : IDisposable
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Init

        // private properties
        private long MaxLX = 1;
        private long MinLX = 1;
        private long MaxLY = 1;
        private long MinLY = 1;

        private long MaxRX = 1;
        private long MinRX = 1;
        private long MaxRY = 1;
        private long MinRY = 1;

        private long MaxPov = 1;
        private long MinPov = 1;

        private long MaxLZ = 1;
        private long MinLZ = 1;
        private long MaxRZ = 1;
        private long MinRZ = 1;

        private vJoy Joystick;
        private vJoy.JoystickState iReport;
        private int ContPovNumber;
        private InputSimulator KbM;
        private MobileDevice CombinedDevice;

        private bool LeftMouseButtonDown = false;
        private bool RightMoustButtonDown = false;  

        // public properties
        public uint Id; // vJoy ID
        public TimeSpan UpdateInterval;

        public List<MobileDevice> DeviceList; // Mobile devices to expect
            
        public bool UserIsRunning = true;
        public bool vJoyEnabled = false;
        public bool vJoyAcquired = false;
        public bool CurrentModeIsFromPhone = false;

        public int signX = -1; // allows axis inversion
        public int signY = -1;
        
        public SimulatorMode Mode;
        private SimulatorMode OldMode;

        public double RCFilterStrength;

        public VirtualDevice(TimeSpan updateInterval)
        {               
            Mode = SimulatorMode.ModeDefault;
            OldMode = Mode;

            UpdateInterval = updateInterval;

            // 0.05 good for mouse movement, 0.15 was a little too smooth
            // 0.05 probably good for VR, where you don't have to aim with the phone
            // 0.00 is good for when you have to aim slowly/precisely
            RCFilterStrength = 0.1;

            KbM = new InputSimulator();
            Joystick = new vJoy();
            iReport = new vJoy.JoystickState();
            CombinedDevice = new MobileDevice();

            // DeviceManager will fill this list as needed
            DeviceList = new List<MobileDevice>();

            // Default boolean states
            vJoyEnabled = false;
            vJoyAcquired = false;

            // Zero out iReport
            ResetValues();
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

            // Mode from primary device only!
            if (deviceNumber == 0)
            {
                int modeIn = int.Parse(instring_sep[IndexOf.DataMode]); // Mode is a fallback
                UpdateMode((SimulatorMode)modeIn);
            }

            // Main joystick axis values
            double[] valsf = ParseVals(instring_sep[IndexOf.DataVals]);

            // Update MobileDevice with processed vals
            DeviceList[deviceNumber].Valsf = valsf;
            DeviceList[deviceNumber].Count++;
            return true;
        }

        public void UpdateMode(SimulatorMode mode)
        {
            if (mode == Mode || mode == OldMode) { return; } // mode is the same as current
            if (!CheckMode(mode)) { return; }

            NeutralizeCurrentVJoyDevice();

            this.Mode = mode;
            this.OldMode = Mode;
            this.CurrentModeIsFromPhone = true;
            log.Info("Obtained mode from phone: " + Mode.ToString());
        }

        public bool ClickedMode(SimulatorMode mode)
        {
            if (!CheckMode(mode))
            {
                log.Debug("Seleted mode not available. vJoy not enabled? " + Mode.ToString());
                return false;
            }

            NeutralizeCurrentVJoyDevice();

            this.Mode = mode;
            this.CurrentModeIsFromPhone = false;
            log.Info("Obtained mode from CFW menu: " + Mode.ToString());
            return true;
        }

        private bool CheckMode(SimulatorMode mode)
        {
            // Must have a vJoy device acquired if trying to switch to Joystick mode
            if (mode != SimulatorMode.ModeWASD &&
                mode != SimulatorMode.ModePaused &&
                mode != SimulatorMode.ModeMouse)
            {
                return vJoyAcquired;
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
            for (int i = 0; i < DeviceList.Count; i++)
            {    
                if (!DeviceList[i].Ready) continue;
               
                valsf[IndexOf.ValVelocity] += DeviceList[i].Valsf[IndexOf.ValVelocity];
                valsf[IndexOf.ValX] += DeviceList[i].Valsf[IndexOf.ValX];
                valsf[IndexOf.ValY] += DeviceList[i].Valsf[IndexOf.ValY];
                CombinedDevice.Buttons = CombinedDevice.Buttons | DeviceList[i].Buttons; // bitmask

                // rolling average of POV, no need to know beforehand how many devices are Ready
                avgCount++;
                avgPOV = avgPOV * (avgCount-1) / avgCount + DeviceList[i].Valsf[IndexOf.ValPOV] / avgCount;
            }

            valsf[IndexOf.ValPOV] = avgPOV;

            valsf = TranslateValuesForVJoy(ProcessValues(valsf)); // converts units for specific device (e.g. vJoy)  

            // Filtering
            for (int i = 0; i < IndexOf.ValCount; i++)
            {
                if (i == IndexOf.ValPOV) { continue; } // do not filter POV because of angle problems

                valsf[i] = Algorithm.LowPassFilter (
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

            if (valsf[0] > 0.1)
            {
            }

            valsf[7] = Algorithm.WrapAngle(valsf[7] / 1000.0); // 0 to 360, do not Clamp


            if (Mode == SimulatorMode.ModeJoystickDecoupled)
            {
                // X and Y are determined by user direction and speed
                valsf[1] = Math.Cos(valsf[7] * Math.PI / 180) * valsf[0]; // -1 to 1 
                valsf[2] = -Math.Sin(valsf[7] * Math.PI / 180) * valsf[0]; // -1 to 1 
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
        private double[] TranslateValuesForVJoy(double[] valsf)
        {
            /* Goal: get data to the point where it can be added to the joystick device 
                * Specific to particular joystick
                * Final step before filtering and adding */

            // Use default valsf for keyboard, mouse modes
            if (Mode == SimulatorMode.ModeWASD ||
                Mode == SimulatorMode.ModeMouse ||
                Mode == SimulatorMode.ModePaused)
            {
                return valsf;
            }

            // Y axis in some modes
            valsf[0] = valsf[0] * MaxLY / 2;

            // 3 axes
            if (Mode == SimulatorMode.ModeJoystickDecoupled)
            {
                valsf[1] = valsf[1] * MaxLX / 2;
                valsf[2] = valsf[2] * MaxLY / 2;
            }
            else
            {
                valsf[1] = valsf[1] * MaxLX / 2 * 2; // max at half turn
                valsf[2] = valsf[2] * MaxLY / 2 * 2;
            }
            valsf[3] = valsf[3] * MaxRX / 2 * 2;
            valsf[4] = valsf[4] * MaxRY / 2 * 2;

            // 2 triggers
            valsf[5] = -valsf[5] * MaxLZ * 2; // max out at a quarter turn
            valsf[6] = valsf[6] * MaxLZ * 2;

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
            iReport.AxisX = 0;
            iReport.AxisY = 0;
            iReport.AxisXRot = 0;
            iReport.AxisYRot = 0;
            iReport.AxisZ = 0;
            iReport.AxisZRot = 0;

            iReport.Buttons = 0;
        }

        static private double ThreshRun = 0.1;
        static private double ThreshWalk = 0.1;
        private void AddValues(double[] valsf)
        {
            /* Simply update joystick with vals */
            switch (Mode)
            {
                case SimulatorMode.ModeWASD:
                    if (valsf[0] > VirtualDevice.ThreshRun)
                    {   
                        SendInputWrapper.KeyDown(SendInputWrapper.ScanCodeShort.KEY_W);
                        //KbM.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                        UserIsRunning = true;
                    }
                    else if (valsf[0] <= VirtualDevice.ThreshRun)
                    {
                        SendInputWrapper.KeyUp(SendInputWrapper.ScanCodeShort.KEY_W);
                        //KbM.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                        UserIsRunning = false;
                    }

                    if (false) //TODO: Implement jumping on iPhone
                    {
                        KbM.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                    }
                    break;

                case SimulatorMode.ModeJoystickCoupled:

                    /* no strafing */
                    iReport.AxisY += (int)(signY*valsf[IndexOf.ValVelocity]);
                    iReport.bHats = (uint)(valsf[IndexOf.ValPOV]);
                    break;

                case SimulatorMode.ModeJoystickDecoupled:

                    /* strafing but no turning*/
                    iReport.AxisX += (int)(signX*valsf[IndexOf.ValX]);
                    iReport.AxisY += (int)(signY*valsf[IndexOf.ValY]);
                    iReport.bHats = (uint)(valsf[IndexOf.ValPOV]);
                    break;

                case SimulatorMode.ModeJoystickTurn:

                    // still in testing
                    iReport.AxisY += signY*(int)valsf[0];
                    iReport.bHats = (uint)valsf[7];

                    KbM.Mouse.MoveMouseBy(-(int)valsf[9], // negative because device is not assumed upside down
                                            0); // dx, dy (pixels)
                    break;

                case SimulatorMode.ModeGamepad:

                    /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                    // Full gamepad simulation
                    // NOT FINISHED YET

                    iReport.AxisX += -signX*(int)valsf[1];
                    iReport.AxisY += signY*(int)valsf[2];
                    iReport.AxisXRot += (int)valsf[3];
                    iReport.AxisYRot += -(int)valsf[4];
                    iReport.AxisZ += (int)valsf[6];
                    iReport.AxisZRot += (int)valsf[5];
                    iReport.bHats = (uint)valsf[7];
                    break;

                case SimulatorMode.ModeMouse:
                    /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                    // Control mouse on screen
                    KbM.Mouse.MoveMouseBy(-(int)valsf[9]*5, // negative because device is not assumed upside down
                                            -(int)valsf[8]*5); // dx, dy (pixels)
                    break;
            }
        }

        public void AddJoystickConstants()
        {
            iReport.AxisX += (int)MaxLX / 2;
            iReport.AxisY += (int)MaxLY / 2;
            iReport.AxisXRot += (int)MaxRX / 2;
            iReport.AxisYRot += (int)MaxLY / 2;

            iReport.AxisZ += (int)MaxLZ / 2;
            iReport.AxisZRot += (int)MaxRZ / 2;
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
            AddJoystickConstants();
            FeedVJoy();
            ResetValues();
        }

        public void AddControllerState(State state)
        {
            iReport.AxisX += state.Gamepad.LeftThumbX/2;
            iReport.AxisY -= state.Gamepad.LeftThumbY/2; // inverted 
            iReport.AxisXRot += state.Gamepad.RightThumbX/2;
            iReport.AxisYRot += state.Gamepad.RightThumbY/2;
            iReport.AxisZ += state.Gamepad.LeftTrigger; // not the right scale
            iReport.AxisZRot += state.Gamepad.RightTrigger; // not the right scale
            iReport.Buttons = iReport.Buttons | (uint)state.Gamepad.Buttons;
        }

        public void FeedVJoy()
        {
            if (Mode == SimulatorMode.ModeMouse || Mode == SimulatorMode.ModePaused || Mode == SimulatorMode.ModeWASD)
            {
                return;
            }

            // vJoy joysticks are generally neutral at 50% values, this function takes care of that.
            AddJoystickConstants();

            //Feed the driver with the position packet - ignore failures
            if (!Joystick.UpdateVJD(Id, ref iReport))
            {
            }
        }

        #endregion

        #region vJoy helper methods
        /// <summary>
        /// Tries to acquire given vJoy device, relinquishing current device if necessary.
        /// </summary>
        /// <param name="id">vJoy device ID (1-16)</param>
        /// <returns>Boolean indicating if device was acquired.</returns>
        public bool SwapToVJoyDevice(uint id)
        {
            log.Info("Will try to acquire device " + id.ToString() + " and return result.");
            if (vJoyAcquired)
            {
                log.Info("First, relinquishing device " + this.Id.ToString());
                Joystick.ResetVJD(this.Id);
                Joystick.RelinquishVJD(this.Id);
                vJoyAcquired = false;
            }

            if (id < 1 || id > 16)
            {
                log.Debug("Device index " + id + " was invalid. Returning false.");
                return false;
            }

            if (!IsVJoyDriverEnabled())
            {
                vJoyEnabled = false;
                log.Debug("vJoy not enabled. I could try to enable it in the future. Returning false.");
                return false;
            }

            vJoyEnabled = true;

            if (!IsVJoyDeviceOwnedOrFree(id))
            {
                log.Debug("Chosen device is is not free! Returning false");
                return false;
                // return AcquireUnusedVJoyDevice();
            }

            if (AcquireVJoyDevice(id))
            {
                log.Info("Successfully acquired device " + id + ". Returning true.");
                this.Id = id;
                vJoyAcquired = true;
                GetJoystickProperties(id);
                ResetValues();
                AddJoystickConstants();
                Joystick.UpdateVJD(Id, ref iReport);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Loop through vJoy devices, find the first disabled device. Enable, config, and acquire it.
        /// </summary>
        /// <returns>Bool indicating if device was found, enabled, created, and acquired. </returns>
        private bool AcquireUnusedVJoyDevice()
        {
            log.Info("Will acquire first available vJoy device");
            vJoyAcquired = false;

            // find a disabled device
            for (int i = 1; i <= 16; i++)
            {
                if (IsVJoyDeviceDisabled((uint)i))
                {
                    // acquire device
                    if (EnableDefaultVJoyDevice((uint)i))
                    {
                        if (AcquireVJoyDevice((uint)i))
                        {
                            log.Info("Acquired device " + i);
                            this.Id = (uint)i;
                            vJoyAcquired = true;
                            GetJoystickProperties((uint)i);
                            ResetValues();
                            AddJoystickConstants();
                            Joystick.UpdateVJD(Id, ref iReport);
                        }
                    }
                }
            }
            return false;
        }

        private bool IsVJoyDriverEnabled()
        {
            // Get the driver attributes (Vendor ID, Product ID, Version Number)
            if (!Joystick.vJoyEnabled())
            {
                log.Info("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                return false;
            }
            else
            {
                log.Info(String.Format("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", Joystick.GetvJoyManufacturerString(), Joystick.GetvJoyProductString(), Joystick.GetvJoySerialNumberString()));
            }
            return true;
        }

        private bool IsVJoyDeviceOwnedOrFree(uint id)
        {
            if (id <= 0 || id > 16)
            {
                log.Info(String.Format("Illegal device ID {0}\nExit!", id));
                return false;
            }

            // Get the state of the requested device
            VjdStat status = Joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    log.Info(String.Format("vJoy Device {0} is already owned by this feeder\n", id));
                    break;
                case VjdStat.VJD_STAT_FREE:
                    log.Info(String.Format("vJoy Device {0} is free\n", id));
                    break;
                case VjdStat.VJD_STAT_BUSY:
                    log.Info(String.Format("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id));
                    return false;
                case VjdStat.VJD_STAT_MISS:
                    log.Info(String.Format("vJoy Device {0} is not installed or disabled\nCannot continue\n", id));
                    return false;
                default:
                    log.Info(String.Format("vJoy Device {0} general error\nCannot continue\n", id));
                    return false;
            };
            return true;
        }

        private bool IsVJoyDeviceDisabled(uint id)
        {
            // Get the state of the requested device
            VjdStat status = Joystick.GetVJDStatus(id);
            switch (status)
            {
                case VjdStat.VJD_STAT_OWN:
                    log.Info(String.Format("vJoy Device {0} is already owned by this feeder\n", id));
                    return false;
                case VjdStat.VJD_STAT_FREE:
                    log.Info(String.Format("vJoy Device {0} is free\n", id));
                    return false;
                case VjdStat.VJD_STAT_BUSY:
                    log.Info(String.Format("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id));
                    return false;
                case VjdStat.VJD_STAT_MISS:
                    log.Info(String.Format("vJoy Device {0} is not installed or disabled\nCannot continue\n", id));
                    break;
                default:
                    log.Info(String.Format("vJoy Device {0} general error\nCannot continue\n", id));
                    return false;
            };
            return true;
        }

        private bool AcquireVJoyDevice(uint id)
        {
            if (id <= 0 || id > 16)
            {
                log.Info(String.Format("Illegal device ID {0}\nExit!", id));
                return false;
            }

            VjdStat status = Joystick.GetVJDStatus(id);

            // Check which axes are supported
            bool AxisX = Joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
            bool AxisY = Joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
            bool AxisZ = Joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z);
            bool AxisRX = Joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
            bool AxisRZ = Joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            int nButtons = Joystick.GetVJDButtonNumber(id);
            int ContPovNumber = Joystick.GetVJDContPovNumber(id);
            int DiscPovNumber = Joystick.GetVJDDiscPovNumber(id);

            // Print results
            log.Info(String.Format("\nvJoy Device {0} capabilities:\n", id));
            log.Info(String.Format("Numner of buttons\t\t{0}\n", nButtons));
            log.Info(String.Format("Numner of Continuous POVs\t{0}\n", ContPovNumber));
            log.Info(String.Format("Numner of Descrete POVs\t\t{0}\n", DiscPovNumber));
            log.Info(String.Format("Axis X\t\t{0}\n", AxisX ? "Yes" : "No"));
            log.Info(String.Format("Axis Y\t\t{0}\n", AxisX ? "Yes" : "No"));
            log.Info(String.Format("Axis Z\t\t{0}\n", AxisX ? "Yes" : "No"));
            log.Info(String.Format("Axis Rx\t\t{0}\n", AxisRX ? "Yes" : "No"));
            log.Info(String.Format("Axis Rz\t\t{0}\n", AxisRZ ? "Yes" : "No"));

            // Test if DLL matches the driver
            UInt32 DllVer = 0, DrvVer = 0;
            bool match = Joystick.DriverMatch(ref DllVer, ref DrvVer);
            if (match)
            {
                log.Info(String.Format("Version of Driver Matches DLL Version ({0:X})\n", DllVer));
            }
            else
            {
                log.Info(String.Format("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer));
            }


            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!Joystick.AcquireVJD(id))))
            {
                log.Info(String.Format("Failed to acquire vJoy device number {0}.\n", id));
                return false;
            }
            else
            {
                log.Info(String.Format("Acquired: vJoy device number {0}.\n", id));
            }

            return true;
        }

        private void GetJoystickProperties(UInt32 id)
        {
            // Get max range of joysticks
            // Neutral position is max/2
            Joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref MaxLX);
            Joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_X, ref MinLX);
            Joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_Y, ref MaxLY);
            Joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_Y, ref MinLY);

            Joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RX, ref MaxRX);
            Joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_RX, ref MinRX);
            Joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RY, ref MaxRY);
            Joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_RY, ref MinRY);

            Joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_POV, ref MaxPov);
            Joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_POV, ref MinPov);

            Joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_Z, ref MaxLZ);
            Joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_Z, ref MinLZ);
            Joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RZ, ref MaxRZ);
            Joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_RZ, ref MinRZ);
            ContPovNumber = Joystick.GetVJDContPovNumber(id);
        }

        public List<int> GetEnabledDevices()
        {
            List<int> enabledDevs = new List<int>();

            if (!Joystick.vJoyEnabled()) return enabledDevs;

            for (int i = 1; i <= 16; i++)
            {
               if(Joystick.isVJDExists((uint)i))
                {
                    enabledDevs.Add(i);
                }
            }
            return enabledDevs;
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
            Joystick.ResetVJD(this.Id);
            Joystick.RelinquishVJD(this.Id);
            this.Id = 0;
            vJoyAcquired = false;
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
        #endregion

        public void Dispose()
        {
            log.Info("Relinquish VJD " + Id.ToString());
            Joystick.ResetVJD(this.Id);
            Joystick.RelinquishVJD(Id);
        }
    }
    
}
