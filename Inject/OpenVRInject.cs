﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHook;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;
using Valve.VR;

namespace CFW.Business
{
    public class OpenVRInject : EasyHook.IEntryPoint
    {
        LocalHook GetControllerStateHook;
        LocalHook GetControllerStateWithPoseHook;
        LocalHook PollNextEventHook;

        PSInterface Interface;
        bool UserRunning;
        IntPtr GetControllerStatePtr;
        IntPtr GetControllerStateWithPosePtr;
        IntPtr PollNextEventPtr;

        uint LeftHandIndex;
        uint RightHandIndex;
        EVRButtonId RunButton;
        EVRHand Hand;
        uint ChosenDeviceIndex;  
    
        public OpenVRInject(RemoteHooking.IContext context, String channelName)
        {
            // Get reference to IPC to host application
            // Note: any methods called or events triggered against _interface will execute in the host process.

            Interface = RemoteHooking.IpcConnectClient<PSInterface>(channelName);
            RunButton = Interface.RunButton;
            Hand = Interface.Hand;
        }

        /// <summary>
        /// Called by EasyHook to begin any hooking etc in the target process
        /// </summary>
        /// <param name="InContext"></param>
        /// <param name="InArg1"></param>
        public void Run(RemoteHooking.IContext InContext, String InArg1)
        {
            int pid = RemoteHooking.GetCurrentProcessId();
            try
            {
                // NOTE: We are now already running within the target process
                
                // We want to hook each method of we are interested in  
                IntPtr pGetControllerState = IntPtr.Zero;
                IntPtr pGetControllerStateWithPose = IntPtr.Zero;
                IntPtr pPollNextEvent = IntPtr.Zero;

                if (RemoteHooking.IsX64Process(pid))
                {
                    Interface.Write("64 bit process");
                    pGetControllerState = GetIVRSystemFunctionAddress64((short)OpenVRFunctionIndex.GetControllerState, (int)OpenVRFunctionIndex.Count);
                    pGetControllerStateWithPose = GetIVRSystemFunctionAddress64((short)OpenVRFunctionIndex.GetControllerStateWithPose, (int)OpenVRFunctionIndex.Count);
                    pPollNextEvent = GetIVRSystemFunctionAddress64((short)OpenVRFunctionIndex.PollNextEvent, (int)OpenVRFunctionIndex.Count);
                }
                else
                {
                    Interface.Write("32 bit process");
                    pGetControllerState = GetIVRSystemFunctionAddress32((short)OpenVRFunctionIndex.GetControllerState, (int)OpenVRFunctionIndex.Count);
                    pGetControllerStateWithPose = GetIVRSystemFunctionAddress32((short)OpenVRFunctionIndex.GetControllerStateWithPose, (int)OpenVRFunctionIndex.Count);
                    pPollNextEvent = GetIVRSystemFunctionAddress32((short)OpenVRFunctionIndex.PollNextEvent, (int)OpenVRFunctionIndex.Count);
                }

                Interface.Write("GetControllerState function pointer: " + (pGetControllerState).ToString());
                Interface.Write("GetControllerStateWithPose function pointer: " + (pGetControllerStateWithPose).ToString());
                Interface.Write("PollEvent function pointer: " + (pPollNextEvent).ToString());

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

                /*
                 * Don't forget that all hooks will start deactivated...
                 * The following ensures that all threads are intercepted:
                 * Note: you must do this for each hook.
                 */
                GetControllerStateHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                GetControllerStateWithPoseHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
                PollNextEventHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }

            catch (Exception e)
            {
                /*
                    We should notify our host process about this error...
                 */
                Interface.ReportError(pid, e);
                return;
            }

            Interface.IsInstalled(pid);

            // Wait for host process termination...
            try
            {
                ChosenDeviceIndex = 0;
                while (ChosenDeviceIndex == 0)
                {
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
                    ChosenDeviceIndex = Interface.Hand == EVRHand.Left ? LeftHandIndex : RightHandIndex;
                    Thread.Sleep(300);
                }

                while (true)
                {
                    Thread.Sleep(10);
                    bool running = Interface.UserIsRunning;
                    if (running != UserRunning)
                    {
                        RunButton = Interface.RunButton;
                        ChosenDeviceIndex = Interface.Hand == EVRHand.Left ? LeftHandIndex : RightHandIndex;

                        UserRunning = running;
                        MyEvent = new ButtonEvent();
                        MyEvent.Queued = true;
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
                //       host application. One solution would be to create and dispose the Surface for 
                //       each capture request.
                Interface.Write("Cleanup Hook");
                Cleanup();
            }
        }

        void SetControllerState(ref VRControllerState_t pControllerState, uint unControllerDeviceIndex)
        {
            if (UserRunning && unControllerDeviceIndex==ChosenDeviceIndex)
            {
                pControllerState.ulButtonPressed = pControllerState.ulButtonPressed | (1UL << ((int)RunButton));
                pControllerState.ulButtonTouched = pControllerState.ulButtonTouched | (1UL << ((int)RunButton));
                pControllerState.rAxis0.y = 0.7f;
                // Interface.Write("Touch");
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

        private bool CreateEvent(ref VREvent_t pEvent)
        {
            // First, touch the trackpad, then fully press it on the next event. This is reversed on Untouch/Unpress
            if (MyEvent.ShouldTouch && !MyEvent.TouchDone)
            {
                pEvent.eventType = UserRunning ? (uint)EVREventType.VREvent_ButtonTouch : (uint)EVREventType.VREvent_ButtonUnpress;
                MyEvent.TouchDone = true;
                if (!MyEvent.ShouldPress) MyEvent.Queued = false;
            }
            else if (MyEvent.ShouldPress && !MyEvent.PressDone)
            {
                pEvent.eventType = UserRunning ? (uint)EVREventType.VREvent_ButtonPress : (uint)EVREventType.VREvent_ButtonUntouch;
                MyEvent.PressDone = true;
                MyEvent.Queued = false;
            }
            pEvent.eventAgeSeconds = 0;
            pEvent.trackedDeviceIndex = 3;
            pEvent.data.controller.button = (uint)EVRButtonId.k_EButton_Axis0;
            return true;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate bool vr_GetControllerStateDelegate(IntPtr instance, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize);

        static bool GetControllerState_Hooked(IntPtr instance, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize)
        {
            bool res = false;
            OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;
            /*
            This.Interface.Write("GetControllerState");
            This.Interface.Write("  Device index " + (int)unControllerDeviceIndex);
            This.Interface.Write("  Controller state size " + (int)unControllerStateSize);
            This.Interface.Write("    Packet num " + (int)pControllerState.unPacketNum);
            This.Interface.Write("    Buttons pressed " + pControllerState.ulButtonPressed);
            This.Interface.Write("    Buttons touched " + pControllerState.ulButtonTouched);
            This.Interface.Write("    Axis 0 " + pControllerState.rAxis0.x + ", " + pControllerState.rAxis0.y);
            This.Interface.Write("    Axis 1 " + pControllerState.rAxis1.x + ", " + pControllerState.rAxis1.y);
            This.Interface.Write("    Axis 2 " + pControllerState.rAxis2.x + ", " + pControllerState.rAxis2.y);
            This.Interface.Write("    Axis 3 " + pControllerState.rAxis3.x + ", " + pControllerState.rAxis3.y);
            This.Interface.Write("    Axis 4 " + pControllerState.rAxis4.x + ", " + pControllerState.rAxis4.y);
            */
            try
            {
                var getControllerState = Marshal.GetDelegateForFunctionPointer<vr_GetControllerStateDelegate>(This.GetControllerStatePtr);
                res = getControllerState(instance, unControllerDeviceIndex, ref pControllerState, unControllerStateSize);
                This.SetControllerState(ref pControllerState, unControllerDeviceIndex);
            }
            catch
            {
            }
            return res;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate bool vr_GetControllerStateWithPoseDelegate(IntPtr instance, ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize, ref TrackedDevicePose_t pTrackedDevicePose);

        static bool GetControllerStateWithPose_Hooked(IntPtr instance, ETrackingUniverseOrigin eOrigin, uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize, ref TrackedDevicePose_t pTrackedDevicePose)
        {
            bool res = false;
            OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;
            /*
            This.Interface.Write("GetControllerStateWithPose");
            This.Interface.Write("  Device index " + unControllerDeviceIndex);
            This.Interface.Write("  Controller state size " + unControllerStateSize);
            This.Interface.Write("    Packet num " + pControllerState.unPacketNum);
            This.Interface.Write("    Buttons pressed " + pControllerState.ulButtonPressed);
            This.Interface.Write("    Buttons touched " + pControllerState.ulButtonTouched);
            This.Interface.Write("    Axis 0 " + pControllerState.rAxis0.x + ", " + pControllerState.rAxis0.y);
            This.Interface.Write("    Axis 1 " + pControllerState.rAxis1.x + ", " + pControllerState.rAxis1.y);
            This.Interface.Write("    Axis 2 " + pControllerState.rAxis2.x + ", " + pControllerState.rAxis2.y);
            This.Interface.Write("    Axis 3 " + pControllerState.rAxis3.x + ", " + pControllerState.rAxis3.y);
            This.Interface.Write("    Axis 4 " + pControllerState.rAxis4.x + ", " + pControllerState.rAxis4.y);
            */
            try
            {
                var getControllerStateWithPose = Marshal.GetDelegateForFunctionPointer<vr_GetControllerStateWithPoseDelegate>(This.GetControllerStateWithPosePtr);
                res = getControllerStateWithPose(instance, eOrigin, unControllerDeviceIndex, ref pControllerState, unControllerStateSize, ref pTrackedDevicePose);
                This.SetControllerState(ref pControllerState, unControllerDeviceIndex);
            }
            catch
            {
            }
            return res;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate bool vr_PollNextEventDelegate(IntPtr instance, ref VREvent_t pEvent, uint uncbVREvent);

        static bool PollNextEvent_Hooked(IntPtr instance, ref VREvent_t pEvent, uint uncbVREvent)
        {
            OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;
            try
            {
                var pollNextEvent = Marshal.GetDelegateForFunctionPointer<vr_PollNextEventDelegate>(This.PollNextEventPtr);
                bool res = pollNextEvent(instance, ref pEvent, uncbVREvent);
                if (res)
                {
                    // This.Interface.Write("Event type: " + (EVREventType)pEvent.eventType);
                    // This.Interface.Write("Device index: " + pEvent.trackedDeviceIndex);
                    return true;
                }
                else if(This.MyEvent.Queued)
                {
                    res = This.CreateEvent(ref pEvent);
                    // This.Interface.Write("Event type: " + (EVREventType)pEvent.eventType);
                    // This.Interface.Write("Device index: " + pEvent.trackedDeviceIndex);
                    return true;
                }
            }
            catch (Exception ex)
            {
                This.Interface.Write("Error in PollNextEvent_Hooked " + ex);
            }
            return false;
        }

        /// <summary>
        /// Just ensures that the surface we created it cleaned up.
        /// </summary>
        private void Cleanup()
        {
        }

        /// <summary>
        /// The helper export from the c++ helper dll that will retrieve a function address for the provided index of the IVRSystem interface
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [DllImport("OpenVRHelper32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetIVRSystemFunctionAddress")]
        static extern IntPtr GetIVRSystemFunctionAddress32(short methodIndex, int methodCount);

        [DllImport("OpenVRHelper32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetLeftHandIndex")]
        static extern uint GetLeftHandIndex32();

        [DllImport("OpenVRHelper32.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRightHandIndex")]
        static extern uint GetRightHandIndex32();

        /// <summary>
        /// The helper export from the c++ helper dll that will retrieve a function address for the provided index of the IVRSystem interface
        /// </summary>
        /// <param name="methodIndex"></param>
        /// <returns></returns>
        [DllImport("OpenVRHelper64.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetIVRSystemFunctionAddress")]
        static extern IntPtr GetIVRSystemFunctionAddress64(short methodIndex, int methodCount);

        [DllImport("OpenVRHelper64.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetLeftHandIndex")]
        static extern uint GetLeftHandIndex64();

        [DllImport("OpenVRHelper64.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall, EntryPoint = "GetRightHandIndex")]
        static extern uint GetRightHandIndex64();
    }
}