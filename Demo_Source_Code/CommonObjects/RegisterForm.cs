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
using System.Net.Mail;
using System.Text.RegularExpressions;

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{
    public partial class WebFormServices : Form
    {

        static bool getUpdate = false;
        
        public static string serverURI = "http://www.easefilter.com/WebService.asmx";

        public bool IsVerificationCodeSent = false;
        static public string computerId = string.Empty;
        public string emailAccount = string.Empty;
      
        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern uint GetComputerId();

        [DllImport("FilterAPI.dll", SetLastError = true)]
        public static extern bool ActivateLicense(
                IntPtr outputBuffer,
                uint outputBufferLength);

        public WebFormServices()
        {
            InitializeComponent();

            if (!LoadHardWareId())
            {
                return;
            }
        }

        public static uint GetUniqueComputerId()
        {
            try
            {
                uint computerId = GetComputerId();

                if (0 == computerId)
                {
                    string lastError = FilterAPI.GetLastErrorMessage();
                    return 0;
                }

                return computerId;
            }
            catch (Exception ex)
            {
                string lastError = "Get computerId got exception,system return error:" + ex.Message;
                return 0;
            }

        }

        public static bool IsEmailValid(string email)
        {
            var valid = true;

            try
            {
                var emailAddress = new MailAddress(email);
            }
            catch
            {
                valid = false;
            }

            return valid;
        }

        public static bool IsLicenseValid(string activatedLicense)
        {
            bool retVal = false;

            if (activatedLicense.Length > 0)
            {
                byte[] encryptedLocalLicense = Convert.FromBase64String(activatedLicense);
                IntPtr encryptedLocalLicensePtr = Marshal.UnsafeAddrOfPinnedArrayElement(encryptedLocalLicense, 0);

                retVal = ActivateLicense(encryptedLocalLicensePtr, (uint)encryptedLocalLicense.Length);

            }

            return retVal;

        }

        private bool LoadHardWareId()
        {
            bool retVal = false;
           
            try
            {
                computerId = Environment.MachineName + "-" + GetUniqueComputerId().ToString();
            }
            catch (Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Get license information failed." + ex.Message, "Get license info", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return retVal;
        }

        // Return the OS name.
        private static string GetOsName()
        {
            string version = Utils.WinMajorVersion().ToString() + '.' + Utils.WinMinorVersion().ToString();
            switch (version)
            {
                case "10.0": return "10/Server 2016";
                case "6.3": return "8.1/Server 2012 R2";
                case "6.2": return "8/Server 2012";
                case "6.1": return "7/Server 2008 R2";
                case "6.0": return "Server 2008/Vista";
                case "5.2": return "Server 2003 R2/Server 2003/XP 64-Bit Edition";
                case "5.1": return "XP";
                case "5.0": return "2000";
            }
            return "Unknown";
        }

        private static string GetCurrentFileInfo()
        {
            string currentInfo = string.Empty;

            try
            {
                string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                string hostName = System.Net.Dns.GetHostEntry(Environment.MachineName).HostName;
                string processName = Environment.GetCommandLineArgs()[0];
                string version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                string filterType = GlobalConfig.filterType.ToString();
                string totalSec = "ElapseTime:" + (GlobalConfig.stopWatch.ElapsedMilliseconds / 1000).ToString();
                string verdate = "CreateDate:" + File.GetCreationTime(processName).ToShortDateString();

                string uninst = Path.GetDirectoryName(processName) + "\\uninst.exe";

                if (File.Exists(uninst))
                {
                    verdate += ",InstallDate:" + File.GetLastWriteTime(uninst).ToShortDateString();
                }
                else
                {
                    verdate += ",DownloadDate:" + File.GetLastWriteTime(processName).ToShortDateString();
                }

                try
                {
                    FileInfo fileInfo = new FileInfo(processName);
                    long fileSize = fileInfo.Length;
                    verdate += ";size:" + fileSize + ",isFilterStarted:" + FilterControl.FilterControl.IsStarted;
                }
                catch
                {

                }


                currentInfo = "OS:" + GetOsName() + ";" + totalSec + ";" + userName + ";" + hostName + ";" + processName + ";" + version + ";" + verdate + ";" + FilterControl.FilterControl.licenseKey;

                currentInfo = Utils.AESEncryptDecryptStr(currentInfo, Utils.EncryptType.Encryption);
            }
            catch (Exception ex)
            {
               // EventManager.WriteMessage(43, "GetCurrentFileInfo", EventLevel.Warning, "Get crurrent file info failed with error:" + ex.Message);
            }

            return currentInfo;
        }

        public static ServiceReference1.ServiceSoapClient GetServiceClient(ref string lastError)
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

                client = new EaseFilter.CommonObjects.ServiceReference1.ServiceSoapClient(binding, endpoint);
            }
            catch (Exception ex)
            {
                lastError = "Connecting to server failed with error " + ex.Message; 
            }

            return client;
        }

        private static void AysncGetUpdatedInfo()
        {
            string updatedInfo = string.Empty;

            if (getUpdate)
            {
                return;
            }

            getUpdate = true;

            try
            {
                string lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if (client != null)
                {
                    int actionLevel = 0;
                    string currentInfo = GetCurrentFileInfo();

                    updatedInfo = client.GetUpdatedInfo(currentInfo, ref actionLevel);

                    if (actionLevel == 1 && updatedInfo.Trim().Length > 0)
                    {
                        MessageBox.Show(updatedInfo, "Update Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (actionLevel == 2)
                    {
                        //this is invalid license.
                        GlobalConfig.LicenseKey = "*************************";
                    }
                }
            }
            catch (Exception ex)
            {
               // EventManager.WriteMessage(145, "AysncGetUpdatedInfo", EventLevel.Verbose, "Get crurrent file info failed with error:" + ex.Message);
            }

        }

        public static void GetUpdatedInfo()
        {
         //   System.Threading.Tasks.Task.Factory.StartNew(() => { AysncGetUpdatedInfo(); });
            AysncGetUpdatedInfo();
            return;

        }

        private bool IsValidEmail(string email)
        {
            string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";
            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            return regex.IsMatch(email);
        }

        private void button_RegisterLicense_Click(object sender, EventArgs e)
        {
            emailAccount = textBox_Email.Text.Trim();

            if (string.IsNullOrEmpty(emailAccount) || !IsValidEmail(emailAccount))
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("It is not a valid email address.", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            GlobalConfig.AccountName = emailAccount;

            ActivateLicense();
        }    

        public bool ActivateLicense()
        {
            string activatedLicense = "";

            try
            {
                string lastError = string.Empty;
                ServiceReference1.ServiceSoapClient client = GetServiceClient(ref lastError);

                if( null == client )
                {
                    return IsVerificationCodeSent;
                }

                if (computerId.Length == 0)
                {
                    return IsVerificationCodeSent;
                }

                long expireTime = 0;

                if (client.RegisterDemoApp(emailAccount, 0, computerId, GlobalConfig.AssemblyName, out activatedLicense, out expireTime, out lastError))
                {
                    if (string.Compare(activatedLicense, "verification") == 0)
                    {
                        //verification required.
                        IsVerificationCodeSent = true;
                    }
                    else
                    {
                        GlobalConfig.LicenseKey = activatedLicense;
                        GlobalConfig.ExpireTime = expireTime;
                    }
                }
                else
                {
                    MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                    MessageBox.Show(lastError, "Activation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                //label_ActivatedLicense.Text = activatedLicense;

            }
            catch( Exception ex)
            {
                MessageBoxHelper.PrepToCenterMessageBoxOnForm(this);
                MessageBox.Show("Activate demo app failed," + ex.Message + ", please contact support@easefilter.com", "Activation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            return IsVerificationCodeSent;
           
        }
      
    }
}
