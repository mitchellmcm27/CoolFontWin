using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoolFontUdp; 


namespace CoolFontWin
{
    class Program
    {
        /**<summary>
         * Program to test UdpListener class 
         * Receives a set number of packets and displays them in the console </summary>
         */
        static void Main(string[] args)
        {
            /* Instantiate listener using port */
            UdpListener listener = new UdpListener(5555); // port must match Java (and iOS)

            int t = 0; // number of packets to receive
            while (t < 600)
            {
                /* Receive one string */
                string rcvd = listener.receiveStringSync();
                /* Parse to int[] */
                int[] vals = listener.parseString2Ints(rcvd);

                /* Display int values one by one */
                int i = 0;
                foreach (int val in vals)
                {
                    if (i == 0)
                    {
                        Console.WriteLine("Parsed integers:");
                    }
                    Console.WriteLine("x" + i + ": %d{0}", val);
                    i++;
                }
            }
                listener.listener.Close();
        }
    }
}
