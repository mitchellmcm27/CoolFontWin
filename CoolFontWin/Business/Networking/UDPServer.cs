﻿using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace PocketStrafe
{
    /// <summary>
    /// Asynchronous UDP listen server. Passes data to DeviceManager.
    /// </summary>
    public class UDPServer
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public event EventHandler<EventArgs> ClientAdded;

        private Socket ServerSocket;
        private List<EndPoint> ClientList;
        private byte[] ByteData = new byte[1024];
        private PocketStrafeDeviceManager DeviceManager;
        public int Port;

        private void OnClientAdded()
        {
            ClientAdded?.Invoke(this, null);
        }

        public UDPServer(PocketStrafeDeviceManager d)
        {
            ServerSocket = null;
            ClientList = new List<EndPoint>();
            Port = 0;
            DeviceManager = d;
        }

        /// <summary>
        /// Start listening for UDP packets on a random port.
        /// </summary>
        public void Start()
        {
            Start(0);
        }

        /// <summary>
        /// Start listening for UDP packets on given port.
        /// </summary>
        /// <param name="port">Port to bind to socket.</param>
        public void Start(int port)
        {
            log.Info("Starting UDP server given port " + port.ToString());
            this.Port = port;

            // create IPv6 socket, and enable dual mode so that it supports IPv4
            this.ServerSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Dgram, ProtocolType.Udp);
            this.ServerSocket.DualMode = true; // ipv4 and v6; default is false
            this.ServerSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            this.ServerSocket.EnableBroadcast = false; // default is false

            this.ServerSocket.Bind(new IPEndPoint(IPAddress.IPv6Any, this.Port));
            this.Port = ((IPEndPoint)this.ServerSocket.LocalEndPoint).Port;

            EndPoint newClientEP = new IPEndPoint(IPAddress.IPv6Any, 0);
            log.Info("!! Ready to receive on port " + this.Port.ToString());
            this.ServerSocket.BeginReceiveFrom(this.ByteData, 0, this.ByteData.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, newClientEP);
        }

        private void DoReceiveFrom(IAsyncResult iar)
        {
            try
            {
                EndPoint clientEP = new IPEndPoint(IPAddress.IPv6Any, 0);
                int dataLen = 0;
                byte[] data = null;
                try
                {
                    // get the received message
                    dataLen = this.ServerSocket.EndReceiveFrom(iar, ref clientEP);
                    data = new byte[dataLen];
                    Array.Copy(this.ByteData, data, dataLen);
                }
                catch (Exception e)
                {
                }
                finally
                {
                    // start listening for a new message
                    EndPoint newClientEP = new IPEndPoint(IPAddress.IPv6Any, 0);
                    this.ServerSocket.BeginReceiveFrom(this.ByteData, 0, this.ByteData.Length, SocketFlags.None, ref newClientEP, DoReceiveFrom, newClientEP);
                }

                if (!this.ClientList.Any(client => client.Equals(clientEP)))
                {
                    log.Info("!! Began receiving from device " + clientEP.ToString());
                    this.ClientList.Add(clientEP);
                    OnClientAdded();
                }

                // send data to device manager singleton (vjoy etc)
                DeviceManager.PassDataToDevices(data);
            }
            catch (ObjectDisposedException)
            {
            }
        }

        public void SendTo(byte[] data, EndPoint clientEP)
        {
            try
            {
                this.ServerSocket.SendTo(data, clientEP);
            }
            catch (System.Net.Sockets.SocketException)
            {
                this.ClientList.Remove(clientEP);
            }
        }

        public void SendToAll(byte[] data)
        {
            foreach (var client in this.ClientList)
            {
                this.SendTo(data, client);
            }
        }

        public void Stop()
        {
            this.ServerSocket.Close();
            this.ServerSocket = null;
            this.ClientList.Clear();
        }
    }
}