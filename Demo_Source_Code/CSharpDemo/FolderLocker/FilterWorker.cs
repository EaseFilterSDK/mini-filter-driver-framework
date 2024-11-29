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
using System.Windows.Forms;

using EaseFilter.CommonObjects;

namespace EaseFilter.FolderLocker
{

    public class FilterWorker
    {


        public FilterWorker()
        {
        }

        static public bool StartService(ref string lastError)
        {
            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string registerKey = GlobalConfig.registerKey;

            bool ret = false;
            lastError = string.Empty;

            try
            {

                ret = FilterAPI.StartFilter((int)GlobalConfig.FilterConnectionThreads
                                                , registerKey
                                                , new FilterAPI.FilterDelegate(FilterCallback)
                                                , new FilterAPI.DisconnectDelegate(DisconnectCallback)
                                                , ref lastError);
                if (!ret)
                {
                    lastError = "Start filter service failed with error " + lastError;
                    EventManager.WriteMessage(43, "StartFilter", EventLevel.Error, lastError);
                    return ret;
                }

     
                ApplySettingsToFilterDriver();

                EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");
            }
            catch (Exception ex)
            {
                lastError = "Start filter service failed with error " + ex.Message;
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, lastError);
            }

            return ret;
        }

        static public void ApplySettingsToFilterDriver()
        {
            FilterRule filterRuleShareFolder = new FilterRule();
            filterRuleShareFolder.Id = GlobalConfig.GetFilterRuleId();
            filterRuleShareFolder.IncludeFileFilterMask = GlobalConfig.ShareFolder + "\\*";
            filterRuleShareFolder.AccessFlag |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE | FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
            filterRuleShareFolder.AccessFlag &= (uint)(~FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE);// this folder won't encrypt the new file.
            filterRuleShareFolder.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
            filterRuleShareFolder.EncryptionPassPhrase = GlobalConfig.MasterPassword;

            GlobalConfig.AddFilterRule(filterRuleShareFolder);

            //send the filter rule settings to the filter driver here.
            GlobalConfig.SaveConfigSetting();

        }

        static void DisconnectCallback()
        {
            EventManager.WriteMessage(697, "DisconnectCallback", EventLevel.Information, "Filter Disconnected." + FilterAPI.GetLastErrorMessage());
        }


        static public bool StopService()
        {
            FilterAPI.StopFilter();
            GlobalConfig.Stop();

            return true;
        }

        public void ClearMessage()
        {
        }

        static private Boolean FilterCallback(IntPtr sendDataPtr, IntPtr replyDataPtr)
        {
            bool retVal = true;

            try
            {
                FilterAPI.MessageSendData messageSend = new FilterAPI.MessageSendData();
                messageSend = (FilterAPI.MessageSendData)Marshal.PtrToStructure(sendDataPtr, typeof(FilterAPI.MessageSendData));

                if (FilterAPI.MESSAGE_SEND_VERIFICATION_NUMBER != messageSend.VerificationNumber)
                {
                    EventManager.WriteMessage(139, "FilterCallback", EventLevel.Error, "Received message corrupted.Please check if the MessageSendData structure is correct.");
                    return false;
                }

                if (messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY)
                {

                    if ((replyDataPtr.ToInt64() != 0))
                    {

                        //this is the customized tag data which was attahced to the encrypted file when it was created.
                        uint tagDataLength = messageSend.DataBufferLength;
                        byte[] tagData = messageSend.DataBuffer;

                        FilterAPI.MessageReplyData messageReply = (FilterAPI.MessageReplyData)Marshal.PtrToStructure(replyDataPtr, typeof(FilterAPI.MessageReplyData));
                        messageReply.MessageId = messageSend.MessageId;
                        messageReply.MessageType = messageSend.MessageType;

                        //get permission for secure shared file from server, here just demo the server in local,
                        //in reality, your server could be in remote computer.
                        retVal = DRServer.GetFileAccessPermission(ref messageSend, ref messageReply);

                        if (retVal)
                        {
                            messageReply.ReturnStatus = (uint)FilterAPI.NTSTATUS.STATUS_SUCCESS;
                        }
                        else
                        {
                            //if you don't want to authorize the process to read the encrytped file,you can set the value as below:
                            messageReply.ReturnStatus = (uint)FilterAPI.NTSTATUS.STATUS_ACCESS_DENIED;
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }

                        Marshal.StructureToPtr(messageReply, replyDataPtr, true);

                        if (!retVal)
                        {
                            messageSend.Status = (uint)FilterAPI.NTSTATUS.STATUS_ACCESS_DENIED;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(134, "FilterCallback", EventLevel.Error, "filter callback exception." + ex.Message);
                return false;
            }

            return retVal;

        }

    }

}
