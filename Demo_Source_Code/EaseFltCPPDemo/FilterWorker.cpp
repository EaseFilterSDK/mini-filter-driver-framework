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
		if ( pSendMessage->MessageType == FILTER_REQUEST_ENCRYPTION_IV_AND_KEY 
			|| pSendMessage->MessageType == FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA)
		{
			ret = ProcessEncryptionRequest(pSendMessage,pReplyMessage);
		}
		else
		{
			ret = ProcessControlFilter(pSendMessage,pReplyMessage);
		}
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
SendConfigInfoToFilter(ULONG FilterType,WCHAR* FilterFolder,ULONG IoRegistration 
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

	//test control filter to exclude the file access 
	//accessFlag = ALLOW_MAX_RIGHT_ACCESS | EXCLUDE_FILE_ACCESS;

	if( FilterFolder )
	{
		if( !AddNewFilterRule(accessFlag,FilterFolder))
		{
			PrintLastErrorMessage(L"AddFilterRule failed.");
			return;
		}
		
		if( (accessFlag&ENABLE_FILE_ENCRYPTION_RULE) > 0)
		{
			if( keyLength > 0)
			{
				if(ivLength >0 )
				{
					//Set an encryption folder, all encrypted files use the same encryption key and IV key. 
					//this is mostly for the test purpose.

					AddEncryptionKeyAndIVToFilterRule(FilterFolder,keyLength,encryptionKey,ivLength,iv);
				}
				else
				{
					//Set an encryption folder, every encrypted file has the unique iv key, the encrypted information was embedded into to the file as the header, 
					//The same folder can mix encrypted files and unencrypted files, the filter driver will know if the file was encrypted by checking the header.
					//If you want to copy the encrytped file with crypted data, you need to unauthorize the copy process, for example 'explorer.exe', or you will get the clear data.
					AddEncryptionKeyToFilterRule(FilterFolder,keyLength,encryptionKey);
				}
			}
			else
			{
				//with this setting, to open or create encrypted file, it will request the encryption key and iv from the user mode callback service.
				
				AddBooleanConfigToFilterRule(FilterFolder, REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE);
				
			}
		}


	}


	//Exclude the process Id from the filter.
 /*   if(!AddExcludedProcessId(GetCurrentProcessId()))
	{
		PrintLastErrorMessage(L"AddExcludedProcessId failed.");
		return;
	}*/
	
	//Only include process id will be watched,all others processes will be excluded.
	//AddIncludedProcessId(GetCurrentProcessId());
	
	if(!RegisterIoRequest(IoRegistration))
	{
		PrintLastErrorMessage(L"RegisterIoRequest failed.");
		return;
	}
}


