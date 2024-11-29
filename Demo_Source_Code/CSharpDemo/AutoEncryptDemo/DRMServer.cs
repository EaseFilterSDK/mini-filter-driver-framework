using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace AutoEncryptDemo
{
    /// <summary>
    /// This is the DRM information to control the encrypted file access.
    /// </summary>
    public class DRMInfo
    {
        /// <summary>
        /// If it is not empty, only the processes in the process list can access the files.
        /// </summary>
        public string AuthorizedProcessNames = string.Empty;
        /// <summary>
        /// If it is not empty, all the processes in the process list can not access the files.
        /// </summary>
        public string UnauthorizedProcessNames = string.Empty;
        /// <summary>
        /// If it is not empty, only the users in the user name list can access the files.
        /// </summary>
        public string AuthorizedUserNames = string.Empty;
        /// <summary>
        /// If it is not empty, all the useres in the user name list can not access the files.
        /// </summary>
        public string UnauthorizedUserNames = string.Empty;
        /// <summary>
        /// It is an account name information.
        /// </summary>
        public string AccountName = string.Empty;
        /// <summary>
        /// If it is not empty, only the computer in the computer id list can access the files.
        /// </summary>
        public string AuthorizedComputerIds = string.Empty;
        /// <summary>
        /// The password of the user account.
        /// </summary>
        public string UserPassword = string.Empty;
        /// <summary>
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
        public string FileName = string.Empty;
        /// <summary>
        /// The encryption key hex string.
        /// </summary>
        public string EncryptionKey = string.Empty;
        /// <summary>
        /// the iv hex string.
        /// </summary>
        public string EncryptionIV = string.Empty;
        /// <summary>
        /// The AES control flag
        /// </summary>
        public AESFlags AESFlags = 0;
    }

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
    /// This is the build in DRM data format which can be embedded into the encrypted file as a header.
    /// </summary>
    public class DRMData
    {
        public uint AESVerificationKey;
        public AESFlags AESFlags;
        public long CreationTime;
        public long ExpireTime;
        public uint AccessFlags;
        public uint LengthOfIncludeProcessNames;
        public uint OffsetOfIncludeProcessNames;
        public uint LengthOfExcludeProcessNames;
        public uint OffsetOfExcludeProcessNames;
        public uint LengthOfIncludeUserNames;
        public uint OffsetOfIncludeUserNames;
        public uint LengthOfExcludeUserNames;
        public uint OffsetOfExcludeUserNames;
        public uint LengthOfAccountName;
        public uint OffsetOfAccountName;
        public uint LengthOfComputerId;
        public uint OffsetOfComputerId;
        public uint LengthOfUserPassword;
        public uint OffsetOfUserPassword;
        public string AuthorizedProcessNames = string.Empty;
        public string UnauthorizedProcessNames = string.Empty;
        public string AuthorizedUserNames = string.Empty;
        public string UnauthorizedUserNames = string.Empty;
        public string AccountName = string.Empty;
        public string ComputerIds = string.Empty;
        public string UserPassword = string.Empty;

    }

    public class DRMServer
    {
        /// <summary>
        /// if it is true, the DRM data will be embedded to the header of the encrypted file.
        /// if the encrypted file was opened, the filter driver will check if the access was authorized by the embedded data 
        /// if it is not authorized, the encrypted file access will be blocked, or it will go to user mode service to get the encryption key.
        /// </summary>
        public static bool embedDRMToFile = true;
        /// <summary>
        /// The defined DRM data, you can store the DRM data to your own remote server to control the encrypted file.
        /// </summary>
        public static DRMInfo dRMInfo = null;

        static DRMData dRMData = new DRMData();

        //define for build in DRM data verification code.
        const uint AES_VERIFICATION_KEY = 0xccb76e80;

        public static string GetComputerId()
        {

            string myComputerId = FilterAPI.GetComputerId().ToString();

            if (string.IsNullOrEmpty(myComputerId.Trim()))
            {
                string lastError = FilterAPI.GetLastErrorMessage();
                return "";
            }

            return myComputerId;

        }

        private static byte[] ConvertDRMDataToByteArray(DRMData dRMData)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                BinaryWriter bw = new BinaryWriter(ms);
                bw.Write(AES_VERIFICATION_KEY);
                bw.Write((uint)(dRMData.AESFlags));
                bw.Write(dRMData.CreationTime);
                bw.Write(dRMData.ExpireTime);
                bw.Write(dRMData.AccessFlags);
                bw.Write(dRMData.LengthOfIncludeProcessNames);
                bw.Write(dRMData.OffsetOfIncludeProcessNames);
                bw.Write(dRMData.LengthOfExcludeProcessNames);
                bw.Write(dRMData.OffsetOfExcludeProcessNames);
                bw.Write(dRMData.LengthOfIncludeUserNames);
                bw.Write(dRMData.OffsetOfIncludeUserNames);
                bw.Write(dRMData.LengthOfExcludeUserNames);
                bw.Write(dRMData.OffsetOfExcludeUserNames);
                bw.Write(dRMData.LengthOfAccountName);
                bw.Write(dRMData.OffsetOfAccountName);
                bw.Write(dRMData.LengthOfComputerId);
                bw.Write(dRMData.OffsetOfComputerId);
                bw.Write(dRMData.LengthOfUserPassword);
                bw.Write(dRMData.OffsetOfUserPassword);

                byte[] strBuffer;
                if (dRMData.LengthOfIncludeProcessNames > 0)
                {
                    strBuffer = UnicodeEncoding.Unicode.GetBytes(dRMData.AuthorizedProcessNames);
                    bw.Write(strBuffer);
                }

                if (dRMData.LengthOfExcludeProcessNames > 0)
                {
                    strBuffer = UnicodeEncoding.Unicode.GetBytes(dRMData.UnauthorizedProcessNames);
                    bw.Write(strBuffer);
                }

                if (dRMData.LengthOfIncludeUserNames > 0)
                {
                    strBuffer = UnicodeEncoding.Unicode.GetBytes(dRMData.AuthorizedUserNames);
                    bw.Write(strBuffer);
                }

                if (dRMData.LengthOfExcludeUserNames > 0)
                {
                    strBuffer = UnicodeEncoding.Unicode.GetBytes(dRMData.UnauthorizedUserNames);
                    bw.Write(strBuffer);
                }

                if (dRMData.LengthOfAccountName > 0)
                {
                    strBuffer = UnicodeEncoding.Unicode.GetBytes(dRMData.AccountName);
                    bw.Write(strBuffer);
                }

                if (dRMData.LengthOfComputerId > 0)
                {
                    strBuffer = UnicodeEncoding.Unicode.GetBytes(dRMData.ComputerIds);
                    bw.Write(strBuffer);
                }

                if (dRMData.LengthOfUserPassword > 0)
                {
                    strBuffer = UnicodeEncoding.Unicode.GetBytes(dRMData.UserPassword);
                    bw.Write(strBuffer);
                }

                byte[] dRMDataArray = ms.ToArray();

                return dRMDataArray;
            }
            catch( Exception ex)
            {
                EventManager.WriteMessage(200, "ConverDRMDataToByteArray", EventLevel.Error, "ConverDRMDataToByteArray got exception:" + ex.Message);
            }

            return null;
        }

        public static bool SetDRMInfo(DRMInfo drmInfo)
        {
            AESFlags aesFlag = 0;
            dRMInfo = drmInfo;

            try
            {
                int nextOffset = Marshal.SizeOf(dRMData.AESVerificationKey) + Marshal.SizeOf((uint)dRMData.AESFlags) + Marshal.SizeOf(dRMData.CreationTime) + Marshal.SizeOf(dRMData.ExpireTime)
                     + Marshal.SizeOf(dRMData.AccessFlags) + Marshal.SizeOf(dRMData.LengthOfIncludeProcessNames) + Marshal.SizeOf(dRMData.OffsetOfIncludeProcessNames)
                     + Marshal.SizeOf(dRMData.LengthOfExcludeProcessNames) + Marshal.SizeOf(dRMData.OffsetOfExcludeProcessNames) + Marshal.SizeOf(dRMData.LengthOfIncludeUserNames) + Marshal.SizeOf(dRMData.OffsetOfIncludeUserNames)
                     + Marshal.SizeOf(dRMData.LengthOfExcludeUserNames) + Marshal.SizeOf(dRMData.OffsetOfExcludeUserNames) + Marshal.SizeOf(dRMData.LengthOfAccountName) + Marshal.SizeOf(dRMData.OffsetOfAccountName)
                     + Marshal.SizeOf(dRMData.LengthOfComputerId) + Marshal.SizeOf(dRMData.OffsetOfComputerId) + Marshal.SizeOf(dRMData.LengthOfUserPassword) + Marshal.SizeOf(dRMData.OffsetOfUserPassword);

                if (dRMInfo.AuthorizedProcessNames.Length > 0)
                {
                    dRMData.AuthorizedProcessNames = dRMInfo.AuthorizedProcessNames;
                    dRMData.LengthOfIncludeProcessNames = (uint)dRMInfo.AuthorizedProcessNames.Length * 2;
                    dRMData.OffsetOfIncludeProcessNames = (uint)nextOffset;
                    nextOffset += dRMInfo.AuthorizedProcessNames.Length * 2;
                    aesFlag |= AESFlags.Flags_Enabled_Check_ProcessName;
                }

                if (dRMInfo.UnauthorizedProcessNames.Length > 0)
                {
                    dRMData.UnauthorizedProcessNames = dRMInfo.UnauthorizedProcessNames;
                    dRMData.LengthOfExcludeProcessNames = (uint)dRMInfo.UnauthorizedProcessNames.Length * 2;
                    dRMData.OffsetOfExcludeProcessNames = (uint)nextOffset;
                    nextOffset += dRMInfo.UnauthorizedProcessNames.Length * 2;
                    aesFlag |= AESFlags.Flags_Enabled_Check_ProcessName;
                }

                if (dRMInfo.AuthorizedUserNames.Length > 0)
                {
                    dRMData.AuthorizedUserNames = dRMInfo.AuthorizedUserNames;
                    dRMData.LengthOfIncludeUserNames = (uint)dRMInfo.AuthorizedUserNames.Length * 2;
                    dRMData.OffsetOfIncludeUserNames = (uint)nextOffset;
                    nextOffset += dRMInfo.AuthorizedUserNames.Length * 2;
                    aesFlag |= AESFlags.Flags_Enabled_Check_UserName;
                }

                if (dRMInfo.UnauthorizedUserNames.Length > 0)
                {
                    dRMData.UnauthorizedUserNames = dRMInfo.UnauthorizedUserNames;
                    dRMData.LengthOfExcludeUserNames = (uint)dRMInfo.UnauthorizedUserNames.Length * 2;
                    dRMData.OffsetOfExcludeUserNames = (uint)nextOffset;
                    nextOffset += dRMInfo.UnauthorizedUserNames.Length * 2;
                    aesFlag |= AESFlags.Flags_Enabled_Check_UserName;
                }

                if (dRMInfo.AccountName.Length > 0)
                {
                    dRMData.AccountName = dRMInfo.AccountName;
                    dRMData.LengthOfAccountName = (uint)dRMInfo.AccountName.Length * 2;
                    dRMData.OffsetOfAccountName = (uint)nextOffset;
                    nextOffset += dRMInfo.AccountName.Length * 2;
                    aesFlag |= AESFlags.Flags_Enabled_Check_User_Permit;
                }

                if (dRMInfo.AuthorizedComputerIds.Length > 0)
                {
                    dRMData.ComputerIds = dRMInfo.AuthorizedComputerIds;
                    dRMData.LengthOfComputerId = (uint)dRMInfo.AuthorizedComputerIds.Length * 2;
                    dRMData.OffsetOfComputerId = (uint)nextOffset;
                    nextOffset += dRMInfo.AuthorizedComputerIds.Length * 2;
                    aesFlag |= AESFlags.Flags_Enabled_Check_Computer_Id;
                }

                if (dRMInfo.UserPassword.Length > 0)
                {
                    dRMData.UserPassword = dRMInfo.UserPassword;
                    dRMData.LengthOfUserPassword = (uint)dRMInfo.UserPassword.Length * 2;
                    dRMData.OffsetOfUserPassword = (uint)nextOffset;
                    nextOffset += dRMInfo.UserPassword.Length * 2;
                    aesFlag |= AESFlags.Flags_Enabled_Check_User_Permit;
                }

                if(dRMInfo.ExpireTime > 0 )
                {
                    dRMData.ExpireTime = dRMInfo.ExpireTime;
                    aesFlag |= AESFlags.Flags_Enabled_Expire_Time;
                }

                dRMData.CreationTime = DateTime.Now.ToFileTime();

                dRMData.AESFlags = aesFlag;
                dRMData.AccessFlags = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;

                return true;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(300, "SetDRMInfo", EventLevel.Error, "SetDRMInfo got exception:" + ex.Message);
            }

            return false;
        }

        /// <summary>
        /// Get the DRM data for the new encrypted file
        /// </summary>
        /// <returns></returns>
        public static byte[] GetDRMTagData(string fileName)
        {
            byte[] dRMDataArray = null;

            try
            {
                if (null == dRMInfo)
                {
                    EventManager.WriteMessage(50, "GetDRMTagData", EventLevel.Error, "No DRM data for file:" + fileName);

                    //the DRM data was not set.
                    return null;
                }

                if (embedDRMToFile)
                {
                    dRMDataArray = ConvertDRMDataToByteArray(dRMData);
                }
                else
                {
                    //the DRM data was managed by yourself, you can store it in your own server, here we just put it in the memory for demo.
                    dRMDataArray = UnicodeEncoding.Unicode.GetBytes("embedDRMToServer");
                }

                return dRMDataArray;
            }
            catch( Exception ex)
            {
                EventManager.WriteMessage(600, "GetDRMTagData", EventLevel.Error, "GetDRMTagData for file:" + fileName + " got exception:" + ex.Message);
                return null;
            }
        }

        public static bool GetEncryptedFileAccessPermission(EncryptEventArgs encryptArgs)
        {
            Boolean retVal = false;
            string fileName = encryptArgs.FileName;
            string lastError = string.Empty;
            string processName = encryptArgs.ProcessName;
            string userName = encryptArgs.UserName;
            string encryptKey = string.Empty;

            try
            {

                if (null == encryptArgs.EncryptionTag || encryptArgs.EncryptionTag.Length == 0)
                {
                    lastError = "There are no encryption tag data.";
                    return false;
                }

                string tagStr = UnicodeEncoding.Unicode.GetString(encryptArgs.EncryptionTag);

                if (string.Compare(tagStr, "embedDRMToServer") != 0)
                {
                    //the DRM data is not in the server.
                    retVal = true;

                    EventManager.WriteMessage(250, "GetEncryptedFileAccessPermission", EventLevel.Information,
                      "GetEncryptedFileAccessPermission:" + encryptArgs.FileName + ",userName:" + encryptArgs.UserName 
                      + ",processName:" + encryptArgs.ProcessName + ", DRM is embedded  to encrypted file, return success.");

                    return true;
                }
                else
                {
                    //you can store your DRM info to server, here we just demo the DRM info in local
                    if (null == dRMInfo)
                    {
                        lastError = "No DRM Info";
                        return false;
                    }

                    if (dRMInfo.ExpireTime > 0)
                    {
                        DateTime exTime = DateTime.FromFileTime(dRMInfo.ExpireTime);

                        if (DateTime.Now > exTime)
                        {
                            lastError = "the file is expired,current time:" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ",expire time:" + exTime.ToString("yyyy-MM-dd-HH-mm-ss");
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(dRMInfo.AuthorizedProcessNames))
                    {
                        if (dRMInfo.AuthorizedProcessNames.IndexOf(processName.ToLower().Trim()) < 0)
                        {
                            lastError = "the process " + processName + " is not in authorized process list.";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(dRMInfo.UnauthorizedProcessNames))
                    {
                        if (dRMInfo.UnauthorizedProcessNames.IndexOf(processName.ToLower().Trim()) >= 0)
                        {
                            lastError = "the process " + processName + " is in unauthorized process list.";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(dRMInfo.AuthorizedUserNames))
                    {
                        if (dRMInfo.AuthorizedUserNames.IndexOf(userName.ToLower().Trim()) < 0)
                        {
                            lastError = "the user " + userName + " is not in authorized user list.";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(dRMInfo.UnauthorizedUserNames))
                    {
                        if (dRMInfo.UnauthorizedUserNames.IndexOf(userName.ToLower().Trim()) >= 0)
                        {
                            lastError = "the user " + userName + " is in unauthorized user list.";
                            return false;
                        }
                    }

                    if (!string.IsNullOrEmpty(dRMInfo.AuthorizedComputerIds))
                    {
                        string currentComputerId = GetComputerId();
                        if (dRMInfo.AuthorizedComputerIds.IndexOf(currentComputerId.Trim()) < 0)
                        {
                            lastError = "the current computer " + currentComputerId + " is unauthorized to access encrypted file.";
                            return false;
                        }
                    }
                }

                retVal = true;
            }
            catch (Exception ex)
            {
                lastError = "GetFileAccessPermission exception." + ex.Message;
                retVal = false;
            }
            finally
            {
                if (!retVal)
                {
                    lastError += ",userName:" + encryptArgs.UserName + ",processName:" + encryptArgs.ProcessName;
                    EventManager.WriteMessage(100, "GetEncryptedFileAccessPermission", EventLevel.Error
                        , "Access denied for file:" + encryptArgs.FileName + "," + lastError );
                }
            }

            return retVal;

        }
      
    }

}
