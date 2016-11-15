using System;
using System.Threading;
using System.Windows.Forms;
using log4net;


//using MutexManager;

namespace CFW.Business
{
    static class Program
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        static void Main(string[] args)
        {
            log.Info("===APP STARTUP===");

            Mutex mutex = AcquireMutex();
            if (mutex == null)
            {
                log.Warn("Application was already running.");
                return;
            }

            AppDomain currentDomain = AppDomain.CurrentDomain;
            currentDomain.UnhandledException += new UnhandledExceptionEventHandler(MyHandler);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var applicationContext = new CFWApplicationContext();

            //applicationContext.CheckForUpdates();

            try
            {
                Application.Run(applicationContext);
            }
            catch (Exception ex)
            {
                log.Error("Unhandled exception: "+ ex);
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

           DialogResult res = MessageBox.Show("Would you like to send a report to CoolFont?", 
               "Program Terminated Unexpectedly", 
               MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (res==DialogResult.Yes || res==DialogResult.OK)
            {
                LogFileManager.EmailLogFile();

            }

        }
    }  
}
