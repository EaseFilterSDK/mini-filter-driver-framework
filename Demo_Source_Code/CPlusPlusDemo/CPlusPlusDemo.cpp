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

// CPlusPlusDemo.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include "Tools.h"
#include "FilterAPI.h"
#include "WindowsService.h"
#include "FilterControl.h"
#include <Sddl.h>

#include "TestData.h"

#define	MAX_ERROR_MESSAGE_SIZE	1024

#define PrintMessage	wprintf //ToDebugger

//set the include directory for this libary to folder Bin\x64  or Bin\Win32
#pragma comment(lib, "FilterAPI.lib")


VOID
Usage (
    VOID
    )
/*++

Routine Description

    Prints usage

Arguments

    None

Return Value

    None

--*/
{
	ULONG computerId = GetComputerId();

	wprintf(L"\nComputerId:%d\n\n\n",computerId );
	
	printf( "\nUsage:		EaseFltCPPDemo  [command] <FilterFolder> <IoRegistration> <AccessFlag>\n" );
	printf( "\nCommands:\n" );
	printf( "		i ----- Install Driver\n" );
	printf( "		u ----- UnInstall Driver\n" );
	printf( "		t ----- Driver UnitTest\n" );
	printf( "		m ----- Start monitor filter.\n" );
	printf( "		c ----- Start control filter.\n" );
	printf( "		d ----- Start DRM encryption filter driver test.\n" );
	printf("		e ----- Start encryption filter driver test.\n");
	printf( "		p ----- Start process filter driver test.\n" );
	printf( "		r ----- Start registry filter driver test.\n" );
	printf( "\n		[FilterFolder]---- the folder mask which will be monitored.\n" );
	printf( "		[IoRegistration]---- register the callback I/O requests.\n" );
	printf( "		[AccessFlag]---- the I/O access control flag.\n" );
	printf( "\r\nExample: \nEaseFltCPPDemo i   ----- Install Driver\r\n" ); 
	printf( "EaseFltCPPDemo u   ----- UnInstall Driver\r\n" ); 
	printf( "EaseFltCPPDemo t   ----- Driver UnitTest\r\n" ); 
	printf( "EaseFltCPPDemo p * 16128 c:\\filterTest\\* 22554420----- run process filter driver: processFilterMask controlFlag fileFilterMask accessFlag.\r\n" );
	printf( "EaseFltCPPDemo d c:\\filterTest\\* notepad.exe;wordpade.xe  ----- encrypt file with DRM data embedding, authorized process notepad and wordpad.\r\n");
	printf( "EaseFltCPPDemo e c:\\filterTest\\*  ----- encrypt filter driver with default settings.\r\n" );
	printf( "EaseFltCPPDemo m c:\\filterTest\\*  ----- monitor filter driver with default settings.\r\n" );
	printf( "EaseFltCPPDemo c c:\\filterTest\\*  ----- control filter driver with default settings.\r\n" );
	printf( "EaseFltCPPDemo c c:\\filterTest\\*  0 33393264   ---control filter driver, prevent files from being changed.\r\n" );
	printf( "EaseFltCPPDemo P *(processNameFilterMask) controlFlag ---process filter driver to monitor/control process activities.\r\n");
	printf( "EaseFltCPPDemo r *(processNameFilterMask) *(keyNameFilterMask)  controlFlag ---registry filter driver to monitor/control registry activities.\r\n");
	
}


