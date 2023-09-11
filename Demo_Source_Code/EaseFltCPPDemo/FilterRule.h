#ifndef __FILEFILTER_H__
#define __FILEFILTER_H__

#include "FilterAPI.h"
#include <vector>
#include <map>

static ULONG g_FilterRuleId = 0;

 class ProcessFilterRule
 {
		public:

		/// <summary>
        /// the id to identyfy this filter rule. 
        /// </summary>
        ULONG FilterRuleId;
		/// <summary>
        /// the process name filter mask, i.e. "notepad.exe","c:\\windows\\*.exe" or "*"
        /// </summary>
        std::wstring ProcessNameFilterMask;
        /// <summary>
        /// The control flag to the processes which match the processNameFilterMask
        /// </summary>
        ULONG ControlFlag;
		/// <summary>
		/// The List of the file access rights of the process.
		/// </summary>
		std::map<std::wstring, ULONG> FileAccessList;

        ProcessFilterRule(WCHAR* filterMask)
		{
			FilterRuleId = ++g_FilterRuleId;
			ProcessNameFilterMask.assign(filterMask);
			ControlFlag = 0;
		}

		void AddFileAccessRightsToProcess(WCHAR* _fileFilterMask, ULONG accessFlag)
		{
			std::wstring fileFilterMask(_fileFilterMask);
			FileAccessList.insert(std::pair<std::wstring, ULONG >(fileFilterMask, accessFlag));

			return;
		}
 };

  class RegistryFilterRule
  {
	public:
		/// <summary>
        /// the id to identyfy this filter rule. 
        /// </summary>
        ULONG FilterRuleId;
        /// <summary>
        /// Control the registry access for the process with this process Id. 
        /// </summary>
        ULONG ProcessId;
        /// <summary>
        /// Control the registry access for the process with this process name if the process Id is 0, or it will skip it. 
        /// </summary>
        std::wstring ProcessNameFilterMask;
        /// <summary>
        /// Control the registry access for the process with this user name
        /// </summary>
        std::wstring UserName;
        /// <summary>
        /// Filter the registry access based on the key name filter mask if it was set
        /// </summary>
        std::wstring RegistryKeyNameFilterMask;
        /// <summary>
        /// The the flag to control how to access the registry for the matched process or user
        /// </summary>
        ULONG ControlFlag;
        /// <summary>
        /// Register the callback when the registry access notification was triggered
        /// </summary>
        ULONGLONG RegCallbackClass;
        /// <summary>
        /// If it is true, the registry access from the matched process or user will be excluded.
        /// </summary>
        BOOL IsExcludeFilter;

		RegistryFilterRule(WCHAR* filterMask)
		{
			FilterRuleId = ++g_FilterRuleId;
			ProcessNameFilterMask.assign(filterMask);
			ProcessId = 0;
			ControlFlag = 0;
			RegCallbackClass = 0;
			IsExcludeFilter = FALSE;
		}
  };

class FileFilterRule
{
public:

	/// <summary>
    /// the id to identyfy this filter rule. 
    /// </summary>
    ULONG FilterRuleId;
	/// <summary>
    /// The file filter mask of this filter rule 
    /// </summary>
    std::wstring FileFilterMask;

    /// <summary>
    /// Skip all the file I/Os when the file open doesn't match with below options when they are not 0.
	/// Please reference the CreateFile API for below options.
    /// </summary>
    ULONG FilterDesiredAccess;
    ULONG FilterDisposition;
    ULONG FilterCreateOptions;

	/// <summary>
    /// if hidden file was enabled, you can add the hidden file filter mask here.
    /// </summary>
    std::vector <std::wstring> HiddenFileFilterMaskList;

	/// <summary>
    /// if reparse file was enabled, you can add reparse file filter mask here.
    /// </summary>
    std::wstring ReparseFileFilterMask;

    /// <summary>
    /// Filter the file I/Os based on the filename, process name,Id, user name.
    /// </summary>
    std::vector<std::wstring> ExcludeFileFilterMaskList;
    std::vector<std::wstring> IncludeProcessNameList;
    std::vector<std::wstring> ExcludeProcessNameList;
    std::vector<ULONG> IncludeProcessIdList;
    std::vector<ULONG> ExcludeProcessIdList;
    std::vector<std::wstring> IncludeUserNameList;
    std::vector<std::wstring> ExcludeUserNameList;

    /// <summary>
    /// Enable the control file filter rule in boot time.
    /// </summary>
    bool IsResident;

    /// <summary>
    /// the bool config setting of the filter, reference FilterAPI.boolConfig
    /// </summary>
    ULONG BooleanConfig;
		
	///--------------control filter only properties start--------------------------
	/// <summary>
    /// register the callback control IO based on the filter setting.
    /// </summary>
    ULONGLONG ControlFileIOEventFilter;

    /// <summary>
    /// the control flag of the filter
    /// reference FilterAPI.AccessFlag enumeration
    /// </summary>
    ULONG AccessFlag;

    /// <summary>
    /// the access right of the process 
    /// </summary>
    std::map<std::wstring, ULONG> ProcessNameAccessRightList;
    std::map<ULONG, ULONG> ProcessIdAccessRightList;

    /// <summary>
    /// the access right of the users
    /// </summary>
    std::map<std::wstring, ULONG> UserNameAccessRightList;

	///--------------control filter only properties end--------------------------

