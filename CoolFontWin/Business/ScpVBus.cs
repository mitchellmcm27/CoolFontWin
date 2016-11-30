﻿using System;
using log4net;
using System.Diagnostics;

namespace CFW.Business
{
    public static class ScpVBus
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // devcon.exe exit codes
        // https://github.com/Microsoft/Windows-driver-samples/blob/master/setup/devcon/devcon.h

        private enum DevconExitCode
        {
            EXIT_OK = 0,
            EXIT_REBOOT = 1,
            EXIT_FAIL = 2,
            EXIT_USAGE = 3
        }

        private static readonly string exe = "scpvbus\\devcon.exe";
        private static readonly string installargs = "install scpvbus\\ScpVBus.inf Root\\ScpVBus";
        private static readonly string uninstallargs = "remove Root\\ScpVBus";

        public static bool Install()
        {
            log.Info("Attempt to install ScpVBus...");
            ProcessStartInfo startInfo = new ProcessStartInfo(exe, installargs);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Verb = "runas";
            try
            {
                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
                proc.WaitForExit();
                DevconExitCode exitCode = (DevconExitCode)proc.ExitCode;
                log.Info("ScpVBus installation finished with code: " + exitCode.ToString());

                switch (exitCode)
                {
                    case DevconExitCode.EXIT_OK:
                        log.Info("ScpVBus installation suceeded.");
                        return true;
                    case DevconExitCode.EXIT_REBOOT:
                        log.Info("ScpVBus installation requires reboot.");
                        return true;
                    case DevconExitCode.EXIT_FAIL:
                        log.Info("ScpVBus installation failed.");
                        return false;
                    case DevconExitCode.EXIT_USAGE:
                        log.Info("Devcon.exe command received incorrect argument.");
                        return false;
                    default:
                        log.Info("Devcon.exe returned an unkown exit code.");
                        return false;
                }
            }
            catch (Exception e)
            {
                log.Warn("Failed to start devcon.exe to install ScpVBus: " + e.Message);
                return false;
            }
        }

        public static bool Uninstall()
        {
            log.Info("Attempt to uninstall ScpVBus...");
            ProcessStartInfo startInfo = new ProcessStartInfo(exe, uninstallargs);
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Verb = "runas";
            try
            {
                Process proc = new Process();
                proc.StartInfo = startInfo;
                proc.Start();
                return true;
                // do not wait to see if successful
            }
            catch (Exception e)
            {
                log.Warn("Failed to start devcon.exe to uninstall ScpVBus: " + e.Message);
                return false;
            }
        }

    }
}