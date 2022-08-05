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
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Reflection;
using System.Globalization;
using System.IO;
using System.Text;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace EaseFilter.FilterControl
{

    public class Utils
    {
        static Dictionary<string, string> userNameTable = new Dictionary<string, string>();
        static Dictionary<uint, string> processNameTable = new Dictionary<uint, string>();


        [DllImport("FilterAPI.dll", SetLastError = true)]
        private static extern bool CreateFileAPI(
             [MarshalAs(UnmanagedType.LPWStr)]string fileName,
              uint dwDesiredAccess,
              uint dwShareMode,
              uint dwCreationDisposition,
              uint dwFlagsAndAttributes,
              ref IntPtr fileHandle);

        public enum EncryptType
        {
            Decryption = 0,
            Encryption,
        }      

        public static string AESEncryptDecryptStr(string inStr, EncryptType encryptType)
        {

            if (string.IsNullOrEmpty(inStr))
            {
                return string.Empty;
            }

            byte[] inbuffer = null;

            if (encryptType == EncryptType.Encryption)
            {
                inbuffer = ASCIIEncoding.UTF8.GetBytes(inStr);
            }
            else if (encryptType == EncryptType.Decryption)
            {
                inbuffer = Convert.FromBase64String(inStr);
            }
            else
            {
                throw new Exception("Failed to encrypt decrypt string, the encryptType " + encryptType.ToString() + " doesn't know.");
            }

            byte[] outBuffer = new byte[inbuffer.Length];

            GCHandle gcHandleIn = GCHandle.Alloc(inbuffer, GCHandleType.Pinned);
            GCHandle gcHandleOut = GCHandle.Alloc(outBuffer, GCHandleType.Pinned);

            IntPtr inBufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(inbuffer, 0);
            IntPtr outBufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(outBuffer, 0);

            try
            {
                bool retVal = FilterAPI.AESEncryptDecryptBuffer(inBufferPtr, outBufferPtr, (uint)inbuffer.Length, 0, null, 0, null, 0);

                if (encryptType == EncryptType.Encryption)
                {
                    return Convert.ToBase64String(outBuffer);
                }
                else //if (encryptType == EncryptType.Decryption)
                {
                    return ASCIIEncoding.UTF8.GetString(outBuffer);
                }
            }
            finally
            {
                gcHandleIn.Free();
                gcHandleOut.Free();
            }

        }


        public static void AESEncryptDecryptBuffer(byte[] inbuffer, long offset, byte[] key, byte[] IV)
        {
            if (null == inbuffer || inbuffer.Length == 0)
            {
                throw new Exception("Failed to encrypt decrypt buffer, the input buffer can't be null");
            }

            GCHandle gcHandle = GCHandle.Alloc(inbuffer, GCHandleType.Pinned);

            try
            {
                IntPtr inBufferPtr = Marshal.UnsafeAddrOfPinnedArrayElement(inbuffer, 0);

                uint keyLength = 0;
                uint IVLength = 0;

                if (key != null)
                {
                    keyLength = (uint)key.Length;
                }

                if (IV != null)
                {
                    IVLength = (uint)IV.Length;
                }


                bool retVal = FilterAPI.AESEncryptDecryptBuffer(inBufferPtr, inBufferPtr, (uint)inbuffer.Length, offset, key, keyLength, IV, IVLength);

                if (!retVal)
                {
                    throw new Exception("Failed to encrypt buffer, return error:" + FilterAPI.GetLastErrorMessage());
                }
            }
            finally
            {
                gcHandle.Free();
            }

            return;
        }

        public static bool DecodeUserName(byte[] sid, out string userName)
        {
            bool ret = true;

            IntPtr sidStringPtr = IntPtr.Zero;
            string sidString = string.Empty;

            userName = string.Empty;

            try
            {

                IntPtr sidBuffer = Marshal.UnsafeAddrOfPinnedArrayElement(sid, 0);

                if (FilterAPI.ConvertSidToStringSid(sidBuffer, out sidStringPtr))
                {

                    sidString = Marshal.PtrToStringAuto(sidStringPtr);

                    lock (userNameTable)
                    {
                        //check the user name cache table
                        if (userNameTable.ContainsKey(sidString))
                        {
                            userName = userNameTable[sidString];
                            return ret;
                        }
                    }

                    try
                    {
                        SecurityIdentifier secIdentifier = new SecurityIdentifier(sidString);
                        IdentityReference reference = secIdentifier.Translate(typeof(NTAccount));
                        userName = reference.Value;
                    }
                    catch
                    {
                    }

                    lock (userNameTable)
                    {
                        //check the user name cache table
                        if (!userNameTable.ContainsKey(sidString))
                        {
                            userNameTable.Add(sidString, userName);
                        }
                    }
                }
                else
                {
                    string errorMessage = "Convert sid to sid string failed with error " + Marshal.GetLastWin32Error();
                    Console.WriteLine(errorMessage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Convert sid to user name got exception:{0}", ex.Message));
                ret = false;

            }
            finally
            {
                if (sidStringPtr != null && sidStringPtr != IntPtr.Zero)
                {
                    FilterAPI.LocalFree(sidStringPtr);
                }
            }

            return ret;
        }

        public static bool DecodeProcessName(uint processId, out string processName)
        {
            bool ret = true;
            processName = string.Empty;


            //this is the optimization of the process to get the process name from the process Id
            //it is not reliable for this process, since the process Id is reuasble when the process was ternmiated.
            lock (processNameTable)
            {
                if (processNameTable.ContainsKey(processId))
                {
                    processName = processNameTable[processId];
                    return true;
                }
            }

            try
            {
                System.Diagnostics.Process requestProcess = System.Diagnostics.Process.GetProcessById((int)processId);
                processName = requestProcess.ProcessName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("Convert pid to process name got exception:{0}", ex.Message));
                ret = false;
            }

            lock (processNameTable)
            {
                if (!processNameTable.ContainsKey(processId))
                {
                    processNameTable.Add(processId, processName);
                }
            }

            return ret;
        }

        public static bool IsDriverChanged()
        {
            bool ret = false;

            try
            {
                System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
                string localPath = Path.GetDirectoryName(assembly.Location);
                string driverName = Path.Combine(localPath, "EaseFlt.sys");

                if (File.Exists(driverName))
                {
                    string driverInstalledPath = Path.Combine(Environment.SystemDirectory, "drivers\\easeflt.sys");

                    if (File.Exists(driverInstalledPath))
                    {
                        FileInfo fsInstalled = new FileInfo(driverInstalledPath);
                        FileInfo fsToInstall = new FileInfo(driverName);

                        if (fsInstalled.LastWriteTime != fsToInstall.LastWriteTime)
                        {
                            return true;
                        }
                    }
                 
                }

            }
            catch
            {
                ret = false;
            }

            return ret;
        }

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

        public static bool CopyOSPlatformDependentFiles(ref string lastError)
        {
            bool retVal = true;

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
                retVal = false;
                lastError = "Copy platform dependent files 'FilterAPI.DLL' and 'EaseFlt.sys' to folder " + localPath + " got exception:" + ex.Message;
            }

            return retVal;
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

            if (0 == keySize)
            {
                keySize = 32;
            }

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


        public class ByteArrayComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] left, byte[] right)
            {
                if (left == null || right == null)
                {
                    return left == right;
                }
                if (left.Length != right.Length)
                {
                    return false;
                }
                for (int i = 0; i < left.Length; i++)
                {
                    if (left[i] != right[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            public int GetHashCode(byte[] key)
            {
                if (key == null)
                    throw new ArgumentNullException("key");
                int sum = 0;
                foreach (byte cur in key)
                {
                    sum += cur;
                }
                return sum;
            }
        }

        /// <summary>
        /// To open encrypted file without the filter driver interception, read the raw data with the return file handle.
        /// The caller is reponsible to close the file handle.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileHandle"></param>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public static bool OpenRawEnCyptedFile(string fileName, out IntPtr fileHandle, out string lastError)
        {
            fileHandle = IntPtr.Zero;
            lastError = string.Empty;
            uint bypassFilterFileAttributes = FilterAPI.FILE_FLAG_OPEN_REPARSE_POINT | FilterAPI.FILE_FLAG_OPEN_NO_RECALL | FilterAPI.FILE_FLAG_NO_BUFFERING | FilterAPI.FILE_ATTRIBUTE_REPARSE_POINT;

            try
            {
                if (!CreateFileAPI(fileName, (uint)FileAccess.Read, (uint)FileShare.None, (uint)FileMode.Open, bypassFilterFileAttributes, ref fileHandle))
                {
                    lastError = FilterAPI.GetLastErrorMessage();
                    return false;
                }
            }
            catch (Exception ex)
            {
                lastError = "OpenRawEnCyptedFile " + fileName + " got exception,system return error:" + ex.Message;
                return false;
            }

            return true;
        }

        public static T DecryptStrToObject<T>(string toDeserialize)
        {
            string decryptedStr = AESEncryptDecryptStr(toDeserialize, EncryptType.Decryption);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader textReader = new StringReader(decryptedStr);
            return (T)xmlSerializer.Deserialize(textReader);
        }

        public static string EncryptObjectToStr<T>(T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, toSerialize);

            string encryptedText = AESEncryptDecryptStr(textWriter.ToString(), EncryptType.Encryption);

            return encryptedText;
        }

        [System.Diagnostics.Contracts.Pure]
        public static string ByteArrayToHex(byte[] value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            const string hexAlphabet = @"0123456789ABCDEF";

            var chars = new char[checked(value.Length * 2)];
            unchecked
            {
                for (int i = 0; i < value.Length; i++)
                {
                    chars[i * 2] = hexAlphabet[value[i] >> 4];
                    chars[i * 2 + 1] = hexAlphabet[value[i] & 0xF];
                }
            }
            return new string(chars);
        }

        [System.Diagnostics.Contracts.Pure]
        public static byte[] HexToByteArray(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");
            if (value.Length % 2 != 0)
                throw new ArgumentException("Hexadecimal value length must be even.", "value");

            unchecked
            {
                byte[] result = new byte[value.Length / 2];
                for (int i = 0; i < result.Length; i++)
                {
                    // 0(48) - 9(57) -> 0 - 9
                    // A(65) - F(70) -> 10 - 15
                    int b = value[i * 2]; // High 4 bits.
                    int val = ((b - '0') + ((('9' - b) >> 31) & -7)) << 4;
                    b = value[i * 2 + 1]; // Low 4 bits.
                    val += (b - '0') + ((('9' - b) >> 31) & -7);
                    result[i] = checked((byte)val);
                }
                return result;
            }
        }

    }
}
