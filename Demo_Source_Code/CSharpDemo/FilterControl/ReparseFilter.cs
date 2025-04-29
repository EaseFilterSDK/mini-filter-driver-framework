using System;
using System.IO;

namespace EaseFilter.FilterControl
{
    public class ReparseFileEventArgs : FileIOEventArgs
    {
        public ReparseFileEventArgs(FilterAPI.MessageSendData messageSend)
            : base(messageSend)
        {
            if (messageSend.DataBufferLength > 0)
            {
                TagData = new byte[messageSend.DataBufferLength];
                Array.Copy(DataBuffer, TagData, messageSend.DataBufferLength);
            }

            if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_REPARSE_FILE_CREATE_REQUEST)
            {
                //when a new created file, it requests the encryption key, iv and tag data.
                isNewCreatedFile = true;
            }
            else if (messageSend.FilterCommand == (uint)FilterAPI.FilterCommand.FILTER_REPARSE_FILE_OPEN_REQUEST)
            {
                /// <summary>
                /// This is reparse point file open request with the tag data.
                /// </summary>
                /// <param name="messageSend"></param>
                isReparseFile = true;
            }
        }

        /// <summary>
        /// This is new created file when the filter command is FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA
        /// </summary>
        private bool isNewCreatedFile = false;

        /// <summary>
        /// This is reparse file with tag data
        /// </summary>
        private bool isReparseFile = false;

        private byte[] tagData = null;
        /// <summary>
        /// The tag data which was embedded to the header of the encrypted file.
        /// </summary>
        public byte[] TagData
        {
            get
            {
                if (null == tagData)
                {
                    tagData = new byte[0];
                }

                return tagData;
            }
            set
            {
                tagData = value;

            }

        }

        /// <summary>
        /// it means it is new created file when it is true.
        /// </summary>
        public bool IsNewCreatedFile
        {
            get { return isNewCreatedFile; }
            set { isNewCreatedFile = value; }
        }

        /// <summary>
        /// it means it is reparse file with tag data when it is true.
        /// </summary>
        public bool IsReparseFile
        {
            get { return isReparseFile; }
            set { isReparseFile = value; }
        }

        string description = string.Empty;
        /// <summary>
        /// The description of the event args
        /// </summary>
        public override string Description
        {
            get
            {
                if (description.Length > 0)
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
        public event EventHandler<ReparseFileEventArgs> OnReparseFilterRequestOpen;

        public bool ReparseFilterHandler(FilterAPI.MessageSendData messageSend, ref FilterAPI.MessageReplyData messageReply)
        {
            bool retVal = true;

            //if the event was subscribed 
            if (null != OnReparseFilterRequestOpen)
            {
                ReparseFileEventArgs reparseEventArgs = new ReparseFileEventArgs(messageSend);
                reparseEventArgs.EventName = "OnReparseFilterRequestOpen";

                OnReparseFilterRequestOpen(this, reparseEventArgs);

                messageReply.ReturnStatus = (uint)reparseEventArgs.ReturnStatus;


                if (reparseEventArgs.IsNewCreatedFile)
                {
                    //this is new created file to request the tag data,
                    //the new file will be created with the reparse point tag data.							

                    //if you want to block the new file creation, return  STATUS_ACCESS_DENIED
                    //messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
                    //messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

                    if (reparseEventArgs.ReturnStatus == NtStatus.Status.Success)
                    {
                        MemoryStream ms = new MemoryStream();
                        BinaryWriter bw = new BinaryWriter(ms);
                        byte[] iv = new byte[16];
                        uint ivLength = (uint)iv.Length;
                        uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
                        bw.Write(accessFlag);
                        bw.Write(ivLength);
                        bw.Write(iv);
                        byte[] encryptionKey = new byte[32];
                        bw.Write(EncryptionKey.Length);
                        bw.Write(encryptionKey);

                        bw.Write(reparseEventArgs.TagData.Length);
                        if (reparseEventArgs.TagData.Length > 0)
                        {
                            bw.Write(reparseEventArgs.TagData);
                        }

                        byte[] dataBuffer = ms.ToArray();
                        messageReply.DataBufferLength = (uint)dataBuffer.Length;
                        Array.Copy(dataBuffer, messageReply.DataBuffer, dataBuffer.Length);

                    }

                }
                else
                {
                    //opening the existing reparse point file.

                    //if you want to block the file open, return  STATUS_ACCESS_DENIED
                    //messageReply->FilterStatus = FILTER_COMPLETE_PRE_OPERATION;
                    //messageReply->ReturnStatus = STATUS_ACCESS_DENIED;

                }
            }

            return retVal;
        }
    }

}
