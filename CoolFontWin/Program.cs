#define ROBUST
//#define EFFICIENT
//TODO:Implement Efficient vJoy feeder (see vjoy-sdk-ex.cs)

using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolFontUdp;
using vJoyInterfaceWrap;
using WindowsInput;

namespace CoolFontWin
{
    class Program
    {
        static public vJoy joystick;
        static public vJoy.JoystickState iReport;
        static public UInt32 id = 1;
        static public int ContPovNumber;

        static public long maxX = 0;
        static public long maxY = 0;
        static public long maxRX = 0;
        static public long maxRY = 0;
        static public long maxPOV = 0;

        static double THRESH_RUN = 0.7;
        static double THRESH_WALK = 0.3;

        static InputSimulator sim = new InputSimulator();
        static int X, Y, rX, rY, POV, d_theta;
        static double POV_f, d_theta_f;
        static bool res;

        public enum MovementModes
        {
            // Controls how the character moves in-game
            None = 0, //TODO: Implement a "pause" button in iOS, useful for navigating menus
            KeyboardMouse, // Full KBM control
            JoystickMove, // Use vJoy/XOutput to move character through game (strafe only, no turning). VR MODE.
            JoystickMoveAndLook, //TODO: Move character forward and turn L/R using joystick. Difficult.
            Mouse2D, // tilt the phone L/R U/D to move the mouse pointer
        };
        static public MovementModes MODE = MovementModes.KeyboardMouse; // default to the most general option

        static string PORT_FILE = "../../../../last-port.txt";

        static void Main(string[] args)
        {
            int tryport;

            if (args.Length == 0) // no port given
            {
                try
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(PORT_FILE);
                    string hdr = file.ReadLine();
                    tryport = Convert.ToInt32(file.ReadLine());
                    file.Close();
                }
                catch (Exception e)
                {
                    tryport = 0; // UdpListener will decide the port
                }
            }
            else // port was passed as an arg
            {
                tryport = Convert.ToInt32(args[0]);
            } 
        
            /* Instantiate listener using port */
            UdpListener listener = new UdpListener(tryport);
            int port = listener.port;
            // write successful port to file
            if (port > 0 & listener.isBound) 
            {
                System.IO.StreamWriter file = new System.IO.StreamWriter(PORT_FILE);
                string hdr = "Last successful port:";
                string port_string = String.Format("{0}", port);
                file.WriteLine(hdr);
                file.WriteLine(port_string);
                file.Close();
            }

            StartDnsService(port); // blocks
            Console.WriteLine("Called java program");
     
            /* Initialize socket IO stuff */
            string rcvd; // packet data will be recvd as a string
            char[] delimiters = { ':' }; // valid delimiters for separating string into ints
            int[] vals; // where the packet's ints will go

            /* vJoy setup? */
            // Try to start vjoy
            StartVJoy(id);
            if (MODE==MovementModes.JoystickMove || MODE==MovementModes.JoystickMoveAndLook)
            {    
                SetUpVJoy();
            }

            int t = 0; // number of packets rcvd

