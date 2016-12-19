using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ookii.Dialogs;
using System.Threading;
using System.Diagnostics;
using log4net;

namespace CFW.Business
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

            taskDialog.WindowTitle = "Install vJoy driver?";
            taskDialog.MainIcon = TaskDialogIcon.Information;

            taskDialog.MainInstruction = "Install vJoy to enable virtual joystick output";
            taskDialog.Content = "A few games work well with a virtual joystick (vJoy), because it is more customizable than controllers.\n\n";
            taskDialog.Content += "You can install the vJoy driver here, or find the latest version online. Beware that some games do not play well with joysticks, but you can always disable or uninstall the driver.";

            taskDialog.ButtonStyle = TaskDialogButtonStyle.CommandLinks;
            var customButton = new TaskDialogButton(ButtonType.Custom);
            customButton.CommandLinkNote = "May require administrator priveleges";
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
