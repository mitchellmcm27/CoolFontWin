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
    public class VXboxOutputDevice : VDevBaseOutputDevice, IPocketStrafeOutputDevice
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public OutputDeviceType Type
        {
            get { return OutputDeviceType.vXbox; }
        }

        public VXboxOutputDevice() : base()
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
            for (uint i = 1; i <= 4; i++)
            {
                try
                {
                    AcquireDevice(i, DevType.vXbox);
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
            // vJoy joysticks are generally neutral at 50% values, this function takes care of that.
            AddJoystickConstants();

            // clamp values to min/max
            _State.X = Algorithm.Clamp(_State.X, _MinAxis, _MaxAxis);
            _State.Y = Algorithm.Clamp(_State.Y, _MinAxis, _MaxAxis);
            _State.RX = Algorithm.Clamp(_State.RX, _MinAxis, _MaxAxis);
            _State.RY = Algorithm.Clamp(_State.RY, _MinAxis, _MaxAxis);
            _State.Z = Algorithm.Clamp(_State.Z, 0, 255);
            _State.RZ = Algorithm.Clamp(_State.RZ, 0, 255);

            _Joystick.SetDevAxis(_HDev, 1, _State.X);
            _Joystick.SetDevAxis(_HDev, 2, _State.Y);
            _Joystick.SetDevAxis(_HDev, 3, _State.Z);
            _Joystick.SetDevAxis(_HDev, 4, _State.RX);
            _Joystick.SetDevAxis(_HDev, 5, _State.RY);
            _Joystick.SetDevAxis(_HDev, 6, _State.RZ);

            _Joystick.SetDevButton(_HDev, 1, (_State.Buttons & PocketStrafeButtons.ButtonA) != 0);
            _Joystick.SetDevButton(_HDev, 2, (_State.Buttons & PocketStrafeButtons.ButtonB) != 0);
            _Joystick.SetDevButton(_HDev, 3, (_State.Buttons & PocketStrafeButtons.ButtonX) != 0);
            _Joystick.SetDevButton(_HDev, 4, (_State.Buttons & PocketStrafeButtons.ButtonY) != 0);
            _Joystick.SetDevButton(_HDev, 5, (_State.Buttons & PocketStrafeButtons.ButtonLTrigger) != 0);
            _Joystick.SetDevButton(_HDev, 6, (_State.Buttons & PocketStrafeButtons.ButtonRTrigger) != 0);
            _Joystick.SetDevButton(_HDev, 7, (_State.Buttons & PocketStrafeButtons.ButtonBack) != 0);
            _Joystick.SetDevButton(_HDev, 8, (_State.Buttons & PocketStrafeButtons.ButtonStart) != 0);
            _Joystick.SetDevButton(_HDev, 9, (_State.Buttons & PocketStrafeButtons.ButtonLAnalog) != 0);
            _Joystick.SetDevButton(_HDev, 10, (_State.Buttons & PocketStrafeButtons.ButtonRAnalog) != 0);

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

            _Joystick.SetDevPov(this._HDev, 1, val);

            ResetState();
        }

        public void GetEnabledDevices()
        {
            var joystick = new vDev();
            log.Info("Get virtual devices able to be acquired...");
            List<int> enabledDevs = new List<int>();

            log.Info("Check drivers enabled: ");
            IsDriverEnabled(DevType.vXbox);

            bool owned = false;
            bool exist = false;
            bool free = false;

            // loop through possible Xbox devices
            for (int i = 1; i <= 4; i++)
            {
                joystick.isDevOwned((uint)i, DevType.vXbox, ref owned);
                joystick.isDevFree((uint)i, DevType.vXbox, ref free);
                joystick.isDevExist((uint)i, DevType.vXbox, ref exist);

                if (free || owned)
                {
                    log.Info("Found vXbox device " + i.ToString());
                    enabledDevs.Add(i + 1000);
                }
            }

            EnabledDevices = enabledDevs;
        }

        public void ForceUnplugAllXboxControllers()
        {
            log.Info("Unplugging all vXbox controllers.");
            for (uint i = 1; i <= 4; i++)
            {
                _Joystick.UnPlugForce(i);
            }
            GetEnabledDevices();
        }
    }
}