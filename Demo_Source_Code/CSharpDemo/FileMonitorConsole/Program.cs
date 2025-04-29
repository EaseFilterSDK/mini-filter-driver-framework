using System;

using EaseFilter.FilterControl;

namespace FileMonitorConsole
{
    class Program
    {
        static FilterControl filterControl = new FilterControl();

        static void Main(string[] args)
        {
            string lastError = string.Empty;
            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string licenseKey =  "****************************************************";
            
            FilterAPI.FilterType filterType = FilterAPI.FilterType.MONITOR_FILTER;
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
                string watchPath = "*";

                if (args.Length > 0)
                {
                    watchPath = args[0];
                }

                //create a file monitor filter rule, every filter rule must have the unique watch path. 
                FileFilter fileFilter = new FileFilter(watchPath);

                //Filter the file change event to monitor all file deleting and renaming events.
                fileFilter.FileChangeEventFilter = FilterAPI.FileChangedEvents.NotifyFileWasDeleted| FilterAPI.FileChangedEvents.NotifyFileWasRenamed|FilterAPI.FileChangedEvents.NotifyFileWasCopied;

                //register the file change callback events.
                fileFilter.NotifyFileWasChanged += NotifyFileChanged;

                //Filter the monitor file IO events
                fileFilter.MonitorFileIOEventFilter = MonitorFileIOEvents.OnFileCreate | MonitorFileIOEvents.OnFileRead;
                fileFilter.OnFileOpen += OnFileCreate;
                fileFilter.OnFileRead += OnFileRead;
               
                fileFilter.EnableSendReadOrWriteBuffer = true;

                filterControl.AddFilter(fileFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    Console.WriteLine("SendConfigSettingsToFilter failed." + lastError);
                    return;
                }

                Console.WriteLine("Start filter service succeeded. Monitoring path:" + watchPath);

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
        /// Fires this event when the file was changed.
        /// </summary>
        static void NotifyFileChanged(object sender, FileChangedEventArgs e)
        {
            Console.WriteLine("NotifyFileChanged:" + e.FileName + ",eventType:" + e.eventType.ToString() + ",userName:" + e.UserName + ",processName:" + e.ProcessName + "\r\n" + e.Description);
        }

        /// <summary>
        /// Fires this event when the file was read.
        /// </summary>
        static void OnFileCreate(object sender, FileCreateEventArgs e)
        {
            Console.WriteLine("FileCreateEventArgs:" + e.FileName + ",userName:" + e.UserName + ",processName:" + e.ProcessName + "\r\n" + e.Description);
        }

        /// <summary>
        /// Fires this event when the file was read.
        /// </summary>
        static void OnFileRead(object sender, FileReadEventArgs e)
        {
            Console.WriteLine("FileReadEventArgs:" + e.FileName + ",userName:" + e.UserName + ",processName:" + e.ProcessName + "\r\n" + e.Description);
        }
    }
}
