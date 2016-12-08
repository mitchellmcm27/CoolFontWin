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

    public enum Output
    {
        Keyboard,
        VJoy,
        XBox
    }

    public class NotifyIconController
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly NotifyIcon NotifyIcon;
        public DNSNetworkService NetworkService;
        public UDPServer Server;
        public DeviceManager SharedDeviceManager;
        public List<string> DeviceNames;
        public bool UDPServerRunning = false;
        public List<int> CurrentDevices
        {
            get { return SharedDeviceManager.EnabledVJoyDevicesList; }
        }

        //static public string PortFile = "last-port.txt";

        public NotifyIconController(NotifyIcon notifyIcon)
        {
            this.NotifyIcon = notifyIcon;
            this.NetworkService = new DNSNetworkService();
            this.Server = new UDPServer();
            Server.ClientAdded += Server_ClientAdded;

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

            Server.Start(Properties.Settings.Default.LastPort);
            
            UDPServerRunning = true;

            // get whatever port finally worked and save it
            int port = Server.Port;
            Properties.Settings.Default.LastPort = port;
            Properties.Settings.Default.Save();

            // publish 1 network service for each device
            for (int i = 0; i < DeviceNames.Count; i++)
            {
                NetworkService.Publish(port, DeviceNames[i]);
            }
        }

        private void Server_ClientAdded(object sender, EventArgs e)
        {
            // Commented out because DeviceManager now plays sounds when devices come and go
            // ResourceSoundPlayer.TryToPlay(Properties.Resources.reverb_good);
        }

        /// <summary>
        /// Publish a new network service on the same port.
        /// </summary>
        /// <param name="name">Name to append to the service (device name).</param>
        public void AddService(string name)
        {

            if (NetworkService.Publish(Server.Port, name))
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
                this.DeviceNames.Add(name);
            }
            

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
            if (DeviceNames.Count==1)
            {
                return;
            }
            // get last-added device name
            string name = DeviceNames.Last();
            this.DeviceNames.Remove(name);

            SharedDeviceManager.MobileDevicesCount = this.DeviceNames.Count;

            // unpublish service containing this name
            NetworkService.Unpublish(name);
            ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);

            // update Defaults 
            StringCollection collection = new StringCollection();
            collection.AddRange(DeviceNames.ToArray());
            Properties.Settings.Default.ConnectedDevices = collection;
            Properties.Settings.Default.Save();
        }

        public void Dispose()
        {
            // Relinquish connected devices
            SharedDeviceManager.Dispose();
        }

        #region ContextMenuStrip handlers and creation

        private Image getImageFromMode(SimulatorMode mode)
        {
            switch (mode)
            {
                case SimulatorMode.ModeWASD:
                    return Properties.Resources.ic_keyboard_white_18dp;
                case SimulatorMode.ModeJoystickCoupled:
                case SimulatorMode.ModeJoystickDecoupled:
                case SimulatorMode.ModeJoystickTurn:
                    return Properties.Resources.ic_videogame_asset_white_18dp;
                case SimulatorMode.ModePaused:
                    return Properties.Resources.ic_pause_white_18dp;
                default:
                    return null;
            }
        }

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

            if (res)
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_good);
            }
            else
            {
                ResourceSoundPlayer.TryToPlay(Properties.Resources.beep_bad);
                MessageBox.Show("Select an output device to use this mode.", "Unable to Switch Modes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void VJoyConf_Click(object sender, EventArgs e)
        {
            string fname = "vJoyConf.exe";
            string path = System.IO.Path.Combine("Program Files", "vJoy");
            try
            {
                System.Diagnostics.Process.Start(System.IO.Path.Combine(Properties.Settings.Default.VJoyDir, fname));
            }
            catch
            {
                string foundPath = FileManager.FindAndLaunch(path, fname);
                if (foundPath.Equals(string.Empty))
                {
                    ShowVJoyNotFoundMessageBox(fname, "Configure vJoy");
                }
                else
                {
                    log.Info("Found " + fname + " in " + foundPath + ". Saving in default setting for later use.");
                    Properties.Settings.Default.VJoyDir = foundPath;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void VJoyMon_Click(object sender, EventArgs e)
        {
            string fname = "JoyMonitor.exe";
            string path = System.IO.Path.Combine("Program Files", "vJoy");
            try
            {
                System.Diagnostics.Process.Start(System.IO.Path.Combine(Properties.Settings.Default.VJoyDir, fname));
            }
            catch
            {
                string foundPath = FileManager.FindAndLaunch(path, fname);
                if (foundPath.Equals(string.Empty))
                {
                    ShowVJoyNotFoundMessageBox(fname, "Monitor vJoy");
                }
                else
                {
                    log.Info("Found " + fname + " in " + foundPath + ". Saving in default setting for later use.");
                    Properties.Settings.Default.VJoyDir = foundPath;
                    Properties.Settings.Default.Save();
                }
            }
        }
    
        private void UnplugAll_Click(object sender, EventArgs e)
        {
            DeviceManager.Instance.ForceUnplugAllXboxControllers();
        }

        /// <summary>
        /// Dialog box for navigating to vJoy installation directory.
        /// </summary>
        private void VJoyOK_Click()
        {
            // Ookii: drop-in replacement for default dialog
            Ookii.Dialogs.VistaFolderBrowserDialog folderBrowser = new Ookii.Dialogs.VistaFolderBrowserDialog();
            folderBrowser.Description = "Select vJoy Folder (usually C:\\Program Files\\vJoy)";
            folderBrowser.UseDescriptionForTitle = true;
            folderBrowser.ShowNewFolderButton = false;

            DialogResult result = folderBrowser.ShowDialog();

            if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
            {
                Properties.Settings.Default.VJoyDir = folderBrowser.SelectedPath;
                Properties.Settings.Default.Save();
            }
        }

        private void ShowVJoyNotFoundMessageBox(string fname, string description)
        {
            string title = "vJoy installation not found!";
            string message = String.Format("If vJoy is installed, you can launch the '{0}' app for Windows, or just browse to the vJoy install location manually. \n \n Browse to vJoy folder manually?", description);
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


        private void addRemoveMobileDevice_Click(object sender, EventArgs e)
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
            string str = "Physical Xbox controller";
            if (SharedDeviceManager.Mode == SimulatorMode.ModeWASD)
            {
                str += "\n(switch to gamepad)";
            }
            return str; 
        }

        private void addRemoveXboxController_Click(object sender, EventArgs e)
        {
            SharedDeviceManager.InterceptXInputDevice = !SharedDeviceManager.InterceptXInputDevice;
        }

        private void deviceID_Click(object sender, EventArgs e)
        {
            int id;
            if (((ToolStripMenuItem)sender).Tag.Equals("alert"))
            {
                id = 0;
            }
            else
            {
                id = (int)((ToolStripMenuItem)sender).Tag;
            }

            if (id==0)
            {
                SharedDeviceManager.RelinquishCurrentDevice();
                Properties.Settings.Default.VJoyID = id;
                Properties.Settings.Default.Save();
            }

            else if (SharedDeviceManager.AcquireVDev((uint)id))
            {
                Properties.Settings.Default.VJoyID = id;
                Properties.Settings.Default.Save();
            }
        }

        private void ViewLog_Click(Object sender, EventArgs e)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                log.Info(path + "\\CoolFontWin\\Log.txt");
                System.Diagnostics.Process.Start(path + "\\CoolFontWin\\Log.txt");
            }
            catch (Exception ex)
            {
                log.Error("Error opening Log.txt: " + ex);
            }
        }

        /// <summary>
        /// Build most of the ContextMenuStrip items and add to menu.
        /// </summary>
        /// <param name="cms">ContextMenuStrip to add items to.</param>
        public void AddToContextMenu(ContextMenuStrip cms)
        {

            // Mode submenu - Display current mode and change modes in dropdown menu
            ToolStripMenuItem modeSelectSubmenu = new ToolStripMenuItem(String.Format("Output mode - {0}", GetDescription(SharedDeviceManager.Mode)));
            modeSelectSubmenu.Image = getImageFromMode(SharedDeviceManager.Mode);
            modeSelectSubmenu.ImageScaling = ToolStripItemImageScaling.None;

            List<string> Modes = CFWMode.GetDescriptions();

            for (int i = 0; i < Modes.Count; i++)
            {
                var item = ToolStripMenuItemWithHandler(Modes[i], SelectedMode_Click);
                item.Tag = i; // = SimulatorMode enum value
                item.Font = new Font(item.Font, item.Font.Style | FontStyle.Regular);
                if (i == (int)SharedDeviceManager.Mode)
                {
                    item.Font = new Font(item.Font, item.Font.Style | FontStyle.Bold);
                    item.Image = Properties.Resources.ic_check_blue_18dp;
                    item.ImageScaling = ToolStripItemImageScaling.None; 
                }
                modeSelectSubmenu.DropDownItems.Add(item);
            }

            // vJoy config menu - Change keybinds, monitor/config vJoy, flip axes...
            ToolStripMenuItem fwdKeyItem = ToolStripMenuItemWithHandler("Rebind forward key", null);
            fwdKeyItem.Image = Properties.Resources.ic_gamepad_white_18dp;
            fwdKeyItem.ImageScaling = ToolStripItemImageScaling.None;
            fwdKeyItem.Enabled = false;

            ToolStripMenuItem flipXItem = ToolStripMenuItemWithHandler("Flip X-axis", FlipX_Click);
            flipXItem.Image = Properties.Resources.ic_swap_horiz_white_18dp;
            flipXItem.ImageScaling = ToolStripItemImageScaling.None;
            ToolStripMenuItem flipYItem = ToolStripMenuItemWithHandler("Flip Y-axis", FlipY_Click);
            flipYItem.Image = Properties.Resources.ic_swap_vert_white_18dp;
            flipYItem.ImageScaling = ToolStripItemImageScaling.None;

            ToolStripMenuItem vJoyConfItem = ToolStripMenuItemWithHandler("vJoy Config", VJoyConf_Click);
            vJoyConfItem.Image = Properties.Resources.ic_launch_white_18dp;
            vJoyConfItem.ImageScaling = ToolStripItemImageScaling.None;

            ToolStripMenuItem vJoyMonItem = ToolStripMenuItemWithHandler("vJoy Monitor", VJoyMon_Click);
            vJoyMonItem.Image = Properties.Resources.ic_launch_white_18dp;
            vJoyMonItem.ImageScaling = ToolStripItemImageScaling.None;

            // Smoothing factor adjustment - double or half
            ToolStripMenuItem smoothingDoubleItem = ToolStripMenuItemWithHandler("Increase signal smoothing", SmoothingDouble_Click);
            smoothingDoubleItem.ImageScaling = ToolStripItemImageScaling.None;
            smoothingDoubleItem.ImageAlign = ContentAlignment.MiddleCenter;
            smoothingDoubleItem.Image = Drawing.CreateBitmapImage("+", Color.White);

            ToolStripMenuItem smoothingHalfItem = ToolStripMenuItemWithHandler("Decrease signal smoothing", SmoothingHalf_Click);
            smoothingHalfItem.ImageScaling = ToolStripItemImageScaling.None;
            smoothingHalfItem.ImageAlign = ContentAlignment.MiddleCenter;
            smoothingHalfItem.Image = Drawing.CreateBitmapImage("-", Color.White);

            ToolStripMenuItem logItem = ToolStripMenuItemWithHandler("View log file", ViewLog_Click);
            logItem.Image = Properties.Resources.ic_folder_open_white_18dp;
            logItem.ImageScaling = ToolStripItemImageScaling.None;

            ToolStripMenuItem ConfigureOutputSubmenu = new ToolStripMenuItem("Debug options");
            ConfigureOutputSubmenu.Image = null;
            ConfigureOutputSubmenu.ImageScaling = ToolStripItemImageScaling.None;
            ConfigureOutputSubmenu.DropDownItems.AddRange(new ToolStripItem[] {
                fwdKeyItem,
                flipXItem,
                flipYItem,
                new ToolStripSeparator(),
                vJoyConfItem,
                vJoyMonItem,
                new ToolStripSeparator(),
                smoothingDoubleItem,
                smoothingHalfItem,
                new ToolStripSeparator(),
                logItem
            });

            // Select vJoy Device menu - Select a vJoy device ID, 1-16 or None
            ToolStripMenuItem outputSelectSubmenu = new ToolStripMenuItem(String.Format("Output virtual gamepad", Properties.Settings.Default.VJoyID));
            if (SharedDeviceManager.CurrentDeviceID == 0)
            {
                outputSelectSubmenu.Image = Properties.Resources.ic_error_orange_18dp;
                //vJoySelectSubMenu.Tag = "alert";
            }
            else
            {
                outputSelectSubmenu.ImageScaling = ToolStripItemImageScaling.SizeToFit;
                outputSelectSubmenu.ImageAlign = ContentAlignment.MiddleCenter;
                uint id = SharedDeviceManager.CurrentDeviceID;
                if (id>1000)
                {
                    switch (id-1000)
                    {
                        case 1:
                            outputSelectSubmenu.Image = Properties.Resources.ic_xbox_1p_blue_18dp;
                            break;
                        case 2:
                            outputSelectSubmenu.Image = Properties.Resources.ic_xbox_2p_blue_18dp;
                            break;
                        case 3:
                            outputSelectSubmenu.Image = Properties.Resources.ic_xbox_3p_blue_18dp;
                            break;
                        case 4:
                            outputSelectSubmenu.Image = Properties.Resources.ic_xbox_4p_blue_18dp;
                            break;
                        default:
                            outputSelectSubmenu.Image = Properties.Resources.ic_xbox_all_blue_18dp;
                            break;
                    }
                    
                }
                else
                {
                    string idString = id.ToString();
                    Color idColor = Colors.IconBlue;
                    outputSelectSubmenu.Image = Drawing.CreateBitmapImage(idString, idColor);
                }
                
            }
            outputSelectSubmenu.ImageScaling = ToolStripItemImageScaling.None;
            List<ToolStripItem> deviceIDItems = new List<ToolStripItem>();

            foreach (int i in DeviceManager.ValidDevIDList)
            {
                // valid vjoy IDs are 1-16

                var item = ToolStripMenuItemWithHandler((i).ToString(), deviceID_Click);
                item.Tag = i;
                item.Visible = SharedDeviceManager.EnabledVJoyDevicesList.Contains(i);

                // doesn't update after install scpvbus

                // 0 is to remove vJoy devices
                if (i == 0)
                {
                    item.Text = "None";
                    item.Enabled = true;
                    item.Visible = true;
                }
                else if (i<=1000)
                {
                    item.Text = "vJoy " + (i);
                }  
                else
                {
                    item.Text = "vXbox " + (i - 1000);
                }

                if (i == SharedDeviceManager.CurrentDeviceID)
                {
                    //if (i==0) item.Tag = "alert";
                    item.Font = new Font(cms.Font, cms.Font.Style | FontStyle.Bold);
                    item.Image = i==0? Properties.Resources.ic_error_outline_orange_18dp : Properties.Resources.ic_check_blue_18dp;
                    item.ImageScaling = ToolStripItemImageScaling.None;
                }

                deviceIDItems.Add(item);
            }
            
            outputSelectSubmenu.DropDownItems.AddRange(deviceIDItems.ToArray());

            // Device Manager Menu -  Add/remove 2nd iPhone, add/remove Xbox controller
            ToolStripMenuItem inputSelectSubmenu = new ToolStripMenuItem(String.Format("Input devices"));
            inputSelectSubmenu.Image = Properties.Resources.ic_input_white_18dp;
            inputSelectSubmenu.ImageScaling = ToolStripItemImageScaling.None;

            ToolStripMenuItem primaryMobileDeviceItem = new ToolStripMenuItem("Primary mobile device");
            primaryMobileDeviceItem.Image = Properties.Resources.ic_check_blue_18dp;
            primaryMobileDeviceItem.ImageScaling = ToolStripItemImageScaling.None;
            primaryMobileDeviceItem.Enabled = false;

            ToolStripMenuItem addRemoveMobileDeviceItem = ToolStripMenuItemWithHandler("Secondary mobile device", addRemoveMobileDevice_Click);
            addRemoveMobileDeviceItem.Image = DeviceNames.Count > 1 ? Properties.Resources.ic_check_blue_18dp : null;
            addRemoveMobileDeviceItem.ImageScaling = ToolStripItemImageScaling.None;
            addRemoveMobileDeviceItem.Enabled = true;

            ToolStripMenuItem addRemoveXboxControllerItem = ToolStripMenuItemWithHandler(AddRemoveXboxControllerString(), addRemoveXboxController_Click);
            if (SharedDeviceManager.InterceptXInputDevice) // xbox device currently active
            {
                addRemoveXboxControllerItem.Image = Properties.Resources.ic_check_blue_18dp;
                addRemoveXboxControllerItem.ImageScaling = ToolStripItemImageScaling.None;
            }
            else
            {
                addRemoveXboxControllerItem.Image = null;
                addRemoveXboxControllerItem.ImageScaling = ToolStripItemImageScaling.None;
            }

            inputSelectSubmenu.DropDownItems.AddRange(new ToolStripItem[] { primaryMobileDeviceItem, addRemoveMobileDeviceItem, addRemoveXboxControllerItem });


            ToolStripMenuItem unplugAllItem = ToolStripMenuItemWithHandler("Unplug all Xbox controllers", UnplugAll_Click);
            unplugAllItem.Image = Properties.Resources.ic_power_white_18dp;
            unplugAllItem.ImageScaling = ToolStripItemImageScaling.None;

            // Add all of these to the Context Menu Strip
            cms.Items.AddRange(
                new ToolStripItem[] {
                    outputSelectSubmenu,
                    inputSelectSubmenu,
                    modeSelectSubmenu,
                    unplugAllItem,
                    new ToolStripSeparator(),
                    ConfigureOutputSubmenu,
                });          
        }

        /// <summary>
        /// Get description from a decorated enum.
        /// </summary>
        /// <param name="value">Enum value</param>
        /// <returns>String of description</returns>
        public static string GetDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            DescriptionAttribute[] attributes =
                  (DescriptionAttribute[])fi.GetCustomAttributes(
                  typeof(DescriptionAttribute), false);
            return (attributes.Length > 0) ? attributes[0].Description : value.ToString();
        }

        /// <summary>
        /// Helper method to create a handler with a toolstrip menu item.
        /// </summary>
        /// <param name="displayText">Item's display text</param>
        /// <param name="eventHandler">Handler for Click event</param>
        /// <returns>ToolStripMenuItem</returns>
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
