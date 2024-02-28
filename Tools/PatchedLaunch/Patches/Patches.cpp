#define WIN32_LEAN_AND_MEAN

#include "Patches.h"

void ShowConsole()
{
	AllocConsole();

	FILE* file;
	freopen_s(&file, "CONOUT$", "w", stdout);
}

void hk_LogInternal(void*, const char* type, const char* category, int, void*, int, const char* message)
{
	printf(message);
}

HANDLE WINAPI hk_CreateMutexA(LPSECURITY_ATTRIBUTES lpMutexAttributes, BOOL bInitialOwner, LPCSTR lpName)
{
	HANDLE hResult = o_CreateMutexA(lpMutexAttributes, bInitialOwner, lpName);

	if (_stricmp(lpName, "BlitzTechAppInstanceMutex") == 0)
	{
		WriteProcessMemory(GetCurrentProcess(), LPVOID(0x412252), "\x90\x90\x90\x90\x90\x90\x90\x90\x90\x90", 10, nullptr);

		MH_STATUS status;

		if ((status = MH_CreateHook((LPVOID)0x8547F0, hk_LogInternal, (LPVOID*)&o_LogInternal)) != MH_OK)
			printf("Create Hook Failed! - %s\n", MH_StatusToString(status));

		MH_EnableHook((LPVOID)0x8547F0);
	}

	return hResult;
}

BOOL APIENTRY DllMain(HMODULE hModule, DWORD ul_reason_for_call, LPVOID lpReserved)
{
	if (ul_reason_for_call == DLL_PROCESS_ATTACH)
	{
		DisableThreadLibraryCalls(hModule);

		ShowConsole();

		MH_Initialize();

		MH_STATUS status;

		if ((status = MH_CreateHookApi("kernel32.dll", "CreateMutexA", hk_CreateMutexA, (LPVOID*)&o_CreateMutexA)) != MH_OK)
			printf("Create Hook Failed! - %s\n", MH_StatusToString(status));

		MH_EnableHook(MH_ALL_HOOKS);

		return TRUE;
	}

	if (ul_reason_for_call == DLL_PROCESS_DETACH)
	{
		// system("pause");

		// FreeConsole();

		return TRUE;
	}

	return FALSE;
}