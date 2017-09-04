using System;
using System.Linq;


namespace CFW.Business.Input
{
    class PocketStrafeDataException : Exception
    {
        public PocketStrafeDataException()
        {
        }

        public PocketStrafeDataException(string msg)
        : base(msg)
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

    class PocketStrafeData
    {


        // Index of data after separating string by $
        private static readonly int IndexDataMode = 0;
        private static readonly int IndexDataValidPOV = 0;
        private static readonly int IndexDataVals = 1;
        private static readonly int IndexDataButtons = 2;
        private static readonly int IndexDataPacketNumber = 3;
        private static readonly int IndexDataDeviceNumber = 4;

        // Index of vals after separating DataVals by :
        private static readonly int IndexSpeed = 0;
        private static readonly int IndexX = 1;
        private static readonly int IndexY = 2;
        private static readonly int IndexRX = 3;
        private static readonly int IndexRY = 4;
        private static readonly int IndexZ = 5;
        private static readonly int IndexRZ = 6;
        private static readonly int IndexPOV = 7;
        private static readonly int IndexMouseDY = 8;
        private static readonly int IndexMouseDX = 9;
        private static readonly int IndexCount = 10;



        /// <summary>
        /// Main method for handling raw data from socket.
        /// </summary>
        /// <param name="data">Byte array representing UTF8 string.</param>
        /// <returns>Bool indicating whether data was ingested.</returns>
        public static PocketStrafeInput GetData(byte[] data)
        {

            // Handle empty data case
            if (data.Length == 0)
            {
                throw new PocketStrafeDataException("Data was length zero");
            }

            // Split on $ for main categories
            string[] arr = System.Text.Encoding.UTF8.GetString(data).Split('$');

            // Split on : for main data values
            string[] ints = arr[IndexOf.DataVals].Split(':');

            var d = new PocketStrafeInput()
            {
                deviceNumber = int.Parse(arr[IndexDataDeviceNumber]),
                packetNumber = int.Parse(arr[IndexDataPacketNumber]),
                buttons = int.Parse(arr[IndexDataButtons]),
                speed = int.Parse(ints[IndexSpeed]) / 1000.0,
                POV = Algorithm.WrapAngle(-int.Parse(ints[IndexPOV]) / 1000.0),
                validPOV = int.Parse(arr[IndexOf.DataValidPOV]) != PocketStrafeDataMode.NoGyro
            };
            return d;
        }
    }
}
