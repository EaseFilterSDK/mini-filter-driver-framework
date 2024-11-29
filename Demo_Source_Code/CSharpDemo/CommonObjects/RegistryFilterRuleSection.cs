///////////////////////////////////////////////////////////////////////////////
//
//    (C) Copyright 2011 EaseFilter Inc.
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
using System.Linq;
using System.Text;
using System.Configuration;

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{
    public class RegistryFilterRuleSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public RegistryFilterRuleCollection Instances
        {
            get { return (RegistryFilterRuleCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class RegistryFilterRuleCollection : ConfigurationElementCollection
    {
        public RegistryFilterRule this[int index]
        {
            get { return (RegistryFilterRule)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(RegistryFilterRule RegistryFilterRule)
        {
            BaseAdd(RegistryFilterRule);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(RegistryFilterRule RegistryFilterRule)
        {
            BaseRemove(RegistryFilterRule.ProcessId.ToString() + RegistryFilterRule.ProcessNameFilterMask);
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
            return new RegistryFilterRule();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            //The key is the process name + user name
            return ((RegistryFilterRule)element).ProcessId.ToString() + ((RegistryFilterRule)element).ProcessNameFilterMask;
        }
    }

    public class RegistryFilterRule : ConfigurationElement
    {
        /// <summary>
        /// Control the registry access for the process with this process Id. 
        /// </summary>
        [ConfigurationProperty("processId", IsRequired = false)]
        public string ProcessId
        {
            get { return (string)base["processId"]; }
            set { base["processId"] = value; }
        }

        /// <summary>
        /// Control the registry access for the process with this process name if the process Id is 0, or it will skip it. 
        /// </summary>
        [ConfigurationProperty("processNameFilterMask", IsKey = false, IsRequired = false)]
        public string ProcessNameFilterMask
        {
            get { return (string)base["processNameFilterMask"]; }
            set { base["processNameFilterMask"] = value; }
        }

        /// <summary>
        /// Control the registry access for the process with this user name
        /// </summary>
        [ConfigurationProperty("userName", IsRequired = false)]
        public string UserName
        {
            get { return (string)base["userName"]; }
            set { base["userName"] = value; }
        }

        /// <summary>
        /// Filter the registry access based on the key name filter mask if it was set
        /// </summary>
        [ConfigurationProperty("registryKeyNameFilterMask", IsRequired = false)]
        public string RegistryKeyNameFilterMask
        {
            get { return (string)base["registryKeyNameFilterMask"]; }
            set { base["registryKeyNameFilterMask"] = value; }
        }

        /// <summary>
        /// if the process name matches the excludeProcessNames, the registry I/O will be skipped by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("excludeProcessNames", IsRequired = false)]
        public string ExcludeProcessNames
        {
            get { return (string)base["excludeProcessNames"]; }
            set { base["excludeProcessNames"] = value; }
        }

        /// <summary>
        /// if the user name matches the excludeUserNames, the registry I/O will be skipped by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("excludeUserNames", IsRequired = false)]
        public string ExcludeUserNames
        {
            get { return (string)base["excludeUserNames"]; }
            set { base["excludeUserNames"] = value; }
        }

        /// <summary>
        /// if the key name matches the excludeKeyNames, the registry I/O will be skipped by filter driver.
        /// seperate the multiple items with ';'
        /// </summary>
        [ConfigurationProperty("excludeKeyNames", IsRequired = false)]
        public string ExcludeKeyNames
        {
            get { return (string)base["excludeKeyNames"]; }
            set { base["excludeKeyNames"] = value; }
        }

        /// <summary>
        /// The the flag to control how to access the registry for the matched process or user
        /// </summary>
        [ConfigurationProperty("accessFlag", IsRequired = true)]
        public uint AccessFlag
        {
            get { return (uint)base["accessFlag"]; }
            set { base["accessFlag"] = value; }
        }

        /// <summary>
        /// Register the callback when the registry access notification was triggered
        /// </summary>
        [ConfigurationProperty("regCallbackClass", IsRequired = true)]
        public ulong RegCallbackClass
        {
            get { return (ulong)base["regCallbackClass"]; }
            set { base["regCallbackClass"] = value; }
        }


        /// <summary>
        /// If it is true, the registry access from the matched process or user will be excluded.
        /// </summary>
        [ConfigurationProperty("isExcludeFilter", IsRequired = true)]
        public bool IsExcludeFilter
        {
            get { return (bool)base["isExcludeFilter"]; }
            set { base["isExcludeFilter"] = value; }
        }

        public RegistryFilterRule Copy()
        {
            RegistryFilterRule dest = new RegistryFilterRule();
            dest.ProcessId = ProcessId;
            dest.ProcessNameFilterMask = ProcessNameFilterMask;
            dest.RegistryKeyNameFilterMask = RegistryKeyNameFilterMask;
            dest.UserName = UserName;
            dest.ExcludeProcessNames = ExcludeProcessNames;
            dest.ExcludeUserNames = ExcludeUserNames;
            dest.ExcludeKeyNames = ExcludeKeyNames;
            dest.AccessFlag = AccessFlag;
            dest.RegCallbackClass = RegCallbackClass;
            dest.IsExcludeFilter = IsExcludeFilter;

            return dest;
        }

        public RegistryFilter ToRegistryFilter()
        {
            RegistryFilter registryFilter = new RegistryFilter();

            if (ProcessId.Trim().Length > 0)
            {
                registryFilter.ProcessId = uint.Parse(ProcessId);
            }
            else
            {
                registryFilter.ProcessId = 0;
            }

            string[] excludeProcessNames = ExcludeProcessNames.Split(new char[] { ';' });
            if (excludeProcessNames.Length > 0)
            {
                foreach (string excludeProcessName in excludeProcessNames)
                {
                    if (excludeProcessName.Trim().Length > 0)
                    {
                        registryFilter.ExcludeProcessNameList.Add(excludeProcessName);
                    }
                }
            }

            string[] excludeUserNames = ExcludeUserNames.Split(new char[] { ';' });
            if (excludeUserNames.Length > 0)
            {
                foreach (string excludeUserName in excludeUserNames)
                {
                    if (excludeUserName.Trim().Length > 0)
                    {
                        registryFilter.ExcludeUserNameList.Add(excludeUserName);
                    }
                }
            }

            string[] excludeKeyNames = ExcludeKeyNames.Split(new char[] { ';' });
            if (excludeKeyNames.Length > 0)
            {
                foreach (string excludeKeyName in excludeKeyNames)
                {
                    if (excludeKeyName.Trim().Length > 0)
                    {
                        registryFilter.ExcludeKeyNameList.Add(excludeKeyName);
                    }
                }
            }

            registryFilter.ProcessNameFilterMask = ProcessNameFilterMask;
            registryFilter.RegistryKeyNameFilterMask = RegistryKeyNameFilterMask;
            registryFilter.UserName = UserName;
            registryFilter.ControlFlag = AccessFlag;
            registryFilter.RegCallbackClass = RegCallbackClass;
            registryFilter.IsExcludeFilter = IsExcludeFilter;

            return registryFilter;
        }

    }


}
