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
#include "EncryptionHandler.h"
#include "FileControlHandler.h"
#include "FilterRule.h"
#include "FileMonitorHandler.h"
#include "ProcessFilterHandler.h"
#include "RegistryFilterHandler.h"
#include "FilterControl.h"

FilterControl* FilterControl::instance = NULL;


BOOL 
FilterControl::StartFilter(int _filterType, int _filterConnectionThreads, int _connectionTimeout, CHAR* _licenseKey)
{
	BOOL retVal = false;

	filterType = _filterType;
	filterConnectionThreads = _filterConnectionThreads;
	connectionTimeout = _connectionTimeout;
	licenseKey.assign(_licenseKey);
	
	if(isFilterStarted)
	{
		return true;
	}

	if (!IsDriverServiceRunning())
    {
        if(!InstallDriver())
        {
            PrintLastErrorMessage(L"Installed driver failed.");
            return false;
        }
    }

	if(!SetRegistrationKey(&*licenseKey.begin()))
	{
		PrintLastErrorMessage( L"SetRegistrationKey failed.");
		return false;
	}

	if(!RegisterMessageCallback(filterConnectionThreads,MessageCallback,DisconnectCallback))
	{
		PrintLastErrorMessage( L"RegisterMessageCallback failed.");
		return false;
	}

	if(!SetConnectionTimeout(connectionTimeout))     
	{
		PrintLastErrorMessage( L"SetConnectionTimeout failed.");
		return false;
	}

    if(!SetFilterType(filterType))
	{
		PrintLastErrorMessage( L"SetFilterType failed.");
		return false;
	}

	isFilterStarted = true;

	SendConfigSettingsToFilter();

	return true;
}

/// <summary>
/// Stop the filter driver service.
/// </summary>
void FilterControl::StopFilter()
{
	if (isFilterStarted)
	{
		printf("Stop Filter Service.\n");
		Disconnect();
		isFilterStarted = false;
	}

	return;
}

BOOL
FilterControl::ClearConfigData()
{
	if (!ResetConfigData())
    {
        PrintLastErrorMessage(L"ResetConfigData failed.");
        return false;
    }

	return true;
}


