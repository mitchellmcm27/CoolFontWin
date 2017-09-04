using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PocketStrafe
{
    public class PocketStrafeBootStrapper : ReactiveObject
    {
        private static readonly ILog log =
                LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string Description = "PocketStrafe";
        private string _Status;

        public string Status
        {
            get { return _Status; }
            set { this.RaiseAndSetIfChanged(ref _Status, value); }
        }

        public UDPServer UdpServer { get; set; }
        public DNSNetworkService DnsServer { get; set; }
        public PocketStrafeDeviceManager DeviceManager { get; set; }
        public AppCastUpdater AppCastUpdater { get; set; }

        public PocketStrafeBootStrapper()
        {
            Status = "Initializing";
            DeviceManager = new PocketStrafeDeviceManager();
            UdpServer = new UDPServer(DeviceManager);
            AppCastUpdater = new AppCastUpdater("http://coolfont.win.app.s3.amazonaws.com/publish/currentversion.xml");
            DnsServer = new DNSNetworkService();
            DeviceManager.Start();
        }

        public void Start()
        {
            // Install ScpVBus every time application is launched
            // Must be installed synchronously
            // Uninstall it on exit (see region below)
            // scpInstaller.Install();

            Status = "Checking for updates";
            AppCastUpdater.Start();

            Status = "Starting network services";
            // Get number of expected mobile device inputs from Default
            UdpServer.Start(Properties.Settings.Default.LastPort);
            int port = UdpServer.Port;
            Properties.Settings.Default.LastPort = port;
            Properties.Settings.Default.Save();

            // publish network service for primary device
            DnsServer.Publish(port, "Primary");

            Status = "Creating virtual devices";

            Properties.Settings.Default.FirstInstall = false;
            Properties.Settings.Default.Save();
            try
            {
                ForceFirewallWindow();
            }
            catch (Exception e)
            {
                log.Error("Unable to open temp TCP socket because: " + e.Message);
                log.Info("Windows Firewall should prompt on the next startup.");
            }
        }

        /// <summary>
        /// Should force Windows Firewall prompt to show.
        /// </summary>
        private void ForceFirewallWindow()
        {
            log.Info("Opening, closing TCP socket so that Windows Firewall prompt appears...");

            // old way, get first IP address found
            //System.Net.IPAddress ipAddress = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList[0];

            // new way, write function that avoids Hamachi network interfaces
            List<System.Net.IPAddress> localAddrs = DnsServer.GetValidLocalAddresses();
            System.Net.IPAddress ipAddress = localAddrs.FirstOrDefault();

            log.Info("Address: " + ipAddress.ToString());

            System.Net.IPEndPoint ipLocalEndPoint = new System.Net.IPEndPoint(ipAddress, 12345);
            System.Net.Sockets.TcpListener t = new System.Net.Sockets.TcpListener(ipLocalEndPoint);
            t.Start();
            t.Stop();
        }
    }
}