            //TODO: execute loop in background thread and allow user to break out
            while (true)
            {
                // Receive data from iPhone, parse it, and translate it to the correct inputs
                /* vals[0]: 0-1000: represents user running at 0 to 100% speed.
                 * vals[1]: 0-360,000: represents the direction user is facing (in degrees)
                 * vals[2]: always 0
                 * vals[3]: 0-infinity: user rotation rate in radians per second (x1000)
                 */
              
                rcvd = listener.receiveStringSync();
                vals = listener.parseString2Ints(rcvd, delimiters);
                // Depending on selected mode, translate vals in the correct way
                // Also feed vjoy device if needed
                UpdateCharacterWithValsForMode(vals, MODE); 
                Console.WriteLine("X {0} Y {1} d {2} t {3}", X, Y, d_theta, t);
                t++;
            }
            listener.Close();
        }
        static void UpdateCharacterWithValsForMode(int[] vals, MovementModes mode)
        {
            switch (mode)
            {
                //TODO: Hold CTRL+W when under running threshold but over walking threshold
                case MovementModes.KeyboardMouse:  
                    sim.Mouse.MoveMouseBy((int)(10.0*vals[3]/1000.0), 0); // dx, dy (pixels)
                    if (vals[0] >= THRESH_RUN)
                    {
                        sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                    }
                    else
                    {
                        sim.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                    }

                    //TODO: Implement jumping on iPhone, send it in the 3rd int value
                    if (vals[2] > 0) 
                    {
                        // vals[2] is always 0 right now so this never executes
                        sim.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
                    }
                    break;

                case MovementModes.JoystickMove:

                    POV_f = vals[1]/1000.0/360.0 * maxPOV;

                    X = (int)(Math.Cos(POV_f/maxPOV * Math.PI*2) * -vals[0]/1000.0*maxX/2 + maxX/2);
                    Y = (int)(Math.Sin(POV_f/maxPOV * Math.PI*2) * -vals[0]/1000.0*maxY/2 + maxY/2);

                    rX = (int)maxRX/2;
                    rY = (int)maxRX/2;

                    /* Feed vJoy device */
                    res = joystick.SetAxis(X, id, HID_USAGES.HID_USAGE_X);
                    res = joystick.SetAxis(Y, id, HID_USAGES.HID_USAGE_Y);
                    res = joystick.SetAxis(rX, id, HID_USAGES.HID_USAGE_RX);
                    res = joystick.SetAxis(rY, id, HID_USAGES.HID_USAGE_RY);
                    if (ContPovNumber > 0)
                    {
                        res = joystick.SetContPov((int)POV_f, id, 1);
                    }
                    break;
                
                //TODO:
                case MovementModes.JoystickMoveAndLook:
                    // NOT FINISHED YET
                    POV_f = vals[1]/1000.0/360.0 * maxPOV;

                    X = (int)maxX / 2; // no strafing
                    Y = -vals[0] * (int)maxY/1000/2 + (int)maxY/2;

                    rX = (int)maxRX / 2; // needs to change
                    rY = (int)maxRX / 2; // look up/down

                    res = joystick.SetAxis(X, id, HID_USAGES.HID_USAGE_X);
                    res = joystick.SetAxis(Y, id, HID_USAGES.HID_USAGE_Y);
                    res = joystick.SetAxis(rX, id, HID_USAGES.HID_USAGE_RX);
                    res = joystick.SetAxis(rY, id, HID_USAGES.HID_USAGE_RY);
                    if (ContPovNumber > 0)
                    {
                        res = joystick.SetContPov((int)POV_f, id, 1);
                    }
                    break;

                case MovementModes.Mouse2D:
                    sim.Mouse.MoveMouseBy((int)(30.0 * vals[3] / 1000.0), // negative because device is not assumed upside down
                                          (int)(60.0 * vals[2] / 1000.0)); // dx, dy (pixels)
                    break;
            }
        }
        static void StartVJoy(UInt32 id)
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
                Console.WriteLine("Defaulting to keyboard simulation.");
                //MODE = MovementModes.KeyboardMouse;
                MODE = MovementModes.Mouse2D;
                return;
            }
            else
            {
                Console.WriteLine("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", joystick.GetvJoyManufacturerString(), joystick.GetvJoyProductString(), joystick.GetvJoySerialNumberString());
                MODE = MovementModes.JoystickMove;
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
                Console.WriteLine("Version of Driver Matches DLL Version ({0:X})\n", DllVer);
            else
                Console.WriteLine("Version of Driver ({0:X}) does NOT match DLL Version ({1:X})\n", DrvVer, DllVer);


            // Acquire the target
            if ((status == VjdStat.VJD_STAT_OWN) || ((status == VjdStat.VJD_STAT_FREE) && (!joystick.AcquireVJD(id))))
            {
                Console.WriteLine("Failed to acquire vJoy device number {0}.\n", id);
                return;
            }
            else
                Console.WriteLine("Acquired: vJoy device number {0}.\n", id);

        }
        static void SetUpVJoy()
        {
            #if ROBUST
            // Reset this device to default values
            joystick.ResetVJD(id);
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
        static void StartDnsService(int port)
        {
            Process myProcess = new Process();
            int exitCode = 0;
            try
            {
                myProcess.StartInfo.UseShellExecute = false;
                myProcess.StartInfo.RedirectStandardError = true;
                myProcess.StartInfo.RedirectStandardOutput = true;
                myProcess.StartInfo.CreateNoWindow = true;

                myProcess.StartInfo.FileName = "java.exe"; // for some reason VS calls java 7
                String jarfile = "../../../../testapp-java.jar";
                String arg0 = String.Format("{0}", port); // -r: register, -u: unregister, -b: both (not useful?)
                String arg1 = "-r";
                myProcess.StartInfo.Arguments = String.Format("-jar {0} {1} {2}", jarfile, arg0, arg1);

                myProcess.Start();
                // This code assumes the process you are starting will terminate itself. 
                // Given that is is started without a window so you cannot terminate it 
                // on the desktop, it must terminate itself or you can do it programmatically
                // from this application using the Kill method.

                string stdoutx = myProcess.StandardOutput.ReadToEnd();
                string stderrx = myProcess.StandardError.ReadToEnd();
                myProcess.WaitForExit();
                exitCode = myProcess.ExitCode;
                Console.WriteLine("Exit code : {0}", exitCode);
                Console.WriteLine("Stdout : {0}", stdoutx);
                Console.WriteLine("Stderr : {0}", stderrx);
            }
            catch (System.ComponentModel.Win32Exception w)
            {
                Console.WriteLine(w.Message);
                Console.WriteLine(w.ErrorCode.ToString());
                Console.WriteLine(w.NativeErrorCode.ToString());
                Console.WriteLine(w.StackTrace);
                Console.WriteLine(w.Source);
                Exception e = w.GetBaseException();
                Console.WriteLine(e.Message);
            }

            if (exitCode == 1)
            {
                Console.WriteLine("DNS service failed to register. Check location of testapp-java.jar");
                Console.WriteLine("Press any key to quit");
                Console.ReadKey();
                return;
            }
        }

    }

}
