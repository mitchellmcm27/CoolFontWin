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
using System.Threading.Tasks;

namespace CFW.Business
{
    public class BusinessModel
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Main business componenents
        private DNSNetworkService DnsServer;
        private UDPServer UdpServer;
        private DeviceManager SharedDeviceManager;

        public List<string> DeviceNames;

        public List<int> EnabledVJoyDevicesList
        {
            get { return SharedDeviceManager.EnabledVJoyDevicesList; }
        }

        public uint CurrentDeviceID
        {
            get { return SharedDeviceManager.CurrentDeviceID; }
        }

        public bool InterceptXInputDevice
        {
            get { return SharedDeviceManager.InterceptXInputDevice; }
            set { SharedDeviceManager.InterceptXInputDevice = value; }
        }

        public bool VJoyDeviceConnected
        {
            get { return SharedDeviceManager.VJoyDeviceConnected; }
        }

        public List<int> CurrentDevices
        {
            get { return SharedDeviceManager.EnabledVJoyDevicesList; }
        }

        public SimulatorMode Mode
        {
            get { return SharedDeviceManager.Mode; }
        }

        public BusinessModel()
        {        
            this.DnsServer = new DNSNetworkService();
            this.UdpServer = new UDPServer();
            UdpServer.ClientAdded += Server_ClientAdded;
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

            UdpServer.Start(Properties.Settings.Default.LastPort);

            // get whatever port finally worked and save it
            int port = UdpServer.Port;
            Properties.Settings.Default.LastPort = port;
            Properties.Settings.Default.Save();

            // publish 1 network service for each device
            for (int i = 0; i < DeviceNames.Count; i++)
            {
                DnsServer.Publish(port, DeviceNames[i]);
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
            if (DnsServer.Publish(UdpServer.Port, name))
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
            // Do not allow removal of primary device
            if (DeviceNames.Count == 1)
            {
                return;
            }

            // get last-added device name and remove it
            string name = DeviceNames.Last();
            this.DeviceNames.Remove(name);
           

            // unpublish service containing this name
            DnsServer.Unpublish(name);
            ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);

            // update Defaults 
            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();

            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;
        }

        public void IncreaseSmoothingFactor()
        {
            SharedDeviceManager.SmoothingFactor *= 2;
        }

        public void DecreaseSmoothingFactor()
        {
            SharedDeviceManager.SmoothingFactor /= 2;
        }

        public bool UpdateMode(int mode)
        {
            return SharedDeviceManager.TryMode(mode);
        }

        public void UnplugAllXbox(bool silent = false)
        {
            SharedDeviceManager.TryMode((int)SimulatorMode.ModeWASD);
            SharedDeviceManager.ForceUnplugAllXboxControllers(silent);
        }

        public async Task UnplugAllXboxAsync(bool silent=false)
        {
            await Task.Run(() => UnplugAllXbox(silent));
        }

        public bool AcquireVDev(uint id)
        {
            bool res = SharedDeviceManager.AcquireVDev(id);
            if (Mode == SimulatorMode.ModeWASD) UpdateMode((int)SimulatorMode.ModeJoystickCoupled);
            return res;
        }

        public async Task<bool> AcquireVDevAsync(uint id)
        {
            return await Task.Run(() => AcquireVDev(id));
        }

        public void FlipX()
        {
            SharedDeviceManager.FlipAxis(Axis.AxisX);
        }

        public void FlipY()
        {
            SharedDeviceManager.FlipAxis(Axis.AxisY);
        }

        public void RelinquishCurrentDevice()
        {
            SharedDeviceManager.RelinquishCurrentDevice();
        }

        public void Dispose()
        {
            // Relinquish connected devices
            SharedDeviceManager.Dispose();
        }
    
    }
}
