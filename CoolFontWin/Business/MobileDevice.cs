using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CFW.Business
{
    public class MobileDevice
    {
        public double[] Valsf;
        public int Buttons;
        public int PacketNumber;

        private int numAxes = IndexOf.ValCount;
        public bool Ready { get; private set; }
        public bool ValidPOV;
        public uint Count;
        private uint LastCount;

        private Timer DeviceTimeoutTimer = new Timer();
        private static readonly double DeviceTimeout = TimeSpan.FromSeconds(2).TotalMilliseconds;

        public MobileDevice()
        {
            this.Valsf = new double[numAxes];
            this.Buttons = 0;
            this.PacketNumber = 0;
            this.Ready = false;
            this.Count = 0;
            LastCount = Count;

            DeviceTimeoutTimer.AutoReset = true;
            DeviceTimeoutTimer.Interval = DeviceTimeout;
            DeviceTimeoutTimer.Elapsed += CheckDeviceTimeout;
            DeviceTimeoutTimer.Enabled = true;
        }

        private void CheckDeviceTimeout(object sender, EventArgs eventargs)
        {
            if (Count > LastCount && Ready)
            {
                // normal situation
                LastCount = Count;             
                return;
            }
            else if (LastCount == Count && Ready)
            {
                // no data rcvd since last check
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                Ready = false;
                Count = 0;
                LastCount = 0;
            }
            else if (Count > LastCount && !Ready)
            {
                // device was idle but new data was rcvd
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                LastCount = Count;
                Ready = true;
            }
        }
    }
}
