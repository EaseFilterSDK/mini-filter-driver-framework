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
using System.Runtime.InteropServices;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
{
    public class FilterWorker
    {
        static MonitorEventHandler monitorEventHandler = new MonitorEventHandler();
        static ControlEventHandler controlEventHandler = new ControlEventHandler();
        static ProcessHandler processHandler = new ProcessHandler();
        static RegistryHandler registryHandler = new RegistryHandler();
        static FilterControl filterControl = new FilterControl();

        public static bool SendConfigSettingsToFilter(ref string lastError)
        {
            try
            {
                filterControl.ClearFilters();

                if (GlobalConfig.ProcessFilterRules.Count == 0)
                {
                    //there are no process filter rule in config file, here is the example to create a process filter rule.
                    //ProcessFilterRule processFilterRule = new ProcessFilterRule();
                    //processFilterRule.ProcessNameFilterMask = "*";
                    //processFilterRule.ControlFlag = (uint)(FilterAPI.ProcessControlFlag.PROCESS_CREATION_NOTIFICATION | FilterAPI.ProcessControlFlag.PROCESS_TERMINATION_NOTIFICATION);

                    //GlobalConfig.AddProcessFilterRule(processFilterRule);
                }

                foreach (ProcessFilterRule filterRule in GlobalConfig.ProcessFilterRules.Values)
                {
                    ProcessFilter processFilter = filterRule.ToProcessFilter();

                    processFilter.OnProcessCreation += processHandler.OnProcessCreation;
                    processFilter.NotifyProcessTerminated += processHandler.NotifyProcessTerminated;
                    processFilter.NotifyThreadCreation += processHandler.NotifyThreadCreation;
                    processFilter.NotifyProcessTerminated += processHandler.NotifyProcessTerminated;
                    processFilter.NotifyProcessHandleInfo += processHandler.NotifyProcessHandleInfo;
                    processFilter.NotifyThreadHandleInfo += processHandler.NotifyThreadHandleInfo;

                    filterControl.AddFilter(processFilter);
                }

                if (GlobalConfig.FilterRules.Count == 0)
                {
                    //there are no filter rule in config file, here is the example filter rule to create.

                    //FileFilterRule fileFilterRule = new FileFilterRule();
                    //fileFilterRule.IncludeFileFilterMask = "*";
                    //fileFilterRule.EnableMonitorEventBuffer = true;
                    //fileFilterRule.RegisterMonitorFileIOEvents = 0x28A8AAAAAAAAAAA;
                    //fileFilterRule.RegisterControlFileIOEvents = 0x0;
                    //fileFilterRule.AccessFlag = (uint)FilterAPI.ALLOW_MAX_RIGHT_ACCESS;

                    //GlobalConfig.AddFileFilterRule(fileFilterRule);
                }

                foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
                {
                    FileFilter fileFilter = filterRule.ToFileFilter();

                    //add the monitor event handler for the file filter.
                    fileFilter.OnFileOpen += monitorEventHandler.OnFileOpen;
                    fileFilter.OnNewFileCreate += monitorEventHandler.OnFileCreate;
                    fileFilter.OnDeleteFile += monitorEventHandler.OnDeleteFile;
                    fileFilter.OnFileRead += monitorEventHandler.OnFileRead;
                    fileFilter.OnFileWrite += monitorEventHandler.OnFileWrite;
                    fileFilter.OnQueryFileBasicInfo += monitorEventHandler.OnQueryFileBasicInfo;
                    fileFilter.OnQueryFileId += monitorEventHandler.OnQueryFileId;
                    fileFilter.OnQueryFileNetworkInfo += monitorEventHandler.OnQueryFileNetworkInfo;
                    fileFilter.OnQueryFileSize += monitorEventHandler.OnQueryFileSize;
                    fileFilter.OnQueryFileStandardInfo += monitorEventHandler.OnQueryFileStandardInfo;
                    fileFilter.OnQueryFileInfo += monitorEventHandler.OnQueryFileInfo;
                    fileFilter.OnSetFileBasicInfo += monitorEventHandler.OnSetFileBasicInfo;
                    fileFilter.OnSetFileNetworkInfo += monitorEventHandler.OnSetFileNetworkInfo;
                    fileFilter.OnSetFileSize += monitorEventHandler.OnSetFileSize;
                    fileFilter.OnSetFileStandardInfo += monitorEventHandler.OnSetFileStandardInfo;
                    fileFilter.OnMoveOrRenameFile += monitorEventHandler.OnMoveOrRenameFile;
                    fileFilter.OnSetFileInfo += monitorEventHandler.OnSetFileInfo;
                    fileFilter.OnQueryFileSecurity += monitorEventHandler.OnQueryFileSecurity;
                    fileFilter.OnQueryDirectoryFile += monitorEventHandler.OnQueryDirectoryFile;
                    fileFilter.OnFileHandleClose += monitorEventHandler.OnFileHandleClose;
                    fileFilter.OnFileClose += monitorEventHandler.OnFileClose;

                    fileFilter.NotifyFileWasChanged += monitorEventHandler.NotifyFileWasChanged;

                    //add the control pre-event handler for the control file filter.
                    fileFilter.OnPreCreateFile += controlEventHandler.OnPreCreateFile;
                    fileFilter.OnPreDeleteFile += controlEventHandler.OnPreDeleteFile;
                    fileFilter.OnPreFileRead += controlEventHandler.OnPreFileRead;
                    fileFilter.OnPreFileWrite += controlEventHandler.OnPreFileWrite;
                    fileFilter.OnPreQueryFileBasicInfo += controlEventHandler.OnPreQueryFileBasicInfo;
                    fileFilter.OnPreQueryFileId += controlEventHandler.OnPreQueryFileId;
                    fileFilter.OnPreQueryFileNetworkInfo += controlEventHandler.OnPreQueryFileNetworkInfo;
                    fileFilter.OnPreQueryFileSize += controlEventHandler.OnPreQueryFileSize;
                    fileFilter.OnPreQueryFileStandardInfo += controlEventHandler.OnPreQueryFileStandardInfo;
                    fileFilter.OnPreQueryFileInfo += controlEventHandler.OnPreQueryFileInfo;
                    fileFilter.OnPreSetFileBasicInfo += controlEventHandler.OnPreSetFileBasicInfo;
                    fileFilter.OnPreSetFileNetworkInfo += controlEventHandler.OnPreSetFileNetworkInfo;
                    fileFilter.OnPreSetFileSize += controlEventHandler.OnPreSetFileSize;
                    fileFilter.OnPreSetFileStandardInfo += controlEventHandler.OnPreSetFileStandardInfo;
                    fileFilter.OnPreMoveOrRenameFile += controlEventHandler.OnPreMoveOrRenameFile;
                    fileFilter.OnPreSetFileInfo += controlEventHandler.OnPreSetFileInfo;
                    fileFilter.OnPreQueryFileSecurity += controlEventHandler.OnPreQueryFileSecurity;
                    fileFilter.OnPreQueryDirectoryFile += controlEventHandler.OnPreQueryDirectoryFile;
                    fileFilter.OnPreFileHandleClose += controlEventHandler.OnPreFileHandleClose;
                    fileFilter.OnPreFileClose += controlEventHandler.OnPreFileClose;
                    //add the control post-event handler for the control file filter.
                    fileFilter.OnPostCreateFile += controlEventHandler.OnPostCreateFile;
                    fileFilter.OnPostDeleteFile += controlEventHandler.OnPostDeleteFile;
                    fileFilter.OnPostFileRead += controlEventHandler.OnPostFileRead;
                    fileFilter.OnPostFileWrite += controlEventHandler.OnPostFileWrite;
                    fileFilter.OnPostQueryFileBasicInfo += controlEventHandler.OnPostQueryFileBasicInfo;
                    fileFilter.OnPostQueryFileId += controlEventHandler.OnPostQueryFileId;
                    fileFilter.OnPostQueryFileNetworkInfo += controlEventHandler.OnPostQueryFileNetworkInfo;
                    fileFilter.OnPostQueryFileSize += controlEventHandler.OnPostQueryFileSize;
                    fileFilter.OnPostQueryFileStandardInfo += controlEventHandler.OnPostQueryFileStandardInfo;
                    fileFilter.OnPostQueryFileInfo += controlEventHandler.OnPostQueryFileInfo;
                    fileFilter.OnPostSetFileBasicInfo += controlEventHandler.OnPostSetFileBasicInfo;
                    fileFilter.OnPostSetFileNetworkInfo += controlEventHandler.OnPostSetFileNetworkInfo;
                    fileFilter.OnPostSetFileSize += controlEventHandler.OnPostSetFileSize;
                    fileFilter.OnPostSetFileStandardInfo += controlEventHandler.OnPostSetFileStandardInfo;
                    fileFilter.OnPostMoveOrRenameFile += controlEventHandler.OnPostMoveOrRenameFile;
                    fileFilter.OnPostSetFileInfo += controlEventHandler.OnPostSetFileInfo;
                    fileFilter.OnPostQueryFileSecurity += controlEventHandler.OnPostQueryFileSecurity;
                    fileFilter.OnPostQueryDirectoryFile += controlEventHandler.OnPostQueryDirectoryFile;
                    fileFilter.OnPostFileHandleClose += controlEventHandler.OnPostFileHandleClose;
                    fileFilter.OnPostFileClose += controlEventHandler.OnPostFileClose;

                    filterControl.AddFilter(fileFilter);
                }

                if (GlobalConfig.RegistryFilterRules.Count == 0)
                {
                    //there are no registry filter rule in config file, here is the example to create a registry filter rule.
                    //RegistryFilterRule registryFilterRule = new RegistryFilterRule();
                    //registryFilterRule.AccessFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;
                    //registryFilterRule.RegCallbackClass = 93092006832128; //by default only register post callback class
                    //registryFilterRule.ProcessNameFilterMask = "*";
                    //registryFilterRule.RegistryKeyNameFilterMask = "*Windows*"; //only display the key name includs 'Windows'.

                    //GlobalConfig.AddRegistryFilterRule(registryFilterRule);
                }

                foreach (RegistryFilterRule filterRule in GlobalConfig.RegistryFilterRules.Values)
                {
                    RegistryFilter registryFilter = filterRule.ToRegistryFilter();

                    registryFilter.OnPreDeleteKey += registryHandler.OnPreDeleteKey;
                    registryFilter.OnPreSetValueKey += registryHandler.OnPreSetValueKey;
                    registryFilter.OnPreDeleteValueKey += registryHandler.OnPreDeleteValueKey;
                    registryFilter.OnPreSetInformationKey += registryHandler.OnPreSetInformationKey;
                    registryFilter.OnPreRenameKey += registryHandler.OnPreRenameKey;
                    registryFilter.OnPreEnumerateKey += registryHandler.OnPreEnumerateKey;
                    registryFilter.OnPreEnumerateValueKey += registryHandler.OnPreEnumerateValueKey;
                    registryFilter.OnPreQueryKey += registryHandler.OnPreQueryKey;
                    registryFilter.OnPreQueryValueKey += registryHandler.OnPreQueryValueKey;
                    registryFilter.OnPreQueryMultipleValueKey += registryHandler.OnPreQueryMultipleValueKey;
                    registryFilter.OnPreCreateKey += registryHandler.OnPreCreateKey;
                    registryFilter.OnPreOpenKey += registryHandler.OnPreOpenKey;
                    registryFilter.OnPreKeyHandleClose += registryHandler.OnPreKeyHandleClose;
                    registryFilter.OnPreCreateKeyEx += registryHandler.OnPreCreateKeyEx;
                    registryFilter.OnPreOpenKeyEx += registryHandler.OnPreOpenKeyEx;
                    registryFilter.OnPreFlushKey += registryHandler.OnPreFlushKey;
                    registryFilter.OnPreLoadKey += registryHandler.OnPreLoadKey;
                    registryFilter.OnPreUnLoadKey += registryHandler.OnPreUnLoadKey;
                    registryFilter.OnPreQueryKeySecurity += registryHandler.OnPreQueryKeySecurity;
                    registryFilter.OnPreSetKeySecurity += registryHandler.OnPreSetKeySecurity;
                    registryFilter.OnPreRestoreKey += registryHandler.OnPreRestoreKey;
                    registryFilter.OnPreSaveKey += registryHandler.OnPreSaveKey;
                    registryFilter.OnPreReplaceKey += registryHandler.OnPreReplaceKey;
                    registryFilter.OnPreQueryKeyName += registryHandler.OnPreQueryKeyName;

                    registryFilter.NotifyDeleteKey += registryHandler.NotifyDeleteKey;
                    registryFilter.NotifySetValueKey += registryHandler.NotifySetValueKey;
                    registryFilter.NotifyDeleteValueKey += registryHandler.NotifyDeleteValueKey;
                    registryFilter.NotifySetInformationKey += registryHandler.NotifySetInformationKey;
                    registryFilter.NotifyRenameKey += registryHandler.NotifyRenameKey;
                    registryFilter.NotifyEnumerateKey += registryHandler.NotifyEnumerateKey;
                    registryFilter.NotifyEnumerateValueKey += registryHandler.NotifyEnumerateValueKey;
                    registryFilter.NotifyQueryKey += registryHandler.NotifyQueryKey;
                    registryFilter.NotifyQueryValueKey += registryHandler.NotifyQueryValueKey;
                    registryFilter.NotifyQueryMultipleValueKey += registryHandler.NotifyQueryMultipleValueKey;
                    registryFilter.NotifyCreateKey += registryHandler.NotifyCreateKey;
                    registryFilter.NotifyOpenKey += registryHandler.NotifyOpenKey;
                    registryFilter.NotifyKeyHandleClose += registryHandler.NotifyKeyHandleClose;
                    registryFilter.NotifyCreateKeyEx += registryHandler.NotifyCreateKeyEx;
                    registryFilter.NotifyOpenKeyEx += registryHandler.NotifyOpenKeyEx;
                    registryFilter.NotifyFlushKey += registryHandler.NotifyFlushKey;
                    registryFilter.NotifyLoadKey += registryHandler.NotifyLoadKey;
                    registryFilter.NotifyUnLoadKey += registryHandler.NotifyUnLoadKey;
                    registryFilter.NotifyQueryKeySecurity += registryHandler.NotifyQueryKeySecurity;
                    registryFilter.NotifySetKeySecurity += registryHandler.NotifySetKeySecurity;
                    registryFilter.NotifyRestoreKey += registryHandler.NotifyRestoreKey;
                    registryFilter.NotifySaveKey += registryHandler.NotifySaveKey;
                    registryFilter.NotifyReplaceKey += registryHandler.NotifyReplaceKey;
                    registryFilter.NotifyQueryKeyName += registryHandler.NotifyQueryKeyName;

                    filterControl.AddFilter(registryFilter);
                }

                //register the volume notification
                //filterControl.VolumeControlFlag = (uint)(FilterAPI.VolumeControlFlag.GET_ATTACHED_VOLUME_INFO | FilterAPI.VolumeControlFlag.VOLUME_ATTACHED_NOTIFICATION | FilterAPI.VolumeControlFlag.VOLUME_DETACHED_NOTIFICATION);
                filterControl.VolumeControlFlag = (FilterAPI.VolumeControlFlag)GlobalConfig.VolumeControlFlag;
                filterControl.NotifyFilterAttachToVolume -= controlEventHandler.NotifyFilterAttachToVolume;
                filterControl.NotifyFilterAttachToVolume += controlEventHandler.NotifyFilterAttachToVolume;
                filterControl.NotifyFilterDetachFromVolume -= controlEventHandler.NotifyFilterDetachFromVolume;
                filterControl.NotifyFilterDetachFromVolume += controlEventHandler.NotifyFilterDetachFromVolume;

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    Console.WriteLine("SendConfigSettingsToFilter failed." + lastError);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("SendConfigSettingsToFilter failed." + ex.Message);
                return false;
            }

        }

        public static bool StartService(out string lastError)
        {
            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string licenseKey = "A93B4-B3D73-2C533-C186E-C3EC0-5BDE3";
            bool ret = false;
                
            lastError = string.Empty;

            try
            {
                ret = filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
                if (!ret)
                {
                    return ret;
                }

                ret = SendConfigSettingsToFilter(ref lastError);

                EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, "Start filter service failed with error " + ex.Message);
                ret = false;
            }

            return ret;
        }

        public static bool StopService()
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();

            EventManager.WriteMessage(102, "StopFilter", EventLevel.Information, "Stopped filter service succeeded.");

            return true;
        }

    }

}
