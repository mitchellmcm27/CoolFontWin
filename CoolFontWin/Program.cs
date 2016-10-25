using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MutexManager;

using Squirrel;

namespace CoolFont.AppWinForms
{
    static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            if (!SingleInstance.Start()) { return; }  // Mutex not obtained so exit

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var applicationContext = new CustomApplicationContext(args);

            /*
            SquirrelAwareApp.HandleEvents(
            onInitialInstall: OnInitialInstall, // install vJoy by running vJoySetup.exe
            onAppUpdate: OnAppUpdate,
            onAppUninstall: OnAppUninstall,
            onFirstRun: OnFirstRun);
   */
   /*
            using (var mgr = new UpdateManager(""))
            {
                // Note, in most of these scenarios, the app exits after this method
                // completes!
                SquirrelAwareApp.HandleEvents(
                  onInitialInstall: v => mgr.CreateShortcutForThisExe(),
                  onAppUpdate: v => mgr.CreateShortcutForThisExe(),
                  onAppUninstall: v => mgr.RemoveShortcutForThisExe(),
                  onFirstRun: () => ShowTheWelcomeWizard = true);
            }
            */



            string version = Assembly.GetExecutingAssembly()
                                         .GetName()
                                         .Version
                                         .ToString();
            Console.WriteLine("COOL FONT WIN version " + version);

            Console.WriteLine(args);

            // Check for app updates via Squirrel
            Task.Run(async () =>
            {
                    //await AppUpdater.AppUpdateManager.UpdateApp();  
                using (var mgr = await UpdateManager.GitHubUpdateManager("https://github.com/mitchellmcm27/coolfontwin"))
                {
                    await mgr.UpdateApp();
                }            
            });

            try
            {             
                Application.Run(applicationContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Program Terminated Unexpectedly", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
            SingleInstance.Stop(); // Release mutex

        }

        #region squirrel helper

        public static void OnAppUpdate(Version obj)
        {
            AppUpdater.CreateShortcutForThisExe();
        }

        public static void OnInitialInstall(Version obj)
        {
            Process.Start("vJoySetup.exe");
            AppUpdater.CreateShortcutForThisExe();
        }

        public static void OnAppUninstall(Version obj)
        {
            AppUpdater.RemoveShortcutForThisExe();
        }

        public static void OnFirstRun()
        {

        }

        #endregion
    }
}
