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
#include "FilterMessage.h"

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