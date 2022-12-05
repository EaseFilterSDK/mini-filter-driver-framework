///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Technologies
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
//    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
//
//    This module contains sample code provided for convenience and
//    demonstration purposes only,this software is provided on an 
//    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
//     either express or implied.  
//
///////////////////////////////////////////////////////////////////////////////

#include "stdafx.h"
#include "Tools.h"
#include "FilterControl.h"
#include "UnitTest.h"

#define	MAX_ERROR_MESSAGE_SIZE	1024

static WCHAR* testFolder = L"c:\\filterTest";
static WCHAR* filterMask = L"c:\\filterTest\\*";
static WCHAR* testFile = L"c:\\filterTest\\myTestFile.txt";
static WCHAR* reparseFolder = L"c:\\reparse";
static WCHAR* reparseMask = L"c:\\reparse\\*";
static WCHAR* reparseFile = L"c:\\reparse\\myTestFile.txt";
static CHAR* testData = "EaseFilter Test Data for read/write,This is the original data when we setup a new test file.\n For callback filter read/write test,It will modify this data";
static CHAR* replaceData = "This replace data test,it is not in the file.It will replace/merge  the original data";
static CHAR* reparseData = "EaseFilter Test Data for reparse open,this is the data from reparse file open.";
static LONGLONG testFileSize = 2147483648; //2 GB;


