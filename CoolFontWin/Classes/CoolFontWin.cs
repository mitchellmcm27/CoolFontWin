using System;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;

using SharpDX.XInput;
using CoolFont.IO;
using CoolFont.Network;
using CoolFont.Simulator;

namespace CoolFont
{
    public class CoolFontWin
    {
        private int Port;
        private readonly NotifyIcon NotifyIcon;

        private bool LogRcvd = false;
        private bool Verbose = false;
        private bool InterceptXInputDevice = false;
        private string[] args;

        public VirtualDevice VDevice;
        public UdpListener sock;
        private bool servicePublished;

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
            sock = new UdpListener(tryport);
            Port = sock.Port;

            if (Port > 0 & sock.IsBound)
            {
                // write successful port to file for next time
                FileManager.WritePortToFile(Port, CoolFontWin.PortFile);
            }

            /* Register DNS service through Mono.Zeroconf */
            servicePublished = sock.PublishOnPort((short)Port);

            ReceiveService(sock);
        }

        private void ProcessArgs()
        {
            foreach (string arg in args)
            {
                if (arg.Equals("log"))
                {
                    LogRcvd = true;
                }
                if (arg.Equals("verbose"))
                {
                    Verbose = true;
                }
                if (arg.Equals("intercept"))
                {
                    InterceptXInputDevice = true;
                }
            }
        }
        
        private void ReceiveService(UdpListener sock)
        {
            /* Set up the simulator */
            Controller xDevice;

            if (InterceptXInputDevice)
            {
                XInputDeviceManager devMan = new XInputDeviceManager();
                xDevice = devMan.getController();
            }
            else
            {
                xDevice = null;
            }
            VDevice = new VirtualDevice(1, sock.SocketPollInterval); // will change Mode if necessary
            VDevice.LogOutput = Verbose; // T or F

            int T = 0; // total time
            int maxGapSize = 90; // set to -1 to always interpolate data
            int gapSize = maxGapSize + 1;

           // VDevice.LogOutput = true;
            new Thread(() =>
            {
                while (true)
                {

                    /* get data from iPhone socket, add to vDev */
                    string rcvd = sock.Poll();
                    bool res = VDevice.HandleNewData(rcvd);
                    gapSize = (res == true) ? 0 : gapSize + 1;

                    /* Tell vDev whether to fill in missing data */
                    if (gapSize > maxGapSize) { VDevice.ShouldInterpolate = false; }

                    /* Get data from connected XInput device, add to vDev*/         
                    if (InterceptXInputDevice && xDevice != null && xDevice.IsConnected)
                    {
                        State state = xDevice.GetState();
                        VDevice.AddControllerState(state);
                    }

                    VDevice.FeedVJoy();
                    T++;

                    if (LogRcvd && (T % 1 == 0))
                        Console.Write("{0}\n", rcvd);

                    if (VDevice.LogOutput) // simulator will write some stuff, then...
                        Console.Write("({0})\n", gapSize);     
                }
              
            }).Start(); 
        }
        #region WinForms
        public void KillOpenProcesses()
        {
            if (servicePublished) { sock.Service.Dispose(); }
        }

        /*
         * Context menu methods
         */ 

        private void SmoothingDouble_Click(object sender, EventArgs e)
        {
            VDevice.RCFilterStrength *= 2;
        }

        private void SmoothingHalf_Click(object sender, EventArgs e)
        {
            VDevice.RCFilterStrength /= 2;
        }

