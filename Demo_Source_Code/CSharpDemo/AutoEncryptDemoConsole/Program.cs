using System;
using System.Collections.Generic;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace AutoEncryptDemoConsole
{  

    class Program
    {
        static FilterControl filterControl = new FilterControl();
        //the process list which can read the encrypted files.
        static string authorizedProcess = "notepad.exe;wordpad.exe";

        static void PrintUsage()
        {
            Console.WriteLine("\r\nUsage: AutoEncryptDemoConsole folderName authorizedProcesses DRM");
            Console.WriteLine("Example:\r\nAutoEncryptDemoConsole  c:\\test\\*   notepad.exe;wordpad.exe  DRM");
            Console.WriteLine("folderName           c:\\test\\*");
            Console.WriteLine("authorizedProcesses  notepad.exe;wordpad.exe");
            Console.WriteLine("DRM                  it is optional,enable DRM if it is DRM.\r\n");
        }

        static void Main(string[] args)
        {
            string lastError = string.Empty;
            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string licenseKey = "****************************************************";            

            FilterAPI.FilterType filterType = FilterAPI.FilterType.CONTROL_FILTER | FilterAPI.FilterType.ENCRYPTION_FILTER | FilterAPI.FilterType.PROCESS_FILTER | FilterAPI.FilterType.MONITOR_FILTER;
            int serviceThreads = 5;
            int connectionTimeOut = 10; //seconds

            try
            {
                //the watch path can use wildcard to be the file path filter mask.i.e. '*.txt' only monitor text file.
                string watchPath = "c:\\test\\*";

                if (args.Length > 0)
                {
                    watchPath = args[0];
                }
                else
                {
                    PrintUsage();
                    return;
                }

                //the process list which can read the encrypted files.
                if (args.Length > 1)
                {
                    authorizedProcess = args[1];
                }
                else
                {
                    PrintUsage();
                    return;
                }

                bool isDRMEnabled = false;
                if (args.Length > 2 && args[2].ToLower().CompareTo("drm") == 0)
                {
                    isDRMEnabled = true;
                }

                licenseKey = GlobalConfig.LicenseKey;

                if (!filterControl.StartFilter(filterType, serviceThreads, connectionTimeOut, licenseKey, ref lastError))
                {
                    Console.WriteLine("Start service failed with error:" + lastError + "\r\n");
                    return;
                }              

                //create a file monitor filter rule, every filter rule must have the unique watch path. 
                FileFilter fileFilter = new FileFilter(watchPath);

                //Filter the file change event to monitor all file deleting and renaming events.
                fileFilter.FileChangeEventFilter = FilterAPI.FileChangedEvents.NotifyFileWasDeleted | FilterAPI.FileChangedEvents.NotifyFileWasRenamed;

                //register the file change callback events.
                fileFilter.NotifyFileWasChanged += NotifyFileChanged;

                //enabled the auto file encryption in the watch folder.
                fileFilter.EnableEncryption = true;
                //It is optional to set the encrypted file attribute,if it was set,
                //the encrypted attribute will be kept even it was copied out to another folder without the encryption.
                fileFilter.BooleanConfig |= (uint)FilterAPI.BooleanConfig.ENABLE_SET_FILE_ATTRIBUTE_ENCRYPTED;

                if (isDRMEnabled)
                {
                    //if we enable the encryption key from service, you can authorize the encryption or decryption for every file
                    //in the callback function OnFilterRequestEncryptKey.
                    fileFilter.EnableEncryptionKeyFromService = true;
                    fileFilter.OnFilterRequestEncryptKey += OnFilterRequestEncryptKey;
                }
                else
                {
                    //setup the encryption with the key for every file and unique iv per file. this method has better preformance, it doesn't require the callback, 
                    //you just need to setup the policies for the filter driver who can read the encrypted file.

                    //get the 256bits encryption key with the passphrase
                    fileFilter.EncryptionKey = Utils.GetKeyByPassPhrase("myTestPassPharse", 32);

                    //allow everyone to read the encrypted data by default, except you remove it from the process access right.
                    //fileFilter.EnableReadEncryptedData = true;

                    //disable the decryption right, read the raw encrypted data for all except the authorized processes or users.
                    fileFilter.EnableReadEncryptedData = false;

                    //if by default all processes can't read the encrypted files, then we need to set the authorized process list here.
                    if (!fileFilter.EnableReadEncryptedData)
                    {
                        List<string> whiteListProcess = new List<string>();
                        string[] processNamesToDecrypt = authorizedProcess.Trim().Split(new char[] { ';' });
                        if (processNamesToDecrypt.Length > 0)
                        {
                            foreach (string processName in processNamesToDecrypt)
                            {
                                whiteListProcess.Add(processName);
                            }
                        }

                        foreach (string processName in whiteListProcess)
                        {
                            //authorized the process, i.e."notepad.exe" with the read encrypted data right.
                            fileFilter.AddTrustedProcessRight(FilterAPI.ALLOW_MAX_RIGHT_ACCESS, processName, "", "");

                        }
                    }

                }

                //add the encryption file filter rule to the filter control
                filterControl.AddFilter(fileFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    Console.WriteLine("SendConfigSettingsToFilter failed with error:" + lastError);
                    return ;
                }

                Console.WriteLine("Start filter service succeeded.\r\nMonitoring path:" + watchPath + "\r\nauthorizedProcessList:" + authorizedProcess + "\r\nisDRMEnabled:" + isDRMEnabled.ToString());
                Console.WriteLine("\r\nHow to test? Copy files to folder " + watchPath + ", the new created files will be encyrypted automatically.");

                // Wait for the user to quit the program.
                Console.WriteLine("\r\nPress any key quit the demo.\r\n");
                Console.Read();

                filterControl.StopFilter();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Start filter service failed with error:" + ex.Message);
            }

        }

        /// <summary>
        /// Fires this event when the encrypted file requests the encryption key.
        /// </summary>
        static public void OnFilterRequestEncryptKey(object sender, EncryptEventArgs e)
        {
            e.ReturnStatus = NtStatus.Status.Success;

            try
            {
                if (e.IsNewCreatedFile)
                {
                    byte[] iv = Guid.NewGuid().ToByteArray();
                    //for the new created file, you can add your custom tag data to the header of the encyrpted file here.
                    //here we add the iv to the tag data.
                    e.EncryptionTag = iv;

                    //if you don't set the iv data, the filter driver will generate the new GUID as iv 
                    e.IV = iv;

                    //here is the encryption key for the new encrypted file, you can set it with your own custom key.
                    e.EncryptionKey = Utils.GetKeyByPassPhrase("myTestPassPharse", 32);

                    //if you want to block the new file creation, you can return access denied status.
                    //e.ReturnStatus = NtStatus.Status.AccessDenied;

                    //if you want to the file being created without encryption, return below status.
                    //e.ReturnStatus = NtStatus.Status.FileIsNoEncrypted;  

                    Console.WriteLine("\r\nNew created file:" + e.FileName + " was encrypted. The process name:" + e.ProcessName + ",user name:" + e.UserName);
                }
                else
                {
                    //this is the encrypted file open request, request the encryption key and iv.
                    //here is the tag data if you set custom tag data when the new created file requested the key.
                    byte[] tagData = e.EncryptionTag;

                    //The encryption key must be the same one which you created the new encrypted file.
                    e.EncryptionKey = Utils.GetKeyByPassPhrase("myTestPassPharse", 32);

                    //here is the iv key we saved in tag data.
                    e.IV = tagData;

                    //if you want to block encrypted file being opened, you can return accessdenied status.
                    //e.ReturnStatus = NtStatus.Status.AccessDenied;

                    //if you want to return the raw encrypted data for this encrypted file, return below status.
                    //e.ReturnStatus = NtStatus.Status.FileIsEncrypted;

                    if (authorizedProcess.Contains(e.ProcessName))
                    {
                        e.ReturnStatus = NtStatus.Status.Success;
                        Console.WriteLine("Decrypted file:" + e.FileName + ",userName:" + e.UserName + ",processName:" + e.ProcessName);
                    }
                    else
                    {
                        //return the raw encrypted data for this encrypted file.
                        e.ReturnStatus = NtStatus.Status.FileIsEncrypted;

                        Console.ForegroundColor = ConsoleColor.Red;                        
                        Console.WriteLine("\r\nDecrypted file:" + e.FileName + " IS NOT ALLOWED for userName:" + e.UserName + ",processName:" + e.ProcessName + ", raw encrypted data was returned.\r\n");
                        Console.ResetColor();
                    }

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("\r\nOnFilterRequestEncryptKey:" + e.FileName + ",got exeception:" + ex.Message);
                e.ReturnStatus = NtStatus.Status.AccessDenied;
            }

        }

        /// <summary>
        /// Fires this event when the file was changed.
        /// </summary>
        static void NotifyFileChanged(object sender, FileChangedEventArgs e)
        {
            Console.WriteLine("NotifyFileChanged:" + e.FileName + "\r\neventType:" + e.eventType.ToString() + "\r\nuserName:" + e.UserName + ",processName:" + e.ProcessName + "\r\n" + e.Description);
        }
    }
}
