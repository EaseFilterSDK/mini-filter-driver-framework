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

//the IO callback IRP name.
WCHAR* IOCommandName[] = {
    L"IOPreFileCreate",
    L"IOPostFileCreate",
    L"IOPreFileRead",
    L"IOPostFileRead",
    L"IOPreFileWrite",
    L"IOPostFileWrite",
    L"IOPreQueryFileSize",
    L"IOPostQueryFileSize",
    L"IOPreQueryFileBasicInfo",
    L"IOPostQueryFileBasicInfo",
    L"IOPreQueryFileStandardInfo",
    L"IOPostQueryFileStandardInfo",
    L"IOPreQueryFileNetworkInfo",
    L"IOPostQueryFileNetworkInfo",
    L"IOPreQueryFileId",
    L"IOPostQueryFileId",
    L"IOPreQueryFileInfo",
    L"IOPostQueryFileInfo",
    L"IOPreSetFileSize",
    L"IOPostSetFileSize",
    L"IOPreSetFileBasicInfo",
    L"IOPostSetFileBasicInfo",
    L"IOPreSetFileStandardInfo",
    L"IOPostSetFileStandardInfo",
    L"IOPreSetFileNetworkInfo",
    L"IOPostSetFileNetworkInfo",
    L"IOPreMoveOrRenameFile",
    L"IOPostMoveOrRenameFile",
    L"IOPreDeleteFile",
    L"IOPostDeleteFile",
    L"IOPreSetFileInfo",
    L"IOPostSetFileInfo",
    L"IOPreQueryDirectoryFile",
    L"IOPostQueryDirectoryFile",
    L"IOPreQueryFileSecurity",
    L"IOPostQueryFileSecurity",
    L"IOPreSetFileSecurity",
    L"IOPostSetFileSecurity",
    L"IOPreFileHandleClose",
    L"IOPostFileHandleClose",
    L"IOPreFileClose",
    L"IOPostFileClose",      
};


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
		if( pSendMessage->Status > STATUS_ERROR )
		{
			ChangeColour(FOREGROUND_RED);
		}
		else if ( pSendMessage->Status > STATUS_WARNING )
		{
			ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN);
		}

		VOLUME_INFO* pVolumeInfo = (VOLUME_INFO*)pSendMessage->DataBuffer;
		if(FILTER_SEND_ATTACHED_VOLUME_INFO == pSendMessage->FilterCommand)
		{
			PrintMessage( L"FILTER_SEND_ATTACHED_VOLUME_INFO\nVolumeName:%ws\nVolumeDosName:%ws\nvolume file system type:0x%0x\nDeviceCharacteristics:0x%0x\n\n"
			,pVolumeInfo->VolumeName, pVolumeInfo->VolumeDosName, pVolumeInfo->VolumeFilesystemType, pVolumeInfo->DeviceCharacteristics);
			return;
		}
		else if(FILTER_SEND_DETACHED_VOLUME_INFO == pSendMessage->FilterCommand)
		{
			PrintMessage( L"FILTER_SEND_DETACHED_VOLUME_INFO\nVolumeName:%ws\n VolumeDosName:%ws\nvolume file system type:0x%0x\nDeviceCharacteristics:0x%0x\n\n"
			,pVolumeInfo->VolumeName, pVolumeInfo->VolumeDosName, pVolumeInfo->VolumeFilesystemType, pVolumeInfo->DeviceCharacteristics);
			return;
		}
		else if (FILTER_SEND_DENIED_VOLUME_DISMOUNT_EVENT == pSendMessage->FilterCommand)
        {
            PrintMessage( L"FILTER_SEND_DENIED_VOLUME_DISMOUNT_EVENT\nVolumeName:%ws\n VolumeDosName:%ws\nvolume file system type:0x%0x\nDeviceCharacteristics:0x%0x\n\n"
			,pVolumeInfo->VolumeName, pVolumeInfo->VolumeDosName, pVolumeInfo->VolumeFilesystemType, pVolumeInfo->DeviceCharacteristics);
			return;
        }       
        else if (FILTER_SEND_DENIED_USB_READ_EVENT == pSendMessage->FilterCommand)
        {
            PrintMessage( L"Reading data from USB was blocked.\n\n");
			return;
        }
        else if (FILTER_SEND_DENIED_USB_WRITE_EVENT == pSendMessage->FilterCommand)
        {
            PrintMessage( L"Writting data to USB was blocked.\n\n");
			return;
        }
       
		if(pSendMessage->SidLength > 0 )
		{
			BOOL ret = LookupAccountSid( NULL,
								pSendMessage->Sid,
								userName,
								(LPDWORD)&userNameSize,
								domainName,
								(LPDWORD)&domainNameSize,
								&snu); 
		}				
	
		if(FILTER_SEND_FILE_CHANGED_EVENT == pSendMessage->FilterCommand)
		{
			 if ((pSendMessage->InfoClass & FILE_WAS_CREATED ) > 0)
            {
                PrintMessage( L"New file %ws was created.\n",pSendMessage->FileName);
            }

            if ((pSendMessage->InfoClass & FILE_WAS_WRITTEN ) > 0)
            {
                PrintMessage( L"File %ws was written.\n",pSendMessage->FileName);
            }

            if ((pSendMessage->InfoClass & FILE_WAS_DELETED ) > 0)
            {
                PrintMessage( L"File %ws was deleted.\n",pSendMessage->FileName);
            }

            if ((pSendMessage->InfoClass & FILE_INFO_CHANGED ) > 0)
            {
				PrintMessage( L"File %ws information was changed.\n",pSendMessage->FileName);
            }

            if ((pSendMessage->InfoClass & FILE_WAS_RENAMED ) > 0)
            {
                if (pSendMessage->DataBufferLength > 0)
                {
                    PrintMessage( L"Rename file %ws to newname %ws\n"
					,pSendMessage->FileName,pSendMessage->DataBuffer);
                }

            }

            if ((pSendMessage->InfoClass & FILE_SECURITY_CHANGED ) > 0)
            {
               PrintMessage( L"File %ws security was changed.\n",pSendMessage->FileName);
            }

			return;
		}
		else if (FILTER_SEND_DENIED_FILE_IO_EVENT == pSendMessage->FilterCommand)
        {
			//if the global boolean config flag ENABLE_SEND_DENIED_EVENT was enabled and the I/O was blocked by filter driver, the denied message will be sent to here.
			PrintMessage( L"IO type:0x%0x to file %ws was blocked by the filter driver with the access flag in filter rule setting.\n",pSendMessage->MessageType, pSendMessage->FileName);
			return;
        }
		else if (FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT == pSendMessage->FilterCommand)
        {
            PrintMessage(L"Teminate the process was blocked by the filter driver.");
			return;
        }

		ULONG commandNameIndex = pSendMessage->FilterCommand - IOPreFileCreate;
		WCHAR* ioName = IOCommandName[commandNameIndex];		

		switch( pSendMessage->MessageType )
		{			
			case PRE_CREATE:
			case POST_CREATE:
			{
				//for Disposition,ShareAccess,DesiredAccess,CreateOptions Please reference Winddows API CreateFile
				//http://msdn.microsoft.com/en-us/library/aa363858%28v=vs.85%29.aspx

				WCHAR* deleteOnCloseInfo = L"";
				if ((pSendMessage->CreateOptions & FILE_DELETE_ON_CLOSE) > 0)
				{
					//the file will be deleted after the file handle was closed.
					deleteOnCloseInfo = L",the file will be deleted on handle closed.";
				}

				//SendMessage->CreateStatus is create status,it is only valid in post create,the possible value is:
				WCHAR* createStatus[] = { L"FILE_SUPERSEDED",L"FILE_OPENED",L"FILE_CREATED",L"FILE_OVERWRITTEN",L"FILE_EXISTS",L"FILE_DOES_NOT_EXIST"};
				WCHAR*	createStatusName = L"";

				if(POST_CREATE == pSendMessage->MessageType && pSendMessage->CreateStatus < 6)
				{					
					createStatusName = createStatus[pSendMessage->CreateStatus];
				}

				PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFileSize:%I64d Attributes:%0x %ws %ws\n"
								,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
								,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
								,pSendMessage->FileSize,pSendMessage->FileAttributes,deleteOnCloseInfo,createStatusName);
			   
			   break;
  			}

			case PRE_QUERY_INFORMATION:
			case POST_QUERY_INFORMATION:				
			case PRE_SET_INFORMATION:
			case POST_SET_INFORMATION:
			{		
				 //FltQueryInformationFile API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff543439%28v=vs.85%29.aspx
				 //FltSetInformationFile API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff544516%28v=VS.85%29.aspx

				 //for POST_QUERY_INFORMATION request, the pSendMessage->DataBuffer contains the data which returned from the file system.
				 //for PRE_SET_INFORMATION request, the pSendMessage->DataBuffer contains the data which will write down to the file system.

				switch(pSendMessage->FilterCommand)
				{
				case IOPreQueryFileSize:
				case IOPostQueryFileSize:
				case IOPreSetFileSize:
				case IOPostSetFileSize:
					{
						//Query or set file information with class FileEndOfFileInformation
						ULONGLONG fileSize = *((ULONGLONG*)pSendMessage->DataBuffer);
						PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFileEndOfFileInformation=%d file size:%d\n"
								,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
								,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
								,pSendMessage->InfoClass,fileSize);
						break;
					}
				case IOPreQueryFileBasicInfo:
				case IOPostQueryFileBasicInfo:
				case IOPreSetFileBasicInfo:
				case IOPostSetFileBasicInfo:
					{
						//Query or set file information with class FileBasicInformation
						FILE_BASIC_INFORMATION fileBasicInfo = *((FILE_BASIC_INFORMATION*)pSendMessage->DataBuffer);
						PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFileBasicInformation=%d creationTime:%I64d lastWriteTime:%I64d attribute:0x%0x\n"
								,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
								,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
								,pSendMessage->InfoClass,fileBasicInfo.CreationTime,fileBasicInfo.LastWriteTime,fileBasicInfo.FileAttributes);

						break;
					}
				case IOPreSetFileStandardInfo:
				case IOPostSetFileStandardInfo:
				case IOPreQueryFileStandardInfo:
				case IOPostQueryFileStandardInfo:
					{
						//Query or set file information with class FileStandardInformation
						FILE_STANDARD_INFORMATION fileStandardInfo = *((FILE_STANDARD_INFORMATION*)pSendMessage->DataBuffer);
						PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFileStandardInformation=%d AllocationSize:%I64d fileSize:%I64d\n"
								,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
								,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
								,pSendMessage->InfoClass,fileStandardInfo.AllocationSize,fileStandardInfo.EndOfFile);

						break;
					}
				case IOPreQueryFileNetworkInfo:
				case IOPostQueryFileNetworkInfo:
				case IOPreSetFileNetworkInfo:
				case IOPostSetFileNetworkInfo:
					{
						//Query or set file information with class FileNetworkInformation
						FILE_NETWORK_OPEN_INFORMATION fileNetworkInfo = *((FILE_NETWORK_OPEN_INFORMATION*)pSendMessage->DataBuffer);
						PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFileNetworkInformation=%d creationTime:%I64d lastWriteTime:%I64d attribute:0x%0,AllocationSize:%I64d fileSize:%I64d\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
							,pSendMessage->InfoClass,fileNetworkInfo.CreationTime,fileNetworkInfo.LastWriteTime,fileNetworkInfo.FileAttributes,fileNetworkInfo.AllocationSize,fileNetworkInfo.EndOfFile);

						break;
					}
				case IOPreMoveOrRenameFile:
				case IOPostMoveOrRenameFile:
					{
						//Set file information with class FileRenameInformation
						ULONG newFileNameLength = pSendMessage->DataBufferLength;
						WCHAR* newFileName = (WCHAR*)pSendMessage->DataBuffer;

						PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFile was renamed to %ws\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status,newFileName);

						break;
					}

				case IOPreDeleteFile:
				case IOPostDeleteFile:
					{
						//Set file information with class FileDispositionInformation
						PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFile was deleted.\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status);

						break;
					}
				default:
					{
						//Set the other file information class
						PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nFileInformation Class = %d\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status,pSendMessage->InfoClass);

						break;
					}

				}
				 
				 break;
			}

			case PRE_QUERY_SECURITY:
			case POST_QUERY_SECURITY:
			case PRE_SET_SECURITY:
			case POST_SET_SECURITY:
			{
				 //FltQuerySecurityObject API,http://msdn.microsoft.com/en-us/library/windows/hardware/ff543441%28v=vs.85%29.aspx
				 //FltSetSecurityObject API,http://msdn.microsoft.com/en-us/library/ff544538

				PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\nSecurityInformation Class = %d\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status,pSendMessage->InfoClass);

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
				//for POST_DIRECTORY request, the pSendMessage->DataBuffer contains the data which return from the file system.

				PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\n FileInformationClass Class = %d\n DirectoryBuffer:%s\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status,pSendMessage->InfoClass,pSendMessage->DataBuffer);
				
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

				PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\n Offset = %I64d Length = %d returnLength = %d\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
							,pSendMessage->Offset,pSendMessage->Length,pSendMessage->DataBufferLength);

			   //If you want to get the read data, you need to enable ENABLE_SEND_DATA_BUFFER flag in global boolean config setting.
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

				PrintMessage( L"\nId#%d IOName:%ws UserName:%ws\\%ws  \nFileName:%ws\nProcessId:%d ThreadId:%d Return Status:%0x\n Offset = %I64d Length = %d returnLength = %d\n"
							,pSendMessage->MessageId,ioName,domainName,userName,pSendMessage->FileName
							,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status
							,pSendMessage->Offset,pSendMessage->Length,pSendMessage->DataBufferLength);

			   //If you want to get the write data, you need to enable ENABLE_SEND_DATA_BUFFER flag in global boolean config setting.
			   //display the write data to file system.
			   //printf("data:%s",pSendMessage->DataBuffer);    //it is ansi code characters
			   //wprintf("data:%ws",pSendMessage->DataBuffer);	//it is unicode characters

			   //for pre write request, the pSendMessage->DataBuffer contains the data which will write down to the file system.
				
				break;
			}	

			default: break;
		}
		

	}
	__finally
	{
		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);  
	}


return ;

}
