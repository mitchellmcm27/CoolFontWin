using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CFW.VR;
using Valve.VR;

namespace CFW.Business
{
    public class PSInterface : MarshalByRefObject
    {
        public bool UserIsRunning { get; set; }
        public EVRButtonId RunButton { get; set; }
        public PStrafeButtonType ButtonType { get; set; }
        public PStrafeHand Hand { get; set; }
        public bool Installed { get; set; }
        public PSInterface()
        {
            Installed = false;
            RunButton = EVRButtonId.k_EButton_Axis0;
            ButtonType = PStrafeButtonType.Press;
            Hand = PStrafeHand.Left;
        }

        public void ReportError(int pid, Exception e)
        {
            Console.WriteLine(string.Format("Error installing hook (pid {0}): {1}", pid, e));
        }

        public void IsInstalled(int pid)
        {
            Installed = true;
            Console.WriteLine("Hook installed successfully! pid: " + pid);
        }

        public void Write(string str)
        {
            Console.WriteLine(str);
        }

        public void Cleanup()
        {
            Installed = false;
        }
    }
}
