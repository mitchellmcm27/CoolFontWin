#define EFFICIENT
//#define ROBUST

using System;
using System.Diagnostics;

using WindowsInput;
using vJoyInterfaceWrap;
using SharpDX.XInput;

using CoolFont.Utils;

namespace CoolFont
{

    public enum SimulatorMode
    {
        // Controls how the character moves in-game
        ModePaused = 0,
        ModeWASD, // Use KB to run forward, mouse to turn
        ModeJoystickCoupled, // Use vJoy/XOutput to move character through game (strafe only, no turning). VR MODE.
        ModeJoystickTurn, //TODO: Move character forward and turn L/R using joystick. Difficult.
        ModeJoystickDecoupled, // phone direction decides which direction the character strafes (no turning)
        ModeMouse, // tilt the phone L/R U/D to move the mouse pointer
        ModeGamepad, // fully functional gamepad similar to Xbox controller

        ModeDefault = ModeJoystickCoupled,
    };

    
    namespace Simulator
    {
        public class VirtualDevice
        {
            private long maxX = 0;
            private long minX = 0;
            private long maxY = 0;
            private long minY = 0;

            private long maxRX = 0;
            private long minRX = 0;
            private long maxRY = 0;
            private long minRY = 0;

            private long maxPOV = 0;
            private long minPOV = 0;

            private long maxZ = 0;
            private long minZ = 0;
            private long maxRZ = 0;
            private long minRZ = 0;

            private vJoy joystick;
            private vJoy.JoystickState iReport;
            private int contPovNumber;

            private InputSimulator kbm;

            private int lX;
            private int lY;
            private int rX;
            private int rY;
            private int lZ;
            private int rZ;
            private int pov;
            private int buttons;

            private bool leftMouseButtonDown = false;
            private bool rightMoustButtonDown = false;  

            private double[] valsf;
            private int updateInterval;
            private uint id;

            /* public properties */
            public bool ShouldInterpolate;
            public bool LogOutput = false;

            // getter and setter allows for future event handling
            public SimulatorMode Mode { get; set; } 

            public double RCFilterStrength;

            
            public VirtualDevice(uint id, int updateInterval)
            {
                Mode = SimulatorMode.ModeDefault;
                this.updateInterval = updateInterval;
                this.id = id;

                // assuming socketPollInterval = 8,000:
                // 0.05 good for mouse movement, 0.15 was a little too smooth
                // 0.05 probably good for VR, where you don't have to aim with the phone
                // 0.00 is good for when you have to aim slowly/precisely
                RCFilterStrength = 0.05;

                ShouldInterpolate = false;
                ConfigureVJoy(this.id);
                StartVJoy(this.id);
                SetUpVJoy(this.id);
                kbm = new InputSimulator();
                ResetValues();
            }

            
            public bool HandleNewData(string rcvd)
            {
                if (rcvd.Length == 0)
                {
                    if (ShouldInterpolate) { InterpolateData(); }
                    return false;
                }

                double[] valsf = ParseString(rcvd);
                if (this.valsf == null) this.valsf = valsf;
                buttons = ParseButtons(rcvd);
                int modeIn = ParseMode(rcvd, (int)Mode); // mode is a fallback

                UpdateMode(modeIn);

                valsf = ProcessValues(valsf); // converts ints to doubles in generic units
                valsf = TranslateValues(valsf); // converts units for specific device (e.g. vJoy)  

                double dt = updateInterval / 1000.0 / 1000.0; // s
                for (int i=0; i < valsf.Length; i++)
                {
                    if (i == 7) { continue; }// do not filter POV
                    valsf[i] = Algorithm.LowPassFilter(valsf[i], this.valsf[i], RCFilterStrength, dt); // filter vals last
                }
                this.valsf = valsf;

                AddValues(this.valsf);
                AddButtons(buttons);

                ShouldInterpolate = true;
                return true;
            }

