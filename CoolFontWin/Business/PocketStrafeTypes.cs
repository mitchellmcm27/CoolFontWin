using System;

namespace PocketStrafe
{
    public class PocketStrafeException : Exception
    {
        public PocketStrafeException()
        {
        }

        public PocketStrafeException(string msg)
        : base(msg)
        {
        }
    }
    public class PocketStrafeDataException : PocketStrafeException
    {
        public PocketStrafeDataException()
        {
        }

        public PocketStrafeDataException(string msg)
        : base(msg)
        {
        }
        
    }

    public class PocketStrafeOutputDeviceException : PocketStrafeException
    {
        public PocketStrafeOutputDeviceException()
        {
        }

        public PocketStrafeOutputDeviceException(string msg) : base(msg)
        {
        }
    }

    public struct PocketStrafeDataMode
    {
        public static readonly int Paused = 0; // no longer used
        public static readonly int Keyboard = 1; // no longer used
        public static readonly int JoystickCoupled = 2; // no longer used
        public static readonly int JoystickDecoupled = 3; // no longer used
        public static readonly int NoGyro = 10; // Androids w/out a gyroscope, decoupled mode will be bad
        public static readonly int GyroOK = 11; // iPhones and Androids with gyros
    }

    public struct PocketStrafeInput
    {
        public int deviceNumber;
        public int packetNumber;
        public int buttons;
        public double speed;
        public double POV;
        public bool validPOV;
    }

    public struct PocketStrafePacketIndex
    {
        // Index of data after separating string by $
        public static readonly int Mode = 0;
        public static readonly int ValidPOV = 0;
        public static readonly int Vals = 1;
        public static readonly int Buttons = 2;
        public static readonly int PacketNumber = 3;
        public static readonly int DeviceNumber = 4;

        // Index of vals after separating DataVals by :
        public static readonly int Speed = 0;
        public static readonly int X = 1;
        public static readonly int Y = 2;
        public static readonly int RX = 3;
        public static readonly int RY = 4;
        public static readonly int Z = 5;
        public static readonly int RZ = 6;
        public static readonly int POV = 7;
        public static readonly int MouseDY = 8;
        public static readonly int MouseDX = 9;
        public static readonly int Count = 10;
    }

    public enum OutputDeviceAxis
    {
        AxisX,
        AxisY,
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
}
