#ifndef __FILEMONITOR_H__
#define __FILEMONITOR_H__

#include "FilterAPI.h"
#include "FilterMessage.h"

VOID
SendFileFilterNotification(PMESSAGE_SEND_DATA messageSend)
{
	if (messageSend->FilterCommand == FILTER_SEND_ATTACHED_VOLUME_INFO)
    {
         VolumeInfoEventArgs* volumeArgs = new VolumeInfoEventArgs(messageSend);
         volumeArgs->EventName = L"VolumeAttached";
		 DisplayFileIOMessage(volumeArgs);
    }
    else if (messageSend->FilterCommand == FILTER_SEND_DETACHED_VOLUME_INFO)
    {
         VolumeInfoEventArgs* volumeArgs = new VolumeInfoEventArgs(messageSend);
         volumeArgs->EventName = L"VolumeDetached";
         DisplayFileIOMessage(volumeArgs);
    }
    else if (messageSend->FilterCommand == FILTER_SEND_DENIED_VOLUME_DISMOUNT_EVENT)
    {
         VolumeInfoEventArgs* volumeArgs = new VolumeInfoEventArgs(messageSend);
         volumeArgs->EventName = L"VolumeDismountWasBlocked";
         DisplayFileIOMessage(volumeArgs);
    }
    else if (messageSend->FilterCommand == FILTER_SEND_DENIED_FILE_IO_EVENT)
    {
         DeniedFileIOEventArgs* deniedIOEventArgs  = new DeniedFileIOEventArgs(messageSend);
		 DisplayFileIOMessage(deniedIOEventArgs);
    }
    else if (messageSend->FilterCommand == FILTER_SEND_DENIED_USB_READ_EVENT)
    {
         DeniedUSBReadEventArgs* deniedIOEventArgs  = new DeniedUSBReadEventArgs(messageSend);
         DisplayFileIOMessage(deniedIOEventArgs);
    }
    else if (messageSend->FilterCommand == FILTER_SEND_DENIED_USB_WRITE_EVENT)
    {
         DeniedUSBWriteEventArgs* deniedIOEventArgs  = new DeniedUSBWriteEventArgs(messageSend);
         DisplayFileIOMessage(deniedIOEventArgs);
    }
    else if (messageSend->FilterCommand == FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT)
    {
         DeniedProcessTerminatedEventArgs* deniedProcessEventArgs  = new DeniedProcessTerminatedEventArgs(messageSend);
         DisplayFileIOMessage(deniedProcessEventArgs);
    }
	else if (messageSend->FilterCommand == FILTER_SEND_FILE_CHANGED_EVENT)
    {                    
         FileChangedEventArgs* fileChangeEventArgs  = new FileChangedEventArgs(messageSend);
		 DisplayFileIOMessage(fileChangeEventArgs);
    }
    else if (messageSend->MessageType == POST_CREATE)
    {
        FileCreateEventArgs* fileCreateEventArgs  = new FileCreateEventArgs(messageSend);
       /* if (fileCreateEventArgs->isNewFileCreated )
        {
            fileCreateEventArgs->EventName = L"OnNewFileCreated";
        }
        else
        {
            fileCreateEventArgs->EventName = L"OnFileOpen";
        }

        if (fileCreateEventArgs->isDeleteOnClose)
        {
            fileCreateEventArgs->EventName = L"OnOpenFileWithDeleteOnClose";
        }*/

		//DisplayFileIOMessage(fileCreateEventArgs);

    }
    else if (messageSend->MessageType == PRE_CACHE_READ
            || messageSend->MessageType == PRE_FASTIO_READ
            || messageSend->MessageType == PRE_NOCACHE_READ
            || messageSend->MessageType == PRE_PAGING_IO_READ)
    {
         FileReadEventArgs* fileReadEventArgs  = new FileReadEventArgs(messageSend);
         fileReadEventArgs->EventName = L"OnPreFileRead-" + fileReadEventArgs->readType;
		 DisplayFileIOMessage(fileReadEventArgs);
    }
    else if (messageSend->MessageType == POST_CACHE_READ
        || messageSend->MessageType == POST_FASTIO_READ
        || messageSend->MessageType == POST_NOCACHE_READ
        || messageSend->MessageType == POST_PAGING_IO_READ)
    {
         FileReadEventArgs* fileReadEventArgs  = new FileReadEventArgs(messageSend);
         fileReadEventArgs->EventName = L"OnFileRead-" + fileReadEventArgs->readType;
		 DisplayFileIOMessage(fileReadEventArgs);
    }
    else if (messageSend->MessageType == PRE_CACHE_WRITE
            || messageSend->MessageType == PRE_FASTIO_WRITE
            || messageSend->MessageType == PRE_NOCACHE_WRITE
            || messageSend->MessageType == PRE_PAGING_IO_WRITE)
    {
		FileWriteEventArgs* fileWriteEventArgs  = new FileWriteEventArgs(messageSend);
        fileWriteEventArgs->EventName = L"OnPreFileWrite-" + fileWriteEventArgs->writeType;
		DisplayFileIOMessage(fileWriteEventArgs);
    }
    else if (messageSend->MessageType == POST_CACHE_WRITE
    || messageSend->MessageType == POST_FASTIO_WRITE
    || messageSend->MessageType == POST_NOCACHE_WRITE
    || messageSend->MessageType == POST_PAGING_IO_WRITE)
    {
		FileWriteEventArgs* fileWriteEventArgs  = new FileWriteEventArgs(messageSend);
        fileWriteEventArgs->EventName = L"OnFileWrite-" + fileWriteEventArgs->writeType;
		DisplayFileIOMessage(fileWriteEventArgs);
    }
    else if (messageSend->MessageType == POST_QUERY_INFORMATION)
    {
        if (messageSend->InfoClass == FileEndOfFileInformation)
        {
            FileSizeEventArgs*  fileSizeArgs = new FileSizeEventArgs(messageSend);
            fileSizeArgs->EventName = L"OnQueryFileSize";
			DisplayFileIOMessage(fileSizeArgs);
        }
        else if (messageSend->InfoClass == FileInternalInformation)
        {
			FileIdEventArgs*  fileIdArgs = new FileIdEventArgs(messageSend);
            fileIdArgs->EventName = L"OnQueryFileId";
			DisplayFileIOMessage(fileIdArgs);

        }
        else if (messageSend->InfoClass == FileBasicInformation)
        {
			FileBasicInfoEventArgs*  fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
            fileBasicInfoArgs->EventName = L"OnQueryFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);
        }
        else if (messageSend->InfoClass == FileStandardInformation)
        {
			FileStandardInfoEventArgs*  fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
            fileStandardInfoArgs->EventName = L"OnQueryFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);
        }
        else if (messageSend->InfoClass == FileNetworkOpenInformation)
        {
			FileNetworkInfoEventArgs*  fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
            fileNetworkInfoArgs->EventName = L"OnQueryFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);
        }
        else 
        {
			FileInfoArgs*  fileInfoArgs = new FileInfoArgs(messageSend);
            fileInfoArgs->EventName = L"OnQueryFileInfo";
			DisplayFileIOMessage(fileInfoArgs);
        }
    }
    else if (messageSend->MessageType == POST_SET_INFORMATION)
    {
        if (messageSend->InfoClass == FileEndOfFileInformation)
        {
			FileSizeEventArgs*  fileSizeArgs = new FileSizeEventArgs(messageSend);
            fileSizeArgs->EventName = L"OnSetFileSize";
			DisplayFileIOMessage(fileSizeArgs);
        }
        else if (messageSend->InfoClass == FileBasicInformation)
        {
			FileBasicInfoEventArgs*  fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
            fileBasicInfoArgs->EventName = L"OnSetFileBasicInfo";
			DisplayFileIOMessage(fileBasicInfoArgs);
         
        }
        else if (messageSend->InfoClass == FileStandardInformation)
        {
			FileStandardInfoEventArgs*  fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
            fileStandardInfoArgs->EventName = L"OnSetFileStandardInfo";
			DisplayFileIOMessage(fileStandardInfoArgs);

        }
        else if (messageSend->InfoClass == FileNetworkOpenInformation)
        {
			FileNetworkInfoEventArgs*  fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
            fileNetworkInfoArgs->EventName = L"OnSetFileNetworkInfo";
			DisplayFileIOMessage(fileNetworkInfoArgs);
        }
        else if (messageSend->InfoClass == FileRenameInformation
            || messageSend->InfoClass == FileRenameInformationEx)
        {
			FileMoveOrRenameEventArgs*  fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
            fileRenameArgs->EventName = L"OnMoveOrRenameFile";
			DisplayFileIOMessage(fileRenameArgs);
        }
        else if (messageSend->InfoClass == FileDispositionInformation
            || messageSend->InfoClass == FileDispositionInformationEx)
        {
			FileIOEventArgs*  fileIOArgs = new FileIOEventArgs(messageSend);
            fileIOArgs->EventName = L"OnDeleteFile";
			fileIOArgs->Description = L"The file was deleted.";
			DisplayFileIOMessage(fileIOArgs);

        }
        else 
        {
			FileInfoArgs*  fileInfoArgs = new FileInfoArgs(messageSend);
            fileInfoArgs->EventName = L"OnSetFileInfo";
			DisplayFileIOMessage(fileInfoArgs);
        }

    }
    else if (messageSend->MessageType == POST_QUERY_SECURITY)
	{
		FileSecurityEventArgs*  fileSecurityArgs = new FileSecurityEventArgs(messageSend);
        fileSecurityArgs->EventName = L"OnQueryFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);
    }
    else if (messageSend->MessageType == POST_SET_SECURITY)
    {
		FileSecurityEventArgs*  fileSecurityArgs = new FileSecurityEventArgs(messageSend);
        fileSecurityArgs->EventName = L"OnSetFileSecurity";
		DisplayFileIOMessage(fileSecurityArgs);

    }
    else if (messageSend->MessageType == POST_DIRECTORY)
    {
		FileQueryDirectoryEventArgs*  directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
        directoryArgs->EventName = L"OnQueryDirectoryFile";
		DisplayFileIOMessage(directoryArgs);
    }
    else if (messageSend->MessageType == POST_CLEANUP)
    {
		FileIOEventArgs*  fileIOArgs = new FileIOEventArgs(messageSend);
        fileIOArgs->EventName = L"OnFileHandleClose";
		fileIOArgs->Description = L"The opened file handle was closed.";
		DisplayFileIOMessage(fileIOArgs);

    }
    else if (messageSend->MessageType == (ULONG)POST_CLOSE)
    {
		FileIOEventArgs*  fileIOArgs = new FileIOEventArgs(messageSend);
        fileIOArgs->EventName = L"OnFileClose";
		fileIOArgs->Description = L"All system references to the fileObject were closed.";
		DisplayFileIOMessage(fileIOArgs);

    }
    else
    {
		FileIOEventArgs*  fileIOArgs = new FileIOEventArgs(messageSend);
        fileIOArgs->EventName = L"UNKNOWN" ;
		fileIOArgs->Description = L"MessageType:" + std::to_wstring(messageSend->MessageType) + L" can't be handled.";
		DisplayFileIOMessage(fileIOArgs);

    }
}




#endif