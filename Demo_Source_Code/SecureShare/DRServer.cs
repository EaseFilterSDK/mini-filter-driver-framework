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
using System.Text;
using System.IO;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading;
using System.Diagnostics;
using System.Management;
using System.Collections;
using System.Configuration;
using System.Windows.Forms;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace  SecureShare
{

     public enum AESFlags : uint
    {
        Flags_Enabled_Expire_Time = 0x00000010,
        Flags_Enabled_Check_ProcessName = 0x00000020,
        Flags_Enabled_Check_UserName = 0x00000040,
        Flags_Enabled_Check_AccessFlags = 0x00000080,
        Flags_Enabled_Check_User_Permit = 0x00000100,
        Flags_AES_Key_Was_Embedded = 0x00000200,
        Flags_Request_AccessFlags_From_User = 0x00000400,
        Flags_Request_IV_And_Key_From_User = 0x00000800, 
        Flags_Enabled_Check_Computer_Id = 0x00001000,
        Flags_Enabled_Check_User_Password = 0x00002000, 

    }

    /// <summary>
    /// this is the return data structure for encryption to filter driver.
    /// when the filter command is FILTER_REQUEST_ENCRYPTION_IV_AND_KEY,
    /// FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_ACCESSFLAG or FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct AESDataBuffer
    {
        public uint AccessFlags;
        public uint IVLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] IV;
        public uint EncryptionKeyLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] EncryptionKey;
        public uint TagDataLength;
        public byte[] TagData;
    } 

    /// <summary>
    /// This is the DR info meta data which will be stored in server if revoke access control is enabled.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class DRPolicy
    {
        /// <summary>
        /// If it is not empty, only the processes in the process list can access the files.
        /// </summary>
        public string AuthorizedProcessNames;
        /// <summary>
        /// If it is not empty, all the processes in the process list can not access the files.
        /// </summary>
        public string UnauthorizedProcessNames;
        /// <summary>
        /// If it is not empty, only the users in the user name list can access the files.
        /// </summary>
        public string AuthorizedUserNames;
        /// <summary>
        /// If it is not empty, all the useres in the user name list can not access the files.
        /// </summary>
        public string UnauthorizedUserNames;
        /// <summary>
        /// If it is not empty, only the computer in the computer id list can access the files.
        /// </summary>
        public string AuthorizedComputerIds;
        /// <summary>
        /// the password of the shared file.
        /// </summary>
        public string UserPassword;
        /// <summary>
        /// the access flags of the shared file.
        /// </summary>
        public uint AccessFlags;
        /// <summary>
        /// The file will be expired after the expire time in UTC format, and it can't be accessed.           
        /// </summary>
        public long ExpireTime;
        /// <summary>
        /// The time in UTC format of the encrypted file was created.
        /// </summary>
        public long CreationTime;
        /// <summary>
        /// the file name which was applied with policy.
        /// </summary>
        public string FileName;
        /// <summary>
        /// The encryption key hex string.
        /// </summary>
        public string EncryptionKey;
        /// <summary>
        /// the iv hex string.
        /// </summary>
        public string EncryptionIV;
    }    

    
    /// <summary>
    /// This the DR data which will be embedded to the encyrpted file    
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct DRPolicyData
    {

        public uint AESVerificationKey;
        public AESFlags AESFlags;
        public uint IVLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] IV;
        public uint EncryptionKeyLength;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
        public byte[] EncryptionKey;
        public long CreationTime;
        public long ExpireTime;
        public uint AccessFlags;
        public long FileSize;
        public uint LengthOfAuthorizedProcessNames;
        public uint LengthOfUnauthorizedProcessNames;
        public uint LengthOfAuthorizedUserNames;
        public uint LengthOfUnauthorizedUserNames;
        public uint LengthOfAccountName;
        public uint LengthOfComputerIds;
        public uint LengthOfUserPassword;
        public string AuthorizedProcessNames;
        public string UnauthorizedProcessNames;
        public string AuthorizedUserNames;
        public string UnauthorizedUserNames;
        public string AccountName;
        public string ComputerIds;
        public string UserPassword;
        public uint SizeOfAESData;

    }


    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public struct UserInfo
    {
        /// <summary>
        /// the owner account name
        /// </summary>
        public string AccountName;
        /// <summary>
        /// the password of the account
        /// </summary>
        public string AccountPassword;
        /// <summary>
        /// the process name which access the file.
        /// </summary>
        public string ProcessName;
        /// <summary>
        /// the user name who access the file.
        /// </summary>
        public string UserName;
        /// <summary>
        /// the computer information which access the file.
        /// </summary>
        public string ComputerId;
        /// <summary>
        /// the encrypted file which was accessed.
        /// </summary>
        public string FileName;
        /// <summary>
        /// the creation time of the file which was accessed.
        /// </summary>
        public long CreationTime;
        /// <summary>
        /// the password of the user input.
        /// </summary>
        public string UserPassword;
        /// the encryption iv hex string;
        /// </summary>
        public string EncryptionIV;
    }


    public class DigitalRightControl
    {
        public const uint AES_VERIFICATION_KEY = 0xccb76e80;
        public static string PassPhrase = "PassPhrase";

        private static byte[] ConvertDRPolicyDataToByteArray(DRPolicyData policy)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);
            bw.Write(AES_VERIFICATION_KEY);
            bw.Write((uint)(policy.AESFlags));
            bw.Write(policy.IVLength);
            bw.Write(policy.IV);
            bw.Write(policy.EncryptionKeyLength);
            bw.Write(policy.EncryptionKey);
            bw.Write(policy.CreationTime);
            bw.Write(policy.ExpireTime);
            bw.Write((uint)policy.AccessFlags);
            bw.Write(policy.FileSize);
            bw.Write(policy.LengthOfAuthorizedProcessNames);
            bw.Write(policy.LengthOfUnauthorizedProcessNames);
            bw.Write(policy.LengthOfAuthorizedUserNames);
            bw.Write(policy.LengthOfUnauthorizedUserNames);
            bw.Write(policy.LengthOfAccountName);
            bw.Write(policy.LengthOfComputerIds);
            bw.Write(policy.LengthOfUserPassword);

            byte[] strBuffer;
            if (policy.LengthOfAuthorizedProcessNames > 0)
            {
                strBuffer = UnicodeEncoding.Unicode.GetBytes(policy.AuthorizedProcessNames);
                bw.Write(strBuffer);
            }

            if (policy.LengthOfUnauthorizedProcessNames > 0)
            {
                strBuffer = UnicodeEncoding.Unicode.GetBytes(policy.UnauthorizedProcessNames);
                bw.Write(strBuffer);
            }

            if (policy.LengthOfAuthorizedUserNames > 0)
            {
                strBuffer = UnicodeEncoding.Unicode.GetBytes(policy.AuthorizedUserNames);
                bw.Write(strBuffer);
            }

            if (policy.LengthOfUnauthorizedUserNames > 0)
            {
                strBuffer = UnicodeEncoding.Unicode.GetBytes(policy.UnauthorizedUserNames);
                bw.Write(strBuffer);
            }

            if (policy.LengthOfAccountName > 0)
            {
                strBuffer = UnicodeEncoding.Unicode.GetBytes(policy.AccountName);
                bw.Write(strBuffer);
            }

            if (policy.LengthOfComputerIds > 0)
            {
                strBuffer = UnicodeEncoding.Unicode.GetBytes(policy.ComputerIds);
                bw.Write(strBuffer);
            }

            if (policy.LengthOfUserPassword > 0)
            {
                strBuffer = UnicodeEncoding.Unicode.GetBytes(policy.UserPassword);
                bw.Write(strBuffer);
            }

            //append the sizeof the DR policy
            int sizeofDRDataArray = (int)ms.Length + 4;// the size of sizeofDRDataArray takes 4 bytes memory as a int data
            bw.Write(sizeofDRDataArray);

            byte[] AESBuffer = ms.ToArray();

            return AESBuffer;
        }
    }


    public class CacheUserAccessInfo
    {
        public string index = string.Empty;
        public bool accessStatus = false;
        public bool isDownloaded = false;
        public uint accessFlags = 0;
        public DateTime lastAccessTime = DateTime.MinValue;
        public string iv = string.Empty;
        public string key = string.Empty;
        public AutoResetEvent syncEvent = new AutoResetEvent(true);
        public string lastError = string.Empty;

    }
   
    public class DRServer
    {

        static Dictionary<string, CacheUserAccessInfo> userAccessCache = new Dictionary<string, CacheUserAccessInfo>();
        static int cacheTimeOutInSeconds = 20;
        static System.Timers.Timer deleteCachedItemTimer = new System.Timers.Timer();

        static DRServer()
        {
            deleteCachedItemTimer.Interval = cacheTimeOutInSeconds * 1000 ; //millisecond
            deleteCachedItemTimer.Start();
            deleteCachedItemTimer.Enabled = true;
            deleteCachedItemTimer.Elapsed += new System.Timers.ElapsedEventHandler(deleteCachedItemTimer_Elapsed);
        }

        static public bool GetFileAccessPermission(EncryptEventArgs encryptEventtArgs)
        {
            Boolean retVal = true;
            string fileName = encryptEventtArgs.FileName;
            string lastError = string.Empty;
            string processName = encryptEventtArgs.ProcessName;
            string userName = encryptEventtArgs.UserName;
            string encryptKey = string.Empty;

            try
            {

                if (null == encryptEventtArgs.EncryptionTag || encryptEventtArgs.EncryptionTag.Length == 0)
                {
                    encryptEventtArgs.Description = "There are no encryption tag data.";

                    return false;
                }

                //by default the tag data format is "accountName;ivStr"
                string tagStr = UnicodeEncoding.Unicode.GetString(encryptEventtArgs.EncryptionTag);

                int index = tagStr.IndexOf(";");
                byte[] iv = encryptEventtArgs.EncryptionTag;

                if (index > 0)
                {
                    string serverAccount = tagStr.Substring(0, index);
                    string ivStr = tagStr.Substring(index + 1);
                    iv = Utils.ConvertHexStrToByteArray(ivStr);
                }

                uint accessFlag = 0;
                
                retVal = IsFileAccessAuthorized(fileName,
                                                userName,
                                                processName,
                                                tagStr,
                                                ref encryptKey,
                                                ref accessFlag,
                                                ref lastError);

                if (retVal && !string.IsNullOrEmpty(encryptKey))
                {
                    byte[] keyArray = Utils.ConvertHexStrToByteArray(encryptKey);

                    encryptEventtArgs.AccessFlags = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
                    encryptEventtArgs.IV = iv;
                    encryptEventtArgs.EncryptionKey = keyArray;
                    encryptEventtArgs.ReturnStatus = NtStatus.Status.Success;

                }
                else
                {
                    encryptEventtArgs.IoStatus = encryptEventtArgs.ReturnStatus = NtStatus.Status.AccessDenied;
                    encryptEventtArgs.Description = lastError;

                }
            }
            catch (Exception ex)
            {
                lastError = "GetFileAccessPermission exception." + ex.Message;
                encryptEventtArgs.Description = lastError;

                EventManager.WriteMessage(340, "GetFileAccessPermission", EventLevel.Error, lastError);
                retVal = false;
            }

            return retVal;

        }
      
      
        static private bool IsFileAccessAuthorized(string fileName,
                                          string userName,
                                          string processName,
                                          string tagStr,
                                          ref string encryptKey,
                                          ref uint accessFlag,
                                          ref string lastError)
        {
            Boolean retVal = true;
            string expireTime = string.Empty;
            string authorizedProcessNames = string.Empty;
            string unauthorizedProcessNames = string.Empty;
            string authorizedUserNames = string.Empty;
            string unauthorizedUserNames = string.Empty;
            string accessFlags = string.Empty;

            lastError = string.Empty;
            DateTime currentTime = DateTime.Now;

            try
            {

                string drFilePath = GlobalConfig.DRInfoFolder + "\\" + tagStr + ".xml";
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                if (!File.Exists(drFilePath))
                {
                    lastError = "The meta data file " + drFilePath + " doesn't exist.";
                    return false;
                }

                Utils.LoadAppSetting(drFilePath, ref keyValues);

                encryptKey = string.Empty;
                keyValues.TryGetValue("key", out encryptKey);
                if (string.IsNullOrEmpty(encryptKey))
                {
                    lastError = "The encryption key is empty.";
                    return false;
                }

                keyValues.TryGetValue("accessFlags", out accessFlags);
                uint.TryParse(accessFlags, out accessFlag);
                keyValues.TryGetValue("expireTime", out expireTime);
                keyValues.TryGetValue("authorizedProcessNames", out authorizedProcessNames);
                keyValues.TryGetValue("unauthorizedProcessNames", out unauthorizedProcessNames);
                keyValues.TryGetValue("authorizedUserNames", out authorizedUserNames);
                keyValues.TryGetValue("unauthorizeUserNames", out unauthorizedUserNames);

                if (!string.IsNullOrEmpty(expireTime))
                {
                    DateTime exTime = DateTime.FromFileTime(long.Parse(expireTime));

                    if (currentTime > exTime)
                    {
                        lastError = "the file is expired,current time:" + currentTime.ToString("yyyy-MM-dd-HH-mm-ss") + ",expire time:" + exTime.ToString("yyyy-MM-dd-HH-mm-ss");
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(authorizedProcessNames))
                {
                    if (authorizedProcessNames.IndexOf(processName.ToLower().Trim()) < 0)
                    {
                        lastError = "the process " + processName + " is not in authorized process list.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(unauthorizedProcessNames))
                {
                    if (unauthorizedProcessNames.IndexOf(processName.ToLower().Trim()) >= 0)
                    {
                        lastError = "the process " + processName + " is in unauthorized process list.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(authorizedUserNames))
                {
                    if (authorizedUserNames.IndexOf(userName.ToLower().Trim()) < 0)
                    {
                        lastError = "the user " + userName + " is not in authorized user list.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(unauthorizedUserNames))
                {
                    if (unauthorizedUserNames.IndexOf(userName.ToLower().Trim()) >= 0)
                    {
                        lastError = "the user " + userName + " is in unauthorized user list.";
                        return false;
                    }
                }


            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(286, "IsFileAccessAuthorized", EventLevel.Error, "Get file " + fileName + "permission failed with exception:" + ex.Message);
                retVal = false;
            }

            return retVal;

        }

        static public bool AddDRInfoToFile(string fileName,
                                    string authorizedProcessNames,
                                    string unauthorizedProcessNames,
                                    string authorizedUserNames,
                                    string unauthorizedUserNames,
                                    DateTime expireTime,
                                    byte[] encryptIV,
                                    byte[] encryptKey,
                                    string accessFlags)
        {

            try
            {
                if (!Directory.Exists(GlobalConfig.DRInfoFolder))
                {
                    Directory.CreateDirectory(GlobalConfig.DRInfoFolder);
                }             

                string iv = Utils.ByteArrayToHexStr(encryptIV);
                string key = Utils.ByteArrayToHexStr(encryptKey);

                string drFilePath = GlobalConfig.DRInfoFolder + "\\" + iv + ".xml";
                Dictionary<string, string> keyValues = new Dictionary<string, string>();

                keyValues.Add("fileName", fileName);
                keyValues.Add("key", key);
                keyValues.Add("iv", iv);
                keyValues.Add("accessFlags", accessFlags);
                keyValues.Add("authorizedProcessNames", authorizedProcessNames.ToLower());
                keyValues.Add("unauthorizedProcessNames", unauthorizedProcessNames.ToLower());
                keyValues.Add("authorizedUserNames", authorizedUserNames.ToLower());
                keyValues.Add("unauthorizedUserNames", unauthorizedUserNames.ToLower());
                keyValues.Add("expireTime", expireTime.ToFileTime().ToString());
                keyValues.Add("creationTime", DateTime.Now.ToFileTime().ToString());

                Utils.SaveAppSetting(drFilePath, keyValues);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        static private void deleteCachedItemTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            try
            {
                List<string> keysToRemove = new List<string>();

                foreach (KeyValuePair<string, CacheUserAccessInfo> userItem in userAccessCache)
                {

                    TimeSpan tsSinceLastAccess = DateTime.Now - userItem.Value.lastAccessTime;

                    if (tsSinceLastAccess.TotalSeconds >= cacheTimeOutInSeconds)
                    {
                        keysToRemove.Add(userItem.Key);
                    }
                }

                foreach (string key in keysToRemove)
                {
                    lock (userAccessCache)
                    {
                        userAccessCache.Remove(key);

                        EventManager.WriteMessage(573, "deleteCachedItemTimer_Elapsed", EventLevel.Verbose, "Delete cached item " + key);
                    }
                }
            }
            catch (System.Exception ex)
            {
                EventManager.WriteMessage(46, "deleteCachedItemTimer_Elapsed", EventLevel.Error, "Delete cached item failed with error:" + ex.Message);
            }

        }

    }
}
