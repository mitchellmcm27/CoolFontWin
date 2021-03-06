﻿using EasyHook;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Valve.VR;

namespace PocketStrafe.VR
{
    public class VRNotInitializedException : Exception
    {
        public VRNotInitializedException()
        {
        }

        public VRNotInitializedException(string message)
            : base(message)
        {
        }

        public VRNotInitializedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }

    public class OpenVRInject : EasyHook.IEntryPoint
    {
        private LocalHook GetControllerStateHook;
        private LocalHook GetControllerStateWithPoseHook;
        private LocalHook PollNextEventHook;
        private LocalHook PollNextEventWithPoseHook;

        private IntPtr GetControllerStatePtr;
        private IntPtr GetControllerStateWithPosePtr;
        private IntPtr PollNextEventPtr;
        private IntPtr PollNextEventWithPosePtr;

        private PSInterface Interface;
        private bool UserRunning;
        private uint LeftHandIndex;
        private uint RightHandIndex;
        private PStrafeButtonType ButtonType;
        private EVRButtonId RunButton;
        private PStrafeHand Hand;
        private uint ChosenDeviceIndex;

        public OpenVRInject(RemoteHooking.IContext context, String channelName)
        {
            // Get reference to IPC to host application
            // Note: any methods called or events triggered against _interface will execute in the host process.

            Interface = RemoteHooking.IpcConnectClient<PSInterface>(channelName);
            RunButton = Interface.RunButton;
            Hand = Interface.Hand;
            ButtonType = Interface.ButtonType;
            Interface.Write("Intialized");
        }

