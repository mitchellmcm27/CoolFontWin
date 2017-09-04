using System;
using System.Data;
using System.Reflection;

namespace PocketStrafe.VR
{
    public class IVRSystemInspector
    {
        public IVRSystemInspector()
        {
        }
     }

    public enum PStrafeHand
    {
        Left = 0,
        Right = 1,
    }
    
    public enum PStrafeButtonType
    {
        Press = 0,
        Touch = 1,
    }

    public enum OpenVRFunctionIndex
    {
        GetRecommendedRenderTargetSize,
        GetProjectionMatrix,
        GetProjectionRaw,
        ComputeDistortion,
        GetEyeToHeadTransform,
        GetTimeSinceLastVsync,
        GetD3D9AdapterIndex,
        GetDXGIOutputInfo,
        GetOutputDevice, // 1.06
        IsDisplayOnDesktop,
        SetDisplayVisibility,
        GetDeviceToAbsoluteTrackingPose,
        ResetSeatedZeroPose,
        GetSeatedZeroPoseToStandingAbsoluteTrackingPose,
        GetRawZeroPoseToStandingAbsoluteTrackingPose,
        GetSortedTrackedDeviceIndicesOfClass,
        GetTrackedDeviceActivityLevel,
        ApplyTransform,
        GetTrackedDeviceIndexForControllerRole,
        GetControllerRoleForTrackedDeviceIndex,
        GetTrackedDeviceClass,
        IsTrackedDeviceConnected,
        GetBoolTrackedDeviceProperty,
        GetFloatTrackedDeviceProperty,
        GetInt32TrackedDeviceProperty,
        GetUint64TrackedDeviceProperty,
        GetMatrix34TrackedDeviceProperty,
        GetStringTrackedDeviceProperty,
        GetPropErrorNameFromEnum,
        PollNextEvent,
        PollNextEventWithPose,
        GetEventTypeNameFromEnum,
        GetHiddenAreaMesh,
        GetControllerState,
        GetControllerStateWithPose,
        TriggerHapticPulse,
        GetButtonIdNameFromEnum,
        GetControllerAxisTypeNameFromEnum,
        CaptureInputFocus,
        ReleaseInputFocus,
        IsInputFocusCapturedByAnotherProcess,
        DriverDebugRequest,
        PerformFirmwareUpdate,
        AcknowledgeQuit_Exiting,
        AcknowledgeQuit_UserPrompt,

        Count,
    }
}
