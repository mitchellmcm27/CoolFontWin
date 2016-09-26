using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SharpDX.XInput;
using CoolFont.IO;
using CoolFont.Network;
using CoolFont.Simulator;
using System.Threading;
using System.Drawing;

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

        private int Port;
        private readonly NotifyIcon NotifyIcon;

        private bool Log = false;
        private bool Verbose = false;
        private string[] args;
        private JavaProc JProc = new JavaProc();

        public VirtualDevice VDevice;

        public CoolFontWin(NotifyIcon notifyIcon, string[] args)
        {
            this.NotifyIcon = notifyIcon;
            this.args = args;
        }

        static public string PortFile = "last-port.txt";

        public void StartService()
        {
            ProcessArgs();
            int tryport = FileManager.TryToReadPortFromFile(CoolFontWin.PortFile); // returns 0 if none
            UdpListener sock = new UdpListener(tryport);
            Port = sock.Port;

            if (Port > 0 & sock.IsBound)
            {
                // write successful port to file for next time
                FileManager.WritePortToFile(Port, CoolFontWin.PortFile);
            }

            /* Register DNS service through Java */
            JProc.StartDnsService(Port); // blocks

            // check to see if everything is set up?
            ReceiveService(sock);
        }

        private void ProcessArgs()
        {
            foreach (string arg in args)
            {
                if (arg.Equals("log"))
                {
                    Log = true;
                }
                if (arg.Equals("verbose"))
                {
                    Verbose = true;
                }
            }
        }
        
        private void ReceiveService(UdpListener sock)
        {
            SetConsoleCtrlHandler(new HandlerRoutine(ConsoleCtrlCheck), true);

            /* Set up the simulator */
            XInputDeviceManager devMan = new XInputDeviceManager();
            Controller xDevice = devMan.getController();
            VDevice = new VirtualDevice(1, sock.SocketPollInterval); // will change Mode if necessary
            VDevice.LogOutput = Verbose; // T or F

            int T = 0; // total time
            int maxGapSize = 90; // set to -1 to always interpolate data
            int gapSize = maxGapSize + 1;

            new Thread(() =>
            {
                while (true)
                {

                    /* get data from iPhone socket, add to vDev */
                    string rcvd = sock.Poll();
                    bool res = VDevice.HandleNewData(rcvd);
                    gapSize = (res == true) ? 0 : gapSize + 1;

                    /* Tell vDev whether we want it to fill in missing data */
                    if (gapSize > maxGapSize) { VDevice.ShouldInterpolate = false; }

                    /* Get data from connected XInput device, add to vDev*/

                    if (xDevice != null && xDevice.IsConnected)
                    {
                        State state = xDevice.GetState();
                        VDevice.AddControllerState(state);
                    }

                    VDevice.FeedVJoy();
                    T++;

                    if (Log && (T % 10 == 0))
                        Console.WriteLine(rcvd);
                    if (VDevice.LogOutput)
                        Console.Write(" ({0}) \n", gapSize);
                }
              
            }).Start(); 
        }

        private bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            if (ctrlType == CtrlTypes.CTRL_CLOSE_EVENT)
            {
                KillOpenProcesses();
                NotifyIcon.Dispose();
            }
            return true;
        }

        public string GetModeString()
        {
            switch (VDevice.Mode)
            {
                case SimulatorMode.ModeGamepad:
                    return "Gamepad";
                case SimulatorMode.ModeJoystickCoupled:
                    return "VR: Coupled";
                case SimulatorMode.ModeJoystickDecoupled:
                    return "VR: Decoupled";
                case SimulatorMode.ModeJoystickTurn:
                    return "VR: Decoupled 2";
                case SimulatorMode.ModeMouse:
                    return "Mouse";
                case SimulatorMode.ModePaused:
                    return "Paused";
                case SimulatorMode.ModeWASD:
                    return "Keyboard/Mouse";
                default:
                    return "Unrecognized";
            }
        }

        public void KillOpenProcesses()
        {
            if (JProc != null & JProc.Running == true) { JProc.Kill(); }
        }

        /*
         * Context menu methods
         */

        private void Reset_Click(object sender, EventArgs e)
        {
            KillOpenProcesses();
            if (JProc != null) { JProc.StartDnsService(Port); }// blocks
        }

        private void SmoothingDouble_Click(object sender, EventArgs e)
        {
            VDevice.RCFilterStrength *= 2;
        }

        private void SmoothingHalf_Click(object sender, EventArgs e)
        {
            VDevice.RCFilterStrength /= 2;
        }

        public void BuildContextMenu(ContextMenuStrip contextMenuStrip)
        {
            contextMenuStrip.Items.Clear();
            ToolStripMenuItem modeItem = new ToolStripMenuItem(string.Format("Mode - {0}", GetModeString()));
            modeItem.Font = new Font(modeItem.Font, modeItem.Font.Style | FontStyle.Bold);
            modeItem.BackColor = Color.DarkSlateBlue;
            modeItem.ForeColor = Color.Lavender;
            modeItem.Enabled = false; // not clickable

            contextMenuStrip.Items.AddRange(
                new ToolStripItem[] {
                    modeItem,
                   ToolStripMenuItemWithHandler("&Reset server", Reset_Click),
                   ToolStripMenuItemWithHandler("Double smoothing factor", SmoothingDouble_Click),
                   ToolStripMenuItemWithHandler("Half smoothing factor", SmoothingHalf_Click),
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
