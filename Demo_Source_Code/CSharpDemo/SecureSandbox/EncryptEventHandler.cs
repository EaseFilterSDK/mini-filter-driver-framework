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

namespace SecureSandbox
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
            //if you want to block the encryption you can return access denied
            // e.ReturnStatus = NtStatus.Status.AccessDenied;
            //or return the encryption key and iv here.
            //e.EncryptionKey = new byte[32]; //put your own encryption key here
            //e.IV = Utils.GetRandomIV();

        }
     
    }
}