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
#include "FilterAPI.h"
#include "UnitTest.h"

#define PrintMessage	wprintf //ToDebugger

//
//Here displays the I/O information from filter driver for monitor filter driver and control filter driver
//For every I/O callback data, you always can get this information:
//
//The file related information: file name,file size, file attributes, file time.
//The user information who initiated the I/O: user name, user SID.
//The process information which initiated the I/O: process name, process Id, thread Id.
//The I/O result for post I/O requests: sucess code or the error code.


VOID
DisplayFilterMessageInfo( IN	PMESSAGE_SEND_DATA pSendMessage )
{
	WCHAR userName[MAX_PATH];
	WCHAR domainName[MAX_PATH];

	int userNameSize = MAX_PATH;
	int domainNameSize = MAX_PATH;
	SID_NAME_USE snu;

	__try
	{
		
		BOOL ret = LookupAccountSid( NULL,
									pSendMessage->Sid,
									userName,
									(LPDWORD)&userNameSize,
									domainName,
									(LPDWORD)&domainNameSize,
									&snu); 
	
		if( pSendMessage->Status > STATUS_ERROR )
		{
			ChangeColour(FOREGROUND_RED);
		}
		else if ( pSendMessage->Status > STATUS_WARNING )
		{
			ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN);
		}

		VOLUME_INFO* pVolumeInfo = (VOLUME_INFO*)pSendMessage->DataBuffer;
		if(FILTER_SEND_ATTACHED_VOLUME_INFO == pSendMessage->MessageType)
		{
			PrintMessage( L"FILTER_SEND_ATTACHED_VOLUME_INFO\n VolumeName:%ws\nVolumeDosName:%ws\nvolume file system type:0x%0x\nDeviceCharacteristics:0x%0x\n\n"
			,pVolumeInfo->VolumeName, pVolumeInfo->VolumeDosName, pVolumeInfo->VolumeFilesystemType, pVolumeInfo->DeviceCharacteristics);
			return;
		}
		else if(FILTER_SEND_DETACHED_VOLUME_INFO == pSendMessage->MessageType)
		{
			PrintMessage( L"FILTER_SEND_DETACHED_VOLUME_INFO\nVolumeName:%ws\n VolumeDosName:%ws\nvolume file system type:0x%0x\nDeviceCharacteristics:0x%0x\n\n"
			,pVolumeInfo->VolumeName, pVolumeInfo->VolumeDosName, pVolumeInfo->VolumeFilesystemType, pVolumeInfo->DeviceCharacteristics);
			return;
		}

		
		PrintMessage( L"\nId# %d MessageType:0X%0x UserName:%ws\\%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFileSize:%I64d Attributes:%0x FileName:%ws\n"
			,pSendMessage->MessageId,pSendMessage->MessageType,domainName,userName
			,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
			,pSendMessage->FileSize,pSendMessage->FileAttributes,pSendMessage->FileName);


		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);
	
		switch( pSendMessage->MessageType )
		{			
			case PRE_CREATE:
			case POST_CREATE:
			{
				//for Disposition,ShareAccess,DesiredAccess,CreateOptions Please reference Winddows API CreateFile
				//http://msdn.microsoft.com/en-us/library/aa363858%28v=vs.85%29.aspx

				PrintMessage( L"CreateRequest DesiredAccess=%d Disposition=%d ShareAccess=%d CreateOptions=0x%0x CreateStatus = %d fileName:%ws\n"
					,pSendMessage->DesiredAccess,pSendMessage->Disposition,pSendMessage->ShareAccess,pSendMessage->CreateOptions,pSendMessage->CreateStatus,pSendMessage->FileName);

			
				//SendMessage->CreateStatus is create status,it is only valid in post create,the possible value is:
				//FILE_SUPERSEDED = 0x00000000,
				//FILE_OPENED = 0x00000001,
				//FILE_CREATED = 0x00000002,
				//FILE_OVERWRITTEN = 0x00000003,
				//FILE_EXISTS = 0x00000004,
				//FILE_DOES_NOT_EXIST = 0x00000005,
				

				////here demo how to open the file which was opening by user for read or write
				//HANDLE  hFile = INVALID_HANDLE_VALUE;

				////open the same file handle,it will bypass the share check.
				//ret = GetFileHandleInFilter(pSendMessage->FileName,GENERIC_READ|GENERIC_WRITE,&hFile);

				//if(!ret)
				//{
				//	PrintLastErrorMessage(L"GetFileHandleInFilter failed.");
				//	break;
				//}
				//else
				//{
				//	PrintMessage( L"Get File Hanle:%p\n"
				//	,hFile);

				//}

				//if( INVALID_HANDLE_VALUE != hFile)
				//{
				//	CloseHandle(hFile);
				//}
			   
			   break;
  			}

			case PRE_QUERY_INFORMATION:
			case POST_QUERY_INFORMATION:
			case PRE_SET_INFORMATION:
			case POST_SET_INFORMATION:
			{
				 //FltQueryInformationFile API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff543439%28v=vs.85%29.aspx
				 //FltSetInformationFile API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff544516%28v=VS.85%29.aspx
				PrintMessage( L"Query/Set information request FileInformationClass = %d oldName:%ws newname:%ws\n"
					,pSendMessage->InfoClass,pSendMessage->FileName,pSendMessage->DataBuffer);

				 //for POST_QUERY_INFORMATION request, the pSendMessage->DataBuffer contains the data which return from the file system.
				 //for PRE_SET_INFORMATION request, the pSendMessage->DataBuffer contains the data which will write down to the file system.
				 
				 break;
			}

			case PRE_QUERY_SECURITY:
			case POST_QUERY_SECURITY:
			case PRE_SET_SECURITY:
			case POST_SET_SECURITY:
			{
				 //FltQuerySecurityObject API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff543441%28v=vs.85%29.aspx
				 //FltSetSecurityObject API,http://msdn.microsoft.com/en-us/library/ff544538
				 PrintMessage( L"Query/Set information request SecurityInformation  = %d \n",pSendMessage->InfoClass);

			/*	LPWSTR strDacl;
				ULONG length = 0;
				BOOL ret = ConvertSecurityDescriptorToStringSecurityDescriptor(pSendMessage->DataBuffer, SDDL_REVISION_1,DACL_SECURITY_INFORMATION, &strDacl, &length);
				DWORD errorCode = 0;
				if(!ret)
				{
					errorCode = GetLastError();
				}
				PrintMessage( L"ret:%d %ws length:%d errorCode:%d\n",ret,strDacl,length,errorCode);
				LocalFree(strDacl);*/

				 //for POST_QUERY_SECURITY request, the pSendMessage->DataBuffer contains the data which return from the file system.
				 //for PRE_SET_SECURITY request, the pSendMessage->DataBuffer contains the data which will write down to the file system.

				 break;
			}


			case PRE_DIRECTORY:
			case POST_DIRECTORY:
			{
				//FltQueryDirectoryFile API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff543433%28v=vs.85%29.aspx
				PrintMessage( L"Browse directory request, DataBuffer:%0x FileInformationClass  = %d \n"	,pSendMessage->DataBuffer,pSendMessage->InfoClass);

				//for POST_DIRECTORY request, the pSendMessage->DataBuffer contains the data which return from the file system.

				 break;
			}

			case PRE_FASTIO_READ:
			case POST_FASTIO_READ:
			case PRE_CACHE_READ:
			case POST_CACHE_READ: 
			case PRE_NOCACHE_READ:
			case POST_NOCACHE_READ:
			case PRE_PAGING_IO_READ:
			case POST_PAGING_IO_READ:
			{
				 //FltReadFile API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff544286%28v=vs.85%29.aspx
				PrintMessage( L"Id#:%d Read requst, Offset = %I64d Length = %d returnLength = %d  \n"
				   ,pSendMessage->MessageId,pSendMessage->Offset,pSendMessage->Length,pSendMessage->DataBufferLength);
			   
			   //display the read data return from file system.
			   //printf("data:%s",pSendMessage->DataBuffer);    //it is ansi code characters
			   //wprintf("data:%ws",pSendMessage->DataBuffer);	//it is unicode characters

				//for post read request, the pSendMessage->DataBuffer contains the data which return from the file system.

			   break;
			}

			case PRE_FASTIO_WRITE:
			case POST_FASTIO_WRITE:
			case PRE_CACHE_WRITE:
			case POST_CACHE_WRITE: 
			case PRE_NOCACHE_WRITE:
			case POST_NOCACHE_WRITE:
			case PRE_PAGING_IO_WRITE:
			case POST_PAGING_IO_WRITE:
			{
				 //FltWriteFile API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff544610%28v=vs.85%29.aspx
				PrintMessage( L"WRITE requst, Offset = %I64d Length = %d returnLength = %d \n"
				   ,pSendMessage->Offset,pSendMessage->Length,pSendMessage->DataBufferLength);
				
			   //display the write data to file system.
			   //printf("data:%s",pSendMessage->DataBuffer);    //it is ansi code characters
			   //wprintf("data:%ws",pSendMessage->DataBuffer);	//it is unicode characters

			   //for pre write request, the pSendMessage->DataBuffer contains the data which will write down to the file system.
				
				break;
			}	

			default: break;
		}

	}
	__except( EXCEPTION_EXECUTE_HANDLER  )
	{
		PrintErrorMessage( L"DisplayFilterMessageInfo failed.",GetLastError());     
	}

return ;

}