        /// <summary>
        /// Called by EasyHook to begin any hooking etc in the target process
        /// </summary>
        /// <param name="InContext"></param>
        /// <param name="InArg1"></param>
        public void Run(RemoteHooking.IContext InContext, String InArg1)
        {
            Interface.Write("Running in target.");
            int pid = RemoteHooking.GetCurrentProcessId();
            try
            {
                // NOTE: We are now already running within the target process

                // We want to hook each method of we are interested in
                IntPtr pGetControllerState = IntPtr.Zero;
                IntPtr pGetControllerStateWithPose = IntPtr.Zero;
                IntPtr pPollNextEvent = IntPtr.Zero;
                IntPtr pPollNextEventWithPose = IntPtr.Zero;

                // TODO: Find out version of openvr dll, which will determine the correct index of the function
                // ver = vr::IVRSystem_Version; returns string
                // string matching determines which enum to use

                if (RemoteHooking.IsX64Process(pid))
                {
                    /*
                    EVRInitError error = EVRInitError.None;
                    OpenVR.Init(ref error);
                    if(error == EVRInitError.None)
                    {
                        Func<in uint, VRControllerState_t, in uint, out bool> ptr = null;
                        ptr = OpenVR.System.GetControllerState;
                    }
                    */

                    Interface.Write("64 bit process");
                    pGetControllerState = GetIVRSystemFunctionAddress64((short)OpenVRFunctionIndex.GetControllerState, (int)OpenVRFunctionIndex.Count);
                    pGetControllerStateWithPose = GetIVRSystemFunctionAddress64((short)OpenVRFunctionIndex.GetControllerStateWithPose, (int)OpenVRFunctionIndex.Count);
                    pPollNextEvent = GetIVRSystemFunctionAddress64((short)OpenVRFunctionIndex.PollNextEvent, (int)OpenVRFunctionIndex.Count);
                    pPollNextEventWithPose = GetIVRSystemFunctionAddress64((short)OpenVRFunctionIndex.PollNextEventWithPose, (int)OpenVRFunctionIndex.Count);
                }
                else
                {
                    Interface.Write("32 bit process");
                    pGetControllerState = GetIVRSystemFunctionAddress32((short)OpenVRFunctionIndex.GetControllerState, (int)OpenVRFunctionIndex.Count);
                    pGetControllerStateWithPose = GetIVRSystemFunctionAddress32((short)OpenVRFunctionIndex.GetControllerStateWithPose, (int)OpenVRFunctionIndex.Count);
                    pPollNextEvent = GetIVRSystemFunctionAddress32((short)OpenVRFunctionIndex.PollNextEvent, (int)OpenVRFunctionIndex.Count);
                    pPollNextEventWithPose = GetIVRSystemFunctionAddress32((short)OpenVRFunctionIndex.PollNextEventWithPose, (int)OpenVRFunctionIndex.Count);
                }

                if ((pGetControllerState == IntPtr.Zero && pGetControllerStateWithPose == IntPtr.Zero)
                 || (pPollNextEvent == IntPtr.Zero || pPollNextEventWithPose == IntPtr.Zero))
                {
                    throw new VRNotInitializedException("No runtime installed, no HMD present, version mismatch, or other error.");
                }

                Interface.Write("GetControllerState function pointer: " + (pGetControllerState).ToString());
                Interface.Write("GetControllerStateWithPose function pointer: " + (pGetControllerStateWithPose).ToString());
                Interface.Write("PollNextEventEvent function pointer: " + (pPollNextEvent).ToString());
                Interface.Write("PollNextEventWithPose function pointer: " + (pPollNextEventWithPose).ToString());

                GetControllerStatePtr = pGetControllerState;
                GetControllerStateHook = LocalHook.Create(
                    pGetControllerState,
                    new vr_GetControllerStateDelegate(GetControllerState_Hooked),
                    this);

                GetControllerStateWithPosePtr = pGetControllerStateWithPose;
                GetControllerStateWithPoseHook = LocalHook.Create(
                    pGetControllerStateWithPose,
                    new vr_GetControllerStateWithPoseDelegate(GetControllerStateWithPose_Hooked),
                    this);

                PollNextEventPtr = pPollNextEvent;
                PollNextEventHook = LocalHook.Create(
                    pPollNextEvent,
                    new vr_PollNextEventDelegate(PollNextEvent_Hooked),
                    this);

                PollNextEventWithPosePtr = pPollNextEventWithPose;
                PollNextEventWithPoseHook = LocalHook.Create(
                    pPollNextEventWithPose,
                    new vr_PollNextEventWithPoseDelegate(PollNextEventWithPose_Hooked),
                    this);

                /*
                 * Don't forget that all hooks will start deactivated...
                 * The following ensures that all threads are intercepted:
                 * Note: you must do this for each hook.
                 */
                GetControllerStateHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                GetControllerStateWithPoseHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                PollNextEventHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                PollNextEventWithPoseHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }
            catch (Exception e)
            {
                /*
                    We should notify our host process about this error...
                 */
                Interface.Write("Error: ");
                Interface.ReportError(pid, e);
                return;
            }

            Interface.Write("DLL installed");
            Interface.IsInstalled(pid);

            // Wait for host process termination...
            try
            {
                ChosenDeviceIndex = 0; // base station
                while (ChosenDeviceIndex == 0)
                {
                    if (!Interface.Installed)
                    {
                        Interface.Write("Need to uninstall hooks!");
                        break;
                    }

                    // wait until controller comes on?
                    if (RemoteHooking.IsX64Process(pid))
                    {
                        LeftHandIndex = GetLeftHandIndex64();
                        RightHandIndex = GetRightHandIndex64();
                    }
                    else
                    {
                        LeftHandIndex = GetLeftHandIndex32();
                        RightHandIndex = GetRightHandIndex32();
                    }
                    Interface.Write("Left hand " + LeftHandIndex);
                    Interface.Write("Right hand " + RightHandIndex);
                    ChosenDeviceIndex = Interface.Hand == PStrafeHand.Left ? LeftHandIndex : RightHandIndex;
                    Thread.Sleep(300);
                }

                while (true)
                {
                    Thread.Sleep(10);
                    if (!Interface.Installed)
                    {
                        break;
                    }
                    bool running = Interface.UserIsRunning;
                    if (running != UserRunning)
                    {
                        // look for interface changes (keybinding, user is running, etc)
                        RunButton = Interface.RunButton;
                        ButtonType = Interface.ButtonType;
                        ChosenDeviceIndex = Interface.Hand == PStrafeHand.Left ? LeftHandIndex : RightHandIndex;
                        UserRunning = running;

                        // create event for vive controller
                        MyEvent = new ButtonEvent()
                        {
                            Queued = true,
                            ShouldPress = ButtonType == PStrafeButtonType.Press,
                            ShouldTouch = true
                        };
                    }
                }
            }
            catch
            {
                // NET Remoting will raise an exception if host is unreachable
                Interface.ReportError(pid, new Exception("Host unreachable?"));
            }
            finally
            {
                // Note: this will probably not get called if the target application closes before the
                //       host application.
                Interface.Write("Remove and cleanup hooks");
                Cleanup();
            }
        }

        public void Cleanup()
        {
            // Remove hooks
            GetControllerStateHook.Dispose();
            GetControllerStateWithPoseHook.Dispose();
            PollNextEventHook.Dispose();
            PollNextEventWithPoseHook.Dispose();

            // Finalise cleanup of hooks
            LocalHook.Release();
        }

