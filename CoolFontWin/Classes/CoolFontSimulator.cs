#define EFFICIENT
//#define ROBUST

using System;
using WindowsInput;
using vJoyInterfaceWrap;

namespace CoolFontWin
{
    class CoolFontSimulator
    {
        private long maxX = 0;
        private long maxY = 0;
        private long maxRX = 0;
        private long maxRY = 0;
        private long maxPOV = 0;

        private vJoy joystick;
        private vJoy.JoystickState iReport;
        private int ContPovNumber;

        private InputSimulator kbm;
        private int X, Y, rX, rY, POV, d_theta;
        private double POV_f, d_theta_f;
        private bool res;
        private byte[] pov;

        public bool logOutput = false;
        public int[] neutralVals { get; set; }


        public CoolFontSimulator(Config.MovementModes MODE)
        {
            StartVJoy(Config.ID);
            SetUpVJoy(Config.ID);
            kbm = new InputSimulator();    

        }

        public void UpdateCharacterWithValsForMode(int[] vals, Config.MovementModes mode)
        {
            switch (mode)
            {
                //TODO: Hold CTRL+W when under running threshold but over walking threshold
                case Config.MovementModes.KeyboardMouse:
                    kbm.Mouse.MoveMouseBy((int)(10.0 * vals[3] / 1000.0), 0); // dx, dy (pixels)

                    if (vals[0] >= Config.THRESH_RUN)
                    {
                        kbm.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                    }
                    else
                    {
                        kbm.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                    }

                    //TODO: Implement jumping on iPhone, send it in the 3rd int value
                    if (false)
                    {
                        kbm.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                    }

                    if (logOutput)
                    {
                        Console.Write("W?: {0} Mouse: {1}", 
                            kbm.InputDeviceState.IsKeyDown(WindowsInput.Native.VirtualKeyCode.VK_W), 
                            (int)(10.0 * vals[3] / 1000.0));
                    }
                    break;

                case Config.MovementModes.JoystickMove:

                    POV_f = vals[1] / 1000.0;

                    while (POV_f > 360)
                    {
                        POV_f -= 360;
                    }
                    while (POV_f < 0)
                    {
                        POV_f += 360;
                    }

                    POV_f *= maxPOV/360;

                    X = (int)(Math.Sin(POV_f / maxPOV * Math.PI * 2) * -vals[0] / 1000.0 * maxX / 2 + maxX / 2);
                    Y = (int)(Math.Cos(POV_f / maxPOV * Math.PI * 2) * -vals[0] / 1000.0 * maxY / 2 + maxY / 2);

                    rX = (int)maxRX / 2;
                    rY = (int)maxRX / 2;

                    /* Feed vJoy device */
                    FeedVJoy();
                    if (logOutput)
                    {
                        Console.Write("X:{0} Y:{1} dir:{2}", X, Y, (int)POV_f);
                    }

                    break;

                //TODO:
                case Config.MovementModes.JoystickMoveAndLook:
                    // NOT FINISHED YET
                    POV_f = vals[1] / 1000.0;

                    while (POV_f > 360)
                    {
                        POV_f -= 360;
                    }
                    while (POV_f < 0)
                    {
                        POV_f += 360;
                    }

                    POV_f *= maxPOV / 360;

                    X = (int)maxX / 2; // no strafing
                    Y = -vals[0] * (int)maxY / 1000 / 2 + (int)maxY / 2;

                    rX = (int)maxRX / 2; // needs to change
                    rY = (int)maxRX / 2; // look up/down

                    kbm.Mouse.MoveMouseBy(-(int)(1 * vals[3] / 1000.0 * Config.mouseSens), // negative because device is not assumed upside down
                                         0); // dx, dy (pixels)
                    FeedVJoy();
                    if (logOutput)
                    {
                        Console.Write("Y:{0} dir:{1}", Y, (int)POV_f);
                    }

                    break;

                case Config.MovementModes.Mouse2D:

                    kbm.Mouse.MoveMouseBy(-(int)(1 * vals[3] / 1000.0 * Config.mouseSens), // negative because device is not assumed upside down
                                          -(int)(2 * vals[2] / 1000.0 * Config.mouseSens)); // dx, dy (pixels)

                    if (logOutput)
                    {
                        Console.Write("dx:{0} dy:{1}",
                            (int)(30.0 * vals[3] / 1000.0), (int)(60.0 * vals[2] / 1000.0));
                    }

                    break;
            }
        }
        private void FeedVJoy()
        {
#if ROBUST
                    res = joystick.SetAxis(X, Config.ID, HID_USAGES.HID_USAGE_X);
                    res = joystick.SetAxis(Y, Config.ID, HID_USAGES.HID_USAGE_Y);
                    res = joystick.SetAxis(rX, Config.ID, HID_USAGES.HID_USAGE_RX);
                    res = joystick.SetAxis(rY, Config.ID, HID_USAGES.HID_USAGE_RY);

                    if (ContPovNumber > 0)
                    {
                        res = joystick.SetContPov((int)POV_f, Config.ID, 1);
                    }
#endif
#if EFFICIENT
            iReport.bDevice = (byte)Config.ID;

            iReport.AxisX = X;
            iReport.AxisY = Y;
            iReport.AxisXRot = rX;
            iReport.AxisYRot = rY;

            if (ContPovNumber > 0)
            {
                iReport.bHats = ((uint)POV_f);
                //iReport.bHats = 0xFFFFFFFF; // Neutral state
            }

            /*Feed the driver with the position packet - is fails then wait for input then try to re-acquire device */
            if (!joystick.UpdateVJD(Config.ID, ref iReport))
            {
                Console.WriteLine("Feeding vJoy device number {0} failed - try to enable device then press enter\n", Config.ID);
                Console.ReadKey(true);
                joystick.AcquireVJD(Config.ID);
            }
#endif
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
                Config.Mode = Config.MovementModes.JoystickMoveAndLook;
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
            joystick.ResetVJD(Config.ID);
#endif
#if EFFICIENT
            pov = new byte[4];
#endif
            // get max range of joysticks
            // neutral position is max/2
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxX);
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_Y, ref maxY);
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RX, ref maxRX);
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_RY, ref maxRY);
            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_POV, ref maxPOV);

            ContPovNumber = joystick.GetVJDContPovNumber(id);
        }
    }
}
