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

namespace SecureShare
{

    public class FilterWorker
    {

        public FilterMessage filterMessage = null;

        public FilterWorker(ListView listView_Info)
        {
            this.filterMessage = new FilterMessage(listView_Info);

        }

        public bool StartService(ref string lastError)
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

                GlobalConfig.RemoveAllFilterRules();

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
            GlobalConfig.RemoveAllFilterRules();
            string[] whiteList = null;
            string[] blacklist = null;

            FilterRule filterRuleShareFolder = new FilterRule();
            filterRuleShareFolder.IncludeFileFilterMask = GlobalConfig.ShareFolder + "\\*";
            filterRuleShareFolder.AccessFlag |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE | FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
            filterRuleShareFolder.AccessFlag  &= (uint)(~FilterAPI.AccessFlag.ALLOW_ENCRYPT_NEW_FILE);// this folder won't encrypt the new file.
            filterRuleShareFolder.AccessFlag &= (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);// all process can't read the encyrpted file except the authorized processes.
            filterRuleShareFolder.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
            filterRuleShareFolder.EncryptionPassPhrase = GlobalConfig.MasterPassword;

            //for whitelist process, it has maximum acess rights.
            if (GlobalConfig.ProtectFolderWhiteList == "*")
            {
                //allow all processes to read the encrypted file except the black list processes.
                filterRuleShareFolder.AccessFlag |= (uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
            }
            else
            {
                //for whitelist process, it has maximum acess rights.
                whiteList = GlobalConfig.ShareFolderWhiteList.Split(new char[] { ';' });
                if (whiteList.Length > 0)
                {
                    foreach (string authorizedUser in whiteList)
                    {
                        if (authorizedUser.Trim().Length > 0)
                        {
                             //not allow to encrypt the new file
                            uint accessFlag = filterRuleShareFolder.AccessFlag |(uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
                            filterRuleShareFolder.ProcessRights += ";" + authorizedUser + "!" + accessFlag.ToString();
                        }
                    }
                }

            }

            //for blacklist process, it has maximum acess rights.
            blacklist = GlobalConfig.ShareFolderBlackList.Split(new char[] { ';' });
            if (blacklist.Length > 0)
            {
                foreach (string unAuthorizedUser in blacklist)
                {
                    if (unAuthorizedUser.Trim().Length > 0)
                    {
                        //can't read the encrypted files, not allow to encrypt the new file
                        uint accessFlag = filterRuleShareFolder.AccessFlag ;
                        filterRuleShareFolder.ProcessRights += ";" + unAuthorizedUser + "!" + accessFlag.ToString();
                    }
                }
            }


            FilterRule filterRuleProtectFolder = new FilterRule();
            filterRuleProtectFolder.IncludeFileFilterMask = GlobalConfig.ProtectFolder + "\\*";
            filterRuleProtectFolder.AccessFlag |= (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE | FilterAPI.ALLOW_MAX_RIGHT_ACCESS;
            filterRuleProtectFolder.AccessFlag &= (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);// all process can't read the encyrpted file except the authorized processes.
            filterRuleProtectFolder.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_UNIQUE_IV;
            filterRuleProtectFolder.EncryptionPassPhrase = GlobalConfig.MasterPassword;

            //for whitelist process, it has maximum acess rights.
            if (GlobalConfig.ProtectFolderWhiteList == "*")
            {
                //allow all processes to read the encrypted file except the black list processes.
                filterRuleProtectFolder.AccessFlag |= (uint)(FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
            }
            else
            {
                //for whitelist process, it has maximum acess rights.
                whiteList = GlobalConfig.ProtectFolderWhiteList.Split(new char[] { ';' });
                if (whiteList.Length > 0)
                {
                    foreach (string authorizedUser in whiteList)
                    {
                        if (authorizedUser.Trim().Length > 0)
                        {
                            filterRuleProtectFolder.ProcessRights += ";" + authorizedUser + "!" + FilterAPI.ALLOW_MAX_RIGHT_ACCESS.ToString();
                        }
                    }
                }
            }

            //for blacklist process, it has maximum acess rights.
            blacklist = GlobalConfig.ProtectFolderBlackList.Split(new char[] { ';' });
            if (blacklist.Length > 0)
            {
                foreach (string unAuthorizedUser in blacklist)
                {
                    if (unAuthorizedUser.Trim().Length > 0)
                    {
                        //can't read the encrypted files
                        uint accessFlag = FilterAPI.ALLOW_MAX_RIGHT_ACCESS & (uint)(~FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES);
                        filterRuleProtectFolder.ProcessRights += ";" + unAuthorizedUser + "!" + accessFlag.ToString();
                    }
                }
            }

            GlobalConfig.AddFilterRule(filterRuleShareFolder);
            GlobalConfig.AddFilterRule(filterRuleProtectFolder);

            //send the filter rule settings to the filter driver here.
            GlobalConfig.SaveConfigSetting();

        }

        static void DisconnectCallback()
        {
            EventManager.WriteMessage(697, "DisconnectCallback", EventLevel.Information, "Filter Disconnected." + FilterAPI.GetLastErrorMessage());
        }


        public bool StopService()
        {
            FilterAPI.StopFilter();
            GlobalConfig.Stop();

            return true;
        }

        public void ClearMessage()
        {
            filterMessage.InitListView();
        }

        private Boolean FilterCallback(IntPtr sendDataPtr, IntPtr replyDataPtr)
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

               if (   messageSend.MessageType == (uint)FilterAPI.FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY)
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

                filterMessage.AddMessage(messageSend);
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
