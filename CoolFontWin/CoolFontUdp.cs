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
         * Implements a UDP socket to listen on a given port, defaults to 5555
         * Currently receives packets consisting of a string
         * Contains a method for splitting string into ints
         * Helper method to get a local PC IP address </summary>
         */

        public UdpClient listener = new UdpClient();
        private byte[] receive_byte_array;
        private IPEndPoint senderEP; // network End Point for the device sending packets

        public UdpListener()
            : this((int)5555)
        {
        }

        public UdpListener(int listenPort)
        {

            /* Call function to choose best address of local network interface */
            /* Supports IPv4 and v6 */
            /* Try Ethernet first, and then wifi */
            string[] localAddrs = GetAllLocalIPv4(NetworkInterfaceType.Ethernet);
            if (localAddrs.Length == 0)
            {
                localAddrs = GetAllLocalIPv4(NetworkInterfaceType.Wireless80211);
            }

            if (localAddrs.Length == 0)
            {
                throw new IndexOutOfRangeException("Must have a reachable network address.");
            }

            IPEndPoint bindEP = new IPEndPoint(IPAddress.Parse(localAddrs[0]), listenPort);
            senderEP = new IPEndPoint(IPAddress.Any, 0); // will be overwritten by the packet
            listener.ExclusiveAddressUse = false;

            /* Client is the underlying Socket */
            listener.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

            /* Bind the socket to a good local address */
            listener.Client.Bind(bindEP);
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
                Console.WriteLine("Listening on " + listener.Client.LocalEndPoint);

                receive_byte_array = listener.Receive(ref senderEP); // blocking

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

        public int[] parseString2Ints(string instring)
        {
            /* Given a string representation of ints, split it into ints */
            /* Return int array */

            char[] delimiterChars = { ':' };
            string[] instring_sep = instring.Split(delimiterChars);
            int[] parsed_ints = new int[] { 0, 0, 0, 0 };

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