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


BOOL
__stdcall
ProcessNotificationCallback(
   IN		PMESSAGE_SEND_DATA messageSend,
   IN OUT	PMESSAGE_REPLY_DATA pReplyMessage)
{

	BOOL	ret = TRUE;
	WCHAR userName[MAX_PATH];
	WCHAR domainName[MAX_PATH];

	int userNameSize = MAX_PATH;
	int domainNameSize = MAX_PATH;
	SID_NAME_USE snu;

	PPROCESS_INFO 	pSendMessage = (PPROCESS_INFO)messageSend->DataBuffer;

	ULONG callbackType = (ULONG)messageSend->MessageType;

	__try
	{
		
		BOOL ret = LookupAccountSid( NULL,
									messageSend->Sid,
									userName,
									(LPDWORD)&userNameSize,
									domainName,
									(LPDWORD)&domainNameSize,
									&snu); 
	
		switch( callbackType )
		{
		case FILTER_SEND_PROCESS_CREATION_INFO:
			   wprintf( L"\r\nCreating New Process\n");
			   break;
		case FILTER_SEND_PROCESS_TERMINATION_INFO:
			   wprintf( L"\r\nProcess termniation\n");
			   break;
		case FILTER_SEND_THREAD_CREATION_INFO:
			   wprintf( L"\nThread creation\n");
			   break;
		case FILTER_SEND_THREAD_TERMINATION_INFO:
			   wprintf( L"\nThread termiation\n");
			   break;
		case FILTER_SEND_PROCESS_HANDLE_INFO:
			   wprintf( L"\nProcess handle operation\n");
			   break;
		case FILTER_SEND_THREAD_HANDLE_INFO:
			   wprintf( L"\nThread handle operation\n");
			   break;
		}

		 wprintf( L"Id#%d UserName:%ws\\%ws\nProcessId:%d ThreadId:%d callbackType:%0x ImageFileName:%ws\n"
			 ,messageSend->MessageId,domainName,userName,messageSend->ProcessId,messageSend->ThreadId,callbackType,messageSend->FileName);


		if(pReplyMessage && messageSend->MessageType == FILTER_SEND_PROCESS_CREATION_INFO )
		{
		    //you can deny the new process creation by return access denied status here.
			//pReplyMessage->FilterStatus = (uint)FILTER_COMPLETE_PRE_OPERATION;
			//pReplyMessage->ReturnStatus = (uint)STATUS_ACCESS_DENIED;
		}
	
	}
	__except( EXCEPTION_EXECUTE_HANDLER  )
	{
		PrintErrorMessage( L"DisplayFilterMessageInfo failed.",GetLastError());     
	}

	return ret;
}

VOID
__stdcall
PsDisconnectCallback()
{
	printf("Filter connection was disconnected.\n");
	return;
}



void 
StartProcessFilterTest(WCHAR* ProcessFilterMask,ULONG ControlFlag,WCHAR* FileFilterMask ,ULONG AccessFlag)
{

	ULONG threadCount = 5;

	//Set the filter maximum wait time for response from the user mode call back function.
	SetConnectionTimeout(10);       

	if(!RegisterMessageCallback(threadCount,ProcessNotificationCallback,PsDisconnectCallback))
	{
		PrintLastErrorMessage( L"RegisterMessageCallback failed.");
		return ;
	}
  
    SetFilterType(FILE_SYSTEM_PROCESS|FILE_SYSTEM_CONTROL);

	//set the control flag of the process which the process name matchs the ProcessFilterMask;
	AddProcessFilterRule((ULONG)wcslen(ProcessFilterMask)*sizeof(WCHAR),ProcessFilterMask,ControlFlag);

	//control the file access which the file name matchs the FileFilterMask for process which the process name matchs the ProcessFilterMask; 
	AddFileControlToProcessByName((ULONG)wcslen(ProcessFilterMask)*sizeof(WCHAR),ProcessFilterMask, (ULONG)wcslen(FileFilterMask)*sizeof(WCHAR), FileFilterMask , AccessFlag);

	printf("Add process filter mask %ws controlFlag:%d fileFilterMask:%ws file access flag:%d.\n",ProcessFilterMask,ControlFlag,FileFilterMask , AccessFlag);
}