BOOL
IsTestFile(WCHAR* fileName,ULONG fileNameLength )
{
	if( _wcsnicmp(testFile,fileName,wcslen(testFile)) == 0)
	{
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

BOOL
IsTestFolder(WCHAR* fileName )
{
	if( _wcsnicmp(testFolder,fileName,wcslen(testFolder)) == 0)
	{
		return TRUE;
	}
	else
	{
		return FALSE;
	}
}

WCHAR*
GetFilterMask()
{
	return filterMask;
}

WCHAR*
GetTestReparseFileName()
{
	return reparseFile;
}

CHAR*
GetReplaceData()
{
	return replaceData;
}

LONGLONG
GetTestFileSize()
{
	return testFileSize;
}

LARGE_INTEGER
GetTestFileTime()
{
	SYSTEMTIME st;
	FILETIME fileTime;
	GetSystemTime(&st);

	/*printf("Current SystemTime: Year:%d Month:%d Day:%d Hour:%d Min:%d Second:% d\n" 
		,st.wYear,st.wMonth,st.wDay,st.wHour,st.wMinute,st.wSecond);*/

	//set the test file time to 2011/11/11
	st.wYear = 2011;
	st.wMonth = 11;
	st.wDay = 11;
	st.wHour = 11;
	st.wMinute = 11;
	st.wSecond = 11;
	st.wMilliseconds = 11;

	SystemTimeToFileTime(&st,&fileTime);

	LARGE_INTEGER large;
	large.LowPart=fileTime.dwLowDateTime;
	large.HighPart=fileTime.dwHighDateTime;

	return large;
}

BOOL
CreateTestFile(WCHAR* folder,WCHAR* fileName,CHAR* data)
{
	LARGE_INTEGER ByteOffset = {0};
    OVERLAPPED Overlapped = {0};
    DWORD dwTransferred = 0;
    DWORD Length = 0;
    int nError = ERROR_SUCCESS;
	BOOL ret = FALSE;

	//Create the test folder.
 	ret = CreateDirectory(folder,NULL);

	if( ret == 0 )
	{
		nError = GetLastError();
		if( nError != ERROR_ALREADY_EXISTS )
		{
			PrintErrorMessage(L"Create test folder failed.", nError);
			return FALSE;
		}
	}

	//Remove readOnly attribue if it exist.
	SetFileAttributes(fileName,FILE_ATTRIBUTE_NORMAL);

	//Create the test file,if it exist, overwrite it.
	HANDLE pFile = CreateFile(fileName,GENERIC_WRITE,NULL,NULL,CREATE_ALWAYS,FILE_ATTRIBUTE_NORMAL,NULL);

	if( pFile == INVALID_HANDLE_VALUE )
	{
		PrintErrorMessage(L"Create test file failed.", GetLastError());
		return FALSE;
	}

	Length = (DWORD)strlen(data);

	// Write test data to the test file.
	if(!WriteFile(pFile, data, Length, &dwTransferred, NULL))
	{
		nError = GetLastError();
		PrintErrorMessage(L"WriteFile failed.", nError);
		ret = FALSE;
		goto EXIT;
	}

	ret = TRUE;

EXIT:

	CloseHandle(pFile);

	return ret;

}

BOOL
SetupTestEnvironment()
{
	BOOL ret = CreateTestFile(testFolder,testFile,testData);

	if(!ret)
	{
		return ret;
	}

	ret = CreateTestFile(reparseFolder,reparseFile,reparseData);

	return ret;
}

BOOL
AccessFlagControlTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;
	CHAR buffer[4096];
	ULONG bufferLength = sizeof(buffer);
	HANDLE pFile = INVALID_HANDLE_VALUE;
	ULONG accessFlag = 0;
	DWORD	dwError = 0;

	FileFilterRule fileFilter(filterMask);
	//after the processes open the managed files, it then can't create the new files till the processes terminated. 
	accessFlag = (~ALLOW_ALL_SAVE_AS)&ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;

	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	pFile = CreateFile(testFile,GENERIC_WRITE,NULL,NULL,OPEN_ALWAYS,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
	  PrintErrorMessage(L"ALLOW_ALL_SAVE_AS test failed, it can't open the protected file.",GetLastError());
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		CloseHandle(pFile);
	}

	pFile = CreateFile(testFile,GENERIC_WRITE,NULL,NULL,CREATE_ALWAYS,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
	  PrintErrorMessage(L"ALLOW_ALL_SAVE_AS test failed.",0);
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_ALL_SAVE_AS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_ALL_SAVE_AS test passed.\n");
	}

	//we need to remove the current process Id from the block save as list, or the current process can't create the new file.
	if(!RemoveBlockSaveAsProcessId(GetCurrentProcessId()))
	{
		PrintLastErrorMessage(L"RemoveBlockSaveAsProcessId failed.");
		ret = FALSE;
		goto EXIT;
	}


	//Test Remove ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY
	accessFlag = (~ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY)&ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;

	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	pFile = CreateFile(testFile,ACCESS_SYSTEM_SECURITY,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
	  PrintErrorMessage(L"ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY test failed.",0);
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY test passed.\n");
	}

	
	//Test Remove ALLOW_OPEN_WITH_READ_ACCESS
	accessFlag = (~ALLOW_OPEN_WITH_READ_ACCESS)&ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;

	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);


	pFile = CreateFile(testFile,GENERIC_READ,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
	  PrintErrorMessage(L"ALLOW_OPEN_WITH_READ_ACCESS test failed.",0);
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_OPEN_WITH_READ_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}
		PrintPassedMessage(L"ALLOW_OPEN_WITH_READ_ACCESS test passed.\n");
	}
	

	//Test Remove ALLOW_OPEN_WITH_WRITE_ACCESS
	accessFlag = (~ALLOW_OPEN_WITH_WRITE_ACCESS)&ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	pFile = CreateFile(testFile,GENERIC_WRITE,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
	  PrintErrorMessage(L"ALLOW_OPEN_WITH_WRITE_ACCESS test failed.",0);
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_OPEN_WITH_WRITE_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_OPEN_WITH_WRITE_ACCESS test passed.\n");
	}
	
	//Not allow open file with create or overwrite option.
	accessFlag = (~ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	pFile = CreateFile(testFile,GENERIC_WRITE,NULL,NULL,CREATE_ALWAYS,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
	  PrintErrorMessage(L"ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_EXIST_FILE test failed.",0);
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_EXIST_FILE test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_EXIST_FILE test passed.\n");
	}
	


	//Test Remove ALLOW_OPEN_WITH_DELETE_ACCESS.
	accessFlag = (~ALLOW_OPEN_WITH_DELETE_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	pFile = CreateFile(testFile,DELETE,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
	  PrintErrorMessage(L"ALLOW_OPEN_WITH_DELETE_ACCESS test failed.",0);
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_OPEN_WITH_DELETE_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_OPEN_WITH_DELETE_ACCESS test passed.\n");
	}
	
	
	//Test Remove ALLOW_READ_ACCESS.
	accessFlag = (~ALLOW_READ_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	pFile = CreateFile(testFile,GENERIC_READ,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
	  PrintErrorMessage(L"Open test file for read access failed.",GetLastError());
	  ret = FALSE;
	  goto EXIT;
	}

	ret = ReadFile(pFile,buffer,bufferLength,&bufferLength,NULL);
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_READ_ACCESS test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_READ_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_READ_ACCESS test passed.\n");
		CloseHandle(pFile);
	}

	//Test Remove ALLOW_WRITE_ACCESS.
	accessFlag = (~ALLOW_WRITE_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	pFile = CreateFile(testFile,GENERIC_WRITE,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
	  PrintErrorMessage(L"Open test file for write access failed.",GetLastError());
	  ret = FALSE;
	  goto EXIT;
	}


	ret = WriteFile(pFile,buffer,bufferLength,&bufferLength,NULL);
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_WRITE_ACCESS test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_WRITE_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}
		PrintPassedMessage(L"ALLOW_WRITE_ACCESS test passed.\n");
		CloseHandle(pFile);
	}


	//Test Remove ALLOW_QUERY_INFORMATION_ACCESS.
	accessFlag = (~ALLOW_QUERY_INFORMATION_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	pFile = CreateFile(testFile,GENERIC_READ,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
	  PrintErrorMessage(L"Open test file for query information failed.",GetLastError());
	  ret = FALSE;
	  goto EXIT;
	}

	BY_HANDLE_FILE_INFORMATION fileInfo;
	ret = GetFileInformationByHandle(pFile,&fileInfo);
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_QUERY_INFORMATION_ACCESS test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_QUERY_INFORMATION_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_QUERY_INFORMATION_ACCESS test passed.\n");
		CloseHandle(pFile);
	}


		
	//Test Remove ALLOW_SET_INFORMATION.
	accessFlag = (~ALLOW_SET_INFORMATION) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	pFile = CreateFile(testFile,GENERIC_ALL,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
	  PrintErrorMessage(L"Open test file for set information failed.",GetLastError());
	  ret = FALSE;
	  goto EXIT;
	}

	//this API only support in windows vista or later version.  
	//ret = SetFileInformationByHandle(pFile,FileBasicInfo,&fileInfo,sizeof(BY_HANDLE_FILE_INFORMATION));

	//we will test with this API 
	ret = SetFileAttributes(testFile,FILE_ATTRIBUTE_READONLY);
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_SET_INFORMATION test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_SET_INFORMATION test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_SET_INFORMATION test passed.\n");
		CloseHandle(pFile);
	}

	//Test Remove ALLOW_FILE_RENAME.
	accessFlag = (~ALLOW_FILE_RENAME) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	ret = MoveFile (testFile,L"c:\\TestMoveFileNameFile.txt");
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_FILE_RENAME test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_FILE_RENAME test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_FILE_RENAME test passed.\n");
	}

	//Test Remove ALLOW_FILE_DELETE.
	accessFlag = (~ALLOW_FILE_DELETE) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	ret = DeleteFile(testFile);
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_FILE_DELETE test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_FILE_DELETE test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_FILE_DELETE test passed.\n");
	}

	//Test Remove ALLOW_FILE_SIZE_CHANGE.
	accessFlag = (~ALLOW_FILE_SIZE_CHANGE) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	pFile = CreateFile(testFile,GENERIC_ALL,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
	  PrintErrorMessage(L"Open test file for file size change failed.",GetLastError());
	  ret = FALSE;
	  goto EXIT;
	}

	DWORD newPointer = SetFilePointer(pFile,65536,NULL,FILE_BEGIN);//set file pointer to 65536 from the beginning.

	if( INVALID_SET_FILE_POINTER == newPointer)
	{
		PrintErrorMessage(L"SetFilePointer failed.",GetLastError());
		goto EXIT;
	}

	ret = SetEndOfFile(pFile);
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_FILE_SIZE_CHANGE test failed.",0);
		ret = FALSE;
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_FILE_SIZE_CHANGE test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_FILE_SIZE_CHANGE test passed.\n");
		CloseHandle(pFile);
	}



	//Test Remove ALLOW_QUERY_SECURITY_ACCESS.
	accessFlag = (~ALLOW_QUERY_SECURITY_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	BYTE SecDescBuff[0x4000];
    DWORD cbSD =  sizeof(SecDescBuff);

    ret = GetFileSecurity(testFile, DACL_SECURITY_INFORMATION, SecDescBuff, cbSD, &cbSD);
	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_QUERY_SECURITY_ACCESS test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_QUERY_SECURITY_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}


		PrintPassedMessage(L"ALLOW_QUERY_SECURITY_ACCESS test passed.\n");
	}


	//Test Remove ALLOW_SET_SECURITY_ACCESS.
	accessFlag = (~ALLOW_SET_SECURITY_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	cbSD =  sizeof(SecDescBuff);

	ret = GetFileSecurity(testFile, GROUP_SECURITY_INFORMATION, SecDescBuff, cbSD, &cbSD);
	if(0 == ret)
	{
		PrintErrorMessage(L"ALLOW_SET_SECURITY_ACCESS GetFileSecurity failed.",GetLastError());
		goto EXIT;
	}
	
	 ret = SetFileSecurity(testFile, GROUP_SECURITY_INFORMATION, SecDescBuff);

	if(0 != ret)
	{
		PrintErrorMessage(L"ALLOW_SET_SECURITY_ACCESS test failed.",0);
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_SET_SECURITY_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}


		PrintPassedMessage(L"ALLOW_SET_SECURITY_ACCESS test passed.\n");
	}

	
	//Test Remove ALLOW_DIRECTORY_LIST_ACCESS.
	accessFlag = (~ALLOW_DIRECTORY_LIST_ACCESS) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	 WIN32_FIND_DATA findFileData;
	 pFile = FindFirstFile(filterMask, &findFileData);

	if(INVALID_HANDLE_VALUE != pFile)
	{
		PrintErrorMessage(L"ALLOW_DIRECTORY_LIST_ACCESS test failed.",0);

		printf("handle:%p  file:%d  ",pFile,findFileData.dwFileAttributes);
		FindClose(pFile);
		pFile = INVALID_HANDLE_VALUE;
		goto EXIT;
	}
	else
	{
		dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"ALLOW_DIRECTORY_LIST_ACCESS test failed.",dwError);
		  ret = FALSE;
		  goto EXIT;
		}

		PrintPassedMessage(L"ALLOW_DIRECTORY_LIST_ACCESS test passed.\n");
	}

	//
	//add exclude filter mask test
	//you can add multiply exclude filter masks with the same accessFlag and same filter mask.
	//
	//Here we do the same test as above test, but we add the exclude filter mask,
	//the above test, it can't open the file with security access, now we add the exclude to our test file,
	//now we should be fine to open the file.
	//
	//
	accessFlag = (~ALLOW_SET_INFORMATION) & ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	fileFilter.AddExcludeFileFilterMask(L"*mytestFile.txt");
	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	pFile = CreateFile(testFile,GENERIC_ALL,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
	  PrintErrorMessage(L"Open test file for set information failed.",GetLastError());
	  ret = FALSE;
	  goto EXIT;
	}

	//we will test with this API 
	ret = SetFileAttributes(testFile,FILE_ATTRIBUTE_READONLY);
	if(0 == ret)
	{
	  PrintErrorMessage(L"exclude filter mask test failed.",0);
	  ret = FALSE;
	  goto EXIT;
	}
	else
	{
		PrintPassedMessage(L"exclude filter mask test passed.\n");
		CloseHandle(pFile);
		pFile = INVALID_HANDLE_VALUE;
	}



	ret = TRUE;

