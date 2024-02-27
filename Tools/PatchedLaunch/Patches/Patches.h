#pragma once

#include <cstdio>
#include <iostream>
#include <Windows.h>
#include <MinHook.h>

typedef void(__cdecl* t_LogInternal)(void*, const char* type, const char* category, int, int, int, const char* message);
typedef HANDLE(WINAPI* t_CreateMutexA)(LPSECURITY_ATTRIBUTES lpMutexAttributes, BOOL bInitialOwner, LPCSTR lpName);

t_LogInternal o_LogInternal = nullptr;
t_CreateMutexA o_CreateMutexA = nullptr;