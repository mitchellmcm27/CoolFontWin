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
using System.Collections;

namespace CoolFont.Business
{
    public class UdpSocketManager
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
        public ArrayList ListenList;
        public List<int> ListenPorts;

        private byte[] RcvBytes = new byte[256];
        private IPEndPoint SenderEP; // network End Point for the device sending packets

        public string[] LocalAddrs;

        public UdpSocketManager()
        {
            // Calls method to choose best address of local network interface
            // Supports IPv4 and v6
            // Tries Ethernet first, and then WiFi

            LocalAddrs = GetAllLocalIPv4(NetworkInterfaceType.Ethernet);

            if (LocalAddrs.Length == 0)
            {
                LocalAddrs = GetAllLocalIPv4(NetworkInterfaceType.Wireless80211);
            }

            if (LocalAddrs.Length == 0)
            {
                throw new WebException("Must have a reachable network address.");
            }

            ListenList = new ArrayList();
            ListenPorts = new List<int>();
        }

        public bool Publish(int port, string appendToName)
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
            Service.Port = (short)port;

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

        private string[] GetAllLocalIPv4(NetworkInterfaceType type)
        {
            // Helper method to choose a local IP address
            // Device may have multiple interfaces/addresses, including IPv6 addresses 
            // Return string array

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

    public class UDPServer
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket serverSocket = null;
        private List<EndPoint> clientList = new List<EndPoint>();
        private List<Tuple<EndPoint, byte[]>> dataList = new List<Tuple<EndPoint, byte[]>>();
        private byte[] byteData = new byte[1024];

        public DeviceManager DevManager;

        public int port = 4242;

        public List<Tuple<EndPoint, byte[]>> DataList
        {
            private set { this.dataList = value; }
            get { return (this.dataList); }
        }

        public UDPServer(int port)
        {
            this.port = port;
            this.DevManager = new DeviceManager();
        }

        public void Start()
        {
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.serverSocket.Bind(new IPEndPoint(IPAddress.Any, this.port));
            this.port = ((IPEndPoint)this.serverSocket.LocalEndPoint).Port;
            EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
            this.serverSocket.BeginReceiveFrom(this.byteData, 0, this.byteData.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, newClientEP);
        }

        private void DoReceiveFrom(IAsyncResult iar)
        {
            try
            {
                EndPoint clientEP = new IPEndPoint(IPAddress.Any, 0);
                int dataLen = 0;
                byte[] data = null;
                try
                {
                    dataLen = this.serverSocket.EndReceiveFrom(iar, ref clientEP);
                    data = new byte[dataLen];
                    Array.Copy(this.byteData, data, dataLen);
                }
                catch (Exception e)
                {
                }
                finally
                {
                    EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
                    this.serverSocket.BeginReceiveFrom(this.byteData, 0, this.byteData.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, newClientEP);
                }

                if (!this.clientList.Any(client => client.Equals(clientEP)))
                    this.clientList.Add(clientEP);

                //DataList.Add(Tuple.Create(clientEP, data));
                //log.Info(data.ToString());
                DevManager.PassDataToDevices(data);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void SendTo(byte[] data, EndPoint clientEP)
        {
            try
            {
                this.serverSocket.SendTo(data, clientEP);
            }
            catch (System.Net.Sockets.SocketException)
            {
                this.clientList.Remove(clientEP);
            }
        }

        public void SendToAll(byte[] data)
        {
            foreach (var client in this.clientList)
            {
                this.SendTo(data, client);
            }
        }

        public void Stop()
        {
            this.serverSocket.Close();
            this.serverSocket = null;

            this.dataList.Clear();
            this.clientList.Clear();
        }
    }
}
