﻿using System;
using SharpDX.XInput;
using log4net;

namespace CFW.Business
{

    class XInputDeviceManager
    {
        private static readonly ILog log =
            LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Controller Controller { get; set; }
        private Controller[] controllers;

        public XInputDeviceManager()
        {
            log.Info("Initializing xinput Controller.");
            // Initialize XInput
            Controller = null;
            controllers = new[] {
                new Controller(UserIndex.One),
                new Controller(UserIndex.Two),
                new Controller(UserIndex.Three),
                new Controller(UserIndex.Four),
            };
            log.Info("Controller array initialized.");
        }

        public Controller getController ()
        {
            foreach (Controller selectController in controllers)
            {
                if (selectController.IsConnected)
                {
                    Controller = selectController;
                    log.Info("Found Controller.");
                    break;
                }
            }

            if (Controller == null)
            {
                log.Info("No XInput controller connected");
            }
            else
            {
                log.Info("Found XInput controller available");
                try
                {
                    log.Info("Will try to get controller capabilities:");
                    log.Info(Controller.GetCapabilities(DeviceQueryType.Any));
                }
                catch (SharpDX.SharpDXException ex)
                {
                    log.Info("Getting capabilities failed: " + ex.Message);
                    log.Info("Returning null");
                    Controller = null;
                }
            }
            return Controller;
        }
    }
}
