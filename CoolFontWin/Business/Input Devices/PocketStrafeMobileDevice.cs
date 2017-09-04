using System;
using System.Timers;

namespace PocketStrafe.Input
{
    public class PocketStrafeMobileDevice
    {
        public bool Ready { get; private set; }

        public PocketStrafeInput State;
        private uint _Count;
        private uint _LastCount;
        private Timer DeviceTimeoutTimer = new Timer();
        private static readonly double DeviceTimeout = TimeSpan.FromSeconds(2).TotalMilliseconds;

        public PocketStrafeMobileDevice()
        {
            this.State = new PocketStrafeInput();
            this.Ready = false;
            this._Count = 0;
            this._LastCount = 0;

            DeviceTimeoutTimer.AutoReset = true;
            DeviceTimeoutTimer.Interval = DeviceTimeout;
            DeviceTimeoutTimer.Elapsed += CheckDeviceTimeout;
            DeviceTimeoutTimer.Enabled = true;
        }

        public void SetState(PocketStrafeInput newState)
        {
            State = newState;
            _Count++;
        }

        private void CheckDeviceTimeout(object sender, EventArgs eventargs)
        {
            if (_Count > _LastCount && Ready)
            {
                // normal situation
                _LastCount = _Count;
                return;
            }
            else if (_LastCount == _Count && Ready)
            {
                // no data rcvd since last check
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                Ready = false;
                _Count = 0;
                _LastCount = 0;
            }
            else if (_Count > _LastCount && !Ready)
            {
                // device was idle but new data was rcvd
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                _LastCount = _Count;
                Ready = true;
            }
        }
    }
}