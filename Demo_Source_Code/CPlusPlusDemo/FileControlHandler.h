#ifndef __FILECONTROL_H__
#define __FILECONTROL_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

bool ichar_equals(wchar_t a, wchar_t b)
{
	return std::tolower(static_cast<wchar_t>(a)) ==
		std::tolower(static_cast<wchar_t>(b));
}


bool iequals(const std::wstring& a, const std::wstring& b)
{
	return std::equal(a.begin(), a.end(), b.begin(), b.end(), ichar_equals);
}


BOOL 
FileControlHandler( 
	IN		PMESSAGE_SEND_DATA		messageSend,
	IN OUT	PMESSAGE_REPLY_DATA		messageReply )
{
	BOOL retVal = TRUE;

	messageReply->ReturnStatus = STATUS_SUCCESS;

	//The filter driver will block and wait for the response.

	//you can block the PRE-IO request with below setting.
	//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
	//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

	if (messageSend->MessageType == PRE_CREATE)
	{
		FileCreateEventArgs* fileCreateEventArgs = new FileCreateEventArgs(messageSend);

		fileCreateEventArgs->EventName = L"OnPreCreateFile";
		
		//if (fileCreateEventArgs->Disposition != FILE_OPEN)
		//{
		//	////It is going to create the new file here if the file doesn't exist,you can block it here.
		//	//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//	//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		//	//wprintf(L"Creating file %ws was blocked in callback handler.", messageSend->FileName);

		//	//return TRUE;
		//}

		if (fileCreateEventArgs->isDeleteOnClose)
		{
			//the file will be deleted on the file close.
			fileCreateEventArgs->EventName = L"OnPreCreateFile-DeleteFileOnClose";

			//you can block the file deletion here if the file was open with delete on close, you also need to block the delete in PRE_SET_INFORMATION FileDispositionInformation class.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;
		}

		DisplayFileIOMessage(fileCreateEventArgs);

		//you can reparse the file open with below setting
		//if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
		//{
		//	//
		//	//This is a test case for demo only
		//	//For reparse file/folder open test.
		//	//
		//	//the file name must be unicode string, file name length must be less than MAX_PATH.
		//	memcpy(messageReply->ReplyData.Data.DataBuffer,GetTestReparseFileName(),(ULONG)wcslen(GetTestReparseFileName())*sizeof(WCHAR));
		//	messageReply->ReplyData.Data.DataBufferLength =(ULONG)wcslen(GetTestReparseFileName())*sizeof(WCHAR);
		//	   
		//	messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
		//	messageReply->ReturnStatus = STATUS_REPARSE;
		//}

		//you can block the file open/create.
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		delete fileCreateEventArgs;

	}
	else if (messageSend->MessageType == POST_CREATE )
	{
		FileCreateEventArgs* fileCreateEventArgs = new FileCreateEventArgs(messageSend);

		fileCreateEventArgs->EventName = L"OnPostCreateFile";

		if (fileCreateEventArgs->isDeleteOnClose)
		{
			fileCreateEventArgs->EventName = L"OnPostCreateFile-DeleteFileOnClose";
		}

		DisplayFileIOMessage(fileCreateEventArgs);

		delete fileCreateEventArgs;

	}
	else if (messageSend->MessageType == PRE_CACHE_READ
			|| messageSend->MessageType == PRE_FASTIO_READ
			|| messageSend->MessageType == PRE_NOCACHE_READ
			|| messageSend->MessageType == PRE_PAGING_IO_READ)
	{
		FileReadEventArgs* fileReadEventArgs = new FileReadEventArgs(messageSend);
		fileReadEventArgs->EventName = L"OnPreFileRead-" + fileReadEventArgs->readType;

		DisplayFileIOMessage(fileReadEventArgs);

		//you can block the file read request here.
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		delete fileReadEventArgs;
	}
	else if (messageSend->MessageType == POST_CACHE_READ
		|| messageSend->MessageType == POST_FASTIO_READ
		|| messageSend->MessageType == POST_NOCACHE_READ
		|| messageSend->MessageType == POST_PAGING_IO_READ)
	{
		FileReadEventArgs* fileReadEventArgs = new FileReadEventArgs(messageSend);
		fileReadEventArgs->EventName = L"OnPostFileRead-" + fileReadEventArgs->readType;

		DisplayFileIOMessage(fileReadEventArgs);
		
	}
	else if (messageSend->MessageType == PRE_CACHE_WRITE
			|| messageSend->MessageType == PRE_FASTIO_WRITE
			|| messageSend->MessageType == PRE_NOCACHE_WRITE
			|| messageSend->MessageType == PRE_PAGING_IO_WRITE)
	{

		FileWriteEventArgs* fileWriteEventArgs = new FileWriteEventArgs(messageSend);
		fileWriteEventArgs->EventName = L"OnPreFileWrite-" + fileWriteEventArgs->writeType;

		DisplayFileIOMessage(fileWriteEventArgs);

		//you can block the file write request here.
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		delete fileWriteEventArgs;

	}
	else if (messageSend->MessageType == POST_CACHE_WRITE
	|| messageSend->MessageType == POST_FASTIO_WRITE
	|| messageSend->MessageType == POST_NOCACHE_WRITE
	|| messageSend->MessageType == POST_PAGING_IO_WRITE)
	{
		
		FileWriteEventArgs* fileWriteEventArgs = new FileWriteEventArgs(messageSend);
		fileWriteEventArgs->EventName = L"OnPostFileWrite-" + fileWriteEventArgs->writeType;

		DisplayFileIOMessage(fileWriteEventArgs);

		delete fileWriteEventArgs;
	}
	else if (messageSend->MessageType == PRE_QUERY_INFORMATION)
	{
		if (messageSend->InfoClass == FileBasicInformation)
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPreQueryFileBasicInfo";
			DisplayFileIOMessage(fileSizeArgs);
							
			//if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
			//{				
			//	//
			//	//This is a test case for demo only
			//	//Change the information which returns from the file system.
			//	//
			//	PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)messageSend->DataBuffer;

			//	basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
			//	basicInfo->FileAttributes |= FILE_ATTRIBUTE_READONLY;

			//	memcpy(messageReply->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
			//	messageReply->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

			//	messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
			//	messageReply->ReturnStatus = STATUS_SUCCESS;

			//}

			//you can block FileBasicInformation query request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileSizeArgs;

		}
		else if (messageSend->InfoClass == FileEndOfFileInformation )
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPreQueryFileSize";
			DisplayFileIOMessage(fileSizeArgs);

			//you can block file size query request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;
			
			delete fileSizeArgs;
		}
		else if (messageSend->InfoClass == FileInternalInformation )
		{
			FileIdEventArgs* fileIdArgs = new FileIdEventArgs(messageSend);
			fileIdArgs->EventName = L"OnPreQueryFileId";
			DisplayFileIOMessage(fileIdArgs);

			//you can block file Id query request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileIdArgs;
		}		
		else if (messageSend->InfoClass == FileStandardInformation)
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPreQueryFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);

			//you can block FileStandardInformation query request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileStandardInfoArgs;
		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPreQueryFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);

			//you can block FileNetworkOpenInformation query request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileNetworkInfoArgs;
		}
		else 
		{
			//all other information class request
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPreQueryFileInfo";
			DisplayFileIOMessage(fileInfoArgs);

			//you can block all other information query request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileInfoArgs;
		}
	}
	else if (messageSend->MessageType == POST_QUERY_INFORMATION)
	{
		if (messageSend->InfoClass == FileBasicInformation)
		{
			FileBasicInfoEventArgs* fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
			fileBasicInfoArgs->EventName = L"OnPostQueryFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);

			//if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
			//{
			//	//
			//	//This is a test case for demo only
			//	//Change the information after the request returned from the file system test.
			//	//
			//		
			//	PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)messageSend->DataBuffer;

			//	basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
			//	basicInfo->FileAttributes = FILE_ATTRIBUTE_READONLY;

			//	memcpy(messageReply->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
			//	messageReply->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

			//	messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
			//	messageReply->ReturnStatus = STATUS_SUCCESS;

			//}

			delete fileBasicInfoArgs;

		}
		else if (messageSend->InfoClass == FileEndOfFileInformation)
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPostQueryFileSize";
			DisplayFileIOMessage(fileSizeArgs);

			delete fileSizeArgs;
		}
		else if (messageSend->InfoClass == FileInternalInformation)
		{
			FileIdEventArgs* fileIdArgs = new FileIdEventArgs(messageSend);
			fileIdArgs->EventName = L"OnPostQueryFileId";
			DisplayFileIOMessage(fileIdArgs);

			delete fileIdArgs;
		}		
		else if (messageSend->InfoClass == FileStandardInformation)
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPostQueryFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);

			delete fileStandardInfoArgs;
		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPostQueryFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);

			delete fileNetworkInfoArgs;
		}
		else 
		{
			//all other information class request
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPostQueryFileInfo";
			DisplayFileIOMessage(fileInfoArgs);

			delete fileInfoArgs;
		}
	}
	else if (messageSend->MessageType == PRE_SET_INFORMATION)
	{
		if (messageSend->InfoClass == FileBasicInformation )
		{
			FileBasicInfoEventArgs* fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
			fileBasicInfoArgs->EventName = L"OnPreSetFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);

			//if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
			//{
			//	//
			//	//This is a test case for demo only
			//	//Change the information before write down to the file system test.
			//	//
			//	PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)messageSend->DataBuffer;
			//	basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
			//	basicInfo->FileAttributes = FILE_ATTRIBUTE_READONLY;

			//	memcpy(messageReply->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
			//	messageReply->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

			//	messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
			//	messageReply->ReturnStatus = STATUS_SUCCESS;

			//}

			//you can block set FileBasicInformation request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileBasicInfoArgs;

		}
		else if (messageSend->InfoClass == FileEndOfFileInformation)
		{
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPreSetFileSize";
			DisplayFileIOMessage(fileInfoArgs);

			//you can block set file size request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileInfoArgs;

		}		
		else if (messageSend->InfoClass == FileStandardInformation )
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPreSetFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);

			//you can block set FileStandardInformation request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileStandardInfoArgs;

		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPreSetFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);

			//you can block set FileNetworkOpenInformation request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileNetworkInfoArgs;

		}
		else if (messageSend->InfoClass == FileRenameInformation
				|| messageSend->InfoClass == FileRenameInformationEx)
		{
			FileMoveOrRenameEventArgs* fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
			fileRenameArgs->EventName = L"OnPreMoveOrRenameFile";
			DisplayFileIOMessage(fileRenameArgs);
			
			//you can block file rename request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileRenameArgs;

		}
		else if (messageSend->InfoClass == FileDispositionInformation
			|| messageSend->InfoClass == FileDispositionInformationEx)
		{

			FileDispositionEventArgs* fileDispostionArgs = new FileDispositionEventArgs(messageSend);
			fileDispostionArgs->EventName = L"OnPreDeleteFile";
			DisplayFileIOMessage(fileDispostionArgs);

			//you can block the file delete request here, also check PRE_CREATE I/O if the file was opened with delete on close.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileDispostionArgs;
		}
		else 
		{
			//all other information class request
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPreSetFileInfo";
			DisplayFileIOMessage(fileInfoArgs);

			//you can block all other set information request here.
			//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
			//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

			delete fileInfoArgs;
		}
	}
	else if (messageSend->MessageType == POST_SET_INFORMATION)
	{
		if (messageSend->InfoClass == FileEndOfFileInformation)
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPostSetFileSize";
			DisplayFileIOMessage(fileSizeArgs);

			delete fileSizeArgs;
		}
		else if (messageSend->InfoClass == FileBasicInformation)
		{
			FileBasicInfoEventArgs* fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
			fileBasicInfoArgs->EventName = L"OnPostSetFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);

			delete fileBasicInfoArgs;
		}
		else if (messageSend->InfoClass == FileStandardInformation)
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPostSetFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);

			delete fileStandardInfoArgs;
		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPostSetFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);

			delete fileNetworkInfoArgs;

		}
		else if (messageSend->InfoClass == FileRenameInformation
				|| messageSend->InfoClass == FileRenameInformationEx )
		{
			FileMoveOrRenameEventArgs* fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
			fileRenameArgs->EventName = L"OnPostMoveOrRenameFile";
			DisplayFileIOMessage(fileRenameArgs);

			delete fileRenameArgs;
		}
		else if (messageSend->InfoClass == FileDispositionInformation
			|| messageSend->InfoClass == FileDispositionInformationEx) 
		{
			FileDispositionEventArgs* fileDispostionArgs = new FileDispositionEventArgs(messageSend);
			fileDispostionArgs->EventName = L"OnPostDeleteFile";
			DisplayFileIOMessage(fileDispostionArgs);

			delete fileDispostionArgs;
		}
		else 
		{
			//all other information class request.
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPostSetFileInfo";
			DisplayFileIOMessage(fileInfoArgs);

			delete fileInfoArgs;
		}
	}
	else if (messageSend->MessageType == PRE_QUERY_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPreQueryFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);

		//you can block security query request here.
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		delete fileSecurityArgs;

	}
	else if (messageSend->MessageType == POST_QUERY_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPostQueryFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);

		delete fileSecurityArgs;
	}
	else if (messageSend->MessageType == PRE_SET_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPreSetFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);

		//you can block set security request here.
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		delete fileSecurityArgs;
	}
	else if (messageSend->MessageType == POST_SET_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPostSetFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);

		delete fileSecurityArgs;
	}
	else if (messageSend->MessageType == PRE_DIRECTORY)
	{
		FileQueryDirectoryEventArgs* directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
		directoryArgs->EventName = L"OnPreQueryDirectoryFile";
		DisplayFileIOMessage(directoryArgs);

		//you can block browsing directory request here.
		//messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
		//messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

		delete directoryArgs;
	}
	else if (messageSend->MessageType == POST_DIRECTORY)
	{
		FileQueryDirectoryEventArgs* directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
		directoryArgs->EventName = L"OnPostQueryDirectoryFile";
		DisplayFileIOMessage(directoryArgs);

		delete directoryArgs;
	}
	else if (messageSend->MessageType == PRE_CLEANUP)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPreFileHandleClose";
		fileIOArgs->Description = L"Before opened file handle is closed.";
		DisplayFileIOMessage(fileIOArgs);

		delete fileIOArgs;
	}
	else if (messageSend->MessageType == POST_CLEANUP)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPostFileHandleClose";
		fileIOArgs->Description = L"The opened file handle was closed.";
		DisplayFileIOMessage(fileIOArgs);

		delete fileIOArgs;
	}
	else if (messageSend->MessageType == PRE_CLOSE)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPreFileClose";
		fileIOArgs->Description = L"Before all system references to fileObject were closed.";
		DisplayFileIOMessage(fileIOArgs);

		delete fileIOArgs;
	}
	else if (messageSend->MessageType == POST_CLOSE)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPostFileClose";
		fileIOArgs->Description = L"All system references to fileObject were closed.";
		DisplayFileIOMessage(fileIOArgs);

		delete fileIOArgs;

	}

	return retVal;
}


#endif