            private void InterpolateData()
            {
                /* given no new data, create some from previously received data */
                /* Could be more complex but right now just returns the last good values */

                AddValues(valsf);
                AddButtons(buttons);
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

            private void ResetValues()
            {
                lX = (int)maxX / 2;
                lY = (int)maxY / 2;
                rX = (int)maxRX / 2;
                rY = (int)maxRY / 2;
                lZ = 0;
                rZ = 0;
                pov = -1; // neutral state
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

            static private int mouseSens = 20;

            private double[] TranslateValues(double[] valsf)
            {
                /* Goal: get data to the point where it can be added to the joystick device 
                 * Specific to particular joystick
                 * Final step before filtering and adding */

                // Y axis in some modes
                valsf[0] = valsf[0] * maxY / 2;

                // 3 axes
                if (Mode == SimulatorMode.ModeJoystickDecoupled)
                {
                    valsf[1] = valsf[1] * maxX/2;
                    valsf[2] = valsf[2] * maxY/2;
                }
                else
                {
                    valsf[1] = valsf[1] * maxX / 2 * 2; // max at half turn
                    valsf[2] = valsf[2] * maxY / 2 * 2;
                }
                valsf[3] = valsf[3] * maxRX / 2 * 2;
                valsf[4] = valsf[4] * maxRY / 2 * 2;

                // 2 triggers
                valsf[5] = -valsf[5] * maxZ * 2; // max out at a quarter turn
                valsf[6] = valsf[6] * maxZ * 2;

                // POV hat
                valsf[7] = valsf[7] / 360 * maxPOV;

                // Mouse movement
                valsf[8] = valsf[8] * mouseSens;
                valsf[9] = valsf[9] * mouseSens;

                return valsf;
            }

            static private double THRESH_RUN = 0.7;
            static private double THRESH_WALK = 0.3;

            private void AddValues(double[] valsf)
            {
                /* Simply update joystick with vals */
                switch (Mode)
                {
                    case SimulatorMode.ModeWASD:
                        kbm.Mouse.MoveMouseBy((int)valsf[9], 0); // dx, dy (pixels)

                        if (valsf[0] >= THRESH_RUN * maxY)
                        {
                            kbm.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                        }
                        else
                        {
                            kbm.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                        }

                        if (false) //TODO: Implement jumping on iPhone
                        {
                            kbm.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                        }

                        if (LogOutput)
                        {
                            Console.Write("W?: {0} Mouse: {1}",
                                kbm.InputDeviceState.IsKeyDown(WindowsInput.Native.VirtualKeyCode.VK_W),
                                (int)valsf[9]);
                        }
                        break;

                    case SimulatorMode.ModeJoystickCoupled:

                        /* no strafing */
                        lX += 0;
                        lY += -(int)valsf[0];
                        rX += 0;
                        rY += 0;
                        pov = (int)valsf[7];

                        if (LogOutput)
                        {
                            Console.Write("Y:{0}", lY);
                        }
                        break;

                    case SimulatorMode.ModeJoystickDecoupled:

                        /* strafing but no turning*/
                        lX += (int)valsf[1];
                        lY += (int)valsf[2];

                        rX += 0;
                        rY += 0;
                        pov = (int)valsf[7];

                        if (LogOutput)
                        {
                            Console.WriteLine("X:{0} Y:{1}", lX, lY);
                        }
                        break;

                    case SimulatorMode.ModeJoystickTurn:

                        // still in testing

                        lX += 0; // no strafing
                        lY += -(int)valsf[0];

                        rX += 0; // look left/right, currently handled by mouse
                        rY += 0; // look up/down
                        pov = (int)valsf[7];

                        kbm.Mouse.MoveMouseBy(-(int)valsf[9], // negative because device is not assumed upside down
                                              0); // dx, dy (pixels)

                        if (LogOutput)
                        {
                            Console.Write("Y:{0} dX:{1}", lY, -(int)valsf[9]);
                        }
                        break;

                    case SimulatorMode.ModeGamepad:

                        /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                        // Full gamepad simulation
                        // NOT FINISHED YET

                        lX += (int)valsf[1];
                        lY += -(int)valsf[2];
                        rX += (int)valsf[3];
                        rY += -(int)valsf[4];
                        lZ += (int)valsf[6];
                        rZ += (int)valsf[5];
                        pov = (int)valsf[7];

                        if (LogOutput)
                        {
                            Console.Write("X:{0} Y:{1} RX:{2} RY:{3} Z:{4} RZ{5} POV{6}", lX, lY, rX, rY, lZ, rZ, pov);
                        }
                        break;

                    case SimulatorMode.ModeMouse:
                        /* vel, X, Y, RX, RY, Z, RZ, POV, dY, dX, */
                        // Control mouse on screen
                        kbm.Mouse.MoveMouseBy(-(int)valsf[9], // negative because device is not assumed upside down
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

                        buttons = buttons | buttonsDown;
                        break;

                    case SimulatorMode.ModeMouse:
                        if ((buttonsDown & 4096) != 0 & !leftMouseButtonDown) // A button pressed on phone
                        {
                            kbm.Mouse.LeftButtonDown();
                            leftMouseButtonDown = true;
                        }
                        if ((buttonsDown & 4096) == 0 & leftMouseButtonDown)
                        {
                            kbm.Mouse.LeftButtonUp();
                            leftMouseButtonDown = false;
                        }

                        if ((buttonsDown & 8192) != 0 & !rightMoustButtonDown) // B button pressed on phone
                        {
                            kbm.Mouse.RightButtonDown();
                            rightMoustButtonDown = true;
                        }
                        if ((buttonsDown & 8192) == 0 & rightMoustButtonDown)
                        {
                            kbm.Mouse.RightButtonUp();
                            rightMoustButtonDown = false;
                        }
                        break;
                }

            }

            private void UpdateMode(int new_mode)
            {
                if (new_mode == (int)Mode) { return; }

                Mode = (SimulatorMode)new_mode;
            }

            public void AddControllerState(State state)
            {
                lX += state.Gamepad.LeftThumbX;
                lY -= state.Gamepad.LeftThumbY; // inverted 
                rX += state.Gamepad.RightThumbX;
                rY += state.Gamepad.RightThumbY;
                lZ += state.Gamepad.LeftTrigger; // not the right scale
                rZ += state.Gamepad.RightTrigger; // not the right scale
                buttons = (short)state.Gamepad.Buttons;
            }

            public void FeedVJoy()
            {
                if (Mode == (SimulatorMode.ModeMouse | SimulatorMode.ModePaused | SimulatorMode.ModeWASD))
                {
                    return;
                }

                /*Feed the driver with the position packet - is fails then wait for input then try to re-acquire device */
                if (!joystick.UpdateVJD(id, ref iReport))
                {
                    Console.WriteLine("vJoy device {0} not enabled. Enable, then press Enter. \n", id);
                    Console.ReadKey(true);
                    joystick.AcquireVJD(id);
                    return;
                }
#if ROBUST
            /* incomplete now */
                    res = joystick.SetAxis(X, ID, HID_USAGES.HID_USAGE_X);
                    res = joystick.SetAxis(Y, ID, HID_USAGES.HID_USAGE_Y);
                    res = joystick.SetAxis(rX, ID, HID_USAGES.HID_USAGE_RX);
                    res = joystick.SetAxis(rY, ID, HID_USAGES.HID_USAGE_RY);

                    if (ContPovNumber > 0)
                    {
                        res = joystick.SetContPov((int)POV_f, ID, 1);
                    }
#endif
#if EFFICIENT
                iReport.bDevice = (byte)id;

                iReport.AxisX = lX;
                iReport.AxisY = lY;
                iReport.AxisXRot = rX;
                iReport.AxisYRot = rY;
                iReport.AxisZ = lZ;
                iReport.AxisZRot = rZ;

                // Press/Release Buttons
                iReport.Buttons = (uint)(buttons);

                if (contPovNumber > 0)
                {
                    iReport.bHats = ((uint)pov);
                    //iReport.bHats = 0xFFFFFFFF; // Neutral state
                }
#endif
                ResetValues();
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

            private void StartVJoy(UInt32 id)
            {
                // Create one joystick object and a position structure.
                joystick = new vJoy();
                iReport = new vJoy.JoystickState();

                if (id <= 0 || id > 16)
                {
                    Console.WriteLine("Illegal device ID {0}\nExit!", id);
                    return;
                }

                // Get the driver attributes (Vendor ID, Product ID, Version Number)
                if (!joystick.vJoyEnabled())
                {
                    Console.WriteLine("vJoy driver not enabled: Failed Getting vJoy attributes.\n");
                    Console.WriteLine("Defaulting to KBM simulation.");
                    return;
                }
                else
                {
                    Console.WriteLine("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", joystick.GetvJoyManufacturerString(), joystick.GetvJoyProductString(), joystick.GetvJoySerialNumberString());
                    Mode = SimulatorMode.ModeJoystickDecoupled;
                }

                // Get the state of the requested device
                VjdStat status = joystick.GetVJDStatus(id);
                switch (status)
                {
                    case VjdStat.VJD_STAT_OWN:
                        Console.WriteLine("vJoy Device {0} is already owned by this feeder\n", id);
                        break;
                    case VjdStat.VJD_STAT_FREE:
                        Console.WriteLine("vJoy Device {0} is free\n", id);
                        break;
                    case VjdStat.VJD_STAT_BUSY:
                        Console.WriteLine("vJoy Device {0} is already owned by another feeder\nCannot continue\n", id);
                        return;
                    case VjdStat.VJD_STAT_MISS:
                        Console.WriteLine("vJoy Device {0} is not installed or disabled\nCannot continue\n", id);
                        return;
                    default:
                        Console.WriteLine("vJoy Device {0} general error\nCannot continue\n", id);
                        return;
                };

                // Check which axes are supported
                bool AxisX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_X);
                bool AxisY = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Y);
                bool AxisZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_Z);
                bool AxisRX = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RX);
                bool AxisRZ = joystick.GetVJDAxisExist(id, HID_USAGES.HID_USAGE_RZ);
                // Get the number of buttons and POV Hat switchessupported by this vJoy device
                int nButtons = joystick.GetVJDButtonNumber(id);
                int ContPovNumber = joystick.GetVJDContPovNumber(id);
                int DiscPovNumber = joystick.GetVJDDiscPovNumber(id);

                // Print results
                Console.WriteLine("\nvJoy Device {0} capabilities:\n", id);
                Console.WriteLine("Numner of buttons\t\t{0}\n", nButtons);
                Console.WriteLine("Numner of Continuous POVs\t{0}\n", ContPovNumber);
                Console.WriteLine("Numner of Descrete POVs\t\t{0}\n", DiscPovNumber);
                Console.WriteLine("Axis X\t\t{0}\n", AxisX ? "Yes" : "No");
                Console.WriteLine("Axis Y\t\t{0}\n", AxisX ? "Yes" : "No");
                Console.WriteLine("Axis Z\t\t{0}\n", AxisX ? "Yes" : "No");
                Console.WriteLine("Axis Rx\t\t{0}\n", AxisRX ? "Yes" : "No");
                Console.WriteLine("Axis Rz\t\t{0}\n", AxisRZ ? "Yes" : "No");

                // Test if DLL matches the driver
                UInt32 DllVer = 0, DrvVer = 0;
                bool match = joystick.DriverMatch(ref DllVer, ref DrvVer);
                if (match)
                {
                    Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
                }
                else
                {
                    Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);
                }


                // Acquire the target
                if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
                {
                    Console.WriteLine("Failed to acquire vJoy device number {0}.\n", id);
                    return;
                }
                else
                {
                    Console.WriteLine("Acquired: vJoy device number {0}.\n", id);
                }

            }

            private void SetUpVJoy(UInt32 id)
            {
#if ROBUST
            // Reset this device to default values
            joystick.ResetVJD(ID);
#endif
#if EFFICIENT
               // pov = new byte[4];
#endif
                // get max range of joysticks
                // neutral position is max/2
                joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxX);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_X, ref minX);
                joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_Y, ref maxY);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_Y, ref minY);

                joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RX, ref maxRX);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_RX, ref minRX);
                joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RY, ref maxRY);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_RY, ref minRY);

                joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_POV, ref maxPOV);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_POV, ref minPOV);

                joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_Z, ref maxZ);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_Z, ref minZ);
                joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RZ, ref maxRZ);
                joystick.GetVJDAxisMin(id, HID_USAGES.HID_USAGE_RZ, ref minRZ);
                contPovNumber = joystick.GetVJDContPovNumber(id);
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
