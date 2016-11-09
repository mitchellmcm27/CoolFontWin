using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Mono.Zeroconf;
using log4net;

namespace CoolFont.Business
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
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private UdpClient Listener;
        public RegisterService Service;

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

            Listener = new UdpClient();
            Listener.ExclusiveAddressUse = false;

            /* Client is the underlying Socket */
            Listener.Client.SetSocketOption(SocketOptionLevel.Socket,
                            SocketOptionName.ReuseAddress, true);

            /* Bind the socket to a good local address */
            IPEndPoint bindEP;
            try
            {
                bindEP = new IPEndPoint(IPAddress.Parse(LocalAddrs[0]), listenPort);
                Listener.Client.Bind(bindEP);
            }
            catch (SocketException sockEx)
            {
                log.Warn("Unable to bind to socket: " + sockEx);
                log.Info("Trying a random port");
                bindEP = new IPEndPoint(IPAddress.Parse(LocalAddrs[0]), 0); // randomly select a port
                Listener.Client.Bind(bindEP);
            }

            IPEndPoint localEP = (IPEndPoint)Listener.Client.LocalEndPoint;
            Port = localEP.Port;
            IsBound = Listener.Client.IsBound;

            SenderEP = new IPEndPoint(IPAddress.Any, 0); // will be overwritten by the packet

            log.Info("Listening on " + Listener.Client.LocalEndPoint);
        }

        public bool PublishOnPort(short port, string appendToName)
        {
            bool res = false;

            Service = new RegisterService();
            string name;

            try
            {
                name = Environment.MachineName.ToLower();
            }
            catch (Exception e)
            {
                name = "PocketStrafe Companion";
            }

            name += (" - " + appendToName);

            Service.Name = name;
            Service.RegType = "_IAmTheBirdman._udp";
            Service.ReplyDomain = "local.";
            Service.Port = port;

            TxtRecord record = null;

            log.Info(String.Format("!! Registering name = '{0}', type = '{1}', domain = '{2}'",
                Service.Name,
                Service.RegType,
                Service.ReplyDomain));

            Service.Response += OnRegisterServiceResponse;
            Service.Register();
            res = true;
            return res;
        }

        private void OnRegisterServiceResponse(object o, RegisterServiceEventArgs args)
        {
            switch (args.ServiceError)
            {
                case ServiceErrorCode.NameConflict:
                    log.Error(String.Format("!! Name Collision! '{0}' is already registered",
                        args.Service.Name));
                    break;
                case ServiceErrorCode.None:
                    log.Info(String.Format("!! Registered name = '{0}'", args.Service.Name));
                    break;
                case ServiceErrorCode.Unknown:
                    log.Error(String.Format("!! Error registering name = '{0}'", args.Service.Name));
                    break;
            }
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
                log.Error(e.ToString());
            }

            return received_data;
        }

        public void MakeFirewallPopup()
        {
            IPAddress ipAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList[0];
            IPEndPoint ipLocalEndPoint = new IPEndPoint(ipAddress, 12345);

            TcpListener t = new TcpListener(ipLocalEndPoint);
            t.Start();
            t.Stop();
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
            Listener.Client.Blocking = false;
            if (Listener.Client.Poll(SocketPollInterval, SelectMode.SelectRead))
            {
                RcvBytes = Listener.Receive(ref SenderEP);
                return Encoding.ASCII.GetString(RcvBytes, 0, RcvBytes.Length);
            }

            return "";
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
