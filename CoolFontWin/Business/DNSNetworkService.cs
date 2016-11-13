using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Mono.Zeroconf;
using log4net;
using System.Collections;

namespace CFW.Business
{

    /// <summary>
    /// Manages DNS Services through Mono.Zeroconf. Works with and requires Bonjour.
    /// </summary>
    public class DNSNetworkService
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

        public List<RegisterService> Services = new List<RegisterService>();
        public List<string> LocalAddrs;

        public DNSNetworkService()
        {
            // Choose best address of local network interface
            // Supports IPv4 and v6
            // Tries Ethernet first, and then WiFi

            LocalAddrs = new List<string>();

            try
            {
                
                log.Info("Will search for LAN IPv4 addresses for this computer...");
                LocalAddrs.AddRange(GetAddresses(NetworkInterfaceType.Ethernet, AddressFamily.InterNetwork));

                log.Info("Will search for WiFi IPv4 addresses...");
                LocalAddrs.AddRange(GetAddresses(NetworkInterfaceType.Wireless80211, AddressFamily.InterNetwork));

                log.Info("Will search for LAN IPv6 addresses for this computer...");
                LocalAddrs.AddRange(GetAddresses(NetworkInterfaceType.Ethernet, AddressFamily.InterNetworkV6));

                log.Info("Will search for WiFi IPv6 addresses...");
                LocalAddrs.AddRange(GetAddresses(NetworkInterfaceType.Wireless80211, AddressFamily.InterNetworkV6));
            }
            catch (Exception e)
            {
                // If something goes wrong, it's probably not a big deal
                log.Warn("Something went wrong getting addresses: " + e);
            }

            if (LocalAddrs.Count == 0)
            {
                log.Error("No addresses found!");
                log.Error("Must have reachable network address!");
            }

            log.Info("Found " + LocalAddrs.Count.ToString() + " addresses.");

        }

        public bool Publish(int port, string appendToName)
        {
            var service = new RegisterService();
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

            service.Name = name;
            service.RegType = "_IAmTheBirdman._udp";
            service.ReplyDomain = "local.";
            service.Port = (short)port;

            TxtRecord record = null;

            log.Info(String.Format("!! Registering name = '{0}', type = '{1}', domain = '{2}'",
                service.Name,
                service.RegType,
                service.ReplyDomain));

            service.Response += OnRegisterServiceResponse;
            service.Register();

            Services.Add(service);
  
            return true;
        }

        public void Unpublish(string appendToName)
        {
            // do not allow first service to be unpublished
            if (Services.Count == 1) return;

            RegisterService serviceToDelete = null;
            foreach (var service in Services)
            {
                if (service.Name.ToLower().Contains(appendToName.ToLower()))
                {
                    serviceToDelete = service;
                    break;
                }
            }

            if(serviceToDelete!=null)
            {
                serviceToDelete.Dispose();
                Services.Remove(serviceToDelete);
            }
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
                    log.Info(String.Format("!! Successfully registered name = '{0}'", args.Service.Name));
                    break;
                case ServiceErrorCode.Unknown:
                    log.Error(String.Format("!! Unknown Error registering name = '{0}'", args.Service.Name +". Details: " + args.ServiceError.ToString()));
                    break;
            }
        }

        private List<string> GetAddresses(NetworkInterfaceType type, AddressFamily family)
        {
            // Helper method to choose a local IP address
            // Device may have multiple interfaces/addresses, including IPv6 addresses 
            // Return string array

            List<string> ipAddrList = new List<string>();
            log.Info("Try to get " + family.ToString() + " addresses on interface " + type.ToString()+".");
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    log.Info("Found operational item: " + item.Name);
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == family)
                        {
                            log.Info("ADDRESS");
                            log.Info("Inforamation:\n" + "...Address: " + ip.Address.ToString() + "\n...IPv4 Mask: " + ip.IPv4Mask.ToString());
                            log.Info("...DNS Eligible?: " + ip.IsDnsEligible.ToString() + "\n...DHCP Lifetime: " + ip.DhcpLeaseLifetime.ToString());
                            ipAddrList.Add(ip.Address.ToString());
                        }
                    }
                }
            }
            return ipAddrList;
        }   
    }
}
