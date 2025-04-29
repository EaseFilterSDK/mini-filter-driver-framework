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
#include "FilterMessage.h"
#include "FilterWorker.h"
#include "AESEncryption.h"
#include "ControlFilterHandler.h"


BOOL
__stdcall
MessageCallback(
   IN		PMESSAGE_SEND_DATA pSendMessage,
   IN OUT	PMESSAGE_REPLY_DATA pReplyMessage)
{

	BOOL	ret = TRUE;

	DisplayFilterMessageInfo(pSendMessage);

	if(pReplyMessage)
	{
		if ( pSendMessage->FilterCommand == FILTER_REQUEST_ENCRYPTION_IV_AND_KEY 
			|| pSendMessage->FilterCommand == FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA)
		{
			ret = EncryptionRequestHandler(pSendMessage,pReplyMessage);
		}
		else
		{
			ret = IOControlHandler(pSendMessage,pReplyMessage);
		}

	}
	else
	{
		//this is notification request.
	}

	return ret;
}

VOID
__stdcall
DisconnectCallback()
{
	printf("Filter connection was disconnected.\n");
	return;
}



void 
SendConfigInfoToFilter(ULONG FilterType,WCHAR* FilterFolder,ULONGLONG IoRegistration 
	,ULONG AccessFlag,UCHAR* encryptionKey,ULONG keyLength,UCHAR* iv,ULONG ivLength)
{

	//Set the filter maximum wait time for response from the user mode call back function.
	SetConnectionTimeout(30);       

	//Set the filter type to file system monitor/call back filter.
    SetFilterType(FilterType);

	//Setup the filter rule,filter mask is the folder to be monitored,
	//accessFlag is the I/O access control of the folder, only affect callback filter,
	//reparseMask is the destination folder for reparse file open, only when the accessFlag = REPARSE_FILE_OPEN.

	ULONG accessFlag = AccessFlag;

	//test control filter to block the file open 
	//accessFlag = ALLOW_MAX_RIGHT_ACCESS | EXCLUDE_FILE_ACCESS;

	if( FilterFolder )
	{
		if( !AddFileFilterRule(accessFlag,FilterFolder))
		{
			PrintLastErrorMessage(L"AddFilterRule failed.");
			return;
		}

		//you can exclude the .exe file from the filter
		/*if( !AddExcludeFileMaskToFilterRule(FilterFolder,L"*.exe"))
		{
			PrintLastErrorMessage(L"AddExcludeFileMaskToFilterRule failed.");
			return;
		}*/

		if( !AddFileFilterRule(accessFlag,FilterFolder))
		{
			PrintLastErrorMessage(L"AddFilterRule failed.");
			return;
		}

		if( (FilterType&FILE_SYSTEM_MONITOR) > 0 )
		{
			//the monitor filter is enabled.
			ULONG fileChangedEvents = FILE_WAS_CREATED|FILE_WAS_WRITTEN|FILE_WAS_RENAMED|FILE_WAS_DELETED|FILE_SECURITY_CHANGED|FILE_INFO_CHANGED;

			if(!RegisterFileChangedEventsToFilterRule(FilterFolder,fileChangedEvents))
			{
				PrintLastErrorMessage(L"RegisterFileChangedEventsToFilterRule failed.");
			}

			if(!RegisterMonitorIOToFilterRule(FilterFolder,IoRegistration))
			{
				PrintLastErrorMessage(L"RegisterMonitorIOToFilterRule failed.");
			}
		}

		if( (FilterType&FILE_SYSTEM_CONTROL) > 0 )
		{
			//the control filter is enabled.
			if(!RegisterControlIOToFilterRule(FilterFolder,IoRegistration))
			{
				PrintLastErrorMessage(L"RegisterControlIOToFilterRule failed.");
			}
		}
		
		if( (accessFlag&ENABLE_FILE_ENCRYPTION_RULE) > 0)
		{
			if( keyLength > 0)
			{
				if(ivLength >0 )
				{
					//Set an encryption folder, all encrypted files use the same encryption key and IV key. 
					//this is for test purpose.

					AddEncryptionKeyAndIVToFilterRule(FilterFolder,keyLength,encryptionKey,ivLength,iv);
				}
				else
				{
					//Set an encryption folder, every encrypted file has the unique new generated iv key, the iv will be embedded into the header of the encrypted file, 
					//The same folder can mix encrypted files and unencrypted files, the filter driver will know if the file was encrypted by checking the header.
					AddEncryptionKeyToFilterRule(FilterFolder,keyLength,encryptionKey);
				}
			}
			else
			{
				//With this setting, if a new created file, it will request encryption key and IV from user mode service.
				//You can set your custom tag data to the header of the encrypted file.
				//When the user opens an encrytped file, it will request the encryption key and iv from user mode service,
				//If the tag data was set in the header of the encrypted file, you will get the tag data in the data buffer.

				AddBooleanConfigToFilterRule(FilterFolder, REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE);
				
			}
		}


	}


	//Register the volume event notification, get current all attached volume information,
	//you can block the USB read with flag BLOCK_USB_READ or write with flag BLOCK_USB_WRITE.
	//get notifcation when the filter driver attached the volume,
	//get notification when the filter detached the volume
	//if(!SetVolumeControlFlag(GET_ATTACHED_VOLUME_INFO|VOLUME_ATTACHED_NOTIFICATION|VOLUME_DETACHED_NOTIFICATION))
	//{
	//	PrintLastErrorMessage(L"SetVolumeControlFlag failed.");
	//}

	//Exclude the current process from the filter.
    if(!AddExcludedProcessId(GetCurrentProcessId()))
	{
		PrintLastErrorMessage(L"AddExcludedProcessId failed.");
		return;
	}

	//set global boolean config
	ULONG globalBooleanConfig = ENABLE_SEND_DENIED_EVENT;

	if(!SetBooleanConfig(globalBooleanConfig))
	{
		PrintLastErrorMessage(L"SetBooleanConfig failed.");
		return;
	}

	
	//Only include process id will be watched,all others processes will be excluded.
	//AddIncludedProcessId(GetCurrentProcessId());
	
}


