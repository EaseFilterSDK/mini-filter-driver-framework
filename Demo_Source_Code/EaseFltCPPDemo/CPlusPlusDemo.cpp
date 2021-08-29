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
#include "UnitTest.h"
#include "AESEncryption.h"
#include "WindowsService.h"
#include "FilterWorker.h"
#include "RegistryFilterTest.h"
#include "ProcessFilterTest.h"
#include <Sddl.h>

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
	WCHAR	computerId[26];
	ULONG	bufferLength = sizeof(computerId);

	if( GetUniqueComputerId((BYTE*)computerId,&bufferLength) )
	{
		wprintf(L"\nComputerId:%ws\n\n\n",computerId );
	}
	else
	{
		PrintLastErrorMessage( L"GetUniqueComputerId failed.");
		printf( "\n\n" );
	}

	printf( "\nUsage:		EaseFltCPPDemo  [command] <FilterFolder> <IoRegistration> <AccessFlag>\n" );
	printf( "\nCommands:\n" );
	printf( "		i ----- Install Driver\n" );
	printf( "		u ----- UnInstall Driver\n" );
	printf( "		t ----- Driver UnitTest\n" );
	printf( "		m ----- Start monitor filter.\n" );
	printf( "		c ----- Start control filter.\n" );
	printf( "		e ----- Start encryption filter driver test.\n" );
	printf( "		p ----- Start process filter driver test.\n" );
	printf( "		r ----- Start registry filter driver test.\n" );
	printf( "\n		[FilterFolder]---- the folder mask which will be monitored.\n" );
	printf( "		[IoRegistration]---- register the callback I/O requests.\n" );
	printf( "		[AccessFlag]---- the I/O access control flag.\n" );
	printf( "\r\nExample: \nEaseFltCPPDemo i   ----- Install Driver\r\n" ); 
	printf( "EaseFltCPPDemo u   ----- UnInstall Driver\r\n" ); 
	printf( "EaseFltCPPDemo t   ----- Driver UnitTest\r\n" ); 
	printf( "EaseFltCPPDemo p * 16128 c:\\filterTest\\* 22554420----- run process filter driver: processFilterMask controlFlag fileFilterMask accessFlag.\r\n" );
	printf( "EaseFltCPPDemo e c:\\filterTest\\*  ----- encrypt filter driver with default settings.\r\n" );
	printf( "EaseFltCPPDemo m c:\\filterTest\\*  ----- monitor filter driver with default settings.\r\n" );
	printf( "EaseFltCPPDemo c c:\\filterTest\\*  ----- control filter driver with default settings.\r\n" );
	printf( "EaseFltCPPDemo c c:\\filterTest\\*  0 33393264   ---control filter driver, prevent files from being changed.\r\n" );
	
}


