using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

using EaseFilter.CommonObjects;
using EaseFilter.FilterControl;

namespace  StubFileDemo
{
    public class FilterWorker
    {
        public static ListView listView_Info = null;
        static FilterControl filterControl = new FilterControl();

        public static bool StartService(ref string lastError)
        {
            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
            //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string licenseKey = GlobalConfig.LicenseKey;

            FilterAPI.FilterType filterType = FilterAPI.FilterType.HSM_FILTER;
            bool ret = false;

            try
            {

                if(!filterControl.StartFilter(filterType, (int)GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError))
                {
                    return false;
                }

                filterControl.ExcludeProcessIdList = GlobalConfig.ExcludePidList;


                //create a file monitor filter rule, every filter rule must have the unique watch path. 
                FileFilter fileFilter = new FileFilter(GlobalConfig.TestStubFolder + "\\*");

                fileFilter.BooleanConfig |= (uint)FilterAPI.BooleanConfig.ENABLE_STUB_FILE_HEADER;

                fileFilter.OnFilterStubFileRead += OnStubFileRequestHandler;

                filterControl.AddFilter(fileFilter);

                if (!filterControl.SendConfigSettingsToFilter(ref lastError))
                {
                    EventManager.WriteMessage(50, "SendConfigSettingsToFilter", EventLevel.Error, lastError);
                    return false;
                }

                ret = true;

            }
            catch (Exception ex)
            {
                lastError = "Start filter service failed with error " + ex.Message;
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, lastError);
                ret = false;
            }

            return ret;
        }

        public static bool StopService()
        {
            GlobalConfig.Stop();
            filterControl.StopFilter();

            return true;
        }

