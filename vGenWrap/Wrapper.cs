using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

public enum HID_USAGES
{
    HID_USAGE_X = 0x30,
    HID_USAGE_Y = 0x31,
    HID_USAGE_Z = 0x32,
    HID_USAGE_RX = 0x33,
    HID_USAGE_RY = 0x34,
    HID_USAGE_RZ = 0x35,
    HID_USAGE_SL0 = 0x36,
    HID_USAGE_SL1 = 0x37,
    HID_USAGE_WHL = 0x38,
    HID_USAGE_POV = 0x39,
}

public enum VjdStat  /* Declares an enumeration data type called BOOLEAN */
{
    VJD_STAT_OWN,	// The  vJoy Device is owned by this application.
    VJD_STAT_FREE,	// The  vJoy Device is NOT owned by any application (including this one).
    VJD_STAT_BUSY,	// The  vJoy Device is owned by another application. It cannot be acquired by this application.
    VJD_STAT_MISS,	// The  vJoy Device is missing. It either does not exist or the driver is down.
    VJD_STAT_UNKN	// Unknown
};

public enum DevType { vJoy, vXbox };

namespace vGenWrap
{
    public class vDev
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct JoystickState
        {
            public byte bDevice;
            public Int32 Throttle;
            public Int32 Rudder;
            public Int32 Aileron;
            public Int32 AxisX;
            public Int32 AxisY;
            public Int32 AxisZ;
            public Int32 AxisXRot;
            public Int32 AxisYRot;
            public Int32 AxisZRot;
            public Int32 Slider;
            public Int32 Dial;
            public Int32 Wheel;
            public Int32 AxisVX;
            public Int32 AxisVY;
            public Int32 AxisVZ;
            public Int32 AxisVBRX;
            public Int32 AxisVBRY;
            public Int32 AxisVBRZ;
            public UInt32 Buttons;
            public UInt32 bHats;	// Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
            public UInt32 bHatsEx1;	// Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
            public UInt32 bHatsEx2;	// Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
            public UInt32 bHatsEx3;	// Lower 4 bits: HAT switch or 16-bit of continuous HAT switch
            public UInt32 ButtonsEx1;
            public UInt32 ButtonsEx2;
            public UInt32 ButtonsEx3;
        };

        // Import from vGenInterface.dll (C)
        //////////////////////////////////////////////////////////////////////////////////////
        // Version
        [DllImport("vGenInterface.dll", EntryPoint = "GetvJoyVersion")]
        private static extern short _GetvJoyVersion();

        [DllImport("vGenInterface.dll", EntryPoint = "vJoyEnabled")]
        private static extern bool _vJoyEnabled();

        /////	vJoy/vXbox Device properties
        [DllImport("vGenInterface.dll", EntryPoint = "GetVJDButtonNumber")]
        private static extern int _GetVJDButtonNumber(UInt32 rID);    // Get the number of buttons defined in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "GetVJDDiscPovNumber")]
        private static extern int _GetVJDDiscPovNumber(UInt32 rID);   // Get the number of descrete-type POV hats defined in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "GetVJDContPovNumber")]
        private static extern int _GetVJDContPovNumber(UInt32 rID);   // Get the number of descrete-type POV hats defined in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "GetVJDAxisExist")]
        private static extern int _GetVJDAxisExist(UInt32 rID, UInt32 Axis); // Test if given axis defined in the specified VDJ


        [DllImport("vGenInterface.dll", EntryPoint = "GetVJDAxisMax")]
        private static extern bool _GetVJDAxisMax(UInt32 rID, UInt32 Axis, ref long Max); // Get logical Maximum value for a given axis defined in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "GetVJDAxisMin")]
        private static extern bool _GetVJDAxisMin(UInt32 rID, UInt32 Axis, ref long Min); // Get logical Minimum value for a given axis defined in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "GetVJDStatus")]
        private static extern int _GetVJDStatus(UInt32 rID);         // Get the status of the specified vJoy Device.

        [DllImport("vGenInterface.dll", EntryPoint = "isVJDExists")]
        private static extern bool _isVJDExists(UInt32 rID);                  // TRUE if the specified vJoy Device exists