int _tmain(int argc, _TCHAR* argv[])
{
    DWORD	threadCount = 5;
	ULONG	filterType = FILE_SYSTEM_MONITOR;
	BOOL	ret = FALSE;
	
	if(argc <= 1)
	{
		Usage();
		return 1;
	}
	
	TCHAR op = *argv[1];


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
				return 1;
			}

			PrintPassedMessage(L"UnInstallDriver succeeded!");

			break;

		}

		case 't': //driver unit test 
		{
			
			ret = UnInstallDriver();
			Sleep(2000);

			ret = InstallDriver();	
			if( !ret )
			{
				PrintLastErrorMessage( L"InstallDriver failed.");
				return 1;
			}

			PrintPassedMessage(L"Install filter driver succeeded!\n");

			ret = SetRegistrationKey(registerKey);
			if( !ret )
			{
				PrintLastErrorMessage( L"SetRegistrationKey failed.");
				return 1;
			}

			ret = RegisterMessageCallback(threadCount,MessageCallback,DisconnectCallback);

			if( !ret )
			{
				PrintLastErrorMessage( L"RegisterMessageCallback failed.");
				return 1;
			}

			//this is the demo how to use filter driver SDK.
			FilterUnitTest();


			Disconnect();

			break;

		}

		case 'c': filterType = FILE_SYSTEM_CONTROL; //start control filter		
		case 'm':
		{

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


			WCHAR* filterFolder = GetFilterMask();
			ULONG ioRegistration = 0;
			ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS;		
			BOOL addMessageToFile = FALSE;

			if( argc >= 3 )
			{
				filterFolder = argv[2];
			}
			
			if( argc >= 4 )
			{
				ioRegistration = std::stoul (argv[3],nullptr,16);
			}
			else
			{
				//Register the I/O request,which will be monitored or will be called back from filter.
				for (int i = 0; i < MAX_REQUEST_TYPE; i++ )
				{
					//register all post request
					if( (double)i/2 != i/2 )
					{
						ioRegistration |= 1<<i;
					}
				}     
			}

			if( argc >= 5 )
			{
				accessFlag = std::stoul (argv[4],nullptr,10);
			}

			if( argc >= 6 )
			{
				if( _ttoi(argv[5]) > 0)
				{
					addMessageToFile = TRUE;

				}
			}

				
			_tprintf(_T("Start Monitor %s  ioregistration:0X%0X accessFlag:0X%0X addMessageToFile:%d\n\n Press any key to stop.\n")
				,filterFolder,ioRegistration,accessFlag,addMessageToFile);


			 //Reset all filter confing setting.
			ResetConfigData();

			ret = SetRegistrationKey(registerKey);
			if( !ret )
			{
				PrintLastErrorMessage( L"SetRegistrationKey failed.");
				return 1;
			}

			ret = RegisterMessageCallback(threadCount,MessageCallback,DisconnectCallback);

			if( !ret )
			{
				PrintLastErrorMessage( L"RegisterMessageCallback failed.");
				return 1;
			}


			//this the demo how to use the control filter.
			SendConfigInfoToFilter(filterType,filterFolder,ioRegistration,accessFlag);

			//prevent the current process from being terminated.
			//AddProtectedProcessId(GetCurrentProcessId());

			//exclude current process IO from filter driver.
			AddExcludedProcessId(GetCurrentProcessId());

			//Register the volume event notification, get current all attached volume information,
			//get notifcation when the filter driver attached the volume,
			//get notification when the filter detached the volume
			SetVolumeControlFlag(GET_ATTACHED_VOLUME_INFO|VOLUME_ATTACHED_NOTIFICATION|VOLUME_DETACHED_NOTIFICATION);		

			system("pause");

			//the process can be termiated now.
			//RemoveProtectedProcessId(GetCurrentProcessId());

			Disconnect();

			break;

		}
		case 'e':  //encryption filter driver test
		{

			UnInstallDriver();
			Sleep(2000);
			
			ret = InstallDriver();	
			if( !ret )
			{
				PrintLastErrorMessage( L"InstallDriver failed.");
				return 1;
			}

			WCHAR* filterFolder = GetFilterMask();
			ULONG ioRegistration = 0;
			ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS|ENABLE_FILE_ENCRYPTION_RULE;			
			
			filterType = FILE_SYSTEM_ENCRYPTION;	

			if( argc >= 3 )
			{
				filterFolder = argv[2];
			}			

			//Reset all filter confing setting.
			ResetConfigData();

			ret = SetRegistrationKey(registerKey);
			if( !ret )
			{
				PrintLastErrorMessage( L"SetRegistrationKey failed.");
				return 1;
			}

			ret = RegisterMessageCallback(threadCount,MessageCallback,DisconnectCallback);

			if( !ret )
			{
				PrintLastErrorMessage( L"RegisterMessageCallback failed.");
				return 1;
			}

			//prevent the current process from being terminated.
			//AddProtectedProcessId(GetCurrentProcessId());

			// Initialization 16 bytes vector
			unsigned char iv[] = {0xf0,0xf1,0xf2,0xf3,0xf4,0xf5,0xf6,0xf7,0xf8,0xf9,0xfa,0xfb,0xfc,0xfd,0xfe,0xff};
			//256 bit,32bytes encrytpion key
			unsigned char key[] = {0x60,0x3d,0xeb,0x10,0x15,0xca,0x71,0xbe,0x2b,0x73,0xae,0xf0,0x85,0x7d,0x77,0x81,0x1f,0x35,0x2c,0x07,0x3b,0x61,0x08,0xd7,0x2d,0x98,0x10,0xa3,0x09,0x14,0xdf,0xf4};
	
			//this will enable encryption for all files in filterFolder.
			SendConfigInfoToFilter(filterType,filterFolder,ioRegistration,accessFlag,key,sizeof(key),iv,sizeof(iv));		

			_tprintf(_T("\n\nStart Encryption for folder %s,\r\nAll new created files in this folder will be encrypted by filter driver. When filter driver is stopped, the encrypted files can't be read. \n\n Press any key to stop the filter driver.\n"),filterFolder);
			system("pause");

			//the process can be termiated now.
			//RemoveProtectedProcessId(GetCurrentProcessId());

			Disconnect();

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
				return 1;
			}

			ret = SetRegistrationKey(registerKey);
			if( !ret )
			{
				PrintLastErrorMessage( L"SetRegistrationKey failed.");
				return 1;
			}

			
			TCHAR* processFilterMask = L"*";
			ULONG controlFlag  = PROCESS_CREATION_NOTIFICATION|PROCESS_TERMINATION_NOTIFICATION|THREAD_CREATION_NOTIFICATION|THREAD_TERMINIATION_NOTIFICATION; 
			TCHAR* fileFilterMask = GetFilterMask();
			ULONG accessFlag = ALLOW_MAX_RIGHT_ACCESS;	

			if( argc >= 3 )
			{
				processFilterMask = argv[2];
			}
			
			if( argc >= 4 )
			{
				controlFlag = std::stoul (argv[3],nullptr,10);
			}
			
			if( argc >= 5 )
			{
			   fileFilterMask = argv[4];
			}
			
			if( argc >= 6 )
			{
				accessFlag = std::stoul (argv[5],nullptr,10);
			}

			StartProcessFilterTest(processFilterMask,controlFlag,fileFilterMask,accessFlag);

			
			_tprintf(_T("Start process filter driver test.\n\n"));

			getchar();

			system("pause");
			

			Disconnect();

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

			ret = SetRegistrationKey(registerKey);
			if( !ret )
			{
				PrintLastErrorMessage( L"SetRegistrationKey failed.");
				return 1;
			}


			StartRegFilterTest();

			_tprintf(_T("Start registry filter driver test.\n\n"));
			system("pause");


			Disconnect();

			break;

		}
		default:
			{
				Usage(); 
				return 1;
			}


	}

		
	return 0;
}

