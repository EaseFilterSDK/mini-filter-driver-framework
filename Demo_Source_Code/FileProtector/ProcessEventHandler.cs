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
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.Principal;
using System.Threading;
using System.Reflection;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace FileProtector
{

    public class ProcessEventHandler : IDisposable
    {
        MessageHandler messageHandler = null;
        bool disposed = false;


        public ProcessEventHandler(MessageHandler _messageHandler)
        {
            this.messageHandler = _messageHandler;
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

        ~ProcessEventHandler()
        {
            Dispose(false);
        }

        private void DisplayEventMessage(FileIOEventArgs fileIOEventArgs)
        {
            if (null != messageHandler)
            {
                messageHandler.DisplayEventMessage(fileIOEventArgs);
            }

        }


        /// <summary>
        /// Fires this event when the new process was being created.
        /// </summary>
        public void OnProcessCreation(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //   //test block the process creation.
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
        }

        /// <summary>
        /// Fires this event before the processs was terminiated.
        /// </summary>
        public void OnProcessPreTermination(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

            //test block the process terminiation.
            //if (e.ImageFileName.IndexOf("cmd.exe") >= 0)
            //{
            //    e.ReturnStatus = NtStatus.Status.AccessDenied;
            //}
        }

        /// <summary>
        /// Fires this event when the process creation was blocked by the setting,
        /// if the control flag 'ENABLE_SEND_PROCESS_DENIED_EVENT' was set.
        /// </summary>
        public void NotifyProcessWasBlocked(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyProcessTerminated(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyThreadCreation(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyThreadTerminated(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyProcessHandleInfo(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

        /// <summary>
        /// Fires this event after the event was triggered.
        /// </summary>
        public void NotifyThreadHandleInfo(object sender, ProcessEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.

        }

    }
}

