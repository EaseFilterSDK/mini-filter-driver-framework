#ifndef __FILTER_WORKER_H__
#define __FILTER_WORKER_H__

VOID
__stdcall
DisconnectCallback();

BOOL
__stdcall
MessageCallback(
   IN		PMESSAGE_SEND_DATA pSendMessage,
   IN OUT	PMESSAGE_REPLY_DATA pReplyMessage);

void 
SendConfigInfoToFilter(ULONG FilterType,WCHAR* FilterFolder,ULONGLONG IoRegistration 
	,ULONG AccessFlag,UCHAR* encryptionKey = NULL,ULONG keyLength = 0,UCHAR* iv =NULL,ULONG ivLength =  0);

#endif