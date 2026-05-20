#ifndef __STUBFILE_H__
#define __STUBFILE_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

BOOL
DownloadBlockOfData( WCHAR* fileName, LONGLONG readOffset, ULONG readLength, PULONG returnLength, UCHAR* buffer)
{

	HANDLE hFile = CreateFile(
		fileName,
		GENERIC_READ,
		FILE_SHARE_READ | FILE_SHARE_WRITE,
		NULL,
		OPEN_EXISTING,
		NULL,
		NULL);

	if (hFile == INVALID_HANDLE_VALUE)
	{
		DWORD lastError = GetLastError();
		wprintf(L"DownloadBlockOfData CreateFile  :%ws return error code:%d\n", (WCHAR*)fileName, lastError);
		PrintErrorMessage(L"CreateFile failed.", lastError);
		return FALSE;
	}

	BOOL ret = ReadFile(hFile, buffer, readLength, returnLength, NULL);

	CloseHandle(hFile);

	return ret;
}

BOOL
StubFileFilterHandler(
	IN		PMESSAGE_SEND_DATA		messageSend,
	IN OUT	PMESSAGE_REPLY_DATA		messageReply)
{
	//this is reparse file filter handler.                        

	BOOL retVal = FALSE;
	BOOL isDownloadWholeFileToCache = FALSE;
	BOOL isDownloadBlockOfData = FALSE;

	StubFileEventArgs* stubFileEventArgs = new StubFileEventArgs(messageSend);

	if (messageSend->FilterCommand == MESSAGE_TYPE_RESTORE_FILE_TO_CACHE)
	{
		//When the file is written, renamed, or its information is changed,
		//or the file was opened for reading with memory mapping. 
		isDownloadWholeFileToCache = TRUE;
	}
	else if (messageSend->FilterCommand == MESSAGE_TYPE_RESTORE_BLOCK_OR_FILE)
	{
		isDownloadBlockOfData = TRUE;

	/*	there are three available options to return the data when the block of data is requested:
			1.	Return the requested block of data.
			2.	Return the cache file name if you want to download the whole file to cache folder.
			3.	Return the rehydrate status to rehydrate the stub file.
			
			if you want to choose option 2 or 3, then set:
			downloadWholeFileToCache = TRUE;
			*/
	}

	//here is the custom tag data which was embedded in the header of the stub file.
	//for the test, tag data is the test source file name path.
	ULONG tagDataLength = stubFileEventArgs->tagDataLength;
	UCHAR* tagData = stubFileEventArgs->tagData.data();

	if (isDownloadWholeFileToCache)
	{
		messageReply->FilterStatus = FILTER_CACHE_FILE_WAS_RETURNED;
		//if you want to rehydrate the file on first read, then return status as below:
		//messageReply->FilterStatus = FILTER_REHYDRATE_FILE_VIA_CACHE_FILE;

		messageReply->ReturnStatus = STATUS_SUCCESS;

		messageReply->ReplyData.Data.DataBufferLength = tagDataLength;
		memcpy(messageReply->ReplyData.Data.DataBuffer, tagData, tagDataLength);

		wprintf(L"Return cache file :%ws filterStatus:0x%0x, return status:0x%0x\n", (WCHAR*)tagData, messageReply->FilterStatus, messageReply->ReturnStatus);
	}
	else if(isDownloadBlockOfData)
	{
		messageReply->FilterStatus = FILTER_BLOCK_DATA_WAS_RETURNED;
		messageReply->ReturnStatus = STATUS_SUCCESS;

		if (!DownloadBlockOfData((WCHAR*)tagData, stubFileEventArgs->offset, stubFileEventArgs->length, &messageReply->ReplyData.Data.DataBufferLength, messageReply->ReplyData.Data.DataBuffer))
		{
			messageReply->ReturnStatus = STATUS_ACCESS_DENIED;
		}

		wprintf(L"Process stub file :%ws block data return, return buffer Length:%d, return status:%0x\n"
			,messageSend->FileName, messageReply->ReplyData.Data.DataBufferLength, messageReply->ReturnStatus);

	}

	return retVal;
}

#endif