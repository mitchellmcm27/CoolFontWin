using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.XInput;

namespace CoolFontWin
{
    class XInputDevice
    {
        public Controller controller;

        public XInputDevice()
        {
            // Initialize XInput
            Controller[] controllers = new[] {
                new Controller(UserIndex.One),
                new Controller(UserIndex.Two),
                new Controller(UserIndex.Three),
                new Controller(UserIndex.Four),
            };

            controller = null;
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
        }
    }
}
