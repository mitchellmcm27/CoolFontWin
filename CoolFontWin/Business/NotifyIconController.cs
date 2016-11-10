using System;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;
using System.Collections;

using SharpDX.XInput;
using log4net;
using System.Net.Sockets;
using System.Collections.Specialized;

namespace CFW.Business
{
    public class NotifyIconController
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly NotifyIcon NotifyIcon;
        private DNSNetworkService NetworkService;
        private UDPServer Server;
        private DeviceManager SharedDeviceManager = DeviceManager.Instance;
        private List<string> DeviceNames;

        static public string PortFile = "last-port.txt";

        public NotifyIconController(NotifyIcon notifyIcon)
        {
            this.NotifyIcon = notifyIcon;
            this.NetworkService = new DNSNetworkService();
        }

        public void StartServices()
        {
            this.StartServices(new List<string> { "" });
        }
        public void StartServices(List<string> names)
        {
            this.DeviceNames = names;

            bool[] servicePublished = new bool[DeviceNames.Count];

            List<int> portsFromFile = FileManager.LinesToInts(FileManager.TryToReadLinesFromFile(PortFile)); // returns 0 if none

            int tryport;
            try
            {
                tryport = portsFromFile[0];
            }
            catch
            {
                tryport = 0;
            }

            Server = new UDPServer(tryport);
            Server.Start();

            int port = Server.port;
            FileManager.WritePortToLine(port, 0, PortFile);

            for (int i = 0; i < DeviceNames.Count; i++)
            {
                NetworkService.Publish(port, DeviceNames[i]);
            }
        }

        public void AddService(string name)
        {
            NetworkService.Publish(Server.port, name);
            this.DeviceNames.Add(name);

            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();
        }

        public void RemoveLastService()
        {
            string name = DeviceNames.Last();
            this.DeviceNames.Remove(name);

            NetworkService.Unpublish(name);

            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();
        }

        #region WinForms
        public void KillOpenProcesses()
        {
        }

        /*
         * Context menu methods
         */ 

        private void SmoothingDouble_Click(object sender, EventArgs e)
        {
            SharedDeviceManager.SmoothingFactor *= 2;
        }

        private void SmoothingHalf_Click(object sender, EventArgs e)
        {
            SharedDeviceManager.SmoothingFactor /= 2;
        }

        private void SelectedMode_Click(object sender, EventArgs e)
        {
            log.Debug(sender);
            bool res = SharedDeviceManager.TryMode((int)((ToolStripMenuItem)sender).Tag);

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
            if (SharedDeviceManager.VJoyDeviceConnected)
            {
                SharedDeviceManager.FlipAxis(Axis.AxisX);
            }
        }

        private void FlipY_Click(object sender, EventArgs e)
        {
            if (SharedDeviceManager.VJoyDeviceConnected)
            {
                SharedDeviceManager.FlipAxis(Axis.AxisY);
            }
        }

        private bool WillAddIPhoneOnRestart = false;

        private void addIPhone_Click(object sender, EventArgs e)
        {
            var devicesCol = Properties.Settings.Default.ConnectedDevices;

            string name;
            switch (devicesCol.Count)
            {
                case 0:
                    name = "Primary";
                    break;
                case 1:
                    name = "Secondary";
                    break;
                default:
                    name = "Device " + (devicesCol.Count+1).ToString();
                    break;
            }

            AddService(name);

            devicesCol.Add(name);
            Properties.Settings.Default.ConnectedDevices = devicesCol;
            Properties.Settings.Default.Save();
        }

        private void removeIphone_Click(object sender, EventArgs e)
        {
            RemoveLastService();
        }

        private void addXInput_Click(object sender, EventArgs e)
        {
            if (!SharedDeviceManager.InterceptXInputDevice)
            {
                bool found = SharedDeviceManager.AcquireXInputDevice();
                if (!found) SharedDeviceManager.InterceptXInputDevice = false;
            }
            SharedDeviceManager.InterceptXInputDevice = !SharedDeviceManager.InterceptXInputDevice;
        }

