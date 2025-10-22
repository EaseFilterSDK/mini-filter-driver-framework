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
///

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
        
        static List<uint> includePidList = new List<uint>();
        static List<uint> excludePidList = new List<uint>();
        static List<uint> protectPidList = new List<uint>();
        static string includedUsers = string.Empty;
        static string excludedUsers = string.Empty;

        static uint requestIORegistration = 0;
        static uint displayEvents = 0;

        static string accountName = "";
        static string masterPassword = "testpassword";
        static string licenseKey = "";
        static long expireTime = 0;

        static int maximumFilterMessages = 1000;
        static string filterMessageLogName = "filterMessage.log";
        static long filterMessageLogFileSize = 10 * 1024 * 1024;
        static bool enableLogTransaction = false;
        static bool outputMessageToConsole = true;
        static bool enableNotification = false;

        static string autoEncryptFolder = AssemblyPath + "\\protectFolder";
        static string authorizedProcessNames = "*";
        static string unAuthorizedProcessNames = "explorer.exe";
        static string authorizedUserNames = "*";
        static string unAuthorizedUserNames = "";
        static long shareFileExpireTime = DateTime.Now.AddDays(2).ToFileTimeUtc();
        //the folder to drop the shared encrypted files
        static string dropFolder = AssemblyPath + "\\dropFolder";
        //the folder to store the local DRM data
        static string drmFolder = AssemblyPath + "\\drmFolder";
        //store the DRM data in server if it is true.
        static bool isDRMDataInLocal = true;

        /// <summary>
        /// rehydrate the stub file on the first read if it is true.
        /// </summary>
        static bool rehydrateFileOnFirstRead = false;
        /// <summary>
        /// download the whole file to the cache folder, and return the cache file name to the filter driver.
        /// </summary>
        static bool returnCacheFileName = false;
        /// <summary>
        /// return the requested block of data if it is true.
        /// </summary>
        static bool returnBlockData = true;
        //the folder to store the test source files
        static string testSourceFolder = AssemblyPath + "\\testSourceFolder";
        //the folder to store the test stub files
        static string testStubFolder = AssemblyPath + "\\testStubFolder";

        static string configFileName = ConfigSetting.GetFilePath();

        //the filter driver will use the default IV to encrypt the new file if it is true.
        static bool enableDefaultIVKey = false;

        static uint currentPid = (uint)System.Diagnostics.Process.GetCurrentProcess().Id;

        //dont display the directory IO request if it is true.
        static bool disableDirIO = false;

        static FilterAPI.BooleanConfig booleanConfig = FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT|FilterAPI.BooleanConfig.ENABLE_SET_FILE_ID_INFO | FilterAPI.BooleanConfig.ENABLE_SET_USER_PROCESS_NAME;

        public static bool isRunning = true;
        public static ManualResetEvent stopEvent = new ManualResetEvent(false);

        public static FilterAPI.FilterType filterType = FilterAPI.FilterType.MONITOR_FILTER;

        public static Stopwatch stopWatch = new Stopwatch();

        static private Dictionary<string, FileFilter> fileFilters = new Dictionary<string, FileFilter>();
        static private Dictionary<string, ProcessFilter> processFilters = new Dictionary<string, ProcessFilter>();
        //the registry filter rule collection
        static private Dictionary<string, RegistryFilter> registryFilters = new Dictionary<string, RegistryFilter>();

        static GlobalConfig()
        {
            string lastError = string.Empty;           

           stopWatch.Start();
           Load();

           Utils.CopyOSPlatformDependentFiles(ref lastError);
           
        }

        public static void Load()
        {
            fileFilters.Clear();
            processFilters.Clear();
            registryFilters.Clear();

            try
            {

                fileFilters = ConfigSetting.GetFileFilters();
                processFilters = ConfigSetting.GetProcessFilters();
                registryFilters = ConfigSetting.GetRegistryFilters();

                booleanConfig =(FilterAPI.BooleanConfig)ConfigSetting.Get("booleanConfig", (uint)booleanConfig);
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
                autoEncryptFolder = ConfigSetting.Get("autoEncryptFolder", autoEncryptFolder);
                authorizedProcessNames = ConfigSetting.Get("authorizedProcessNames", authorizedProcessNames);
                unAuthorizedProcessNames = ConfigSetting.Get("unAuthorizedProcessNames", unAuthorizedProcessNames);
                authorizedUserNames = ConfigSetting.Get("authorizedUserNames", authorizedUserNames);
                unAuthorizedUserNames = ConfigSetting.Get("unAuthorizedUserNames", unAuthorizedUserNames);
                shareFileExpireTime = ConfigSetting.Get("shareFileExpireTime", shareFileExpireTime);
                dropFolder = ConfigSetting.Get("dropFolder", dropFolder);
                drmFolder = ConfigSetting.Get("drmFolder", drmFolder);
                isDRMDataInLocal = ConfigSetting.Get("isDRMDataInLocal", isDRMDataInLocal);

                volumeControlFlag = ConfigSetting.Get("volumeControlFlag", volumeControlFlag);

                rehydrateFileOnFirstRead = ConfigSetting.Get("rehydrateFileOnFirstRead", rehydrateFileOnFirstRead);
                returnCacheFileName = ConfigSetting.Get("returnCacheFileName", returnCacheFileName);
                returnBlockData = ConfigSetting.Get("returnBlockData", returnBlockData);
                testSourceFolder = ConfigSetting.Get("testSourceFolder", testSourceFolder);
                testStubFolder = ConfigSetting.Get("testStubFolder", testStubFolder);

                licenseKey = ConfigSetting.Get("licenseKey", licenseKey);
                expireTime = ConfigSetting.Get("expireTime", expireTime);

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(176, "LoadConfigSetting", EventLevel.Error, "Load config file " + configFileName + " failed with error:" + ex.Message);
            }
        }

        public static string GetVersionInfo()
        {
            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();

            try
            {
                string filterDllPath = Path.Combine(GlobalConfig.AssemblyPath, "FilterAPI.Dll");
                version = FileVersionInfo.GetVersionInfo(filterDllPath).ProductVersion;

                string computerId = FilterAPI.GetComputerId().ToString();

                version ="  Version:" + version;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(43, "LoadFilterAPI Dll", EventLevel.Error, "FilterAPI.dll can't be found." + ex.Message);
            }

            return version;
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
        static public FilterAPI.BooleanConfig BooleanConfig
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
            get { return (booleanConfig & FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT) > 0; }
            set
            {
                if (value)
                {
                    booleanConfig |= FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT;
                }
                else
                {
                    booleanConfig &= (~FilterAPI.BooleanConfig.ENABLE_SEND_DENIED_EVENT);
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
            get
            { 
                return accountName; 
            }
            set
            {
                accountName = value;
                ConfigSetting.Set("accountName", value.ToString());
            }
        }

         public static string LicenseKey
        {
            get
            {
                //Purchase a license key with the link: http://www.easefilter.com/Order.htm
                //Email us to request a trial key: info@easefilter.com //free email is not accepted.

                //for demo code.
                if (string.IsNullOrEmpty(licenseKey))
                {
                    System.Windows.Forms.MessageBox.Show("You don't have a valid license key, Please contact support@easefilter.com to get a trial key.", "LicenseKey",
                        System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }

                return licenseKey;
            }
            set
            {
                licenseKey = value;
                ConfigSetting.Set("licenseKey", value.ToString());
            }
         }

         public static long ExpireTime
         {
             get { return expireTime; }
             set
             {
                 expireTime = value;
                 ConfigSetting.Set("expireTime", value.ToString());
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

        public static string AutoEncryptFolder
        {
            get { return autoEncryptFolder; }
            set
            {
                autoEncryptFolder = value;
                ConfigSetting.Set("autoEncryptFolder", value.ToString());
            }
        }

        public static string AuthorizedProcessNames
        {
            get { return authorizedProcessNames; }
            set
            {
                authorizedProcessNames = value;
                ConfigSetting.Set("authorizedProcessNames", value.ToString());
            }
        }

        public static string UnAuthorizedProcessNames
        {
            get { return unAuthorizedProcessNames; }
            set
            {
                unAuthorizedProcessNames = value;
                ConfigSetting.Set("unAuthorizedProcessNames", value.ToString());
            }
        }

        public static string AuthorizedUserNames
        {
            get { return authorizedUserNames; }
            set
            {
                authorizedUserNames = value;
                ConfigSetting.Set("authorizedUserNames", value.ToString());
            }
        }

        public static string UnAuthorizedUserNames
        {
            get { return unAuthorizedUserNames; }
            set
            {
                unAuthorizedUserNames = value;
                ConfigSetting.Set("unAuthorizedUserNames", value.ToString());
            }
        }

        public static long ShareFileExpireTime
        {
            get { return shareFileExpireTime; }
            set
            {
                shareFileExpireTime = value;
                ConfigSetting.Set("shareFileExpireTime", value.ToString());
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

        /// <summary>
        /// The folder to store the share file DRM data.
        /// </summary>
        public static string DRMFolder
        {
            get { return drmFolder; }
            set
            {
                drmFolder = value;
                ConfigSetting.Set("drmFolder", value.ToString());
            }
        }

        /// <summary>
        /// store the DRM data in server if it is true.
        /// </summary>
        public static bool IsDRMDataInLocal
        {
            get { return isDRMDataInLocal; }
            set
            {
                isDRMDataInLocal = value;
                ConfigSetting.Set("isDRMDataInLocal", value.ToString());
            }
        }

        /// <summary>
        /// if this flag is true, the stub file will be rehydrated on first read.
        /// </summary>
        public static bool RehydrateFileOnFirstRead
        {
            get { return rehydrateFileOnFirstRead; }
            set
            {
                rehydrateFileOnFirstRead = value;
                ConfigSetting.Set("rehydrateFileOnFirstRead", value.ToString());
            }
        }

        /// <summary>
        /// if this flag is true, the whole cache file name will be returned.
        /// </summary>
        public static bool ReturnCacheFileName
        {
            get { return returnCacheFileName; }
            set
            {
                returnCacheFileName = value;
                ConfigSetting.Set("returnCacheFileName", value.ToString());
            }
        }

        /// <summary>
        /// if this flag is true, the block of requested data will be returned.
        /// </summary>
        public static bool ReturnBlockData
        {
            get { return returnBlockData; }
            set
            {
                returnBlockData = value;
                ConfigSetting.Set("returnBlockData", value.ToString());
            }
        }

        /// <summary>
        /// The folder to store the test source files for stub file filter driver.
        /// </summary>
        public static string TestSourceFolder
        {
            get { return testSourceFolder; }
            set
            {
                testSourceFolder = value;
                ConfigSetting.Set("testSourceFolder", value.ToString());
            }
        }

        /// <summary>
        /// The folder to store the test stub files for stub file filter driver.
        /// </summary>
        public static string TestStubFolder
        {
            get { return testStubFolder; }
            set
            {
                testStubFolder = value;
                ConfigSetting.Set("testStubFolder", value.ToString());
            }
        }

        public static bool AddFileFilter( FileFilter fileFilter)
        {
            if (fileFilters.ContainsKey(fileFilter.IncludeFileFilterMask))
            {
                //the exist filter rule already there,remove it
                fileFilters.Remove(fileFilter.IncludeFileFilterMask);
                ConfigSetting.RemoveFileFilter(fileFilter.IncludeFileFilterMask);
            }

            fileFilters.Add(fileFilter.IncludeFileFilterMask, fileFilter);

            ConfigSetting.AddFileFilter(fileFilter);

            return true;
        }

        public static void RemoveFileFilter(string includeFilterMask)
        {
            if (fileFilters.ContainsKey(includeFilterMask))
            {
                fileFilters.Remove(includeFilterMask);
                ConfigSetting.RemoveFileFilter(includeFilterMask);
            }
        }

        public static Dictionary<string, FileFilter> FileFilters
        {
            get { return fileFilters; }
        }

        public static bool AddProcessFilter(ProcessFilter processFilter)
        {
            string key = processFilter.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = processFilter.ProcessId.ToString();
            }

            if (processFilters.ContainsKey(key))
            {
                processFilters.Remove(key);
                ConfigSetting.RemoveProcessFilter(processFilter);
            }

            processFilters.Add(key, processFilter);
            ConfigSetting.AddProcessFilter(processFilter);

            return true;
        }

        public static void RemoveProcessFilter(ProcessFilter processFilter)
        {
            string key = processFilter.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = processFilter.ProcessId.ToString();
            }

            if (processFilters.ContainsKey(key))
            {
                processFilters.Remove(key);
                ConfigSetting.RemoveProcessFilter(processFilter);
            }
        }

        public static void ClearProcessFilterRule()
        {
            string[] filterKeys = new string[processFilters.Count];
            processFilters.Keys.CopyTo( filterKeys,0);

            foreach(string key in filterKeys)
            {
                ProcessFilter processFilter = processFilters[key];
                processFilters.Remove(key);
                ConfigSetting.RemoveProcessFilter(processFilter);
            }
        }

        public static ProcessFilter GetProcessFilter(string processId, string processNameFilterMask)
        {
            string key = processNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = processId;
            }

            if (processFilters.ContainsKey(key))
            {
                return processFilters[key];
            }

            return null;
        }

        public static Dictionary<string, ProcessFilter> ProcessFilters
        {
            get { return processFilters; }
        }

        public static bool AddRegistryFilter(RegistryFilter registryFilter)
        {
            string key = registryFilter.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = registryFilter.ProcessId.ToString();
            }

            if (registryFilters.ContainsKey(key))
            {
                registryFilters.Remove(key);
                ConfigSetting.RemoveRegistryFilter(registryFilter);
            }

            registryFilters.Add(key, registryFilter);
            ConfigSetting.AddRegistryFilter(registryFilter);

            return true;
        }

        public static void RemoveRegistryFilter(RegistryFilter registryFilter)
        {
            string key = registryFilter.ProcessNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = registryFilter.ProcessId.ToString();
            }

            if (registryFilters.ContainsKey(key))
            {
                registryFilters.Remove(key);
                ConfigSetting.RemoveRegistryFilter(registryFilter);
            }
        }

        public static RegistryFilter GetRegistryFilter(string processId, string processNameFilterMask)
        {
            string key = processNameFilterMask;

            if (key.Trim().Length == 0)
            {
                key = processId;
            }

            if (registryFilters.ContainsKey(key))
            {
                return registryFilters[key];
            }

            return null;
        }

        public static Dictionary<string, RegistryFilter> RegistryFilters
        {
            get { return registryFilters; }
        }
    }
}
