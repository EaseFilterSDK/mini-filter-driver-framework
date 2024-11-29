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
using System.IO;
using System.Collections.Generic;
using System.Threading;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace RegMon
{
    public class RegistryUnitTest
    {
        FilterControl filterControl = null;

        public  RichTextBox unitTestResult = new RichTextBox();
        public  bool postQueryValueKeyPassed = false;

        private void AppendUnitTestResult(string text, Color color)
        {
            if (color == Color.Black)
            {
                unitTestResult.AppendText(Environment.NewLine);
                unitTestResult.SelectionFont = new Font("Arial", 12, FontStyle.Bold);
            }

            unitTestResult.SelectionStart = unitTestResult.TextLength;
            unitTestResult.SelectionLength = 0;

            unitTestResult.SelectionColor = color;
            unitTestResult.AppendText(text + Environment.NewLine);
            unitTestResult.SelectionColor = unitTestResult.ForeColor;

            if (color == Color.Black)
            {
                unitTestResult.AppendText(Environment.NewLine);
            }


        }

        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public void NotifyQueryValueKey(object sender, RegistryEventArgs e)
        {
            if (e.FileName.IndexOf("EaseFilter") > 0)
            {
                //this is our test key.
                postQueryValueKeyPassed = true;
            }
        }

        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public  void OnPreCreateKey(object sender, RegistryEventArgs e)
        {
            //   //test block the registry event.
            if (e.FileName.IndexOf("EaseFilter") > 0)
            {
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }
        }

          
        /// <summary>
        /// Fires this event when the register registry event was triggered.
        /// </summary>
        public  void OnPreQueryValueKey(object sender, RegistryEventArgs e)
        {

            //   //test block the registry event.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            KEY_VALUE_INFORMATION_CLASS keyValuseInformationClass = (KEY_VALUE_INFORMATION_CLASS)e.InfoClass;

            //test to replace the query value with your own data.
            if (e.FileName.IndexOf("EaseFilter") > 0)
            {

                //below code is to demo how to complete pre-callback registry call with our own data.
                switch (keyValuseInformationClass)
                {
                    case KEY_VALUE_INFORMATION_CLASS.KeyValueBasicInformation:
                        {
                            //public struct KEY_VALUE_BASIC_INFORMATION
                            // {
                            //     public uint TitleIndex;
                            //     public uint Type;
                            //     public uint NameLength;
                            //     public byte[] Name;
                            // }

                            uint titleIndex = 0;
                            uint type = (uint)VALUE_DATA_TYPE.REG_SZ;
                            byte[] valueName = Encoding.Unicode.GetBytes("value1");
                            uint valueNameLength = (uint)valueName.Length;

                            MemoryStream ms = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(ms);
                            bw.Write(titleIndex);
                            bw.Write(type);
                            bw.Write(valueNameLength);
                            bw.Write(valueName);

                            e.ReturnDataBuffer = ms.ToArray();
                            e.IsDataModified = true;

                            break;
                        }
                    case KEY_VALUE_INFORMATION_CLASS.KeyValueFullInformation:
                        {
                            //KeyValueFullInformation class structure
                            //public uint TitleIndex;
                            //public uint Type;
                            //public uint DataOffset;
                            //public uint DataLength;
                            //public uint NameLength;
                            //public byte[] Name;

                            uint titleIndex = 0;
                            //uint type = (uint)VALUE_DATA_TYPE.REG_DWORD;
                            //uint testData = 12345;

                            uint type = (uint)VALUE_DATA_TYPE.REG_SZ;
                            byte[] testData = UnicodeEncoding.Unicode.GetBytes("S12345");

                            uint testDataLength = sizeof(uint);
                            byte[] valueName = Encoding.Unicode.GetBytes("value1");
                            uint valueNameLength = (uint)valueName.Length;
                            uint dataOffset = 5 * sizeof(uint) + valueNameLength;

                            MemoryStream ms = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(ms);
                            bw.Write(titleIndex);
                            bw.Write(type);
                            bw.Write(dataOffset);
                            bw.Write(testDataLength);
                            bw.Write(valueNameLength);
                            bw.Write(valueName);
                            bw.Write(testData);

                            e.ReturnDataBuffer = ms.ToArray();
                            e.IsDataModified = true;

                            break;
                        }
                    case KEY_VALUE_INFORMATION_CLASS.KeyValuePartialInformation:
                        {
                            // public struct KEY_VALUE_PARTIAL_INFORMATION
                            //{
                            //    public uint TitleIndex;
                            //    public uint Type;
                            //    public uint DataLength;
                            //    public byte[] Data;
                            //}

                            uint titleIndex = 0;
                            uint type = (uint)VALUE_DATA_TYPE.REG_SZ;
                            //uint testData = 12345;
                            byte[] testData = UnicodeEncoding.Unicode.GetBytes("S12345");
                            //uint testDataLength = (uint)sizeof(uint);
                            uint testDataLength = (uint)testData.Length;

                            MemoryStream ms = new MemoryStream();
                            BinaryWriter bw = new BinaryWriter(ms);

                            bw.Write(titleIndex);
                            bw.Write(type);
                            bw.Write(testDataLength);
                            bw.Write(testData);

                            e.ReturnDataBuffer = ms.ToArray();
                            e.IsDataModified = true;

                            break;
                        }


                    default: break;
                }
            }
        }

        public  void RegistryFilterUnitTest(FilterControl _filterControl, RichTextBox richTextBox_TestResult, string licenseKey)
        {

            string lastError = string.Empty;
            string userName = Environment.UserDomainName + "\\" + Environment.UserName;
            string testRegistryKey = "SYSTEM\\CurrentControlSet\\Services\\EaseFilter";
            string testValueKey = "DisplayName";

            //full registry access rights
            uint accessFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;

            bool testPassed = true;

            unitTestResult = richTextBox_TestResult;

            try
            {
                string message = "Registry Filter Driver Unit Test.";
                AppendUnitTestResult(message, Color.Black);

                filterControl = _filterControl;

                if (!filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError))
                {
                    MessageBox.Show(lastError, "StartFilter", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Thread.Sleep(3000);

                
                //registry access flag test,set full registry access rights for current process.
                
                RegistryFilter regFilter = new RegistryFilter();
                regFilter.ControlFlag = accessFlag;
                regFilter.RegCallbackClass = 0;
                regFilter.ProcessId = FilterAPI.GetCurrentProcessId();

                filterControl.AddFilter(regFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    AppendUnitTestResult("SendConfigSettingsToFilter failed:" + lastError, Color.Red);
                    return;
                }

                using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(testRegistryKey))
                {
                    string valueName = (string)regkey.GetValue(testValueKey);
                    AppendUnitTestResult("1.Test full registry access rights in accessFlag passed, return valueName:" + valueName, Color.Gray);
                }
            }
            catch (Exception ex)
            {
                AppendUnitTestResult("1.Test registry access flag failed with error:" + ex.Message, Color.Red);
                testPassed = false;
            }

            if (testPassed)
            {
                //disable registry open key right test
                filterControl.ClearFilters();
                RegistryFilter regFilter = new RegistryFilter();
                regFilter.ControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG & (uint)(~FilterAPI.RegControlFlag.REG_ALLOW_OPEN_KEY); ;
                regFilter.RegCallbackClass = 0;
                regFilter.ProcessId = FilterAPI.GetCurrentProcessId();

                filterControl.AddFilter(regFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    AppendUnitTestResult("SendConfigSettingsToFilter failed:" + lastError, Color.Red);
                    return;
                }

                try
                {
                    using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(testRegistryKey))
                    {
                        string valueName = (string)regkey.GetValue(testValueKey);
                        AppendUnitTestResult("2.Test disable open registry key right in accessFlag failed, get valueName " + valueName + " succedded.", Color.Red);
                    }
                }
                catch
                {
                    AppendUnitTestResult("2.Test disable open registry key right in accessFlag passed.", Color.Green);
                }
            }

            try
            {

                //test query value key callback, we will receive the callback registry query value key.
                filterControl.ClearFilters();
                RegistryFilter regFilter = new RegistryFilter();
                regFilter.ControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;
                regFilter.RegCallbackClass = (uint)FilterAPI.RegCallbackClass.Reg_Post_Query_Value_Key;
                regFilter.NotifyQueryValueKey += NotifyQueryValueKey;
                regFilter.ProcessId = FilterAPI.GetCurrentProcessId();

                filterControl.AddFilter(regFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    AppendUnitTestResult("SendConfigSettingsToFilter failed:" + lastError, Color.Red);
                    return;
                }

                using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(testRegistryKey))
                {
                    string valueName = (string)regkey.GetValue(testValueKey);

                    Thread.Sleep(3000);

                    if (postQueryValueKeyPassed)
                    {
                        AppendUnitTestResult("3.Test registry query value key Reg_Post_Query_Value_Key callback passed, return valueName:" + valueName, Color.Green);
                    }
                    else
                    {
                        AppendUnitTestResult("3.Test registry query value key Reg_Post_Query_Value_Key callback failed, didn't receive callback message.", Color.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                AppendUnitTestResult("3.Test registry query value key Reg_Post_Query_Value_Key callback failed:" + ex.Message, Color.Red);
            }


            //test registry access callback control, in callback function we will block the setting of the value name if it succeeds.

            try
            {
                filterControl.ClearFilters();
                RegistryFilter regFilter = new RegistryFilter();
                regFilter.ControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;
                regFilter.RegCallbackClass = (uint)FilterAPI.RegCallbackClass.Reg_Pre_Create_Key | (uint)FilterAPI.RegCallbackClass.Reg_Pre_Create_KeyEx;
                regFilter.OnPreCreateKey += OnPreCreateKey;
                regFilter.OnPreCreateKeyEx += OnPreCreateKey;

                regFilter.ProcessId = FilterAPI.GetCurrentProcessId();

                filterControl.AddFilter(regFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    AppendUnitTestResult("SendConfigSettingsToFilter failed:" + lastError, Color.Red);
                    return;
                }

                using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(testRegistryKey))
                {
                    regkey.CreateSubKey("newSubkey");
                    AppendUnitTestResult("4.Test block creating new registry sub key in callback failed, the new subkey was created in callback.", Color.Red);
                }
            }
            catch (Exception ex)
            {
                AppendUnitTestResult("4.Test block creating new registry sub key in callback passed." + ex.Message, Color.Green);
            }

            //test registry access callback control, in callback function we will replace our virutal value back to user if it succeeds.
            try
            {
                filterControl.ClearFilters();
                RegistryFilter regFilter = new RegistryFilter();
                regFilter.ControlFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;
                regFilter.RegCallbackClass = (uint)FilterAPI.RegCallbackClass.Reg_Pre_Query_Value_Key ;
                regFilter.OnPreQueryValueKey += OnPreQueryValueKey;

                regFilter.ProcessId = FilterAPI.GetCurrentProcessId();

                filterControl.AddFilter(regFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    AppendUnitTestResult("SendConfigSettingsToFilter failed:" + lastError, Color.Red);
                    return;
                }

                using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(testRegistryKey))
                {
                    object value = regkey.GetValue("value1");
                    AppendUnitTestResult("5.Test modify registry return data in callback passed, return value " + value + ",type:" + value.GetType().ToString(), Color.Green);
                }
            }
            catch (Exception ex)
            {
                AppendUnitTestResult("5.Test modify registry return data in callback failed: " + ex.Message, Color.Red);
            }
        }
    }
}