        private void SetControllerState(IntPtr pControllerState, uint unControllerDeviceIndex)
        {
            VRControllerState_t cs = (VRControllerState_t)Marshal.PtrToStructure(pControllerState, typeof(VRControllerState_t));
            
                //Interface.Write("GetControllerState");
                //Interface.Write("  Device index " + (int)unControllerDeviceIndex);
                //Interface.Write("    Packet num " + cs.unPacketNum);
                //Interface.Write("    Buttons pressed " + cs.ulButtonPressed);
                //Interface.Write("    Buttons touched " + cs.ulButtonTouched);
                //Interface.Write("    Axis 0 " + cs.rAxis0.x + ", " + cs.rAxis0.y);
                //Interface.Write("    Axis 1 " + cs.rAxis1.x + ", " + cs.rAxis1.y);
                //Interface.Write("    Axis 2 " + cs.rAxis2.x + ", " + cs.rAxis2.y);
                //Interface.Write("    Axis 3 " + cs.rAxis3.x + ", " + cs.rAxis3.y);
               // Interface.Write("    Axis 4 " + cs.rAxis4.x + ", " + cs.rAxis4.y);
              
            if (UserRunning && unControllerDeviceIndex == ChosenDeviceIndex)
            {
                // Interface.Write("Running");
                if (ButtonType == PStrafeButtonType.Press)
                {
                    cs.ulButtonPressed = cs.ulButtonPressed | (1UL << ((int)RunButton));
                }

                cs.ulButtonTouched = cs.ulButtonTouched | (1UL << ((int)RunButton));

                if (RunButton == EVRButtonId.k_EButton_SteamVR_Touchpad || RunButton == EVRButtonId.k_EButton_Axis0)
                {
                    // -1 to 1
                    cs.rAxis0.y = 1.0f;
                }
                else if (RunButton == EVRButtonId.k_EButton_Axis1)
                {
                    // 0 to 1
                    cs.rAxis1.x = 1.0f;
                }
                Marshal.StructureToPtr(cs, pControllerState, true);
            }
            else
            {
            }
            
           
        }

        public class ButtonEvent
        {
            public bool Queued = false;
            public bool ShouldTouch = true;
            public bool TouchDone = false;
            public bool ShouldPress = true;
            public bool PressDone = false;
        }

        public ButtonEvent MyEvent = new ButtonEvent();

