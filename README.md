#Readme

##CoolFontWin 
Companion program for the PocketStrafe iPhone app. Receives accelerometer data from iPhone and feeds it to input devices.

Options for vJoy (generic joystick, setup executable bundled with CFW) and keyboard input.

Opens a UDP socket and advertises this on the network through DNS-SD/mDNS.
Sets up a virtual joystick using vJoy and feeds it data from the socket.
