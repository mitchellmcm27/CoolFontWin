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
        
        [DllImport("Kernel32")]
        public static extern bool SetConsoleCtrlHandler(HandlerRoutine Handler, bool Add);

        // A delegate type to be used as the handler routine 
        // for SetConsoleCtrlHandler.
        public delegate bool HandlerRoutine(CtrlTypes CtrlType);

        // An enumerated type for the control messages
        // sent to the handler routine.
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        private int _port;
        private readonly NotifyIcon notifyIcon;

        private bool log = false;
        private bool verbose = false;
        private string[] args;

        public CoolFontWin(NotifyIcon notifyIcon, string[] args)
        {
            this.notifyIcon = notifyIcon;
            this.args = args;
        }

       public void StartService()
        {
            ProcessArgs();
            int tryport = FileManager.TryToReadPortFromFile(Config.PORT_FILE); // returns 0 if none
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

        private void ProcessArgs()
        {
            foreach (string arg in args)
            {
                if (arg.Equals("log"))
                {
                    log = true;
                }
                if (arg.Equals("verbose"))
                {
                    verbose = true;
                }
            }
        }

        private void ReceiveService(UdpListener sock)
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            /* Set up the simulator */
            Config.Mode = Config.MODE.ModeMouse;
            XInputDeviceManager devMan = new XInputDeviceManager();
            Controller xDevice = devMan.getController();
            VirtualDevice vDevice = new VirtualDevice(Config.Mode); // will change Mode if necessary

            vDevice.logOutput = verbose; // T or F

            int T = 0; // total time
            int maxGapSize = 90; // set to -1 to always interpolate data
            int gapSize = maxGapSize + 1;

            new Thread(() =>
            {
                while (true)
                {

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

                    if (log && (T % 10 == 0))
                        Console.WriteLine(rcvd);
                    if (vDevice.logOutput)
                        Console.Write(" ({0}) \n", gapSize);
                }
              
            }).Start(); 
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            if (ctrlType == CtrlTypes.CTRL_CLOSE_EVENT)
            {
                JavaProc.Kill();
            }
            return true;
        }

        public string GetModeString()
        {
            switch (Config.Mode)
            {
                case Config.MODE.ModeGamepad:
                    return "Gamepad";
                case Config.MODE.ModeJoystickCoupled:
                    return "VR: Coupled";
                case Config.MODE.ModeJoystickDecoupled:
                    return "VR: Decoupled";
                case Config.MODE.ModeJoystickTurn:
                    return "VR: Decoupled 2";
                case Config.MODE.ModeMouse:
                    return "Mouse";
                case Config.MODE.ModePaused:
                    return "Paused";
                case Config.MODE.ModeWASD:
                    return "Keyboard/Mouse";
                default:
                    return "Unrecognized";
            }
        }

        /*
         * Context menu methods
         */

        private void reset_Click(object sender, EventArgs e)
        {
            JavaProc.Kill();
            JavaProc.StartDnsService(_port); // blocks
        }

        private void smoothing2_Click(object sender, EventArgs e)
        {
            Config.RCFilterStrength *= 2;
        }

        private void smoothingHalf_Click(object sender, EventArgs e)
        {
            Config.RCFilterStrength /= 2;
        }

        public void BuildContextMenu(ContextMenuStrip contextMenuStrip)
        {
            contextMenuStrip.Items.Clear();
            contextMenuStrip.Items.AddRange(
                new ToolStripItem[] {
                    new ToolStripMenuItem(String.Format("Mode: {0}", GetModeString())), 
                    new ToolStripSeparator(),
                   ToolStripMenuItemWithHandler("&Reset Server", reset_Click),
                   ToolStripMenuItemWithHandler("Double smoothing factor", smoothing2_Click),
                   ToolStripMenuItemWithHandler("Half smoothing factor", smoothingHalf_Click),
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
