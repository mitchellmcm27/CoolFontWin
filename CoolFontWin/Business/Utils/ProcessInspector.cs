using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace PocketStrafe
{
    public static class ProcessInspector
    {
        private static readonly ILog log =
        LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<string> GetProcesses()
        {
            var thisName = Process.GetCurrentProcess().ProcessName;
            var list = new List<string>();
            foreach (var proc in Process.GetProcesses())
            {
                if (proc.MainWindowTitle.Length > 0 && !proc.ProcessName.Equals(thisName) && Is64Bit(proc))
                {
                    list.Add(proc.ProcessName);
                }
            }
            return list;
        }

        public static bool Is64Bit(Process process)
        {
            if (Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE") == "x86")
                return false;

            bool isWow64;
            try
            {
                bool success = IsWow64Process(process.Handle, out isWow64);
                if (!success)
                    return false;
                return !isWow64;
            }
            catch (Exception ex)
            {
                log.Error(ex);
                return false;
            }
        }

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);
    }
}
