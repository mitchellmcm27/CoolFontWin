using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace CoolFont
{
    namespace Simulator
    { 
        class XInputDeviceManager
        {
            public Controller controller { get; set; }
            private Controller[] controllers;

            public XInputDeviceManager()
            {
                // Initialize XInput
                controller = null;
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
                        controller = selectController;
                        break;
                    }
                }

                if (controller == null)
                {
                    Console.WriteLine("No XInput controller installed");
                }

                else
                {
                    Console.WriteLine("Found a XInput controller available");
                    Console.WriteLine(controller.GetCapabilities(DeviceQueryType.Any));
                }

                return controller;
            }
        }
    }
}