        /////	Write access to vJoy Device - Basic
        [DllImport("vGenInterface.dll", EntryPoint = "AcquireVJD")]
        private static extern bool _AcquireVJD(UInt32 rID);               // Acquire the specified vJoy Device.

        [DllImport("vGenInterface.dll", EntryPoint = "RelinquishVJD")]
        private static extern void _RelinquishVJD(UInt32 rID);            // Relinquish the specified vJoy Device.

        [DllImport("vGenInterface.dll", EntryPoint = "UpdateVJD")]
        private static extern bool _UpdateVJD(UInt32 rID, ref JoystickState pData);   // Update the position data of the specified vJoy Device.

        /////	Write access to vJoy Device - Modifyiers
        // This group of functions modify the current value of the position data
        // They replace the need to create a structure of position data then call UpdateVJD

        //// Device-Reset functions
        [DllImport("vGenInterface.dll", EntryPoint = "ResetVJD")]
        private static extern bool _ResetVJD(UInt32 rID);         // Reset all controls to predefined values in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "ResetAll")]
        private static extern void _ResetAll();             // Reset all controls to predefined values in all VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "ResetButtons")]
        private static extern bool _ResetButtons(UInt32 rID);     // Reset all buttons (To 0) in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "ResetPovs")]
        private static extern bool _ResetPovs(UInt32 rID);        // Reset all POV Switches (To -1) in the specified VDJ

        // Write data
        [DllImport("vGenInterface.dll", EntryPoint = "SetAxis")]
        private static extern bool _SetAxis(Int32 Value, UInt32 rID, UInt32 Axis);       // Write Value to a given axis defined in the specified VDJ 

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtn")]
        private static extern bool _SetBtn(bool Value, UInt32 rID, Byte nBtn);       // Write Value to a given button defined in the specified VDJ 

        [DllImport("vGenInterface.dll", EntryPoint = "SetDiscPov")]
        private static extern bool _SetDiscPov(Int32 Value, UInt32 rID, Byte nPov);    // Write Value to a given descrete POV defined in the specified VDJ

        [DllImport("vGenInterface.dll", EntryPoint = "SetContPov")]
        private static extern bool _SetContPov(Int32 Value, UInt32 rID, Byte nPov);  // Write Value to a given continuous POV defined in the specified VDJ 

        /*
        // FFB function
        VGENINTERFACE_API FFBEType  __cdecl FfbGetEffect(); // Returns effect serial number if active, 0 if inactive
        VGENINTERFACE_API VOID      __cdecl FfbRegisterGenCB(FfbGenCB cb, PVOID data);

    __declspec(deprecated("** FfbStart function was deprecated - you can remove it from your code **")) \
		VGENINTERFACE_API BOOL      __cdecl FfbStart(UINT rID);				  // Start the FFB queues of the specified vJoy Device.

    __declspec(deprecated("** FfbStop function was deprecated - you can remove it from your code **")) \
		VGENINTERFACE_API VOID      __cdecl FfbStop(UINT rID);                // Stop the FFB queues of the specified vJoy Device.

        // Added in 2.1.6
        VGENINTERFACE_API BOOL      __cdecl IsDeviceFfb(UINT rID);
        VGENINTERFACE_API BOOL      __cdecl IsDeviceFfbEffect(UINT rID, UINT Effect);

        //  Force Feedback (FFB) helper functions
        VGENINTERFACE_API DWORD     __cdecl Ffb_h_DeviceID(const FFB_DATA* Packet, int* DeviceID);
        VGENINTERFACE_API DWORD     __cdecl Ffb_h_Type(const FFB_DATA* Packet, FFBPType *Type);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_Packet(const FFB_DATA* Packet, WORD *Type, int* DataSize, BYTE *Data[]);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_EBI(const FFB_DATA* Packet, int* Index);
        VGENINTERFACE_API DWORD     __cdecl Ffb_h_Eff_Report(const FFB_DATA* Packet, FFB_EFF_REPORT*  Effect);
	//__declspec(deprecated("** Ffb_h_Eff_Const function was deprecated - Use function Ffb_h_Eff_Report **")) \
	//VGENINTERFACE_API DWORD 	__cdecl Ffb_h_Eff_Const(const FFB_DATA * Packet, FFB_EFF_REPORT*  Effect);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_Eff_Ramp(const FFB_DATA* Packet, FFB_EFF_RAMP*  RampEffect);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_EffOp(const FFB_DATA* Packet, FFB_EFF_OP*  Operation);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_DevCtrl(const FFB_DATA* Packet, FFB_CTRL *  Control);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_Eff_Period(const FFB_DATA* Packet, FFB_EFF_PERIOD*  Effect);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_Eff_Cond(const FFB_DATA* Packet, FFB_EFF_COND*  Condition);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_DevGain(const FFB_DATA* Packet, BYTE * Gain);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_Eff_Envlp(const FFB_DATA* Packet, FFB_EFF_ENVLP*  Envelope);
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_EffNew(const FFB_DATA* Packet, FFBEType * Effect);

	// Added in 2.1.6
	VGENINTERFACE_API DWORD     __cdecl Ffb_h_Eff_Constant(const FFB_DATA* Packet, FFB_EFF_CONSTANT *  ConstantEffect);

*/

