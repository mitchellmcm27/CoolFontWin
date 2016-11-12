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
        private DeviceManager SharedDeviceManager;
        private List<string> DeviceNames;
        private bool UDPServerRunning = false;

        static public string PortFile = "last-port.txt";

        public NotifyIconController(NotifyIcon notifyIcon)
        {
            this.NotifyIcon = notifyIcon;
            this.NetworkService = new DNSNetworkService();
            this.Server = new UDPServer();
            this.SharedDeviceManager = DeviceManager.Instance;
        }

        /// <summary>
        /// Starts network services (DNS and UDP server) for each device in Default Settings.
        /// </summary>
        public void StartServices()
        {
            // get list of default devices
            var devicesCol = Properties.Settings.Default.ConnectedDevices;
            StartServices(devicesCol.Cast<string>().ToList());
        }

        /// <summary>
        /// Starts network services (DNS and UDP server) for each device by name.
        /// </summary>
        /// <param name="names">List of strings represting device names.</param>
        public void StartServices(List<string> names)
        {
            this.DeviceNames = names;
            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;

            // read past-used ports from file
            // returns 0 if file not found or other error
            List<int> portsFromFile = FileManager.LinesToInts(FileManager.TryToReadLinesFromFile(PortFile));

            int tryport = 0;
            foreach(int p in portsFromFile)
            {
                try
                {
                    tryport = p;
                    break;
                }
                catch { }
            }

            Server.Start(tryport);

            // get whatever port finally worked
            int port = Server.Port;
            FileManager.WritePortToLine(port, 0, PortFile);

            // publish one service for each device
            for (int i = 0; i < DeviceNames.Count; i++)
            {
                NetworkService.Publish(port, DeviceNames[i]);
            }

            UDPServerRunning = true;
        }

        /// <summary>
        /// Publish a new network service on the same port.
        /// </summary>
        /// <param name="name">Name to append to the service (device name).</param>
        public void AddService(string name)
        {
            NetworkService.Publish(Server.Port, name);
            this.DeviceNames.Add(name);

            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;

            // update Defaults with this name
            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Remove the last service that was published.
        /// </summary>
        public void RemoveLastService()
        {
            // get last-added device name
            string name = DeviceNames.Last();
            this.DeviceNames.Remove(name);

            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;

            // unpublish service containing this name
            NetworkService.Unpublish(name);

            // update Defaults 
            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();
        }

        #region WinForms
        public void Dispose()
        {
            SharedDeviceManager.Dispose(); // Relinquish devices
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
            if (launched)
            {
                SharedDeviceManager.RelinquishCurrentDevice();
            }
            else
            {
                ShowVJoyNotFoundMessageBox(fname, "Configure vJoy");
            }

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

        private string AddRemoveLegString()
        {
            if (DeviceNames.Count > 1)
            {
                return "Remove secondary leg";
            }
            else
            {
                return "Add secondary leg";
            }
        }

        private void addRemoveSecondaryLeg_Click(object sender, EventArgs e)
        {
            if (DeviceNames.Count > 1)
            {
                RemoveLastService();
            }
            else
            {
                AddService("Secondary");
            }
        }

        private string AddRemoveXboxControllerString()
        {
            if (!SharedDeviceManager.InterceptXInputDevice)
            {
                return "Use Xbox controller";
            }
            else
            {
                return "Release Xbox controller";
            }
        }

        private void addRemoveXboxController_Click(object sender, EventArgs e)
        {
            if (!SharedDeviceManager.InterceptXInputDevice)
            {
                bool found = SharedDeviceManager.AcquireXInputDevice();
                if (!found) SharedDeviceManager.InterceptXInputDevice = false;
            }
            SharedDeviceManager.InterceptXInputDevice = !SharedDeviceManager.InterceptXInputDevice;
        }

        private void deviceID_Click(object sender, EventArgs e)
        {
            int id = (int)((ToolStripMenuItem)sender).Tag;
            if (id==0)
            {
                SharedDeviceManager.RelinquishCurrentDevice();
                Properties.Settings.Default.VJoyID = id;
                Properties.Settings.Default.Save();
            }

            if (SharedDeviceManager.AcquireVJoyDevice((uint)id))
            {
                Properties.Settings.Default.VJoyID = id;
                Properties.Settings.Default.Save();
            }
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
            ToolStripMenuItem fwdKeyItem = ToolStripMenuItemWithHandler("Rebind forward key", null);
            fwdKeyItem.Image = Properties.Resources.ic_gamepad_white_18dp;
            fwdKeyItem.Enabled = false;

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
                fwdKeyItem,
                flipXItem,
                flipYItem,
                new ToolStripSeparator(),
                vJoyConfItem,
                vJoyMonItem,
            });

            ToolStripMenuItem vJoySelectSubMenu = new ToolStripMenuItem(String.Format("Select a vJoy device", Properties.Settings.Default.VJoyID));
            if (SharedDeviceManager.CurrentDeviceID == 0)
            {
                vJoySelectSubMenu.Image = Properties.Resources.ic_error_outline_white_18dp;
            }
            else
            {
                vJoySelectSubMenu.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                vJoySelectSubMenu.ImageAlign = ContentAlignment.MiddleCenter;
                vJoySelectSubMenu.Image = Drawing.CreateBitmapImage(SharedDeviceManager.CurrentDeviceID.ToString(), Color.White);
            }

            ToolStripItem[] deviceIDItems = new ToolStripItem[17];
            for (int i = 0; i < 17; i++)
            {
                // valid vjoy IDs are 1-16

                var item = ToolStripMenuItemWithHandler((i).ToString(), deviceID_Click);
                item.Tag = i;
                item.Enabled = SharedDeviceManager.EnabledVJoyDevicesList.Contains(i);

                // 0 is to remove vJoy devices
                if (i == 0)
                {
                    item.Text = "None";
                    item.Enabled = true;
                }

                if (i == SharedDeviceManager.CurrentDeviceID)
                {
                    item.Font = new Font(item.Font, modeSubMenu.Font.Style | FontStyle.Bold);
                    item.Image = Properties.Resources.ic_done_white_16dp;
                }

                deviceIDItems[i] = item;
            }
            vJoySelectSubMenu.DropDownItems.AddRange(deviceIDItems);

            // Smoothing factor adjustment
            ToolStripMenuItem smoothingDoubleItem = ToolStripMenuItemWithHandler("Increase signal smoothing", SmoothingDouble_Click);
            smoothingDoubleItem.Image = Properties.Resources.ic_line_weight_white_18dp;
            ToolStripMenuItem smoothingHalfItem = ToolStripMenuItemWithHandler("Decrease signal smoothing", SmoothingHalf_Click);
            smoothingHalfItem.Image = Properties.Resources.ic_line_style_white_18dp;

            // Connect to multiple devices, add device
            ToolStripMenuItem deviceSubMenu = new ToolStripMenuItem(String.Format("Manage devices"));
            deviceSubMenu.Image = SharedDeviceManager.CurrentModeIsFromPhone ? Properties.Resources.ic_phone_iphone_white_18dp : Properties.Resources.ic_link_white_18dp;

            ToolStripMenuItem addRemoveSecondaryLegItem = ToolStripMenuItemWithHandler(AddRemoveLegString(), addRemoveSecondaryLeg_Click);
            addRemoveSecondaryLegItem.Image = DeviceNames.Count > 1 ? Properties.Resources.ic_phonelink_erase_white_18dp : Properties.Resources.ic_phonelink_ring_white_18dp;
            addRemoveSecondaryLegItem.Enabled = true;

            ToolStripMenuItem addRemoveXboxControllerItem = ToolStripMenuItemWithHandler(AddRemoveXboxControllerString(), addRemoveXboxController_Click);
            if (SharedDeviceManager.Mode==SimulatorMode.ModeWASD)
            {
                addRemoveXboxControllerItem.Enabled = false;
            }
            addRemoveXboxControllerItem.Image = SharedDeviceManager.InterceptXInputDevice ? Properties.Resources.ic_clear_white_18dp : Properties.Resources.ic_videogame_asset_white_18dp;

            deviceSubMenu.DropDownItems.AddRange(new ToolStripItem[] { addRemoveSecondaryLegItem, addRemoveXboxControllerItem });

            // Add to Context Menu Strip
            contextMenuStrip.Items.AddRange(
                new ToolStripItem[] {
                    modeSubMenu,
                    vJoySubMenu,
                    vJoySelectSubMenu,
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
