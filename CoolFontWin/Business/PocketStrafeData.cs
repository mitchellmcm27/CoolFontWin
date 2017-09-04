namespace PocketStrafe
{
    public class PocketStrafeData
    {
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
            string[] ints = arr[PocketStrafePacketIndex.Vals].Split(':');

            var d = new PocketStrafeInput()
            {
                deviceNumber = int.Parse(arr[PocketStrafePacketIndex.DeviceNumber]),
                packetNumber = int.Parse(arr[PocketStrafePacketIndex.PacketNumber]),
                buttons = int.Parse(arr[PocketStrafePacketIndex.Buttons]),
                speed = int.Parse(ints[PocketStrafePacketIndex.Speed]) / 1000.0,
                POV = Algorithm.WrapAngle(-int.Parse(ints[PocketStrafePacketIndex.POV]) / 1000.0),
                validPOV = int.Parse(arr[PocketStrafePacketIndex.ValidPOV]) != PocketStrafeDataMode.NoGyro
            };
            return d;
        }
    }
}
