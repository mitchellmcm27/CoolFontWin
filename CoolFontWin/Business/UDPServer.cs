using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using log4net;

namespace CFW.Business
{
    public class UDPServer
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private Socket serverSocket = null;
        private List<EndPoint> clientList = new List<EndPoint>();
        private List<Tuple<EndPoint, byte[]>> dataList = new List<Tuple<EndPoint, byte[]>>();
        private byte[] byteData = new byte[1024];

        private DeviceManager SharedDeviceManager = DeviceManager.Instance;

        public int port = 4242;

        public List<Tuple<EndPoint, byte[]>> DataList
        {
            private set { this.dataList = value; }
            get { return (this.dataList); }
        }

        public UDPServer(int port)
        {
            this.port = port;
        }

        public void Start()
        {
            this.serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.serverSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.serverSocket.Bind(new IPEndPoint(IPAddress.Any, this.port));
            this.port = ((IPEndPoint)this.serverSocket.LocalEndPoint).Port;
            EndPoint newClientEP = new IPEndPoint(IPAddress.Any, 0);
            log.Info("!! Ready to receive data.");
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
                {
                    log.Info("!! Began receiving from device " + clientEP.ToString());
                    this.clientList.Add(clientEP);
                }

                //DataList.Add(Tuple.Create(clientEP, data));
                //log.Info(data.ToString());
                SharedDeviceManager.PassDataToDevices(data);
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
