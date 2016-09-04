#define ROBUST
//#define EFFICIENT

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolFontUdp;
using vJoyInterfaceWrap;

namespace CoolFontWin
{
    class Program
    {
        /**<summary>
         * Main program
         * </summary>*/

        /* vJoy globals */
        // Declaring one joystick (Device id 1) and a position structure. 
        static public vJoy joystick;
        static public vJoy.JoystickState iReport;
        static public uint id = 1;

        /* other globals */
        static string PORT_FILE = "../../../../last-port.txt";
        static int port;

        static void Main(string[] args)
        {
            /* First make sure we have a socket connection */

            if (args.Length==0)
            {
                /* Read the port number written by Java to file */
                /* This has to work so don't put it in try..except */
                System.IO.StreamReader file = new System.IO.StreamReader(PORT_FILE);
                string hdr = file.ReadLine();
                port = Convert.ToInt32(file.ReadLine());
                file.Close();
            }
            else
            {
                port = Convert.ToInt32(args[0]);
            }
   
            /* Instantiate listener using port */
            UdpListener listener = new UdpListener(port); // port must match Java (and iOS)


            /* Now set up vJoy device */
            UInt32 id = 1;

            setUpVJoy(id);

            int X, Y, Z, ZR, XR;
            uint count = 0;
            long maxval = 0;

            string rcvd;
            char[] delimiters = { ':' };
            int[] vals;

#if ROBUST
            bool res;
            // Reset this device to default values
            joystick.ResetVJD(id);
#endif

            joystick.GetVJDAxisMax(id, HID_USAGES.HID_USAGE_X, ref maxval);

            int t = 0; // number of packets to receive
            while (t < 600)
            {
      
                /* Receive one string synchronously */
                rcvd = listener.receiveStringSync();
                /* Parse to int[] */
                vals = listener.parseString2Ints(rcvd, delimiters);

                /*update joystick*/
                X = vals[1]*(int)maxval/1000;
                Y = vals[4]*(int)maxval/1000;
                res = joystick.SetAxis(X, id, HID_USAGES.HID_USAGE_X);
                res = joystick.SetAxis(Y, id, HID_USAGES.HID_USAGE_Y);

                /* Display X,Y,t values  */
                Console.WriteLine("X {0} Y {1} t {2}", X, Y, t);
                t++;
            }
            listener.Close();

        }

        static void setUpVJoy(UInt32 id)
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
                return;
            }
            else
                Console.WriteLine("Vendor: {0}\nProduct :{1}\nVersion Number:{2}\n", joystick.GetvJoyManufacturerString(), joystick.GetvJoyProductString(), joystick.GetvJoySerialNumberString());

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

    }

}
