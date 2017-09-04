using System;
using System.Collections.Generic;
using SharpDX.XInput;
using CFW.Business.Input;
using ReactiveUI;

namespace CFW.Business.Output
{

    public class PocketStrafeOutputDeviceException : Exception
    {
        public PocketStrafeOutputDeviceException()
        {
        }

        public PocketStrafeOutputDeviceException(string msg) : base(msg)
        {
        }
    }

    public class OutputDeviceState
    {
        public double X;
        public double Y;
        public double Z;
        public double RX;
        public double RY;
        public double RZ;
        public double Slider0;
        public double Slider1;
        public double POV;
        public double DPad;
        public double Speed;
        public PocketStrafeButtons Buttons;
    }

    public enum PocketStrafeButtons
    {
        //  ButtonNone      = 0,
        ButtonUp = 1 << 0,  // 00000001 = 1
        ButtonDown = 1 << 1,  // 00000010 = 2
        ButtonLeft = 1 << 2,  // 00000100 = 4
        ButtonRight = 1 << 3,  // 8
        ButtonStart = 1 << 4,  // 16
        ButtonBack = 1 << 5,  // 32
        ButtonLAnalog = 1 << 6,  // 64
        ButtonRAnalog = 1 << 7,  // 128
        ButtonLTrigger = 1 << 8,  // 256
        ButtonRTrigger = 1 << 9,  // 512
        ButtonA = 1 << 12, // 4096
        ButtonB = 1 << 13, // 8192
        ButtonX = 1 << 14, // 16384
        ButtonY = 1 << 15, // 32768
        ButtonHome = 1 << 16, // 65536
        ButtonChooseL = 1 << 17, // 131072
        ButtonChooseR = 1 << 18, // 262144
    };

    public enum OutputDeviceType
    {
        None,
        vJoy,
        vXbox,
        Keyboard,
        OpenVRInject,
        OpenVREmulator
    }


    public interface IPocketStrafeOutputDevice
    {
        void Connect(uint id);
        void Connect();
        void Disconnect();
        void AddInput(PocketStrafeInput input);
        void AddController(State state);
        void Update(); // probably need to reset state here too
        void SetCoupledLocomotion(bool coupled);
        void SwapToDevice(int id);

        double RCFilterStrength { get; set; }
        int SignX { get; set; }
        int SignY { get; set; }
        OutputDeviceType Type { get; }
        bool UserIsRunning { get; }
        uint Id { get; }
        string Keybind { get; set; }
        bool Coupled { get; }
        List<int> EnabledDevices { get; }
    }

    public abstract class BaseOutputDevice : ReactiveObject
    {

        // 0.05 good for mouse movement, 0.15 was a little too smooth
        // 0.05 probably good for VR, where you don't have to aim with the phone
        // 0.00 is good for when you have to aim slowly/precisely
        public double RCFilterStrength { get; set; }
        public int SignX { get; set; }
        public int SignY { get; set; }
        public TimeSpan UpdateInterval;
        protected OutputDeviceState _State;
        protected bool _Coupled;
        public bool Coupled { get { return _Coupled; } }

        protected BaseOutputDevice()
        {
            _State = new OutputDeviceState();
            SignX = 1;
            SignY = 1;
            RCFilterStrength = 0.05;
            ResetState();
            _Coupled = true;
        }

        protected void ResetState()
        {
            _State.X = 0;
            _State.Y = 0;
            _State.Z = 0;
            _State.RX = 0;
            _State.RY = 0;
            _State.RZ = 0;
            _State.POV = -1;
            _State.DPad = -1;
            _State.Buttons = 0;
            _State.Speed = 0;
        }

        public void SetCoupledLocomotion(bool coupled)
        {
            _Coupled = coupled;
        }
    }
}
