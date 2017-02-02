using System;
using Valve.VR;

namespace CFW.Business
{
    public class PSInterface : MarshalByRefObject
    {

        public void ReportError(int pid, Exception e)
        {
            Console.WriteLine(string.Format("Error installing hook (pid {0}): {1}", pid, e));
        }

        public void IsInstalled(int pid)
        {
            Console.WriteLine("Hook installed successfully! pid: " + pid);
        }

        public void Write(string str)
        {
            Console.WriteLine(str);
        }

        public bool UserIsRunning { get; set; }

        public EVRButtonId RunButton { get; set; }
        public EVRButtonType ButtonType { get; set; }
        public EVRHand Hand { get; set; }
        public PSInterface()
        {
            RunButton = EVRButtonId.k_EButton_Axis0;
            ButtonType = EVRButtonType.Press;
            Hand = EVRHand.Left;
        }
    }
}
