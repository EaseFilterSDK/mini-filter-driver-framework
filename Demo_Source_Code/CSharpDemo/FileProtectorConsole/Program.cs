using System;
using EaseFilter.FilterControl;

namespace FileProtectorConsole
{
    class Program
    {
        static FilterControl filterControl = new FilterControl();

        static void Main(string[] args)
        {
            string lastError = string.Empty;
            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string licenseKey = "**************************************";

            FilterAPI.FilterType filterType = FilterAPI.FilterType.MONITOR_FILTER|FilterAPI.FilterType.CONTROL_FILTER
                |FilterAPI.FilterType.PROCESS_FILTER|FilterAPI.FilterType.REGISTRY_FILTER|FilterAPI.FilterType.ENCRYPTION_FILTER;

            int serviceThreads = 5;
            int connectionTimeOut = 10; //seconds

            try
            {
                if (!filterControl.StartFilter(filterType, serviceThreads, connectionTimeOut, licenseKey, ref lastError))
                {
                    Console.WriteLine("Start Filter Service failed with error:" + lastError);
                    return;
                }

                //the watch path can use wildcard to be the file path filter mask.i.e. '*.txt' only monitor text file.
                string watchPath = "c:\\test\\*";

                if (args.Length > 0)
                {
                    watchPath = args[0];
                }

                //create a file protector filter rule, every filter rule must have the unique watch path. 
                FileFilter fileProtectorFilter = new FileFilter(watchPath);

                //configure the access right for the protected folder

                //prevent the file from being deleted.
                fileProtectorFilter.EnableDeleteFile = false;

                //prevent the file from being renamed.
                fileProtectorFilter.EnableRenameOrMoveFile = false;

                //prevent the file from being written.
                fileProtectorFilter.EnableWriteToFile = false;

                //authorize process with full access right
                fileProtectorFilter.AddTrustedProcessRight(FilterAPI.ALLOW_MAX_RIGHT_ACCESS, "notepad.exe", "", "");


                //set the access rights for the specific user, here is to remove the delete and rename rights.
                uint accessRights = FilterAPI.ALLOW_MAX_RIGHT_ACCESS & (uint)(~(FilterAPI.AccessFlag.ALLOW_FILE_DELETE | FilterAPI.AccessFlag.ALLOW_FILE_RENAME));
                fileProtectorFilter.UserAccessRightList.Add("domainname or computer\\username", accessRights);

                //you can enable/disalbe more access right by setting the properties of the fileProtectorFilter.

                //Filter the callback file IO events, here get callback before the file was opened/created, and file was deleted.
                fileProtectorFilter.ControlFileIOEventFilter = (ControlFileIOEvents.OnPreFileCreate | ControlFileIOEvents.OnPreDeleteFile);

                fileProtectorFilter.OnPreCreateFile += OnPreCreateFile;
                fileProtectorFilter.OnPreDeleteFile += OnPreDeleteFile;

                filterControl.AddFilter(fileProtectorFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    Console.WriteLine("SendConfigSettingsToFilter failed." + lastError);
                    return;
                }

                Console.WriteLine("Start filter service succeeded.");

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;

                filterControl.StopFilter();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Start filter service failed with error:" + ex.Message);
            }

        }

        /// <summary>
        /// Fires this event before the file was opened. 
        /// </summary>
        static void OnPreCreateFile(object sender, FileCreateEventArgs e)
        {
            Console.WriteLine("OnPreCreateFile:" + e.FileName + ",userName:" + e.UserName + ",processName:" + e.ProcessName);

            //you can block the file open here by returning below status.
            e.ReturnStatus = NtStatus.Status.AccessDenied;

        }

        /// <summary>
        /// Fires this event before the file was deleted.
        /// </summary>
        static void OnPreDeleteFile(object sender, FileIOEventArgs e)
        {
            Console.WriteLine("OnPreDeleteFile:" + e.FileName  + ",userName:" + e.UserName + ",processName:" + e.ProcessName);

            //you can block the file being deleted here by returning below status.
            e.ReturnStatus = NtStatus.Status.AccessDenied;
        }
    }
}