BOOL
FilterControl::SendFileFilterRuleToFilter(FileFilterRule* fileFilter)
{
	//add filter rule to filter driver here, the filter rule is unique with the include file filter mask.
	//you can't have the mutiple filter rules with the same file filter mask,if there are the same 
	//one exist, the new one with accessFlag will overwrite the old accessFlag.
	//for control filter, if isResident is true, the access control will be enabled in boot time.
	if (!AddFileFilterRule(fileFilter->AccessFlag, &fileFilter->FileFilterMask[0], fileFilter->IsResident, fileFilter->FilterRuleId))
    {
        PrintLastErrorMessage(L"Send file filter rule failed.");
        return false;
    }

    if (fileFilter->FileChangeEventFilter > 0 && !RegisterFileChangedEventsToFilterRule(&fileFilter->FileFilterMask[0], fileFilter->FileChangeEventFilter))
    {
        PrintLastErrorMessage(L"Register file change event failed.");
        return false;
    }

	if (fileFilter->MonitorFileIOEventFilter > 0 && !RegisterMonitorIOToFilterRule(&fileFilter->FileFilterMask[0], fileFilter->MonitorFileIOEventFilter))
    {
        PrintLastErrorMessage(L"Register monitor IO event failed.");
        return false;
    }

	if (fileFilter->ControlFileIOEventFilter > 0 && !RegisterControlIOToFilterRule(&fileFilter->FileFilterMask[0], fileFilter->ControlFileIOEventFilter))
    {
        PrintLastErrorMessage(L"Register control IO event failed.");
        return false;
    }

    if (fileFilter->FilterDesiredAccess > 0 || fileFilter->FilterDisposition > 0 || fileFilter->FilterCreateOptions > 0)
    {
        if (!AddCreateFilterToFilterRule(&fileFilter->FileFilterMask[0], fileFilter->FilterDesiredAccess, fileFilter->FilterDisposition, fileFilter->FilterCreateOptions))
        {
            PrintLastErrorMessage(L"AddCreateFilterToFilterRule failed.");
            return false;
        }
    }         

	//every filter rule can have multiple exclude file filter masks, you can exclude the files 
    //which matches the exclude filter mask.
	for(std::vector<std::wstring>::iterator excludeFilterMask = fileFilter->ExcludeFileFilterMaskList.begin(); 
		excludeFilterMask != fileFilter->ExcludeFileFilterMaskList.end(); ++excludeFilterMask) 
    {
		if (excludeFilterMask->length() > 0 )
        {
            if (!AddExcludeFileMaskToFilterRule(&fileFilter->FileFilterMask[0], &(*excludeFilterMask)[0]))
            {
                PrintLastErrorMessage(L"AddExcludeFileMaskToFilterRule failed.");
                return false;
            }
        }
    }

	//only the I/O from the included processes can be managed by this filter rule, all other processes will pass through.
	for(std::vector<std::wstring>::iterator includeProcessName = fileFilter->IncludeProcessNameList.begin(); 
		includeProcessName != fileFilter->IncludeProcessNameList.end(); ++includeProcessName) 
    {
		if (includeProcessName->length() > 0)
        {
            if (!AddIncludeProcessNameToFilterRule(&fileFilter->FileFilterMask[0], &(*includeProcessName)[0]))
            {
                PrintLastErrorMessage(L"AddIncludeProcessNameToFilterRule failed.");
                return false;
            }
        }
    }

	//all the I/O from the excluded processes will pass through, won't be intercepted by filter driver.
	for(std::vector<std::wstring>::iterator excludeProcessName = fileFilter->ExcludeProcessNameList.begin(); 
		excludeProcessName != fileFilter->ExcludeProcessNameList.end(); ++excludeProcessName) 
    {
		if (excludeProcessName->length() > 0)
        {
            if (!AddExcludeProcessNameToFilterRule(&fileFilter->FileFilterMask[0], &(*excludeProcessName)[0]))
            {
                PrintLastErrorMessage(L"AddExcludeProcessNameToFilterRule failed.");
                return false;
            }
        }
    }

	//only the I/O from the included processes can be managed by this filter rule, all other processes will pass through.
	for(std::vector<ULONG>::iterator includeProcessId = fileFilter->IncludeProcessIdList.begin(); 
		includeProcessId != fileFilter->IncludeProcessIdList.end(); ++includeProcessId) 
    {
            if (!AddIncludeProcessIdToFilterRule(&fileFilter->FileFilterMask[0], *includeProcessId))
        {
            PrintLastErrorMessage(L"AddIncludeProcessIdToFilterRule failed.");
            return false;
        }
    }

	//all the I/O from the excluded processes will pass through, won't be intercepted by filter driver.
	for(std::vector<ULONG>::iterator excludeProcessId = fileFilter->ExcludeProcessIdList.begin(); 
		excludeProcessId != fileFilter->ExcludeProcessIdList.end(); ++excludeProcessId) 
    {
        if (!AddExcludeProcessIdToFilterRule(&fileFilter->FileFilterMask[0], *excludeProcessId))
        {
            PrintLastErrorMessage(L"AddExcludeProcessIdToFilterRule failed.");
            return false;
        }
    }

	//only the I/O from the included users can be managed by this filter rule, all other users will pass through.
	for(std::vector<std::wstring>::iterator includeUserName = fileFilter->IncludeUserNameList.begin(); 
		includeUserName != fileFilter->IncludeUserNameList.end(); ++includeUserName) 
    {
		if (includeUserName->length() > 0)
        {
            if (!AddIncludeUserNameToFilterRule(&fileFilter->FileFilterMask[0], &(*includeUserName)[0]))
            {
                PrintLastErrorMessage(L"AddIncludeUserNameToFilterRule failed.");
                return false;
            }
        }
    }   

	//all the I/O from the excluded user will pass through, won't be intercepted by filter driver.
	for(std::vector<std::wstring>::iterator excludeUserName = fileFilter->ExcludeUserNameList.begin(); 
		excludeUserName != fileFilter->ExcludeUserNameList.end(); ++excludeUserName) 
    {
		if (excludeUserName->length() > 0)
        {
            if (!AddExcludeUserNameToFilterRule(&fileFilter->FileFilterMask[0], &(*excludeUserName)[0]))
            {
                PrintLastErrorMessage(L"AddExcludeUserNameToFilterRule failed.");
                return false;
            }
        }
    }

    //set the access rights to the specific user, you can add or remove the user access rights here.
    std::map<std::wstring, ULONG>::iterator it = fileFilter->UserNameAccessRightList.begin();
    while (it != fileFilter->UserNameAccessRightList.end())
    {
        std::wstring userName = it->first;
        ULONG accessFlag = it->second;

        if (!AddUserRightsToFilterRule(&fileFilter->FileFilterMask[0], &userName[0], accessFlag))
        {
            PrintLastErrorMessage(L"AddUserRightsToFilterRule failed.");
            return false;
        }

        ++it;
    }

	//set the access rights to the specific process, you can add or remove the process access rights here.
	it = fileFilter->ProcessNameAccessRightList.begin();
    while (it != fileFilter->ProcessNameAccessRightList.end())
    {
		std::wstring processName = it->first;
		ULONG accessFlag = it->second;

        if (!AddProcessRightsToFilterRule(&fileFilter->FileFilterMask[0],&processName[0], accessFlag))
        {
            PrintLastErrorMessage(L"AddProcessRightsToFilterRule failed.");
            return false;
        }

		++it;
    }

	//set the access rights to the specific process, you can add or remove the process access rights here.
	std::map<ULONG, ULONG>::iterator pidRights = fileFilter->ProcessIdAccessRightList.begin();
    while (pidRights != fileFilter->ProcessIdAccessRightList.end())
    {
		ULONG pid = pidRights->first;
		ULONG accessFlag = pidRights->second;

        if (!AddProcessIdRightsToFilterRule(&fileFilter->FileFilterMask[0],pid, accessFlag))
        {
            PrintLastErrorMessage(L"AddProcessRightsToFilterRule failed.");
            return false;
        }

		++pidRights;
    }
        
    //Hide the files which match the hidden file filter masks when the user browse the managed directory.
    if (fileFilter->IsHiddenFileEnabled())
    {
		for(std::vector<std::wstring>::iterator hiddenFilterMask = fileFilter->HiddenFileFilterMaskList.begin(); 
		hiddenFilterMask != fileFilter->HiddenFileFilterMaskList.end(); ++hiddenFilterMask) 
        if (hiddenFilterMask->length() > 0)
        {
            if (!AddHiddenFileMaskToFilterRule(&fileFilter->FileFilterMask[0], &(*hiddenFilterMask)[0]))
            {
                PrintLastErrorMessage(L"AddHiddenFileMaskToFilterRule failed.");
                return false;
            }
        }
    }

    //reparse the file open to another file with the filter mask.
    //For example:
    //FilterMask = c:\test\*txt
    //ReparseFilterMask = d:\reparse\*doc
    //If you open file c:\test\MyTest.txt, it will reparse to the file d:\reparse\MyTest.doc.
    if (fileFilter->IsReparseFileEnabled())
    {
        if (!AddReparseFileMaskToFilterRule(&fileFilter->FileFilterMask[0], &fileFilter->ReparseFileFilterMask[0]))
        {
            PrintLastErrorMessage(L"AddReparseFileMaskToFilterRule failed.");
            return false;
        }

    }

	if (fileFilter->IsEncryptionEnabled())
    {
		if ( fileFilter->EncryptionKey.size() > 0)
        {
            if (!AddEncryptionKeyToFilterRule(&fileFilter->FileFilterMask[0],(ULONG)fileFilter->EncryptionKey.size(), &fileFilter->EncryptionKey[0]))
            {
				PrintLastErrorMessage(L"AddEncryptionKeyToFilterRule failed.");
                return false;
            }
        }
		else
		{
			printf("Encryption key is not set, it will get the encryption key from callback function all the time.\n");

			//if you didn't set the master encryption key, then require the encryption key from callback function all the time.
			fileFilter->BooleanConfig |= REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE;
		}
    }

    if (fileFilter->BooleanConfig > 0 && !AddBooleanConfigToFilterRule(&fileFilter->FileFilterMask[0], fileFilter->BooleanConfig))
    {
        PrintLastErrorMessage(L"AddBooleanConfigToFilterRule  failed.");
        return false;
    }

	return true;

}