EXIT:

	if(!ret )
	{
	   PrintFailedMessage(L"\nAccess Flag Test failed.\n\n");	   
	}
	else
	{
	   PrintPassedMessage(L"\nAccess Flag Test Passed.\n\n");
	}

	if( INVALID_HANDLE_VALUE != pFile )
	{
	   CloseHandle(pFile);
	}

	return ret;
}

BOOL
ReparseFilterRuleTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	ULONG accessFlag = ENABLE_REPARSE_FILE_OPEN|ALLOW_MAX_RIGHT_ACCESS;	
	fileFilter.AccessFlag = accessFlag;
	fileFilter.ReparseFileFilterMask = reparseMask;

	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);
	
	HANDLE pFile = CreateFile(testFile,GENERIC_READ,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( NULL == pFile )
	{
		PrintErrorMessage(L"Open test file failed.",GetLastError());
		goto EXIT;
	}
	else
	{
		ULONG bufferLength =(ULONG)strlen(reparseData);
		CHAR* buffer = (CHAR*)malloc( bufferLength );

		if(NULL == buffer)
		{
			PrintErrorMessage(L"Reparse filter rule test failed.Can't allocate memory for test.",0);
			goto EXIT;
		}

		ret = ReadFile(pFile,buffer,bufferLength,&bufferLength,NULL);

		if(0 == ret)
		{
			PrintErrorMessage(L"Read test file failed.",GetLastError());
			goto EXIT;
		}

		if( memcmp(buffer,reparseData,bufferLength) == 0)
		{
			ret = TRUE;
		}
		else
		{
			PrintErrorMessage(L"The file open didnt reparse to the new file.",GetLastError());
			printf("ReparseData:%s\n\n",reparseData);
			printf("DataReturn:%s\n\n",buffer);
			ret = FALSE;
		}
	  
	}

EXIT:

	if( ret )
	{
		PrintPassedMessage(L"\nReparse filter rule test passed.\n\n");
	}
	else
	{
		PrintFailedMessage(L"\nReparse filter rule test failed.\n\n");
	}

	if(pFile)
	{
		CloseHandle(pFile);
	}

	return ret;

}

