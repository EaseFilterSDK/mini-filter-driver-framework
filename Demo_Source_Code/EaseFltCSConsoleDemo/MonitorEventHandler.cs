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
using System.Text;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.IO;
using System.Threading;
using System.Reflection;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace EaseFltCSConsoleDemo
{
    
    public class MonitorEventHandler : IDisposable
    {
        bool disposed = false;
        public MonitorEventHandler()
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

        ~MonitorEventHandler()
        {
            Dispose(false);
        }


        public void DisplayEventMessage(FileIOEventArgs fileIOEventArgs)
        {
            try
            {
                string message = string.Empty;
                message +="MonitorFilter-MessageId:" + fileIOEventArgs.MessageId.ToString() + "\r\n";
                message += "UserName:" + fileIOEventArgs.UserName + "\r\n";
                message += "ProcessName:" + fileIOEventArgs.ProcessName + "  (" + fileIOEventArgs.ProcessId + ")" + "\r\n";
                message += "ThreadId:" + fileIOEventArgs.ThreadId.ToString() + "\r\n";
                message += "EventName:" + fileIOEventArgs.EventName + "\r\n";
                message += "FileName:" + fileIOEventArgs.FileName + "\r\n";
                message += "FileSize:" + fileIOEventArgs.FileSize.ToString() + "\r\n";
                message += "FileAttributes:" + ((FileAttributes)fileIOEventArgs.FileAttributes).ToString() + "\r\n";
                message += "LastWriteTime:" + DateTime.FromFileTime(fileIOEventArgs.LastWriteTime).ToString("yyyy-MM-ddTHH:mm") + "\r\n";
                message += "IOStatus:" + fileIOEventArgs.IOStatusToString() + "\r\n";
                message += "Description:" + fileIOEventArgs.Description + "\r\n";

                if ((uint)fileIOEventArgs.IoStatus >= (uint)NtStatus.Status.Error)
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else if ((uint)fileIOEventArgs.IoStatus > (uint)NtStatus.Status.Warning)
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
                Console.WriteLine("DisplayEventMessage failed."   + ex.Message);
            }

        }

        /// <summary>
        /// Fires this event after the file was opened, the handle is not closed. 
        /// </summary>
        public void OnFileOpen(object sender, FileCreateEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event after the new file was created, the handle is not closed.
        /// </summary>
        public void OnFileCreate(object sender, FileCreateEventArgs e)
        {
             DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the read IO was returned.
        /// </summary>
        public void OnFileRead(object sender, FileReadEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the write IO was returned.
        /// </summary>
        public void OnFileWrite(object sender, FileWriteEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the query file size IO was returned.
        /// </summary>
        public void OnQueryFileSize(object sender, FileSizeEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the query file basic information IO was returned.
        /// </summary>
        public void OnQueryFileBasicInfo(object sender, FileBasicInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the query file standard information IO was returned.
        /// </summary>
        public void OnQueryFileStandardInfo(object sender, FileStandardInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the query file network information IO was returned.
        /// </summary>
        public void OnQueryFileNetworkInfo(object sender, FileNetworkInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the query file Id IO was returned.
        /// </summary>
        public void OnQueryFileId(object sender, FileIdEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the query file info IO was returned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnQueryFileInfo(object sender, FileInfoArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the set file size IO was returned.
        /// </summary>
        public void OnSetFileSize(object sender, FileSizeEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the set file basic information was returned.
        /// </summary>
        public void OnSetFileBasicInfo(object sender, FileBasicInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the set file standard information IO was returned.
        /// </summary>
        public void OnSetFileStandardInfo(object sender, FileStandardInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the set file network information IO was returned.
        /// </summary>
        public void OnSetFileNetworkInfo(object sender, FileNetworkInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the file was moved or renamed.
        /// </summary>
        public void OnMoveOrRenameFile(object sender, FileMoveOrRenameEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the delete IO was returned.
        /// </summary>
        public void OnDeleteFile(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the set file info IO was returned.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnSetFileInfo(object sender, FileInfoArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the query file security IO was returned.
        /// </summary>
        public void OnQueryFileSecurity(object sender, FileSecurityEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the set file security IO was returned.
        /// </summary>
        public void OnSetFileSecurity(object sender, FileSecurityEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the directory enumeration query IO was returned.
        /// </summary>
        public void OnQueryDirectoryFile(object sender, FileQueryDirectoryEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the file was cleanuped.
        /// </summary>
        public void OnFileHandleClose(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event after the last file handle of the file was closed.
        /// </summary>
        public void OnFileClose(object sender, FileIOEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
       
        /// <summary>
        /// Fires this event when a file was changed after the file handle closed
        /// </summary>
        public void NotifyFileWasChanged(object sender, FileChangedEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

        /// <summary>
        /// Fires this event when the filter driver attached to a volume.
        /// </summary>
        public void OnFilterAttachToVolume(object sender, VolumeInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }
        /// <summary>
        /// Fires this event when a volume was detached from the filter driver.
        /// </summary>
        public void OnFilterDetachFromVolume(object sender, VolumeInfoEventArgs e)
        {
            DisplayEventMessage(e);
            //do your job here.
        }

    }
}