	///--------------monitor filter only properties start--------------------------
	/// <summary>         
    /// Register the file change event for monitor filter, you will get the notification if the file was changed, 
	/// register the file events, trigger the notifcation when the events were happened after the file was closed,
    /// you can choose the events from FileEventType to register.
    /// </summary>
    /// </summary>
    ULONG FileChangeEventFilter;

    /// <summary>
    /// register the monitor notification of the IOs, this is 16 bytes integer,        
    /// </summary>
    ULONGLONG MonitorFileIOEventFilter;
    /// <summary>
    /// the maximum buffer size for monitor events if it was enabled.
    /// </summary>
    ULONG MaxEventBufferSize;//50MB

	///--------------monitor filter only properties end--------------------------
		
	/// <summary>
    /// Encryption filter only property, if the encryption was enabled, this is the encryption key will be used for the encrytped file.
    /// </summary>
	std::vector<BYTE> EncryptionKey;

	FileFilterRule(WCHAR* filterMask)
	{
		FilterRuleId = ++g_FilterRuleId;
		FileFilterMask.assign(filterMask);
		FilterDesiredAccess = 0;
		FilterDisposition = 0;
		FilterCreateOptions = 0;
		IsResident = false;
		BooleanConfig = 0;
		ControlFileIOEventFilter = 0;
		AccessFlag = ALLOW_MAX_RIGHT_ACCESS;
		FileChangeEventFilter = 0;
		MonitorFileIOEventFilter = 0;
			
		MaxEventBufferSize = 1024*1024*50;//50MB


	}

	//to check if the encryption is enabled for this filter rule.
	bool IsEncryptionEnabled()
	{
		if( (AccessFlag & ENABLE_FILE_ENCRYPTION_RULE) > 0 )
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	//to check if the reparse file is enabled for this filter rule.
	bool IsReparseFileEnabled()
	{
		if( (AccessFlag & ENABLE_REPARSE_FILE_OPEN) > 0 )
		{
			return true;
		}
		else
		{
			return false;
		}
	}


	//to check if the hidden file is enabled for this filter rule.
	bool IsHiddenFileEnabled()
	{
		if( (AccessFlag & ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) > 0 )
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	bool AddHiddenFileFilterMask(WCHAR* _hiddenFileFilterMask)
	{
		if(NULL == _hiddenFileFilterMask)
		{
			return false;
		}

		std::wstring hiddenFileFilterMask(_hiddenFileFilterMask);
		HiddenFileFilterMaskList.push_back(hiddenFileFilterMask);

		return true;
	}

	bool AddExcludeFileFilterMask(WCHAR* _excludeFileFilterMask)
	{
		if(NULL == _excludeFileFilterMask)
		{
			return false;
		}

		std::wstring excludeFileFilterMask(_excludeFileFilterMask);
		ExcludeFileFilterMaskList.push_back(excludeFileFilterMask);

		return true;
	}

	bool AddIncludeProcessName(WCHAR* _processName)
	{
		if(NULL == _processName)
		{
			return false;
		}

		std::wstring processName(_processName);
		IncludeProcessNameList.push_back(processName);

		return true;
	}

	bool AddExcludeProcessName(WCHAR* _processName)
	{
		if(NULL == _processName)
		{
			return false;
		}

		std::wstring processName(_processName);
		ExcludeProcessNameList.push_back(processName);

		return true;
	}

	void AddIncludeProcessId(ULONG processId)
	{
		IncludeProcessIdList.push_back(processId);
		return ;
	}

	void AddExcludeProcessId(ULONG processId)
	{
		ExcludeProcessIdList.push_back(processId);
		return ;
	}

	bool AddIncludeUserName(WCHAR* _userName)
	{
		if(NULL == _userName)
		{
			return false;
		}

		std::wstring userName(_userName);
		IncludeUserNameList.push_back(userName);

		return true;
	}

	bool AddExcludeUserName(WCHAR* _userName)
	{
		if(NULL == _userName)
		{
			return false;
		}

		std::wstring userName(_userName);
		ExcludeUserNameList.push_back(userName);

		return true;
	}
		
	bool set_EncryptionKey(UCHAR* _encryptionKey,ULONG keyLength)
	{
		if(		NULL == _encryptionKey
			||	(keyLength != 16 && keyLength != 24 && keyLength != 32))
		{
			//this is not valid key.
			printf("The key length:%d is invalid, it has to be 16, 24 or 32 bytes\n");
			return false;
		}

		EncryptionKey.clear();
		EncryptionKey.assign(_encryptionKey, _encryptionKey + keyLength);

		return true;
	}


	bool AddAccessRightsToProcessName(WCHAR* _processName, ULONG accessFlag)
	{
		if( NULL == _processName )
		{
			return false;
		}

		std::wstring processName(_processName);
		ProcessNameAccessRightList.insert(std::pair<std::wstring, ULONG >(processName, accessFlag));

		return true;
	}

	void AddAccessRightsToProcessId(ULONG _processId, ULONG accessFlag)
	{
		ProcessIdAccessRightList.insert(std::pair<ULONG, ULONG >(_processId, accessFlag));
		return;
	}

	bool AddAccessRightsToUserName(WCHAR* _userName, ULONG accessFlag)
	{
		if( NULL == _userName )
		{
			return false;
		}

		std::wstring userName(_userName);
		UserNameAccessRightList.insert(std::pair<std::wstring, ULONG >(userName, accessFlag));

		return true;
	}

};




#endif