BOOL
ReadControlFilterTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	ULONG accessFlag = ALLOW_OPEN_WITH_READ_ACCESS |ALLOW_READ_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	fileFilter.BooleanConfig = ENABLE_SEND_DATA_BUFFER;
	fileFilter.ControlFileIOEventFilter =  PRE_FASTIO_READ|PRE_CACHE_READ|PRE_NOCACHE_READ|PRE_PAGING_IO_READ;

	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);


	HANDLE pFile = CreateFile(testFile,GENERIC_READ,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
		PrintErrorMessage(L"Open file for read test failed.",GetLastError());
		goto EXIT;
	}
	else
	{
		ULONG bufferLength = (ULONG)strlen(replaceData);
		CHAR* buffer = (CHAR*)malloc( bufferLength );

		if(NULL == buffer)
		{
			PrintErrorMessage(L"Can't allocate memory for test.",0);
			goto EXIT;
		}

		ret = ReadFile(pFile,buffer,bufferLength,&bufferLength,NULL);

		if(0 == ret)
		{
			PrintErrorMessage(L"Read test file failed.",GetLastError());
			goto EXIT;
		}

		if( memcmp(buffer,replaceData,bufferLength) == 0)
		{

			//
			//in pre-read call back fucntion test, it will return replace data instead of the test data.
			//				
			ret = TRUE;
		}		
		else
		{
			PrintErrorMessage(L"Read data wasn't replaced by replace data.",0);

			printf("Read return data:\r\n%s\r\n\r\n",buffer);
			printf("Replace data:\r\n%s\r\n\r\n",replaceData);


			ret = FALSE;
		}
	  
	}

