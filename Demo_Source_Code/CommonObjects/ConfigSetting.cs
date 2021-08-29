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
using System.Configuration;
using System.Collections;
using System.Collections.Generic;

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{

    public class ConfigSetting
    {
        static private string configPath = string.Empty;
        static private System.Configuration.Configuration config = null;
        static private FilterRuleSection filterRuleSection = new FilterRuleSection();
        static private RegistryFilterRuleSection registryFilterRuleSection = new RegistryFilterRuleSection();
        static private ProcessFilterRuleSection processFilterRuleSection = new ProcessFilterRuleSection();

        static ConfigSetting()
        {
            try
            {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                configPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;

                filterRuleSection = (FilterRuleSection)config.Sections["FilterRuleSection"];
                registryFilterRuleSection = (RegistryFilterRuleSection)config.Sections["RegistryFilterRuleSection"];
                processFilterRuleSection = (ProcessFilterRuleSection)config.Sections["ProcessFilterRuleSection"];

                if (filterRuleSection == null)
                {
                    filterRuleSection = new FilterRuleSection();
                    config.Sections.Add("FilterRuleSection", filterRuleSection);

                }

                if (registryFilterRuleSection == null)
                {
                    registryFilterRuleSection = new RegistryFilterRuleSection();
                    config.Sections.Add("RegistryFilterRuleSection", registryFilterRuleSection);

                }

                if (processFilterRuleSection == null)
                {
                    processFilterRuleSection = new ProcessFilterRuleSection();
                    config.Sections.Add("ProcessFilterRuleSection", processFilterRuleSection);

                }
            }
            catch
            {
            }
        }

        public static void Save()
        {
            config.Save(ConfigurationSaveMode.Full);
        }

        public static Dictionary<string, FileFilterRule> GetFilterRules()
        {
            Dictionary<string, FileFilterRule> filterRules = new Dictionary<string, FileFilterRule>();

            foreach (FileFilterRule filterRule in filterRuleSection.Instances)
            {
                filterRules.Add(filterRule.IncludeFileFilterMask, filterRule);
            }

            return filterRules;
        }

        public static void AddFilterRule(FileFilterRule filterRule)
        {

            filterRuleSection.Instances.Add(filterRule);

            return ;
        }

        public static void RemoveFilterRule(string includeFilterMask)
        {
            filterRuleSection.Instances.Remove(includeFilterMask);
            FilterAPI.RemoveFilterRule(includeFilterMask);

            return;
        }

        public static Dictionary<string, ProcessFilterRule> GetProcessFilterRules()
        {
            Dictionary<string, ProcessFilterRule> filterRules = new Dictionary<string, ProcessFilterRule>();

            foreach (ProcessFilterRule filterRule in processFilterRuleSection.Instances)
            {
                filterRules.Add(filterRule.ProcessNameFilterMask + filterRule.ProcessId, filterRule);
            }

            return filterRules;
        }

        public static void AddProcessFilterRule(ProcessFilterRule filterRule)
        {
            processFilterRuleSection.Instances.Add(filterRule);
            return;
        }

        public static void RemoveProcessFilterRule(ProcessFilterRule filterRule)
        {
            processFilterRuleSection.Instances.Remove(filterRule.ProcessNameFilterMask + filterRule.ProcessId);

            if (filterRule.ProcessNameFilterMask.Length > 0)
            {
                FilterAPI.RemoveProcessFilterRule((uint)filterRule.ProcessNameFilterMask.Length * 2, filterRule.ProcessNameFilterMask);
            }

            return;
        }

        public static Dictionary<string, RegistryFilterRule> GetRegistryFilterRules()
        {
            Dictionary<string, RegistryFilterRule> registryFilterRules = new Dictionary<string, RegistryFilterRule>();

            foreach (RegistryFilterRule registryFilterRule in registryFilterRuleSection.Instances)
            {
                registryFilterRules.Add(registryFilterRule.ProcessId + registryFilterRule.ProcessNameFilterMask, registryFilterRule);
            }

            return registryFilterRules;
        }

        public static void AddRegistryFilterRule(RegistryFilterRule registryFilterRule)
        {

            registryFilterRuleSection.Instances.Add(registryFilterRule);

            return;
        }

        public static void RemoveRegistryFilterRule(string processId,string processName)
        {
            registryFilterRuleSection.Instances.Remove(processId + processName );

            return;
        }

        public static string GetFilePath()
        {
            return configPath;
        }

        public static bool Get(string name, bool value)
        {
            try
            {
                return bool.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static byte Get(string name, byte value)
        {
            try
            {
                return byte.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static sbyte Get(string name, sbyte value)
        {
            try
            {
                return sbyte.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static char Get(string name, char value)
        {
            try
            {
                return char.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static decimal Get(string name, decimal value)
        {
            try
            {
                return decimal.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static double Get(string name, double value)
        {
            try
            {
                return double.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }

        public static float Get(string name, float value)
        {
            try
            {
                return float.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static int Get(string name, int value)
        {
            try
            {
                return int.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }

        public static uint Get(string name, uint value)
        {
            try
            {
                return uint.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static long Get(string name, long value)
        {
            try
            {
                return long.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static ulong Get(string name, ulong value)
        {
            try
            {
                return ulong.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static short Get(string name, short value)
        {
            try
            {
                return short.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }


        public static ushort Get(string name, ushort value)
        {
            try
            {
                return ushort.Parse(config.AppSettings.Settings[name].Value);
            }
            catch
            {
                return value;
            }
        }

        public static string Get(string name, string value)
        {
            string str = string.Empty;
            try
            {
                str = config.AppSettings.Settings[name].Value;
            }
            catch
            {
                return value;
            }

            if (str == null)
                str = value;

            return str;
        }

        public static void Set(string name, string value)
        {
            try
            {
                config.AppSettings.Settings.Remove(name);
                config.AppSettings.Settings.Add(name, value);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

            }
            catch
            {
            }
        }

    }
}
