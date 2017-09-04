using System;
using System.Diagnostics;
using log4net;

namespace PocketStrafe.Output
{
    class OpenVrEmulator
    {
        private static readonly ILog log =
    LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string emu = "client_commandline.exe";
        private int SerialNumber = 0;
        private int ControllerNumber = 0;

        public OpenVrEmulator()
        {
            Process.Start(emu, "listdevices");
            Process P = Process.Start(emu, String.Format("addcontroller pocketstrafe%02d", SerialNumber++));
            P.WaitForExit();
            int result = P.ExitCode;
            log.Debug("Created virtual Vive controller " + result);
            ControllerNumber = result;

            SetupController(result);
        }

        private void SetupController(int n)
        {
           
        }

        public void Update()
        {

        }
    }
}
