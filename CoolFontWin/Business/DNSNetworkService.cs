using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using Mono.Zeroconf;
using log4net;
using System.Windows.Forms;
using ReactiveUI;
using System.Collections.Specialized;
using System.Linq;

namespace CFW.Business
{

    /// <summary>
    /// Manages DNS Services through Mono.Zeroconf. Works with and requires Bonjour.
    /// </summary>
    public class DNSNetworkService : ReactiveObject
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

        public List<RegisterService> PublishedServices = new List<RegisterService>();

        List<string> _DeviceNames = new List<string>();
        public List<string> DeviceNames
        {
            get { return _DeviceNames; }
            set
            {
                this.RaiseAndSetIfChanged(ref _DeviceNames, value);
            }
        }

        private int _DeviceCount;
        public int DeviceCount
        {
            get { return _DeviceCount; }
            set { this.RaiseAndSetIfChanged(ref _DeviceCount, value); }
        }

        private int _Port = 0;
        public int Port
        {
            get { return _Port; }
            set { this.RaiseAndSetIfChanged(ref _Port, value); }
        }

        private DeviceManager DeviceHub;

        public DNSNetworkService(DeviceManager deviceHub)
        {
            DeviceHub = deviceHub;
        }

        private bool _BonjourInstalled = true;
        public bool BonjourInstalled
        {
            get { return _BonjourInstalled; }
            set { this.RaiseAndSetIfChanged(ref _BonjourInstalled, value); }
        }

        private string _Address;
        public string Address
        {
            get { return _Address; }
            set { this.RaiseAndSetIfChanged(ref _Address, value); }
        }

        public List<IPAddress> GetValidLocalAddresses()
        {
            // Choose best address of local network interface
            // Supports IPv4 and v6
            // Tries Ethernet first, and then WiFi, then IPv6

            List<IPAddress> localAddrs = new List<IPAddress>();
            try
            {
                log.Info("Searching for network interfaces...");
                log.Info("  LAN IPv4...");
                localAddrs.AddRange(GetAddresses(NetworkInterfaceType.Ethernet, AddressFamily.InterNetwork));

                log.Info("  WiFi IPv4...");
                localAddrs.AddRange(GetAddresses(NetworkInterfaceType.Wireless80211, AddressFamily.InterNetwork));

                log.Info("  LAN IPv6...");
                localAddrs.AddRange(GetAddresses(NetworkInterfaceType.Ethernet, AddressFamily.InterNetworkV6));

                log.Info("  WiFi IPv6...");
                localAddrs.AddRange(GetAddresses(NetworkInterfaceType.Wireless80211, AddressFamily.InterNetworkV6));
            }
            catch (Exception e)
            {
                // If something goes wrong, it's probably not a big deal
                log.Warn("Something went wrong getting addresses: " + e);
            }

            if (localAddrs.Count == 0)
            {
                log.Error("No addresses found!");
                log.Error("Must have reachable network address!");
            }

            log.Info("Found " + localAddrs.Count.ToString() + " addresses.");
           
            if (localAddrs.Count>0)
            {
                Address = localAddrs.First().ToString();
            }
            else
            {
                Address = "No discoverable IP address found.";
            }
            return localAddrs;
        }

        public bool Publish(int port, string appendToName)
        {
            Port = port;
            RegisterService service;
            try
            {
                service = new RegisterService();
            }
            catch (Exception e)
            {
                BonjourInstalled = false;
                log.Error("Unable to register service: " + e.Message);
                ShowBonjourDialog();
                return false;
            }

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

            PublishedServices.Add(service);
            DeviceNames.Add(appendToName);
            DeviceCount = DeviceNames.Count;
            return true;
        }

        public void ShowBonjourDialog()
        {
            System.Media.SystemSounds.Exclamation.Play();
            DialogResult res = MessageBox.Show("Bonjour for Windows is required. Please download Bonjour from Apple and restart the app. \n\n Go to download page?", "Bonjour not Installed", MessageBoxButtons.YesNo);
            if (res == DialogResult.Yes)
            {
                // go to website
                System.Diagnostics.Process.Start("https://support.apple.com/kb/DL999?locale=en_US");
            }

        }

        public void Unpublish(string appendToName)
        {
            // do not allow first service to be unpublished
            if (PublishedServices.Count == 1) return;

            RegisterService serviceToDelete = null;
            foreach (var service in PublishedServices)
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
                PublishedServices.Remove(serviceToDelete);
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

        /// <summary>
        /// Publish a new network service on the same port.
        /// </summary>
        /// <param name="name">Name to append to the service (device name).</param>
        public void AddService(string name)
        {
            if (Publish(Port, name))
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);    
            }
            DeviceHub.MobileDevicesCount = DeviceNames.Count;
        }

        /// <summary>
        /// Remove the last service that was published.
        /// </summary>
        public void RemoveLastService()
        {
            // Do not allow removal of primary device
            if (DeviceNames.Count == 1)
            {
                log.Info("Can't remove primary device. Return.");
                return;
            }

            // get last-added device name and remove it
            string name = DeviceNames.Last();
            DeviceNames.Remove(name);
            DeviceCount = DeviceNames.Count;
            log.Info("Removed " + name + " from device list.");

            // unpublish service containing this name
            Unpublish(name);
            ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
            DeviceHub.MobileDevicesCount = DeviceNames.Count;
        }

        private static List<IPAddress> GetAddresses(NetworkInterfaceType type, AddressFamily family)
        {
            // Helper method to choose a local IP address
            // Device may have multiple interfaces/addresses, including IPv6 addresses 
            // Return string array

            List<IPAddress> ipAddrList = new List<IPAddress>();
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == type && item.OperationalStatus == OperationalStatus.Up)
                {
                    log.Info("    Found operational item: " + item.Name);
                    if (item.Name.Equals("Hamachi"))
                    {
                        log.Info("    Skipping Hamachi interface");
                        continue;
                    }

                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == family)
                        {
                            log.Info("    Address Info:");
                            log.Info("      Address " + ip.Address.ToString());
                            log.Info("      IPv4 Mask " + ip.IPv4Mask.ToString());
                            log.Info("      DNS Eligible " + ip.IsDnsEligible.ToString());
                            ipAddrList.Add(ip.Address);
                        }
                    }
                }
            }
            return ipAddrList;
        }   
    }
}
