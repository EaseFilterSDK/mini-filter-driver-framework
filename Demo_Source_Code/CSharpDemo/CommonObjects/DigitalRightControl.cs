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
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Xml.Serialization;

namespace EaseFilter.CommonObjects
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

        /// <summary>
        /// Create an encrypted file with embedded digital right policy, distribute the encrypted file via internet, 
        /// only the authorized users and processes can access the encrypted file.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="passPhrase"></param>
        /// <param name="policy"></param>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public static bool EncryptFileWithEmbeddedDRPolicy(string sourceFileName, string destFileName, byte[] encryptIV, byte[] encryptKey,DRPolicyData policy, out string lastError)
        {
            bool ret = false;
            FileStream fs = null;
            lastError = string.Empty;

            try
            {
                if (!File.Exists(sourceFileName))
                {
                    lastError = sourceFileName + " doesn't exist.";
                    return false;
                }

                byte[] AESBuffer = ConvertDRPolicyDataToByteArray(policy);

                //encrypt the file with encryption key and a iv key.
                ret = FilterAPI.AESEncryptFileToFile(sourceFileName, destFileName, (uint)encryptKey.Length, encryptKey, (uint)encryptIV.Length, encryptIV, false);
                if (!ret)
                {
                    lastError = "Create encrypt file " + destFileName + " failed with error:" + FilterAPI.GetLastErrorMessage();
                    return ret;
                }

                fs = new FileStream(destFileName, FileMode.Append, FileAccess.Write, FileShare.Read);

                //append the DR policy to the encrypted file.
                fs.Write(AESBuffer, 0, AESBuffer.Length);

              
            }
            catch (Exception ex)
            {
                ret = false;
                lastError = "Encrypt file " + sourceFileName + " failed with error:" + ex.Message;
            }
            finally
            {
                if (null != fs)
                {
                    fs.Close();
                }
            }

            return ret;
        }

        /// <summary>
        /// Process the encrypted file's embedded access policy, remove embedded information, add AESTagData to encrypted file, 
        /// Create a filter driver aware encrypted file. Then you can read the encrypted file transparently via filter driver encryption engine.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="passPhrase"></param>
        /// <param name="lastError"></param>
        /// <returns></returns>
        public static bool ProcessSecureShareFile(string fileName, out string lastError)
        {
            bool ret = false;
            lastError = string.Empty;

            try
            {
                if (!File.Exists(fileName))
                {
                    lastError = fileName + " doesn't exist.";
                    return false;
                }

                if (!fileName.EndsWith(GlobalConfig.ShareFileExt))
                {
                    lastError = fileName + " extension is not correct.";
                    return false;
                }

                FileAttributes attributes = File.GetAttributes(fileName);
                attributes = (~FileAttributes.ReadOnly) & attributes;
                File.SetAttributes(fileName, attributes);

                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                long fileSize = fs.Length;

                //read the last 4 bytes data, it is the total size of the embedded data.

                fs.Position = fileSize - 4;
                BinaryReader br = new BinaryReader(fs);
                uint sizeOfAESData = br.ReadUInt32();

                if (sizeOfAESData >= fileSize)
                {
                    lastError = fileName + " is not valid share encrypted file, the sizeOfAESData:" + sizeOfAESData + " >= file size:" + fileSize;
                    return false;
                }

                fs.Position = fileSize - sizeOfAESData;

                //Read the embedded data 
                byte[] AESBuffer = new byte[sizeOfAESData];
                fs.Read(AESBuffer, 0, (int)sizeOfAESData);

                //since the last 4 bytes for sizeOfAESData is not encrypted, we need to put back the clear value back.
                MemoryStream ms = new MemoryStream(AESBuffer);
                ms.Position = 0;
                br = new BinaryReader(ms);
                uint verificationKey = br.ReadUInt32();

                //verify if this is the valid embedded data.
                if (verificationKey != AES_VERIFICATION_KEY)
                {
                    lastError = fileName + " is not valid share encrypted file, the encryption key:" + verificationKey + " is not valid.";
                    return false;
                }

        
                //Remove the embedded data, this is the original file size without the embedded information.
                fs.SetLength(fileSize - sizeOfAESData);

                fs.Close();
                fs = null;

                if (fileName.EndsWith(GlobalConfig.ShareFileExt))
                {
                    string newFileName = fileName.Remove(fileName.Length - GlobalConfig.ShareFileExt.Length);
                    File.Move(fileName, newFileName);
                    fileName = newFileName;
                }

                //add the DR data to the encrypted file as a tag data.
                ret = FilterAPI.EmbedDRPolicyDataToFile(fileName, AESBuffer, out lastError);

            }
            catch (Exception ex)
            {
                ret = false;
                lastError = "ProcessSecureShareFile " + fileName + " failed with error:" + ex.Message;
            }


            return ret;
        }


        public static T DecryptStrToObject<T>( string toDeserialize)
        {
            string decryptedStr = FilterAPI.AESEncryptDecryptStr(toDeserialize, FilterAPI.EncryptType.Decryption);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringReader textReader = new StringReader(decryptedStr);
            return (T)xmlSerializer.Deserialize(textReader);
        }

        public static string EncryptObjectToStr<T>( T toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            StringWriter textWriter = new StringWriter();
            xmlSerializer.Serialize(textWriter, toSerialize);

            string encryptedText = FilterAPI.AESEncryptDecryptStr(textWriter.ToString(), FilterAPI.EncryptType.Encryption);

            return encryptedText;
        }

        public static byte[] ConvertAESDataToByteArray(AESDataBuffer aesData)
        {
            MemoryStream ms = new MemoryStream();
            BinaryWriter bw = new BinaryWriter(ms);

            bw.Write(aesData.AccessFlags);
            bw.Write(aesData.IVLength);
            bw.Write(aesData.IV);
            bw.Write(aesData.EncryptionKeyLength);
            bw.Write(aesData.EncryptionKey);
            bw.Write(aesData.TagDataLength);

            if (aesData.TagDataLength > 0 && aesData.TagData != null)
            {
                bw.Write(aesData.TagData);
            }

            return ms.ToArray();
        }
    }
}