EXIT:

	if( ret )
	{
		PrintPassedMessage(L"\nRead Control Filter Test Passed.\n\n");
	}
	else
	{
		PrintFailedMessage(L"\nRead Control Filter Test Failed.\n\n");
	}

	if(pFile)
	{
		CloseHandle(pFile);
	}

	return ret;

}

BOOL
WriteControlFilterTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	fileFilter.BooleanConfig = ENABLE_SEND_DATA_BUFFER;
	fileFilter.ControlFileIOEventFilter =  PRE_FASTIO_WRITE|PRE_CACHE_WRITE|PRE_NOCACHE_WRITE|PRE_PAGING_IO_WRITE;

	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	HANDLE pFile = CreateFile(testFile,GENERIC_ALL,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
		PrintErrorMessage(L"Open file for write test failed.",GetLastError());
		goto EXIT;
	}
	else
	{
		CHAR* buffer ="Test write data to my file,it will replace the test data in pre-write call back test function.";
		ULONG bufferLength = (ULONG)strlen(buffer);

		ret = WriteFile(pFile,buffer,bufferLength,&bufferLength,NULL);

		if(0 == ret)
		{
			PrintErrorMessage(L"WriteFile failed.",GetLastError());
			goto EXIT;
		}

		ULONG replaceDataLength = (ULONG)strlen(replaceData);
		CHAR* replaceDataBuffer = (CHAR*)malloc( replaceDataLength );

		if(NULL == replaceDataBuffer)
		{
			ret = FALSE;
			PrintErrorMessage(L"Can't allocate memory for test.",0);
			goto EXIT;
		}

		//set file pointer the begining.
		SetFilePointer(pFile,0,NULL,FILE_BEGIN);
		ret = ReadFile(pFile,replaceDataBuffer,replaceDataLength,&replaceDataLength,NULL);

		if(0 == ret)
		{
			PrintErrorMessage(L"WriteControlFilterTest ReadFile failed.",GetLastError());
			goto EXIT;
		}

		if( memcmp(replaceDataBuffer,replaceData,replaceDataLength) == 0)
		{

			//
			//in pre-write call back fucntion test, it will return replace data instead of the test data.
			//

			ret = TRUE;
		}		
		else
		{
			printf("Write Test Failed.\nDataFromFile:%s \n CorrectData:%s\n",replaceDataBuffer,replaceData);
			ret = FALSE;
		}
	  
	}

