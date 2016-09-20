using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace CoolFontUdp
{
    public class UdpListener
    {
        /**<summary>
         * Implements a UDP socket to listen on a given port, defaults to 5555.
         * Currently receives packets consisting of a string.
         * Contains a method for splitting string into ints.
         * Helper method to get a local PC IP address .
         * 
         * You can access the underlying UdpClient listener using the public .Listener property.
         * You can access the underlying Socket using the public .listener.Client property.
         * </summary>*/

        private UdpClient _listener;
        public int port;
        public bool isBound = false;
        private byte[] _rcv_bytes;
        private IPEndPoint _senderEP; // network End Point for the device sending packets

        public UdpListener() : this(0) { }

        public UdpListener(int listenPort)
        {

            /* Calls method to choose best address of local network interface */
            /* Supports IPv4 and v6 */
            /* Tries Ethernet first, and then WiFi */
            string[] localAddrs = GetAllLocalIPv4(NetworkInterfaceType.Ethernet);

            if (localAddrs.Length == 0)
            {
                localAddrs = GetAllLocalIPv4(NetworkInterfaceType.Wireless80211);
            }

            if (localAddrs.Length == 0)
            {
                throw new WebException("Must have a reachable network address.");
            }

            IPEndPoint bindEP = new IPEndPoint(IPAddress.Parse(localAddrs[0]), listenPort);
            _senderEP = new IPEndPoint(IPAddress.Any, 0); // will be overwritten by the packet

            _listener = new UdpClient();
            _listener.ExclusiveAddressUse = false;

            /* Client is the underlying Socket */
            _listener.Client.SetSocketOption(SocketOptionLevel.Socket, 
                                            SocketOptionName.ReuseAddress, true);

            /* Bind the socket to a good local address */
            _listener.Client.Bind(bindEP);
            IPEndPoint localEP = (IPEndPoint)_listener.Client.LocalEndPoint;
            port = localEP.Port;
            isBound = _listener.Client.IsBound;
            Console.WriteLine("Listening on " + _listener.Client.LocalEndPoint);
        }

        public string receiveStringSync()
        {
            /* Receive a string from the instantiated UdpClient socket */
            /* This method is synchronous (blocking) */
            /* See below for an asynchronous version (WIP) */

            bool done = false;
            string received_data = "";
            _listener.Client.Blocking = true;

            try
            {
                /* ref keyword: pass by reference, not by value (value can change inside the method) */
                _rcv_bytes = _listener.Receive(ref _senderEP); // blocking

                /* GetString args: byte array, index of first byte, number of bytes to decode */
                received_data = Encoding.ASCII.GetString(_rcv_bytes, 0, _rcv_bytes.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return received_data;
        }

        public string receiveStringAsync()
        {
            bool done = false;
            string received_data = "";
            _listener.Client.Blocking = false;
            try
            {
                _rcv_bytes = _listener.Receive(ref _senderEP); // non-blocking mode, returns immediately
                /* GetString args: byte array, index of first byte, number of bytes to decode */
                received_data = Encoding.ASCII.GetString(_rcv_bytes, 0, _rcv_bytes.Length);
            }
            catch (SocketException se)
            {
                switch (se.ErrorCode)
                {
                    case 10035:
                        return "";      
                }
                throw se;
            }

            return received_data;
        }

        public string pollSocket(int rate_us)
        {
            bool done = false;
            string received_data = "";
            _listener.Client.Blocking = false;
            if (_listener.Client.Poll(rate_us, SelectMode.SelectRead))
            {
                _rcv_bytes = _listener.Receive(ref _senderEP);
                received_data = Encoding.ASCII.GetString(_rcv_bytes, 0, _rcv_bytes.Length);
            }

            return received_data;
        }

        public void Close()
        {
            _listener.Close();
            isBound = false;
            port = 0;
        }

        public int[] parseString2Ints(string instring, char[] delimiterChars)
        {
            /* Given a string representation of ints, split it into ints */
            /* Return int array */

            //Console.WriteLine(instring);
            string[] instring_sep = instring.Split(delimiterChars);
            int[] parsed_ints = new int[4]; // TODO: don't hard code in 4
            int i = 0;
            int j = 0;
            bool copy = true;

            foreach (string s in instring_sep)
            {
                if (s == "$")
                {
                    copy = false;
                }
                if (copy)
                {
                    parsed_ints[j] = Int32.Parse(s);
                    j++;
                }
                i++;
            }

            
            return parsed_ints;
        }

        public int parseButtons(string instring, char[] delimiterChars)
        {
            /* Parse string representation of bitmask (unsigned int) 
             * String array is separated by ":"
             * Assume bitmask follows the "$" string */
             
            string[] instring_sep = instring.Split(delimiterChars);
            bool copy = false;

            foreach (string s in instring_sep)
            {
                if (copy)
                {
                    return int.Parse(s);
                }

                if (s == "$")
                {
                    // next string will be the one
                    copy = true;
                }
            }

            return 0;
        }

        private string[] GetAllLocalIPv4(NetworkInterfaceType _type)
        {
            /* Helper method to choose a local IP address */
            /* Device may have multiple interfaces/addresses, including IPv6 addresses */
            /* Return string array */

            List<string> ipAddrList = new List<string>();

            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return ipAddrList.ToArray();
        }
    } // end of class UDPListener
} // end of namespace

namespace CoolFontIO
{
    public static class FileManager
    {
        public static int ReadPortFromFile(string filename)
        {
            try
            {
                System.IO.StreamReader file = new System.IO.StreamReader(filename);
                string hdr = file.ReadLine();
                int port = Convert.ToInt32(file.ReadLine());
                file.Close();
                return port;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public static void WritePortToFile(int port, string filename)
        {
            System.IO.StreamWriter file = new System.IO.StreamWriter(filename);
            string hdr = "Last successful port:";
            string port_string = String.Format("{0}", port);
            file.WriteLine(hdr);
            file.WriteLine(port_string);
            file.Close();
        }
    }

    public static class JavaProc
    {
        public static Process myProcess = new Process();
        public static int exitCode = 0;
        public static void StartDnsService(int port)
        {    
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

                //string stdoutx = myProcess.StandardOutput.ReadToEnd();
                //string stderrx = myProcess.StandardError.ReadToEnd();
                /*
                myProcess.WaitForExit();
                exitCode = myProcess.ExitCode;
                Console.WriteLine("Exit code : {0}", exitCode);
                */
                //Console.WriteLine("Stdout : {0}", stdoutx);
                //Console.WriteLine("Stderr : {0}", stderrx);
                try
                {
                    exitCode = myProcess.ExitCode;
                }
                catch (InvalidOperationException ioe)
                {
                }
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
            else
            {
                Console.WriteLine("Called java program");
            }
        }
        public static void Kill()
        {
            myProcess.Kill();
            exitCode = myProcess.ExitCode;
            Console.WriteLine("Exit code : {0}", exitCode);
        }
    }
}

namespace CoolFontUtils
{
    public static class Algorithm
    {
        public static int[] LowPassFilter(int[] vals, int[] vals_last, double RC, double dt)
        {
            double alpha = dt / (RC + dt); // smoothing factor, 0 to 1

            for (int i=0; i<vals.Length; i++)
            {
                vals[i] = (int)(vals[i] * alpha + (1.0 - alpha) * vals_last[i]);
            }

            return vals;
        }
    }
}