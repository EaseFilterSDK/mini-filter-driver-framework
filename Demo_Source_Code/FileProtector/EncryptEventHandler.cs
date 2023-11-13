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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Threading;
using System.Reflection;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace FileProtector
{
    
    public class EncryptEventHandler : IDisposable
    {
        bool disposed = false;

        public EncryptEventHandler()
        {
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
            }

            disposed = true;
        }

        ~EncryptEventHandler()
        {
            Dispose(false);
        }

        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public void OnFilterRequestEncryptKey(object sender, EncryptEventArgs e)
        {

            e.ReturnStatus = NtStatus.Status.Success;

            try
            {
                if (e.IsNewCreatedFile)
                {
                    byte[] iv = Guid.NewGuid().ToByteArray();
                    //for the new created file, you can add your custom tag data to the header of the encyrpted file here.
                    //here we add the iv to the tag data.
                    e.EncryptionTag = iv;

                    //if you don't set the iv data, the filter driver will generate the new GUID as iv 
                    e.IV = iv;

                    //here is the encryption key for the new encrypted file, you can set it with your own unique custom key.
                    e.EncryptionKey = Utils.GetKeyByPassPhrase(GlobalConfig.MasterPassword, 32);

                    //if you want to block the new file creation, you can return access denied status.
                    //e.ReturnStatus = NtStatus.Status.AccessDenied;

                    //if you want to the file being created without encryption, return below status.
                    //e.ReturnStatus = NtStatus.Status.FileIsNoEncrypted;  

                }
                else
                {
                    //this is the encrytped file open request, request the encryption key and iv.
                    //here is the tag data if you set custom tag data when the new created file requested the key.
                    byte[] tagData = e.EncryptionTag;

                    //if (!GetEncryptedFileAccessPermission(e))
                    //{
                    //    //here didn't get the permission for the encrytped file open, it will return the raw encrypted data.
                    //    e.ReturnStatus = NtStatus.Status.FileIsEncrypted;
                    //}

                    //The encryption key must be the same one which you created the new encrypted file.
                    e.EncryptionKey = Utils.GetKeyByPassPhrase(GlobalConfig.MasterPassword, 32);

                    //here is the iv key we saved in tag data.
                    //e.IV = tagData;

                    //if you want to block encrypted file being opened, you can return accessdenied status.
                    //e.ReturnStatus = NtStatus.Status.AccessDenied;

                    //if you want to return the raw encrypted data for this encrypted file, return below status.
                    //e.ReturnStatus = NtStatus.Status.FileIsEncrypted;

                    EventManager.WriteMessage(250, "OpenEncryptedFile", EventLevel.Information,
                        "OpenEncryptedFile:" + e.FileName + ",userName:" + e.UserName + ",processName:" + e.ProcessName + ",return status:" + e.ReturnStatus.ToString());

                }

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(500, "OnFilterRequestEncryptKey", EventLevel.Error, "OnFilterRequestEncryptKey:" + e.FileName + ",got exeception:" + ex.Message);
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }


        }
     
    }
}