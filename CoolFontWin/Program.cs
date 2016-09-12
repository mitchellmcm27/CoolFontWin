using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolFontUdp;

namespace CoolFontWin
{

    class Program
    {
        static void Main(string[] args)
        {
            Config.Mode = Config.MovementModes.KeyboardMouse; // default to the most general option
            int tryport;
            if (args.Length == 0) // no port given
            {
                try
                {
                    System.IO.StreamReader file = new System.IO.StreamReader(Config.PORT_FILE);
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
                System.IO.StreamWriter file = new System.IO.StreamWriter(Config.PORT_FILE);
                string hdr = "Last successful port:";
                string port_string = String.Format("{0}", port);
                file.WriteLine(hdr);
                file.WriteLine(port_string);
                file.Close();
            }

            JavaProc.StartDnsService(port); // blocks
            CoolFontSimulator sim = new CoolFontSimulator(Config.Mode);

            string rcvd; 
            char[] delimiters = { ':' };
            int[] vals;
            int t = 0; 

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
                sim.writeOutput = true;
                sim.UpdateCharacterWithValsForMode(vals, Config.Mode);
                if (sim.writeOutput) { Console.Write(" ({0}) \n", t); }
                t++;
            }
            listener.Close();
        }
    }
}