        private void SelectedMode_Click(object sender, EventArgs e)
        {
            Console.WriteLine(sender);
            bool res = VDevice.ClickedMode((int)((ToolStripMenuItem)sender).Tag);
            if (!res)
            {
                MessageBox.Show("Enable vJoy and restart to use this mode", "Unable to switch modes", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }
        }

        private void VJoyConf_Click(object sender, EventArgs e)
        {
            string fname = "vJoyConf.exe";
            bool launched = FileManager.FindAndLaunch(Properties.Settings.Default.VJoyDir, fname);
            if (!launched) { ShowVJoyNotFoundMessageBox(fname, "Configure vJoy"); }

        }

        private void VJoyMon_Click(object sender, EventArgs e)
        {
            string fname = "JoyMonitor.exe";
            bool launched = FileManager.FindAndLaunch(Properties.Settings.Default.VJoyDir, fname);
            if (!launched) { ShowVJoyNotFoundMessageBox(fname, "Monitor vJoy"); }
        }

        private void VJoyOK_Click()
        {
            // drop-in replacement for default dialog
            Ookii.Dialogs.VistaFolderBrowserDialog fbd = new Ookii.Dialogs.VistaFolderBrowserDialog();
            fbd.Description = "Select vJoy Folder (usually C:\\Program Files\\vJoy)";
            fbd.UseDescriptionForTitle = true;
            fbd.ShowNewFolderButton = false;
            DialogResult result = fbd.ShowDialog();
            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                Properties.Settings.Default.VJoyDir = fbd.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void ShowVJoyNotFoundMessageBox(string fname, string description)
        {
            string title = "vJoy not found in default directory";
            string message = String.Format("If vJoy is installed, you can launch the '{0}' app for Windows or just browse to the vJoy install location manually. \n \n Browse to vJoy folder manually?", description);
            DialogResult clicked = MessageBox.Show(message, title, MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (clicked == DialogResult.OK)
            {
                VJoyOK_Click();
            }
        }

        private void FlipX_Click(object sender, EventArgs e)
        {
            if (VDevice != null)
            {
                VDevice.signX = -VDevice.signX;
            }
        }

        private void FlipY_Click(object sender, EventArgs e)
        {
            if (VDevice != null)
            {
                VDevice.signY = -VDevice.signY;
            }
        }

        /**
         * Build the main context menu items and submenus
         * */
        public void BuildContextMenu(ContextMenuStrip contextMenuStrip)
        {
            contextMenuStrip.Items.Clear();

            // Mode submenu
            ToolStripMenuItem modeSubMenu = new ToolStripMenuItem(String.Format("Mode ({0})", GetDescription(VDevice.Mode)));
            //modeSubMenu.Font = new Font(modeSubMenu.Font, modeSubMenu.Font.Style | FontStyle.Bold);         
#if DEBUG
            int numModes = (int)SimulatorMode.ModeCountDebug;
#else
            int numModes = (int)SimulatorMode.ModeCountRelease;
#endif
            for (int i=0; i < numModes; i++)
            {
                var item = ToolStripMenuItemWithHandler(GetDescription((SimulatorMode)i), SelectedMode_Click);
                item.Tag = i; // = SimulatorMode value
                item.Font = new Font(modeSubMenu.Font, modeSubMenu.Font.Style | FontStyle.Regular);
                if (i==(int)VDevice.Mode)
                {
                    item.Font = new Font(modeSubMenu.Font, modeSubMenu.Font.Style | FontStyle.Bold);
                }
                modeSubMenu.DropDownItems.Add(item);
            }

            // vJoy config and monitor
            ToolStripMenuItem flipXItem = ToolStripMenuItemWithHandler("Flip X-axis", FlipX_Click);         
            ToolStripMenuItem flipYitem = ToolStripMenuItemWithHandler("Flip Y-axis", FlipY_Click);

            ToolStripMenuItem vJoyConfItem = ToolStripMenuItemWithHandler("Configure", VJoyConf_Click);
            vJoyConfItem.Image = Properties.Resources._383_new_tab;
            ToolStripMenuItem vJoyMonItem = ToolStripMenuItemWithHandler("Monitor", VJoyMon_Click);
            vJoyMonItem.Image = Properties.Resources._383_new_tab;

            ToolStripMenuItem vJoySubMenu = new ToolStripMenuItem("Virtual Joystick");
            vJoySubMenu.DropDownItems.AddRange(new ToolStripItem[] {
                flipXItem,
                flipYitem,
                vJoyConfItem,
                vJoyMonItem,
            });

            // Smoothing factor adjustment
            ToolStripMenuItem smoothingDoubleItem = ToolStripMenuItemWithHandler("Double smoothing factor", SmoothingDouble_Click);
            smoothingDoubleItem.Image = Properties.Resources._315_arrow_up2;
            ToolStripMenuItem smoothingHalfItem = ToolStripMenuItemWithHandler("Half smoothing factor", SmoothingHalf_Click);
            smoothingHalfItem.Image = Properties.Resources._319_arrow_down2;


            // Add to Context Menu Strip
            contextMenuStrip.Items.AddRange(
                new ToolStripItem[] {
                    modeSubMenu,
                    vJoySubMenu,
                    new ToolStripSeparator(),
                    smoothingDoubleItem,
                   smoothingHalfItem,
                });          
        }

        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        public ToolStripMenuItem ToolStripMenuItemWithHandler(string displayText, EventHandler eventHandler)
        {
            var item = new ToolStripMenuItem(displayText);
            // add image
            if (eventHandler != null) { item.Click += eventHandler; }
            return item;
        }

        #endregion
    }
}
