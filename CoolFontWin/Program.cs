using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;
using CoolFontUdp;
using CoolFontIO;
using CoolFontUtils;

namespace CoolFontWin
{
    class Program
    {
        static void Main(string[] args)
        {           
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
            UdpListener listener = new UdpListener(tryport);
            int port = listener.port;      

            if (port > 0 & listener.isBound)
            {
                // write successful port to file for next time
                FileManager.WritePortToFile(port, Config.PORT_FILE);
            }

            /* Register DNS service through Java */
            JavaProc.StartDnsService(port); // blocks


            State state;
            /* Set up the simulator */
            XInputDevice dev = new XInputDevice(); // whhat happens if no controller is plugged in?
            Config.Mode = Config.MovementModes.Mouse2D; // try this first 
            CoolFontSimulator sim = new CoolFontSimulator(Config.Mode); // will change Mode if necessary

            string rcvd;
            char[] delimiters = { ':' };
            int[] vals = { 0, 0, 0, 0 };
            int[] vals_last = vals;
            int buttons = 0; // bitmask
            int T = 0; // total time
            int timeout = 30; // set to -1 to block on every socket read
            int tries = timeout + 1;
            sim.logOutput = false;


            //TODO: execute loop in background thread and allow user to break out

            while (true)
            {
                if (IsKeyPressed(ConsoleKey.Escape))
                {
                    break;
                }

                // Receive data from iPhone, parse it, and translate it to the correct inputs
                /* vals[0]: 0-1000: represents user running at 0 to 100% speed.
                 * vals[1]: 0-360,000: represents the direction user is facing (in degrees)
                 * vals[2]: always 0
                 * vals[3]: -infinity to infinity: user rotation rate in radians per second (x1000)
                 */

                // do not block, returns "" if nothing to rcv
                //rcvd = listener.receiveStringAsync();
                rcvd = listener.pollSocket(Config.socketPollInterval);
                try
                {
                    vals = listener.parseString2Ints(rcvd, delimiters);
                    buttons = listener.parseButtons(rcvd, delimiters);
                    vals = Algorithm.LowPassFilter(vals,vals_last,Config.RCFilterStrength,Config.dt);
                    vals_last = vals;
                    tries = 0;
                }
                catch
                {
                     // assume empty packet
                    if (tries <= timeout)
                    {
                        vals = vals_last;
                    }
                    tries++; // number of empty packets
                }

                /* Get input from connected XInput device */
                sim.AddValues(vals, Config.Mode);
               // sim.AddButtons(buttons, Config.Mode);

                if (dev.controller != null && dev.controller.IsConnected)
                {
                    state = dev.controller.GetState();
                    sim.AddControllerState(state);
                }
                sim.AddButtons(buttons, Config.Mode);
                sim.FeedVJoy();
                sim.ResetValues();
                if (sim.logOutput) { Console.Write(" ({0}) \n", tries); }
                T++;
            }

            sim.DisableVJoy(Config.ID);
            JavaProc.Kill();
            listener.Close();
            dev.controller = null;
            dev = null;
        }

        public static bool IsKeyPressed(ConsoleKey key)
        {
            return false; // Console.KeyAvailable && Console.ReadKey(true).Key == key;
        }

        static int ProcessArgs(string[] args)
        {
            if (args.Length > 0) // no port given
            {
                return Convert.ToInt32(args[0]);
            }
            return 0;
        }
    }
}
