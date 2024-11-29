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
    public class ProcessFilterRuleSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public ProcessFilterRuleCollection Instances
        {
            get { return (ProcessFilterRuleCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class ProcessFilterRuleCollection : ConfigurationElementCollection
    {
        public ProcessFilterRule this[int index]
        {
            get { return (ProcessFilterRule)BaseGet(index); }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }

        public void Add(ProcessFilterRule ProcessFilterRule)
        {
            BaseAdd(ProcessFilterRule);
        }

        public void Clear()
        {
            BaseClear();
        }

        public void Remove(ProcessFilterRule ProcessFilterRule)
        {
            BaseRemove(ProcessFilterRule.ProcessNameFilterMask + ProcessFilterRule.ProcessId);
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
            return new ProcessFilterRule();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ProcessFilterRule)element).ProcessNameFilterMask + ((ProcessFilterRule)element).ProcessId;
        }
    }

    public class ProcessFilterRule : ConfigurationElement
    {

        /// <summary>
        /// the process name filter mask, i.e. "notepad.exe","c:\\windows\\*.exe" 
        /// </summary>
        [ConfigurationProperty("processNameFilterMask", IsKey = true, IsRequired = true)]
        public string ProcessNameFilterMask
        {
            get { return (string)base["processNameFilterMask"]; }
            set { base["processNameFilterMask"] = value; }
        }

        /// <summary>
        /// The process Id here is to control the file access.
        /// </summary>
        [ConfigurationProperty("processId",IsRequired = false)]
        public string ProcessId
        {
            get { return (string)base["processId"]; }
            set { base["processId"] = value; }
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
        /// The file access rights to the processes which match the processNameFilterMask
        /// the format is "FileMask!accessFalg;" e.g. "c:\sandbox\*!12356;"
        /// </summary>
        [ConfigurationProperty("fileAccessRights", IsRequired = false)]
        public string FileAccessRights
        {
            get { return (string)base["fileAccessRights"]; }
            set { base["fileAccessRights"] = value; }
        }

        /// <summary>
        /// The control flag to to the processes which match the processNameFilterMask
        /// </summary>
        [ConfigurationProperty("controlFlag", IsRequired = false)]
        public uint ControlFlag
        {
            get { return (uint)base["controlFlag"]; }
            set { base["controlFlag"] = value; }
        }

        public ProcessFilterRule Copy()
        {
            ProcessFilterRule dest = new ProcessFilterRule();
            dest.ProcessId = ProcessId;
            dest.ProcessNameFilterMask = ProcessNameFilterMask;
            dest.ExcludeProcessNames = ExcludeProcessNames;
            dest.ExcludeUserNames = ExcludeUserNames;
            dest.FileAccessRights = FileAccessRights;
            dest.ControlFlag = ControlFlag;

            return dest;
        }

        public ProcessFilter ToProcessFilter()
        {
            ProcessFilter processFilter = new ProcessFilter(ProcessNameFilterMask);

            processFilter.FilterType = FilterAPI.FilterType.PROCESS_FILTER;

            if (ProcessId.Trim().Length > 0)
            {
                processFilter.ProcessId = uint.Parse(ProcessId);
            }
            else
            {
                processFilter.ProcessId = 0;
            }

            string[] excludeProcessNames = ExcludeProcessNames.Split(new char[] { ';' });
            if (excludeProcessNames.Length > 0)
            {
                foreach (string excludeProcessName in excludeProcessNames)
                {
                    if (excludeProcessName.Trim().Length > 0)
                    {
                        processFilter.ExcludeProcessNameList.Add(excludeProcessName);
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
                        processFilter.ExcludeUserNameList.Add(excludeUserName);
                    }
                }
            }

            processFilter.ProcessNameFilterMask = ProcessNameFilterMask;      
            processFilter.ControlFlag = ControlFlag;

            string[] fileAccessRights = FileAccessRights.Split(new char[] { ';' });
            if (fileAccessRights.Length > 0)
            {
                foreach (string fileAccessRight in fileAccessRights)
                {
                    if (fileAccessRight.Trim().Length > 0)
                    {
                        string fileNamFilterMask = fileAccessRight.Substring(0, fileAccessRight.IndexOf('!'));
                        uint accessFlags = uint.Parse(fileAccessRight.Substring(fileAccessRight.IndexOf('!') + 1));
                        processFilter.FileAccessRights.Add(fileNamFilterMask, accessFlags);
                    }
                }
            }

            return processFilter;
        }

    }


}