EXIT:

	if( ret )
	{
		PrintPassedMessage(L"\nWrite Control Filter Test Passed.\n\n");
	}
	else
	{
		PrintFailedMessage(L"\nWrite Control Filter Test Failed.\n\n");
	}

	if(pFile)
	{
		CloseHandle(pFile);
	}

	return ret;

}

BOOL 
QueryInformationControlTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	ULONG accessFlag = ALLOW_OPEN_WITH_READ_ACCESS|ALLOW_QUERY_INFORMATION_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	fileFilter.ControlFileIOEventFilter = PRE_QUERY_INFORMATION;

	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	HANDLE pFile = CreateFile(testFile,GENERIC_READ,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE == pFile )
	{
		PrintErrorMessage(L"Open test file failed.",GetLastError());
		goto EXIT;
	}
	else
	{
		  FILETIME creationTime;
		  FILETIME lastAccessTime;
		  FILETIME lastWriteTime;
		
		  ret = GetFileTime(pFile,&creationTime,&lastAccessTime,&lastWriteTime );

		if(0 == ret)
		{
			PrintErrorMessage(L"QueryInformationControlTest GetFileInformationByHandle failed.",GetLastError());
			goto EXIT;
		}
		else 
		{
			LARGE_INTEGER writeTime = GetTestFileTime();

			if( writeTime.HighPart != lastWriteTime.dwHighDateTime || writeTime.LowPart != lastWriteTime.dwLowDateTime )
			{
				ret = FALSE;
				PrintErrorMessage(L"QueryInformationControlTest failed. Return last write time is not correct", 0 );
			}

		}

	  
	}

