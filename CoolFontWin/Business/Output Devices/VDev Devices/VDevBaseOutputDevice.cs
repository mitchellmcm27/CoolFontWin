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
    public abstract class VDevBaseOutputDevice : BaseOutputDevice
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // private properties
        protected static readonly double _MaxAxis = 100.0;

        protected static readonly double _MinAxis = 0.0;
        protected static readonly double _MaxPov = 359.9;
        protected static readonly double _MinPov = 0.0;

        protected vDev _Joystick;
        protected int _HDev;


        protected uint _Id;

        public uint Id // 1-16 for vJoy, 1001-1004 for vXbox
        {
            get
            {
                return _Id;
            }
            protected set
            {
                this.RaiseAndSetIfChanged(ref _Id, value);
            }
        }

        public string Keybind { get; set; }

        protected List<int> _EnabledDevices;

        public List<int> EnabledDevices
        {
            get { return _EnabledDevices; }
            protected set { _EnabledDevices = value; }
        }

        protected bool _UserIsRunning;
        public bool UserIsRunning { get { return _UserIsRunning; } }

        public bool DriverEnabled;
        public bool VDevAcquired;

        public VDevBaseOutputDevice() : base()
        {
            _Joystick = new vDev();
            DriverEnabled = false;
            VDevAcquired = false;
            _HDev = 0;
            Keybind = "Left Thumbstick";
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

        protected void AcquireDevice(uint id, DevType devType)
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

            log.Info("j acquired " + (devType == DevType.vJoy ? "vJoy" : "xBox") + " device " + id);
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

        protected void NeutralizeCurrentDevice()
        {
            log.Info("Feeding vJoy device with neutral values.");
            _Joystick.ResetAll();
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
                log.Info("First, relinquishing device " + Id.ToString());
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

        protected bool IsDriverEnabled(DevType devType)
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

        public void Disconnect()
        {
            _Joystick.ResetAll();
            _Joystick.RelinquishDev(_HDev);
            Id = 0;
            _HDev = 0;
            VDevAcquired = false;
        }

        #endregion vDev helper methods
    }
}