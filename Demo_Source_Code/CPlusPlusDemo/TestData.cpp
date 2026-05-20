#include "stdafx.h"
#include "FilterAPI.h"
#include "TestData.h"

WCHAR authorizedProcess[MAX_PATH] = L"notepad.exe;wordpad.exe";

VOID
SetAuthorizedProcess(WCHAR* processNames)
{
	ULONG len = (ULONG)wcslen(processNames);
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

VOID
CreateTestFiles(WCHAR* stubFileFolder)
{
	BOOL ret = FALSE;
	HANDLE hFile = INVALID_HANDLE_VALUE;


	WCHAR sourceFolder[MAX_PATH];
	if (!GetCurrentDirectory(MAX_PATH, sourceFolder))
	{
		wprintf(L"Get current directory failed.");
		return;
	}

	wcscat_s(sourceFolder, MAX_PATH, L"\\testSourceFiles");
	
	ret = CreateDirectory(sourceFolder, NULL);
	if (!ret)
	{
		DWORD lastError = GetLastError();
		if (lastError != ERROR_ALREADY_EXISTS)
		{
			wprintf(L"Create sourceFolder %s failed, last error is:%d", sourceFolder, lastError);
			return;
		}
	}

	ret = CreateDirectory(stubFileFolder, NULL);
	if (!ret)
	{
		DWORD lastError = GetLastError();
		if (lastError != ERROR_ALREADY_EXISTS)
		{
			wprintf(L"Create stubFileFolder %s failed, last error is:%d", stubFileFolder, lastError);
			return;
		}
	}

	//we will create the test files in source folder.
	//when the user open the test folder, it will get file list from the source folder,
	//if user read the virtual file, it should read the data from source folder.

	char* testFileContent = "This is a test source file for stub file test.\r\n";

	WCHAR testFileName[260];
	ZeroMemory(testFileName, sizeof(testFileName));

	WCHAR testStubFileName[260];
	ZeroMemory(testStubFileName, sizeof(testStubFileName));

	for (ULONG i = 0; i < 10; i++)
	{
		swprintf_s(testFileName, L"%s\\test.%d.txt", sourceFolder, i);

		HANDLE handle = CreateFile(testFileName, GENERIC_WRITE, NULL, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);

		if (handle == INVALID_HANDLE_VALUE)
		{
			wprintf(L"Create test file %s failed, last error is:%d", testFileName, GetLastError());
			continue;
		}

		DWORD bytesWritten = 0;

		for (ULONG j = 0; j < (i + 1) * 1024; j++)
		{
			if (!WriteFile(handle, (LPVOID)testFileContent, (DWORD)strlen(testFileContent), &bytesWritten, NULL))
			{
				wprintf(L"WriteFile %s failed, last error is:%d", testFileName, GetLastError());

				CloseHandle(handle);
				continue;
			}
		}

		LONGLONG testFileSize = GetFileSize(handle, NULL);

		CloseHandle(handle);

		swprintf_s(testStubFileName, L"%s\\test.%d.txt", stubFileFolder, i);

		//for the stub file test, we set the tag data to the test source file name.
		CreateVirtualStubFile(testStubFileName, testFileSize, (ULONG)wcslen(testFileName) * 2, (BYTE*)testFileName);

		SetFileAttributes(testStubFileName,FILE_ATTRIBUTE_OFFLINE);

		wprintf(L"Created test source file %s testStubFile:%s\r\n", testFileName, testStubFileName);

	}

}