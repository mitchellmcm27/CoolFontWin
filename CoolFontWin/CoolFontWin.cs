using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms; 
using SharpDX.XInput;
using CoolFont.IO;
using CoolFont.Network;
using CoolFont.Simulator;
using CoolFont.Utils;
using System.Threading; 

namespace CoolFont
{
    public class CoolFontWin
    {
        private int _port;
        // A delegate type to be used as the handler routine 
        // for SetConsoleCtrlHandler.

        private readonly NotifyIcon notifyIcon;

        public CoolFontWin(NotifyIcon notifyIcon)
        {
            this.notifyIcon = notifyIcon; 
        }

       public void StartService()
        {

            // SysTray.Run(); 

            int tryport = FileManager.ReadPortFromFile(Config.PORT_FILE);

            /* Instantiate listener using port */
            UdpListener sock = new UdpListener(tryport);
            _port = sock.port;

            if (_port > 0 & sock.isBound)
            {
                // write successful port to file for next time
                FileManager.WritePortToFile(_port, Config.PORT_FILE);
            }

            /* Register DNS service through Java */
            JavaProc.StartDnsService(_port); // blocks

            // check to see if everything is set up?
            ReceiveService(sock);
            

        }
        [STAThread]
        private void ReceiveService(UdpListener sock)
        {
            /* Set up the simulator */
            Config.Mode = Config.MovementModes.Mouse2D;
            XInputDeviceManager devMan = new XInputDeviceManager();
            Controller xDevice = devMan.getController();
            VirtualDevice vDevice = new VirtualDevice(Config.Mode); // will change Mode if necessary

            //TODO: execute loop in background thread and allow user to break out
            int T = 0; // total time
            int maxGapSize = 30; // set to -1 to always interpolate data
            int gapSize = maxGapSize + 1;
            new Thread(() =>
            {
                while (true)
                {
                    vDevice.logOutput = false;
                    bool logRcvd = false;

                    /* get data from iPhone socket, add to vDev */
                    string rcvd = sock.pollSocket(Config.socketPollInterval);
                    bool res = vDevice.HandleNewData(rcvd);
                    gapSize = (res == true) ? 0 : gapSize + 1;

                    /* Tell vDev whether we want it to fill in missing data */
                    if (gapSize > maxGapSize) { vDevice.shouldInterpolate = false; }

                    /* Get data from connected XInput device, add to vDev*/

                    if (xDevice != null && xDevice.IsConnected)
                    {
                        State state = xDevice.GetState();
                        vDevice.AddControllerState(state);
                    }

                    vDevice.FeedVJoy();
                    T++;

                    if (logRcvd && (T % 10 == 0))
                        Console.WriteLine(rcvd);
                    if (vDevice.logOutput)
                        Console.Write(" ({0}) \n", gapSize);
                }
              
            }).Start(); 
        }


        /*
         * 
         */
        private void reset_Click(object sender, EventArgs e)
        {
            JavaProc.Kill();
            JavaProc.StartDnsService(_port); // blocks
        }

        public void BuildContextMenu(ContextMenuStrip contextMenuStrip)
        {
            contextMenuStrip.Items.Clear();
            contextMenuStrip.Items.AddRange(
                new ToolStripItem[] {        
                    new ToolStripSeparator(),
                   ToolStripMenuItemWithHandler("&Reset Server", reset_Click),
                });          
        }

        public ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            // add image
            if (eventHandler != null) { item.Click += eventHandler; }
            return item;
        }

    

    }

}
