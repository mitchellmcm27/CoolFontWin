using ReactiveUI;
using System;

namespace PocketStrafe.Output
{
    /// <summary>
    /// Abstract class that implements shared functionality of all output devices. Output device classes should, but do not have to, inherit from this class.
    /// </summary>
    public abstract class BaseOutputDevice : ReactiveObject
    {

        public int SignX { get; set; }
        public int SignY { get; set; }
        public TimeSpan UpdateInterval;
        protected OutputDeviceState _State;
        protected bool _Coupled;
        public bool Coupled
        {
            get { return _Coupled; }
            protected set { this.RaiseAndSetIfChanged(ref _Coupled, value); }
        }

        protected BaseOutputDevice()
        {
            _State = new OutputDeviceState();
            SignX = 1;
            SignY = 1;
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
            Coupled = coupled;
        }
    }
}
