using System;
using System.Collections.Generic;
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
            Mouse2D, // tilt the phone L/R U/D to move the mouse pointer
        };

        static public MovementModes Mode { get; set; }

        static public string PORT_FILE = "../../../../last-port.txt";

        static public double THRESH_RUN = 0.7;
        static public double THRESH_WALK = 0.3;
        static public int mouseSens = 5;
        static public int socketPollInterval = 8*1000; // microseconds (us)
    }
}
