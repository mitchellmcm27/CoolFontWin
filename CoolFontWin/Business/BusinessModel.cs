using System;
using System.Linq;
using System.Collections.Generic;
using log4net;
using System.Collections.Specialized;
using System.Threading.Tasks;
using ReactiveUI;

namespace CFW.Business
{
    public class BusinessModel : ReactiveObject
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // Main business componenents
        private DNSNetworkService DnsServer;
        private UDPServer UdpServer;
        private DeviceManager SharedDeviceManager;
        
        public BusinessModel()
        {        

        }

       

        public void IncreaseSmoothingFactor()
        {
            SharedDeviceManager.SmoothingFactor *= 2;
        }

        public void DecreaseSmoothingFactor()
        {
            SharedDeviceManager.SmoothingFactor /= 2;
        }

        public async Task UpdateModeAsync(int mode)
        {
            await Task.Run(()=> UpdateMode(mode));
        }

        public bool UpdateMode(int mode)
        {
            bool res = SharedDeviceManager.TryMode(mode);
            return res;
        }

        public void UnplugAllXbox(bool silent = false)
        {         
            SharedDeviceManager.ForceUnplugAllXboxControllers(silent);
            SharedDeviceManager.TryMode((int)SimulatorMode.ModeWASD);
        }

        public async Task UnplugAllXboxAsync(bool silent=false)
        {
            await Task.Run(() => UnplugAllXbox(silent));
        }

        public bool AcquireVDev(uint id)
        {
            bool res = SharedDeviceManager.AcquireVDev(id);    
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
