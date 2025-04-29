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
RegistryNotificationCallback(
   IN		PMESSAGE_SEND_DATA pSendMessage,
   IN OUT	PMESSAGE_REPLY_DATA pReplyMessage)
{

	BOOL	ret = TRUE;
	WCHAR userName[MAX_PATH];
	WCHAR domainName[MAX_PATH];

	int userNameSize = MAX_PATH;
	int domainNameSize = MAX_PATH;
	SID_NAME_USE snu;

	ULONGLONG regCallbackClass = pSendMessage->Offset;

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

		wprintf( L"\nId#%d UserName:%ws\\%ws\nProcessId:%d ThreadId:%d Return Status:%0x\n",pSendMessage->MessageId,domainName,userName,pSendMessage->ProcessId,pSendMessage->ThreadId,pSendMessage->Status);

		 if (pSendMessage->MessageType != FILTER_SEND_REG_CALLBACK_INFO )
         {
			 wprintf(L"MessageType:%d is not FILTER_SEND_REG_CALLBACK_INFO.\n",pSendMessage->MessageType);
			 return ret;
		 }

		wprintf(L"regCallbackClass:%I64d RegistryKeyName:%ws\n",regCallbackClass,pSendMessage->FileName);



		ChangeColour(FOREGROUND_RED|FOREGROUND_GREEN|FOREGROUND_BLUE);

		if(pReplyMessage)
		{
		   //you can deny the registry access by return access denied status here.
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
RegDisconnectCallback()
{
	printf("Filter connection was disconnected.\n");
	return;
}



void 
StartRegFilterTest()
{

	ULONG threadCount = 5;

	//Set the filter maximum wait time for response from the user mode call back function.
	SetConnectionTimeout(10);       

	if(!RegisterMessageCallback(threadCount,RegistryNotificationCallback,RegDisconnectCallback))
	{
		PrintLastErrorMessage( L"RegisterMessageCallback failed.");
		return ;
	}

	ULONG filterType = FILE_SYSTEM_REGISTRY; 
    ULONGLONG regCallbackClass = 93092006832128; 
    SetFilterType(filterType);

	AddRegistryFilterRule(2,L"*",0,2,L"*",0,NULL,REG_MAX_ACCESS_FLAG,regCallbackClass,FALSE, 0);


}


