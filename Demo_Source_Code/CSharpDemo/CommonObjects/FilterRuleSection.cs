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
using System.Configuration;

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{
    public class FilterRuleSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public FilterRuleCollection Instances
        {
            get { return (FilterRuleCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class FilterRuleCollection : ConfigurationElementCollection
    {
        public FileFilterRule this[int index]
        {
            get { return (FileFilterRule)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(FileFilterRule filterRule)
        {
            BaseAdd(filterRule);
        }

        public void Add(FileFilter fileFilter)
        {
            FileFilterRule filterRule = new FileFilterRule(fileFilter);
            BaseAdd(filterRule);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(FileFilterRule filterRule)
        {
            BaseRemove(filterRule.IncludeFileFilterMask);
        }

        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }

        public void Remove(string name)
        {
            BaseRemove(name);
        }

        protected override ConfigurationElement CreateNewElement()
        {
            return new FileFilterRule();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //set to whatever Element Property you want to use for a key
            return ((FileFilterRule)element).IncludeFileFilterMask;
        }
    }

    public class FileFilterRule : ConfigurationElement
    {
        //A filter rule must have a unique include file filter mask, 
        //A filter rule can have multiple exclude file filter masks.
        //Make sure to set IsKey=true for property exposed as the GetElementKey above
        /// <summary>
        /// if the file path matches the includeFileFilterMask, the filter driver will process the registered IOs, or it will skip the IOs for this file.   
        /// </summary>
        [ConfigurationProperty("includeFileFilterMask", IsKey = true, IsRequired = true)]
        public string IncludeFileFilterMask
        {
            get { return (string)base["includeFileFilterMask"]; }
            set { base["includeFileFilterMask"] = value; }
        }

        /// <summary>
        /// If the file path matches the excludeFileFilterMasks, the filter driver will skip the IOs for this file.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("excludeFileFilterMasks", IsRequired = false)]
        public string ExcludeFileFilterMasks
        {
            get { return (string)base["excludeFileFilterMasks"]; }
            set { base["excludeFileFilterMasks"] = value; }
        }

        /// <summary>
        /// if the file path matches the includeFileFilterMask and the includeProcessNames is not empty,
        /// only the IOs from the process name which matches the includeProcessNames will be managed by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("includeProcessNames", IsRequired = false)]
        public string IncludeProcessNames
        {
            get { return (string)base["includeProcessNames"]; }
            set { base["includeProcessNames"] = value; }
        }

        /// <summary>
        /// if the file path matches the includeFileFilterMask and the excludeProcessNames is not empty,
        /// the IOs from the process name which matches the excludeProcessNames will be skipped by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("excludeProcessNames", IsRequired = false)]
        public string ExcludeProcessNames
        {
            get { return (string)base["excludeProcessNames"]; }
            set { base["excludeProcessNames"] = value; }
        }

        /// <summary>
        /// if the file path matches the includeFileFilterMask and the includeUserNames is not empty,
        /// only the IOs from the user name which matches the includeUserNames will be managed by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("includeUserNames", IsRequired = false)]
        public string IncludeUserNames
        {
            get { return (string)base["includeUserNames"]; }
            set { base["includeUserNames"] = value; }
        }

        /// <summary>
        /// if the file path matches the includeFileFilterMask and the excludeUserNames is not empty,
        /// the IOs from the user name which matches the excludeUserNames will be skipped by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("excludeUserNames", IsRequired = false)]
        public string ExcludeUserNames
        {
            get { return (string)base["excludeUserNames"]; }
            set { base["excludeUserNames"] = value; }
        }

        /// <summary>
        /// if the file path matches the includeFileFilterMask and the includeProcessIds is not empty,
        /// only the IOs from the process Id which matches the includeProcessIds will be managed by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("includeProcessIds", IsRequired = false)]
        public string IncludeProcessIds
        {
            get { return (string)base["includeProcessIds"]; }
            set { base["includeProcessIds"] = value; }
        }

        /// <summary>
        /// if the file path matches the includeFileFilterMask and the excludeProcessIds is not empty,
        /// the IOs from the process Id which matches the excludeProcessIds will be skipped by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("excludeProcessIds", IsRequired = false)]
        public string ExcludeProcessIds
        {
            get { return (string)base["excludeProcessIds"]; }
            set { base["excludeProcessIds"] = value; }
        }

        /// <summary>
        /// if the file path matches hiddenFileFilterMasks, the file will be hidden 
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("hiddenFileFilterMasks", IsRequired = false)]
        public string HiddenFileFilterMasks
        {
            get { return (string)base["hiddenFileFilterMasks"]; }
            set { base["hiddenFileFilterMasks"] = value; }
        }

        /// <summary>
        /// if the reparse file was enabled, the open to the file path will be reparsed to the new path replaced by reparseFileFilterMasks
        /// </summary>
        [ConfigurationProperty("reparseFileFilterMask", IsRequired = false)]
        public string ReparseFileFilterMask
        {
            get { return (string)base["reparseFileFilterMask"]; }
            set { base["reparseFileFilterMask"] = value; }
        }

        /// <summary>
        /// The file access rights for the user name list, seperated with ";" for multiple users
        /// the format is "userName|accessFlags;", e.g. "mydomain\user1|123456;"
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("userRights", IsRequired = false)]
        public string UserRights
        {
            get { return (string)base["userRights"]; }
            set { base["userRights"] = value; }
        }

        /// <summary>
        /// the process access rights with format "accessFlags|processName|certificateName|image256Hash"
        /// for example: "123456|notepad.exe|EaseFilter Technologies|A123234234BADSFSFASDFSFASFASDF"
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("processNameRights", IsRequired = false)]
        public string ProcessNameRights
        {
            get { return (string)base["processNameRights"]; }
            set { base["processNameRights"] = value; }
        }

        /// <summary>
        /// The file access rights inside the filter rule for the process Id list, seperated with ";" for multiple processes
        /// the format is "processId|accessFlags;", e.g. "1234|123456;"
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("processIdRights", IsRequired = false)]
        public string ProcessIdRights
        {
            get { return (string)base["processIdRights"]; }
            set { base["processIdRights"] = value; }
        }

    
        [ConfigurationProperty("encryptionPassPhrase", IsRequired = false)]
        public string EncryptionPassPhrase
        {
            get
            {
                string key = (string)base["encryptionPassPhrase"];
                if (Utils.IsBase64String(key))
                {
                    key = Utils.AESEncryptDecryptStr(key, Utils.EncryptType.Decryption);
                }

                return key;
            }
            set 
            {
                string key = value.Trim();

                if (key.Length > 0)
                {
                    key = Utils.AESEncryptDecryptStr(key, Utils.EncryptType.Encryption);
                }

                base["encryptionPassPhrase"] = key; 
            }
        }

        /// <summary>
        /// the AES encryption key size, it could be 8bytes(128bit), 24bytes(196bits), 32bytes(256bits).
        /// </summary>
        [ConfigurationProperty("encryptionKeySize", IsRequired = false)]
        public int EncryptionKeySize
        {
            get { return (int)base["encryptionKeySize"]; }
            set { base["encryptionKeySize"] = value; }
        }

        /// <summary>
        /// If the encrypt write buffer size is greater than 0, then the small buffer encryption write will be combined together to a bigger buffer,
        /// and write it to the disk.
        /// </summary>
        [ConfigurationProperty("encryptWriteBufferSize", IsRequired = false)]
        public uint EncryptWriteBufferSize
        {
            get { return (uint)base["encryptWriteBufferSize"]; }
            set { base["encryptWriteBufferSize"] = value; }
        }


        /// <summary>
        /// the file access control flags for the filter rule.
        /// </summary>
        [ConfigurationProperty("accessFlag", IsRequired = true)]
        public uint AccessFlag
        {
            get { return (uint)base["accessFlag"]; }
            set { base["accessFlag"] = value; }
        }

        /// <summary>
        /// The register the file events
        /// </summary>
        [ConfigurationProperty("fileChangeEventFilter", IsRequired = false)]
        public uint FileChangeEventFilter
        {
            get { return (uint)base["fileChangeEventFilter"]; }
            set { base["fileChangeEventFilter"] = value; }
        }

        /// <summary>
        /// register monitor I/O requests
        /// </summary>
        [ConfigurationProperty("registerMonitorFileIOEvents", IsRequired = false)]
        public ulong RegisterMonitorFileIOEvents
        {
            get { return (ulong)base["registerMonitorFileIOEvents"]; }
            set { base["registerMonitorFileIOEvents"] = value; }
        }       
       
        /// <summary>
        /// If it is true, the filter driver will send the monitor events asynchronously, the monitor event buffer will be used. 
        /// if it is false, the filter driver will send the monitor events synchronously, block and wait till the events being picked up.
        /// </summary>
        [ConfigurationProperty("enableMonitorEventBuffer", IsRequired = false)]
        public bool EnableMonitorEventBuffer
        {
            get { return (bool)base["enableMonitorEventBuffer"]; }
            set { base["enableMonitorEventBuffer"] = value; }
        }

        /// <summary>
        /// register control I/O requests, the filter driver will block and wait for the response.
        /// </summary>
        [ConfigurationProperty("registerControlFileIOEvents", IsRequired = false)]
        public ulong RegisterControlFileIOEvents
        {
            get { return (ulong)base["registerControlFileIOEvents"]; }
            set { base["registerControlFileIOEvents"] = value; }
        }

        [ConfigurationProperty("filterDesiredAccess", IsRequired = false)]
        public uint FilterDesiredAccess
        {
            get { return (uint)base["filterDesiredAccess"]; }
            set { base["filterDesiredAccess"] = value; }
        }

        [ConfigurationProperty("filterDisposition", IsRequired = false)]
        public uint FilterDisposition
        {
            get { return (uint)base["filterDisposition"]; }
            set { base["filterDisposition"] = value; }
        }

        [ConfigurationProperty("filterCreateOptions", IsRequired = false)]
        public uint FilterCreateOptions
        {
            get { return (uint)base["filterCreateOptions"]; }
            set { base["filterCreateOptions"] = value; }
        }
        /// <summary>
        /// Enable the filter rule in boot time for control filter if it is true.
        /// </summary>
        [ConfigurationProperty("isResident", IsRequired = false)]
        public bool IsResident
        {
            get { return (bool)base["isResident"]; }
            set { base["isResident"] = value; }
        }

        /// <summary>
        /// the encryption method for filter driver based on the filter rule, reference EncryptMethod enumeration definition
        /// </summary>
        [ConfigurationProperty("encryptMethod", IsRequired = false)]
        public int EncryptMethod
        {
            get { return (int)base["encryptMethod"]; }
            set { base["encryptMethod"] = value; }
        }

        /// <summary>
        /// The filter rule type to categorize the filter rules.
        /// </summary>
        [ConfigurationProperty("type", IsRequired = false)]
        public int Type
        {
            get { return (int)base["type"]; }
            set { base["type"] = value; }
        }

        public FileFilterRule()
        {

        }

        public FileFilterRule(FileFilter fileFilter)
        {
            IncludeFileFilterMask = fileFilter.IncludeFileFilterMask;
            AccessFlag = (uint)fileFilter.AccessFlags;
            FileChangeEventFilter = (uint)fileFilter.FileChangeEventFilter;
            IsResident = fileFilter.IsResident;
            FilterCreateOptions = fileFilter.FilterCreateOptions;
            FilterDesiredAccess = fileFilter.FilterDesiredAccess;
            FilterDisposition = fileFilter.FilterDisposition;
            EnableMonitorEventBuffer = fileFilter.EnableMonitorEventBuffer;
            EncryptionPassPhrase = fileFilter.EncryptionPassPhrase;
            EncryptWriteBufferSize = fileFilter.EncryptWriteBufferSize;
            RegisterControlFileIOEvents = (ulong)fileFilter.ControlFileIOEventFilter;
            RegisterMonitorFileIOEvents = (ulong)fileFilter.MonitorFileIOEventFilter;
            ExcludeFileFilterMasks = fileFilter.ExcludeFileFilterMaskString;
            ExcludeProcessNames = fileFilter.ExcludeProcessNameString;
            ExcludeUserNames = fileFilter.ExcludeUserNameString;
            IncludeProcessNames = fileFilter.IncludeProcessNameString;
            IncludeUserNames = fileFilter.IncludeUserNameString;
            ProcessNameRights = fileFilter.ProcessNameAccessRightString;
            UserRights = fileFilter.UserAccessRightString;
            HiddenFileFilterMasks = fileFilter.HiddenFileFilterMaskString;
            ReparseFileFilterMask = fileFilter.ReparseFileFilterMask;
            IncludeProcessIds = fileFilter.IncludeProcessIdString;
            ExcludeProcessIds = fileFilter.ExcludeProcessIdString;
            ProcessIdRights = fileFilter.ProcessIdAccessRightString;

        }

        public FileFilter ToFileFilter()
        {
            try
            {
                FileFilter fileFilter = new FileFilter(IncludeFileFilterMask);
                fileFilter.AccessFlags = (FilterAPI.AccessFlag)AccessFlag;
                fileFilter.FileChangeEventFilter = (FilterAPI.FileChangedEvents)FileChangeEventFilter;
                fileFilter.IsResident = IsResident;
                fileFilter.FilterCreateOptions = FilterCreateOptions;
                fileFilter.FilterDesiredAccess = FilterDesiredAccess;
                fileFilter.FilterDisposition = FilterDisposition;
                fileFilter.EnableMonitorEventBuffer = EnableMonitorEventBuffer;
                fileFilter.EncryptWriteBufferSize = EncryptWriteBufferSize;
                fileFilter.ControlFileIOEventFilter = (ControlFileIOEvents)RegisterControlFileIOEvents;
                fileFilter.MonitorFileIOEventFilter = (MonitorFileIOEvents)RegisterMonitorFileIOEvents;

                if ((fileFilter.AccessFlags & FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0)
                {
                    fileFilter.EnableBlockSaveAs = true;
                }

                fileFilter.ExcludeFileFilterMaskString = ExcludeFileFilterMasks;
                fileFilter.ExcludeProcessIdString = ExcludeProcessIds;
                fileFilter.ExcludeProcessNameString = ExcludeProcessNames;
                fileFilter.ExcludeUserNameString = ExcludeUserNames;
                fileFilter.IncludeProcessIdString = IncludeProcessIds;
                fileFilter.IncludeProcessNameString = IncludeProcessNames;
                fileFilter.IncludeUserNameString = IncludeUserNames;
                fileFilter.ProcessIdAccessRightString = ProcessIdRights;
                fileFilter.ProcessNameAccessRightString = ProcessNameRights;
                fileFilter.UserAccessRightString = UserRights;

                if ((AccessFlag & (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) > 0)
                {
                    fileFilter.EnableHiddenFile = true;
                    fileFilter.HiddenFileFilterMaskString = HiddenFileFilterMasks;
                }

                fileFilter.ReparseFileFilterMask = ReparseFileFilterMask;

                if ((AccessFlag & (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) > 0)
                {
                    fileFilter.EnableEncryption = true;
                    fileFilter.EncryptionPassPhrase = EncryptionPassPhrase;
                }

                return fileFilter;
            }
            catch (Exception ex)
            {
                throw new Exception("Convert filter rule to FileFilter failed," + ex.Message);
            }
        }
        
    }


}
