using System;
using SharpDX.XInput;

namespace CoolFont
{
    namespace Simulator
    { 
        class XInputDeviceManager
        {
            public Controller Controller { get; set; }
            private Controller[] controllers;

            public XInputDeviceManager()
            {
                Console.WriteLine("Initializing xinput Controller");
                // Initialize XInput
                Controller = null;
                Console.WriteLine("Controller is null");
                controllers = new[] {
                    new Controller(UserIndex.One),
                    new Controller(UserIndex.Two),
                    new Controller(UserIndex.Three),
                    new Controller(UserIndex.Four),
                };
                Console.WriteLine("Controller array controllers[] is filled with 4 Controllers");
            }

            public Controller getController ()
            {
                foreach (Controller selectController in controllers)
                {
                    if (selectController.IsConnected)
                    {
                        Controller = selectController;
                        break;
                    }
                }

                if (Controller == null)
                {
                    Console.WriteLine("No XInput controller installed");
                }

                else
                {
                    Console.WriteLine("Found a XInput controller available");
                    Console.WriteLine(Controller.GetCapabilities(DeviceQueryType.Any));
                }

                return Controller;
            }
        }
    }
}
