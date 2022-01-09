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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Text.RegularExpressions;
using System.Threading;
using System.Reflection;
using System.Diagnostics;

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{

    public class GlobalConfig
    {
        //Purchase a license key with the link: http://www.easefilter.com/Order.htm
        //Email us to request a trial key: info@easefilter.com //free email is not accepted.
        public static string licenseKey = "******************************************";

        static Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
        public static string AssemblyPath = Path.GetDirectoryName(assembly.Location);
        public static string AssemblyName = assembly.Location;

        //the message output level. It will output the messages which less than this level.
        static EventLevel eventLevel = EventLevel.Information;
        static bool[] selectedDisplayEvents = new bool[] { false, true, true, true, false, false };
        static EventOutputType eventOutputType = EventOutputType.File;
        //The log file name if outputType is ToFile.
        static string eventLogFileName = "EventLog.txt";
        static int maxEventLogFileSize = 4 * 1024 * 1024; //4MB
        static string eventSource = "EaseFilter";
        static string eventLogName = "EaseFilter";

        static uint volumeControlFlag = (uint)(FilterAPI.VolumeControlFlag.VOLUME_ATTACHED_NOTIFICATION | FilterAPI.VolumeControlFlag.VOLUME_DETACHED_NOTIFICATION);

        static int filterConnectionThreads = 5;
        static int connectionTimeOut = 10; //seconds
        static private Dictionary<string, FileFilterRule> filterRules = new Dictionary<string, FileFilterRule>();
        static List<uint> includePidList = new List<uint>();
        static List<uint> excludePidList = new List<uint>();
        static List<uint> protectPidList = new List<uint>();
        static string includedUsers = string.Empty;
        static string excludedUsers = string.Empty;

        static uint requestIORegistration = 0;
        static uint displayEvents = 0;

        static string accountName = "";
        static string masterPassword = "testpassword";

        static int maximumFilterMessages = 1000;
        static string filterMessageLogName = "filterMessage.log";
        static long filterMessageLogFileSize = 10 * 1024 * 1024;
        static bool enableLogTransaction = false;
        static bool outputMessageToConsole = true;
        static bool enableNotification = false;

        static string protectFolder = "c:\\EaseFilter\\protectFolder";
        static string protectFolderWhiteList = "*";
        static string protectFolderBlackList = "explorer.exe";
        static string shareFolder = "c:\\EaseFilter\\shareFolder";
        static string shareFolderWhiteList = "notepad.exe;wordpad.exe";
        static string shareFolderBlackList = "explorer.exe";
        static string drInfoFolder = "c:\\EaseFilter\\DRInfoFolder";
        static string dropFolder = "c:\\EaseFilter\\dropFolder";
        static bool storeSharedFileMetaDataInServer = false;
        static string shareFileExt = ".psf";

        static string configFileName = ConfigSetting.GetFilePath();

        //the filter driver will use the default IV to encrypt the new file if it is true.
        static bool enableDefaultIVKey = false;

        static uint currentPid = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;

        //dont display the directory IO request if it is true.
        static bool disableDirIO = false;

        static uint booleanConfig = (uint)FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT;

        public static bool isRunning = true;
        public static ManualResetEvent stopEvent = new ManualResetEvent(false);

        public static FilterAPI.FilterType filterType = FilterAPI.FilterType.MONITOR_FILTER;

        public static Stopwatch stopWatch = new Stopwatch();

        //the process filter rule collection
        static private Dictionary<string, ProcessFilterRule> processFilterRules = new Dictionary<string, ProcessFilterRule>();

        //the registry filter rule collection
        static private Dictionary<string, RegistryFilterRule> registryFilterRules = new Dictionary<string, RegistryFilterRule>();

        static GlobalConfig()
        {
            string lastError = string.Empty;
            Utils.CopyOSPlatformDependentFiles(ref lastError);

           stopWatch.Start();
           Load();           
           
        }

        public static void Load()
        {
            filterRules.Clear();
            processFilterRules.Clear();
            registryFilterRules.Clear();

            try
            {

                filterRules = ConfigSetting.GetFilterRules();
                processFilterRules = ConfigSetting.GetProcessFilterRules();
                registryFilterRules = ConfigSetting.GetRegistryFilterRules();

                booleanConfig = ConfigSetting.Get("booleanConfig", booleanConfig);
                filterConnectionThreads = ConfigSetting.Get("filterConnectionThreads", filterConnectionThreads);
                requestIORegistration = ConfigSetting.Get("requestIORegistration", requestIORegistration);
                displayEvents = ConfigSetting.Get("displayEvents", displayEvents);
                filterMessageLogName = ConfigSetting.Get("filterMessageLogName", filterMessageLogName);
                filterMessageLogFileSize = ConfigSetting.Get("filterMessageLogFileSize", filterMessageLogFileSize);
                maximumFilterMessages = ConfigSetting.Get("maximumFilterMessages", maximumFilterMessages);
                enableLogTransaction = ConfigSetting.Get("enableLogTransaction", enableLogTransaction);
                enableDefaultIVKey = ConfigSetting.Get("enableDefaultIVKey", enableDefaultIVKey);
                accountName = ConfigSetting.Get("accountName", accountName);

                outputMessageToConsole = ConfigSetting.Get("outputMessageToConsole", outputMessageToConsole);
                enableNotification = ConfigSetting.Get("enableNotification", enableNotification);
                eventLevel = (EventLevel)ConfigSetting.Get("eventLevel", (uint)eventLevel);

                string savedMasterPassword = ConfigSetting.Get("masterPassword", "");
                if (savedMasterPassword.Length > 0)
                {
                    masterPassword = Utils.AESEncryptDecryptStr(savedMasterPassword, Utils.EncryptType.Decryption);
                }

                includedUsers = ConfigSetting.Get("includedUsers", includedUsers);
                excludedUsers = ConfigSetting.Get("excludedUsers", excludedUsers);

                //setting for secure file sharing
                protectFolder = ConfigSetting.Get("protectFolder", protectFolder);
                protectFolderWhiteList = ConfigSetting.Get("protectFolderWhiteList", protectFolderWhiteList);
                protectFolderBlackList = ConfigSetting.Get("protectFolderBlackList", protectFolderBlackList);
                drInfoFolder = ConfigSetting.Get("drInfoFolder", drInfoFolder);
                shareFolder = ConfigSetting.Get("shareFolder", shareFolder);
                dropFolder = ConfigSetting.Get("dropFolder", dropFolder);
                shareFolderWhiteList = ConfigSetting.Get("shareFolderWhiteList", shareFolderWhiteList);
                shareFolderBlackList = ConfigSetting.Get("shareFolderBlackList", shareFolderBlackList);
                storeSharedFileMetaDataInServer = ConfigSetting.Get("storeSharedFileMetaDataInServer", storeSharedFileMetaDataInServer);
                shareFileExt = ConfigSetting.Get("shareFileExt", shareFileExt);

                volumeControlFlag = ConfigSetting.Get("volumeControlFlag", volumeControlFlag);

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(176, "LoadConfigSetting", EventLevel.Error, "Load config file " + configFileName + " failed with error:" + ex.Message);
            }
        }

        public static void Stop()
        {
            isRunning = false;
            stopEvent.Set();
            EventManager.Stop();           
        }
     
        public static bool SaveConfigSetting()
        {
            bool ret = true;

            try
            {
                ConfigSetting.Save();             
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(235, "SaveConfigSetting", EventLevel.Error, "Save config file " + configFileName + " failed with error:" + ex.Message);
                ret = false;
            }

            return ret;
        }


        static public string  ConfigFilePath
        {
            get { return configFileName; }
            set {  configFileName = value; }
        }

         /// <summary>
        /// The volume control flag setting
        /// </summary>
        static public uint VolumeControlFlag
        {
            get { return volumeControlFlag; }
            set 
            {
                volumeControlFlag = value;
                ConfigSetting.Set("volumeControlFlag", value.ToString());
            }
        }

        /// <summary>
        /// the globalboolean config setting, please reference the enumeration of FilterAPI.BooleanConfig
        /// </summary>
        static public uint BooleanConfig
        {
            get { return booleanConfig; }
            set 
            {
                booleanConfig = value;
                ConfigSetting.Set("booleanConfig", value.ToString());
            }
        }

        static public bool EnableSendDeniedEvent
        {
            get { return (booleanConfig & (uint)FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT) > 0; }
            set
            {
                if (value)
                {
                    booleanConfig |= (uint)FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT;
                }
                else
                {
                    booleanConfig &= (~(uint)FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT);
                }
            }
        }

        /// <summary>
        /// don't display the directory IO request if it is true.
        /// </summary>
        static public bool DisableDirIO
        {
            get { return disableDirIO; }
            set { disableDirIO = value; }
        }

        static public bool IsRunning
        {
            get { return isRunning; }
        }

        static public ManualResetEvent StopEvent
        {
            get { return stopEvent; }
        }

        static public bool[] SelectedDisplayEvents
        {
            get
            {
                return selectedDisplayEvents;
            }
            set
            {
                selectedDisplayEvents = value;
            }
        }

        static public EventLevel EventLevel
        {
            get
            {
                return eventLevel;
            }
            set
            {
                eventLevel = value;
                EventManager.Level = value;
                ConfigSetting.Set("eventLevel", ((uint)value).ToString());
            }
        }

        static public EventOutputType EventOutputType
        {
            get
            {
                return eventOutputType;
            }
            set
            {
                eventOutputType = value;
            }
        }

        static public string EventLogFileName
        {
            get
            {
                return eventLogFileName;
            }
            set
            {
                eventLogFileName = value;
            }
        }

        static public int MaxEventLogFileSize
        {
            get
            {
                return maxEventLogFileSize;
            }
            set
            {
                maxEventLogFileSize = value;
            }
        }

        static public string EventSource
        {
            get
            {
                return eventSource;
            }
            set
            {
                eventSource = value;
            }
        }


        static public string EventLogName
        {
            get
            {
                return eventLogName;
            }
            set
            {
                eventLogName = value;
            }
        }


        public static int FilterConnectionThreads
        {
            get { return filterConnectionThreads; }
            set
            { 
                filterConnectionThreads = value;
                ConfigSetting.Set("filterConnectionThreads", value.ToString());
            }
        }

        public static uint RequestIORegistration
        {
            get { return requestIORegistration; }
            set 
            { 
                requestIORegistration = value;
                ConfigSetting.Set("requestIORegistration", value.ToString());
            }
        }

        public static uint DisplayEvents
        {
            get { return displayEvents; }
            set 
            { 
                displayEvents = value;
                ConfigSetting.Set("displayEvents", value.ToString());
            }
        }

        public static string FilterMessageLogName
        {
            get { return filterMessageLogName; }
            set 
            { 
                filterMessageLogName = value;
                ConfigSetting.Set("filterMessageLogName", value.ToString());
            }
        }

        public static long FilterMessageLogFileSize
        {
            get { return filterMessageLogFileSize; }
            set 
            {
                filterMessageLogFileSize = value;
                ConfigSetting.Set("filterMessageLogFileSize", value.ToString());
            }
        }

        public static int MaximumFilterMessages
        {
            get { return maximumFilterMessages; }
            set
            { 
                maximumFilterMessages = value;
                ConfigSetting.Set("maximumFilterMessages", value.ToString());
            }
        }

        public static bool EnableLogTransaction
        {
            get { return enableLogTransaction; }
            set 
            { 
                enableLogTransaction = value;
                ConfigSetting.Set("enableLogTransaction", value.ToString());
            }
        }

        public static bool OutputMessageToConsole
        {
            get { return outputMessageToConsole; }
            set
            {
                outputMessageToConsole = value;
                ConfigSetting.Set("outputMessageToConsole", value.ToString());
            }
        }

        public static bool EnableNotification
        {
            get { return enableNotification; }
            set
            {
                enableNotification = value;
                ConfigSetting.Set("enableNotification", value.ToString());
            }
        }

        public static List<uint> IncludePidList
        {
            get { return includePidList; }
            set { includePidList = value; }
        }

        public static List<uint> ExcludePidList
        {
            get { return excludePidList; }
            set { excludePidList = value; }
        }

        public static List<uint> ProtectPidList
        {
            get { return protectPidList; }
            set { protectPidList = value; }
        }


        public static int ConnectionTimeOut
        {
            get { return connectionTimeOut; }
            set 
            {
                connectionTimeOut = value;
                ConfigSetting.Set("connectionTimeOut", value.ToString());
            }
        }

        public static bool EnableDefaultIVKey
        {
            get { return enableDefaultIVKey; }
            set
            {
                enableDefaultIVKey = value;
                ConfigSetting.Set("enableDefaultIVKey", value.ToString());
            }
        }

        public static string AccountName
        {
            get { return accountName; }
            set
            {
                accountName = value;
                ConfigSetting.Set("accountName", value.ToString());
            }
        }


        public static string MasterPassword
        {
            get
            {
                return masterPassword; 
            }
            set
            {
                masterPassword = value;
                string encryptedPassword = Utils.AESEncryptDecryptStr(value.ToString(), Utils.EncryptType.Encryption);
                ConfigSetting.Set("masterPassword", encryptedPassword);
            }
        }

        public static string IncludedUsers
        {
            get { return includedUsers; }
            set
            {
                includedUsers = value;
                ConfigSetting.Set("includedUsers", value.ToString());
            }
        }

        public static string ExcludedUsers
        {
            get { return excludedUsers; }
            set
            {
                excludedUsers = value;
                ConfigSetting.Set("excludedUsers", value.ToString());
            }
        }

        public static string ProtectFolder
        {
            get { return protectFolder; }
            set
            {
                protectFolder = value;
                ConfigSetting.Set("protectFolder", value.ToString());
            }
        }

        public static string ProtectFolderWhiteList
        {
            get { return protectFolderWhiteList; }
            set
            {
                protectFolderWhiteList = value;
                ConfigSetting.Set("protectFolderWhiteList", value.ToString());
            }
        }

        public static string ProtectFolderBlackList
        {
            get { return protectFolderBlackList; }
            set
            {
                protectFolderBlackList = value;
                ConfigSetting.Set("protectFolderBlackList", value.ToString());
            }
        }

        public static string DRInfoFolder
        {
            get { return drInfoFolder; }
            set
            {
                drInfoFolder = value;
                ConfigSetting.Set("drInfoFolder", value.ToString());
            }
        }

        public static string ShareFolder
        {
            get { return shareFolder; }
            set
            {
                shareFolder = value;
                ConfigSetting.Set("shareFolder", value.ToString());
            }
        }

        /// <summary>
        /// The folder to drop the share file which was encrypted.
        /// the file will be automatially decrypted 
        /// </summary>
        public static string DropFolder
        {
            get { return dropFolder; }
            set
            {
                dropFolder = value;
                ConfigSetting.Set("dropFolder", value.ToString());
            }
        }

        public static string ShareFolderWhiteList
        {
            get { return shareFolderWhiteList; }
            set
            {
                shareFolderWhiteList = value;
                ConfigSetting.Set("shareFolderWhiteList", value.ToString());
            }
        }

        public static string ShareFolderBlackList
        {
            get { return shareFolderBlackList; }
            set
            {
                shareFolderBlackList = value;
                ConfigSetting.Set("shareFolderBlackList", value.ToString());
            }
        }

        public static bool StoreSharedFileMetaDataInServer
        {
            get { return storeSharedFileMetaDataInServer; }
            set
            {
                storeSharedFileMetaDataInServer = value;
                ConfigSetting.Set("storeSharedFileMetaDataInServer", value.ToString());
            }
        }

        public static string ShareFileExt
        {
            get { return shareFileExt; }
            set
            {
                shareFileExt = value;
                ConfigSetting.Set("shareFileExt", value.ToString());
            }
        }

        public static bool AddFileFilterRule( FileFilterRule newRule)
        {
            if (filterRules.ContainsKey(newRule.IncludeFileFilterMask))
            {
                //the exist filter rule already there,remove it
                filterRules.Remove(newRule.IncludeFileFilterMask);
                ConfigSetting.RemoveFilterRule(newRule.IncludeFileFilterMask);
            }

            filterRules.Add(newRule.IncludeFileFilterMask, newRule);

            ConfigSetting.AddFilterRule(newRule);

            return true;
        }

        public static void RemoveFilterRule(string includeFilterMask)
        {
            if (filterRules.ContainsKey(includeFilterMask))
            {
                filterRules.Remove(includeFilterMask);
                ConfigSetting.RemoveFilterRule(includeFilterMask);
            }

        }

        public static void RemoveAllFilterRules()
        {
            foreach (FileFilterRule filterRule in filterRules.Values)
            {
                ConfigSetting.RemoveFilterRule(filterRule.IncludeFileFilterMask);
            }

            filterRules.Clear();
        }

        public static bool IsFilterRuleExist(string includeFilterMask)
        {
            if (filterRules.ContainsKey(includeFilterMask))
            {
                return true;
            }

            return false;
        }

        public static Dictionary<string, FileFilterRule> FilterRules
        {
            get { return filterRules; }
        }

        public static bool AddProcessFilterRule(ProcessFilterRule filterRule)
        {
            string key = filterRule.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = filterRule.ProcessId;
            }

            if (processFilterRules.ContainsKey(key))
            {
                processFilterRules.Remove(key);
                ConfigSetting.RemoveProcessFilterRule(filterRule);
            }

            processFilterRules.Add(key, filterRule);
            ConfigSetting.AddProcessFilterRule(filterRule);

            return true;
        }

        public static void RemoveProcessFilterRule(ProcessFilterRule filterRule)
        {
            string key = filterRule.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = filterRule.ProcessId;
            }

            if (processFilterRules.ContainsKey(key))
            {
                processFilterRules.Remove(key);
                ConfigSetting.RemoveProcessFilterRule(filterRule);
            }
        }

        public static void ClearProcessFilterRule()
        {
            string[] filterKeys = new string[processFilterRules.Count];
            processFilterRules.Keys.CopyTo( filterKeys,0);

            foreach(string key in filterKeys)
            {
                ProcessFilterRule processFilterRule = processFilterRules[key];
                processFilterRules.Remove(key);
                ConfigSetting.RemoveProcessFilterRule(processFilterRule);
            }
        }

        public static ProcessFilterRule GetProcessFilterRule(string processId, string processNameFilterMask)
        {
            string key = processNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = processId;
            }

            if (processFilterRules.ContainsKey(key))
            {
                return processFilterRules[key];
            }

            return null;
        }

        public static Dictionary<string, ProcessFilterRule> ProcessFilterRules
        {
            get { return processFilterRules; }
        }

        public static bool AddRegistryFilterRule(RegistryFilterRule filterRule)
        {
            string key = filterRule.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = filterRule.ProcessId;
            }

            if (registryFilterRules.ContainsKey(key))
            {
                registryFilterRules.Remove(key);
                ConfigSetting.RemoveRegistryFilterRule(filterRule.ProcessId, filterRule.ProcessNameFilterMask);
            }

            registryFilterRules.Add(key, filterRule);
            ConfigSetting.AddRegistryFilterRule(filterRule);

            return true;
        }

        public static void RemoveRegistryFilterRule(RegistryFilterRule filterRule)
        {
            string key = filterRule.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = filterRule.ProcessId;
            }

            if (registryFilterRules.ContainsKey(key))
            {
                registryFilterRules.Remove(key);
                ConfigSetting.RemoveRegistryFilterRule(filterRule.ProcessId, filterRule.ProcessNameFilterMask);
            }
        }

        public static RegistryFilterRule GetRegistryFilterRule(string processId, string processNameFilterMask)
        {
            string key = processNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = processId;
            }

            if (registryFilterRules.ContainsKey(key))
            {
                return registryFilterRules[key];
            }

            return null;
        }

        public static Dictionary<string, RegistryFilterRule> RegistryFilterRules
        {
            get { return registryFilterRules; }
        }
    }
}
