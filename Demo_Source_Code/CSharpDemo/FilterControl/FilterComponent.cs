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

using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace EaseFilter.FilterControl
{

    public partial class FilterControl 
    {
        //The FilterControl is the component to communicate with the filter driver.
        //With this control, you can start or stop the filter service, setup the global configuration setting, and handle the filter callback request.
        //To start the file I/O, process and registry monitoring and protection, you need to add the filter rule to the FilterControl.
        //To monitor or control the file I/O, you can add the file filter rule which includes the monitor filter, control filter and encryption filter to the FilterControl.
        //To monitor or control the process operation, you can add the process filter rule to the FilterControl.
        //To monitor or control the registry access, you can add the registry filter rule to the FilterControl.

        delegate Boolean FilterDelegate(IntPtr sendData, IntPtr replyData);
        delegate void DisconnectDelegate();
        GCHandle gchFilter;
        GCHandle gchDisconnect;

        public static bool IsStarted = false;
        static bool isFilterStarted = false;

        uint globalFilterId = 1;

        int filterConnectionThreads = 5;
        int connectionTimeout = 10;
        FilterAPI.FilterType filterType = FilterAPI.FilterType.MONITOR_FILTER;

        Dictionary<uint, Filter> filterRuleList = new Dictionary<uint, Filter>();
        List<uint> protectedProcessIdList = new List<uint>();
        List<uint> includeProcessIdList = new List<uint>();
        List<uint> excludeProcessIdList = new List<uint>();

        public static string licenseKey = string.Empty;

        public FilterControl()
        {
        }


        /// <summary>
        /// The global boolean config setting
        /// </summary>
        public uint BooleanConfig { get; set; }
        /// <summary>
        /// The volume control flag, reference FilterAPI.VolumeControlFlag
        /// </summary>
        public FilterAPI.VolumeControlFlag VolumeControlFlag { get; set; }
        /// <summary>
        /// The global registered callback IO
        /// </summary>
        public uint RegisterCallbackIo { get; set; }
        /// <summary>
        /// The protected process Id list, prevent the processes from being terminiated.
        /// </summary>
        public List<uint> ProtectedProcessIdList
        {
            get { return protectedProcessIdList; }
            set { protectedProcessIdList = value; }
        }
        /// <summary>
        /// The global include process Id list, only IO from the process in the list will be managed by the filter driver.
        /// </summary>
        public List<uint> IncludeProcessIdList
        {
            get { return includeProcessIdList; }
            set { includeProcessIdList = value; }
        }
        /// <summary>
        /// The global exclude process Id list, skip all the IOs from the exclude process id list.
        /// </summary>
        public List<uint> ExcludeProcessIdList
        {
            get { return excludeProcessIdList; }
            set { excludeProcessIdList = value; }
        }


        /// <summary>
        /// Fires this event when the filter driver attached to a new volume or you get attached volume information.
        /// </summary>
        public event EventHandler<VolumeInfoEventArgs> NotifyFilterAttachToVolume;
        /// <summary>
        /// fires this event when a volume detached from a filter driver.
        /// </summary>
        public event EventHandler<VolumeInfoEventArgs> NotifyFilterDetachFromVolume;
        /// <summary>
        /// fires this event when the file IO was blocked by the access flag control setting when the SendDeniedFileIOEvent was true.
        /// </summary>
        public event EventHandler<DeniedFileIOEventArgs> NotifiyFileIOWasBlocked;
        /// <summary>
        /// fires this event when the read from USB was blocked.
        /// </summary>
        public event EventHandler<DeniedUSBReadEventArgs> NotifyUSBReadWasBlocked;
        /// <summary>
        /// fires this event when the write to USB was blocked.
        /// </summary>
        public event EventHandler<DeniedUSBWriteEventArgs> NotifyUSBWriteWasBlocked;
        /// <summary>
        /// fires this event when the process was terminated ungratefully was blocked.
        /// </summary>
        public event EventHandler<DeniedProcessTerminatedEventArgs> NotifiyProcessTerminatedWasBlocked;

        /// <summary>
        /// Start the filter driver service.
        /// </summary>
        /// <param name="filterConnectionThreads"></param>
        /// <param name="connectionTimeout"></param>
        /// <param name="licenseKey"></param>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public bool StartFilter(FilterAPI.FilterType _filterType, int _filterConnectionThreads, int _connectionTimeout, string _licenseKey, ref string lastError)
        {

            bool ret = true;
            FilterDelegate filterCallback = new FilterDelegate(FilterRequestHandler);
            DisconnectDelegate disconnectCallback = new DisconnectDelegate(DisconnectCallback);           

            try
            {
                filterType = _filterType;
                filterConnectionThreads = _filterConnectionThreads;
                connectionTimeout = _connectionTimeout;
                licenseKey = _licenseKey;

                //copy the right binary(EaseFlt.sys,FilterAPI.DLL) to the local executable binary folder.
                Utils.CopyOSPlatformDependentFiles(ref lastError);

                if (Utils.IsDriverChanged())
                {
                    //uninstall or install driver needs the Admin permission.
                    FilterAPI.UnInstallDriver();

                    //wait for 3 seconds for the uninstallation completed.
                    System.Threading.Thread.Sleep(3000);
                }

                if (!FilterAPI.IsDriverServiceRunning())
                {
                    ret = FilterAPI.InstallDriver();
                    if (!ret)
                    {
                        lastError = "Installed driver failed with error:" + FilterAPI.GetLastErrorMessage();
                        return false;
                    }
                }


                if (!isFilterStarted)
                {

                    if (!FilterAPI.SetRegistrationKey(licenseKey))
                    {
                        lastError = "Set license key failed with error:" + FilterAPI.GetLastErrorMessage();
                        return false;
                    }

                    gchFilter = GCHandle.Alloc(filterCallback);
                    IntPtr filterCallbackPtr = Marshal.GetFunctionPointerForDelegate(filterCallback);

                    gchDisconnect = GCHandle.Alloc(disconnectCallback);
                    IntPtr disconnectCallbackPtr = Marshal.GetFunctionPointerForDelegate(disconnectCallback);

                    isFilterStarted = FilterAPI.RegisterMessageCallback(filterConnectionThreads, filterCallbackPtr, disconnectCallbackPtr);
                    if (!isFilterStarted)
                    {
                        lastError = "Connect to the filter driver failed with error:" + FilterAPI.GetLastErrorMessage();
                        return false;
                    }

                    ret = true;

                    IsStarted = true;

                }
            }
            catch (Exception ex)
            {
                ret = false;
                lastError = "Start filter failed with error " + ex.Message;
            }
            finally
            {
                if (!ret)
                {
                    lastError = lastError + " Make sure you run this application as administrator.";
                }
              
            }

            return ret;
        }

        /// <summary>
        ///The filter driver service is started if it is true.
        /// </summary>
        public bool IsFilterStarted
        {
            get { return isFilterStarted; }
        }

        /// <summary>
        /// Stop the filter driver service.
        /// </summary>
        public void StopFilter()
        {
            if (isFilterStarted)
            {
                FilterAPI.Disconnect();
                gchFilter.Free();
                gchDisconnect.Free();
                isFilterStarted = false;
            }

            return;
        }

        /// <summary>
        /// Add the filter rule to the filter driver.
        /// </summary>
        /// <param name="filter"></param>
        public void AddFilter(Filter filter)
        {
            lock (filterRuleList)
            {
                if (filter.FilterId > 0 && filterRuleList.ContainsKey(filter.FilterId))
                {
                    filterRuleList.Remove(filter.FilterId);
                }

                filter.FilterId = globalFilterId++;
                filterRuleList.Add(filter.FilterId, filter);
            }
        }

        /// <summary>
        ///Remove the filter rule from the filter driver.
        /// </summary>
        /// <param name="filter"></param>
        public void RemoveFilter(Filter filter)
        {
            lock (filterRuleList)
            {
                if (filterRuleList.ContainsKey(filter.FilterId))
                {
                    filterRuleList.Remove(filter.FilterId);
                }
            }
        }

        /// <summary>
        /// Clear all registered filters
        /// </summary>
        public void ClearFilters()
        {
            lock (filterRuleList)
            {
                filterRuleList.Clear();
            }
        }

        /// <summary>
        /// Install the filter driver service.
        /// </summary>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public bool InstallDriver(ref string lastError)
        {
            if (!FilterAPI.InstallDriver())
            {
                lastError = "Installed driver failed with error:" + FilterAPI.GetLastErrorMessage();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Uninstall the filter driver service.
        /// </summary>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public bool UnInstallDriver(ref string lastError)
        {
            if (!FilterAPI.UnInstallDriver())
            {
                lastError = "UnInstallDriver failed with error:" + FilterAPI.GetLastErrorMessage();
                return false;
            }

            return true;
        }

        public bool ResetConfigData(ref string lastError)
        {
            if (!FilterAPI.ResetConfigData())
            {
                lastError = "ResetConfigData failed:" + FilterAPI.GetLastErrorMessage();
                return false;
            }

            return true;
        }

        public bool SendConfigSettingsToFilter(ref string lastError)
        {
            try
            {
                if (!isFilterStarted)
                {
                    lastError = "the filter driver is not started.";
                    return true;
                }

                if (!FilterAPI.ResetConfigData())
                {
                    lastError = "ResetConfigData failed:" + FilterAPI.GetLastErrorMessage();
                    return false;
                }

                if (!FilterAPI.SetConnectionTimeout((uint)connectionTimeout))
                {
                    lastError = "SetConnectionTimeout failed:" + FilterAPI.GetLastErrorMessage();
                    return false;
                }

                if (!FilterAPI.SetFilterType((uint)filterType))
                {
                    lastError = "SetFilterType " + filterType.ToString() + " failed:" + FilterAPI.GetLastErrorMessage();
                    return false;
                }

                if (BooleanConfig > 0 && !FilterAPI.SetBooleanConfig(BooleanConfig))
                {
                    lastError = "SetBooleanConfig " + BooleanConfig + " failed:" + FilterAPI.GetLastErrorMessage();
                    return false;
                }

                if (VolumeControlFlag > 0 && !FilterAPI.SetVolumeControlFlag((uint)VolumeControlFlag))
                {
                    lastError = "SetVolumeControlFlag " + VolumeControlFlag + " failed:" + FilterAPI.GetLastErrorMessage();
                    return false;
                }


                foreach (uint includedPid in IncludeProcessIdList)
                {
                    if (!FilterAPI.AddIncludedProcessId(includedPid))
                    {
                        lastError = "AddIncludedProcessId " + includedPid + " failed:" + FilterAPI.GetLastErrorMessage();
                        return false;
                    }
                }

                foreach (uint excludedPid in ExcludeProcessIdList)
                {
                    if (!FilterAPI.AddExcludedProcessId(excludedPid))
                    {
                        lastError = "AddExcludedProcessId " + excludedPid + " failed:" + FilterAPI.GetLastErrorMessage();
                        return false;
                    }
                }

                if (RegisterCallbackIo > 0 && !FilterAPI.RegisterIoRequest(RegisterCallbackIo))
                {
                    lastError = "RegisterIoRequest " + RegisterCallbackIo + " failed:" + FilterAPI.GetLastErrorMessage();
                    return false;
                }

                foreach (uint protectPid in ProtectedProcessIdList)
                {
                    if (!FilterAPI.AddProtectedProcessId(protectPid))
                    {
                        lastError = "AddProtectedProcessId " + protectPid + " failed:" + FilterAPI.GetLastErrorMessage();
                        return false;
                    }
                }


                foreach (Filter filter in filterRuleList.Values)
                {
                    if (filter.FilterType == FilterAPI.FilterType.PROCESS_FILTER)
                    {
                        //
                        //Process filter driver config settings here.
                        //
                        ProcessFilter processFilter = (ProcessFilter)filter;

                        if (processFilter.ProcessNameFilterMask.Length > 0)
                        {
                            //set the process control flag for the process filter mask. 
                            if (!FilterAPI.AddProcessFilterRule((uint)processFilter.ProcessNameFilterMask.Length * 2, processFilter.ProcessNameFilterMask, processFilter.ControlFlag, processFilter.FilterId))
                            {
                                lastError = "AddProcessFilterRule " + processFilter.ProcessNameFilterMask + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }

                        foreach (string excludeProcessName in processFilter.ExcludeProcessNameList)
                        {
                            if (excludeProcessName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeProcessNameToProcessFilterRule(processFilter.ProcessNameFilterMask, excludeProcessName.Trim()))
                                {
                                    lastError = "AddExcludeProcessNameToProcessFilterRule " + excludeProcessName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                        foreach (string excludeUserName in processFilter.ExcludeUserNameList)
                        {
                            if (excludeUserName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeUserNameToProcessFilterRule(processFilter.ProcessNameFilterMask, excludeUserName.Trim()))
                                {
                                    lastError = "AddExcludeUserNameToProcessFilterRule " + excludeUserName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }


                        foreach (KeyValuePair<string, uint> entry in processFilter.FileAccessRights)
                        {
                            string fileNameMask = entry.Key;
                            uint accessFlag = entry.Value;

                            if (processFilter.ProcessId > 0)
                            {
                                //filter by process Id
                                if (!FilterAPI.AddFileControlToProcessById(processFilter.ProcessId, (uint)fileNameMask.Length * 2, fileNameMask.Trim(), accessFlag))
                                {
                                    lastError = "AddFileControlToProcessById fileNameMask:" + fileNameMask + ",accessFlags:" + accessFlag + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                            else if (processFilter.ProcessNameFilterMask.Length > 0)
                            {
                                //add file access control with process name
                                if (!FilterAPI.AddFileControlToProcessByName((uint)processFilter.ProcessNameFilterMask.Length * 2,
                                    processFilter.ProcessNameFilterMask, (uint)fileNameMask.Length * 2, fileNameMask.Trim(), accessFlag))
                                {
                                    lastError = "AddFileControlToProcessByName " + processFilter.ProcessNameFilterMask + ",fileNameMask:" + fileNameMask + ",accessFlags:" + accessFlag + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }

                                if (processFilter.MonitorFileIOEventFilter > 0 || processFilter.ControlFileIOEventFilter > 0)
                                {
                                    ulong registerMonitorIOToFilter = processFilter.MonitorFileIOEventFilter;
                                    ulong registerControlIOToFilter = processFilter.ControlFileIOEventFilter;

                                    if (!FilterAPI.AddFileCallbackIOToProcessByName((uint)processFilter.ProcessNameFilterMask.Length * 2,
                                        processFilter.ProcessNameFilterMask, (uint)fileNameMask.Length * 2, fileNameMask.Trim(), registerMonitorIOToFilter, registerControlIOToFilter, processFilter.FilterId))
                                    {
                                        lastError = "AddFileControlToProcessByName failed:" + FilterAPI.GetLastErrorMessage();
                                        return false;
                                    }
                                }
                            }
                        }
                    }
                    else if (filter.FilterType == FilterAPI.FilterType.REGISTRY_FILTER)
                    {
                        //
                        //Registry filter driver config setting
                        //
                        RegistryFilter registryFilter = (RegistryFilter)filter;

                        if (!FilterAPI.AddRegistryFilterRule((uint)registryFilter.ProcessNameFilterMask.Length * 2, registryFilter.ProcessNameFilterMask, registryFilter.ProcessId,
                            (uint)registryFilter.UserName.Length * 2, registryFilter.UserName, (uint)registryFilter.RegistryKeyNameFilterMask.Length * 2, registryFilter.RegistryKeyNameFilterMask,
                                registryFilter.ControlFlag, registryFilter.RegCallbackClass, registryFilter.IsExcludeFilter, registryFilter.FilterId))
                        {
                            lastError = "AddRegistryFilterRule failed:" + FilterAPI.GetLastErrorMessage();
                            return false;
                        }

                        foreach (string excludeProcessName in registryFilter.ExcludeProcessNameList)
                        {
                            if (excludeProcessName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeProcessNameToRegistryFilterRule(
                                    registryFilter.ProcessNameFilterMask,registryFilter.RegistryKeyNameFilterMask, excludeProcessName.Trim()))
                                {
                                    lastError = "AddExcludeProcessNameToRegistryFilterRule " + excludeProcessName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                        foreach (string excludeUserName in registryFilter.ExcludeUserNameList)
                        {
                            if (excludeUserName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeUserNameToRegistryFilterRule(
                                    registryFilter.ProcessNameFilterMask, registryFilter.RegistryKeyNameFilterMask, excludeUserName.Trim()))
                                {
                                    lastError = "AddExcludeUserNameToRegistryFilterRule " + excludeUserName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                        foreach (string excludeKeyName in registryFilter.ExcludeKeyNameList)
                        {
                            if (excludeKeyName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeKeyNameToRegistryFilterRule(
                                    registryFilter.ProcessNameFilterMask, registryFilter.RegistryKeyNameFilterMask, excludeKeyName.Trim()))
                                {
                                    lastError = "AddExcludeKeyNameToRegistryFilterRule " + excludeKeyName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                    }
                    else //this is file filter.
                    {

                        FileFilter fileFilter = (FileFilter)filter;

                        //add filter rule to filter driver here, the filter rule is unique with the include file filter mask.
                        //you can't have the mutiple filter rules with the same include file filter mask,if there are the same 
                        //one exist, the new one with accessFlag will overwrite the old accessFlag.
                        //for control filter, if isResident is true, the access control will be enabled in boot time.
                        if (!FilterAPI.AddFileFilterRule((uint)fileFilter.AccessFlags, fileFilter.IncludeFileFilterMask, fileFilter.IsResident, fileFilter.FilterId))
                        {
                            lastError = "Send filter rule failed:" + FilterAPI.GetLastErrorMessage();
                            return false;
                        }

                        if (fileFilter.FileChangeEventFilter > 0 && !FilterAPI.RegisterFileChangedEventsToFilterRule(fileFilter.IncludeFileFilterMask, (uint)fileFilter.FileChangeEventFilter))
                        {
                            lastError = "Register file event type:" + fileFilter.FileChangeEventFilter + " failed:" + FilterAPI.GetLastErrorMessage();
                            return false;
                        }

                        if (fileFilter.MonitorFileIOEventFilter > 0 && !FilterAPI.RegisterMonitorIOToFilterRule(fileFilter.IncludeFileFilterMask, fileFilter.MonitorFileIOEventFilter))
                        {
                            lastError = "Register monitor IO:" + fileFilter.MonitorFileIOEventFilter.ToString("X") + " failed:" + FilterAPI.GetLastErrorMessage();
                            return false;
                        }

                        if (fileFilter.ControlFileIOEventFilter > 0 && !FilterAPI.RegisterControlIOToFilterRule(fileFilter.IncludeFileFilterMask, fileFilter.ControlFileIOEventFilter))
                        {
                            lastError = "Register control IO:" + fileFilter.ControlFileIOEventFilter.ToString("X") + " failed:" + FilterAPI.GetLastErrorMessage();
                            return false;
                        }

                        if (fileFilter.FilterDesiredAccess > 0 || fileFilter.FilterDisposition > 0 || fileFilter.FilterCreateOptions > 0)
                        {
                            if (!FilterAPI.AddCreateFilterToFilterRule(fileFilter.IncludeFileFilterMask, fileFilter.FilterDesiredAccess, fileFilter.FilterDisposition, fileFilter.FilterCreateOptions))
                            {
                                lastError = "AddCreateFilterToFilterRule failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }

                        //every filter rule can have multiple exclude file filter masks, you can exclude the files 
                        //which matches the exclude filter mask.
                        foreach (string excludeFilterMask in fileFilter.ExcludeFileFilterMaskList)
                        {
                            if (excludeFilterMask.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeFileMaskToFilterRule(fileFilter.IncludeFileFilterMask, excludeFilterMask.Trim()))
                                {
                                    lastError = "AddExcludeFileMaskToFilterRule " + excludeFilterMask + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }


                        if (fileFilter.EnableEncryption)
                        {
                            if (null != fileFilter.EncryptionKey && fileFilter.EncryptionKey.Length > 0)
                            {
                                if (null != fileFilter.EncryptionIV && fileFilter.EncryptionIV.Length > 0)
                                {
                                    if (!FilterAPI.AddEncryptionKeyAndIVToFilterRule(fileFilter.IncludeFileFilterMask, (uint)fileFilter.EncryptionKey.Length, fileFilter.EncryptionKey, (uint)fileFilter.EncryptionIV.Length, fileFilter.EncryptionIV))
                                    {
                                        lastError = "AddEncryptionKeyAndIVToFilterRule " + fileFilter.IncludeFileFilterMask + " failed:" + FilterAPI.GetLastErrorMessage();
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (!FilterAPI.AddEncryptionKeyToFilterRule(fileFilter.IncludeFileFilterMask, (uint)fileFilter.EncryptionKey.Length, fileFilter.EncryptionKey))
                                    {
                                        lastError = "AddEncryptionKeyToFilterRule " + fileFilter.IncludeFileFilterMask + " failed:" + FilterAPI.GetLastErrorMessage();
                                        return false;
                                    }
                                }
                            }

                            if (fileFilter.EncryptWriteBufferSize > 0)
                            {
                                if (!FilterAPI.SetEncryptWriteBufferSize(fileFilter.EncryptWriteBufferSize))
                                {
                                    lastError = "SetEncryptWriteBufferSize " + fileFilter.EncryptWriteBufferSize + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }

                        }

                        if (fileFilter.BooleanConfig > 0 && !FilterAPI.AddBooleanConfigToFilterRule(fileFilter.IncludeFileFilterMask, fileFilter.BooleanConfig))
                        {
                            lastError = "AddBooleanConfigToFilterRule " + fileFilter.IncludeFileFilterMask + " failed:" + FilterAPI.GetLastErrorMessage();
                            return false;
                        }

                        if (fileFilter.EnableMonitorEventBuffer && (fileFilter.MaxMonitorEventBufferSize > 0)
                            && !FilterAPI.SetMaxMonitorEventBuffersize((uint)fileFilter.MaxMonitorEventBufferSize))
                        {
                            lastError = "SetMaxMonitorEventBuffersize " + fileFilter.MaxMonitorEventBufferSize + " failed:" + FilterAPI.GetLastErrorMessage();
                            return false;
                        }


                        foreach (string includeProcessName in fileFilter.IncludeProcessNameList)
                        {
                            if (includeProcessName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddIncludeProcessNameToFilterRule(fileFilter.IncludeFileFilterMask, includeProcessName.Trim()))
                                {
                                    lastError = "AddIncludeProcessNameToFilterRule " + includeProcessName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                        foreach (string excludeProcessName in fileFilter.ExcludeProcessNameList)
                        {
                            if (excludeProcessName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeProcessNameToFilterRule(fileFilter.IncludeFileFilterMask, excludeProcessName.Trim()))
                                {
                                    lastError = "AddExcludeProcessNameToFilterRule " + excludeProcessName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                        //set include process list for this filter rule.
                        foreach (uint pid in fileFilter.IncludeProcessIdList)
                        {
                            if (!FilterAPI.AddIncludeProcessIdToFilterRule(fileFilter.IncludeFileFilterMask, pid))
                            {
                                lastError = "AddIncludeProcessIdToFilterRule " + fileFilter.IncludeFileFilterMask + " PID:" + pid + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }

                        //set exclude process list for this filter rule.
                        foreach (uint pid in fileFilter.ExcludeProcessIdList)
                        {
                            if (!FilterAPI.AddExcludeProcessIdToFilterRule(fileFilter.IncludeFileFilterMask, pid))
                            {
                                lastError = "AddExcludeProcessIdToFilterRule " + fileFilter.IncludeFileFilterMask + " PID:" + pid + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }

                        }

                        foreach (string includeUserName in fileFilter.IncludeUserNameList)
                        {
                            if (includeUserName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddIncludeUserNameToFilterRule(fileFilter.IncludeFileFilterMask, includeUserName.Trim()))
                                {
                                    lastError = "AddIncludeUserNameToFilterRule " + includeUserName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                        foreach (string excludeUserName in fileFilter.ExcludeUserNameList)
                        {
                            if (excludeUserName.Trim().Length > 0)
                            {
                                if (!FilterAPI.AddExcludeUserNameToFilterRule(fileFilter.IncludeFileFilterMask, excludeUserName.Trim()))
                                {
                                    lastError = "AddExcludeUserNameToFilterRule " + excludeUserName + " failed:" + FilterAPI.GetLastErrorMessage();
                                    return false;
                                }
                            }
                        }

                        foreach (KeyValuePair<string, uint> entry in fileFilter.ProcessNameAccessRightList)
                        {
                            string processName = entry.Key;
                            uint accessFlags = entry.Value;

                            if (!FilterAPI.AddProcessRightsToFilterRule(fileFilter.IncludeFileFilterMask, processName.Trim(), accessFlags))
                            {
                                lastError = "AddProcessRightsToFilterRule " + fileFilter.IncludeFileFilterMask + ",processName:" + processName + ",accessFlags:" + accessFlags + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }

                        foreach (KeyValuePair<byte[], uint> entry in fileFilter.Sha256ProcessAccessRightList)
                        {
                            byte[] processSha256 = entry.Key;
                            uint accessFlags = entry.Value;

                            if (!FilterAPI.AddSha256ProcessAccessRightsToFilterRule(fileFilter.IncludeFileFilterMask, processSha256, (uint)processSha256.Length, accessFlags))
                            {
                                lastError = "AddProcessSha256RightsToFilterRule " + fileFilter.IncludeFileFilterMask + ",accessFlags:" + accessFlags + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }

                        foreach (KeyValuePair<string, uint> entry in fileFilter.SignedProcessAccessRightList)
                        {
                            string certificateName = entry.Key;
                            uint accessFlags = entry.Value;

                            if (!FilterAPI.AddSignedProcessAccessRightsToFilterRule(fileFilter.IncludeFileFilterMask, certificateName.Trim(),(uint)certificateName.Length*2, accessFlags))
                            {
                                lastError = "AddSignedProcessAccessRightsToFilterRule " + fileFilter.IncludeFileFilterMask + ",certificateName:" + certificateName + ",accessFlags:" + accessFlags + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }


                        foreach (KeyValuePair<uint, uint> entry in fileFilter.ProcessIdAccessRightList)
                        {
                            uint processId = entry.Key;
                            uint accessFlags = entry.Value;

                            if (!FilterAPI.AddProcessIdRightsToFilterRule(fileFilter.IncludeFileFilterMask, processId, accessFlags))
                            {
                                lastError = "AddProcessIdRightsToFilterRule " + fileFilter.IncludeFileFilterMask + ",processId:" + processId + ",accessFlags:" + accessFlags + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }

                        foreach (KeyValuePair<string, uint> entry in fileFilter.userAccessRightList)
                        {
                            string userName = entry.Key;
                            uint accessFlags = entry.Value;

                            if (!FilterAPI.AddUserRightsToFilterRule(fileFilter.IncludeFileFilterMask, userName.Trim(), accessFlags))
                            {
                                lastError = "AddUserRightsToFilterRule " + fileFilter.IncludeFileFilterMask + ",userName:" + userName + ",accessFlags:" + accessFlags + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }
                        }

                        //Hide the files which match the hidden file filter masks when the user browse the managed directory.
                        if (fileFilter.EnableHiddenFile)
                        {
                            foreach (string hiddenFilterMask in fileFilter.HiddenFileFilterMaskList)
                            {
                                if (hiddenFilterMask.Trim().Length > 0)
                                {
                                    if (!FilterAPI.AddHiddenFileMaskToFilterRule(fileFilter.IncludeFileFilterMask, hiddenFilterMask.Trim()))
                                    {
                                        lastError = "AddHiddenFileMaskToFilterRule " + fileFilter.IncludeFileFilterMask + " hiddenFilterMask:" + hiddenFilterMask + " failed:" + FilterAPI.GetLastErrorMessage();
                                        return false;
                                    }
                                }
                            }
                        }

                        //reparse the file open to another file with the filter mask.
                        //For example:
                        //FilterMask = c:\test\*txt
                        //ReparseFilterMask = d:\reparse\*doc
                        //If you open file c:\test\MyTest.txt, it will reparse to the file d:\reparse\MyTest.doc.
                        if (fileFilter.EnableReparseFile && fileFilter.ReparseFileFilterMask.Trim().Length > 0)
                        {
                            string reparseFileFilterMask = fileFilter.ReparseFileFilterMask;
                            if (!FilterAPI.AddReparseFileMaskToFilterRule(fileFilter.IncludeFileFilterMask, reparseFileFilterMask.Trim()))
                            {
                                lastError = "AddReparseFileMaskToFilterRule " + fileFilter.IncludeFileFilterMask + " reparseFileFilterMask:" + reparseFileFilterMask + " failed:" + FilterAPI.GetLastErrorMessage();
                                return false;
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                lastError = "Send config settings to filter failed with error " + ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Handle the requests from the filter driver, process the notification if no return needed, 
        /// or reply the message back to the filter which the filter driver is wating for.
        /// </summary>
        /// <param name="sendDataPtr"></param>
        /// <param name="replyDataPtr"></param>
        /// <returns></returns>
        Boolean FilterRequestHandler(IntPtr sendDataPtr, IntPtr replyDataPtr)
        {
            Boolean ret = true;

            try
            {

                FilterAPI.MessageSendData messageSend = (FilterAPI.MessageSendData)Marshal.PtrToStructure(sendDataPtr, typeof(FilterAPI.MessageSendData));

                if (FilterAPI.MESSAGE_SEND_VERIFICATION_NUMBER != messageSend.VerificationNumber)
                {
                    //Received message corrupted.Please check if the MessageSendData structure is correct.
                    return false;
                }

                if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_ATTACHED_VOLUME_INFO)
                {
                    if (null != NotifyFilterAttachToVolume)
                    {
                        VolumeInfoEventArgs volumeArgs = new VolumeInfoEventArgs(messageSend);
                        volumeArgs.EventName = "VolumeAttached";
                        NotifyFilterAttachToVolume(null, volumeArgs);
                    }
                }
                else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DETACHED_VOLUME_INFO)
                {
                    if (null != NotifyFilterDetachFromVolume)
                    {
                        VolumeInfoEventArgs volumeArgs = new VolumeInfoEventArgs(messageSend);
                        volumeArgs.EventName = "VolumeDetached";
                        NotifyFilterDetachFromVolume(null, volumeArgs);
                    }
                }
                else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_VOLUME_DISMOUNT_EVENT)
                {
                    if (null != NotifyFilterDetachFromVolume)
                    {
                        VolumeInfoEventArgs volumeArgs = new VolumeInfoEventArgs(messageSend);
                        volumeArgs.EventName = "VolumeDismountWasBlocked";
                        NotifyFilterDetachFromVolume(null, volumeArgs);
                    }
                }
                else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_FILE_IO_EVENT)
                {
                    if (null != NotifiyFileIOWasBlocked)
                    {
                        DeniedFileIOEventArgs deniedIOEventArgs = new DeniedFileIOEventArgs(messageSend);
                        NotifiyFileIOWasBlocked(null, deniedIOEventArgs);
                    }
                }
                else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_USB_READ_EVENT)
                {
                    if (null != NotifyUSBReadWasBlocked)
                    {
                        DeniedUSBReadEventArgs deniedIOEventArgs = new DeniedUSBReadEventArgs(messageSend);
                        NotifyUSBReadWasBlocked(null, deniedIOEventArgs);
                    }
                }
                else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_USB_WRITE_EVENT)
                {
                    if (null != NotifyUSBWriteWasBlocked)
                    {
                        DeniedUSBWriteEventArgs deniedIOEventArgs = new DeniedUSBWriteEventArgs(messageSend);
                        NotifyUSBWriteWasBlocked(null, deniedIOEventArgs);
                    }
                }
                else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT)
                {
                    if (null != NotifiyProcessTerminatedWasBlocked)
                    {
                        DeniedProcessTerminatedEventArgs deniedProcessEventArgs = new DeniedProcessTerminatedEventArgs(messageSend);
                        NotifiyProcessTerminatedWasBlocked(null, deniedProcessEventArgs);
                    }
                }
                else if (filterRuleList.ContainsKey(messageSend.FilterRuleId))
                {
                    Filter registerFilter = filterRuleList[messageSend.FilterRuleId];

                    if (replyDataPtr.ToInt64() == 0)
                    {
                        registerFilter.SendNotification(messageSend);
                    }
                    else
                    {
                        ret = registerFilter.ReplyMessage(messageSend, replyDataPtr);
                    }

                }
                else
                {
                    Console.WriteLine("There are no handler for the filter command." + messageSend.FilterCommand.ToString("X") + ",filterRuleId:" + messageSend.FilterRuleId
                        + ",messageType:" + messageSend.MessageType.ToString("X") + ",fileName:" + messageSend.FileName);
                }

                return ret;
            }
            catch (Exception ex)
            {
                //EventManager.WriteMessage(134, "FilterCallback", EventLevel.Error, "filter callback exception." + ex.Message);
                Utils.ToDebugger("filter callback exception." + ex.Message);
                return false;
            }

        }

        void DisconnectCallback()
        {
            return;
        }
    }
}
