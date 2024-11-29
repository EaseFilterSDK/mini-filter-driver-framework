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
using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Security.Principal;
using System.Threading;
using System.Reflection;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
{

    public class ProcessHandler : IDisposable
    {

        bool disposed = false;

        public ProcessHandler()
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

        ~ProcessHandler()
        {
            Dispose(false);
        }

        public void DisplayEventMessage(ProcessEventArgs processArgs)
        {

            try
            {
                string message = string.Empty;
                message += "ProcessFilter-MessageId:" + processArgs.MessageId.ToString() + "\r\n";
                message += "UserName:" + processArgs.UserName + "\r\n";
                message += "ImageFileName:" + processArgs.ImageFileName + "  (" + processArgs.ProcessId + ")" + "\r\n";
                message += "ThreadId:" + processArgs.ThreadId.ToString() + "\r\n";
                message += "EventName:" + processArgs.EventName + "\r\n";
                message += "IOStatus:" + processArgs.IOStatusToString() + "\r\n";
                message += "Description:" + processArgs.Description + "\r\n";

                if ((uint)processArgs.IoStatus >= (uint)NtStatus.Status.Error)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if ((uint)processArgs.IoStatus > (uint)NtStatus.Status.Warning)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                Console.WriteLine(message);

            }
            catch (Exception ex)
            {
                Console.WriteLine("DisplayEventMessage failed." + ex.Message);
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

