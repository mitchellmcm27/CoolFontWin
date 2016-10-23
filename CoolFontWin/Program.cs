using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using MutexManager;
using Squirrel;

namespace CoolFont
{
   static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            if (!SingleInstance.Start()) { return;  }  // Mutex not obtained so exit

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
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
                var applicationContext = new CustomApplicationContext(args);             
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
