#define WIN32_LEAN_AND_MEAN

#include <stdio.h>
#include <cstdlib>
#include <Windows.h>

bool ProcessCreate(LPPROCESS_INFORMATION pi, char* lpszCommandLine)
{
	STARTUPINFOA si;
	ZeroMemory(&si, sizeof(si));
	si.cb = sizeof(si);
	ZeroMemory(pi, sizeof(PROCESS_INFORMATION));

	return CreateProcess(NULL, lpszCommandLine, NULL, NULL, FALSE, 0, NULL, NULL, &si, pi);
}

bool IsExecutableAddress(HANDLE hProcess, LPCVOID lpAddress)
{
	while (true)
	{
		MEMORY_BASIC_INFORMATION mbi = { NULL };

		if (VirtualQueryEx(hProcess, lpAddress, &mbi, sizeof(mbi)) == 0)
			return false;

		if (mbi.AllocationProtect == PAGE_EXECUTE_WRITECOPY &&
			mbi.State == MEM_COMMIT &&
			mbi.Protect == PAGE_EXECUTE_READ &&
			mbi.Type == MEM_IMAGE)
			return true;
	}

	return false;
}

bool ProcessInject(LPPROCESS_INFORMATION pi, const char* lpszFilePath)
{
	HMODULE hKernelModule = LoadLibrary("kernel32.dll");

	if (hKernelModule == NULL)
		return false;

	LPVOID lpLoadLibrary = (LPVOID)GetProcAddress(hKernelModule, "LoadLibraryA");

	if (lpLoadLibrary == NULL)
		return false;

	HANDLE hProcess = OpenProcess(PROCESS_CREATE_THREAD | PROCESS_QUERY_INFORMATION | PROCESS_VM_OPERATION | PROCESS_VM_WRITE | PROCESS_VM_READ, FALSE, pi->dwProcessId);

	if (hProcess == NULL)
		return false;

	if (!IsExecutableAddress(hProcess, lpLoadLibrary))
		return false;

	LPVOID lpBaseAddress = VirtualAllocEx(hProcess, NULL, strlen(lpszFilePath), MEM_RESERVE | MEM_COMMIT, PAGE_READWRITE);

	if (lpBaseAddress == NULL)
		return false;

	if (!WriteProcessMemory(hProcess, lpBaseAddress, lpszFilePath, strlen(lpszFilePath), NULL))
		return false;

	HANDLE hThread = CreateRemoteThread(hProcess, NULL, 0, (LPTHREAD_START_ROUTINE)lpLoadLibrary, lpBaseAddress, 0, NULL);

	if (hThread == NULL)
		return false;

	bool bResult = WaitForSingleObject(hThread, INFINITE) == WAIT_OBJECT_0;

	if (bResult)
	{
		DWORD dwExitCode;
		GetExitCodeThread(hThread, &dwExitCode);

		if (pi->hThread)
			ResumeThread(pi->hThread);
	}
	else
	{
		TerminateProcess(hProcess, 0);
	}

	CloseHandle(hThread);
	CloseHandle(hProcess);

	return bResult;
}

int main()
{
	char current_directory[MAX_PATH];
	if (GetCurrentDirectory(sizeof(current_directory), current_directory) == 0)
		return EXIT_FAILURE;

	char szCommandLine[0x2000];
	sprintf_s(szCommandLine, "%s\\SkySaga.exe envvarsfromfile=1", current_directory);

	PROCESS_INFORMATION pi;

	if (!ProcessCreate(&pi, szCommandLine))
		return EXIT_FAILURE;

	if (!ProcessInject(&pi, "Patches.dll"))
		return EXIT_FAILURE;

	// WaitForSingleObject(pi.hProcess, INFINITE);

	CloseHandle(pi.hProcess);
	CloseHandle(pi.hThread);

	return EXIT_SUCCESS;
}