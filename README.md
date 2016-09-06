#Readme

##testapp-java
Publish a network service using mDNS/zeroconf/Bonjour. 
Writes the successful for to file last-port.txt.
Includes an option to automitically run the c# program.

##CoolFontWin 
Main program written in C\#. 
Reads the port from last-port.txt and binds a UDP socket to this port.
Sets up a virtual joystick using vJoy and feeds it data from the socket.

###Linking vJoy binaries to C\# project

The vJoy dlls were originally written in C.
A C# wrapper, **vJoyInterfaceWrap.dll**, was included by the vJoy creator. 
There are versions for 32 bit (x86) and 64 bit (x64) processors. 
Reference the correct dll by *Add Reference...* in Visual Studio.

You also need to copy to the original **vJoyInterface.dll** to the Main.exe location. 
To have Visual Studio do this for you, *Add an Existing Item* to your project, and choose **vJoyInterface.dll** (make sure to choose the correct bitness). 
Then in the item properties, change *Copy to Output Director* to *Copy if Newer*. 
This will make sure that your wrapper dll can always see a copy of the base dll.

##How to get it running
0. Launch vJoyConfig and enable Device 1
1. Run testapp-java to publish the service
2. Run (or have testapp-java run) the CoolFontWin main program
3. Launch the iPhone app and select the service
  * Check the c# console to see if the data is getting through
  * Check vJoy Monitor to see if the virtual device is responding

