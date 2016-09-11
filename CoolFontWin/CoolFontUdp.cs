using System;
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

        public UdpClient Listener = new UdpClient();
        public int port;
        public Boolean isBound = false;
        private byte[] receive_byte_array;
        private IPEndPoint senderEP; // network End Point for the device sending packets


        public UdpListener()
            : this(0)
        {
        }
        
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
                throw new Exception("Must have a reachable network address.");
            }

            IPEndPoint bindEP = new IPEndPoint(IPAddress.Parse(localAddrs[0]), listenPort);
            senderEP = new IPEndPoint(IPAddress.Any, 0); // will be overwritten by the packet
            Listener.ExclusiveAddressUse = false;

            /* Client is the underlying Socket */
            Listener.Client.SetSocketOption(SocketOptionLevel.Socket, 
                                            SocketOptionName.ReuseAddress, true);

            /* Bind the socket to a good local address */
            Listener.Client.Bind(bindEP);
            IPEndPoint localEP = (IPEndPoint)Listener.Client.LocalEndPoint;
            port = localEP.Port;
            isBound = Listener.Client.IsBound;
            Console.WriteLine("Listening on " + Listener.Client.LocalEndPoint);
        }

        public string receiveStringSync()
        {
            /* Receive a string from the instantiated UdpClient socket */
            /* This method is synchronous (blocking) */
            /* See below for an asynchronous version (WIP) */

            bool done = false;
            string received_data = "";

            try
            {
                /* ref keyword: pass by reference, not by value (value can change inside the method) */
                receive_byte_array = Listener.Receive(ref senderEP); // blocking

                /* GetString args: byte array, index of first byte, number of bytes to decode */
                received_data = Encoding.ASCII.GetString(receive_byte_array, 0, receive_byte_array.Length);
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
            byte[] receive_byte_array;
            /* 

            ... 

            */
            return received_data;
        }

        public void Close()
        {
            Listener.Close();
        }

        public int[] parseString2Ints(string instring, char[] delimiterChars)
        {
            /* Given a string representation of ints, split it into ints */
            /* Return int array */

            string[] instring_sep = instring.Split(delimiterChars);
            int[] parsed_ints = new int[instring_sep.Length];

            int i = 0;
            foreach (string s in instring_sep)
            {
                parsed_ints[i] = Int32.Parse(s);
                i++;
            }
            return parsed_ints;
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