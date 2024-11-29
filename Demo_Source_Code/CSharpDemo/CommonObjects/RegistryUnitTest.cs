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

namespace EaseFilter.CommonObjects
{
    static public class RegistryUnitTest
    {
        public static RichTextBox unitTestResult = new RichTextBox();
        public static bool postQueryValueKeyPassed = false;

        static private void AppendUnitTestResult(string text, Color color)
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

        public static void RegistryFilterUnitTest(RichTextBox richTextBox_TestResult)
        {

            string lastError = string.Empty;
            string userName = Environment.UserDomainName + "\\" + Environment.UserName;
            string testRegistryKey = "SYSTEM\\CurrentControlSet\\Services\\EaseFilter";
            string testValueKey = "DisplayName";

            //full registry access rights
            uint accessFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;
            ulong regCallbackClass = 0;

            bool testPassed = true;

            unitTestResult = richTextBox_TestResult;

            try
            {
                string message = "Registry Filter Driver Unit Test.";
                AppendUnitTestResult(message, Color.Black);

                //
                //registry access flag test,set full registry access rights for current process.
                //
                if (!FilterAPI.AddRegFilterEntryById(FilterAPI.GetCurrentProcessId(), accessFlag, regCallbackClass, false))
                {
                    AppendUnitTestResult("AddRegFilterEntryById failed:" + FilterAPI.GetLastErrorMessage(), Color.Red);
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
                accessFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG & (uint)(~FilterAPI.RegControlFlag.REG_ALLOW_OPEN_KEY);

                try
                {
                    if (!FilterAPI.AddRegFilterEntryById(FilterAPI.GetCurrentProcessId(), accessFlag, regCallbackClass, false))
                    {
                        AppendUnitTestResult("AddRegFilterEntryById failed:" + FilterAPI.GetLastErrorMessage(), Color.Red);
                        return;
                    }

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

            //test query value key callback, we will receive the callback registry query value key.
            accessFlag = FilterAPI.MAX_REGITRY_ACCESS_FLAG;
            regCallbackClass = (uint)FilterAPI.RegCallbackClass.Reg_Post_Query_Value_Key;

            try
            {
                if (!FilterAPI.AddRegFilterEntryById(FilterAPI.GetCurrentProcessId(), accessFlag, regCallbackClass, false))
                {
                    AppendUnitTestResult("AddRegFilterEntryById failed:" + FilterAPI.GetLastErrorMessage(), Color.Red);
                    return;
                }

                using (Microsoft.Win32.RegistryKey regkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(testRegistryKey))
                {
                    string valueName = (string)regkey.GetValue(testValueKey);
                    Thread.Sleep(2000);

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
            catch( Exception ex)
            {
                AppendUnitTestResult("3.Test registry query value key Reg_Post_Query_Value_Key callback failed:" + ex.Message, Color.Red);
            }
           

            //test registry access callback control, in callback function we will block the setting of the value name if it succeeds.
            regCallbackClass = (uint)FilterAPI.RegCallbackClass.Reg_Pre_Create_Key | (uint)FilterAPI.RegCallbackClass.Reg_Pre_Create_KeyEx;

            try
            {
                if (!FilterAPI.AddRegFilterEntryById(FilterAPI.GetCurrentProcessId(), accessFlag, regCallbackClass, false))
                {
                    AppendUnitTestResult("AddRegFilterEntryById failed:" + FilterAPI.GetLastErrorMessage(), Color.Red);
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
            regCallbackClass = (uint)FilterAPI.RegCallbackClass.Reg_Pre_Query_Value_Key;

            try
            {
                if (!FilterAPI.AddRegFilterEntryById(FilterAPI.GetCurrentProcessId(), accessFlag, regCallbackClass, false))
                {
                    AppendUnitTestResult("AddRegFilterEntryById failed:" + FilterAPI.GetLastErrorMessage(), Color.Red);
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