        /**
         * Build the main context menu items and submenus
         * */
        public void AddToContextMenu(ContextMenuStrip contextMenuStrip)
        {
            // Mode submenu
            ToolStripMenuItem modeSubMenu = new ToolStripMenuItem(String.Format("Mode ({0})", GetDescription(SharedDeviceManager.Mode)));
            modeSubMenu.Image = SharedDeviceManager.CurrentModeIsFromPhone ? Properties.Resources.ic_phone_iphone_white_18dp : Properties.Resources.ic_link_white_18dp;
#if DEBUG
            int numModes = (int)SimulatorMode.ModeCountDebug;
#else
            int numModes = (int)SimulatorMode.ModeCountRelease;
#endif
            for (int i = 0; i < numModes; i++)
            {
                var item = ToolStripMenuItemWithHandler(GetDescription((SimulatorMode)i), SelectedMode_Click);
                item.Tag = i; // = SimulatorMode value
                item.Font = new Font(modeSubMenu.Font, modeSubMenu.Font.Style | FontStyle.Regular);
                if (i == (int)SharedDeviceManager.Mode)
                {
                    item.Font = new Font(modeSubMenu.Font, modeSubMenu.Font.Style | FontStyle.Bold);
                    item.Image = Properties.Resources.ic_done_white_16dp;
                }
                modeSubMenu.DropDownItems.Add(item);
            }

            // vJoy config and monitor
            ToolStripMenuItem flipXItem = ToolStripMenuItemWithHandler("Flip X-axis", FlipX_Click);
            flipXItem.Image = Properties.Resources.ic_swap_horiz_white_18dp;
            ToolStripMenuItem flipYItem = ToolStripMenuItemWithHandler("Flip Y-axis", FlipY_Click);
            flipYItem.Image = Properties.Resources.ic_swap_vert_white_18dp;

            ToolStripMenuItem vJoyConfItem = ToolStripMenuItemWithHandler("Launch Config", VJoyConf_Click);
            vJoyConfItem.Image = Properties.Resources.ic_open_in_browser_white_18dp;

            ToolStripMenuItem vJoyMonItem = ToolStripMenuItemWithHandler("Launch Monitor", VJoyMon_Click);
            vJoyMonItem.Image = Properties.Resources.ic_open_in_browser_white_18dp;

            ToolStripMenuItem vJoySubMenu = new ToolStripMenuItem("Virtual Joystick");
            vJoySubMenu.Image = Properties.Resources.ic_settings_white_18dp;
            vJoySubMenu.DropDownItems.AddRange(new ToolStripItem[] {
                flipXItem,
                flipYItem,
                new ToolStripSeparator(),
                vJoyConfItem,
                vJoyMonItem,
            });

            // Smoothing factor adjustment
            ToolStripMenuItem smoothingDoubleItem = ToolStripMenuItemWithHandler("Increase signal smoothing", SmoothingDouble_Click);
            smoothingDoubleItem.Image = Properties.Resources.ic_line_weight_white_18dp;
            ToolStripMenuItem smoothingHalfItem = ToolStripMenuItemWithHandler("Decrease signal smoothing", SmoothingHalf_Click);
            smoothingHalfItem.Image = Properties.Resources.ic_line_style_white_18dp;

            // Connect to multiple devices, add device
            ToolStripMenuItem deviceSubMenu = new ToolStripMenuItem(String.Format("Manage devices"));
            deviceSubMenu.Image = SharedDeviceManager.CurrentModeIsFromPhone ? Properties.Resources.ic_phone_iphone_white_18dp : Properties.Resources.ic_link_white_18dp;

            ToolStripMenuItem addIPhoneItem = ToolStripMenuItemWithHandler("Add Secondary Leg", addIPhone_Click);
            addIPhoneItem.Image = Properties.Resources.ic_settings_cell_white_18dp;
            addIPhoneItem.Enabled = DeviceNames.Count > 1 ? false : true;

            ToolStripMenuItem removeIPhoneItem = ToolStripMenuItemWithHandler("Remove Secondary Leg", removeIphone_Click);
            removeIPhoneItem.Image = null;
            removeIPhoneItem.Enabled = DeviceNames.Count > 1 ? true: false;

            ToolStripMenuItem addXboxControllerItem = ToolStripMenuItemWithHandler("Intercept XBox controller", addXInput_Click);
            if (SharedDeviceManager.Mode==SimulatorMode.ModeWASD)
            {
                addXboxControllerItem.Enabled = false;
            }
            addXboxControllerItem.Image = SharedDeviceManager.InterceptXInputDevice ? Properties.Resources.ic_done_white_16dp : null;

            deviceSubMenu.DropDownItems.AddRange(new ToolStripItem[] { addIPhoneItem, removeIPhoneItem, addXboxControllerItem });

            // Add to Context Menu Strip
            contextMenuStrip.Items.AddRange(
                new ToolStripItem[] {
                    modeSubMenu,
                    vJoySubMenu,
                    new ToolStripSeparator(),
                    smoothingDoubleItem,
                    smoothingHalfItem,
                    new ToolStripSeparator(),
                    deviceSubMenu,
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