        //////////////////////////////////////////////////////////////////////////////////////
        /// 
        ///  vXbox interface fuctions
        ///
        ///  Device range: 1-4 (Not necessarily related to Led number)
        ///
        //////////////////////////////////////////////////////////////////////////////////////

        // Virtual vXbox bus information
        [DllImport("vGenInterface.dll", EntryPoint = "isVBusExist")]
        private static extern Int32 _isVBusExist();

        [DllImport("vGenInterface.dll", EntryPoint = "GetNumEmptyBusSlots")]
        private static extern Int32 _GetNumEmptyBusSlots(ref byte nSlots);

        // Device Status (Plugin/Unplug and check ownership)
        [DllImport("vGenInterface.dll", EntryPoint = "isControllerPluggedIn")]
        private static extern UInt32 _isControllerPluggedIn(UInt32 UserIndex, ref bool Exist); // typedef BOOL near *PBOOL?

        [DllImport("vGenInterface.dll", EntryPoint = "isControllerOwned")]
        private static extern UInt32 _isControllerOwned(UInt32 UserIndex, ref bool Exist); // typedef BOOL near *PBOOL?

        [DllImport("vGenInterface.dll", EntryPoint = "PlugIn")]
        private static extern UInt32 _PlugIn(UInt32 UserIndex);

        [DllImport("vGenInterface.dll", EntryPoint = "PlugInNext")]
        private static extern UInt32 _PlugInNext(ref UInt32 UserIndex); // typdef unsinged int UINT pointer?

        [DllImport("vGenInterface.dll", EntryPoint = "UnPlug")]
        private static extern UInt32 _UnPlug(UInt32 UserIndex);

        [DllImport("vGenInterface.dll", EntryPoint = "UnPlogForce")]
        private static extern UInt32 _UnPlugForce(UInt32 UserIndex);

        // Reset Devices
        [DllImport("vGenInterface.dll", EntryPoint = "ResetController")]
        private static extern UInt32 _ResetController(UInt32 UserIndex);

        [DllImport("vGenInterface.dll", EntryPoint = "ResetAllControllers")]
        private static extern UInt32 _ResetAllControllers();

        [DllImport("vGenInterface.dll", EntryPoint = "ResetControllerBtns")]
        private static extern UInt32 _ResetControllerBtns(UInt32 UserIndex);

        [DllImport("vGenInterface.dll", EntryPoint = "ResetControllerDPad")]
        private static extern UInt32 _ResetControllerDPad(UInt32 UserIndex);

        // Button functions: Per-button Press/Release
        [DllImport("vGenInterface.dll", EntryPoint = "SetButton")]
        private static extern UInt32 _SetButton(UInt32 UserIndex, UInt16 Button, Boolean Press);

