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
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

using EaseFilter.FilterControl;
using EaseFilter.CommonObjects;

namespace AutoFileCryptTool
{
    public class FilterDriverService
    {

        static FilterControl filterControl = new FilterControl();

        public static bool SendConfigSettingsToFilter(ref string lastError)
        {
            filterControl.ClearFilters();
            foreach (FileFilterRule filterRule in GlobalConfig.FilterRules.Values)
            {
                FileFilter fileFilter = filterRule.ToFileFilter();
                filterControl.AddFilter(fileFilter);
            }

            if (!filterControl.SendConfigSettingsToFilter(ref lastError))
            {
                return false;
            }

            return true;
        }

        public static bool StartFilterService(out string lastError)
        {
            //Purchase a license key with the link: http://www.easefilter.com/Order.htm
        //Email us to request a trial key: info@easefilter.com //free email is not accepted.
            string licenseKey = GlobalConfig.licenseKey;
            GlobalConfig.filterType = FilterAPI.FilterType.ENCRYPTION_FILTER;

            bool ret = false;

            lastError = string.Empty;

            try
            {
                ret = filterControl.StartFilter(GlobalConfig.filterType, GlobalConfig.FilterConnectionThreads, GlobalConfig.ConnectionTimeOut, licenseKey, ref lastError);
                if (!ret)
                {
                    return ret;
                }

                ret = SendConfigSettingsToFilter(ref lastError);

                EventManager.WriteMessage(102, "StartFilter", EventLevel.Information, "Start filter service succeeded.");
            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(104, "StartFilter", EventLevel.Error, "Start filter service failed with error " + ex.Message);
                ret = false;
            }

            return ret;
        }

        public static bool StopService()
        {           
            GlobalConfig.Stop();
            filterControl.StopFilter();

            EventManager.WriteMessage(102, "StopFilter", EventLevel.Information, "Stopped filter service succeeded.");

            return true;
        }

    }
}