        private bool CreateEvent(IntPtr pEvent)
        {
            VREvent_t ev;
            ev = (VREvent_t)Marshal.PtrToStructure(pEvent, typeof(VREvent_t));
            // First, touch the trackpad, then fully press it on the next event. This is reversed on Untouch/Unpress
            if (MyEvent.ShouldTouch && !MyEvent.TouchDone)
            {
                ev.eventType = UserRunning ? (uint)EVREventType.VREvent_ButtonTouch : (uint)EVREventType.VREvent_ButtonUnpress;
                MyEvent.TouchDone = true;
                if (!MyEvent.ShouldPress) MyEvent.Queued = false;
            }
            else if (MyEvent.ShouldPress && !MyEvent.PressDone)
            {
                ev.eventType = UserRunning ? (uint)EVREventType.VREvent_ButtonPress : (uint)EVREventType.VREvent_ButtonUntouch;
                MyEvent.PressDone = true;
                MyEvent.Queued = false;
            }
            ev.eventAgeSeconds = 0;
            ev.trackedDeviceIndex = ChosenDeviceIndex;
            ev.data.controller.button = (uint)RunButton;
            Marshal.StructureToPtr(ev, pEvent, true);
            return true;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool vr_GetControllerStateDelegate(IntPtr instance, uint unControllerDeviceIndex, IntPtr pControllerState, uint unControllerStateSize);

        private static bool GetControllerState_Hooked(IntPtr instance, uint unControllerDeviceIndex, IntPtr pControllerState, uint unControllerStateSize)
        {
            bool res = false;
            OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;

            try
            {
                var getControllerState = Marshal.GetDelegateForFunctionPointer<vr_GetControllerStateDelegate>(This.GetControllerStatePtr);
                res = getControllerState(instance, unControllerDeviceIndex, pControllerState, unControllerStateSize);
                This.SetControllerState(pControllerState, unControllerDeviceIndex);       
            }
            catch
            {
            }
            return res;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool vr_GetControllerStateWithPoseDelegate(IntPtr instance, ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, IntPtr pControllerState, uint unControllerStateSize, IntPtr pTrackedDevicePose);

        private static bool GetControllerStateWithPose_Hooked(IntPtr instance, ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, IntPtr pControllerState, uint unControllerStateSize, IntPtr pTrackedDevicePose)
        {
            bool res = false;
            OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;
  
            try
            {
                var getControllerStateWithPose = Marshal.GetDelegateForFunctionPointer<vr_GetControllerStateWithPoseDelegate>(This.GetControllerStateWithPosePtr);
                res = getControllerStateWithPose(instance, eOrigin, unControllerDeviceIndex, pControllerState, unControllerStateSize, pTrackedDevicePose);
                This.SetControllerState( pControllerState, unControllerDeviceIndex);
            }
            catch
            {
            }
            return res;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool vr_PollNextEventDelegate(IntPtr instance, IntPtr pEvent, uint uncbVREvent);

        private static bool PollNextEvent_Hooked(IntPtr instance, IntPtr pEvent, uint uncbVREvent)
        {
            OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;
            try
            {
                var pollNextEvent = Marshal.GetDelegateForFunctionPointer<vr_PollNextEventDelegate>(This.PollNextEventPtr);
                bool res = pollNextEvent(instance, pEvent, uncbVREvent); // CRASH on 32 bit
                if (res)
                {
                    //This.Interface.Write("Event type: " + (EVREventType)pEvent.eventType);
                    //This.Interface.Write("Device index: " + pEvent.trackedDeviceIndex);
                    return true;
                }
                else if (This.MyEvent.Queued)
                {
                    res = This.CreateEvent(pEvent);
                    //This.Interface.Write("Event type: " + (EVREventType)pEvent.eventType);
                    //This.Interface.Write("Device index: " + pEvent.trackedDeviceIndex);
                    return true;
                }
            }
            catch (Exception ex)
            {
                // This.Interface.Write("Error in PollNextEvent_Hooked " + ex);
            }
            return false;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        private delegate bool vr_PollNextEventWithPoseDelegate(IntPtr instance, ETrackingUniverseOrigin eOrigin, IntPtr pEvent, uint uncbVREvent, IntPtr pTrackedDevicePose);

        private static bool PollNextEventWithPose_Hooked(IntPtr instance, ETrackingUniverseOrigin eOrigin, IntPtr pEvent, uint uncbVREvent, IntPtr pTrackedDevicePose)
        {
            OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;
            bool res = false;
            try
            {
                var pollNextEventWithPose = Marshal.GetDelegateForFunctionPointer<vr_PollNextEventWithPoseDelegate>(This.PollNextEventPtr);
                res = pollNextEventWithPose(instance, eOrigin, pEvent, uncbVREvent, pTrackedDevicePose);
                if (res)
                {
                    //This.Interface.Write("Event type: " + (EVREventType)pEvent.eventType);
                    //This.Interface.Write("Device index: " + pEvent.trackedDeviceIndex);
                    return true;
                }
                else if (This.MyEvent.Queued)
                {
                    res = This.CreateEvent(pEvent);
                    //This.Interface.Write("Event type: " + (EVREventType)pEvent.eventType);
                    //This.Interface.Write("Device index: " + pEvent.trackedDeviceIndex);
                    return true;
                }
            }
            catch
            {
            }
            return res;
        }

        /// <summary>
        /// The helper export from the c++ helper dll that will retrieve a function address for the provided index of the IVRSystem interface
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [DllImport("OpenVRHelper32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetIVRSystemFunctionAddress")]
        private static extern IntPtr GetIVRSystemFunctionAddress32(short methodIndex, int methodCount);

        [DllImport("OpenVRHelper32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetLeftHandIndex")]
        private static extern uint GetLeftHandIndex32();

        [DllImport("OpenVRHelper32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRightHandIndex")]
        private static extern uint GetRightHandIndex32();

        /// <summary>
        /// The helper export from the c++ helper dll that will retrieve a function address for the provided index of the IVRSystem interface
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [DllImport("OpenVRHelper64.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetIVRSystemFunctionAddress")]
        private static extern IntPtr GetIVRSystemFunctionAddress64(short methodIndex, int methodCount);

        [DllImport("OpenVRHelper64.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetLeftHandIndex")]
        private static extern uint GetLeftHandIndex64();

        [DllImport("OpenVRHelper64.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRightHandIndex")]
        private static extern uint GetRightHandIndex64();
    }
}