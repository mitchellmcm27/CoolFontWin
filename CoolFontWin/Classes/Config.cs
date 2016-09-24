using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoolFontWin
{
    public static class Config
    {
        static public UInt32 ID = 1;

        public enum MovementModes
        {
            // Controls how the character moves in-game
            Paused = 0, //TODO: Implement a "pause" button in iOS, useful for navigating menus
            KeyboardMouse, // Use KB to run forward, mouse to turn
            JoystickMove, // Use vJoy/XOutput to move character through game (strafe only, no turning). VR MODE.
            JoystickMoveAndLook, //TODO: Move character forward and turn L/R using joystick. Difficult.
            JoystickStrafe, // phone direction decides which direction the character strafes (no turning)
            Mouse2D, // tilt the phone L/R U/D to move the mouse pointer
            Gamepad, // fully functional gamepad similar to Xbox controller

        };

        static public MovementModes Mode { get; set; }

        static public string PORT_FILE = "../../../../last-port.txt";

        static public double THRESH_RUN = 0.7;
        static public double THRESH_WALK = 0.3;
        static public int mouseSens = 30;
        static public int socketPollInterval = 8*1000; // microseconds (us)
        static public double dt = socketPollInterval / 1000.0 / 1000.0; // s

        // assuming socketPollInterval = 8,000:
        // 0.05 good for mouse movement, 0.15 was a little too smooth
        // 0.05 probably good for VR, where you don't have to aim with the phone
        // 0.00 is good for when you have to aim slowly/precisely
        static public double RCFilterStrength = 0.05;
    }
}
