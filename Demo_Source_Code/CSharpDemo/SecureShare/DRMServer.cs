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
using System.Net;
using System.Windows.Forms;
using System.ServiceModel;


using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace  SecureShare
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
    /// This is the DRM info meta data which will be stored in server if revoke access control is enabled.
    /// </summary>
    public class DRMData
    {
        /// <summary>
        /// If it is not empty, only the processes in the process list can access the files.
        /// </summary>
        public string AuthorizedProcessNames = string.Empty;
        /// <summary>
        /// If it is not empty, all the processes in the process list can not access the files.
        /// </summary>
        public string UnauthorizedProcessNames = string.Empty;
        /// <summary>
        /// If it is not empty, only the users in the user name list can access the files.
        /// </summary>
        public string AuthorizedUserNames = string.Empty;
        /// <summary>
        /// If it is not empty, all the useres in the user name list can not access the files.
        /// </summary>
        public string UnauthorizedUserNames = string.Empty;
        /// <summary>
        /// If it is not empty, only the computer in the computer id list can access the files.
        /// </summary>
        public string AuthorizedComputerIds = string.Empty;
        /// <summary>
        /// If it is not empty, only the Ip in the Ip list can access the files.
        /// </summary>
        public string AuthorizedIps = string.Empty;
        /// <summary>
        /// the password of the shared file.
        /// </summary>
        public string UserPassword = string.Empty;
        /// <summary>
        /// the access flags of the shared file.
        /// </summary>
        public uint AccessFlags = 0;
        /// <summary>
        /// The file will be expired after the expire time in UTC format, and it can't be accessed.           
        /// </summary>
        public long ExpireTime = 0;
        /// <summary>
        /// The time in UTC format of the encrypted file was created.
        /// </summary>
        public long CreationTime = 0;
        /// <summary>
        /// the file name which was applied with policy.
        /// </summary>
        public string FileName = string.Empty;
        /// <summary>
        /// The encryption key hex string.
        /// </summary>
        public string EncryptionKey = string.Empty;
        /// <summary>
        /// the iv hex string.
        /// </summary>
        public string EncryptionIV = string.Empty;
        /// <summary>
        /// the email account which was applied with policy.
        /// </summary>
        public string EmailAccount = string.Empty;
        /// <summary>
        /// the computerId which was applied with policy.
        /// </summary>
        public string ComputerId = string.Empty;
    }

    public class CacheUserAccessInfo
    {
        public uint processId = 0;
        public byte[] iv = null;
        public byte[] key = null;
        public NtStatus.Status status = NtStatus.Status.AccessDenied;

    }

    public class DRMServer
    {

        /// <summary>
        /// the cache table for the access DRM data based on the process Id.
        /// </summary>
        static Dictionary<string, CacheUserAccessInfo> userAccessCacheTable = new Dictionary<string, CacheUserAccessInfo>();
        static DRMServer()
        {
        }

       
        public static ServiceReference1.SecureShareSoapClient GetServiceClient(ref string lastError)
        {
            string serverURI = "http://www.easefilter.com/SecureShare.asmx";
            ServiceReference1.SecureShareSoapClient client = null;

            string licenseKey = GlobalConfig.LicenseKey;

            try
            {
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.OpenTimeout = new TimeSpan(0, 0, 10);
                binding.CloseTimeout = new TimeSpan(0, 0, 10);
                binding.SendTimeout = new TimeSpan(0, 0, 10);
                binding.ReceiveTimeout = new TimeSpan(0, 0, 10);

                EndpointAddress endpoint = new EndpointAddress(new Uri(serverURI));

                client = new ServiceReference1.SecureShareSoapClient(binding, endpoint);
            }
            catch (Exception ex)
            {
                lastError = "Connecting to server failed with error " + ex.Message;
            }

            return client;
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);
            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        static public string DecodeFrom64(string encodedData)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(encodedData);
            string returnValue = System.Text.ASCIIEncoding.ASCII.GetString(encodedDataAsBytes);

            return returnValue;
        }


        static private string EncodeDRMData(DRMData dRM)
        {
            string drmInfo = string.Empty;
            drmInfo = dRM.EmailAccount + "|";
            drmInfo += dRM.FileName + "|";
            drmInfo += dRM.EncryptionKey + "|";
            drmInfo += dRM.EncryptionIV + "|";
            drmInfo += dRM.AccessFlags + "|";
            drmInfo += dRM.AuthorizedProcessNames + "|";
            drmInfo += dRM.UnauthorizedProcessNames + "|";
            drmInfo += dRM.AuthorizedUserNames + "|";
            drmInfo += dRM.UnauthorizedUserNames + "|";
            drmInfo += dRM.AuthorizedComputerIds + "|";
            drmInfo += dRM.AuthorizedIps + "|";
            drmInfo += dRM.ExpireTime + "|";
            drmInfo += dRM.CreationTime + "|";
            drmInfo += dRM.ComputerId + "|";
            drmInfo = EncodeTo64(drmInfo);

            return drmInfo;
        }

        static bool DecodeDRMStr(string drmInfoHexStr, bool isEncoded, out DRMData dRM, out string lastError)
        {
            dRM = new DRMData();
            lastError = string.Empty;
            bool retVal = false;
            string drmInfo = drmInfoHexStr;

            try
            {
                if (isEncoded)
                {
                    drmInfo = DecodeFrom64(drmInfoHexStr);
                }

                string[] infos = drmInfo.Split(new char[] { '|' });

                if (infos.Length < 14)
                {
                    lastError = "Invalid DRMInfo." + drmInfo;
                    return false;
                }

                dRM.EmailAccount = infos[0];
                dRM.FileName = infos[1];
                dRM.EncryptionKey = infos[2];
                dRM.EncryptionIV = infos[3];
                dRM.AccessFlags = uint.Parse(infos[4]);
                dRM.AuthorizedProcessNames = infos[5];
                dRM.UnauthorizedProcessNames = infos[6];
                dRM.AuthorizedUserNames = infos[7];
                dRM.UnauthorizedUserNames = infos[8];
                dRM.AuthorizedComputerIds = infos[9];
                dRM.AuthorizedIps = infos[10];
                dRM.ExpireTime = long.Parse(infos[11]);
                dRM.CreationTime = long.Parse(infos[12]);
                dRM.ComputerId = infos[13];

                retVal = true;
            }
            catch (Exception ex)
            {
                lastError = ex.Message;
                retVal = false;
            }

            return retVal;

        }

        static public bool GetFilePermission( EncryptEventArgs e , ref string lastError)
        {
            bool retVal = false;
            string computerName = Environment.MachineName;
            lastError = string.Empty;

            //tagdata format: GlobalConfig.AccountName + ";" + RegisterForm.GetUniqueComputerId().ToString() + ";" + encryptionIV;
            string tagDataStr = ASCIIEncoding.ASCII.GetString(e.EncryptionTag);
            string[] accountInfo = tagDataStr.Split(new char[] { ';' });

            try
            {

                string clientInfo = string.Empty;              

                if (accountInfo.Length < 3)
                {
                    //we can't get the correct encryption tag data.
                    lastError = "EncryptionTag " + tagDataStr + " is not valid for encrypted file:" + e.FileName;
                    EventManager.WriteMessage(250, "GetFilePermission", EventLevel.Error, lastError);

                    e.ReturnStatus = NtStatus.Status.AccessDenied;
                    return false;
                }

                lock (userAccessCacheTable)
                {
                    if (userAccessCacheTable.ContainsKey(e.ProcessId.ToString() + tagDataStr))
                    {
                        CacheUserAccessInfo userAccessInfo = userAccessCacheTable[e.ProcessId.ToString() + tagDataStr];

                        e.IV = userAccessInfo.iv;
                        e.EncryptionKey = userAccessInfo.key;
                        e.ReturnStatus = userAccessInfo.status;

                        lastError = "ProcessId:" + e.ProcessId + " in cache, ReturnStatus: " + e.ReturnStatus.ToString();
                        EventManager.WriteMessage(240, "GetFilePermission", EventLevel.Information, lastError + ",FileName:" + e.FileName);

                        if (e.ReturnStatus == NtStatus.Status.Success)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }


                clientInfo = tagDataStr;

                string ipv4 = string.Empty;
                IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ip in host.AddressList)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ipv4 = ip.ToString();
                        break;
                    }
                }

                string drmInfo = string.Empty;
                clientInfo += ";" + e.ProcessName + ";" + e.UserName + ";" + computerName + ";" + e.FileName + ";" + ipv4;

                if (GlobalConfig.IsDRMDataInLocal)
                {
                    retVal = LocalClient.GetFilePermission(clientInfo, out drmInfo, out lastError);
                }
                else
                {
                    ServiceReference1.SecureShareSoapClient client = GetServiceClient(ref lastError);

                    if (null == client)
                    {
                        return false;
                    }

                    retVal = client.GetFilePermission(clientInfo, out drmInfo, out lastError);
                }

                if (retVal)
                {
                    DRMData dRMData = new DRMData();
                    retVal = DecodeDRMStr(drmInfo, true, out dRMData, out lastError);

                    if (retVal)
                    {
                        e.EncryptionKey = Utils.ConvertHexStrToByteArray(dRMData.EncryptionKey);
                        e.IV = Utils.ConvertHexStrToByteArray(dRMData.EncryptionIV);

                        e.ReturnStatus = NtStatus.Status.Success;

                        lastError = "Get file " + e.FileName + " permission from server succeed.";
                        EventManager.WriteMessage(310, "GetFilePermission-DecryptDRMString", EventLevel.Information, lastError);
                    }
                    else
                    {
                        lastError = "Decode returned DRMData failed." + lastError;
                        EventManager.WriteMessage(300, "DecryptDRMString", EventLevel.Error, lastError);
                        e.ReturnStatus = NtStatus.Status.AccessDenied;
                    }
                }
                else
                {
                    lastError = "Get file " + e.FileName + " permission from server failed with error:" + lastError;

                    EventManager.WriteMessage(305, "GetFilePermission", EventLevel.Error, lastError);
                    e.ReturnStatus = NtStatus.Status.AccessDenied;
                }
            }
            catch (Exception ex)
            {
                lastError = "GetFileAccessPermission exception." + ex.Message;
                retVal = false;

                EventManager.WriteMessage(315, "GetFilePermission", EventLevel.Error, "Get file " + e.FileName + " permission from server failed with error:" + lastError);
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }
            finally
            {
                lock (userAccessCacheTable)
                {
                    //add the access permission to cache table.
                    if (!userAccessCacheTable.ContainsKey(e.ProcessId.ToString() + tagDataStr))
                    {
                        CacheUserAccessInfo userAccessInfo = new CacheUserAccessInfo();
                        userAccessInfo.processId = e.ProcessId;
                        userAccessInfo.iv = e.IV; ;
                        userAccessInfo.key = e.EncryptionKey;
                        userAccessInfo.status = e.ReturnStatus;
                        userAccessCacheTable[e.ProcessId.ToString() + tagDataStr] = userAccessInfo;

                        EventManager.WriteMessage(360, "GetFilePermission", EventLevel.Information,
                            "Get file " + e.FileName + " permission from server, add it to cache, returnStatus:" + e.ReturnStatus.ToString() + ",ivStr:" + tagDataStr);
                    }
                }
            }

            return retVal;

        }

        static public bool AddDRMDataToServer(string fileName,
                                    string authorizedProcessNames,
                                    string unauthorizedProcessNames,
                                    string authorizedUserNames,
                                    string unauthorizedUserNames,
                                    string authorizedComputerIds,
                                    string authorizedIps,
                                    long expireTime,
                                    string encryptIV,
                                    string encryptKey,
                                    uint accessFlags,
                                    out string lastError)
        {
            lastError = string.Empty;

            try
            {
                
                DRMData dRM = new DRMData();

                dRM.EmailAccount = GlobalConfig.AccountName;
                dRM.ComputerId = RegisterForm.GetUniqueComputerId().ToString();
                dRM.FileName = fileName;
                dRM.EncryptionKey = encryptKey;
                dRM.EncryptionIV = encryptIV;
                dRM.AccessFlags = accessFlags;
                dRM.AuthorizedProcessNames = authorizedProcessNames;
                dRM.UnauthorizedProcessNames = unauthorizedProcessNames;
                dRM.AuthorizedUserNames = authorizedUserNames;
                dRM.UnauthorizedUserNames = unauthorizedUserNames;
                dRM.AuthorizedComputerIds = authorizedComputerIds;
                dRM.AuthorizedIps = authorizedIps;
                dRM.ExpireTime = expireTime;
                dRM.CreationTime = DateTime.Now.ToFileTime();
                string drmInfo = EncodeDRMData(dRM);

                if(GlobalConfig.IsDRMDataInLocal)
                {
                    return LocalClient.AddDRMDataToServer(drmInfo, out lastError);
                }

                ServiceReference1.SecureShareSoapClient client = GetServiceClient(ref lastError);
                if (null == client)
                {
                    return false;
                }

                return client.AddDRMDataToServer(drmInfo, out lastError);

            }
            catch (Exception ex)
            {
                lastError = "AddDRMDataToServer exception:" + ex.Message;
                return false;
            }

        }

        static public bool ModifyDRMDataFromServer(string fileName,
                                    string authorizedProcessNames,
                                    string unauthorizedProcessNames,
                                    string authorizedUserNames,
                                    string unauthorizedUserNames,
                                    string authorizedComputerIds,
                                    string authorizedIps,
                                    long expireTime,
                                    string encryptIV,
                                    string encryptKey,
                                    uint accessFlags,
                                    out string lastError)
        {
            lastError = string.Empty;

            try
            {
               
                DRMData dRM = new DRMData();

                dRM.EmailAccount = GlobalConfig.AccountName;
                dRM.ComputerId = RegisterForm.GetUniqueComputerId().ToString();
                dRM.FileName = fileName;
                dRM.EncryptionKey = encryptKey;
                dRM.EncryptionIV = encryptIV;
                dRM.AccessFlags = accessFlags;
                dRM.AuthorizedProcessNames = authorizedProcessNames;
                dRM.UnauthorizedProcessNames = unauthorizedProcessNames;
                dRM.AuthorizedUserNames = authorizedUserNames;
                dRM.UnauthorizedUserNames = unauthorizedUserNames;
                dRM.AuthorizedComputerIds = authorizedComputerIds;
                dRM.AuthorizedIps = authorizedIps;
                dRM.ExpireTime = expireTime;
                dRM.CreationTime = DateTime.Now.ToFileTime();
                string encrypteDRMData = EncodeDRMData(dRM);

                if (GlobalConfig.IsDRMDataInLocal)
                {
                    return LocalClient.ModifyDRMDataFromServer(encrypteDRMData, out lastError);
                }

                ServiceReference1.SecureShareSoapClient client = GetServiceClient(ref lastError);
                if (null == client)
                {
                    return false;
                }

                return client.ModifyDRMDataFromServer(encrypteDRMData, out lastError);

            }
            catch (Exception ex)
            {
                lastError = "ModifyDRMDataFromServer exception:" + ex.Message;
                return false;
            }
        }

        static public bool DeleteDRMDataFromServer(
                                 string encryptIV,
                                 out string lastError)
        {
            lastError = string.Empty;

            try
            {

                string emailAccount = GlobalConfig.AccountName;
                string computerId = RegisterForm.GetUniqueComputerId().ToString();
                string clientInfo = emailAccount + ";" + computerId + ";" + encryptIV;

                if (GlobalConfig.IsDRMDataInLocal)
                {
                    return LocalClient.DeleteDRMDataFromServer(clientInfo, out lastError);
                }

                ServiceReference1.SecureShareSoapClient client = GetServiceClient(ref lastError);
                if (null == client)
                {
                    return false;
                }

                return client.DeleteDRMDataFromServer(clientInfo, out lastError);

            }
            catch (Exception ex)
            {
                lastError = "ModifyDRMDataFromServer exception:" + ex.Message;
                return false;
            }
        }

        static public bool GetSharedFileList(string emailAccount, ListView listView_SharedFiles, out string lastError)
        {
            lastError = string.Empty;
            string sharedFileList = string.Empty;

            try
            {
                string computerId = RegisterForm.GetUniqueComputerId().ToString();
                string clientInfo = emailAccount + ";" + computerId;
                bool retVal = false;

                if (GlobalConfig.IsDRMDataInLocal)
                {
                    retVal = LocalClient.GetSharedFileList(clientInfo, out sharedFileList, out lastError);
                }
                else
                {
                    ServiceReference1.SecureShareSoapClient client = GetServiceClient(ref lastError);
                    if (null == client)
                    {
                        return false;
                    }

                    retVal = client.GetSharedFileList(clientInfo, out sharedFileList, out lastError);
                }

                if (retVal)
                {
                    listView_SharedFiles.Items.Clear();
                    string shareFiles = DecodeFrom64(sharedFileList);

                    string[] logs = shareFiles.Split(new char[] { '\n' });

                    foreach (string log in logs)
                    {
                        if (log.Trim().Length == 0)
                        {
                            continue;
                        }

                        DRMData drm = new DRMData();
                        if (DecodeDRMStr(log, false, out drm, out lastError))
                        {
                            ListViewItem lvItem = new ListViewItem();

                            string[] listData = new string[listView_SharedFiles.Columns.Count];
                            int col = 0;
                            listData[col++] = (listView_SharedFiles.Items.Count + 1).ToString();
                            listData[col++] = drm.FileName;
                            listData[col++] = DateTime.FromFileTime(drm.CreationTime).ToString("yyyy-MM-ddTHH:mm");
                            listData[col++] = DateTime.FromFileTime(drm.ExpireTime).ToString("yyyy-MM-ddTHH:mm");
                            listData[col++] = drm.AuthorizedProcessNames;
                            listData[col++] = drm.UnauthorizedProcessNames;
                            listData[col++] = drm.AuthorizedUserNames;
                            listData[col++] = drm.UnauthorizedUserNames;

                            lvItem = new ListViewItem(listData, 0);
                            lvItem.Tag = drm;


                            listView_SharedFiles.Items.Add(lvItem);
                        }
                        else
                        {
                            return false;
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                lastError = "GetSharedFileList exception:" + ex.Message;
                return false;
            }
        }

        static public bool GetAccessLog(ListView listView_AccessLog, out string lastError)
        {
            lastError = string.Empty;            

            try
            {                
                string computerId = RegisterForm.GetUniqueComputerId().ToString();
                string emailAccount = GlobalConfig.AccountName;
                string clientInfo = emailAccount + ";" + computerId;
                string accessLogHex = string.Empty;
                bool retVal = false;

                if (GlobalConfig.IsDRMDataInLocal)
                {
                    retVal = LocalClient.GetAccessLog(clientInfo, out accessLogHex, out lastError);
                }
                else
                {
                    ServiceReference1.SecureShareSoapClient client = GetServiceClient(ref lastError);
                    if (null == client)
                    {
                        return false;
                    }

                    retVal = client.GetAccessLog(clientInfo, out accessLogHex, out lastError);
                }

                if (retVal)
                {
                    listView_AccessLog.Items.Clear();

                    string accessLog = DecodeFrom64(accessLogHex);

                    //DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + ";" + emailAccount + ";"
                    //         + processName + ";" + userName + ";" + computerName + ";" + iP + ";" + status + ";" + filename;


                    string[] logs = accessLog.Split(new char[] { '\n' });

                    foreach (string log in logs)
                    {
                        string[] accessInfo = log.Split(new char[] { ';' });

                        if (accessInfo.Length < 8)
                        {
                            continue;
                        }

                        string[] listData = new string[listView_AccessLog.Columns.Count];
                        int col = 0;
                        listData[col++] = (listView_AccessLog.Items.Count + 1).ToString();
                        listData[col++] = accessInfo[0];
                        listData[col++] = accessInfo[6];
                        listData[col++] = accessInfo[1];
                        listData[col++] = accessInfo[2];
                        listData[col++] = accessInfo[3];
                        listData[col++] = accessInfo[4];
                        listData[col++] = accessInfo[5];

                        if (accessInfo[7].Trim().Length == 0)
                        {
                            listData[col++] = "Succeed";
                        }
                        else
                        {
                            listData[col++] = "Denied: " + accessInfo[7];
                        }

                        ListViewItem lvItem = new ListViewItem(listData, 0);

                        if (accessInfo[7].Trim().Length > 0)
                        {
                            lvItem.BackColor = System.Drawing.Color.LightGray;
                            lvItem.ForeColor = System.Drawing.Color.Red;
                        }

                        listView_AccessLog.Items.Add(lvItem);
                    }
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                lastError = "GetAccessLog exception:" + ex.Message;
                return false;
            }
        }
    }
}
