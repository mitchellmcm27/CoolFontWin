using System;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

using SharpDX.XInput;
using log4net;
using System.Net.Sockets;
using System.Collections.Specialized;

namespace CFW.Business
{
    public class BusinessModel
    {

        public enum Output
        {
            Keyboard,
            VJoy,
            XBox
        }

        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly NotifyIcon NotifyIcon;
        public DNSNetworkService NetworkService;
        public UDPServer Server;
        public DeviceManager SharedDeviceManager;
        public List<string> DeviceNames;
        public bool UDPServerRunning = false;
        public List<int> CurrentDevices
        {
            get { return SharedDeviceManager.EnabledVJoyDevicesList; }
        }

        //static public string PortFile = "last-port.txt";

        public BusinessModel()
        {        
            this.NetworkService = new DNSNetworkService();
            this.Server = new UDPServer();
            Server.ClientAdded += Server_ClientAdded;

            this.SharedDeviceManager = DeviceManager.Instance;
        }

        /// <summary>
        /// Starts network services (DNS and UDP server) for each device in Default Settings.
        /// </summary>
        public void StartServices()
        {
            // get list of default devices
            var devicesCol = Properties.Settings.Default.ConnectedDevices;
            StartServices(devicesCol.Cast<string>().ToList());
        }

        /// <summary>
        /// Starts network services (DNS and UDP server) for each device by name.
        /// </summary>
        /// <param name="names">List of strings represting device names.</param>
        public void StartServices(List<string> names)
        {
            this.DeviceNames = names;
            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;

            Server.Start(Properties.Settings.Default.LastPort);

            UDPServerRunning = true;

            // get whatever port finally worked and save it
            int port = Server.Port;
            Properties.Settings.Default.LastPort = port;
            Properties.Settings.Default.Save();

            // publish 1 network service for each device
            for (int i = 0; i < DeviceNames.Count; i++)
            {
                NetworkService.Publish(port, DeviceNames[i]);
            }
        }

        private void Server_ClientAdded(object sender, EventArgs e)
        {
            // Commented out because DeviceManager now plays sounds when devices come and go
            // ResourceSoundPlayer.TryToPlay(Properties.Resources.reverb_good);
        }

        /// <summary>
        /// Publish a new network service on the same port.
        /// </summary>
        /// <param name="name">Name to append to the service (device name).</param>
        public void AddService(string name)
        {

            if (NetworkService.Publish(Server.Port, name))
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                this.DeviceNames.Add(name);
            }


            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;

            // update Defaults with this name
            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Remove the last service that was published.
        /// </summary>
        public void RemoveLastService()
        {
            if (DeviceNames.Count == 1)
            {
                return;
            }
            // get last-added device name
            string name = DeviceNames.Last();
            this.DeviceNames.Remove(name);

            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;

            // unpublish service containing this name
            NetworkService.Unpublish(name);
            ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);

            // update Defaults 
            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();
        }

        public void Dispose()
        {
            // Relinquish connected devices
            SharedDeviceManager.Dispose();
        }
    
    }
}
