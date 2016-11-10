﻿using System;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpDX.XInput;
using log4net;


namespace CFW.Business
{
    public enum Axis
    {
        AxisX,
        AxisY,
    }

    public sealed class DeviceManager
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private VirtualDevice VDevice;
        private XInputDeviceManager XMgr;
        private Controller XDevice;

        public double SmoothingFactor
        {
            get
            {
                return VDevice.RCFilterStrength;
            }
            set
            {
                VDevice.RCFilterStrength = value;
            }
        }

        public SimulatorMode Mode
        {
            get
            {
                return VDevice.Mode;
            }
        }

        public bool CurrentModeIsFromPhone
        {
            get
            {
                return VDevice.CurrentModeIsFromPhone;
            }
        }

        public bool InterceptXInputDevice = false;
        public bool XInputDeviceConnected = false;
        public bool VJoyDeviceConnected = false;

        // private lazy instantiation of singleton
        // static: execute only once
        private static readonly Lazy<DeviceManager> lazy = 
            new Lazy<DeviceManager>(() => new DeviceManager());
        
        // public getter for the instance
        // static: triggers private constructor on first access
        public static DeviceManager Instance
        {
            get
            {
                return lazy.Value;
            }
        }

        private Timer VDeviceUpdateTimer;
        private int TimerCount = 0;
        private static int MaxInterpolateCount;

        private DeviceManager()
        {
            XMgr = new XInputDeviceManager();
            XDevice = XMgr.getController();

            if (XDevice != null && XDevice.IsConnected)
            {
                XInputDeviceConnected = true;
            }

            VDevice = new VirtualDevice((uint)Properties.Settings.Default.VJoyID);
            VJoyDeviceConnected = true;

            InitializeTimer();
        }

        private void InitializeTimer()
        {
            VDeviceUpdateTimer = new Timer(16); // elaps every 1/60 sec , appx 16 ms.
            VDeviceUpdateTimer.Elapsed += new ElapsedEventHandler(TimerElapsed); //define a handler
            VDeviceUpdateTimer.Enabled = false; //enable the timer.
            VDeviceUpdateTimer.AutoReset = true;
            MaxInterpolateCount = (int)Math.Floor(1 / VDeviceUpdateTimer.Interval * 1000 / 2); // = approx 0.5 sec
        }

        public bool TryMode(int mode)
        {
            return VDevice.ClickedMode(mode);
        }

        public void FlipAxis(Axis axis)
        {
            switch (axis)
            {
                case Axis.AxisX:
                    VDevice.signX = -VDevice.signX;
                    break;
                case Axis.AxisY:
                    VDevice.signY = -VDevice.signY;
                    break;

            }
        }

        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            TimerCount++;
            PassDataToDevices(new byte[] { });
            if (TimerCount > MaxInterpolateCount)
            {
                VDeviceUpdateTimer.Stop();
                VDevice.ShouldInterpolate = false;
            }
        }

        public void PassDataToDevices(byte[] data)
        {
            string rcvd = System.Text.Encoding.UTF8.GetString(data);
            

            if (VDevice.HandleNewData(rcvd))
            {
                TimerCount = 0;
                VDevice.ShouldInterpolate = true;
                
                VDevice.AddJoystickConstants();
            }

            if (InterceptXInputDevice)
            {
                if (VDevice.Mode == SimulatorMode.ModeWASD)
                {
                    InterceptXInputDevice = false;
                }
                else
                {
                    try
                    {
                        State state = XDevice.GetState();
                        VDevice.AddControllerState(state);
                    }
                    catch
                    {
                        InterceptXInputDevice = false;
                        XInputDeviceConnected = false;
                        System.Media.SystemSounds.Beep.Play();
                    }
                }
            }

            VDevice.FeedVJoy();
            VDevice.ResetValues();
            VDeviceUpdateTimer.Start();
        }
        
    }
}
