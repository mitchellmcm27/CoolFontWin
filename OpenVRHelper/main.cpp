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
#include <ctime>
#include "openvr.h"

using namespace std;

#include "main.h"

//Globals
ofstream ofile;	
char dlldir[320];
HMODULE dllHandle;
bool dllLoaded;
void** g_deviceFunctionAddresses;
uint32_t unLeftHandIndex;
uint32_t unRightHandIndex;
vr::IVRSystem *pVRSystem;
void *pGenericInterface;

extern "C" __declspec(dllexport) void* APIENTRY GetIVRSystemFunctionAddress(short methodIndex, const int methodCount)
{
	// There are 119 functions defined in the IDirect3DDevice9 interface (including our 3 IUnknown methods QueryInterface, AddRef, and Release)
	const int interfaceMethodCount = methodCount;

	// If we do not yet have the addresses, we need to create our own Direct3D Device and determine the addresses of each of the methods
	// Note: to create a Direct3D device we need a valid HWND - in this case we will create a temporary window ourselves - but it could be a HWND
	//       passed through as a parameter of this function or some other initialisation export.
	if (!g_deviceFunctionAddresses) 
	{
		// Ensure hook dll is loaded

		__try 
		{
			add_log("Load openvr_api.dll...");
			HMODULE hMod = LoadLibraryA("openvr_api.dll"); // load the dll
			if (!hMod)
			{
				DWORD dw = GetLastError();
				LPVOID lpMsgBuf;

				FormatMessage(
					FORMAT_MESSAGE_ALLOCATE_BUFFER |
					FORMAT_MESSAGE_FROM_SYSTEM |
					FORMAT_MESSAGE_IGNORE_INSERTS,
					NULL,
					dw,
					MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
					(LPTSTR)&lpMsgBuf,
					0, NULL);

				add_log("  Could not load openvr_api.dll: %i %s", dw, lpMsgBuf);
			}
			add_log("  Module address: 0x%x", (uintptr_t)hMod);

			char buffer[320];
			GetModuleFileNameA(hMod, buffer, sizeof(buffer));
			
			add_log("  Module path: %s", buffer);

			const char *ver;

			bool installed = vr::VR_IsRuntimeInstalled();
			add_log("  Runtime installed? %s", installed ? "YES" : "NO");

			if (installed)
			{
				const char * path = vr::VR_RuntimePath();
				add_log("    %s", path);
			}

			bool hmd = vr::VR_IsHmdPresent();
			add_log("  HMD Present? %s", hmd ? "YES" : "NO");

			if (!hmd)
			{
				add_log("    No HMD, returning 0");
				return 0;
			}

			// request generic interface
			ver = vr::IVRSystem_Version;
				
			bool valid_version = vr::VR_IsInterfaceVersionValid(ver);
			add_log("  Interface version %s valid? %s", ver, valid_version ? "YES" : "NO");

			if (!valid_version)
			{
				add_log("    Version not valid, returning 0");
				return 0;
			}

			add_log("Get generic interface...");
			pVRSystem = NULL;
			vr::EVRInitError eError = vr::VRInitError_None;
			pGenericInterface = NULL;
			pGenericInterface = vr::VR_GetGenericInterface(ver, &eError);

			if (eError != vr::VRInitError_None)
			{
				add_log("  Error initializing: %s", VR_GetVRInitErrorAsSymbol(eError));
				return 0;
			}

			if (!pGenericInterface)
			{
				add_log("  Generic Interface was null");
				add_log("  Error initializing: %s", VR_GetVRInitErrorAsSymbol(eError));
				return 0;
			}

			pVRSystem = static_cast<vr::IVRSystem *>(pGenericInterface);

			if (!pVRSystem)
			{
				return 0;
			}

			add_log("Get vtable...");
			__try 
			{
				// retrieve a pointer to the VTable
				uintptr_t* pInterfaceVTable;
				if (pVRSystem)
				{
					pInterfaceVTable = (uintptr_t*)*(uintptr_t*)pVRSystem;
				}
				g_deviceFunctionAddresses = new void*[interfaceMethodCount]; // array size depends on how many methods

				// Retrieve the addresses of each of the methods (note first 3 IUnknown methods)
				// See steamvr.h to see the list of methods, the order they appear there
				// is the order they appear in the VTable, 1st one is index 0 and so on.
				for (int i=0; i<interfaceMethodCount; i++) {
					g_deviceFunctionAddresses[i] = (void*)pInterfaceVTable[i];
						
					// Log the address offset
					add_log("Method [%i] offset: 0x%x", i, pInterfaceVTable[i] - (uintptr_t)hMod);
				}
				add_log("Done");
			}
			__except(EXCEPTION_EXECUTE_HANDLER)
			{
				add_log("Something went wrong getting the vtable");
			}
		}
		__except(EXCEPTION_EXECUTE_HANDLER)
		{
			add_log("Something went wrong");
		}
	}

	// Return the address of the method requested
	if (g_deviceFunctionAddresses) {
		return g_deviceFunctionAddresses[methodIndex];
	} else {
		return 0;
	}
}

extern "C" __declspec(dllexport) uint32_t APIENTRY GetLeftHandIndex()
{
	if (!pVRSystem)
	{
		return 0;
	}

	for (vr::TrackedDeviceIndex_t i = 0; i < 16; i++)
	{
		vr::ETrackedControllerRole role = pVRSystem->GetControllerRoleForTrackedDeviceIndex(i);
		if (role == vr::TrackedControllerRole_LeftHand)
		{
			add_log("Left hand is index %d", i);
			return i;
		}
	}
	return 0;
}

extern "C" __declspec(dllexport) uint32_t APIENTRY GetRightHandIndex()
{
	if (!pVRSystem)
	{
		return 0;
	}

	for (vr::TrackedDeviceIndex_t i = 0; i < 16; i++)
	{
		vr::ETrackedControllerRole role = pVRSystem->GetControllerRoleForTrackedDeviceIndex(i);
		if (role == vr::TrackedControllerRole_RightHand)
		{
			add_log("Right hand is index %d", i);
			return i;
		}
	}
	return 0;
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

		add_log("\r\n---------------------\r\nOpenVRHelper Loaded\r\n---------------------");
		time_t now = time(0);
		add_log(ctime(&now));
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
