using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyHook;
using System.Runtime.InteropServices;
using System.IO;
using System.Threading;

namespace CFW.Business
{
    public class OpenVRInject : EasyHook.IEntryPoint
    {
        LocalHook GetControllerStateHook;
        Main Interface;
        bool UserRunning;
        public OpenVRInject(RemoteHooking.IContext context, String channelName)
        {
            // Get reference to IPC to host application
            // Note: any methods called or events triggered against _interface will execute in the host process.

            Interface = RemoteHooking.IpcConnectClient<Main>(channelName);
            UserRunning = false;
        }

        /// <summary>
        /// Called by EasyHook to begin any hooking etc in the target process
        /// </summary>
        /// <param name="InContext"></param>
        /// <param name="InArg1"></param>
        public void Run(RemoteHooking.IContext InContext, String InArg1)
        {
            try
            {
                // NOTE: We are now already running within the target process

                // We want to hook each method of we are interested in
                IntPtr pGetControllerState = GetOpenVRFunctionAddress((short)19);
                GetControllerStateHook = LocalHook.Create(
                    pGetControllerState,
                    new vr_GetControllerStateDelegate(GetControllerState_Hooked),
                    this);

                /*
                 * Don't forget that all hooks will start deactivated...
                 * The following ensures that all threads are intercepted:
                 * Note: you must do this for each hook.
                 */
                GetControllerStateHook.ThreadACL.SetExclusiveACL(new Int32[] { 0 });
            }

            catch (Exception e)
            {
                /*
                    We should notify our host process about this error...
                 */
                Interface.ReportError(RemoteHooking.GetCurrentProcessId(), e);
                return;
            }

            Interface.IsInstalled(RemoteHooking.GetCurrentProcessId());

            // Wait for host process termination...
            try
            {
                while (true)
                {
                    Thread.Sleep(10);
                    UserRunning = Interface.GetUserRunning();
                }
            }
            catch
            {
                // NET Remoting will raise an exception if host is unreachable
            }
            finally
            {
                // Note: this will probably not get called if the target application closes before the 
                //       host application. One solution would be to create and dispose the Surface for 
                //       each capture request.
                Cleanup();
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VRControllerAxis_t
        {
            public float x;
            public float y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct VRControllerState_t
        {
            public uint unPacketNum;
            public ulong ulButtonPressed;
            public ulong ulButtonTouched;
            public VRControllerAxis_t rAxis0; //VRControllerAxis_t[5]
            public VRControllerAxis_t rAxis1;
            public VRControllerAxis_t rAxis2;
            public VRControllerAxis_t rAxis3;
            public VRControllerAxis_t rAxis4;
        }

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        delegate bool vr_GetControllerStateDelegate(uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize);

        static bool GetControllerState_Hooked(uint unControllerDeviceIndex, ref VRControllerState_t pControllerState, uint unControllerStateSize)
        {
            try
            {
                OpenVRInject This = (OpenVRInject)HookRuntimeInfo.Callback;
                This.Interface.Write("Hooked!");
                return false;
            }
            catch
            {
                return false;
            }
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
        [DllImport("OpenVRHelper.dll", CharSet = CharSet.Unicode, SetLastError = true, CallingConvention = CallingConvention.StdCall)]
        static extern IntPtr GetOpenVRFunctionAddress(short methodIndex);
    }
}
