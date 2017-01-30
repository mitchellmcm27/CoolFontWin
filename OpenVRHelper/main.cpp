/*
	Author	: Justin S
	Date	: March 14th, 2010

	Credits :
		Matthew L (Azorbix) @ http://www.game-deception.com
		Plenty of background information and examples on hooking (and I used the add_log function from there).

	Tools used:
		Microsoft Visual Studio .NET 2008
		Microsoft DirectX SDK (February 2010)

	Information:
		This D3D helper was developed for an article on Direct3D 9 hooking at http://spazzarama.wordpress.com
*/

// Check windows
#if _WIN32 || _WIN64
#if _WIN64
#define ENVIRONMENT64
#else
#define ENVIRONMENT32
#endif
#endif

// Check GCC
#if __GNUC__
#if __x86_64__ || __ppc64__
#define ENVIRONMENT64
#else
#define ENVIRONMENT32
#endif
#endif

#include <windows.h>
#include <fstream>
#include <stdio.h>
#include "openvr.h"

using namespace std;

#include "main.h"

//Globals
ofstream ofile;	
char dlldir[320];
HMODULE dllHandle;

bool dllLoaded;

uintptr_t** g_deviceFunctionAddresses;
extern "C" __declspec(dllexport) uintptr_t* APIENTRY GetOpenVRFunctionAddress(short methodIndex)
{
	// There are 119 functions defined in the IDirect3DDevice9 interface (including our 3 IUnknown methods QueryInterface, AddRef, and Release)
	const int interfaceMethodCount = 44;

	// If we do not yet have the addresses, we need to create our own Direct3D Device and determine the addresses of each of the methods
	// Note: to create a Direct3D device we need a valid HWND - in this case we will create a temporary window ourselves - but it could be a HWND
	//       passed through as a parameter of this function or some other initialisation export.
	if (!g_deviceFunctionAddresses) {
		// Ensure hook dll is loaded

		__try 
		{
			HMODULE hMod = LoadLibraryA("openvr_api.dll"); // load the dll
			// create VR object
			vr::IVRSystem *pVRSystem = NULL;
			vr::HmdError peError = vr::VRInitError_None;
			pVRSystem = vr::VR_Init(&peError, vr::VRApplication_Background);

			if (peError != vr::VRInitError_None)
			{
				return 0;
			}

			__try 
			{
				// retrieve a pointer to the VTable
				uintptr_t* pInterfaceVTable = (uintptr_t*)*(uintptr_t*)pVRSystem;
				g_deviceFunctionAddresses = new uintptr_t*[interfaceMethodCount]; // array size depends on how many methods

				// Retrieve the addresses of each of the methods (note first 3 IUnknown methods)
				// See d3d9.h IDirect3D9Device to see the list of methods, the order they appear there
				// is the order they appear in the VTable, 1st one is index 0 and so on.
				for (int i=0; i<interfaceMethodCount; i++) {
					g_deviceFunctionAddresses[i] = (uintptr_t*)pInterfaceVTable[i];
						
					// Log the address offset
					add_log("Method [%i] offset: 0x%x", i, pInterfaceVTable[i] - (uintptr_t)hMod);
				}
			}
			__finally
			{
				
			}
		}
		__finally 
		{
		}
	}

	// Return the address of the method requested
	if (g_deviceFunctionAddresses) {
		return g_deviceFunctionAddresses[methodIndex];
	} else {
		return 0;
	}
}

bool WINAPI DllMain(HMODULE hDll, DWORD dwReason, PVOID pvReserved)
{
	if(dwReason == DLL_PROCESS_ATTACH)
	{
		// No need to call DllMain for each thread start/stop
		DisableThreadLibraryCalls(hDll);

		dllHandle = hDll;

		// Prepare logging
		GetModuleFileNameA(hDll, dlldir, 512);
		for(int i = strlen(dlldir); i > 0; i--) { if(dlldir[i] == '\\') { dlldir[i+1] = 0; break; } }
		ofile.open(GetDirectoryFile("OpenVRHelperLog.txt"), ios::app);

		add_log("\r\n---------------------\r\nOpenVRHelper Loaded...\r\n---------------------");

		dllLoaded = true;
		return true;
	}

	else if(dwReason == DLL_PROCESS_DETACH)
	{
		add_log("---------------------\r\nOpenVRHelper Exiting...\r\n---------------------\r\n");
		if(ofile) { ofile.close(); }
		// Only free memory allocated on the heap if we are exiting due to a dynamic unload 
		// (application termination can leave the heap in an unstable state and we should leave the OS to clean up)
		// http://msdn.microsoft.com/en-us/library/ms682583(VS.85).aspx
		if (!pvReserved) {
			if(g_deviceFunctionAddresses) { delete g_deviceFunctionAddresses; }
		}
	}

    return false;
}

char *GetDirectoryFile(char *filename)
{
	static char path[320];
	strcpy(path, dlldir);
	strcat(path, filename);
	return path;
}

void __cdecl add_log (const char *fmt, ...)
{
	if(!ofile) {
		GetModuleFileNameA(dllHandle, dlldir, 512);
		for(int i = strlen(dlldir); i > 0; i--) { if(dlldir[i] == '\\') { dlldir[i+1] = 0; break; } }
		ofile.open(GetDirectoryFile("OpenVRHelperLog.txt"), ios::app);
	}

	if(ofile)
	{
		if(!fmt) { return; }

		va_list va_alist;
		char logbuf[256] = {0};

		va_start (va_alist, fmt);
		_vsnprintf (logbuf+strlen(logbuf), sizeof(logbuf) - strlen(logbuf), fmt, va_alist);
		va_end (va_alist);

		ofile << logbuf << endl;
	}
}
