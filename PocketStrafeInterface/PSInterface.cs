using System;

namespace CFW.Unused
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
    }
}
