using System;
using System.Diagnostics;
using System.ComponentModel;
using WindowsInput;
using vJoyInterfaceWrap;

// using SharpDX.XInput;

using CoolFont.Utils;
using log4net;

namespace CoolFont
{
    namespace Simulator
    {

        public enum SimulatorMode
        {
            // Controls how the character moves in-game
            [Description("Pause")]
            ModePaused = 0,

            [Description("Keyboard")]
            ModeWASD, // Use KB to run forward, mouse to turn

            [Description("Joystick (Coupled)")]
            ModeJoystickCoupled, // Use vJoy/XOutput to move character through game (strafe only, no turning). VR MODE.

            [Description("Joystick (Decoupled)")]
            ModeJoystickDecoupled, // phone direction decides which direction the character strafes (no turning)

            [Description("Joystick (Halo)")]
            ModeJoystickTurn, //TODO: Move character forward and turn L/R using joystick. Difficult.

            [Description("Mouse")]
            ModeMouse, // tilt the phone L/R U/D to move the mouse pointer

            [Description("Gamepad")]
            ModeGamepad, // fully functional gamepad similar to Xbox controller

            ModeCountDebug,
            ModeCountRelease = 4,
            ModeDefault = ModeWASD,
        };

        

        public class VirtualDevice
        {
            private static readonly ILog log =
    LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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

            private int LX;
            private int LY;
            private int RX;
            private int RY;
            private int LZ;
            private int RZ;
            private int Pov;
            private int Buttons;

            private bool LeftMouseButtonDown = false;
            private bool RightMoustButtonDown = false;  

            private double[] Valsf;
            private int PacketNumber;
            private static int MaxPacketNumber = 999;
            private int UpdateInterval;
            private uint Id;

            /* public properties */
            public bool ShouldInterpolate;
            public bool LogOutput = false;
            public bool UserIsRunning = true;
            public bool vJoyEnabled = false;
            public bool CurrentModeIsFromPhone = false;
            public int signX = -1;
            public int signY = -1;
            
            // getter and setter allows for future event handling
            public SimulatorMode Mode { get; set; }
            public SimulatorMode OldMode;

            public double RCFilterStrength;

            public VirtualDevice(uint id, int updateInterval)
            {               
                Mode = SimulatorMode.ModeDefault;
                OldMode = Mode;

                this.UpdateInterval = updateInterval;
                this.Id = id;

                // assuming socketPollInterval = 8,000:
                // 0.05 good for mouse movement, 0.15 was a little too smooth
                // 0.05 probably good for VR, where you don't have to aim with the phone
                // 0.00 is good for when you have to aim slowly/precisely
                RCFilterStrength = 0.1;
                
                ShouldInterpolate = false;
                ConfigureVJoy(this.Id);
                vJoyEnabled = StartVJoy(this.Id);
                if (vJoyEnabled == true) { SetUpVJoy(this.Id); }
                KbM = new InputSimulator();

                ResetValues();
            }
  
