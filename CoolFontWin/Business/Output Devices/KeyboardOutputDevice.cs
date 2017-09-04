using log4net;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using WindowsInput;

namespace PocketStrafe.Output
{
    /// <summary>
    /// Emulates vJoy, Keyboard, and Mouse devices on Windows.
    /// </summary>
    public class KeyboardOutputDevice : BaseOutputDevice, IPocketStrafeOutputDevice
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // private properties
        private InputSimulator _KbM;

        private SendInputWrapper.ScanCodeShort _VirtualKeyCode;
        private TypeConverter _Converter;
        private bool _LeftMouseButtonDown;
        private bool _RightMouseButtonDown;
        private bool _UpButtonPressed;
        private bool _DownButtonPressed;
        private bool _LeftButtonPressed;
        private bool _RightButtonPressed;

        //public
        private bool _UserIsRunning;

        public bool UserIsRunning
        {
            get { return _UserIsRunning; }
            private set { this.RaiseAndSetIfChanged(ref _UserIsRunning, value); }
        }

        private uint _Id = 2;

        public uint Id
        {
            get
            {
                return _Id;
            }
            set
            {
                this.RaiseAndSetIfChanged(ref _Id, value);
            }
        }

        public OutputDeviceType Type
        {
            get { return OutputDeviceType.Keyboard; }
        }

        private string _Keybind;

        public string Keybind
        {
            get { return _Keybind; }
            set
            {
                this.RaiseAndSetIfChanged(ref _Keybind, value);
            }
        }

        private List<int> _EnabledDevices = new List<int> { 1 };
        public List<int> EnabledDevices { get { return _EnabledDevices; } }

        public KeyboardOutputDevice()
        {
            _KbM = new InputSimulator();
            _Converter = TypeDescriptor.GetConverter(typeof(Keys));
            _LeftMouseButtonDown = false;
            _RightMouseButtonDown = false;
            _UserIsRunning = false;
            Keybind = "W";
            SetKeybind(Keybind);
        }

        public void Connect()
        {
        }

        public void Connect(uint id)
        {
            Connect();
        }

        public void Disconnect()
        {
        }

        public void SwapToDevice(int id)
        {
        }

        public void SetKeybind(string key)
        {
            var keybindOld = Keybind;
            try
            {
                Keybind = key.ToCharArray()[0].ToString().ToUpper();

                // http://www.pinvoke.net/default.aspx/user32/MapVirtualKey.html
                _VirtualKeyCode = (SendInputWrapper.ScanCodeShort)SendInputWrapper.MapVirtualKey((uint)(Keys)Enum.Parse(typeof(Keys), _Keybind, true), 0x00);
                log.Info("Changed keybind to " + Keybind);
            }
            catch (Exception e)
            {
                log.Debug("Unable to set keybind: " + e.Message);
                Keybind = keybindOld;
            }
        }

        public void AddInput(PocketStrafeInput input)
        {
            _State.Speed += input.speed;

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
            _State.Speed += _State.Y / 50.0;
        }

        private readonly double _ThreshRun = 0.1;
        private readonly double _ThreshWalk = 0.1;

        public void Update()
        {
            if (_State.Speed > _ThreshRun && !_UserIsRunning)
            {
                SendInputWrapper.KeyDown(_VirtualKeyCode);
                //KbM.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_W);
                UserIsRunning = true;
            }
            else if (_State.Speed <= _ThreshRun && UserIsRunning)
            {
                SendInputWrapper.KeyUp(_VirtualKeyCode);
                //KbM.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.VK_W);
                UserIsRunning = false;
            }

            if (false) // TODO: Implement jumping on iPhone
            {
                _KbM.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.SPACE);
            }

            if ((_State.Buttons & PocketStrafeButtons.ButtonA) != 0 & !_LeftMouseButtonDown) // A button pressed on phone
            {
                _KbM.Mouse.LeftButtonDown();
                _LeftMouseButtonDown = true;
            }
            if ((_State.Buttons & PocketStrafeButtons.ButtonA) == 0 & _LeftMouseButtonDown)
            {
                _KbM.Mouse.LeftButtonUp();
                _LeftMouseButtonDown = false;
            }

            if ((_State.Buttons & PocketStrafeButtons.ButtonB) != 0 & !_RightMouseButtonDown) // B button pressed on phone
            {
                _KbM.Mouse.RightButtonDown();
                _RightMouseButtonDown = true;
            }
            if ((_State.Buttons & PocketStrafeButtons.ButtonB) == 0 & _RightMouseButtonDown)
            {
                _KbM.Mouse.RightButtonUp();
                _RightMouseButtonDown = false;
            }

            if ((_State.Buttons & PocketStrafeButtons.ButtonUp) != 0 & !_UpButtonPressed) // Up-arrow pressed on phone
            {
                SendInputWrapper.KeyDown(GetScanCode(Keys.Up));
                _UpButtonPressed = true;
            }
            else if ((_State.Buttons & PocketStrafeButtons.ButtonUp) == 0 & _UpButtonPressed)
            {
                SendInputWrapper.KeyUp(GetScanCode(Keys.Up));
                _UpButtonPressed = false;
            }

            if ((_State.Buttons & PocketStrafeButtons.ButtonDown) != 0 & !_DownButtonPressed) // Down-arrow pressed on phone
            {
                SendInputWrapper.KeyDown(GetScanCode(Keys.Down));
                _DownButtonPressed = true;
            }
            else if ((_State.Buttons & PocketStrafeButtons.ButtonDown) == 0 & _DownButtonPressed)
            {
                SendInputWrapper.KeyUp(GetScanCode(Keys.Down));
                _DownButtonPressed = false;
            }

            if ((_State.Buttons & PocketStrafeButtons.ButtonLeft) != 0 && !_LeftButtonPressed) // Left-arrow pressed on phone
            {
                SendInputWrapper.KeyDown(GetScanCode(Keys.Left));
                _LeftButtonPressed = true;
            }
            else if ((_State.Buttons & PocketStrafeButtons.ButtonLeft) == 0 && _LeftButtonPressed)
            {
                SendInputWrapper.KeyUp(GetScanCode(Keys.Left));
                _LeftButtonPressed = false;
            }

            if ((_State.Buttons & PocketStrafeButtons.ButtonRight) != 0 && !_RightButtonPressed) // Right-arrow pressed on phone
            {
                SendInputWrapper.KeyDown(GetScanCode(Keys.Right));
                _RightButtonPressed = true;
            }
            else if ((_State.Buttons & PocketStrafeButtons.ButtonRight) == 0 && _RightButtonPressed)
            {
                SendInputWrapper.KeyUp(GetScanCode(Keys.Right));
                _RightButtonPressed = false;
            }

            ResetState();
        }

        private SendInputWrapper.ScanCodeShort GetScanCode(Keys key)
        {
            return (SendInputWrapper.ScanCodeShort)SendInputWrapper.MapVirtualKey((uint)key, 0x00);
        }
    }
}