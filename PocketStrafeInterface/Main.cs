using System;

namespace CFW.Business
{
    public class Main : MarshalByRefObject
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

        public bool IsRunning;

        public bool GetUserRunning()
        {
            return IsRunning;
        }
    }
}