            public bool HandleNewData(string rcvd)
            {

                if (rcvd.Length == 0)
                {
                    if (ShouldInterpolate) { InterpolateData(); }
                    //need to reset vjoy ?
                    return false;
                }

                // packet number goes from 0 to 99 (MaxPacketNumber)
                // when packet number reaches 99, it resets to 0
                // we want to check if we received an outdated packet
                /*
                int packetNumber = ParsePacketNumber(rcvd);

                // if new packet # is smaller than previous 
                // and if it's not just the number resetting to 0
                // e.g. have 99, received 0  ->  99 -> false 
                // e.g. have 99, received 3  ->  95 -> false
                // e.g. have 99, received 98 ->   1 -> true
                // e.g. have 0, received 95  -> -95 -> true
                // e.g. have 10, received 98 -> -88 -> true
               
                if (packetNumber < this.PacketNumber && this.PacketNumber - packetNumber < MaxPacketNumber/3) // received an out-dated packet
                {
                    if (ShouldInterpolate) { InterpolateData(); }
                    return false;
                }                
                */

               // this.PacketNumber = packetNumber;

                double[] valsf = ParseString(rcvd);

                if (this.Valsf == null) { this.Valsf = valsf; }

                Buttons = ParseButtons(rcvd);
                int modeIn = ParseMode(rcvd, (int)Mode); // mode is a fallback
                UpdateMode(modeIn);

                valsf = ProcessValues(valsf); // converts ints to doubles in generic units

                valsf = TranslateValues(valsf); // converts units for specific device (e.g. vJoy)  

                double dt = UpdateInterval / 1000.0 / 1000.0; // s
                for (int i=0; i < valsf.Length; i++)
                {
                    if (i == 7) { continue; }// do not filter POV
                    valsf[i] = Algorithm.LowPassFilter(valsf[i], this.Valsf[i], RCFilterStrength, dt); // filter vals last
                }
                this.Valsf = valsf;
                
                AddValues(this.Valsf);
                AddButtons(Buttons);

                // received packet after a long delay
                if (!ShouldInterpolate)
                {
                    log.Info("!! Began receiving.");
                }
                ShouldInterpolate = true;
                return true;
            }

            private void InterpolateData()
            {
                /* given no new data, create some from previously received data */
                /* Could be more complex but right now just returns the last good values */

                AddValues(Valsf);
                AddButtons(Buttons);
            }

            private int ParsePacketNumber(string instring)
            {
                /* Parse string representation of bitmask (unsigned int) 
                 * String array is separated by "$"
                 * Mode bitmask is the 0th string */

                string[] instring_sep = instring.Split('$');
                string packetNumberString = instring_sep[3];
                try
                {
                    return int.Parse(packetNumberString);
                }
                catch
                {
                    return 0;
                }
            }

            private double[] ParseString(string instring)
            {
                /* Given a string representation of ints, split it into ints */
                /* Return int array */

                //Console.WriteLine(instring);
                string[] instring_sep = instring.Split('$');
                string axes_string = instring_sep[1];
                string[] axes_sep = axes_string.Split(':');

                double[] parsed = new double[axes_sep.Length];

                for (int i = 0; i < axes_sep.Length; i++)
                {
                    parsed[i] = Int32.Parse(axes_sep[i]);
                }

                return parsed;
            }

            private int ParseButtons(string instring)
            {
                /* Parse string representation of bitmask (unsigned int) 
                 * String array is separated by "$"
                 * Button bitmask is the (2nd string starting from 0)*/

                string[] instring_sep = instring.Split('$');
                string button_string = instring_sep[2];

                try
                {
                    return int.Parse(button_string);
                }
                catch
                {
                    return 0; // no buttons pressed
                }
            }

            private int ParseMode(string instring, int mode_old)
            {
                /* Parse string representation of bitmask (unsigned int) 
                 * String array is separated by "$"
                 * Mode bitmask is the 0th string */

                string[] instring_sep = instring.Split('$');
                string mode_string = instring_sep[0];
                try
                {
                    return int.Parse(mode_string);
                }
                catch
                {
                    return mode_old; // current mode
                }
            }

