using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.XInput;
using CoolFont.IO;
using CoolFont.Network;
using CoolFont.Simulator;
using CoolFont.Utils;


namespace CoolFont
{
    public class Program : SysTray
    {
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);
        public static int globalPort; 
        // A delegate type to be used as the handler routine 
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }


        static void Main(string[] args)
        {
           
           // SysTray.Run(); 
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);
            int tryport = ProcessArgs(args);

            if (tryport == 0)
            {
                tryport = FileManager.ReadPortFromFile(Config.PORT_FILE);
            }

            if (tryport == 0)
            {
                Console.WriteLine("No port given or found: Will select random port.");
            }

            /* Instantiate listener using port */
            UdpListener sock = new UdpListener(tryport);
            int port = sock.port;      

            if (port > 0 & sock.isBound)
            {
                // write successful port to file for next time
                FileManager.WritePortToFile(port, Config.PORT_FILE);
            }

            /* Register DNS service through Java */
            JavaProc.StartDnsService(port); // blocks

            /* Set up the simulator */
            Config.Mode = Config.MovementModes.Mouse2D;
            XInputDeviceManager devMan = new XInputDeviceManager();
            Controller xDevice = devMan.getController();    
            VirtualDevice vDevice = new VirtualDevice(Config.Mode); // will change Mode if necessary

            //TODO: execute loop in background thread and allow user to break out
            int T = 0; // total time
            int maxGapSize = 30; // set to -1 to always interpolate data
            int gapSize = maxGapSize + 1;

            while (true)
            {
                vDevice.logOutput = false;
                bool logRcvd = true;

                /* get data from iPhone socket, add to vDev */
                string rcvd = sock.pollSocket(Config.socketPollInterval);           
                bool res = vDevice.HandleNewData(rcvd);
                gapSize = (res == true) ? 0 : gapSize + 1;

                /* Tell vDev whether we want it to fill in missing data */
                if (gapSize > maxGapSize) { vDevice.shouldInterpolate = false; }

                /* Get data from connected XInput device, add to vDev*/

                if (xDevice != null && xDevice.IsConnected)
                {
                    State state = xDevice.GetState();
                    vDevice.AddControllerState(state);
                }

                vDevice.FeedVJoy();
                T++;

                if (logRcvd && (T % 10 == 0))
                    Console.WriteLine(rcvd);
                if (vDevice.logOutput)
                    Console.Write(" ({0}) \n", gapSize);

            }
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            if (ctrlType == CtrlTypes.CTRL_CLOSE_EVENT)
            {
                JavaProc.Kill();
            }
            return true;
        }

        static int ProcessArgs(string[] args)
        {
            if (args.Length > 0) // port given
            {
                return Convert.ToInt32(args[0]);
            }
            return 0;
        }
    }
   


}
