///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2015 EaseFilter Technologies Inc.
//    All Rights Reserved
//
//    This software is part of a licensed software product and may
//    only be used or copied in accordance with the terms of that license.
//
///////////////////////////////////////////////////////////////////////////////

#ifndef __FILTERCONTROL_H__
#define __FILTERCONTROL_H__

#include "FilterAPI.h"
#include "FilterRule.h"


#ifndef FO_REMOTE_ORIGIN
#define FO_REMOTE_ORIGIN                0x01000000
#endif

 class FileIOEventArgs 
 {
 public:

        /// The Message Id.
        ULONG MessageId;
		/// The filter rule Id.
        ULONG FilterRuleId;
        ///This is the IO completion status, either STATUS_SUCCESS if the requested operation was completed successfully 
        ///or an informational, warning, or error STATUS_XXX value, only effect on post IO.
        ULONG IoStatus;
        /// the transaction time in UTC of this IO request
        LONGLONG TransactionTime;
        /// The fileObject is an unique Id for the file I/O from open till the close.
        PVOID FileObject;
        /// A pointer to the FSRTL_ADVANCED_FCB_HEADER header structure that  is contained 
        /// within a file-system-specific structure,it is unique per file.
        PVOID FsContext;
        /// The process Id who initiates the IO.
        ULONG ProcessId;
        /// The process name who initiates the IO.
        std::wstring ProcessName;
        /// The thread Id who initiates the IO.
        ULONG ThreadId;
        /// The user name who initiates the IO.
        std::wstring UserName;
        /// The file name of the file IO.
        std::wstring FileName;
        /// The file size of the file IO.
        LONGLONG FileSize;
        /// The creation time in UTC of the file.
        LONGLONG CreationTime;
        /// The last write time in UTC of the file.
        LONGLONG LastWriteTime;
        /// The file attributes of the file IO.
        ULONG FileAttributes;
        /// The file CreateOptions of the file create API.
        ULONG CreateOptions;
        /// The file DesiredAccess options of the file create API.
		ULONG DesiredAccess;
		/// The file Disposition options of the file create API.
        ULONG Disposition;
        /// The ShareAccess for file open, please reference CreateFile windows API.
        ULONG ShareAccess;
        /// The name of the event which uses this eventargs
        std::wstring EventName;
        /// The file open was from the network
        BOOL IsRemoteAccess;
        /// The IP address of the remote computer who is opening the file.
        /// This feature is enabled only for Win7 or later OS.
        std::wstring RemoteIp;
        /// The description of the IO.
        std::wstring Description;        
        /// Change the return status of the IO, it is only for control filter or encryption filter
        /// set the status to AccessDenied if you want to block this IO.
        ULONG ReturnStatus;
        /// Set it to true, if the return data was modified. 
        BOOL IsDataModified;

        FileIOEventArgs(PMESSAGE_SEND_DATA messageSend)
        {		
           	if(messageSend->SidLength > 0 )
			{
				WCHAR userName[MAX_PATH];
				WCHAR domainName[MAX_PATH];

				int userNameSize = MAX_PATH;
				int domainNameSize = MAX_PATH;
				SID_NAME_USE snu;

				
					BOOL ret = LookupAccountSid( NULL,
										messageSend->Sid,
										userName,
										(LPDWORD)&userNameSize,
										domainName,
										(LPDWORD)&domainNameSize,
										&snu); 

					UserName.assign(domainName);
					UserName.append(L"\\");
					UserName.append(userName);
				
			}	

			if( messageSend->ProcessId > 0)
			{
				WCHAR processName[MAX_PATH];
				if( GetProcessNameByPid(messageSend->ProcessId,processName,MAX_PATH))
				{
					ProcessName.assign(processName);
				}
				else
				{
					ProcessName.assign(L"UNKNOWN");
				}
			}

			FileName.assign(messageSend->FileName);

            FileObject = messageSend->FileObject;
            FsContext = messageSend->FsContext;
            MessageId = messageSend->MessageId;
            FilterRuleId = messageSend->FilterRuleId;
            TransactionTime = messageSend->TransactionTime;
            CreationTime = messageSend->CreationTime;
            LastWriteTime = messageSend->LastWriteTime;
            ProcessId = messageSend->ProcessId;
            ThreadId = messageSend->ThreadId;           
            FileSize = messageSend->FileSize;
            FileAttributes = messageSend->FileAttributes;
            IoStatus = messageSend->Status;

            CreateOptions = messageSend->CreateOptions;
            DesiredAccess = messageSend->DesiredAccess;
            Disposition = messageSend->Disposition;
            ShareAccess = messageSend->ShareAccess;

            IsRemoteAccess = false;
			RemoteIp.assign(L"");
            if ((messageSend->CreateOptions & FO_REMOTE_ORIGIN ) > 0)
            {
                //this is the request comes from remote server
                IsRemoteAccess = true;
				RemoteIp.assign(messageSend->RemoteIP);
            }

		}
      
    };

	/// <summary>
    /// The callback parameter information of the file change event.
    /// </summary>
    class FileChangedEventArgs : public FileIOEventArgs
    {
	public:
        /// The new file name of the rename IO
        std::wstring newFileName;
        /// The event type of the file change event.
        ULONG eventType;

        FileChangedEventArgs(PMESSAGE_SEND_DATA messageSend):FileIOEventArgs(messageSend)
        {
            if ((messageSend->InfoClass & FILE_WAS_CREATED ) > 0)
            {
                eventType = FILE_WAS_CREATED;
				EventName.assign(L"NotifyFileWasCreated;");
				Description.assign(L"The new file was created.");
            }

            if ((messageSend->InfoClass & FILE_WAS_WRITTEN ) > 0)
            {
                eventType |= FILE_WAS_WRITTEN;
				EventName.append(L"NotifyFileWasWritten;");
                Description.append(L" the file was written.");
            }

            if ((messageSend->InfoClass & FILE_WAS_DELETED ) > 0)
            {
                eventType |= FILE_WAS_DELETED;
                EventName.append(L"NotifyFileWasDeleted;");
                Description.append(L"The file was deleted.");
            }

            if ((messageSend->InfoClass & FILE_INFO_CHANGED ) > 0)
            {
                eventType |= FILE_INFO_CHANGED;
                EventName.append(L"NotifyFileInfoWasChanged;");
                Description.append(L"The file information was changed.");
            }

            if ((messageSend->InfoClass & FILE_WAS_RENAMED ) > 0)
            {
                if (messageSend->DataBufferLength > 0)
                {
					newFileName.assign((WCHAR*)messageSend->DataBuffer);
                }

                eventType |= FILE_WAS_RENAMED;
                EventName.append(L"NotifyFileWasRenamed;");
				Description += L"The file was renamed to " + newFileName;
            }

            if ((messageSend->InfoClass & FILE_SECURITY_CHANGED ) > 0)
            {
                eventType |= FILE_SECURITY_CHANGED;
                EventName.append(L"NotifyFileSecurityWasChanged;");
                Description += L" security was changed.";
            }

            if ((messageSend->InfoClass & FILE_WAS_READ ) > 0)
            {
                eventType |= FILE_WAS_READ;
                EventName.append(L"NotifyFileWasRead;");
                Description.append(L"The file was read.");
            }

        }       
      
    };

	class FileCreateEventArgs : public FileIOEventArgs
    {
	public:
		 /// <summary>
        /// The file will be deleted on close when it was true.
        /// </summary>
        BOOL isDeleteOnClose;
        /// <summary>
        /// It indicates that the new file was created if it is true.
        /// </summary>
        BOOL isNewFileCreated ;

        FileCreateEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {                       
            if (    messageSend->Status == 0 
                && POST_CREATE == messageSend->MessageType)
            {
                if (	messageSend->CreateStatus == FILE_CREATED
                    ||  messageSend->CreateStatus == FILE_SUPERSEDED )
                {
					 Description = L"The new file was created.";
                    isNewFileCreated = true;
                }
                else
                {
                    isNewFileCreated = false;

					if( messageSend->CreateStatus == FILE_OPENED )
					{
						 Description = L"The file was opened.";
					}
					else if( messageSend->CreateStatus == FILE_OVERWRITTEN )
					{
						 Description = L"The file was overwritten.";
					}
					else if( messageSend->CreateStatus == FILE_EXISTS )
					{
						 Description = L"The file already exists.";
					}
					else if( messageSend->CreateStatus == FILE_DOES_NOT_EXIST )
					{
						 Description = L"The file does not exist.";
					}

                }

            }        

			if ((messageSend->CreateOptions & FILE_DELETE_ON_CLOSE) > 0)
            {
                isDeleteOnClose = true;
				Description += L",The file will be deleted on close.";
            }
            else
            {
                isDeleteOnClose = false;
            }

		}
	};

    class FileReadEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The read offset
        /// </summary>
        LONGLONG offset ;
        /// <summary>
        /// The length of the read
        /// </summary>
        ULONG readLength ;
        /// <summary>
        /// The return length of the read
        /// </summary>
        ULONG returnReadLength ;
        /// <summary>
        /// The read data buffer
        /// </summary>
        std::vector<UCHAR> buffer ;
        /// <summary>
        /// The read type
        /// </summary>
        std::wstring readType ;
		
		FileReadEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            offset = messageSend->Offset;
            readLength = messageSend->Length;
            returnReadLength = messageSend->ReturnLength;

            if (messageSend->DataBufferLength > 0 && messageSend->DataBufferLength <= sizeof(messageSend->DataBuffer))
            {
				/*buffer.resize(messageSend->DataBufferLength);
				memcpy(buffer.data(), messageSend->DataBuffer, messageSend->DataBufferLength);*/
            }

            if (messageSend->MessageType == POST_NOCACHE_READ
                || messageSend->MessageType == PRE_NOCACHE_READ)
            {
                readType = L"NonCacheRead";
            }
            else if (   messageSend->MessageType == PRE_PAGING_IO_READ
                       || messageSend->MessageType == POST_PAGING_IO_READ)
            {
                readType = L"PagingIORead";
            }
            else
            {
                readType = L"CacheRead";
            }

			Description =  L"ReadType:" + readType + L",ReadOffset:" + std::to_wstring(offset) + L",ReadLength:" + std::to_wstring((ULONGLONG)readLength) 
				+ L",returnReadLength:" + std::to_wstring((ULONGLONG)returnReadLength);
            
        }
    };

    class FileWriteEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The write offset
        /// </summary>
        LONGLONG offset ;
        /// <summary>
        /// The length of the write
        /// </summary>
        ULONG writeLength ;
        /// <summary>
        /// The length of the written
        /// </summary>
        ULONG writtenLength ;
        /// <summary>
        /// The data buffer length
        /// </summary>
        ULONG bufferLength ;
        /// <summary>
        /// The write data buffer
        /// </summary>
        std::vector<UCHAR> buffer ;
        /// <summary>
        /// The read type
        /// </summary>
        std::wstring writeType ;

		FileWriteEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            offset = messageSend->Offset;
            writeLength = messageSend->Length;
            writtenLength = messageSend->ReturnLength;
            bufferLength = messageSend->DataBufferLength;

            if (messageSend->DataBufferLength > 0 && messageSend->DataBufferLength <= sizeof(messageSend->DataBuffer))
            {
				/*buffer.resize(messageSend->DataBufferLength);
				memcpy(buffer.data(), messageSend->DataBuffer, messageSend->DataBufferLength);*/
            }

            if (	messageSend->MessageType == POST_NOCACHE_READ
                ||	messageSend->MessageType == PRE_NOCACHE_READ)
            {
                writeType = L"NonCacheWrite";
            }
            else if(	messageSend->MessageType == PRE_PAGING_IO_READ
                     || messageSend->MessageType == POST_PAGING_IO_READ)
            {
                 writeType = L"PagingIOWrite";
            }
            else
            {
                writeType = L"CacheWrite";
            }
        
			Description = L"WriteType:" + writeType +  L",WriteOffset:" + std::to_wstring(offset) + L",writeLength:" + std::to_wstring((ULONGLONG)writeLength) 
				+ L",WrittenLength:" + std::to_wstring((ULONGLONG)writtenLength);
        }
    };

    class FileSizeEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The size of the file used by FileEndOfFileInformation class
        /// </summary>
        LONGLONG fileSizeToQueryOrSet ;

		FileSizeEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            if (messageSend->DataBufferLength > 0)
            {
                fileSizeToQueryOrSet = *(LONGLONG*)messageSend->DataBuffer;
            }

			Description = L"FileSize:" + std::to_wstring(fileSizeToQueryOrSet);
		}
	};

    class FileIdEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The file Id used by FileInternalInformation class
        /// </summary>
        LONGLONG fileId ;

		FileIdEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            if (messageSend->DataBufferLength > 0)
            {
                fileId = *(LONGLONG*)messageSend->DataBuffer;
            }

			Description = L"FileId:" + std::to_wstring(fileId);
        }
       
    };

    class FileBasicInfoEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The file basic information
        /// </summary>
        FILE_BASIC_INFORMATION basicInfo ;

		FileBasicInfoEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            if (messageSend->DataBufferLength > 0)
            {
                basicInfo = *(FILE_BASIC_INFORMATION*)messageSend->DataBuffer;
            
				Description = L"FileBasicInformation,creation time:" + GetFileTimeStr(basicInfo.CreationTime.QuadPart);
				Description += L",last access time:" +  GetFileTimeStr(basicInfo.LastAccessTime.QuadPart);
				Description += L",last write time:" +  GetFileTimeStr(basicInfo.LastWriteTime.QuadPart);
				Description += L",file attributes:" +  std::to_wstring((ULONGLONG)basicInfo.FileAttributes);
			}

        }
    };

    class FileStandardInfoEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The file standard information
        /// </summary>
        FILE_STANDARD_INFORMATION standardInfo ;

		FileStandardInfoEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            if (messageSend->DataBufferLength > 0)
            {
                standardInfo = *(FILE_STANDARD_INFORMATION*)messageSend->DataBuffer;
           
				Description = L"FileStandardInformation,file size:" + std::to_wstring(standardInfo.EndOfFile.QuadPart);
				Description += L",lallocation size:" +  std::to_wstring(standardInfo.AllocationSize.QuadPart);
            }
        }
    };

    class FileNetworkInfoEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The file network information
        /// </summary>
        FILE_NETWORK_OPEN_INFORMATION networkInfo ;

		FileNetworkInfoEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            if (messageSend->DataBufferLength > 0)
            {
                networkInfo = *(FILE_NETWORK_OPEN_INFORMATION*)messageSend->DataBuffer;
           
				Description = L"FileNetworkOpenInformation,file size:" + std::to_wstring(networkInfo.EndOfFile.QuadPart);
				Description +=  L",file attributes:" +  std::to_wstring((ULONGLONG)networkInfo.FileAttributes);
            }
     
        }
        
    };

    class FileMoveOrRenameEventArgs : public FileIOEventArgs
    {
	public:
		/// <summary>
        /// The new file name of the move or rename IO
        /// </summary>
        std::wstring newFileName;

        FileMoveOrRenameEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            if (messageSend->DataBufferLength > 0)
            {
				newFileName.assign((WCHAR*)messageSend->DataBuffer);
				Description = L"File was renamed to " + newFileName;
            }
        }
    };

    class FileInfoArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The information class of the IO
        /// </summary>
        ULONG FileInfoClass ;
        /// <summary>
        /// The information data of the file associated to the info class.
        /// </summary>
        std::vector<UCHAR> buffer ;

		FileInfoArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            FileInfoClass = messageSend->InfoClass;

           if (messageSend->DataBufferLength > 0 && messageSend->DataBufferLength <= sizeof(messageSend->DataBuffer))
            {
				/*buffer.resize(messageSend->DataBufferLength);
				memcpy(buffer.data(), messageSend->DataBuffer, messageSend->DataBufferLength);*/
            }
       
			Description +=  L"FileInformationClass:" +  std::to_wstring((ULONGLONG)FileInfoClass);
        }
    };

    class FileSecurityEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The security information to be queried/set
        /// </summary>
        ULONG securityInformation ;
		/// <summary>
        /// The security data of the file associated to the info class.
        /// </summary>
        std::vector<UCHAR> buffer ;

		FileSecurityEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            securityInformation = messageSend->InfoClass;

           if (messageSend->DataBufferLength > 0 && messageSend->DataBufferLength <= sizeof(messageSend->DataBuffer))
            {
				/*buffer.resize(messageSend->DataBufferLength);
				memcpy(buffer.data(), messageSend->DataBuffer, messageSend->DataBufferLength);*/
            }
       
			Description +=  L"SecurityInformation:" +  std::to_wstring((ULONGLONG)securityInformation);
        }

    };

    class FileQueryDirectoryEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        /// The type of information to be returned about files in the directory.
        /// </summary>
        ULONG fileInfomationClass ;
        /// <summary>
        /// The buffer that receives the desired information about the file. 
        /// The structure of the information returned in the buffer is defined by the FileInformationClass.
        /// </summary>
        std::vector<UCHAR> buffer ;

		FileQueryDirectoryEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            fileInfomationClass = messageSend->InfoClass;

           if (messageSend->DataBufferLength > 0 && messageSend->DataBufferLength <= sizeof(messageSend->DataBuffer))
            {
				/*buffer.resize(messageSend->DataBufferLength);
				memcpy(buffer.data(), messageSend->DataBuffer, messageSend->DataBufferLength);*/
            }
       
			Description +=  L"Query directory fileInfomationClass:" +  std::to_wstring((ULONGLONG)fileInfomationClass);
        }
    };

    class DeniedFileIOEventArgs : public FileIOEventArgs
    {
	public:
		
		DeniedFileIOEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            EventName =  L"The I/O request " +  std::to_wstring(messageSend->MessageType) + L" was blocked.";

            if (messageSend->DataBufferLength > 0 && messageSend->InfoClass == ALLOW_FILE_RENAME)
            {
                std::wstring newFileName;
				newFileName.assign((WCHAR*)messageSend->DataBuffer);

                Description = L"Rename file to " + newFileName + L" was blocked by the setting.";
            }
            else
            {
                Description =	L"The IO request was blocked with the file access control flag " + std::to_wstring((ULONGLONG)messageSend->InfoClass);
            }
        }
    };

    class DeniedUSBReadEventArgs : public FileIOEventArgs
    {
	public:
		
		DeniedUSBReadEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            EventName = L"BlockUSBRead";
            Description = L"Reading file from USB was blocked by the setting.";
        }
    };

    class DeniedUSBWriteEventArgs : public FileIOEventArgs
    {
	public:
		
		DeniedUSBWriteEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            EventName = L"BlockUSBWrite";

            if (messageSend->InfoClass == ALLOW_COPY_PROTECTED_FILES_TO_USB)
            {
                Description = L"Copy the protected file to USB was blocked by the setting.";
            }
            else
            {
                Description = L"Writting the file to USB was blocked by the setting.";
            }
        }
    };

  
    class DeniedProcessTerminatedEventArgs : public FileIOEventArgs
    {
	public:
		DeniedProcessTerminatedEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            EventName = L"DeniedProcessTerminated";
            Description = L"Block killing process ungratefully.";
        }

    };

    class VolumeInfoEventArgs : public FileIOEventArgs
    {
	public:
		
		 /// <summary>
        /// the volume name
        /// </summary>
        std::wstring VolumeName;
        /// <summary>
        /// the volume dos name
        /// </summary>
        std::wstring  VolumeDosName;
        /// <summary>
        /// the volume file system type.
        /// </summary>
        ULONG VolumeFilesystemType;
        /// <summary>
        /// the Characteristics of the attached device object if existed. 
        /// </summary>
        ULONG DeviceCharacteristics;

		VolumeInfoEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            if (messageSend->DataBufferLength > 0)
            {
				PVOLUME_INFO volumeInfo = (PVOLUME_INFO)messageSend->DataBuffer;
                
				VolumeName.assign(volumeInfo->VolumeName);
				VolumeDosName.assign(volumeInfo->VolumeDosName);
                VolumeFilesystemType = volumeInfo->VolumeFilesystemType;
                DeviceCharacteristics = volumeInfo->DeviceCharacteristics;

		        Description = L"VolumeName" + VolumeName + L",VolumeDosName[" + VolumeDosName;
			}

        }
    };

	class ProcessEventArgs : public FileIOEventArgs
    {
	public:
		
		/// <summary>
        ///The process ID of the parent process for the new process. Note that the parent process is not necessarily the same process as the process that created the new process.  
        /// </summary>
        ULONG ParentProcessId;
        /// <summary>
        ///The process ID of the process that created the new process.
        /// </summary>
        ULONG CreatingProcessId;
        /// <summary>
        /// The thread ID of the thread that created the new process.
        /// </summary>
        ULONG CreatingThreadId;
        /// <summary>
        ///An ACCESS_MASK value that specifies the access rights to grant for the handle
        /// </summary>
        ULONG DesiredAccess;
        /// <summary>
        ///The type of handle operation. This member might be one of the following values:OB_OPERATION_HANDLE_CREATE,OB_OPERATION_HANDLE_DUPLICATE
        /// </summary>
        ULONG Operation;
        /// <summary>
        /// A Boolean value that specifies whether the ImageFileName member contains the exact file name that is used to open the process executable file.
        /// </summary>
        bool FileOpenNameAvailable;
        /// <summary>
        /// The file name of the executable. If the FileOpenNameAvailable member is TRUE, the string specifies the exact file name that is used to open the executable file. 
        /// If FileOpenNameAvailable is FALSE, the operating system might provide only a partial name.
        /// </summary>
        std::wstring ImageFileName;
        /// <summary>
        /// The command that is used to execute the process.
        /// </summary>
        std::wstring CommandLine;

		ProcessEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            ImageFileName = messageSend->FileName;

            if (messageSend->DataBufferLength > 0)
            {
                PROCESS_INFO* processInfo = (PROCESS_INFO*)messageSend->DataBuffer;

                ParentProcessId = processInfo->ParentProcessId;
                CreatingProcessId = processInfo->CreatingProcessId;
                CreatingThreadId = processInfo->CreatingThreadId;
                DesiredAccess = processInfo->DesiredAccess;
                Operation = processInfo->Operation;
                FileOpenNameAvailable = processInfo->FileOpenNameAvailable > 0;                
                CommandLine = processInfo->CommandLine;

                switch (messageSend->FilterCommand)
                {
                    case FILTER_SEND_PROCESS_CREATION_INFO:
                        {
                            Description = L"New process was created, parentPid:" + std::to_wstring((ULONGLONG)ParentProcessId) + L";CreatingPid:" + std::to_wstring((ULONGLONG)CreatingProcessId) 
								+ L";CreatingTheadId:" + std::to_wstring((ULONGLONG)CreatingThreadId) + L";CommandLine:" + CommandLine;

                            break;
                        }
					case FILTER_SEND_PROCESS_TERMINATION_INFO:
						{
							Description = L"The process was terminated.";
							break;
						}
					case FILTER_SEND_THREAD_CREATION_INFO:
						{
							Description = L"The new thread was created.";
							break;
						}
					case FILTER_SEND_THREAD_TERMINATION_INFO:
						{
							Description = L"The thread was terminated.";
							break;
						}
                    case FILTER_SEND_PROCESS_HANDLE_INFO:
                    case FILTER_SEND_THREAD_HANDLE_INFO:
                        {
                            if (processInfo->Operation == 1)
                            {
                                Description = L"Create Handle";
                            }
                            else
                            {
                                Description = L"Duplicate Handle";
                            }

                            Description += L"; DesiredAccess:" + std::to_wstring((ULONGLONG)processInfo->DesiredAccess);
                            break;
                        }

                }

            }
        }
     
    };

	class RegistryEventArgs : public FileIOEventArgs
    {
	public:
		  /// <summary>
        ///The registry callback class.  
        /// </summary>
        ULONGLONG RegCallbackClass;
        /// <summary>
        ///  The Key Information class.
        /// </summary>
        ULONG InfoClass;


		RegistryEventArgs(PMESSAGE_SEND_DATA messageSend) : FileIOEventArgs(messageSend)
        {
            RegCallbackClass = messageSend->MessageType;
            InfoClass = messageSend->InfoClass;

            if (messageSend->Status == STATUS_SUCCESS)
            {
                switch (RegCallbackClass)
                {
                    case Reg_Pre_Create_KeyEx:
                    case Reg_Post_Create_KeyEx:
                    case Reg_Pre_Open_KeyEx:
                    case Reg_Post_Open_KeyEx:
                        {
                            Description = L"Open registry key.";
                            break;
                        }
                    case Reg_Pre_Delete_Key:
                    case Reg_Post_Delete_Key:
                        {
                            Description = L"registry key is being deleted.";
                            break;
                        }
                    case Reg_Pre_Set_Value_Key:
                    case Reg_Post_Set_Value_Key:
                        {
                            Description = L"Set value key.";
                            break;
                        }
                    case Reg_Pre_Delete_Value_Key:
                    case Reg_Post_Delete_Value_Key:
                        {
                            Description = L"registry key's value is being deleted.";
                            break;
                        }
                    case Reg_Pre_SetInformation_Key:
                    case Reg_Post_SetInformation_Key:
                        {
                            Description = L"SetInformationKey";
                            break;
                        }
                    case Reg_Pre_Rename_Key:
                    case Reg_Post_Rename_Key:
                        {
                            std::wstring newName((WCHAR*)messageSend->DataBuffer);
                            Description = L"registry key's name is being changed to " + newName;
                            break;
                        }
                    case Reg_Pre_Enumerate_Key:
                    case Reg_Post_Enumerate_Key:
                        {
                            Description = L"EnumberateKey";
                            break;
                        }
                    case Reg_Pre_Enumerate_Value_Key:
                    case Reg_Post_Enumerate_Value_Key:
                        {
                            Description = L"EnumberateValueKey";
                            break;
                        }
                    case Reg_Pre_Query_Key:
					case Reg_Post_Query_Key:
                        {
                            Description = L"QueryKey";
                            break;
                        }
                    case Reg_Pre_Query_Value_Key:
                    case Reg_Post_Query_Value_Key:
                        {
                            Description = L"QueryValueKey";
                            break;
                        }
                    case Reg_Pre_Query_Multiple_Value_Key:
                    case Reg_Post_Query_Multiple_Value_Key:
                        {
                            Description = L"QueryMultipleValueKey";
                            break;
                        }
                  
                    case Reg_Pre_Load_Key:
                    case Reg_Post_Load_Key:
                        {
                            Description = L"LoadKey";
                            break;
                        }
                    case Reg_Pre_Replace_Key:
                    case Reg_Post_Replace_Key:
                        {
                            Description = L"ReplaceKey";
                            break;
                        }

                    case Reg_Pre_Query_KeyName:
                    case Reg_Post_Query_KeyName:
                        {
							Description = L"QueryKeyName";
                            break;
                        }
                }
            }

        }
      
    };

