﻿using log4net;
using Ookii.Dialogs;
using System;
using System.Diagnostics;
using System.Threading;

namespace PocketStrafe
{
    public static class VJoyInfoDialog
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly string exe = "vJoy\\vJoySetup.exe";

        public static void ShowVJoyInfoDialog()
        {
            var taskDialog = new TaskDialog();
            taskDialog.Width = 200;
            taskDialog.AllowDialogCancellation = true;

            taskDialog.WindowTitle = "vJoy not found";
            taskDialog.MainIcon = TaskDialogIcon.Shield;

            taskDialog.MainInstruction = "Enable vJoy";
            taskDialog.Content = "vJoy is either not installed or not enabled. If it's installed, enable vJoy using vJoy Config.";

            taskDialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;
            var customButton = new TaskDialogButton(ButtonType.Custom);
            customButton.CommandLinkNote = "Virtual joysticks";
            customButton.Text = "Install vJoy driver";
            customButton.Default = true;
            taskDialog.Buttons.Add(customButton);
            taskDialog.Buttons.Add(new TaskDialogButton(ButtonType.Close));

            taskDialog.ExpandFooterArea = true;
            taskDialog.ExpandedControlText = "More information about vJoy";
            taskDialog.ExpandedInformation = "vJoy is an open source project created by Shaul Eizikovich. It has a large, active user base on http://vjoystick.sourceforge.net/. vJoy devices can have 8 axes and up to 128 buttons and are highly configurable.";

            new Thread(() =>
            {
                try
                {
                    TaskDialogButton res = taskDialog.Show(); // Windows Vista and later
                    if (res != null && res.ButtonType == ButtonType.Custom)
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(exe);
                        startInfo.Verb = "runas";
                        Process proc = new Process();
                        proc.StartInfo = startInfo;
                        proc.Start();
                    }

                    if (taskDialog.IsVerificationChecked)
                    {
                        Properties.Settings.Default.ShowScpVbusDialog = false;
                        Properties.Settings.Default.Save();
                    }
                }
                catch (Exception e)
                {
                    log.Warn("Something went wrong installing vJoy: " + e);
                    log.Warn(e.Message);
                    return;
                }
            }).Start();
        }
    }
}