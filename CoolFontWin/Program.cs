using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolFontUdp;
using CoolFontIO;

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

            /* Set up the simulator */
            Config.Mode = Config.MovementModes.Mouse2D; // try this first 
            CoolFontSimulator sim = new CoolFontSimulator(Config.Mode); // will change Mode if necessary

            string rcvd;
            char[] delimiters = { ':' };
            int[] vals;
            int[] vals_last = sim.neutralVals;
            int T = 0; // total time
            int timeout = 30; // set to -1 to block on every socket read
            int tries = timeout + 1;
            sim.logOutput = true;

            //TODO: execute loop in background thread and allow user to break out
            while (true)
            {
                // Receive data from iPhone, parse it, and translate it to the correct inputs
                /* vals[0]: 0-1000: represents user running at 0 to 100% speed.
                 * vals[1]: 0-360,000: represents the direction user is facing (in degrees)
                 * vals[2]: always 0
                 * vals[3]: -infinity to infinity: user rotation rate in radians per second (x1000)
                 */
                if (tries > timeout)
                {
                    /* block until we git the first packet 
                     * or if we've gotten 10 empty packets
                     * then reset counter
                     */
                    rcvd = listener.receiveStringSync(); 
                    vals = listener.parseString2Ints(rcvd, delimiters);
                    vals_last = vals;
                    tries = 0;
                }
                else
                {
                    // do not block, returns "" if nothing to rcv
                    //rcvd = listener.receiveStringAsync();
                    rcvd = listener.pollSocket(Config.socketPollInterval);
                    try
                    {
                        vals = listener.parseString2Ints(rcvd, delimiters);
                        vals_last = vals;
                        tries = 0;
                    }
                    catch
                    {
                        // assume empty packet
                        vals = vals_last;
                        tries++; // number of empty packets
                    }
                }
     
                // Depending on selected mode, translate vals in the correct way
                // Also feed vjoy device if needed
                sim.UpdateCharacterWithValsForMode(vals, Config.Mode);

                if (sim.logOutput) { Console.Write(" ({0}) \n", tries); }
                T++;
                if (T==10)
                {
                    JavaProc.Kill();
                }
            }

            listener.Close();
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
