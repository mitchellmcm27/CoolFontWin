using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using MutexManager;


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
            Console.WriteLine(args);
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
