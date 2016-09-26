using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace CoolFont
{
    namespace Network
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

            private UdpClient Listener;
            private byte[] RcvBytes;
            private IPEndPoint SenderEP; // network End Point for the device sending packets

            public int Port;
            public bool IsBound = false;
            public string[] LocalAddrs;

            public UdpListener() : this(0)
            {
            }

            public UdpListener(int listenPort)
            {

                /* Calls method to choose best address of local network interface */
                /* Supports IPv4 and v6 */
                /* Tries Ethernet first, and then WiFi */

                LocalAddrs = GetAllLocalIPv4(NetworkInterfaceType.Ethernet);

                if (LocalAddrs.Length == 0)
                {
                    LocalAddrs = GetAllLocalIPv4(NetworkInterfaceType.Wireless80211);
                }

                if (LocalAddrs.Length == 0)
                {
                    throw new WebException("Must have a reachable network address.");
                }

                IPEndPoint bindEP = new IPEndPoint(IPAddress.Parse(LocalAddrs[0]), listenPort);
                SenderEP = new IPEndPoint(IPAddress.Any, 0); // will be overwritten by the packet

                Listener = new UdpClient();
                Listener.ExclusiveAddressUse = false;

                /* Client is the underlying Socket */
                Listener.Client.SetSocketOption(SocketOptionLevel.Socket,
                                                SocketOptionName.ReuseAddress, true);

                /* Bind the socket to a good local address */
                Listener.Client.Bind(bindEP);
                IPEndPoint localEP = (IPEndPoint)Listener.Client.LocalEndPoint;
                Port = localEP.Port;
                IsBound = Listener.Client.IsBound;
                Console.WriteLine("Listening on " + Listener.Client.LocalEndPoint);
            }

            public string ReceiveStringSync()
            {
                /* Receive a string from the instantiated UdpClient socket */
                /* This method is synchronous (blocking) */
                /* See below for an asynchronous version (WIP) */

                bool done = false;
                string received_data = "";
                Listener.Client.Blocking = true;

                try
                {
                    /* ref keyword: pass by reference, not by value (value can change inside the method) */
                    RcvBytes = Listener.Receive(ref SenderEP); // blocking

                    /* GetString args: byte array, index of first byte, number of bytes to decode */
                    received_data = Encoding.ASCII.GetString(RcvBytes, 0, RcvBytes.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

                return received_data;
            }

            public string ReceiveStringAsync()
            {
                bool done = false;
                string received_data = "";
                Listener.Client.Blocking = false;
                try
                {
                    RcvBytes = Listener.Receive(ref SenderEP); // non-blocking mode, returns immediately
                                                                   /* GetString args: byte array, index of first byte, number of bytes to decode */
                    received_data = Encoding.ASCII.GetString(RcvBytes, 0, RcvBytes.Length);
                }
                catch (SocketException se)
                {
                    switch (se.ErrorCode) // nothing there
                    {
                        case 10035:
                            return "";
                    }
                    throw se;
                }

                return received_data;
            }

            public int SocketPollInterval = 8 * 1000; // microseconds (us)
            public string Poll()
            {
                bool done = false;
                string received_data = "";
                Listener.Client.Blocking = false;
                if (Listener.Client.Poll(SocketPollInterval, SelectMode.SelectRead))
                {
                    RcvBytes = Listener.Receive(ref SenderEP);
                    received_data = Encoding.ASCII.GetString(RcvBytes, 0, RcvBytes.Length);
                }

                return received_data;
            }

            public void Close()
            {
                /* Close but do not delete */
                Listener.Close();
                IsBound = false;
            } 

            private string[] GetAllLocalIPv4(NetworkInterfaceType type)
            {
                /* Helper method to choose a local IP address */
                /* Device may have multiple interfaces/addresses, including IPv6 addresses */
                /* Return string array */

                List<string> ipAddrList = new List<string>();

                foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
                {
                    if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
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
    }

    namespace IO
    {
        public static class FileManager
        {
            public static int TryToReadPortFromFile(string filename)
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

        public class JavaProc
        {
            private Process MyProcess;
            private int ExitCode;

            public bool Running;

            public JavaProc()
            {
                MyProcess = new Process();
                ExitCode = 0;
                Running = false;
            }
            public void StartDnsService(int port)
            {
                try
                {
                    MyProcess.StartInfo.UseShellExecute = false;
                    MyProcess.StartInfo.RedirectStandardError = true;
                    MyProcess.StartInfo.RedirectStandardOutput = true;
                    MyProcess.StartInfo.CreateNoWindow = true;

                    MyProcess.StartInfo.FileName = "java.exe"; // for some reason VS calls java 7
                    String jarfile = "../../../../testapp-java.jar";
                    String arg0 = String.Format("{0}", port); // -r: register, -u: unregister, -b: both (not useful?)
                    String arg1 = "-r";
                    MyProcess.StartInfo.Arguments = String.Format("-jar {0} {1} {2}", jarfile, arg0, arg1);

                    MyProcess.Start();
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
                        ExitCode = MyProcess.ExitCode;
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

                if (ExitCode == 1)
                {
                    Console.WriteLine("DNS service failed to register. Check location of testapp-java.jar");
                    Console.WriteLine("Press any key to quit");
                    Console.ReadKey();
                    return;
                }
                else
                {
                    Running = true;
                    Console.WriteLine("Called java program");
                }
            }
            public void Kill()
            {
                MyProcess.Kill();
                ExitCode = MyProcess.ExitCode;
                Console.WriteLine("JavaProc Exit code : {0}", ExitCode);
                Running = false;
            }
        }
    }

    namespace Utils
    {
        public static class Algorithm
        {

            public static double LowPassFilter(double val, double last, double RC, double dt)
            {
                if (last ==  0) // If it's a valid 0 it doesn't make a difference in the filter
                    return val;

                double alpha = dt / (RC + dt); // smoothing factor, 0 to 1

                val = val * alpha + last * (1.0 - alpha);

                return val;
            }

            public static double WrapAngle(double ang)
            {
                while (ang > 360) { ang -= 360; }
                while (ang < 0) { ang += 360; }

                return ang;
            }

            public static double WrapQ2toQ4(double ang)
            {
                while (ang > 180) { ang -= 360; }
                while (ang < -180) { ang += 360; }

                return ang;
            }

            public static double Clamp(double val, double min, double max)
            {
                return Math.Min(Math.Max(val, min), max);
            }
        }
    }
}