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

        public static Dictionary<string, FileFilter> GetFileFilters()
        {
            Dictionary<string, FileFilter> fileFilters = new Dictionary<string, FileFilter>();

            foreach (FileFilterRule filterRule in filterRuleSection.Instances)
            {
                fileFilters.Add(filterRule.IncludeFileFilterMask, filterRule.ToFileFilter());
            }

            return fileFilters;
        }

        public static void AddFileFilter(FileFilter fileFilter)
        {
            filterRuleSection.Instances.Remove(fileFilter.IncludeFileFilterMask);
            filterRuleSection.Instances.Add(fileFilter);

            return ;
        }

        public static void RemoveFileFilter(string includeFilterMask)
        {
            filterRuleSection.Instances.Remove(includeFilterMask);
            FilterAPI.RemoveFilterRule(includeFilterMask);

            return;
        }

        public static Dictionary<string, ProcessFilter> GetProcessFilters()
        {
            Dictionary<string, ProcessFilter> processFilters = new Dictionary<string, ProcessFilter>();

            foreach (ProcessFilterRule processFilterRule in processFilterRuleSection.Instances)
            {
                processFilters.Add(processFilterRule.ProcessNameFilterMask + processFilterRule.ProcessId, processFilterRule.ToProcessFilter());
            }

            return processFilters;
        }

        public static void AddProcessFilter(ProcessFilter processFilter)
        {
            processFilterRuleSection.Instances.Remove(processFilter.ProcessNameFilterMask + processFilter.ProcessId);
            processFilterRuleSection.Instances.Add(processFilter);

            return;
        }

        public static void RemoveProcessFilter(ProcessFilter processFilter)
        {
            processFilterRuleSection.Instances.Remove(processFilter);

            if (processFilter.ProcessNameFilterMask.Length > 0)
            {
                FilterAPI.RemoveProcessFilterRule((uint)processFilter.ProcessNameFilterMask.Length * 2, processFilter.ProcessNameFilterMask);
            }

            return;
        }

        public static Dictionary<string, RegistryFilter> GetRegistryFilters()
        {
            Dictionary<string, RegistryFilter> registryFilters = new Dictionary<string, RegistryFilter>();

            foreach (RegistryFilterRule registryFilterRule in registryFilterRuleSection.Instances)
            {
                registryFilters.Add(registryFilterRule.ProcessId + registryFilterRule.ProcessNameFilterMask, registryFilterRule.ToRegistryFilter());
            }

            return registryFilters;
        }

        public static void AddRegistryFilter(RegistryFilter registryFilter)
        {
            registryFilterRuleSection.Instances.Remove(registryFilter);
            registryFilterRuleSection.Instances.Add(registryFilter);

            return;
        }

        public static void RemoveRegistryFilter(RegistryFilter registryFilter)
        {
            registryFilterRuleSection.Instances.Remove(registryFilter);

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
                bool result = value;
                if( bool.TryParse(config.AppSettings.Settings[name].Value,out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                byte result = value;
                if (byte.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                sbyte result = value;
                if (sbyte.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                char result = value;
                if (char.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                decimal result = value;
                if (decimal.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                double result = value;
                if (double.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                float result = value;
                if (float.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                int result = value;
                if (int.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                uint result = value;
                if (uint.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                long result = value;
                if (long.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                ulong result = value;
                if (ulong.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                short result = value;
                if (short.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
                ushort result = value;
                if (ushort.TryParse(config.AppSettings.Settings[name].Value, out result))
                {
                    return result;
                }
                else
                {
                    return value;
                }
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
