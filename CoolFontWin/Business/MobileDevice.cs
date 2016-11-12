using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFW.Business
{
    public class MobileDevice
    {
        public double[] Valsf;
        public int Buttons;
        public int PacketNumber;

        private int numAxes = (int)JoystickVal.ValCount;

        public MobileDevice()
        {
            this.Valsf = new double[numAxes];
            this.Buttons = 0;
            this.PacketNumber = 0;
        }
    }
}
