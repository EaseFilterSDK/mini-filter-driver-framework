using System;
using System.IO;
using System.Text.RegularExpressions;

using EaseFilter.CommonObjects;

namespace SecureShare
{
    public class LocalClient
    {
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

        static bool GetDRMData(string drmInfoHexStr, bool isEncoded, out DRMData dRM, out string drmInfo, out string lastError)
        {
            dRM = new DRMData();
            lastError = string.Empty;
            bool retVal = false;
            drmInfo = drmInfoHexStr;

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
                lastError += "GetDRMData got exception:" + ex.Message + ",drmInfo:" + drmInfo + ",drmInfoHexStr:" + drmInfoHexStr;
                retVal = false;
            }

            return retVal;

        }

        //[WebMethod]
        static public bool AddDRMDataToServer(string drmHexStr, out string lastError)
        {
            bool retVal = false;
            lastError = string.Empty;
            string sharedFileLog = GlobalConfig.DRMFolder + "\\sharedFileLog.txt";

            try
            {
                DRMData drm = new DRMData();
                string drmInfo = string.Empty;
                if (!GetDRMData(drmHexStr, true, out drm, out drmInfo, out lastError))
                {
                    return false;
                }

                if (drm.EmailAccount.Length == 0)
                {
                    lastError = "The email account can't be empty.";
                    return false;
                }

                string dir = Path.GetDirectoryName(sharedFileLog);

                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(sharedFileLog);
                }

                if (File.Exists(sharedFileLog))
                {
                    string[] sharedFiles = File.ReadAllLines(sharedFileLog);
                    for (int i = 0; i < sharedFiles.Length; i++)
                    {
                        if (sharedFiles[i].IndexOf(drm.EncryptionIV) > 0)
                        {
                            lastError = "The file already exist, please delete or modify the old file.";
                            return false;
                        }
                    }

                }

                File.AppendAllText(sharedFileLog, drmInfo + Environment.NewLine);


                retVal = true;

            }
            catch (Exception ex)
            {
                lastError = "AddDRMDataToServer got exception:" + ex.Message;
            }

