using System;
using System.Diagnostics;
using WindowsInput;
using vGenWrap;
using log4net;
using System.Collections.Generic;
using ReactiveUI;
using System.ComponentModel;
using System.Windows.Forms;
using CFW.Business.Input;

namespace CFW.Business.Output
{
    /// <summary>
    /// Emulates vJoy, Keyboard, and Mouse devices on Windows.
    /// </summary>
    public class VDevOutputDevice : BaseOutputDevice, IPocketStrafeOutputDevice
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

 

        // private properties
        private static readonly double _MaxAxis = 100.0;
        private static readonly double _MinAxis = 0.0;
        private static readonly double _MaxPov = 359.9;
        private static readonly double _MinPov = 0.0;

        private vDev _Joystick;
        private int _HDev;

        // public properties
        public DevType VDevType;

        public OutputDeviceType Type
        {
            get { return (VDevType == DevType.vJoy ? OutputDeviceType.vJoy : VDevType == DevType.vXbox ? OutputDeviceType.vXbox : OutputDeviceType.None); }
        }

        private uint _Id;
        public uint Id // 1-16 for vJoy, 1001-1004 for vXbox
        {
            get
            {
                return _Id;
            }
            private set
            {
                _Id = value;
                VDevType = value < 1001 ? DevType.vJoy : DevType.vXbox;
            }
        }

        public string Keybind { get; set; }

        private List<int> _EnabledDevices;
        public List<int> EnabledDevices
        {
            get { return _EnabledDevices; }
            private set { _EnabledDevices = value; }
        }

        private bool _UserIsRunning;
        public bool UserIsRunning { get { return _UserIsRunning; } }

        public bool DriverEnabled ;
        public bool VDevAcquired;



        public VDevOutputDevice():base()
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
            log.Info("Will acquire first available vXbox device");
            VDevAcquired = false;

