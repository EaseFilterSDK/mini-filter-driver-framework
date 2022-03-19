#ifndef __FILECONTROL_H__
#define __FILECONTROL_H__

#include "UnitTest.h"
#include "FilterAPI.h"
#include "FilterMessage.h"

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
                   
		if ( messageSend->MessageType == PRE_CREATE)
		{

			fileCreateEventArgs->EventName = L"OnPreCreateFile";

			if (fileCreateEventArgs->isDeleteOnClose )
			{
				//the file will be deleted on the file close.
				fileCreateEventArgs->EventName = L"OnPreCreateFile-DeleteFileOnClose";
			}

			DisplayFileIOMessage(fileCreateEventArgs);	

			//you can reparse the file open with below setting
			if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
			{
				//
				//This is a test case for demo only
				//For reparse file/folder open test.
				//
				//the file name must be unicode string, file name length must be less than MAX_PATH.
				memcpy(messageReply->ReplyData.Data.DataBuffer,GetTestReparseFileName(),(ULONG)wcslen(GetTestReparseFileName())*sizeof(WCHAR));
				messageReply->ReplyData.Data.DataBufferLength =(ULONG)wcslen(GetTestReparseFileName())*sizeof(WCHAR);
				   
				messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
				messageReply->ReturnStatus = STATUS_REPARSE;
			}

		}
		
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

	}
	else if (messageSend->MessageType == PRE_CACHE_READ
			|| messageSend->MessageType == PRE_FASTIO_READ
			|| messageSend->MessageType == PRE_NOCACHE_READ
			|| messageSend->MessageType == PRE_PAGING_IO_READ)
	{
		FileReadEventArgs* fileReadEventArgs = new FileReadEventArgs(messageSend);
		fileReadEventArgs->EventName = L"OnPreFileRead-" + fileReadEventArgs->readType;

		DisplayFileIOMessage(fileReadEventArgs);

		if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
		{
			//
			//This is a test case for demo only
			//Return your own read data without going down to the file system.
			//

			ULONG dataLength = (ULONG)strlen(GetReplaceData());

			if( messageSend->Length < dataLength )
			{
				dataLength = messageSend->Length;
			}

			memcpy(messageReply->ReplyData.Data.DataBuffer,GetReplaceData(),dataLength);
			messageReply->ReplyData.Data.DataBufferLength = dataLength;

			messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
			messageReply->ReturnStatus = STATUS_SUCCESS;
		}
	}
	else if (messageSend->MessageType == POST_CACHE_READ
		|| messageSend->MessageType == POST_FASTIO_READ
		|| messageSend->MessageType == POST_NOCACHE_READ
		|| messageSend->MessageType == POST_PAGING_IO_READ)
	{
		FileReadEventArgs* fileReadEventArgs = new FileReadEventArgs(messageSend);
		fileReadEventArgs->EventName = L"OnPostFileRead-" + fileReadEventArgs->readType;

		DisplayFileIOMessage(fileReadEventArgs);
		
		if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
		{
			//
			//This is a test case for demo only
			//Modify the data which returns from the file system.
			//
					
			memcpy(messageReply->ReplyData.Data.DataBuffer,messageSend->DataBuffer,messageSend->DataBufferLength);
			//merge test data to the beginning of the buffer.
			ULONG dataLength = (ULONG)strlen(GetReplaceData());

			if( messageSend->Length < dataLength )
			{
				dataLength = messageSend->Length;
			}

			memcpy(messageReply->ReplyData.Data.DataBuffer,GetReplaceData(),dataLength);
			messageReply->ReplyData.Data.DataBufferLength = dataLength;

									
			messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
			messageReply->ReturnStatus = STATUS_SUCCESS;
		}
		
	}
	else if (messageSend->MessageType == PRE_CACHE_WRITE
			|| messageSend->MessageType == PRE_FASTIO_WRITE
			|| messageSend->MessageType == PRE_NOCACHE_WRITE
			|| messageSend->MessageType == PRE_PAGING_IO_WRITE)
	{

		FileWriteEventArgs* fileWriteEventArgs = new FileWriteEventArgs(messageSend);
		fileWriteEventArgs->EventName = L"OnPreFileWrite-" + fileWriteEventArgs->writeType;

		DisplayFileIOMessage(fileWriteEventArgs);

		if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
		{
			//
			//This is a test case for demo only
			//Modiy the write data buffer before it goes down to the file system.
			//

			memcpy(messageReply->ReplyData.Data.DataBuffer,messageSend->DataBuffer,messageSend->DataBufferLength);
			//merge test data to the beginning of the buffer.
			ULONG dataLength = (ULONG)strlen(GetReplaceData());

			if( messageSend->Length < dataLength )
			{
				dataLength = messageSend->Length;
			}

			memcpy(messageReply->ReplyData.Data.DataBuffer,GetReplaceData(),dataLength);
			messageReply->ReplyData.Data.DataBufferLength = dataLength;


			messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
			messageReply->ReturnStatus = STATUS_SUCCESS;				

		}
	}
	else if (messageSend->MessageType == POST_CACHE_WRITE
	|| messageSend->MessageType == POST_FASTIO_WRITE
	|| messageSend->MessageType == POST_NOCACHE_WRITE
	|| messageSend->MessageType == POST_PAGING_IO_WRITE)
	{
		
		FileWriteEventArgs* fileWriteEventArgs = new FileWriteEventArgs(messageSend);
		fileWriteEventArgs->EventName = L"OnPostFileWrite-" + fileWriteEventArgs->writeType;

		DisplayFileIOMessage(fileWriteEventArgs);

	}
	else if (messageSend->MessageType == PRE_QUERY_INFORMATION)
	{
		if (messageSend->InfoClass == FileBasicInformation)
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPreQueryFileBasicInfo";
			DisplayFileIOMessage(fileSizeArgs);
							
			if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
			{				
				//
				//This is a test case for demo only
				//Change the information which returns from the file system.
				//
				PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)messageSend->DataBuffer;

				basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
				basicInfo->FileAttributes |= FILE_ATTRIBUTE_READONLY;

				memcpy(messageReply->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
				messageReply->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

				messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
				messageReply->ReturnStatus = STATUS_SUCCESS;

			}

		}
		else if (messageSend->InfoClass == FileEndOfFileInformation )
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPreQueryFileSize";
			DisplayFileIOMessage(fileSizeArgs);
			
		}
		else if (messageSend->InfoClass == FileInternalInformation )
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPreQueryFileId";
			DisplayFileIOMessage(fileSizeArgs);
		}		
		else if (messageSend->InfoClass == FileStandardInformation)
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPreQueryFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);
		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPreQueryFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);
		}
		else 
		{
			//all other information class request
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPreQueryFileInfo";
			DisplayFileIOMessage(fileInfoArgs);
		}
	}
	else if (messageSend->MessageType == POST_QUERY_INFORMATION)
	{
		if (messageSend->InfoClass == FileBasicInformation)
		{
			FileBasicInfoEventArgs* fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
			fileBasicInfoArgs->EventName = L"OnPostQueryFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);

			if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
			{
				//
				//This is a test case for demo only
				//Change the information after the request returned from the file system test.
				//
					
				PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)messageSend->DataBuffer;

				basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
				basicInfo->FileAttributes = FILE_ATTRIBUTE_READONLY;

				memcpy(messageReply->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
				messageReply->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

				messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
				messageReply->ReturnStatus = STATUS_SUCCESS;

			}

		}
		else if (messageSend->InfoClass == FileEndOfFileInformation)
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPostQueryFileSize";
			DisplayFileIOMessage(fileSizeArgs);
		}
		else if (messageSend->InfoClass == FileInternalInformation)
		{
			FileIdEventArgs* fileIdArgs = new FileIdEventArgs(messageSend);
			fileIdArgs->EventName = L"OnPostQueryFileId";
			DisplayFileIOMessage(fileIdArgs);
		}		
		else if (messageSend->InfoClass == FileStandardInformation)
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPostQueryFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);

		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPostQueryFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);

		}
		else 
		{
			//all other information class request
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPostQueryFileInfo";
			DisplayFileIOMessage(fileInfoArgs);
		}
	}
	else if (messageSend->MessageType == PRE_SET_INFORMATION)
	{
		if (messageSend->InfoClass == FileBasicInformation )
		{
			FileBasicInfoEventArgs* fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
			fileBasicInfoArgs->EventName = L"OnPreSetFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);

			if( IsTestFile(messageSend->FileName,messageSend->FileNameLength) )
			{
				//
				//This is a test case for demo only
				//Change the information before write down to the file system test.
				//
				PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)messageSend->DataBuffer;
				basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
				basicInfo->FileAttributes = FILE_ATTRIBUTE_READONLY;

				memcpy(messageReply->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
				messageReply->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

				messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
				messageReply->ReturnStatus = STATUS_SUCCESS;

			}
		}
		else if (messageSend->InfoClass == FileEndOfFileInformation)
		{
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPreSetFileSize";
			DisplayFileIOMessage(fileInfoArgs);
		}		
		else if (messageSend->InfoClass == FileStandardInformation )
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPreSetFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);
		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPreSetFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);
		}
		else if (messageSend->InfoClass == FileRenameInformation
				|| messageSend->InfoClass == FileRenameInformationEx)
		{
			FileMoveOrRenameEventArgs* fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
			fileRenameArgs->EventName = L"OnPreMoveOrRenameFile";
			DisplayFileIOMessage(fileRenameArgs);
		}
		else if (messageSend->InfoClass == FileDispositionInformation
			|| messageSend->InfoClass == FileDispositionInformationEx)
		{
			FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
			fileIOArgs->EventName = L"OnPreDeleteFile";
			DisplayFileIOMessage(fileIOArgs);
		}
		else 
		{
			//all other information class request
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPreSetFileInfo";
			DisplayFileIOMessage(fileInfoArgs);
		}
	}
	else if (messageSend->MessageType == POST_SET_INFORMATION)
	{
		if (messageSend->InfoClass == FileEndOfFileInformation)
		{
			FileSizeEventArgs* fileSizeArgs = new FileSizeEventArgs(messageSend);
			fileSizeArgs->EventName = L"OnPostSetFileSize";
			DisplayFileIOMessage(fileSizeArgs);
		}
		else if (messageSend->InfoClass == FileBasicInformation)
		{
			FileBasicInfoEventArgs* fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
			fileBasicInfoArgs->EventName = L"OnPostSetFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);
		}
		else if (messageSend->InfoClass == FileStandardInformation)
		{
			FileStandardInfoEventArgs* fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
			fileStandardInfoArgs->EventName = L"OnPostSetFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);
		}
		else if (messageSend->InfoClass == FileNetworkOpenInformation)
		{
			FileNetworkInfoEventArgs* fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
			fileNetworkInfoArgs->EventName = L"OnPostSetFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);

		}
		else if (messageSend->InfoClass == FileRenameInformation
				|| messageSend->InfoClass == FileRenameInformationEx )
		{
			FileMoveOrRenameEventArgs* fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
			fileRenameArgs->EventName = L"OnPostMoveOrRenameFile";
			DisplayFileIOMessage(fileRenameArgs);
		}
		else if (messageSend->InfoClass == FileDispositionInformation
			|| messageSend->InfoClass == FileDispositionInformationEx) 
		{
			FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
			fileIOArgs->EventName = L"OnPostDeleteFile";
			fileIOArgs->Description = L"The file was deleted.";
			DisplayFileIOMessage(fileIOArgs);
		}
		else 
		{
			//all other information class request.
			FileInfoArgs* fileInfoArgs = new FileInfoArgs(messageSend);
			fileInfoArgs->EventName = L"OnPostSetFileInfo";
			DisplayFileIOMessage(fileInfoArgs);
		}
	}
	else if (messageSend->MessageType == PRE_QUERY_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPreQueryFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);

	}
	else if (messageSend->MessageType == POST_QUERY_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPostQueryFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);
	}
	else if (messageSend->MessageType == PRE_SET_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPreSetFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);
	}
	else if (messageSend->MessageType == POST_SET_SECURITY)
	{
		FileSecurityEventArgs* fileSecurityArgs = new FileSecurityEventArgs(messageSend);
		fileSecurityArgs->EventName = L"OnPostSetFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);
	}
	else if (messageSend->MessageType == PRE_DIRECTORY)
	{
		FileQueryDirectoryEventArgs* directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
		directoryArgs->EventName = L"OnPreQueryDirectoryFile";
		DisplayFileIOMessage(directoryArgs);
	}
	else if (messageSend->MessageType == POST_DIRECTORY)
	{
		FileQueryDirectoryEventArgs* directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
		directoryArgs->EventName = L"OnPostQueryDirectoryFile";
		DisplayFileIOMessage(directoryArgs);
		
		if(STATUS_SUCCESS == messageSend->Status && IsTestFolder(messageSend->FileName) )
		{	
			//
			//This is a test case for demo only
			//Change the information which returns from the file system.
			//				
	
			//here we use FileBothDirectoryInformation as example, for other classes, you can do the same operation.
			if( messageSend->InfoClass == FileBothDirectoryInformation )
			{
				memcpy(messageReply->ReplyData.Data.DataBuffer,messageSend->DataBuffer,messageSend->DataBufferLength);
				messageReply->ReplyData.Data.DataBufferLength = messageSend->DataBufferLength;
				messageReply->ReturnStatus = STATUS_SUCCESS;

				PFILE_BOTH_DIR_INFORMATION		fileBothDirInfo = (PFILE_BOTH_DIR_INFORMATION)messageReply->ReplyData.Data.DataBuffer;
				ULONG	returnBufferLength = 0;
				ULONG	totalBufferLength = messageReply->ReplyData.Data.DataBufferLength;

				if( NULL == fileBothDirInfo )
				{
					PrintErrorMessage(L"POST_DIRECTORY return empty databuffer",0);
				}

				BOOL isDatabufferUpdated = FALSE;

				while( fileBothDirInfo )
				{
					ULONG nextOffset = fileBothDirInfo->NextEntryOffset;

					//You can modify the data of the fileBothDirInfo, but must be careful to change the size of the entry.
					//This structure must be aligned on a LONGLONG (8-byte) boundary. If a buffer contains two or more of these structures, 
					//the NextEntryOffset value in each entry, except the last, falls on an 8-byte boundary. 

					//Test: Hide all the files whose file size greater than 1024 bytes.
					if( fileBothDirInfo->EndOfFile.QuadPart > 1024 )
					{				
						if( nextOffset != 0 )
						{
							memmove (fileBothDirInfo,(PUCHAR)fileBothDirInfo + nextOffset,totalBufferLength - returnBufferLength - nextOffset );
							isDatabufferUpdated = TRUE;
							continue;
						}
						else
						{
							fileBothDirInfo->NextEntryOffset = 0;
						}
					}

					//Test: Add ReadOnly to the FileAttributes, change FileSize to test file size for all files;
					if(NULL == (fileBothDirInfo->FileAttributes & FILE_ATTRIBUTE_DIRECTORY) )
					{
						fileBothDirInfo->FileAttributes |= FILE_ATTRIBUTE_READONLY;
						fileBothDirInfo->EndOfFile.QuadPart = GetTestFileSize();
						isDatabufferUpdated = TRUE;
					}

					returnBufferLength += nextOffset;
							
					if( nextOffset == 0)
					{
						returnBufferLength += sizeof(FILE_BOTH_DIR_INFORMATION) + fileBothDirInfo->FileNameLength - sizeof(WCHAR);
						fileBothDirInfo = NULL;
					}
					else
					{
						fileBothDirInfo = (PFILE_BOTH_DIR_INFORMATION)((PUCHAR)fileBothDirInfo + nextOffset);
					}

				}

				messageReply->ReplyData.Data.DataBufferLength = returnBufferLength;

				//if you filter out all the files, you can return no more files status. The system won't generate another request.
				if( returnBufferLength == 0 )
				{
					messageReply->ReturnStatus = STATUS_NO_MORE_FILES;	
				}

				if( isDatabufferUpdated )
				{
					messageReply->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
				}
			}
		}
	}
	else if (messageSend->MessageType == PRE_CLEANUP)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPreFileHandleClose";
		fileIOArgs->Description = L"Before opened file handle is closed.";
		DisplayFileIOMessage(fileIOArgs);
	}
	else if (messageSend->MessageType == POST_CLEANUP)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPostFileHandleClose";
		fileIOArgs->Description = L"The opened file handle was closed.";
		DisplayFileIOMessage(fileIOArgs);
	}
	else if (messageSend->MessageType == PRE_CLOSE)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPreFileClose";
		fileIOArgs->Description = L"Before all system references to fileObject were closed.";
		DisplayFileIOMessage(fileIOArgs);
	}
	else if (messageSend->MessageType == POST_CLOSE)
	{
		FileIOEventArgs* fileIOArgs = new FileIOEventArgs(messageSend);
		fileIOArgs->EventName = L"OnPostFileClose";
		fileIOArgs->Description = L"All system references to fileObject were closed.";
		DisplayFileIOMessage(fileIOArgs);

	}

	return retVal;
}


#endif