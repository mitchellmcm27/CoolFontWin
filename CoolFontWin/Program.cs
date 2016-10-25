using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MutexManager;

using Squirrel;

namespace CoolFont
{
    static class Program
    {
        static bool ShowTheWelcomeWizard;

        [STAThread]
        static void Main(string[] args)
        {
            if (!SingleInstance.Start()) { return; }  // Mutex not obtained so exit

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var applicationContext = new CustomApplicationContext(args);

            SquirrelAwareApp.HandleEvents(
            onInitialInstall: applicationContext.OnInitialInstall, // install vJoy by running vJoySetup.exe
            onAppUpdate: applicationContext.OnAppUpdate,
            onAppUninstall: applicationContext.OnAppUninstall,
            onFirstRun: applicationContext.OnFirstRun);
   


            
            string version = Assembly.GetExecutingAssembly()
                                         .GetName()
                                         .Version
                                         .ToString();
            Console.WriteLine("COOL FONT WIN version " + version);

            Console.WriteLine(args);

            // Check for app updates via Squirrel
            Task.Run(async () =>
            {
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
    }  
}