class FilterControl
{	  
private:

	static FilterControl *instance;

	bool isFilterStarted;
	int filterType;
	int filterConnectionThreads;
	int connectionTimeout;
	std::string licenseKey;
	
	static BOOL
	__stdcall
	MessageCallback(
	   IN		PMESSAGE_SEND_DATA pSendMessage,
	   IN OUT	PMESSAGE_REPLY_DATA pReplyMessage);

	static VOID
	__stdcall
	DisconnectCallback();

	BOOL
	SendConfigSettingsToFilter();

public:



	/// <summary>
	/// The global boolean config setting
	/// </summary>
	ULONG globalBooleanConfig;
	/// <summary>
	/// The volume control flag, reference VolumeControlFlag
	/// </summary>
	ULONG VolumeControlSettings;
	/// <summary>
	/// Prevent the protected process from being killed ungratefully.
	/// </summary>
	std::vector<ULONG> ProtectedProcessIdList;
	/// <summary>
	/// The List of the File Filter Rule
	/// </summary>
	std::vector<FileFilterRule> FileFilterRules;
	/// <summary>
	/// The List of the Process Filter Rule
	/// </summary>
	std::vector<ProcessFilterRule> ProcessFilterRules;
	/// <summary>
	/// The List of the Registry Filter Rule
	/// </summary>
	std::vector<RegistryFilterRule> RegistryFilterRules;