int _tmain(int argc, _TCHAR* argv[])
{
    DWORD	threadCount = 5;
	DWORD   connectionTimeout = 20; //SECONDS
	ULONG	filterType = FILE_SYSTEM_MONITOR;
	BOOL	ret = FALSE;
	
	PrintPassedMessage(L"test Install filter driver succeeded!");

	if(argc <= 1)
	{
		Usage();
		return 1;
	}	

	TCHAR op = *argv[1];

	EnableDebugPrivileges();

	PrintPassedMessage(L"before Install filter driver succeeded!");

	FilterControl* filterControl = FilterControl::GetSingleInstance();

	//
	PrintPassedMessage(L"after Install filter driver succeeded!");

	switch(op)
	{
	
			/*How to run as windows service, do the following steps:
			1.Create windows service:  sc create filterService binpath=  c:\easefilter\x64\cplusplusdemo.exe w,
									   replace the path with your own path,Note: binpath=(space)binarypath
			2. cplusplusdemo.exe i  //install the driver manully. 
			3. Sc start easefilter  //start the windows service.
			*/
		case 'w':  //start windows service for the monitor filter
				  {
					  StartWindowsService();
					  break;
				  }

		case 'i': //install driver
		{
			//Install the driver only once.
			//After the installation, you can use command "fltmc unload EaseFilter" to unload the driver
			//or "fltmc load EaseFilter" to load the driver, or "fltmc" to check the status of the driver.

			ret = UnInstallDriver();
			Sleep(2000);

			ret = InstallDriver();	
			if( !ret )
			{
				PrintLastErrorMessage( L"InstallDriver failed.");
				return 1;
			}

			PrintPassedMessage(L"Install filter driver succeeded!");

			break;
		}

		case 'u': //uninstall driver
		{
			ret = UnInstallDriver();
			if( !ret )
			{
				PrintLastErrorMessage( L"UnInstallDriver failed.");
				break;
			}

			PrintPassedMessage(L"UnInstallDriver succeeded!");

			break;

		}		

		case 'c': filterType = FILE_SYSTEM_CONTROL; //start control filter		
		case 'm':
		{
			//To monitor filter:
			//you can register the file changed events to get the notification when the file was created, renamed, deleted, written and changed.
			//you only can register the post I/O to get the notification after the I/O was processed by the file system.

			//To control Filter:
			//You can allow or deny the file I/O by setting the access flags in the filter rule.
			//You can register the pre-I/O to get the callback in user mode application before the I/O goes down to the file system,
			//in your callback function you can allow or deny the I/O.
			//You can register the post I/O to get the callback in user mode application after the I/O was returned from the file system,
			//in your callback function you can modify the returned I/O data.

			if( !IsDriverServiceRunning())
			{
				ret = InstallDriver();	
				if( !ret )
				{
					PrintLastErrorMessage( L"InstallDriver failed.");
					return 1;
				}

				PrintPassedMessage(L"Install filter driver succeeded!");
			}


			ULONGLONG allPostIO = POST_CREATE|POST_FASTIO_READ|POST_CACHE_READ|POST_NOCACHE_READ|POST_PAGING_IO_READ;
				allPostIO |= POST_FASTIO_WRITE|POST_CACHE_WRITE|POST_NOCACHE_WRITE|POST_PAGING_IO_WRITE|POST_QUERY_INFORMATION;
				allPostIO |= POST_SET_INFORMATION|POST_DIRECTORY|POST_QUERY_SECURITY|POST_SET_SECURITY|POST_CLEANUP|POST_CLOSE;

			WCHAR* fileFilterMask = L"c:\\test\\*";
			ULONGLONG ioCallbackClass = allPostIO;
			ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS;		

			if (argc >= 3)
			{
				fileFilterMask = argv[2];			
			}
			
			//To get the I/O callback registration class, you can check the IOCallbackClass for your reference.
			if( argc >= 4 )
			{
				ioCallbackClass = std::stoul (argv[3],nullptr,16);
			}
			
			if( argc >= 5 )
			{
				accessFlag = std::stoul (argv[4],nullptr,10);
			}					

			FileFilterRule fileFilterRule(fileFilterMask);
			fileFilterRule.AccessFlag = accessFlag;

			//block the new file read/rename/delete/write in the filter driver
			//fileFilterRule.AccessFlag = ALLOW_MAX_RIGHT_ACCESS & (~(ALLOW_READ_ACCESS |ALLOW_FILE_RENAME|ALLOW_FILE_DELETE|ALLOW_WRITE_ACCESS));

			fileFilterRule.BooleanConfig = ENABLE_MONITOR_EVENT_BUFFER;
			fileFilterRule.FileChangeEventFilter = FILE_WAS_CREATED|FILE_WAS_WRITTEN|FILE_WAS_RENAMED|FILE_WAS_DELETED|FILE_WAS_READ;
			//fileFilterRule.MonitorFileIOEventFilter = ioCallbackClass;
		
			//you can filter the file I/O only opens with this option( file is going to be created.)
			//fileFilterRule.FilterDisposition = FILE_CREATE | FILE_OPEN_IF | FILE_OVERWRITE | FILE_OVERWRITE_IF;

			if( filterType == FILE_SYSTEM_CONTROL)
			{
				//if you want to hide the files, you need to enable this flag.
				//fileFilterRule.AccessFlag |= ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING;
				//hide the txt files from the directory.
				//fileFilterRule.AddHiddenFileFilterMask(L"*.txt");

				//if you want to reparse the file open, you need to enable this flag.
				//fileFilterRule.AccessFlag |= ENABLE_REPARSE_FILE_OPEN;
                //FilterMask = c:\test\*txt
                //ReparseFilterMask = d:\reparse\*doc
                //If you open file c:\test\MyTest.txt, it will reparse to the file d:\reparse\MyTest.doc.
				//fileFilterRule.ReparseFileFilterMask= L" d:\\reparse\*doc";

				//get the control callback I/O request, you can block the I/O request in the pre-IO
				fileFilterRule.ControlFileIOEventFilter = ioCallbackClass;

				//You can allow/block the file rename/delete in the callback handler by register PRE_RENAME_FILE|PRE_DELETE_FILE.
				/*ULONGLONG preIOCallbackClass = PRE_RENAME_FILE | PRE_DELETE_FILE;
				fileFilterRule.ControlFileIOEventFilter = preIOCallbackClass;*/

				//disable the file being renamed, deleted and written access rights.
				ULONG processAccessRights = accessFlag & (~(ALLOW_FILE_RENAME|ALLOW_FILE_DELETE|ALLOW_WRITE_ACCESS));
				//set this new access rights to process cmd, cmd can't rename,delete or write to the file.
				//this feature requires the process filter driver feature, it need to enable the process filter driver.
				filterType |= FILE_SYSTEM_PROCESS;
				fileFilterRule.AddAccessRightsToProcessName(L"cmd.exe", processAccessRights);
			}

			filterControl->AddFileFilter(fileFilterRule);

			//Register the volume event notification with volume control setting.
			//you can block the USB read with flag BLOCK_USB_READ or write with flag BLOCK_USB_WRITE.
			//filterControl->VolumeControlSettings = BLOCK_USB_READ|BLOCK_USB_WRITE;

			//you can get the block message notification by the access flag with below setting.
			//set global boolean config
			filterControl->globalBooleanConfig |= ENABLE_SEND_DENIED_EVENT;

			if (!filterControl->StartFilter(filterType, threadCount, connectionTimeout, registerKey))
			{
				break;
			}

			if (op == 'm')
			{
				_tprintf(_T("Start Monitor filter, fileFilterMask=%s  ioCallbackClass:0X%0X accessFlag:0X%0X \n\n Press any key to stop.\n"), fileFilterMask, ioCallbackClass, accessFlag);
			}
			else
			{
				_tprintf(_T("Start control filter, fileFilterMask=%s  ioCallbackClass:0X%0X accessFlag:0X%0X \n\n Press any key to stop.\n"), fileFilterMask, ioCallbackClass, accessFlag);
			}
			
			//prevent the current process from being terminated.
			//AddProtectedProcessId(GetCurrentProcessId());

			getchar();

			//the process can be terminated now.
			//RemoveProtectedProcessId(GetCurrentProcessId());

			break;

		}
		case 'd':  //DRM encryption filter driver test
		{

			UnInstallDriver();
			Sleep(2000);

			ret = InstallDriver();
			if (!ret)
			{
				PrintLastErrorMessage(L"InstallDriver failed.");
				break;
			}

			WCHAR* fileFilterMask = L"c:\\test\\*";

			if (argc >= 3)
			{
				fileFilterMask = argv[2];
			}

			if (argc >= 4)
			{
				SetAuthorizedProcess(argv[3]);
			}

			//by default all users/processes will get the decrypted data
			//it meant by default all users/processes are in whitelist
			ULONG accessFlag = (ALLOW_MAX_RIGHT_ACCESS | ENABLE_FILE_ENCRYPTION_RULE);

			filterType = FILE_SYSTEM_ENCRYPTION | FILE_SYSTEM_CONTROL | FILE_SYSTEM_PROCESS;

			FileFilterRule fileFilterRule(fileFilterMask);
			fileFilterRule.AccessFlag = accessFlag;

			//if we enable the encryption key from service, you can authorize the decryption for every file
			//in the callback function OnFilterRequestEncryptKey, with this flag enabled.
			fileFilterRule.BooleanConfig |= REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE;

			//Add the FILE_ATTRIBUTE_ENCRYPTED to the encrypted file. The attribute will be kept if you copy&paste to another folder
			//even the file was not encrypted anymore by Windows explorer.
			//fileFilterRule.BooleanConfig |= ENABLE_SET_FILE_ATTRIBUTE_ENCRYPTED;

			filterControl->AddFileFilter(fileFilterRule);
			
			if (!filterControl->StartFilter(filterType, threadCount, connectionTimeout, registerKey))
			{
				break;
			}			

			_tprintf(_T("\n\nStart DRM Encryption for folder %ws,\r\nThe new created file will be encrypted, the encrypted file can be decrypted automatically in this folder\
				.\r\n Press any key to stop the filter driver.\n"),fileFilterMask);

			system("pause");
			getchar();

			break;

		}
		case 'e':  //encryption filter driver test
		{

			UnInstallDriver();
			Sleep(2000);

			ret = InstallDriver();
			if (!ret)
			{
				PrintLastErrorMessage(L"InstallDriver failed.");
				break;
			}

			WCHAR* fileFilterMask = L"c:\\test\\*";

			if (argc >= 3)
			{
				fileFilterMask = argv[2];
			}

			//by default all users/processes will get the raw encrypted data
			//it meant by default all users/processes are in blacklist
			ULONG accessFlag = (ALLOW_MAX_RIGHT_ACCESS | ENABLE_FILE_ENCRYPTION_RULE) & (~ALLOW_READ_ENCRYPTED_FILES);

			//by default all users/processes will get the decrypted data
			//it meant by default all users/processes are in whitelist
			//ULONG accessFlag = (ALLOW_MAX_RIGHT_ACCESS | ENABLE_FILE_ENCRYPTION_RULE);

			filterType = FILE_SYSTEM_ENCRYPTION | FILE_SYSTEM_CONTROL | FILE_SYSTEM_PROCESS;

			FileFilterRule fileFilterRule(fileFilterMask);
			fileFilterRule.AccessFlag = accessFlag;

			//if we enable the encryption key from service, you can authorize the decryption for every file
			//in the callback function OnFilterRequestEncryptKey, with this flag enabled, you don't need to set the encryption key.
			//fileFilterRule.BooleanConfig |= REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE;

			//Add the FILE_ATTRIBUTE_ENCRYPTED to the encrypted file. The attribute will be kept if you copy&paste to another folder
			//even the file was not encrypted anymore by Windows explorer.
			//fileFilterRule.BooleanConfig |= ENABLE_SET_FILE_ATTRIBUTE_ENCRYPTED;

			//if you have a master key, you can set it here, or if you want to get the encryption key from the callback function then don't set the key here.
			//256 bit,32bytes encryption key
			unsigned char key[] = { 0x60,0x3d,0xeb,0x10,0x15,0xca,0x71,0xbe,0x2b,0x73,0xae,0xf0,0x85,0x7d,0x77,0x81,0x1f,0x35,0x2c,0x07,0x3b,0x61,0x08,0xd7,0x2d,0x98,0x10,0xa3,0x09,0x14,0xdf,0xf4 };
			if (!fileFilterRule.set_EncryptionKey(key, sizeof(key)))
			{
				break;
			}

			//set the blacklist of the process, if the default filter rule is whitelist to all users/processes.
			//ULONG rawEncryptionRights = ALLOW_MAX_RIGHT_ACCESS & (~ALLOW_READ_ENCRYPTED_FILES);
			//fileFilterRule.AddAccessRightsToProcessName(L"explorer.exe", rawEncryptionRights,NULL,NULL);

			//set the whitelist for the user "AzureAD\\Alice"
			//fileFilterRule.AddAccessRightsToUserName(L"AzureAD\\Alice", ALLOW_MAX_RIGHT_ACCESS);

			//set the whitelist for the process "wordpad.exe"
			fileFilterRule.AddAccessRightsToProcessName(L"wordpad.exe", ALLOW_MAX_RIGHT_ACCESS, NULL, NULL);

			//set the whitelist for the process "notepad.exe"
			fileFilterRule.AddAccessRightsToProcessName(L"notepad.exe", ALLOW_MAX_RIGHT_ACCESS, NULL, NULL);

			filterControl->AddFileFilter(fileFilterRule);

			if (!filterControl->StartFilter(filterType, threadCount, connectionTimeout, registerKey))
			{
				break;
			}

			_tprintf(_T("\n\nStart Encryption for folder %ws,\r\nThe new created file will be encrypted, the encrypted file can be decrypted automatically in this folder\
				.\r\n Press any key to stop the filter driver.\n"), fileFilterMask);

			system("pause");
			getchar();

			break;

		}
		case 'l':  //reparse file filter driver test
		{

			UnInstallDriver();
			Sleep(2000);

			ret = InstallDriver();
			if (!ret)
			{
				PrintLastErrorMessage(L"InstallDriver failed.");
				return 1;
			}

			_tprintf(_T("Start reparse filter driver test. press any key to stop.\n\n"));

			filterType = FILE_SYSTEM_REGISTRY;

			WCHAR* fileFilterMask = L"c:\\test\\*";

			if (argc >= 3)
			{
				fileFilterMask = argv[2];
			}

			ULONG accessFlag = (ALLOW_MAX_RIGHT_ACCESS | ENABLE_REPARSE_FILE_OPEN);
			filterType = FILE_SYSTEM_REPARSE;

			FileFilterRule fileFilterRule(fileFilterMask);
			fileFilterRule.AccessFlag = accessFlag;

			filterControl->AddFileFilter(fileFilterRule);

			if (!filterControl->StartFilter(filterType, threadCount, connectionTimeout, registerKey))
			{
				break;
			}

			_tprintf(_T("\n\nStart reparse file filter for folder %ws,\r\nThe new created file will be embedded with tag data.\
				.\r\n Press any key to stop the filter driver.\n"), fileFilterMask);

			getchar();

			break;

		}
		case 'p':  //process filter driver test
		{

			UnInstallDriver();
			Sleep(2000);
			
			ret = InstallDriver();	
			if( !ret )
			{
				PrintLastErrorMessage( L"InstallDriver failed.");
				break;
			}

					
			WCHAR* processFilterMask = L"*";
			ULONG controlFlag  = PROCESS_CREATION_NOTIFICATION|PROCESS_TERMINATION_NOTIFICATION|THREAD_CREATION_NOTIFICATION|THREAD_TERMINATION_NOTIFICATION; 

			//if you want to block the application in c:\test folder to be launched, you can use below setting:
			//controlFlag = DENY_NEW_PROCESS_CREATION;
			//processFilterMask = L"c:\\test\\*.exe";

			if( argc >= 3 )
			{
				processFilterMask = argv[2];
			}
			
			if( argc >= 4 )
			{
				controlFlag = std::stoul (argv[3],nullptr,10);
			}

			filterType = FILE_SYSTEM_PROCESS;
			
			ProcessFilterRule processFilterRule(processFilterMask);
			processFilterRule.ControlFlag = controlFlag;

			filterControl->AddProcessFilter(processFilterRule);

			//block the process to write,rename or delete to folder c:\test
			ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS & ~(ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS | ALLOW_WRITE_ACCESS | ALLOW_FILE_RENAME | ALLOW_FILE_DELETE);
			processFilterRule.AddFileAccessRightsToProcess(L"c:\\test\\*", accessFlag);

			if (!filterControl->StartFilter(filterType, threadCount, connectionTimeout, registerKey))
			{
				break;
			}
			
			_tprintf(_T("Start Process filter, processFilterMask=%s  controlFlag:0X%0X \n\n Press any key to stop.\n"), processFilterMask, controlFlag);

			getchar();

			break;

		}
		case 'r':  //registry filter driver test
		{

			UnInstallDriver();
			Sleep(2000);
			
			ret = InstallDriver();	
			if( !ret )
			{
				PrintLastErrorMessage( L"InstallDriver failed.");
				return 1;
			}

			_tprintf(_T("Start registry filter driver test. press any key to stop.\n\n"));

			filterType = FILE_SYSTEM_REGISTRY; 			

			WCHAR* processNameFilterMask = L"*";
			WCHAR* keyNameFilterMask = L"*";

			if (argc >= 3)
			{
				processNameFilterMask = argv[2];
			}

			if (argc >= 4)
			{
				keyNameFilterMask = argv[3];
			}

			ULONG controlFlag = REG_MAX_ACCESS_FLAG;
			if (argc >= 5)
			{
				controlFlag = std::stoul(argv[5], nullptr, 10);
			}

			RegistryFilterRule registryFilterRule(processNameFilterMask);
			
			registryFilterRule.RegistryKeyNameFilterMask = keyNameFilterMask;

			//you can block the registry key being renamed/deleted/created.
			//controlFlag = REG_MAX_ACCESS_FLAG & (~(REG_ALLOW_RENAME_KEY | REG_ALLOW_DELETE_KEY | REG_ALLOW_CREATE_KEY));

			registryFilterRule.ControlFlag = controlFlag;
			registryFilterRule.RegCallbackClass = Max_Reg_Callback_Class;

			filterControl->AddRegistryFilter(registryFilterRule);

			if (!filterControl->StartFilter(filterType, threadCount, connectionTimeout, registerKey))
			{
				break;
			}

			_tprintf(_T("Start Registry filter, processNameFilterMask=%s keyNameFilterMask=%s controlFlag:0X%0X \n\n Press any key to stop.\n")
				,processNameFilterMask, keyNameFilterMask, controlFlag);
								
			getchar();

			break;

		}
		default:
			{
				Usage(); 
				return 1;
			}


	}

	filterControl->StopFilter();
	delete filterControl;
		
	return 0;
}