EXIT:

	if( ret )
	{
		PrintPassedMessage(L"\nQueryInformationControlTest passed.\n\n");
	}
	else
	{
		PrintFailedMessage(L"\nQueryInformationControlTest failed.\n\n");
	}

	if(pFile)
	{
		CloseHandle(pFile);
	}

	return ret;
}

BOOL
SetInformationControlTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	fileFilter.ControlFileIOEventFilter = PRE_SET_INFORMATION;

	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	//
	//In PRE_SET_INFORMATION call back test function,we add the readonly attribute to the file.
	//

	ret = SetFileAttributes(testFile,FILE_ATTRIBUTE_NORMAL);

	if( 0 == ret)
	{
		PrintErrorMessage(L"SetInformationControlTest SetFileAttributes failed.",GetLastError());
		return FALSE;
	}

	ULONG attributes = GetFileAttributes(testFile);

	if( INVALID_FILE_ATTRIBUTES == attributes )
	{
		PrintErrorMessage(L"SetInformationControlTest GetFileAttributes failed.",GetLastError());
		return FALSE;
	}

	if( attributes & FILE_ATTRIBUTE_READONLY )
	{
		ret = TRUE;
		PrintPassedMessage(L"\nSetInformationControlTest passed.\n\n");
	}
	else
	{
		PrintFailedMessage(L"\nSetInformationControlTest failed.\n\n");
	}


	return ret;
}

BOOL
BrowseDirectoryControlTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	ULONG accessFlag = ALLOW_OPEN_WITH_READ_ACCESS|ALLOW_DIRECTORY_LIST_ACCESS;
	fileFilter.AccessFlag = accessFlag;
	fileFilter.ControlFileIOEventFilter = POST_DIRECTORY;

	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	WIN32_FIND_DATA ffd;
	HANDLE pFile = FindFirstFile(filterMask, &ffd);

	do
    {
      if (ffd.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY)
      {
         printf("  %ws   <DIR>\n", ffd.cFileName);
      }
      else
      {
		 LARGE_INTEGER filesize;
         filesize.LowPart = ffd.nFileSizeLow;
         filesize.HighPart = ffd.nFileSizeHigh;
         printf("  %ws   %I64d bytes\n", ffd.cFileName, filesize.QuadPart);

		 if( filesize.QuadPart != GetTestFileSize())
		 {
			 PrintErrorMessage(L"Browse directory failed.Return file size is not the same as test file size.",0);
			 ret = FALSE;
			 break;
		 }
      }
   }
   while (FindNextFile(pFile, &ffd) != 0);

   if( INVALID_HANDLE_VALUE != pFile )
	{
		FindClose(pFile);
	}
 
   DWORD dwError = GetLastError();
   if (dwError != ERROR_NO_MORE_FILES) 
   {
	   PrintErrorMessage(L"Browse directory test failed. Return error:",dwError);
	   ret = FALSE;
   }
   else
   {
		PrintPassedMessage(L"\nBrowseDirectoryControlTest passed.\n\n");
		ret = TRUE;
    }
	
	return ret;
}