        static void OnStubFileRequestHandler(object sender, StubFileEventArgs e)
        {

            try
            {

                //here the data buffer is the reparse point tag data, in our test, we added the test source file name of the stub file as the tag data.
                string cacheFileName = Encoding.Unicode.GetString(e.TagData);
                //remove the extra data of the file name.
                cacheFileName = cacheFileName.Substring(0, (int)e.TagDataLength / 2);

                if (e.DownloadFullFileToCache)
                {
                    //for the write request, the filter driver needs to restore the whole file first,
                    //here we need to download the whole cache file and return the cache file name to the filter driver,
                    //the filter driver will replace the stub file data with the cache file data.

                    //for memory mapped file open( for example open file with notepad in local computer )
                    //it also needs to download the whole cache file and return the cache file name to the filter driver,
                    //the filter driver will read the cache file data, but it won't restore the stub file.

                    e.ReturnCacheFileName = cacheFileName;

                    //if you want to rehydrate the stub file, please return with REHYDRATE_FILE_VIA_CACHE_FILE
                    if (GlobalConfig.RehydrateFileOnFirstRead)
                    {
                        e.FilterStatus = FilterAPI.FilterStatus.REHYDRATE_FILE_VIA_CACHE_FILE;
                    }
                    else
                    {
                        e.FilterStatus = FilterAPI.FilterStatus.CACHE_FILE_WAS_RETURNED;
                    }

                    e.ReturnStatusToFilter = NtStatus.Status.Success;
                }
                else if (e.DownloadBockData)
                {

                    e.ReturnCacheFileName = cacheFileName;

                    //for this request, the user is trying to read block of data, you can either return the whole cache file
                    //or you can just restore the block of data as the request need, you also can rehydrate the file at this point.

                    //if you want to rehydrate the stub file, please return with REHYDRATE_FILE_VIA_CACHE_FILE
                    if (GlobalConfig.RehydrateFileOnFirstRead)
                    {
                        e.FilterStatus = FilterAPI.FilterStatus.REHYDRATE_FILE_VIA_CACHE_FILE;
                    }
                    else if (GlobalConfig.ReturnCacheFileName)
                    {
                        e.FilterStatus = FilterAPI.FilterStatus.CACHE_FILE_WAS_RETURNED;
                    }
                    else
                    {
                        //we return the block the data back to the filter driver.
                        FileStream fs = new FileStream(cacheFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                        fs.Position = e.ReadOffset;

                        int returnReadLength = fs.Read(e.ReturnBuffer, 0, (int)e.ReadLength);
                        e.ReturnBufferLength = (uint)returnReadLength;

                        e.FilterStatus = FilterAPI.FilterStatus.BLOCK_DATA_WAS_RETURNED;

                        fs.Close();

                    }

                    e.ReturnStatusToFilter = NtStatus.Status.Success;
                }
                else
                {
                    EventManager.WriteMessage(140, "ProcessRequest", EventLevel.Error, "File " + e.FileName + " messageType can't be handled.");
                    e.ReturnStatusToFilter = NtStatus.Status.Unsuccessful;

                }

                DisplayMessage(e);

                //EventManager.WriteMessage(158, "ProcessRequest", EventLevel.Information, "Return MessageId#" + e.MessageId
                //         + " ReturnStatus:" + ((e.ReturnStatus)).ToString() + ",FilterStatus:" + e.FilterStatus
                //         + ",ReturnLength:" + e.ReturnBufferLength + " fileName:" + e.FileName + ",cacheFileName:" + e.ReturnCacheFileName);

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(181, "ProcessRequest", EventLevel.Error, "Process request exception:" + ex.Message);
            }

        }

        static public void InitListView()
        {
            if (null != listView_Info)
            {
                //init ListView control
                listView_Info.Clear();		//clear control
                //create column header for ListView
                listView_Info.Columns.Add("#", 40, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("Time", 50, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("UserName", 150, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("ProcessName(PID)", 120, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("ThreadId", 70, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("MessageType", 160, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("FileName", 200, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("FileSize", 50, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("FileAttributes", 100, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("LastWriteTime", 60, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("Offset", 50, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("Length", 50, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("ReturnStatus", 100, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("ReturnFilterStatus", 150, System.Windows.Forms.HorizontalAlignment.Left);
                listView_Info.Columns.Add("ReturnBufferLength", 100, System.Windows.Forms.HorizontalAlignment.Left);
            }
        }

        static public void DisplayMessage(StubFileEventArgs e)
        {

            string[] item = new string[listView_Info.Columns.Count];
            item[0] = e.MessageId.ToString();
            item[1] = FormatDateTime(DateTime.Now.ToFileTime());
            item[2] = e.UserName;
            item[3] = e.ProcessName + "(" + e.ProcessId.ToString() + ")";
            item[4] = e.ThreadId.ToString();
            item[5] = ((FilterAPI.FilterCommand)e.filterCommand).ToString();
            item[6] = e.FileName;
            item[7] = e.FileSize.ToString();
            item[8] = ((FileAttributes)e.FileAttributes).ToString();
            item[9] = FormatDateTime(e.LastWriteTime);
            item[10] = e.ReadOffset.ToString();
            item[11] = e.ReadLength.ToString();
            item[12] = e.ReturnStatus.ToString();
            item[13] = e.FilterStatus.ToString();
            item[14] = e.ReturnBufferLength.ToString();

            ListViewItem lvItem = new ListViewItem(item, 0);

            listView_Info.Items.Add(lvItem);

            if (listView_Info.Items.Count > 0 && listView_Info.Items.Count > GlobalConfig.MaximumFilterMessages)
            {
                //the message records in the list view reached to the maximum value, remove the first one till the record less than the maximum value.
                listView_Info.Items.RemoveAt(0);
            }

            listView_Info.EnsureVisible(listView_Info.Items.Count - 1);

        }


        static string FormatDateTime(long lDateTime)
        {
            try
            {
                if (0 == lDateTime)
                {
                    return "0";
                }

                DateTime dateTime = DateTime.FromFileTime(lDateTime);
                string ret = dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
                return ret;
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(502, "FormatDateTime", EventLevel.Error, "FormatDateTime :" + lDateTime.ToString() + " failed." + ex.Message);
                return ex.Message;
            }
        }
    }
}