            // find a disabled device
            for (uint i = 1; i <= 4; i++)
            {
                try
                {
                    AcquireDevice(i, DevType.vXbox);
                }
                catch (PocketStrafeDataException ex)
                {
                    log.Info(ex.Message);
                    continue;
                }
                return;
            }
            throw new PocketStrafeOutputDeviceException("Unable to acquire any devices.");
        }

        public void Connect(uint id)
        {
            DevType devType;
            if (id > 0 && id < 17)
            {
                devType = DevType.vJoy;
                AcquireDevice(id, devType);
            }
            else if (id > 1000 && id < 1005)
            {
                devType = DevType.vXbox;
                AcquireDevice(id, devType);
            }
            else
            {
                throw new PocketStrafeOutputDeviceException("Invalid Id");
            }
        }

        private void AcquireDevice(uint id, DevType devType)
        {
            if (devType == DevType.vJoy && (id < 1 || id > 16) // vjoy
                ||
                devType == DevType.vXbox && (id < 1 || id > 4)) // xbox
            {
                log.Debug("AcquireVJoyDevice: Device index " + id + " was invalid. Returning false.");
                throw new PocketStrafeOutputDeviceException("Unable to acquire device " + id + ". Invalid index.");
            }

            bool owned = false;
            bool free = false;
            bool exist = false;
            _Joystick.isDevOwned((uint)id, devType, ref owned);
            _Joystick.isDevFree((uint)id, devType, ref free);
            _Joystick.isDevExist((uint)id, devType, ref exist);

            //Test if DLL matches the driver
            short DllVer = 0, DrvVer = 0;
            DrvVer = _Joystick.GetvJoyVersion();
            log.Info("vJoy version " + DrvVer.ToString());

            // Acquire the target (sets hDev)
            _Joystick.AcquireDev(id, devType, ref this._HDev);
            if (owned || (free && _HDev == 0))
            {
                log.Info(String.Format("Failed to acquire " + (devType == DevType.vXbox ? "xBox" : "vJoy") + " device number {0}.", id));
                throw new PocketStrafeOutputDeviceException(String.Format("Failed to acquire " + (devType == DevType.vXbox ? "xBox" : "vJoy") + " device number {0}.", id));
            }
            else
            {
                log.Info(String.Format("Acquired: " + (devType == DevType.vXbox ? "xBox" : "vJoy") + " device number {0}.", id));
            }


            bool AxisX = false;
            _Joystick.isAxisExist(_HDev, 1, ref AxisX);
            bool AxisY = false;
            _Joystick.isAxisExist(_HDev, 2, ref AxisY);
            bool AxisZ = false;
            _Joystick.isAxisExist(_HDev, 3, ref AxisZ);
            bool AxisRX = false;
            _Joystick.isAxisExist(_HDev, 4, ref AxisRX);
            bool AxisRY = false;
            _Joystick.isAxisExist(_HDev, 5, ref AxisRY);
            bool AxisRZ = false;
            _Joystick.isAxisExist(_HDev, 6, ref AxisRZ);
            // Get the number of buttons and POV Hat switchessupported by this vJoy device
            uint nBtn = 0;
            _Joystick.GetDevButtonN(_HDev, ref nBtn);
            uint nHat = 0;
            _Joystick.GetDevHatN(_HDev, ref nHat);

            int DiscPovNumber = _Joystick.GetVJDDiscPovNumber(id);


            // Print results
            log.Info(String.Format("Device {0} capabilities:", id));
            log.Info(String.Format("  Number of buttons\t\t{0}", nBtn));
            log.Info(String.Format("  Number of Hats\t{0}", nHat));
            log.Info(String.Format("  Number of Descrete POVs\t\t{0}", DiscPovNumber));
            log.Info(String.Format("  Axis X\t\t{0}", AxisX ? "Yes" : "No"));
            log.Info(String.Format("  Axis Y\t\t{0}", AxisY ? "Yes" : "No"));
            log.Info(String.Format("  Axis Z\t\t{0}", AxisZ ? "Yes" : "No"));
            log.Info(String.Format("  Axis Rx\t\t{0}", AxisRX ? "Yes" : "No"));
            log.Info(String.Format("  Axis Ry\t\t{0}", AxisRY ? "Yes" : "No"));
            log.Info(String.Format("  Axis Rz\t\t{0}", AxisRZ ? "Yes" : "No"));

            if (devType == DevType.vXbox)
            {
                log.Info("Checking for valid vXbox controller");
                log.Info("  Checking axes...");
                if (AxisX && AxisY && AxisZ && AxisRX && AxisRY && AxisRZ)
                {
                    log.Info("     All axes found.");
                }
                else
                {
                    log.Info("    Required axes not found, returning false");
                    throw new PocketStrafeOutputDeviceException("Virtual XBox controller invalid. Incorrect axes.");
                }

                log.Info("  Checking buttons...");
                if (nBtn >= 10)
                {
                    log.Info("    All buttons found.");
                }
                else
                {
                    log.Info("    Buttons not found, returning false");
                    throw new PocketStrafeOutputDeviceException("Virtual XBox controller invalid. Incorrect buttons.");
                }
            }

            log.Info("Successfully acquired " + (devType == DevType.vJoy ? "vJoy" : "xBox") + " device " + id);
            this.VDevType = devType;
            this.Id = devType == DevType.vXbox ? id + 1000 : id;
            this.VDevAcquired = true;
            ResetState();
            _Joystick.ResetAll();
        }

        public void AddInput(PocketStrafeInput input)
        {
            _UserIsRunning = input.speed > 0.1;
            if (_Coupled)
            {
                _State.Y += SignY * input.speed * _MaxAxis / 2;
                _State.POV = input.POV;
            }
            else
            {
                _State.X += SignX * Math.Sin(input.POV * Math.PI / 180) * input.speed * _MaxAxis / 2;
                _State.Y += SignY * Math.Cos(input.POV * Math.PI / 180) * input.speed * _MaxAxis / 2;
                _State.POV = input.POV / 360 * _MaxPov;
            }

            if ((input.buttons & 32768) != 0) // Y button pressed on phone
            {
                input.buttons = (short.MinValue | input.buttons & ~32768); // Y button pressed in terms of XInput
            }
            _State.Buttons = (PocketStrafeButtons)input.buttons;
        }

        public void AddController(SharpDX.XInput.State state)
        {
            // -50 to 50
            _State.X += state.Gamepad.LeftThumbX / 327.68 / 2;
            _State.Y += state.Gamepad.LeftThumbY / 327.68 / 2;
            _State.RX += state.Gamepad.RightThumbX / 327.68 / 2;
            _State.RY += state.Gamepad.RightThumbY / 327.68 / 2;

            // 0 to 100
            _State.Z += state.Gamepad.RightTrigger / 2.55;
            _State.RZ += state.Gamepad.LeftTrigger / 2.55;

            _State.Buttons = (PocketStrafeButtons)((uint)_State.Buttons | (uint)state.Gamepad.Buttons);
        }

        public void AddJoystickConstants()
        {
            // 50
            _State.X += _MaxAxis / 2;
            _State.Y += _MaxAxis / 2;
            _State.RX += _MaxAxis / 2;
            _State.RY += _MaxAxis / 2;
            // 0
            // _State.Z += 0;
            // _State.RZ += 0;
        }

        private void NeutralizeCurrentDevice()
        {
            log.Info("Feeding vJoy device with neutral values.");
            ResetState();
            Update();
            ResetState();
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

            ResetState();
        }

        #region vDev helper methods
        /// <summary>
        /// Tries to acquire given vDev device, relinquishing current device if necessary.
        /// </summary>
        /// <param name="id">Device ID (1-16 for vJoy, 1001-1004 for vXbox)</param>
        /// <returns>Boolean indicating if device was acquired.</returns>
        public void SwapToDevice(int id)
        {
            DevType devType;
            if (id > 1000)
            {
                devType = DevType.vXbox;
                id -= 1000;
            }
            else
            {
                devType = DevType.vJoy;
            }

            log.Info("Will try to acquire " + (devType == DevType.vJoy ? "vJoy" : "xBox") + " device " + id.ToString() + " and return result.");
            if (VDevAcquired)
            {
                log.Info("First, relinquishing " + (VDevType == DevType.vJoy ? "vJoy" : "xBox") + " device " + Id.ToString());
                _Joystick.ResetAll();
                _Joystick.RelinquishDev(_HDev);
                VDevAcquired = false;
            }


            if (devType == DevType.vJoy && (id < 1 || id > 16) // vjoy
                ||
                devType == DevType.vXbox && (id < 1 || id > 4)) // xbox
            {
                log.Debug("SwapToVJoyDevice: Device index " + id + " was invalid.");
                throw new PocketStrafeOutputDeviceException("Invalid device index given");
            }

            if (!IsDriverEnabled(devType))
            {
                DriverEnabled = false;
                log.Debug("Correct driver not enabled. I could try to enable it in the future. Returning false.");
                throw new PocketStrafeOutputDeviceException("VDev driver not installed");
            }

            DriverEnabled = true;

            AcquireDevice((uint)id, devType);
        }

        private bool IsDriverEnabled(DevType devType)
        {
            switch (devType)
            {
                case DevType.vJoy:
                    log.Info("vJoy Version: " + _Joystick.GetvJoyVersion());
                    if (!_Joystick.vJoyEnabled())
                    {
                        log.Info("  vJoy driver not enabled: Failed Getting vJoy attributes.");
                        return false;
                    }
                    break;

                case DevType.vXbox:
                    if (!_Joystick.isVBusExist())
                    {
                        log.Info("ScpVBus driver not installed!");
                        return false;
                    }
                    else
                    {
                        byte nSlots = 0;
                        _Joystick.GetNumEmptyBusSlots(ref nSlots);
                        log.Info("ScpVBus enabled with " + nSlots.ToString() + " empty bus slots.");
                    }
                    break;
            }
            return true;
        }

        public void GetEnabledDevices()
        {
            _Joystick = new vDev();
            log.Info("Get virtual devices able to be acquired...");
            List<int> enabledDevs = new List<int>();

            log.Info("Check drivers enabled: ");
            IsDriverEnabled(DevType.vJoy);
            IsDriverEnabled(DevType.vXbox);

            bool owned = false;
            bool exist = false;
            bool free = false;

            // loop through possible vJoy devices
            for (int i = 1; i <= 16; i++)
            {
                _Joystick.isDevOwned((uint)i, DevType.vJoy, ref owned);
                _Joystick.isDevFree((uint)i, DevType.vJoy, ref free);
                _Joystick.isDevExist((uint)i, DevType.vJoy, ref exist);

                if (free || owned)
                {
                    log.Info("Found vJoy device " + i.ToString());
                    enabledDevs.Add(i);
                }
            }

            // loop through possible Xbox devices
            for (int i = 1; i <= 4; i++)
            {
                _Joystick.isDevOwned((uint)i, DevType.vXbox, ref owned);
                _Joystick.isDevFree((uint)i, DevType.vXbox, ref free);
                _Joystick.isDevExist((uint)i, DevType.vXbox, ref exist);

                if (free || owned)
                {
                    log.Info("Found vXbox device " + i.ToString());
                    enabledDevs.Add(i + 1000);
                }
            }

            EnabledDevices = enabledDevs;
        }

        public void Disconnect()
        {
            _Joystick.ResetAll();
            _Joystick.RelinquishDev(_HDev);
            Id = 0;
            _HDev = 0;
            VDevAcquired = false;
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
        #endregion
    }

}
