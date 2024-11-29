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
using System.Linq;
using System.Text;
using System.Globalization;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using System.Text.RegularExpressions;
using System.Configuration;

namespace EaseFilter.CommonObjects
{

    public class Utils
    {
        public static uint WinMajorVersion()
        {
            dynamic major;
            // The 'CurrentMajorVersionNumber' string value in the CurrentVersion key is new for Windows 10, 
            // and will most likely (hopefully) be there for some time before MS decides to change this - again...
            if (TryGeRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMajorVersionNumber", out major))
            {
                return (uint)major;
            }

            // When the 'CurrentMajorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
            dynamic version;
            if (!TryGeRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", out version))
                return 0;

            var versionParts = ((string)version).Split('.');
            if (versionParts.Length != 2) return 0;

            uint majorAsUInt;
            return uint.TryParse(versionParts[0], out majorAsUInt) ? majorAsUInt : 0;
        }

        /// <summary>
        ///     Returns the Windows minor version number for this computer.
        /// </summary>
        public static uint WinMinorVersion()
        {
            dynamic minor;
            // The 'CurrentMinorVersionNumber' string value in the CurrentVersion key is new for Windows 10, 
            // and will most likely (hopefully) be there for some time before MS decides to change this - again...
            if (TryGeRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMinorVersionNumber",
                out minor))
            {
                return (uint)minor;
            }

            // When the 'CurrentMinorVersionNumber' value is not present we fallback to reading the previous key used for this: 'CurrentVersion'
            dynamic version;
            if (!TryGeRegistryKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentVersion", out version))
                return 0;

            var versionParts = ((string)version).Split('.');
            if (versionParts.Length != 2) return 0;
            uint minorAsUInt;
            return uint.TryParse(versionParts[1], out minorAsUInt) ? minorAsUInt : 0;
        }


        private static bool TryGeRegistryKey(string path, string key, out dynamic value)
        {
            value = null;
            try
            {
                var rk = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(path);
                if (rk == null) return false;
                value = rk.GetValue(key);
                return value != null;
            }
            catch
            {
                return false;
            }
        }

        public static void CopyOSPlatformDependentFiles()
        {
            Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
            string localPath = Path.GetDirectoryName(assembly.Location);
            string targetName = Path.Combine(localPath, "FilterAPI.DLL");

            bool is64BitOperatingSystem = Environment.Is64BitOperatingSystem;
            uint winMajorVersion = WinMajorVersion();
            uint winMinorVersion = WinMinorVersion();

            string sourceFolder = localPath;

            try
            {

                if (is64BitOperatingSystem)
                {
                    if(     (winMajorVersion >= 10 )
                        ||  (winMajorVersion >= 6 && winMinorVersion >=3) )
                    {
                        sourceFolder = Path.Combine(localPath, "Bin\\Win10X64");
                    }
                    else
                    {
                        sourceFolder = Path.Combine(localPath, "Bin\\x64");
                    }
                }
                else
                {
                    if ((winMajorVersion >= 10)
                       || (winMajorVersion >= 6 && winMinorVersion >= 3))
                    {
                        sourceFolder = Path.Combine(localPath, "Bin\\Win10X86");
                    }
                    else
                    {
                        sourceFolder = Path.Combine(localPath, "Bin\\win32");
                    }
                }

                string sourceFile = Path.Combine(sourceFolder, "FilterAPI.DLL");

                //only copy files for x86 platform, by default for x64, the files were there already.

                bool skipCopy = false;
                if (File.Exists(targetName))
                {
                    FileInfo sourceFileInfo = new FileInfo(sourceFile);
                    FileInfo targetFileInfo = new FileInfo(targetName);

                    if (sourceFileInfo.LastWriteTime.ToFileTime() == targetFileInfo.LastWriteTime.ToFileTime())
                    {
                        skipCopy = true;
                    }
                }

                if (!skipCopy)
                {
                    File.Copy(sourceFile, targetName, true);
                }


                sourceFile = Path.Combine(sourceFolder, "EaseFlt.sys");
                targetName = Path.Combine(localPath, "EaseFlt.sys");


                skipCopy = false;
                if (File.Exists(targetName))
                {
                    FileInfo sourceFileInfo = new FileInfo(sourceFile);
                    FileInfo targetFileInfo = new FileInfo(targetName);

                    if (sourceFileInfo.LastWriteTime.ToFileTime() == targetFileInfo.LastWriteTime.ToFileTime())
                    {
                        skipCopy = true;
                    }
                }

                if (!skipCopy)
                {
                    File.Copy(sourceFile, targetName, true);
                }

            }
            catch (Exception ex)
            {
                string lastError = "Copy platform dependent files 'FilterAPI.DLL' and 'EaseFlt.sys' to folder " + localPath + " got exception:" + ex.Message;
                EventManager.WriteMessage(80, "CopyOSPlatformDependentFiles", EventLevel.Error, lastError);
            }
        }


        public static string ByteArrayToHexStr(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
            {
                hex.AppendFormat("{0:x2}", b);
            }

            return hex.ToString().ToUpper();
        }

        public static byte[] ConvertHexStrToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0)
            {
                throw new ArgumentException(String.Format("The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] HexAsBytes = new byte[hexString.Length / 2];
            for (int index = 0; index < HexAsBytes.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                HexAsBytes[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return HexAsBytes;
        }

        /// <summary>
        /// Generate 32 bytes key array by pass phrase string
        /// </summary>
        /// <param name="pwStr"></param>
        /// <returns></returns>
        public static byte[] GetKeyByPassPhrase(string pwStr,int keySize)
        {
            byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwStr);

            var rfckey = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            byte[] key = rfckey.GetBytes(keySize);

            return key;


        }

        /// <summary>
        /// Generate 16 bytes iv array by pass phrase string
        /// </summary>
        /// <param name="pwStr"></param>
        /// <returns></returns>
        public static byte[] GetIVByPassPhrase(string pwStr)
        {
            byte[] saltBytes = new byte[] { 0xa1, 0xa2, 0xa3, 0xa4, 0xa5, 0xa6, 0xa7, 0xa8 };
            byte[] passwordBytes = Encoding.UTF8.GetBytes(pwStr);

            var rfckey = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
            byte[] key = rfckey.GetBytes(16);

            return key;


        }

        public static byte[] GetRandomKey()
        {
            AesManaged aesManaged = new AesManaged();
            aesManaged.KeySize = 256;
            byte[] key = aesManaged.Key;

            return key;
        }

        public static byte[] GetRandomIV()
        {
            AesManaged aesManaged = new AesManaged();
            Guid guid = Guid.NewGuid();

            byte[] IV = guid.ToByteArray();

            return IV;
        }

        public static bool IsBase64String(string s)
        {
            s = s.Trim();

            if ((s.Length % 4 == 0) && Regex.IsMatch(s, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public static void SaveAppSetting(string fileName, Dictionary<string, string> keyValues)
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = fileName;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

            settings.Clear();

            foreach (KeyValuePair<string, string> item in keyValues)
            {
                settings.Add(item.Key, item.Value);
            }

            configuration.Save();
        }

        public static void LoadAppSetting(string fileName, ref Dictionary<string, string> keyValues)
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = fileName;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);
            KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;

            foreach (KeyValueConfigurationElement item in settings)
            {
                keyValues.Add(item.Key, item.Value);
            }

        }


        public static void ToDebugger(string message)
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(false);
            string caller = st.GetFrame(1).GetMethod().Name;
            System.Diagnostics.Debug.WriteLine(caller + " Time:" + DateTime.Now.ToLongTimeString() + ": " + message);
        }

     
    }
}
