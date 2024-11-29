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

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System;


namespace EaseFilter.CommonObjects
{
    public enum OutputMessage : uint
    {
        IO_Name = 0x00000000,
        Process_Name,
        File_Name,
        File_Size,
        Attributes,
        Last_Write_Time,
        IO_Status,
        Description,

    }

    public interface IFilterService
    {
        void Notify(string text);
        bool AddFilterRule(string includeFileFilterMask, uint eventType, uint monitorIO, uint accessFlags);
        bool DeleteFilterRule(string includeFileFilterMask);
    }

    public class Cache
    {
        private static Cache myInstance;
        public static IFilterService iFilterService;

        private Cache()
        {

        }

        public static void Attach(IFilterService observer)
        {
            iFilterService = observer;
        }
        public static Cache GetInstance()
        {
            if (myInstance == null)
            {
                myInstance = new Cache();
            }
            return myInstance;
        }

        public string MessageString
        {
            set
            {
                iFilterService.Notify(value);
            }
        }

        public  bool AddFilterRule(string includeFileFilterMask, uint eventType, uint monitorIO, uint accessFlags)
        {
            return iFilterService.AddFilterRule(includeFileFilterMask, eventType, monitorIO, accessFlags);
        }

        public bool DeleteFilterRule(string includeFileFilterMask)
        {
            return iFilterService.DeleteFilterRule(includeFileFilterMask);
        }
    }

    public class FilterRemoteObject : MarshalByRefObject
    {

        public FilterRemoteObject()
        {

        }

        public void Notify(string text)
        {
            Cache.GetInstance().MessageString = text;
        }

        public bool AddFilterRule(string includeFileFilterMask, uint eventType, uint monitorIO, uint accessFlags)
        {
           return  Cache.GetInstance().AddFilterRule(includeFileFilterMask, eventType, monitorIO, accessFlags);
        }

        public bool DeleteFilterRule(string includeFileFilterMask)
        {
            return Cache.GetInstance().DeleteFilterRule(includeFileFilterMask);
        }

    }

    public class RemoteServer : IFilterService
    {
        private FilterRemoteObject remoteObject;
        TcpChannel channel = new TcpChannel(65118);

        public RemoteServer()
        {
        }

        public void Start()
        {
            try
            {
                remoteObject = new FilterRemoteObject();               
                ChannelServices.RegisterChannel(channel,false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(FilterRemoteObject), "filterAPIs", WellKnownObjectMode.Singleton);
                Cache.Attach(this);
            }
            catch (Exception ex)
            {
                throw new Exception("Create remote server failed." + ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                ChannelServices.UnregisterChannel(channel);
            }
            catch { }
        }

        public void Notify(string text)
        {
            Console.WriteLine("Receive txt:" + text);
        }

        public bool AddFilterRule(string includeFileFilterMask, uint eventType, uint monitorIO, uint accessFlags)
        {
            bool retVal = false;
            try
            {

                FilterRule filterRule = new FilterRule();

                filterRule.IncludeFileFilterMask = includeFileFilterMask;
                filterRule.EventType = eventType;
                filterRule.MonitorIO = monitorIO;
                filterRule.AccessFlag = accessFlags;
                filterRule.ExcludeProcessNames = "explorer.exe";
                filterRule.Id = GlobalConfig.GetFilterRuleId();

                if ((filterRule.AccessFlag & (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) == (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING)
                {
                    filterRule.HiddenFileFilterMasks = "*";
                }

                if ((filterRule.AccessFlag & (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE) == (uint)FilterAPI.AccessFlag.ENABLE_FILE_ENCRYPTION_RULE)
                {
                    filterRule.EncryptMethod = (int)FilterAPI.EncryptionMethod.ENCRYPT_FILE_WITH_SAME_KEY_AND_IV;
                    filterRule.EncryptionPassPhrase = "easefilter";
                }

                Console.WriteLine("Receive AddFilterRule command,includeFileFilterMask:" + includeFileFilterMask + ",eventType:(0x)" + eventType.ToString("x")
                    + ",monitorIO:(0x)" + monitorIO.ToString("x") + ",accessFlags:(0x)" + accessFlags.ToString("x"));

                retVal = GlobalConfig.AddFilterRule(filterRule);

                if (!retVal)
                {
                    FilterAPI.GetLastErrorMessage();
                    EventManager.WriteMessage(43, "AddFilterRule", EventLevel.Error, "AddFilterRule " + filterRule.IncludeFileFilterMask + " failed,filter returned:" + FilterAPI.GetLastErrorMessage());
                    return false;
                }

                GlobalConfig.SendConfigSettingsToFilter();
                EventManager.WriteMessage(43, "AddFilterRule", EventLevel.Verbose, "AddFilterRule " + filterRule.IncludeFileFilterMask + " succeeded.");

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(43, "AddFilterRule", EventLevel.Error, "AddFilterRule " + includeFileFilterMask + " failed,system reported:" + ex.Message);
                return false;
            }

            return true;
        }

        public bool DeleteFilterRule(string includeFileFilterMask)
        {
            try
            {
                GlobalConfig.RemoveFilterRule(includeFileFilterMask);
                GlobalConfig.SendConfigSettingsToFilter();

                Console.WriteLine("Receive DeleteFilterRule command,includeFileFilterMask:" + includeFileFilterMask);

                EventManager.WriteMessage(43, "RemoveFilterRule", EventLevel.Verbose, "RemoveFilterRule " + includeFileFilterMask + " succeeded.");

            }
            catch (Exception ex)
            {
                EventManager.WriteMessage(43, "RemoveFilterRule", EventLevel.Error, "RemoveFilterRule " + includeFileFilterMask + " failed,system reported:" + ex.Message);
                return false;
            }

            return true;
        }
    }


    public class RemoteClient
    {

        FilterRemoteObject remoteObject = null;
        TcpChannel chan = new TcpChannel();

        public RemoteClient()
        {
            try
            {                
                ChannelServices.RegisterChannel(chan,false);
                remoteObject = (FilterRemoteObject)Activator.GetObject(typeof(FilterRemoteObject), "tcp://localhost:65118/filterAPIs");
            }
            catch (Exception ex)
            {
                throw new Exception("Create remoteClient object failed." + ex.Message);
            }
        }

        public void Stop()
        {
            try
            {
                ChannelServices.UnregisterChannel(chan);
            }
            catch { }
        }

        public FilterRemoteObject IRemoteServer
        {
            get
            {
                return remoteObject;
            }
        }
    }

  
}
