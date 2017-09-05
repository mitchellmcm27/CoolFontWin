using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using vGenWrap;

namespace PocketStrafe.Output
{
    /// <summary>
    /// Emulates vJoy, Keyboard, and Mouse devices on Windows.
    /// </summary>
    public class VJoyOutputDevice : VDevBaseOutputDevice, IPocketStrafeOutputDevice
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OutputDeviceType Type
        {
            get { return OutputDeviceType.vJoy; }
        }

        public VJoyOutputDevice() : base()
        {
            _Joystick = new vDev();
            DriverEnabled = false;
            VDevAcquired = false;
            _HDev = 0;
            Keybind = "Left Thumbstick";
        }

        /// <summary>
        /// Loop through vJoy devices, find the first disabled device. Enable, config, and acquire it.
        /// </summary>
        /// <returns>Bool indicating if device was found, enabled, created, and acquired. </returns>
        public void Connect()
        {
            log.Info("Will acquire first available vJoy device");
            VDevAcquired = false;

            // find a disabled device
            for (uint i = 1; i <= 16; i++)
            {
                try
                {
                    AcquireDevice(i, DevType.vJoy);
                    return;
                }
                catch (PocketStrafeOutputDeviceException ex)
                {
                    log.Info(ex.Message);
                    continue;
                }
            }
            throw new PocketStrafeOutputDeviceException("Unable to acquire any devices.");
        }

        public void Update()
        {
            BaseUpdate();

            // NOTE: useful to instantly see direction user is facing, but commented out in case it interferes with games
            // _Joystick.SetDevPov(this._HDev, 1, _State.POV);

            // vJoy deals with arrow buttons by controlling the main joystick
            double val = -1;
            if ((_State.Buttons & PocketStrafeButtons.ButtonUp) != 0)
            {
                if ((_State.Buttons & PocketStrafeButtons.ButtonRight) != 0)
                {
                    val = 45;
                }
                else if ((_State.Buttons & PocketStrafeButtons.ButtonLeft) != 0)
                {
                    val = 315;
                }
                else { val = 0; }
            }
            else if ((_State.Buttons & PocketStrafeButtons.ButtonDown) != 0)
            {
                if ((_State.Buttons & PocketStrafeButtons.ButtonLeft) != 0)
                {
                    val = 225;
                }
                else if ((_State.Buttons & PocketStrafeButtons.ButtonRight) != 0)
                {
                    val = 135;
                }
                else { val = 180; }
            }
            else if ((_State.Buttons & PocketStrafeButtons.ButtonRight) != 0)
            {
                val = 90;
            }
            else if ((_State.Buttons & PocketStrafeButtons.ButtonLeft) != 0)
            {
                val = 270;
            }

            if (val > -1)
            {
                var x = _MaxAxis / 2 + _MaxAxis * Math.Sin(val * Math.PI / 180.0);
                var y = _MaxAxis / 2 + _MaxAxis * Math.Cos(val * Math.PI / 180.0);
                _Joystick.SetDevAxis(_HDev, 1, x);
                _Joystick.SetDevAxis(_HDev, 2, y);
            }

            ResetState();
        }

        public void GetEnabledDevices()
        {
            var joystick = new vDev();
            log.Info("Get virtual devices able to be acquired...");
            List<int> enabledDevs = new List<int>();

            log.Info("Check drivers enabled: ");
            IsDriverEnabled(DevType.vJoy);

            bool owned = false;
            bool exist = false;
            bool free = false;

            // loop through possible vJoy devices
            for (int i = 1; i <= 16; i++)
            {
                joystick.isDevOwned((uint)i, DevType.vJoy, ref owned);
                joystick.isDevFree((uint)i, DevType.vJoy, ref free);
                joystick.isDevExist((uint)i, DevType.vJoy, ref exist);

                if (free || owned)
                {
                    log.Info("Found vJoy device " + i.ToString());
                    enabledDevs.Add(i);
                }
            }

            EnabledDevices = enabledDevs;
        }
    }
}