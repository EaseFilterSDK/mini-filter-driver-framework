#include "stdafx.h"
#include "TestData.h"

WCHAR authorizedProcess[MAX_PATH] = L"notepad.exe;wordpad.exe";

VOID
SetAuthorizedProcess(WCHAR* processNames)
{
	ULONG len = wcslen(processNames);
	if (len < MAX_PATH)
	{
		ZeroMemory(authorizedProcess, MAX_PATH * sizeof(WCHAR));
		memcpy(authorizedProcess, processNames, len * sizeof(WCHAR));

		//wprintf(L"\nSetAuthorizedProcess :%ws  for test ONLY.\n", authorizedProcess);
	}
}

WCHAR*
GetAuthorizedProcess()
{
	//wprintf(L"\nGetAuthorizedProcess :%ws  for test ONLY.\n", authorizedProcess);

	return authorizedProcess;
}