BOOL
IncludeExcludeProcessNameTest(FilterControl* filterControl)
{
	BOOL ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	//Test Remove ALLOW_OPEN_WITH_WRITE_ACCESS for current process 
	ULONG accessFlag = (~ALLOW_OPEN_WITH_WRITE_ACCESS)&ALLOW_MAX_RIGHT_ACCESS;
	ULONG currentPid = GetCurrentProcessId();

	fileFilter.AccessFlag = accessFlag;
	fileFilter.AddIncludeProcessId(currentPid);

	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	HANDLE pFile = CreateFile(testFile,GENERIC_WRITE,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
	  PrintErrorMessage(L"IncludeExcludeProcessNameTest test failed.",0);
	  return FALSE;
	}
	else
	{
		DWORD dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"IncludeExcludeProcessNameTest test failed.",dwError);
		  return FALSE;
		}
		PrintPassedMessage(L"IncludeExcludeProcessNameTest test passed.\n");
	}

	return ret;
}



BOOLEAN
IncludeExcludeUserNameTest(FilterControl* filterControl)
{
	BOOLEAN ret = FALSE;

	FileFilterRule fileFilter(filterMask);
	//Test Remove ALLOW_OPEN_WITH_WRITE_ACCESS for current process 
	ULONG accessFlag = (~ALLOW_OPEN_WITH_WRITE_ACCESS)&ALLOW_MAX_RIGHT_ACCESS;
		
	ULONG userNameSize = MAX_PATH;
	WCHAR userName[MAX_PATH];
	
	if( 0 == GetUserName(userName,&userNameSize) )
	{
		 PrintErrorMessage(L"GetUserName test failed.",GetLastError());
		 return FALSE;
	}

	fileFilter.AccessFlag = accessFlag;
	fileFilter.AddIncludeUserName(userName);

	
	filterControl->ClearConfigData();
	filterControl->SendFileFilterRuleToFilter(&fileFilter);

	HANDLE pFile = CreateFile(testFile,GENERIC_WRITE,NULL,NULL,OPEN_EXISTING,FILE_ATTRIBUTE_NORMAL,NULL);
	if( INVALID_HANDLE_VALUE != pFile )
	{
		PrintErrorMessage(L"IncludeExcludeUserNameTest test failed.",0);
		return FALSE;

	}
	else
	{
		DWORD dwError = GetLastError();	

		if( ERROR_ACCESS_DENIED != dwError )
		{
		  PrintErrorMessage(L"IncludeExcludeUserNameTest test failed.",dwError);
		   return FALSE;

		}

		PrintPassedMessage(L"IncludeExcludeUserNameTest test passed.\n");
	}


	return ret;
}

void 
FilterUnitTest()
{
   
	if( !SetupTestEnvironment())
	{
		PrintErrorMessage(L"Failed to setup test environment.",0);
		return;
	}

	FilterControl* filterControl = FilterControl::GetSingleInstance();

	ULONG filterType = FILE_SYSTEM_CONTROL|FILE_SYSTEM_MONITOR|FILE_SYSTEM_ENCRYPTION|FILE_SYSTEM_PROCESS;
	ULONG threadCount = 5;
	ULONG connectionTimeout = 20;
	
	filterControl->StartFilter(filterType,threadCount,connectionTimeout,registerKey);
	
	Sleep(5000);
	
	IncludeExcludeProcessNameTest(filterControl);

	IncludeExcludeUserNameTest(filterControl);

	ReparseFilterRuleTest(filterControl);

	WriteControlFilterTest(filterControl);

	AccessFlagControlTest(filterControl);

	ReadControlFilterTest(filterControl);

	QueryInformationControlTest(filterControl);

	SetInformationControlTest(filterControl);
	
	BrowseDirectoryControlTest(filterControl);

	////this the demo how to use the encryption filter.
	EncryptionUnitTest(filterControl);

	system("pause");

	filterControl->StopFilter();
	delete filterControl;
}																		    