	// Private constructor so that no objects can be created.
	FilterControl()
	{
		globalBooleanConfig = 0;
		VolumeControlSettings = 0;
		isFilterStarted = false;
		filterType = FILE_SYSTEM_MONITOR;
		filterConnectionThreads = 5;
		connectionTimeout = 20;
	}

	static FilterControl *GetSingleInstance()
	{
		if (!instance)
		{
			instance = new FilterControl();
		}

		return instance;
	}

	BOOL StartFilter(int _filterType, int _filterConnectionThreads, int _connectionTimeout, CHAR* _licenseKey);

	void StopFilter();

	BOOL ClearConfigData(); 

	void ClearFilterRules()
	{
		FileFilterRules.clear();
		ProcessFilterRules.clear();
		RegistryFilterRules.clear();
	}

	VOID AddFileFilter(FileFilterRule fileFilterRule)
	{
		FileFilterRules.push_back(fileFilterRule);
	}

	VOID AddProcessFilter(ProcessFilterRule processFilterRule)
	{
		ProcessFilterRules.push_back(processFilterRule);
	}

	VOID AddRegistryFilter(RegistryFilterRule registryFilterRule)
	{
		RegistryFilterRules.push_back(registryFilterRule);
	}

	BOOL
	SendProcessFilterRuleToFilter(ProcessFilterRule* processFilter);

	BOOL
	SendRegistryFilterRuleToFilter(RegistryFilterRule* registryFilter);

	BOOL
	SendFileFilterRuleToFilter(FileFilterRule* fileFilter);

};

#endif