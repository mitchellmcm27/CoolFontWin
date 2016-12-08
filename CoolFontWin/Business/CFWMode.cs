using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CFW.Business
{
    public enum SimulatorMode
    {
        // Controls how the character moves in-game
        [Description("Pause")]
        ModePaused = 0,

        [Description("Keyboard")]
        ModeWASD, // Use KB to run forward, mouse to turn

        [Description("Coupled gamepad")]
        ModeJoystickCoupled, // Use vJoy/XOutput to move character through game (strafe only, no turning). VR MODE.

        [Description("Decoupled gamepad")]
        ModeJoystickDecoupled, // phone direction decides which direction the character strafes (no turning)

        [Description("Gamepad+Mouse")]
        ModeJoystickTurn, //TODO: Move character forward and turn L/R using joystick. Difficult.

        [Description("Mouse")]
        ModeMouse, // tilt the phone L/R U/D to move the mouse pointer

        [Description("Gamepad")]
        ModeGamepad, // fully functional gamepad similar to Xbox controller

        ModeDefault = ModeWASD,
    };

    public static class CFWMode
    {

#if DEBUG
        private static readonly int ModeCount = 7;
#else
        private static readonly int ModeCount = 4;
#endif

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

        public static List<string> GetDescriptions()
        {
            var list = new List<string>();
            for (int i=0; i < ModeCount; i ++)
            {
                list.Add(GetDescription((SimulatorMode)i));
            }
            return list;
        }
    }
}
