using log4net;
using SharpDX.XInput;

namespace PocketStrafe.Input
{
    /// <summary>
    /// Finds and returns connected XInput devices using SharpDX.
    /// </summary>
    internal class XInputDeviceManager
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Controller Controller { get; set; }
        private Controller[] controllers;

        /// <summary>
        /// Initializes an array with 4 slots for XInput devices and sets Controller to null.
        /// </summary>
        public XInputDeviceManager()
        {
            log.Info("Initializing xinput Controller.");
            // Initialize XInput
            Controller = null;
            controllers = new[] {
                new Controller(UserIndex.One),
                new Controller(UserIndex.Two),
                new Controller(UserIndex.Three),
                new Controller(UserIndex.Four),
            };
            log.Info("Controller array initialized.");
        }

        /// <summary>
        /// Loop through device IDs and check if controllers are connected.
        /// </summary>
        /// <returns>Returns first connected Controller or null.</returns>
        public Controller getController()
        {
            foreach (Controller selectController in controllers)
            {
                if (selectController.IsConnected)
                {
                    Controller = selectController;
                    log.Info("Found Controller.");
                    break;
                }
            }

            if (Controller == null)
            {
                log.Info("No XInput controller connected");
            }
            else
            {
                log.Info("Found XInput controller available");
                try
                {
                    log.Info("Will try to get controller capabilities:");
                    log.Info(Controller.GetCapabilities(DeviceQueryType.Any));
                }
                catch (SharpDX.SharpDXException ex)
                {
                    log.Info("Getting capabilities failed: " + ex.Message);
                    log.Info("Returning null");
                    Controller = null;
                }
            }
            return Controller;
        }
    }
}