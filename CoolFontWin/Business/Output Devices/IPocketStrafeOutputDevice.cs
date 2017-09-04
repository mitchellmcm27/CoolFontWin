using System.Collections.Generic;
using SharpDX.XInput;

namespace PocketStrafe.Output
{
    public interface IPocketStrafeOutputDevice
    {
        /// <summary>
        /// Connects to an output device of given id.
        /// </summary>
        /// <param name="id">Identification number of output device. 1-16 for vJoy, 1-4 for vXbox, doesn't matter for others.</param>
        void Connect(uint id);

        /// <summary>
        /// Connects to first available output device.
        /// </summary>
        void Connect();

        /// <summary>
        /// Resets and gives up control of output device.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Gives the device input from PocketStrafe mobile devices.
        /// </summary>
        /// <param name="input"></param>
        void AddInput(PocketStrafeInput input);

        /// <summary>
        /// Gives the device input from Xbox controller.
        /// </summary>
        /// <param name="state"></param>
        void AddController(State state);

        /// <summary>
        /// Updates the device driver with the latest input. Must be called for device to do anything. Make sure to ResetState if needed.
        /// </summary>
        void Update(); // probably need to reset state here too

        /// <summary>
        /// Tells the output device whether or not to treat locomotion as coupled. Some devices change behavior based on this, others don't.
        /// </summary>
        /// <param name="coupled"></param>
        void SetCoupledLocomotion(bool coupled);

        /// <summary>
        /// For devices with multiple ID's, swap to another one given by id (vJoy, vXbox).
        /// </summary>
        /// <param name="id"></param>
        void SwapToDevice(int id);

        /// <summary>
        /// Sign of X Axis (1 for regular, -1 for inverse).
        /// </summary>
        int SignX { get; set; }

        /// <summary>
        /// Sign of Y Axis (1 for regular, -1 for inverse).
        /// </summary>
        int SignY { get; set; }

        /// <summary>
        /// Type of output device (See OutputDeviceType enum)
        /// </summary>
        OutputDeviceType Type { get; }

        /// <summary>
        /// Set by the device when it detects that the user is running.
        /// </summary>
        bool UserIsRunning { get; }

        /// <summary>
        /// Identification number set by device (only applies to vJoy or vXbox).
        /// </summary>
        uint Id { get; }

        /// <summary>
        /// Keybind that maps to user running (only used for keyboard output device).
        /// </summary>
        string Keybind { get; set; }

        /// <summary>
        /// Property set by the device indicating whether it is using coupled locomotion.
        /// </summary>
        bool Coupled { get; }

        /// <summary>
        /// List of device ID's that can be acquired (vJoy or vXbox).
        /// </summary>
        List<int> EnabledDevices { get; }
    }
}