            public void ResetValues()
            {
                LX = (int)MaxLX / 2;
                LY = (int)MaxLY / 2;
                RX = (int)MaxRX / 2;
                RY = (int)MaxRY / 2;
                LZ = 0;
                RZ = 0;
                Pov = -1; // neutral state
               // Joystick.ResetVJD(); // need to reset vjoy?
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

                if ( valsf[0] > 0.1)
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

            private double[] TranslateValues(double[] valsf)
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
                    valsf[1] = valsf[1] * MaxLX/2;
                    valsf[2] = valsf[2] * MaxLY/2;
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

            static private double ThreshRun = 0.1;
            static private double ThreshWalk = 0.1;

            private void AddValues(double[] valsf)
            {
                /* Simply update joystick with vals */
                switch (Mode)
                {
                    case SimulatorMode.ModeWASD:
                     //   KbM.Mouse.MoveMouseBy((int)valsf[9], 0); // dx, dy (pixels)

                        if (valsf[0] > VirtualDevice.ThreshRun)
                        {
                            KbM.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                            UserIsRunning = true;
                        }
                        else
                        {
                            KbM.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                            UserIsRunning = false;
                        }

                        if (false) //TODO: Implement jumping on iPhone
                        {
                            KbM.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                        }

                        if (LogOutput)
                        {
                            Console.Write("W?: {0} Mouse: {1} Val:{2:.#}/{3}",
                                KbM.InputDeviceState.IsKeyDown(WindowsInput.Native.VirtualKeyCode.VK_W),
                                (int)valsf[9],
                                valsf[0],
                                VirtualDevice.ThreshRun * MaxLY/2);
                        }
                        break;

                    case SimulatorMode.ModeJoystickCoupled:

                        /* no strafing */
                        LX += 0;
                        LY += signY*(int)valsf[0];
                        RX += 0;
                        RY += 0;
                        Pov = (int)valsf[7];

                        if (LogOutput)
                        {
                            Console.Write("Y:{0}", LY);
                        }
                        break;

                    case SimulatorMode.ModeJoystickDecoupled:

                        /* strafing but no turning*/
                        LX += signX*(int)valsf[1];
                        LY += signY*(int)valsf[2];

                        RX += 0;
                        RY += 0;
                        Pov = (int)valsf[7];

                        if (LogOutput)
                        {
                            Console.WriteLine("X:{0} Y:{1}", LX, LY);
                        }
                        break;

                    case SimulatorMode.ModeJoystickTurn:

                        // still in testing

                        LX += 0; // no strafing
                        LY += signY*(int)valsf[0];

                        RX += 0; // look left/right, currently handled by mouse
                        RY += 0; // look up/down
                        Pov = (int)valsf[7];

                        KbM.Mouse.MoveMouseBy(-(int)valsf[9], // negative because device is not assumed upside down
                                              0); // dx, dy (pixels)

                        if (LogOutput)
                        {
                            Console.Write("Y:{0} dX:{1}", LY, -(int)valsf[9]);
                        }
                        break;

                    case SimulatorMode.ModeGamepad:

                        /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                        // Full gamepad simulation
                        // NOT FINISHED YET

                        LX += -signX*(int)valsf[1];
                        LY += signY*(int)valsf[2];
                        RX += (int)valsf[3];
                        RY += -(int)valsf[4];
                        LZ += (int)valsf[6];
                        RZ += (int)valsf[5];
                        Pov = (int)valsf[7];

                        if (LogOutput)
                        {
                            Console.Write("X:{0} Y:{1} RX:{2} RY:{3} Z:{4} RZ{5} POV{6}", LX, LY, RX, RY, LZ, RZ, Pov);
                        }
                        break;

                    case SimulatorMode.ModeMouse:
                        /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                        // Control mouse on screen
                        KbM.Mouse.MoveMouseBy(-(int)valsf[9], // negative because device is not assumed upside down
                                              -(int)valsf[8]); // dx, dy (pixels)
                        if (LogOutput)
                        {
                            Console.Write("dx:{0} dy:{1}", (int)valsf[9], (int)valsf[9]);
                        }

                        break;
                }
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

                        Buttons = Buttons | buttonsDown;
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

            }

            public void UpdateMode(int mode)
            {
                if (mode == (int)Mode) { return; } // mode is the same as current
                if (mode == (int)OldMode) { return; } // mode incoming from phone is outdated

                if (!CheckMode(mode)) { return; }

                Mode = (SimulatorMode)mode;
                OldMode = Mode;
                this.CurrentModeIsFromPhone = true;  
            }

            public bool ClickedMode(int mode)
            {
                if(!CheckMode(mode)) { return false; }
                Mode = (SimulatorMode)mode;
                this.CurrentModeIsFromPhone = false;
                return true;
                
            }

            private bool CheckMode(int mode)
            {
                if (mode != (int)SimulatorMode.ModeWASD &&
                    mode != (int)SimulatorMode.ModePaused &&
                    mode != (int)SimulatorMode.ModeMouse)
                {
                    return vJoyEnabled;
                }

                return true;
            }

            /*
            public void AddControllerState(State state)
            {
                LX += state.Gamepad.LeftThumbX;
                LY -= state.Gamepad.LeftThumbY; // inverted 
                RX += state.Gamepad.RightThumbX;
                RY += state.Gamepad.RightThumbY;
                LZ += state.Gamepad.LeftTrigger; // not the right scale
                RZ += state.Gamepad.RightTrigger; // not the right scale
                Buttons = (short)state.Gamepad.Buttons;
            }
            */

            public void FeedVJoy()
            {
                if (Mode == (SimulatorMode.ModeMouse | SimulatorMode.ModePaused | SimulatorMode.ModeWASD))
                {
                    return;
                }

                /*Feed the driver with the position packet - is fails then wait for input then try to re-acquire device */
                if (!Joystick.UpdateVJD(Id, ref iReport))
                {
                   // Console.WriteLine("vJoy device {0} not enabled. Enable, then press Enter. \n", Id);
                   // Console.ReadKey(true);
                   // Joystick.AcquireVJD(Id);
                    return;
                }
                iReport.bDevice = (byte)Id;

                iReport.AxisX = LX;
                iReport.AxisY = LY;
                iReport.AxisXRot = RX;
                iReport.AxisYRot = RY;
                iReport.AxisZ = LZ;
                iReport.AxisZRot = RZ;

                // Press/Release Buttons
                iReport.Buttons = (uint)(Buttons);

                if (ContPovNumber > 0)
                {
                    iReport.bHats = ((uint)Pov);
                    //iReport.bHats = 0xFFFFFFFF; // Neutral state
                }
            }

            private void ConfigureVJoy(uint id)
            {
                /* Enable and Configure a vJoy device by calling 2 external processes
                 * Requires path to the vJoy dll directory */
                String filename = "C:\\Program Files\\vJoy\\x64\\vJoyConfig";
                String enableArgs = "enable on";
                String createArgs = String.Format("{0}", id);
                String configArgs = String.Format("{0} -f -a x y rx ry z rz -b 14 -p 1", id);

                ProcessStartInfo[] infos = new ProcessStartInfo[]
                {
                    /* 
                     * leads to a bug at the moment
                     * Not safe with an Xbox controller plugged in
                     * For now, rely on the vJoy Config GUI 
                     */

                    //  new ProcessStartInfo(filename, enableArgs),
                    //  new ProcessStartInfo(filename, configArgs),
                    //  new ProcessStartInfo(filename, createArgs),
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

            private bool StartVJoy(uint id)
            {
                // Create one joystick object and a position structure.
                Joystick = new vJoy();
                iReport = new vJoy.JoystickState();

                if (id <= 0 || id > 16)
                {
                    log.Info(String.Format("Illegal device ID {0}\nExit!", id));
                    return false;
                }

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

            private void SetUpVJoy(UInt32 id)
            {
               // pov = new byte[4]; // discrete pov hat directions

                // get max range of joysticks
                // neutral position is max/2
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

            public void DisableVJoy(uint id)
            {
                /* Enable and Configure a vJoy device by calling 2 external processes
                 * Requires path to the vJoy dll directory */
                String filename = "C:\\Program Files\\vJoy\\x64\\vJoyConfig";
                String disableArgs = "enable off";
                String deleteArgs = String.Format("-d {0}", id);

                ProcessStartInfo[] infos = new ProcessStartInfo[]
                {
                new ProcessStartInfo(filename, deleteArgs),
                new ProcessStartInfo(filename, disableArgs),
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
        }
    }
}