        // BOOL -> bool ... 
        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnA")]
        private static extern bool _SetBtnA(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnB")]
        private static extern bool _SetBtnB(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnX")]
        private static extern bool _SetBtnX(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnY")]
        private static extern bool _SetBtnY(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnLT")]
        private static extern bool _SetBtnLT(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnRT")]
        private static extern bool _SetBtnRT(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnLB")]
        private static extern bool _SetBtnLB(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnRB")]
        private static extern bool _SetBtnRB(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnStart")]
        private static extern bool _SetBtnStart(UInt32 UserIndex, bool Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetBtnBack")]
        private static extern bool _SetBtnBack(UInt32 UserIndex, bool Press);

        // Trigger/Axis functions: Set value in the range
        [DllImport("vGenInterface.dll", EntryPoint = "SetTriggerL")]
        private static extern UInt32 _SetTriggerL(UInt32 UserIndex, Byte Value);

        [DllImport("vGenInterface.dll", EntryPoint = "SetTriggerR")]
        private static extern UInt32 _SetTriggerR(UInt32 UserIndex, Byte Value);

        [DllImport("vGenInterface.dll", EntryPoint = "SetAxisLx")]
        private static extern UInt32 _SetAxisLx(UInt32 UserIndex, Byte Value); // Left Stick X

        [DllImport("vGenInterface.dll", EntryPoint = "SetAxisLy")]
        private static extern UInt32 _SetAxisLy(UInt32 UserIndex, Byte Value); // Left Stick Y

        [DllImport("vGenInterface.dll", EntryPoint = "SetAxisRx")]
        private static extern UInt32 _SetAxisRx(UInt32 UserIndex, Byte Value); // Right Stick X

        [DllImport("vGenInterface.dll", EntryPoint = "SetAxisRy")]
        private static extern UInt32 _SetAxisRy(UInt32 UserIndex, Byte Value); // Right Stick Y

        // DPAD Functions
        [DllImport("vGenInterface.dll", EntryPoint = "SetDpad")]
        private static extern UInt32 _SetDpad(UInt32 UserIndex, Byte Value); // DPAD Set Value, typedef unsigned char UCHAR -> Byte

        [DllImport("vGenInterface.dll", EntryPoint = "SetDpadUp")]
        private static extern bool _SetDpadUp(UInt32 UserIndex); // DPAD Up

        [DllImport("vGenInterface.dll", EntryPoint = "SetDpadRight")]
        private static extern bool _SetDpadRight(UInt32 UserIndex); // DPAD Right

        [DllImport("vGenInterface.dll", EntryPoint = "SetDpadDown")]
        private static extern bool _SetDpadDown(UInt32 UserIndex); // DPAD Down

        [DllImport("vGenInterface.dll", EntryPoint = "SetDpadLeft")]
        private static extern bool _SetDpadLeft(UInt32 UserIndex); // DPAD Left

        [DllImport("vGenInterface.dll", EntryPoint = "SetDpadOff")]
        private static extern bool _SetDpadOff(UInt32 UserIndex); // DPAD Off

        // Feedback Polling: Assigned Led number / Vibration values
        [DllImport("vGenInterface.dll", EntryPoint = "GetLedNumber")]
        private static extern UInt32 _GetLedNumber(UInt32 UserIndex, ref Byte pLed); // PBYTE -> ref Byte???

        /*
        VGENINTERFACE_API DWORD       __cdecl GetVibration(UINT UserIndex, PXINPUT_VIBRATION pVib);
        */

        // Common to vJoy and vXbox
        // Device Administration, Manipulation and Information
        [DllImport("vGenInterface.dll", EntryPoint = "AcquireDev")]
        private static extern UInt32 _AcquireDev(UInt32 DevId, DevType dType, ref int hDev);   // Acquire a Device.

        [DllImport("vGenInterface.dll", EntryPoint = "RelinquishDev")]
        private static extern UInt32 _RelinquishDev(Int32 hDev);            // Relinquish a Device.

        [DllImport("vGenInterface.dll", EntryPoint = "GetDevType")]
        private static extern UInt32 _GetDevType(Int32 hDev, ref DevType dType);   // Get device type (vJoy/vXbox)

        [DllImport("vGenInterface.dll", EntryPoint = "GetDevNumber")]
        private static extern UInt32 _GetDevNumber(Int32 hDev, ref UInt32 dNumber);  // If vJoy: Number=Id; If vXbox: Number=Led#

        [DllImport("vGenInterface.dll", EntryPoint = "GetDevId")]
        private static extern UInt32 _GetDevId(Int32 hDev, ref UInt32 dID);                  // Return Device ID to be used with vXbox API and Backward compatibility API

        [DllImport("vGenInterface.dll", EntryPoint = "isDevOwned")]
        private static extern UInt32 _isDevOwned(UInt32 DevId, DevType dType, ref Boolean Owned); // Is device plugged-in/Configured by this feeder

        [DllImport("vGenInterface.dll", EntryPoint = "isDevExist")]
        private static extern UInt32 _isDevExist(UInt32 DevId, DevType dType, ref Boolean Exist); // Is device plugged-in/Configured

        [DllImport("vGenInterface.dll", EntryPoint = "isDevFree")]
        private static extern UInt32 _isDevFree(UInt32 DevId, DevType dType, ref Boolean Free);   // Is device unplugged/Free

        [DllImport("vGenInterface.dll", EntryPoint = "GetDevHandle")]
        private static extern UInt32 _GetDevHandle(UInt32 DevId, DevType dType, ref Int32 hDev);// Return device handle from Device ID and Device type

        [DllImport("vGenInterface.dll", EntryPoint = "isAxisExist")]
        private static extern UInt32 _isAxisExist(Int32 hDev, UInt32 nAxis, ref Boolean Exist); // Does Axis exist. See above table

        [DllImport("vGenInterface.dll", EntryPoint = "GetDevButtonN")]
        private static extern UInt32 _GetDevButtonN(Int32 hDev, ref UInt32 nBtn);            // Get number of buttons in device

        [DllImport("vGenInterface.dll", EntryPoint = "GetDevHatN")]
        private static extern UInt32 _GetDevHatN(Int32 hDev, ref UInt32 nHat);               // Get number of Hat Switches in device

        // Position Setting
        [DllImport("vGenInterface.dll", EntryPoint = "SetDevButton")]
        private static extern UInt32 _SetDevButton(Int32 hDev, UInt32 Button, Boolean Press);

        [DllImport("vGenInterface.dll", EntryPoint = "SetDevAxis")]
        private static extern UInt32 _SetDevAxis(Int32 hDev, UInt32 Axis, Single Value);

        [DllImport("vGenInterface.dll", EntryPoint = "SetDevPov")]
        private static extern UInt32 _SetDevPov(Int32 hDev, UInt32 nPov, Single Value);


        /***************************************************/
        /********** Export functions (C#) ******************/
        /***************************************************/

        /////	General driver data
        public short GetvJoyVersion() { return _GetvJoyVersion(); }
        public bool vJoyEnabled() { return _vJoyEnabled(); }

        /////	vJoy Device properties
        public bool GetVJDAxisExist(UInt32 rID, HID_USAGES Axis)
        {
            int res = _GetVJDAxisExist(rID, (uint)Axis);
            if (res == 1)
                return true;
            else
                return false;
        }
        public bool GetVJDAxisMax(UInt32 rID, HID_USAGES Axis, ref long Max) { return _GetVJDAxisMax(rID, (uint)Axis, ref Max); }
        public bool GetVJDAxisMin(UInt32 rID, HID_USAGES Axis, ref long Min) { return _GetVJDAxisMin(rID, (uint)Axis, ref Min); }
        public int GetVJDButtonNumber(UInt32 rID) { return _GetVJDButtonNumber(rID); }
        public int GetVJDDiscPovNumber(UInt32 rID) { return _GetVJDDiscPovNumber(rID); }
        public int GetVJDContPovNumber(UInt32 rID) { return _GetVJDContPovNumber(rID); }
        public bool isVJDExists(UInt32 rID) { return _isVJDExists(rID); }

        /////	Write access to vJoy Device - Basic
        public bool AcquireVJD(UInt32 rID) { return _AcquireVJD(rID); }
        public void RelinquishVJD(uint rID) { _RelinquishVJD(rID); }
        public bool UpdateVJD(UInt32 rID, ref JoystickState pData) { return _UpdateVJD(rID, ref pData); }
        public VjdStat GetVJDStatus(UInt32 rID) { return (VjdStat)_GetVJDStatus(rID); }

        //// Reset functions
        public bool ResetVJD(UInt32 rID) { return _ResetVJD(rID); }
        public void ResetAll() { _ResetAll(); }
        public bool ResetButtons(UInt32 rID) { return _ResetButtons(rID); }
        public bool ResetPovs(UInt32 rID) { return _ResetPovs(rID); }

        ////// Write data
        public bool SetAxis(Int32 Value, UInt32 rID, HID_USAGES Axis) { return _SetAxis(Value, rID, (uint)Axis); }
        public bool SetBtn(bool Value, UInt32 rID, uint nBtn) { return _SetBtn(Value, rID, (Byte)nBtn); }
        public bool SetDiscPov(Int32 Value, UInt32 rID, byte nPov) { return _SetDiscPov(Value, rID, nPov); }
        public bool SetContPov(Int32 Value, UInt32 rID, byte nPov) { return _SetContPov(Value, rID, nPov); }

        ///// vXbox Device properties
        // Virtual vXbox bus information
        public bool isVBusExist() { return _isVBusExist() == 0 ? true : false; } 
        public int GetNumEmptyBusSlots(ref byte nSlots) { return _GetNumEmptyBusSlots(ref nSlots); }

        // Device Status (Plugin/Unplug and check ownership)
        public uint isControllerPluggedIn(UInt32 UserIndex, ref bool Exist) { return _isControllerPluggedIn(UserIndex, ref Exist); }
        public uint isControllerOwned(UInt32 UserIndex, ref bool Exist) { return _isControllerOwned(UserIndex, ref Exist); }
        public uint PlugIn(UInt32 UserIndex) { return _PlugIn(UserIndex); }
        public uint PlugInNext(ref UInt32 UserIndex) { return _PlugInNext(ref UserIndex); }
        public uint UnPlug(UInt32 UserIndex) { return _UnPlug(UserIndex); }
        public uint UnPlugForce(UInt32 UserIndex) { return _UnPlugForce(UserIndex); }

        // Reset Devices
        public uint ResetController(UInt32 UserIndex) { return _ResetController(UserIndex); }
        public uint ResetAllControllers() { return _ResetAllControllers(); }
        public uint ResetControllerBtns(UInt32 UserIndex) { return _ResetControllerBtns(UserIndex); }
        public uint ResetControllerDPad(UInt32 UserIndex) { return _ResetControllerDPad(UserIndex); }

        // Button functions: Per-button Press/Release
        public uint SetButton(UInt32 UserIndex, UInt16 Button, bool Press) { return _SetButton(UserIndex, Button, Press); }

        public bool SetBtnA(UInt32 UserIndex, bool Press) { return _SetBtnA(UserIndex, Press); }
        public bool SetBtnB(UInt32 UserIndex, bool Press) { return _SetBtnB(UserIndex, Press); }
        public bool SetBtnX(UInt32 UserIndex, bool Press) { return _SetBtnX(UserIndex, Press); }
        public bool SetBtnY(UInt32 UserIndex, bool Press) { return _SetBtnY(UserIndex, Press); }
        public bool SetBtnLT(UInt32 UserIndex, bool Press) { return _SetBtnLT(UserIndex, Press); }
        public bool SetBtnRT(UInt32 UserIndex, bool Press) { return _SetBtnRT(UserIndex, Press); }
        public bool SetBtnLB(UInt32 UserIndex, bool Press) { return _SetBtnLB(UserIndex, Press); }
        public bool SetBtnRB(UInt32 UserIndex, bool Press) { return _SetBtnRB(UserIndex, Press); }
        public bool SetBtnStart(UInt32 UserIndex, bool Press) { return _SetBtnStart(UserIndex, Press); }
        public bool SetBtnBack(UInt32 UserIndex, bool Press) { return _SetBtnBack(UserIndex, Press); }
        public uint SetTriggerL(UInt32 UserIndex, byte Value) { return _SetTriggerL(UserIndex, Value); }
        public uint SetTriggerR(UInt32 UserIndex, byte Value) { return _SetTriggerR(UserIndex, Value); }
        public uint SetAxisLx(UInt32 UserIndex, byte Value) { return _SetAxisLx(UserIndex, Value); }
        public uint SetAxisLy(UInt32 UserIndex, byte Value) { return _SetAxisLy(UserIndex, Value); }
        public uint SetAxisRx(UInt32 UserIndex, byte Value) { return _SetAxisRx(UserIndex, Value); }
        public uint SetAxisRy(UInt32 UserIndex, byte Value) { return _SetAxisRy(UserIndex, Value); }

        // DPAD Functions
        public uint SetDpad(UInt32 UserIndex, Byte Value) { return _SetDpad(UserIndex, Value); }
        public bool SetDpadUp(UInt32 UserIndex) { return _SetDpadUp(UserIndex); }
        public bool SetDpadRight(UInt32 UserIndex) { return _SetDpadRight(UserIndex); }
        public bool SetDpadDown(UInt32 UserIndex) { return _SetDpadDown(UserIndex); }
        public bool SetDpadLeft(UInt32 UserIndex) { return _SetDpadLeft(UserIndex); }
        public bool SetDpadOff(UInt32 UserIndex) { return _SetDpadOff(UserIndex); }

        // Feedback Polling: Assigned Led number / Vibration values
        public uint GetLedNumber(UInt32 UserIndex, ref Byte pLed) { return _GetLedNumber(UserIndex, ref pLed); }

        ///// Common to vJoy and vXbox
        // Device Administration, Manipulation and Information
        public uint AcquireDev(UInt32 DevId, DevType dType, ref int hDev) { return _AcquireDev(DevId, dType, ref hDev); }
        public uint RelinquishDev(Int32 hDev) { return _RelinquishDev(hDev); }
        public uint GetDevType(Int32 hDev, ref DevType dType) { return _GetDevType(hDev, ref dType); }
        public uint GetDevNumber(Int32 hDev, ref UInt32 dNumber) { return _GetDevNumber(hDev, ref dNumber); }
        public uint GetDevId(Int32 hDev, ref UInt32 dID) { return _GetDevId(hDev, ref dID); }
        public uint isDevOwned(UInt32 DevId, DevType dType, ref Boolean Owned) { return _isDevOwned(DevId, dType, ref Owned); }
        public uint isDevExist(UInt32 DevId, DevType dType, ref Boolean Exist) { return _isDevExist(DevId, dType, ref Exist); }
        public uint isDevFree(UInt32 DevId, DevType dType, ref Boolean Free) { return _isDevFree(DevId, dType, ref Free); }
        public uint GetDevHandle(UInt32 DevId, DevType dType, ref Int32 hDev) { return _GetDevHandle(DevId, dType, ref hDev); }
        public uint isAxisExist(Int32 hDev, UInt32 nAxis, ref Boolean Exist) { return _isAxisExist(hDev, nAxis, ref Exist); }
        public uint GetDevButtonN(Int32 hDev, ref UInt32 nBtn) { return _GetDevButtonN(hDev, ref nBtn); }
        public uint GetDevHatN(Int32 hDev, ref UInt32 nHat) { return _GetDevHatN(hDev, ref nHat); }

        // Position Setting
        public uint SetDevButton(Int32 hDev, UInt32 Button, Boolean Press) { return _SetDevButton(hDev, Button, Press); }
        public uint SetDevAxis(Int32 hDev, UInt32 Axis, Single Value) { return _SetDevAxis(hDev, Axis, Value); }
        public uint SetDevPov(Int32 hDev, UInt32 nPov, Single Value) { return _SetDevPov(hDev, nPov, Value); }
    }
}
