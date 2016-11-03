
using System;
using SharpDX.XInput;
using log4net;

namespace CoolFont
{
    namespace Simulator
    { 
        class XInputDeviceManager
        {
            private static readonly ILog log =
                LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            public Controller Controller { get; set; }
            private Controller[] controllers;

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

            public Controller getController ()
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
                    log.Info("No XInput controller installed");
                }

                else
                {
                    log.Info("Found a XInput controller available");
                    log.Info(Controller.GetCapabilities(DeviceQueryType.Any));
                }

                return Controller;
            }
        }
    }
}

