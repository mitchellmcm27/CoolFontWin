using System;
using System.Threading;
using System.Windows.Forms;
using log4net;


//using MutexManager;

namespace CoolFont.AppWinForms
{
    static class Program
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        static void Main(string[] args)
        {
            log.Info("===APP STARTUP===");
            log.Info("CoolFontWin Version " + AssemblyInfo.Version);

            log.Debug(args.ToString());

            Mutex mutex = AcquireMutex();
            if (mutex == null)
            {
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

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
                log.Info("===APP SHUTDOWN===");
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

        static void MyHandler(object sender, UnhandledExceptionEventArgs args)
        {
            Exception e = (Exception)args.ExceptionObject;
            log.Fatal("Unhandled Exception: " + e.Message);
            log.Fatal(String.Format("Runtime terminating: {0}", args.IsTerminating));
        }
    }  
}
