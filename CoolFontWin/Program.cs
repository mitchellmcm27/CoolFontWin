using System;
using System.Reflection;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Deployment;

//using MutexManager;

namespace CoolFont.AppWinForms
{
    static class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(args.ToString());

            Mutex mutex = AcquireMutex();
            if (mutex == null)
            {
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var applicationContext = new CustomApplicationContext(args);

            applicationContext.CheckForUpdates();

            try
            {
                Application.Run(applicationContext);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Program Terminated Unexpectedly", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }

        private static Mutex AcquireMutex()
        {
            Mutex appGlobalMutex = new Mutex(false, "mutex");
            if (!appGlobalMutex.WaitOne(3000))
            {
                return null;
            }
            return appGlobalMutex;
        }
    }  
}
