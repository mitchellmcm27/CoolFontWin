using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Resources;
using System.Drawing;
using System.Diagnostics;
using CoolFontUdp;
using CoolFontIO;

namespace CoolFontWin
{
    public class SysTray
    {

        public class MyCustomApplicationContext : ApplicationContext
        {
            private NotifyIcon trayIcon;
            Icon myIcon = new Icon("AppIcon.ico");
            Process proc = new Process();
          
            public MyCustomApplicationContext()
            {
                Icon myIcon = new Icon("AppIcon.ico");
                // Initialize Tray Icon
                trayIcon = new NotifyIcon()
                {
                       
                    Icon = myIcon,
                    ContextMenu = new ContextMenu(new MenuItem[] {
                        new MenuItem("Reset", Reset),
                new MenuItem("Exit", Exit)
                }),
                    Visible = true
                };
                
            }

            void Reset(object sender, EventArgs e)
            {
                JavaProc.Kill();
                /* Instantiate listener using port */
                UdpListener listener = new UdpListener();
                int port = listener.port;

                if (port > 0 & listener.isBound)
                {
                    // write successful port to file for next time
                    FileManager.WritePortToFile(port, Config.PORT_FILE);
                }

                /* Register DNS service through Java */
                JavaProc.StartDnsService(port); // blocks    
                //proc.StartInfo.FileName = @"C:\User\Roy\Desktop\CoolFontWin\testapp-java.jar";
                //proc.StartInfo.Arguments = Application.ExecutablePath;
                //proc.Kill(); 
            }
            void Exit(object sender, EventArgs e)
            {
                // Hide tray icon, otherwise it will remain shown until user mouses over it
                trayIcon.Visible = false;
               // proc.Kill(); 
                Application.Exit();
                Environment.Exit(0);
            }
        }
    }
}