BOOL
FilterControl::SendProcessFilterRuleToFilter(ProcessFilterRule* processFilter)
{
	if (processFilter->ProcessNameFilterMask.length() > 0)
	{
		//set the process control flag for the process filter mask. 
		if (!AddProcessFilterRule((ULONG)processFilter->ProcessNameFilterMask.length() * 2, &processFilter->ProcessNameFilterMask[0], processFilter->ControlFlag, processFilter->FilterRuleId))
		{
			PrintLastErrorMessage(L"AddProcessFilterRule failed.");
			return false;
		}

		//set the file access rights of the file filter mask to the process
		std::map<std::wstring, ULONG>::iterator it = processFilter->FileAccessList.begin();
		while (it != processFilter->FileAccessList.end())
		{
			std::wstring fileFilterMask = it->first;
			ULONG accessFlag = it->second;

			if(!AddFileControlToProcessByName((ULONG)processFilter->ProcessNameFilterMask.length() * 2, &processFilter->ProcessNameFilterMask[0],
				(ULONG)(fileFilterMask.length()) * 2, &(fileFilterMask[0]), accessFlag))
			{
				PrintLastErrorMessage(L"AddProcessRightsToFilterRule failed.");
				return false;
			}

			++it;
		}

        for (std::vector<std::wstring>::iterator excludeProcessName = processFilter->ExcludeProcessNameList.begin();
            excludeProcessName != processFilter->ExcludeProcessNameList.end(); ++excludeProcessName)
        {
            if (excludeProcessName->length() > 0)
            {
                if (!AddExcludeProcessNameToProcessFilterRule(&processFilter->ProcessNameFilterMask[0], &(*excludeProcessName)[0]))
                {
                    PrintLastErrorMessage(L"AddExcludeProcessNameToProcessFilterRule failed.");
                    return false;
                }
            }
        }


        for (std::vector<std::wstring>::iterator excludeUserName = processFilter->ExcludeUserNameList.begin();
            excludeUserName != processFilter->ExcludeUserNameList.end(); ++excludeUserName)
        {
            if (excludeUserName->length() > 0)
            {
                if (!AddExcludeUserNameToProcessFilterRule(&processFilter->ProcessNameFilterMask[0], &(*excludeUserName)[0]))
                {
                    PrintLastErrorMessage(L"AddExcludeUserNameToProcessFilterRule failed.");
                    return false;
                }
            }
        }

	}

	return true;
}

