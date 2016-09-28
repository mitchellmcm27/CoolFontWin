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
                // Initialize XInput
                Controller = null;
                controllers = new[] {
                    new Controller(UserIndex.One),
                    new Controller(UserIndex.Two),
                    new Controller(UserIndex.Three),
                    new Controller(UserIndex.Four),
                };
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
