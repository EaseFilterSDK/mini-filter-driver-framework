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
using System.IO;
using System.Text;

namespace EaseFilter.FilterControl
{
    public class EncryptEventArgs : FileIOEventArgs
    {
        public EncryptEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                EncryptionTag = new byte[messageSend.DataBufferLength];
                Array.Copy(messageSend.DataBuffer, EncryptionTag, messageSend.DataBufferLength);


            }
        }

        /// <summary>
        /// The access control flag of the encrypted file
        /// </summary>
        public uint AccessFlags { get; set; }
        /// <summary>
        /// The encryption iv of the encrypted file
        /// </summary>
        public byte[] IV { get; set; }
        /// <summary>
        /// The encryption key of the encrypted file
        /// </summary>
        public byte[] EncryptionKey { get; set; }
        /// <summary>
        /// The tag data which attached to the encrypted file.
        /// </summary>
        public byte[] EncryptionTag { get; set; }

        string description = string.Empty;
        /// <summary>
        /// The description of the event args
        /// </summary>
        public override string Description
        {
            get
            {
                if( description.Length > 0)
                {
                    return description;
                }
                else
                {
                    return "Request encryption key and iv for file:" + FileName;
                }
            }

            set { description = value; }

        }
    }

    partial class FileFilter
    {
        /// <summary>
        /// Fires this event when the encrypted file request the encryption key and iv.
        /// </summary>
        public event EventHandler<EncryptEventArgs> OnFilterRequestEncryptKey;

        /// <summary>
        /// enable the encryption feature when the encryption key was set.
        /// </summary>
        byte[] encryptionKey = null;

        /// <summary>
        /// generate encryptio key with this passphrase
        /// </summary>
        string encryptionPassPhrase = string.Empty;

        /// <summary>
        /// enable the encryption with the same IV.
        /// </summary>
        byte[] encryptionIV = null;


        /// <summary>
        /// Enable the encryption feature when it is true
        /// only when the license key supports the encryption filter driver.
        /// </summary>
        public bool EnableEncryption
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE;
                }
            }
        }

        /// <summary>
        /// Encrypt the new created file when it is true and the encryption was enabled.
        /// </summary>
        public bool EnableEncryptNewFile
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE;
                }
            }
        }

        /// <summary>
        /// Automatically decrypt the data in read when it is true, 
        /// or you will get thw raw data of the encrypted file.
        /// </summary>
        public bool EnableReadEncryptedData
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
                }
            }
        }

        /// <summary>
        /// This feature is to encrypt the data on the read when the encryption was enabled, encrypt new file was disabled.
        /// the file won't be encrypted in the loca disk, i.e. encrypt the file before it was sent out of your folder.
        /// </summary>
        public bool EnableEncryptionOnTheGo
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.DISABLE_ENCRYPT_DATA_ON_READ) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.DISABLE_ENCRYPT_DATA_ON_READ;                    
                }
                else
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.DISABLE_ENCRYPT_DATA_ON_READ;
                }
            }
        }

        /// <summary>
        /// Set the encryption key of the filter rule,
        /// the encryption key length must be 16 bytes, 24 bytes or 32 bytes
        /// </summary>
        public byte[] EncryptionKey
        {
            get { return encryptionKey; }
            set { encryptionKey = value; }
        }

        /// <summary>
        /// Use the pass phrase to generate the 32 bytes encryption key
        /// </summary>
        public string EncryptionPassPhrase
        {
            get { return encryptionPassPhrase; }
            set 
            {
                encryptionPassPhrase = value;
                encryptionKey = Utils.GetKeyByPassPhrase(value, 32);
            }
        }

        /// <summary>
        /// use the same encryption IV for all encryption when it was set.
        /// </summary>
        public byte[] EncryptionIV
        {
            get { return encryptionIV; }
            set { encryptionIV = value; }
        }

        /// <summary>
        /// if it is true, the filter driver will request the encryption key and iv from the callback deletegate for every file.
        /// </summary>
        public bool EnableEncryptKeyandIVFromService
        {
            get
            {
                return (booleanConfig & (uint)FilterAPI.BooleanConfig.REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE) > 0;
            }

            set
            {
                if (value)
                {
                    booleanConfig |= (uint)FilterAPI.BooleanConfig.REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE;
                }
                else
                {
                    booleanConfig &= ~(uint)FilterAPI.BooleanConfig.REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE;
                }
            }
        }
     
        public bool RequestEncryptionKey(FilterAPI.MessageSendData messageSend, ref FilterAPI.MessageReplyData messageReply)
        {
            bool retVal = true;

            //if the event was subscribed 
            if (null != OnFilterRequestEncryptKey)
            {
                EncryptEventArgs fileEncryptArgs = new EncryptEventArgs(messageSend);
                fileEncryptArgs.EventName = "OnFilterRequestEncryptKey";

                OnFilterRequestEncryptKey(this, fileEncryptArgs);

                if (fileEncryptArgs.ReturnStatus == NtStatus.Status.Success)
                {
                    MemoryStream ms = new MemoryStream();
                    BinaryWriter bw = new BinaryWriter(ms);

                    bw.Write(fileEncryptArgs.AccessFlags);
                    bw.Write(fileEncryptArgs.IV.Length);
                    bw.Write(fileEncryptArgs.IV);
                    bw.Write(fileEncryptArgs.EncryptionKey.Length);
                    bw.Write(fileEncryptArgs.EncryptionKey);

                    byte[] dataBuffer = ms.ToArray();
                    messageReply.DataBufferLength = (uint)dataBuffer.Length;
                    Array.Copy(dataBuffer, messageReply.DataBuffer, dataBuffer.Length);

                    messageReply.ReturnStatus = (uint)NtStatus.Status.Success;
                }
                else
                {
                    messageReply.ReturnStatus = (uint)NtStatus.Status.AccessDenied;
                }
            }

            return retVal;
        }
    }
}
