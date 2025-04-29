#ifndef __REPARSEFILE_H__
#define __REPARSEFILE_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

BOOL
ReparseFilterHandler(
	IN		PMESSAGE_SEND_DATA		messageSend,
	IN OUT	PMESSAGE_REPLY_DATA		messageReply)
{
	//this is reparse file filter handler.                        

	BOOL retVal = FALSE;

	ReparseFileEventArgs* reparseEventArgs = new ReparseFileEventArgs(messageSend);

	if (reparseEventArgs->isNewCreatedFile)
	{
		//this is new created file to request the tag data,
		//the new file will be created with the reparse point tag data.							

		//if you want to block the new file creation, return  STATUS_ACCESS_DENIED
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		//if you want to return the encryption key and iv for new created file, then return  STATUS_SUCCESS
		messageReply->ReturnStatus = STATUS_SUCCESS;

		//for new created file, you can set your own custom tag data to the header.
		//here we put the file name as the tag data for test purpose.

		messageReply->ReplyData.AESData.Data.TagDataLength = messageSend->FileNameLength;
		memcpy(messageReply->ReplyData.AESData.Data.TagData, messageSend->FileName, messageSend->FileNameLength);

		//the total return size
		messageReply->ReplyData.AESData.SizeOfData = sizeof(messageReply->ReplyData.AESData.Data) + messageReply->ReplyData.AESData.Data.TagDataLength;

		wprintf(L"New created file :%ws is requesting reparse point tag data, return status:%0x\n", messageSend->FileName, messageReply->ReturnStatus);

	}
	else 
	{
		//opening the existing reparse point file.

		//here is the custom tag data which was embedded in the reparse point tag.
		ULONG tagDataLength = reparseEventArgs->tagDataLength;
		UCHAR* tagData = reparseEventArgs->tagData.data();

		//if you want to block the file open, return  STATUS_ACCESS_DENIED
		messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		//if you want to decrypt the file, then return status success, and the encryption key and iv.
		//messageReply->ReturnStatus = STATUS_SUCCESS;

		wprintf(L"Reparse point file :%ws is requesting file open, the reparse point tag data %ws, return status:%0x\n", messageSend->FileName, (WCHAR*)tagData, messageReply->ReturnStatus);

	}

	return retVal;
}


#endif