            return retVal;

        }

        //[WebMethod]
        static public bool ModifyDRMDataFromServer(string drmHexStr, out string lastError)
        {
            bool retVal = false;
            lastError = string.Empty;
            string sharedFileLog = GlobalConfig.DRMFolder + "\\sharedFileLog.txt";

            try
            {
                DRMData drmNew = new DRMData();
                string drmInfo = string.Empty;

                if (!GetDRMData(drmHexStr, true, out drmNew, out drmInfo, out lastError))
                {
                    return false;
                }
            
                string[] sharedFiles = File.ReadAllLines(sharedFileLog);

                bool isFileExist = false;

                for (int i = 0; i < sharedFiles.Length; i++)
                {
                    if (sharedFiles[i].IndexOf(drmNew.EncryptionIV) > 0)
                    {
                        isFileExist = true;
                        sharedFiles[i] = drmInfo;
                        break;
                    }
                }

                if (isFileExist)
                {
                    File.WriteAllLines(sharedFileLog, sharedFiles);
                    retVal = true;
                }
                else
                {
                    retVal = false;
                    lastError = " shared file DRM data doesn't exist in server.";
                }

            }
            catch (Exception ex)
            {
                lastError = "ModifyDRMDataFromServer got exception:" + ex.Message;               
            }

            return retVal;

        }

        //[WebMethod]
        static public bool DeleteDRMDataFromServer(string ClientInfo, out string lastError)
        {
            bool retVal = false;
            lastError = string.Empty;
            string sharedFileLog = GlobalConfig.DRMFolder + "\\sharedFileLog.txt";

            try
            {
                string[] infos = ClientInfo.Split(new char[] { ';' });

                if (infos.Length < 3)
                {
                    lastError = "Missed verification data." + ClientInfo;
                    return false;
                }

                string emailAccount = infos[0];
                string computerId = infos[1];
                string iv = infos[2];

                bool isFileExist = false;              
                if (!File.Exists(sharedFileLog))
                {
                    lastError = "there is no shared file in server.";
                    return false;
                }

                string[] sharedFiles = File.ReadAllLines(sharedFileLog);
                string newSharedFiles = string.Empty;

                for (int i = 0; i < sharedFiles.Length; i++)
                {
                    if (sharedFiles[i].Trim().Length == 0)
                    {
                        continue;
                    }

                    DRMData dRMData = new DRMData();
                    string drmInfo = string.Empty;
                    if (GetDRMData(sharedFiles[i], false, out dRMData, out drmInfo, out lastError))
                    {
                        if (dRMData.EncryptionIV != iv)
                        {
                            newSharedFiles += sharedFiles[i] + Environment.NewLine;
                        }
                        else
                        {
                            isFileExist = true;
                        }

                    }
                }

                if (isFileExist)
                {
                    File.WriteAllText(sharedFileLog, newSharedFiles);
                    retVal = true;
                }
                else
                {
                    lastError += "Shared file DRM doesn't exist in the server." + iv;
                    return false;
                }

            }
            catch (Exception ex)
            {
                lastError = "DeleteDRMDataFromServer got exception:" + ex.Message;              
            }

            return retVal;

        }

       // [WebMethod]
        static public bool GetSharedFileList(string ClientInfo, out string sharedFiles, out string lastError)
        {
            bool retVal = false;
            lastError = string.Empty;
            string sharedFileLog = GlobalConfig.DRMFolder + "\\sharedFileLog.txt";

            string emailAccount = string.Empty;
            string computerId = string.Empty;

            sharedFiles = string.Empty;

            try
            {
                string[] infos = ClientInfo.Split(new char[] { ';' });

                if (infos.Length < 2)
                {
                    lastError = "Missed verification data." + ClientInfo;
                    return false;
                }

                emailAccount = infos[0];
                computerId = infos[1];

            
                if (!File.Exists(sharedFileLog))
                {
                    lastError = "There is no shared file for this account " + emailAccount;
                    return false;
                }

                sharedFiles = File.ReadAllText(sharedFileLog);
                sharedFiles = EncodeTo64(sharedFiles);

                retVal = true;
            }
            catch (Exception ex)
            {
                lastError = "GetSharedFileList got exception:" + ex.Message;               
            }

            return retVal;
        }


        //[WebMethod]
        static public bool GetAccessLog(string ClientInfo, out string accessLog, out string lastError)
        {
            bool retVal = false;
            lastError = string.Empty;            
            string accessLogFile = GlobalConfig.DRMFolder + "\\accessLog.log";

            string emailAccount = string.Empty;
            string computerId = string.Empty;

            accessLog = string.Empty;

            try
            {
                string[] infos = ClientInfo.Split(new char[] { ';' });

                if (infos.Length < 2)
                {
                    lastError = "Missed verification data." + ClientInfo;
                    return false;
                }

                emailAccount = infos[0];
                computerId = infos[1];

                if (!File.Exists(accessLogFile))
                {
                    lastError = "There is no access acitivities for this account.";
                    return false;
                }

                accessLog = EncodeTo64((File.ReadAllText(accessLogFile)));

                retVal = true;
            }
            catch (Exception ex)
            {
                lastError = "AddDRInfoToServer got exception:" + ex.Message;
            }

            return retVal;
        }

        static bool IsNameMatch(string Name, string NameMask, bool matchLastProcessName)
        {
            string name = Name.ToLower();
            string[] nameMasks = NameMask.ToLower().Split(new char[] { ';' });

            foreach (string nameMask in nameMasks)
            {
                if (nameMask.Length > 0)
                {
                    if (matchLastProcessName)
                    {
                        if (name.ToLower().EndsWith(nameMask.ToLower()))
                        {
                            //name="c:\windows\notepad.exe" nameMask= "notepad.exe";
                            return true;
                        }
                    }

                    // Escape regex special characters except for * and ?
                    string regexPattern = "^" + Regex.Escape(nameMask)
                                               .Replace("\\*", ".*")
                                               .Replace("\\?", ".") + "$";

                    bool isMatch = Regex.IsMatch(Name, regexPattern, RegexOptions.IgnoreCase);

                    if (isMatch)
                    {
                        return true;
                    }
                }

            }

            return false;
        }

        //[WebMethod]
        static public bool GetFilePermission(string ClientInfo, out string drmInfo, out string lastError)
        {
            bool retVal = false;
            lastError = string.Empty;
            string sharedFileLog = GlobalConfig.DRMFolder + "\\sharedFileLog.txt";
            string accessLogFile = GlobalConfig.DRMFolder + "\\accessLog.log";
            string accessInfo = string.Empty;
            string emailAccount = string.Empty;
            string iv = string.Empty;
            string computerId = string.Empty;

            drmInfo = string.Empty;

            try
            {
                string[] infos = ClientInfo.Split(new char[] { ';' });

                if (infos.Length < 8)
                {
                    lastError = "Missed verification data." + ClientInfo;
                    return false;
                }

                emailAccount = infos[0];
                computerId = infos[1];
                iv = infos[2];
                string processName = infos[3];
                string userName = infos[4];
                string computerName = infos[5];
                string fileName = infos[6];
                string iP = infos[7];

                //DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + ";" + emailAccount + ";" + computerName + ";" + iP + ";" + userName + processName + ";"  + ";" 
                accessInfo = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss") + ";" + emailAccount + ";" + computerName + ";" + iP + ";" + userName + ";" + processName + ";" + fileName + ";";

                bool isFileExist = false;
                string[] sharedFiles = File.ReadAllLines(sharedFileLog);
                DRMData drm = new DRMData();

                for (int i = 0; i < sharedFiles.Length; i++)
                {
                    string drmHexStr = sharedFiles[i];
                    string drmStr = string.Empty;

                    if (!GetDRMData(drmHexStr, false, out drm, out drmStr, out lastError))
                    {
                        return false;
                    }

                    if (drm.EncryptionIV == iv)
                    {
                        drmInfo = EncodeTo64(drmStr);
                        isFileExist = true;
                        break;
                    }

                }

                if (!isFileExist)
                {
                    lastError = "the DRM file doesn't exist in server.";
                    return false;
                }

                if (drm.ExpireTime > 0)
                {
                    DateTime currentTime = DateTime.Now;
                    DateTime exTime = DateTime.FromFileTimeUtc(drm.ExpireTime);

                    if (currentTime > exTime)
                    {
                        lastError = "the file is expired";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(drm.AuthorizedProcessNames))
                {
                    if (!IsNameMatch(processName, drm.AuthorizedProcessNames, true))
                    {
                        lastError = "the process " + processName + " is not authorized to decrypt the file.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(drm.UnauthorizedProcessNames))
                {
                    if (IsNameMatch(processName, drm.UnauthorizedProcessNames, true))
                    {
                        lastError = "the process " + processName + " is not authorized to decrypt the file.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(drm.AuthorizedUserNames))
                {
                    if (!IsNameMatch(userName, drm.AuthorizedUserNames, false))
                    {
                        lastError = "the user " + userName + " is not authorized to decrypt the file.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(drm.UnauthorizedUserNames))
                {
                    if (IsNameMatch(userName, drm.UnauthorizedUserNames, false))
                    {
                        lastError = "the user " + userName + " is not authorized to decrypt the file.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(drm.AuthorizedComputerIds))
                {
                    if (drm.AuthorizedComputerIds.IndexOf(computerId.Trim()) < 0)
                    {
                        lastError = "the computerId " + computerId + " is not authorized to decrypt the file.";
                        return false;
                    }
                }

                if (!string.IsNullOrEmpty(drm.AuthorizedIps))
                {
                    if (drm.AuthorizedUserNames.IndexOf(iP.Trim()) < 0)
                    {
                        lastError = "the Ip " + iP + " is not authorized to decrypt the file.";
                        return false;
                    }
                }

                retVal = true;

            }
            catch (Exception ex)
            {
                lastError = "AddDRInfoToServer got exception:" + ex.Message;
            }
            finally
            {
                try
                {
                    if (retVal)
                    {
                        accessInfo += Environment.NewLine;
                    }
                    else
                    {
                        accessInfo += lastError + Environment.NewLine;
                    }

                    File.AppendAllText(accessLogFile, accessInfo);
                  
                }
                catch (Exception ex)
                {
                }
            }

            return retVal;

        }
    }
}
