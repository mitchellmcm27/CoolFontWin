# CoolFontWin
## Companion app for PocketStrafe iOS and Android apps
### A VR locomotion solution

Allows VR locomotion by running in place. CoolFontWin receives IMU data from mobile devices over WiFi. This is done in a few steps: open a UDP listen socket, register a network service discoverable through Bonjour on the same socket, acquire and manage physical Xbox controllers, virtual gamepads, and joysticks to send input to the user's applications.

### High level change-log
* v1.1 moved most of the functions from the crowded context menu into their own window (WPF/MVVM).
* v1.0 added support for virtual [virtual Xbox 360 controllers](https://github.com/nefarius/ScpVBus).
* v0.2 began support for multiple sources of input (two mobile phones) and introduced custom sound effects, automatic ClickOnce update-checking in the background. Version 0.2.3 enabled an enlarged context menu when VR is running on the user's system.

### Installation
Run attached setup.exe, x64 only.

### Building from source
I use VS 2015. All of the required binaries are included in the git repo (lib folder).

### Thanks
Check out [my GitHub stars](https://github.com/mitchellmcm27?tab=stars) for the many open source libraries that I used. A few of the key ones for this application include [vJoy](https://github.com/shauleiz/vJoy), [ScpVBus](https://github.com/nefarius/ScpVBus), and [Material Design in XAML Toolkit](https://github.com/ButchersBoy/MaterialDesignInXamlToolkit).