BOOL
FilterControl::SendRegistryFilterRuleToFilter(RegistryFilterRule* registryFilter)
{
	if (!AddRegistryFilterRule((ULONG)registryFilter->ProcessNameFilterMask.length() * 2, &registryFilter->ProcessNameFilterMask[0], registryFilter->ProcessId,
			(ULONG)registryFilter->UserName.length() * 2, &registryFilter->UserName[0], (ULONG)registryFilter->RegistryKeyNameFilterMask.length() * 2, &registryFilter->RegistryKeyNameFilterMask[0],
			registryFilter->ControlFlag, registryFilter->RegCallbackClass,registryFilter->IsExcludeFilter, registryFilter->FilterRuleId))
	{
		PrintLastErrorMessage(L"AddRegistryFilterRule failed.");
		return false;
	}

    for (std::vector<std::wstring>::iterator excludeProcessName = registryFilter->ExcludeProcessNameList.begin();
        excludeProcessName != registryFilter->ExcludeProcessNameList.end(); ++excludeProcessName)
    {
        if (excludeProcessName->length() > 0)
        {
            if (!AddExcludeProcessNameToRegistryFilterRule(&registryFilter->ProcessNameFilterMask[0], &registryFilter->RegistryKeyNameFilterMask[0], &(*excludeProcessName)[0]))
            {
                PrintLastErrorMessage(L"AddExcludeProcessNameToProcessFilterRule failed.");
                return false;
            }
        }
    }


    for (std::vector<std::wstring>::iterator excludeUserName = registryFilter->ExcludeUserNameList.begin();
        excludeUserName != registryFilter->ExcludeUserNameList.end(); ++excludeUserName)
    {
        if (excludeUserName->length() > 0)
        {
            if (!AddExcludeUserNameToRegistryFilterRule(&registryFilter->ProcessNameFilterMask[0], &registryFilter->RegistryKeyNameFilterMask[0], &(*excludeUserName)[0]))
            {
                PrintLastErrorMessage(L"AddExcludeUserNameToProcessFilterRule failed.");
                return false;
            }
        }
    }

    for (std::vector<std::wstring>::iterator excludeKeyName = registryFilter->ExcludeKeyNameList.begin();
        excludeKeyName != registryFilter->ExcludeKeyNameList.end(); ++excludeKeyName)
    {
        if (excludeKeyName->length() > 0)
        {
            if (!AddExcludeKeyNameToRegistryFilterRule(&registryFilter->ProcessNameFilterMask[0], &registryFilter->RegistryKeyNameFilterMask[0], &(*excludeKeyName)[0]))
            {
                PrintLastErrorMessage(L"AddExcludeKeyNameToRegistryFilterRule failed.");
                return false;
            }
        }
    }


	return true;
}

