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

BOOL 
IOControlHandler( IN	PMESSAGE_SEND_DATA pSendMessage,IN OUT	PMESSAGE_REPLY_DATA pReplyMessage )
{
	BOOL ret = TRUE;

	//if you want to block the I/O, you need to return STATUS_ACCESS_DENIED and complete the I/O in pre I/O operation.
	//you can modify the I/O data in pre or post I/O operation.

	try
	{
		
		switch( pSendMessage->MessageType )
		{
		
		case PRE_CREATE:
			{	
				if ((pSendMessage->CreateOptions & FILE_DELETE_ON_CLOSE) > 0)
				{
					//the file will be deleted after the file handle was closed.
					//you can block the create request with below setting:
					
					//pReplyMessage->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
					//pReplyMessage->ReturnStatus = STATUS_ACCESS_DENIED;

				}

				if( IsTestFile(pSendMessage->FileName,pSendMessage->FileNameLength) )
				{
					//
					//For reparse file/folder open test.
					//
				   PrintMessage(L"File:%ws will be redirected to new file:%ws open",pSendMessage->FileName,GetTestReparseFileName());

				   //the file name must be unicode string, file name length must be less than MAX_PATH.
				   memcpy(pReplyMessage->ReplyData.Data.DataBuffer,GetTestReparseFileName(),(ULONG)wcslen(GetTestReparseFileName())*sizeof(WCHAR));
				   pReplyMessage->ReplyData.Data.DataBufferLength =(ULONG)wcslen(GetTestReparseFileName())*sizeof(WCHAR);
				   
				   pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
				   pReplyMessage->ReturnStatus = STATUS_REPARSE;
				}

				break;
			}

		case PRE_QUERY_INFORMATION:
			{
				
				//
				//Return your own information without go down to the file system.
				//
				
				if( IsTestFile(pSendMessage->FileName,pSendMessage->FileNameLength) )
				{
					FILE_BASIC_INFORMATION basicInfo;

					if( pSendMessage->InfoClass == FileBasicInformation )
					{
						basicInfo.ChangeTime = basicInfo.LastWriteTime 
						= basicInfo.CreationTime = basicInfo.LastAccessTime = GetTestFileTime();

						basicInfo.FileAttributes = FILE_ATTRIBUTE_READONLY;

						memcpy(pReplyMessage->ReplyData.Data.DataBuffer,&basicInfo,sizeof(FILE_BASIC_INFORMATION));
						pReplyMessage->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

						pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
						pReplyMessage->ReturnStatus = STATUS_SUCCESS;

						 PrintMessage(L"File:%ws is test file with pre-query-information-basicInfo,return with status:%0x.",pSendMessage->FileName,pReplyMessage->FilterStatus);
					}


					//for other information class is the same process.
				}			

				break;

			}
			
		case POST_QUERY_INFORMATION:
			{
				
				//
				//Change the information which returns from the file system.
				//
				
				if( IsTestFile(pSendMessage->FileName,pSendMessage->FileNameLength) )
				{				
					if( pSendMessage->InfoClass == FileBasicInformation )
					{
						PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)pSendMessage->DataBuffer;

						basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
						basicInfo->FileAttributes |= FILE_ATTRIBUTE_READONLY;

						memcpy(pReplyMessage->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
						pReplyMessage->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

						pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
						pReplyMessage->ReturnStatus = STATUS_SUCCESS;
					}

					//for other information class is the same process.
				}
			

				break;
			}


		case PRE_SET_INFORMATION:
			{
				switch(pSendMessage->FilterCommand)
				{
				case IOPreMoveOrRenameFile:
					{
						//File will be moved or renamed here.
						ULONG newFileNameLength = pSendMessage->DataBufferLength;
						WCHAR* newFileName = (WCHAR*)pSendMessage->DataBuffer;
						PrintMessage( L"FileInformation Class FileRenameInformation = %d change to new file name:%ws\n"	,pSendMessage->InfoClass,newFileName);

						//you can block the rename request with below setting:
					
						//pReplyMessage->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
						//pReplyMessage->ReturnStatus = STATUS_ACCESS_DENIED;

						break;
					}

				case IOPreDeleteFile:
					{
						//File will be deleted.
						PrintMessage( L"FileInformation Class FileDispositionInformation = %d, the file is going to be deleted\n"	,pSendMessage->InfoClass);

						//you can block the delete request with below setting:
					
						//pReplyMessage->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
						//pReplyMessage->ReturnStatus = STATUS_ACCESS_DENIED;

						break;
					}
				default:break;
				}
						
				
				if( IsTestFile(pSendMessage->FileName,pSendMessage->FileNameLength) )
				{
					//
					//Change the information before write down to the file system test.
					//
					
					if( pSendMessage->InfoClass == FileBasicInformation )
					{
						PFILE_BASIC_INFORMATION basicInfo = (PFILE_BASIC_INFORMATION)pSendMessage->DataBuffer;

						basicInfo->ChangeTime = basicInfo->LastWriteTime = GetTestFileTime();
						basicInfo->FileAttributes = FILE_ATTRIBUTE_READONLY;

						memcpy(pReplyMessage->ReplyData.Data.DataBuffer,basicInfo,sizeof(FILE_BASIC_INFORMATION));
						pReplyMessage->ReplyData.Data.DataBufferLength = sizeof(FILE_BASIC_INFORMATION);

						pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
						pReplyMessage->ReturnStatus = STATUS_SUCCESS;

					}

					//for other information class is the same process.
				}

			
				break;
			}

		//For POST_SET_INFORMATION, you can't change anything, since the data was written to the file system.
	   
		//
		//For PRE_DIRECTORY,you can complete it and return your own directory data,the data structure format
		//must be the same as information class from the request,you can reference POST_DIRECTORY.

		case POST_DIRECTORY:
			{
				//
				//Change the information which returns from the file system.
				//
				if(STATUS_SUCCESS == pSendMessage->Status && IsTestFolder(pSendMessage->FileName) )
				{					


					//here we use FileBothDirectoryInformation as example, for other classes, you can do the same operation.
					if( pSendMessage->InfoClass == FileBothDirectoryInformation )
					{
						memcpy(pReplyMessage->ReplyData.Data.DataBuffer,pSendMessage->DataBuffer,pSendMessage->DataBufferLength);
						pReplyMessage->ReplyData.Data.DataBufferLength = pSendMessage->DataBufferLength;
						pReplyMessage->ReturnStatus = STATUS_SUCCESS;

						PFILE_BOTH_DIR_INFORMATION		fileBothDirInfo = (PFILE_BOTH_DIR_INFORMATION)pReplyMessage->ReplyData.Data.DataBuffer;
						ULONG	returnBufferLength = 0;
						ULONG	totalBufferLength = pReplyMessage->ReplyData.Data.DataBufferLength;

						if( NULL == fileBothDirInfo )
						{
							PrintErrorMessage(L"POST_DIRECTORY return empty databuffer",0);
						}

						BOOL isDatabufferUpdated = FALSE;

						while( fileBothDirInfo )
						{
							ULONG nextOffset = fileBothDirInfo->NextEntryOffset;

							PrintMessage(L"FileName:%ws Attributes:0x%0x FileSize:%I64d\n"
								,fileBothDirInfo->FileName,fileBothDirInfo->FileAttributes,fileBothDirInfo->EndOfFile.QuadPart);

							//You can modify the data of the fileBothDirInfo, but must be careful to change the size of the entry.
							//This structure must be aligned on a LONGLONG (8-byte) boundary. If a buffer contains two or more of these structures, 
							//the NextEntryOffset value in each entry, except the last, falls on an 8-byte boundary. 

							//Test: Hide all the files whose file size greater than 1024 bytes.
							if( fileBothDirInfo->EndOfFile.QuadPart > 1024 )
							{				
								if( nextOffset != 0 )
								{
									PrintMessage(L"File %ws is filtered",fileBothDirInfo->FileName);
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
								PrintMessage(L"File %ws attributs:0x%0x was added ReadOnly,Filesize:%I64d was changed to 1234567890"
									,fileBothDirInfo->FileName,fileBothDirInfo->FileAttributes,fileBothDirInfo->EndOfFile.QuadPart);

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

						pReplyMessage->ReplyData.Data.DataBufferLength = returnBufferLength;

						//if you filter out all the files, you can return no more files status. The system won't generate another request.
						if( returnBufferLength == 0 )
						{
							pReplyMessage->ReturnStatus = STATUS_NO_MORE_FILES;	
						}

						if( isDatabufferUpdated )
						{
							PrintMessage(L"Update directory buffer,original length:%d new length:%d",pSendMessage->DataBufferLength,pReplyMessage->ReplyData.Data.DataBufferLength);
							pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
						}
					}
				}

				break;
			}

			case PRE_FASTIO_READ:
			case PRE_CACHE_READ:
			case PRE_NOCACHE_READ:
			case PRE_PAGING_IO_READ:
			{
				//
				//Return your own data without go down to the file system.
				//
				if( IsTestFile(pSendMessage->FileName,pSendMessage->FileNameLength) )
				{
					ULONG dataLength = (ULONG)strlen(GetReplaceData());

					if( pSendMessage->Length < dataLength )
					{
						dataLength = pSendMessage->Length;
					}

					memcpy(pReplyMessage->ReplyData.Data.DataBuffer,GetReplaceData(),dataLength);
					pReplyMessage->ReplyData.Data.DataBufferLength = dataLength;

					pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_COMPLETE_PRE_OPERATION|FILTER_DATA_BUFFER_IS_UPDATED;
					pReplyMessage->ReturnStatus = STATUS_SUCCESS;
				}

			   break;
			}

			case POST_FASTIO_READ:
			case POST_CACHE_READ: 
			case POST_NOCACHE_READ:
			case POST_PAGING_IO_READ:
			{
				//
				//Modify the data which returns from the file system.
				//
				
				if( IsTestFile(pSendMessage->FileName,pSendMessage->FileNameLength) )
				{
					
					memcpy(pReplyMessage->ReplyData.Data.DataBuffer,pSendMessage->DataBuffer,pSendMessage->DataBufferLength);
					//merge test data to the beginning of the buffer.
					ULONG dataLength = (ULONG)strlen(GetReplaceData());

					if( pSendMessage->Length < dataLength )
					{
						dataLength = pSendMessage->Length;
					}

					memcpy(pReplyMessage->ReplyData.Data.DataBuffer,GetReplaceData(),dataLength);
					pReplyMessage->ReplyData.Data.DataBufferLength = dataLength;

									
					pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
					pReplyMessage->ReturnStatus = STATUS_SUCCESS;
				}


			   break;
			}


			case PRE_FASTIO_WRITE:
			case PRE_CACHE_WRITE:
			case PRE_NOCACHE_WRITE:			
			case PRE_PAGING_IO_WRITE:	
			{
								
				if( IsTestFile(pSendMessage->FileName,pSendMessage->FileNameLength) )
				{
					//
					//Modiy the write data buffer before it goes down to the file system.
					//

					memcpy(pReplyMessage->ReplyData.Data.DataBuffer,pSendMessage->DataBuffer,pSendMessage->DataBufferLength);
					//merge test data to the beginning of the buffer.
					ULONG dataLength = (ULONG)strlen(GetReplaceData());

					if( pSendMessage->Length < dataLength )
					{
						dataLength = pSendMessage->Length;
					}

					memcpy(pReplyMessage->ReplyData.Data.DataBuffer,GetReplaceData(),dataLength);
					pReplyMessage->ReplyData.Data.DataBufferLength = dataLength;


					pReplyMessage->FilterStatus = FILTER_MESSAGE_IS_DIRTY|FILTER_DATA_BUFFER_IS_UPDATED;
					pReplyMessage->ReturnStatus = STATUS_SUCCESS;				

				}

				//you can block the write request with below setting:
                                  
				//pReplyMessage->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
				//pReplyMessage->ReturnStatus = STATUS_ACCESS_DENIED;

		

			   break;
			}

			//For post write operation, the data was written to the file system,at this point, we can't change anything.

			default: break;
		
		}
	}
	catch(...)
	{
		PrintErrorMessage( L"IOControlHandler exception.",GetLastError());     
		ret = FALSE;
	}

	return ret;
}
