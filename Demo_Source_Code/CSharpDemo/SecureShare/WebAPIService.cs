using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Configuration;
using System.ServiceModel;
using System.Diagnostics;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace SecureShare
{
    public static class WebAPIServices 
    {
        public static string serverURI = "http://www.encryptme.net/WebAPI.asmx";
        static string accountName = GlobalConfig.AccountName;
        static string password = "testpassword";

        static WebAPIServices()
        {
            if (accountName.Trim().Length == 0)
            {
                accountName = "testaccount." + Environment.MachineName + "." + Guid.NewGuid().ToString();
                GlobalConfig.AccountName = accountName;
            }

            accountName = Utils.AESEncryptDecryptStr(accountName, Utils.EncryptType.Encryption);
        }


        private static ServiceReference1.ServiceSoapClient GetServiceClient(ref string lastError)
        {
            ServiceReference1.ServiceSoapClient client = null;

            try
            {
                BasicHttpBinding binding = new BasicHttpBinding();
                binding.OpenTimeout = new TimeSpan(0, 0, 10);
                binding.CloseTimeout = new TimeSpan(0, 0, 10);
                binding.SendTimeout = new TimeSpan(0, 0, 10);
                binding.ReceiveTimeout = new TimeSpan(0, 0, 10);

                EndpointAddress endpoint = new EndpointAddress(new Uri(serverURI));

                client = new SecureShare.ServiceReference1.ServiceSoapClient(binding, endpoint);
            }
            catch (Exception ex)
            {
                lastError = "Connecting to server failed with error " + ex.Message; 
            }

            return client;
        }

        public static bool AddShareFile(string policy,ref long creationTime,ref string key,ref string iv, ref string lastError)
        {
            bool retVal = false;

            try
            {

                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

                retVal = client.AddShareFile(accountName, password, policy, ref creationTime, ref key, ref iv, ref lastError);
            }
            catch (Exception ex)
            {
                lastError = "Add new file to server failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }

        public static bool GetSharedFilePermission(  string fileName, string processName, string userName, string tagStr, ref string iv, ref string key, ref uint accessFlags, ref string lastError)
        {
            bool retVal = false;

            try
            {

                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

                string ivStr = tagStr;
                string serverAccount = GlobalConfig.AccountName;
                int index = tagStr.IndexOf(";");
                if ( index > 0)
                {
                    serverAccount = tagStr.Substring(0, index);
                    ivStr = tagStr.Substring(index + 1);
                }

                UserInfo userInfo = new UserInfo();
                string keyStr = string.Empty;

                userInfo.FileName =fileName;
                userInfo.AccountName = serverAccount;
                userInfo.ProcessName = processName;
                userInfo.UserName = userName;
                userInfo.UserPassword = password;
                userInfo.CreationTime = DateTime.Now.ToFileTime(); ;
                userInfo.EncryptionIV = ivStr;

                string userInfoStr = Utils.EncryptObjectToStr<UserInfo>(userInfo);

                retVal = client.GetSharedFilePermission(userInfoStr, ref key, ref iv, ref accessFlags, ref lastError);
            }
            catch (Exception ex)
            {
                lastError = "Get file key failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }

        public static bool GetFileDRInfo( string encryptionIV, ref string encryptedDRPolicy, ref string lastError)
        {
            bool retVal = false;

            try
            {

                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

               retVal = client.GetSharedFileDRInfo(accountName, password, encryptionIV, ref encryptedDRPolicy, ref lastError);

            }
            catch (Exception ex)
            {
                lastError = "Get file DR info failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }

        public static bool ModifySharedFileDRInfo(string encryptedDRPolicy, ref string lastError)
        {
            bool retVal = false;

            try
            {

                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

                retVal = client.ModifySharedFileDRInfo(accountName, password, encryptedDRPolicy, ref lastError);

            }
            catch (Exception ex)
            {
                lastError = "Set file DR info failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }

        public static bool DeleteShareFile( string encryptionIV, ref string lastError)
        {
            bool retVal = false;

            try
            {

                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

                retVal = client.DeleteSharedFile(accountName, password, encryptionIV, ref lastError);

            }
            catch (Exception ex)
            {
                lastError = "DeleteShareFile failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }


        public static bool GetFileList( ref string fileListStr, ref string lastError)
        {
            bool retVal = false;

            try
            {          
                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

                retVal = client.GetFileList(accountName, password, ref fileListStr, ref lastError);

            }
            catch (Exception ex)
            {
                lastError = "Get file list failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }

        public static bool GetAccessLog(ref string accessLog, ref string lastError)
        {
            bool retVal = false;

            try
            {

                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

                retVal = client.GetAccessLog(accountName, password, ref accessLog, ref lastError);
            }
            catch (Exception ex)
            {
                lastError = "Get access log failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }

        public static bool ClearAccessLog(ref string lastError)
        {
            bool retVal = false;

            try
            {

                lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (null == client)
                {
                    return false;
                }

                 retVal = client.ClearAccessLog(accountName, password, ref lastError);
            }
            catch (Exception ex)
            {
                lastError = "Clear access log failed with error:" + ex.Message;
                retVal = false;
            }

            return retVal;
        }
    }
}