BOOL
FilterControl::SendConfigSettingsToFilter()
{
    if (ProcessFilterRules.size() > 0 || RegistryFilterRules.size() > 0 || FileFilterRules.size() > 0)
    {
        if (!ResetConfigData())
        {
            PrintLastErrorMessage(L"ResetConfigData failed.");
            return false;
        }
    }

    if (!SetConnectionTimeout(connectionTimeout))
    {
        PrintLastErrorMessage(L"SetConnectionTimeout failed.");
        return false;
    }

    if (!SetFilterType(filterType))
    {
        PrintLastErrorMessage(L"SetFilterType failed.");
        return false;
    }

    if (globalBooleanConfig > 0 && !SetBooleanConfig(globalBooleanConfig))
    {
        PrintLastErrorMessage(L"SetBooleanConfig failed.");
        return false;
    }

    if (VolumeControlSettings > 0 && !SetVolumeControlFlag(VolumeControlSettings))
    {
        PrintLastErrorMessage(L"SetVolumeControlFlag failed.");
        return false;
    }
	
	for(std::vector<ULONG>::iterator it = ProtectedProcessIdList.begin(); it != ProtectedProcessIdList.end(); ++it) 
	{
		//
		//Send process Id to the protected list, to prevent the process from being killed ungratefully.
		//
		if (!AddProtectedProcessId(*it))
		{
			PrintLastErrorMessage(L"AddProtectedProcessId failed.");
			return false;
		}
    }

	//
    //Send process filter rules to filter driver.
    //
	for(int i = 0; i < ProcessFilterRules.size(); i++)
	{
		SendProcessFilterRuleToFilter(&ProcessFilterRules[i]);
	}

	//
    //Send registry filter rules to filter driver.
    //
	for(int i = 0; i < RegistryFilterRules.size(); i++) 
	{               
		SendRegistryFilterRuleToFilter(&RegistryFilterRules[i]);
	}

	//
    //Send file filter rules to filter driver.
    //
	for(int i = 0; i < FileFilterRules.size(); i++) 
	{
		SendFileFilterRuleToFilter(&FileFilterRules[i]);
	}

	return true;
}

VOID
SendNotification(PMESSAGE_SEND_DATA messageSend)
{
	if (messageSend->FilterCommand == FILTER_SEND_REG_CALLBACK_INFO || messageSend->FilterCommand == FILTER_SEND_DENIED_REGISTRY_ACCESS_EVENT)
    {
        SendRegistryFilterNotification(messageSend);
    }
    else if( ( messageSend->FilterCommand >= FILTER_SEND_PROCESS_CREATION_INFO &&  messageSend->FilterCommand <= FILTER_SEND_THREAD_HANDLE_INFO)
			|| (messageSend->FilterCommand == FILTER_SEND_DENIED_PROCESS_CREATION_EVENT)
            || (messageSend->FilterCommand == FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT)  )
    {
	   SendProcessFilterNotification(messageSend);
    }
	else
	{
		SendFileFilterNotification(messageSend);
	}
}

BOOL
FilterControl::MessageCallback(
   IN		PMESSAGE_SEND_DATA messageSend,
   IN OUT	PMESSAGE_REPLY_DATA messageReply )
{

  BOOL ret = true;

__try
{	  

	if (MESSAGE_SEND_VERIFICATION_NUMBER != messageSend->VerificationNumber)
    {
        //Received message corrupted.Please check if the MessageSendData structure is correct.
		printf("\n\nmessageSend data is corrupted in MessageCallback function.\n\n");
        return false;
    }

	if( NULL == messageReply)
	{
		SendNotification(messageSend);
	}
	else if(messageSend->FilterCommand == FILTER_SEND_REG_CALLBACK_INFO)
    {
        ret = RegistryFilterHandler(messageSend,messageReply);
    }
    else if(	messageSend->FilterCommand == FILTER_SEND_PROCESS_CREATION_INFO 
			||  messageSend->FilterCommand == FILTER_SEND_PRE_TERMINATE_PROCESS_INFO)
    {
		ret = ProcessFilterHandler(messageSend,messageReply);
    }
	else if(	messageSend->FilterCommand == FILTER_REQUEST_ENCRYPTION_IV_AND_KEY 
			||	messageSend->FilterCommand == FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA )
	{
		ret = EncryptionHandler(messageSend,messageReply);
	}
	else
	{
		ret = FileControlHandler(messageSend,messageReply);
	}
}
__except( EXCEPTION_EXECUTE_HANDLER  )
{         
	printf("\n\nMessageCallback handler got exception, messageId:%d messageType:0x%I64x FilterCommand:0x%0x fileName:%ws\n\n"
		,messageSend->MessageId, messageSend->MessageType, messageSend->FilterCommand, messageSend->FileName );

	ret = false;
}

return ret;

}

VOID
FilterControl::DisconnectCallback()
{
	printf("Filter connection was disconnected.\n");
	return;
}
