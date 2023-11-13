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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Globalization;

using EaseFilter.FilterControl;

namespace EaseFilter.CommonObjects
{
    public partial class OptionForm : Form
    {
        OptionType optionType = OptionType.MonitorFileIOEvents;
        string value = string.Empty;

        uint monitorFileEvents = 0;
        ulong monitorIOEvents = 0;
        ulong controlIOEvents = 0;
        string processId = "0";
        uint accessFlags = 0;
        uint debugModules = 0xffff;
        uint filterStatus = 0;
        uint returnStatus = 0;
        uint registryAccessFlags = 0;
        ulong registryCallbackClass = 0;
        byte filterType = 0;
        uint processControlFlag = 0;
        uint filterDesiredAccess = 0;
        uint filterDisposition = 0;
        uint filterCreateOptions = 0;

        public bool isMonitorFilter = false;

        public enum OptionType
        {
            MonitorFileEvents = 0,
            MonitorFileIOEvents,
            ControlFileIOEvents,
            ProccessId,
            Access_Flag,
            Filter_Status,
            Return_Status,            
            ShareFileAccessFlag,
            RegistryAccessFlag,
            RegistryCallbackClass,
            FilterType,
            ProcessControlFlag,
            FilterDesiredAccess,
            FilterDisposition,
            FilterCreateOptions
        }

        public OptionForm(OptionType formType, string defaultValue, bool _isMoitorFilter)
        {
            this.optionType = formType;
            this.value = defaultValue;
            this.isMonitorFilter = _isMoitorFilter;

            InitializeComponent();
            InitForm();
        }

        public OptionForm(OptionType formType, string defaultValue)
        {
            this.optionType = formType;
            this.value = defaultValue;

            InitializeComponent();
            InitForm();
        }

        public uint FilterCreateOptions
        {
            get { return filterCreateOptions; }
        }

        public uint FilterDisposition
        {
            get { return filterDisposition; }
        }

        public uint FilterDesiredAccess
        {
            get { return filterDesiredAccess; }
        }

        public uint ProcessControlFlag
        {
            get { return processControlFlag; }
        }

        public byte FilterType
        {
            get { return filterType; }
        }

        public uint MonitorFileEvents
        {
            get { return monitorFileEvents; }
        }

        public uint FilterStatus
        {
            get { return filterStatus; }
        }
        public uint ReturnStatus
        {
            get { return returnStatus; }
        }

        public ulong MonitorIOEvents
        {
            get { return monitorIOEvents; }
        }

        public ulong ControlIOEvents
        {
            get { return controlIOEvents; }
        }

        public string ProcessId
        {
            get { return processId; }
        }


        public uint AccessFlags
        {
            get { return accessFlags; }
        }

        public uint DebugModules
        {
            get { return debugModules; }
        }

        public uint RegAccessFlags
        {
            get { return registryAccessFlags; }
        }

        public ulong RegCallbackClass
        {
            get { return registryCallbackClass; }
        }


        void InitForm()
        {
            this.Text = optionType.ToString();

            switch (optionType)
            {
                case OptionType.MonitorFileEvents:
                    {
                        listView1.Clear();	
                        //create column header for ListView
                        listView1.Columns.Add("Select monitor file event type", 200, System.Windows.Forms.HorizontalAlignment.Left);

                        monitorFileEvents = uint.Parse(value);

                        foreach (FilterAPI.FileChangedEvents monitorFileEvent in Enum.GetValues(typeof(FilterAPI.FileChangedEvents)))
                        {

                            string item = monitorFileEvent.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = (uint)monitorFileEvent;

                            if ((monitorFileEvents & (uint)monitorFileEvent) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

                case OptionType.MonitorFileIOEvents:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select monitor I/O events", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        monitorIOEvents = ulong.Parse(value);

                        foreach (MonitorFileIOEvents monitorFileIOevent in Enum.GetValues(typeof(MonitorFileIOEvents)))
                        {
                            string item = monitorFileIOevent.ToString();
                            
                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = (ulong)monitorFileIOevent;

                            if ((monitorIOEvents & (ulong)monitorFileIOevent) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }
                case OptionType.ControlFileIOEvents:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select control I/O events", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        controlIOEvents = ulong.Parse(value);

                        foreach (ControlFileIOEvents controlFileIOevent in Enum.GetValues(typeof(ControlFileIOEvents)))
                        {
                            string item = controlFileIOevent.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = (ulong)controlFileIOevent;

                            if ((controlIOEvents & (ulong)controlFileIOevent) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

                case OptionType.RegistryAccessFlag:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Registry Access Control Flag", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        registryAccessFlags = uint.Parse(value);

                        foreach (FilterAPI.RegControlFlag regAccessFlag in Enum.GetValues(typeof(FilterAPI.RegControlFlag)))
                        {
                            string item = regAccessFlag.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = regAccessFlag;

                            if ((registryAccessFlags & (uint)regAccessFlag) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

                case OptionType.RegistryCallbackClass:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Registry Callback Class", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        registryCallbackClass = ulong.Parse(value);

                        foreach (FilterAPI.RegCallbackClass regCallbackClass in Enum.GetValues(typeof(FilterAPI.RegCallbackClass)))
                        {
                            string item = regCallbackClass.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = regCallbackClass;

                            if ((registryCallbackClass & (ulong)regCallbackClass) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }
                case OptionType.ProccessId:
                    {
                        Process[] processlist = Process.GetProcesses();

                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Process Id", 100, System.Windows.Forms.HorizontalAlignment.Left);
                        listView1.Columns.Add("Process Name", 300, System.Windows.Forms.HorizontalAlignment.Left);

                        List<uint> pidList = new List<uint>();

                        string[] pids = value.Split(';');
                        foreach (string pid in pids)
                        {
                            if (!string.IsNullOrEmpty(pid))
                            {
                                pidList.Add(uint.Parse(pid));
                            }
                        }


                        for (int i = 0; i < processlist.Length; i++)
                        {
                            string[] item = new string[2];
                            item[0] = processlist[i].Id.ToString();
                            item[1] = processlist[i].ProcessName;

                            if (processlist[i].Id == 0)
                            {
                                //this is idle process, skip it.
                                continue;
                            }

                            ListViewItem lvItem = new ListViewItem(item, 0);

                            lvItem.Tag = processlist[i].Id;

                            if (pidList.Contains((uint)(processlist[i].Id)))
                            {
                                lvItem.Checked = true;
                            }

                            if (i > 0 )
                            {
                                for (int k = 0; k < i; k++)
                                {
                                    if ((int)listView1.Items[k].Tag > processlist[i].Id)
                                    {
                                        listView1.Items.Insert(k, lvItem);
                                        break;
                                    }
                                }

                                if (listView1.Items.Count == i )
                                {
                                    listView1.Items.Insert(i, lvItem);
                                }
                                
                            }
                            else
                            {
                                listView1.Items.Insert(i, lvItem);
                            }

                        }

                        break;
                    }

                case OptionType.Access_Flag:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select AccessFlag", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        accessFlags = uint.Parse(value);

                        foreach (FilterAPI.AccessFlag accessFlag in Enum.GetValues(typeof(FilterAPI.AccessFlag)))
                        {
                            if (accessFlag <= FilterAPI.AccessFlag.EXCLUDE_FILE_ACCESS || accessFlag == FilterAPI.AccessFlag.LEAST_ACCESS_FLAG)
                            {
                                //this is special usage for the filter 
                                continue;
                            }

                            string item = accessFlag.ToString();
                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = accessFlag;

                            if (((uint)accessFlag & accessFlags) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

                case OptionType.ShareFileAccessFlag:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select AccessFlag", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        accessFlags = uint.Parse(value);

                        foreach (FilterAPI.AccessFlag accessFlag in Enum.GetValues(typeof(FilterAPI.AccessFlag)))
                        {
                            if (accessFlag < FilterAPI.AccessFlag.ALLOW_OPEN_WTIH_ACCESS_SYSTEM_SECURITY || accessFlag == FilterAPI.AccessFlag.LEAST_ACCESS_FLAG)
                            {
                                //this is special usage for the filter 
                                continue;
                            }

                            string item = accessFlag.ToString();
                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = accessFlag;

                            if (((uint)accessFlag & accessFlags) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }


                case OptionType.Filter_Status:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Filter Status", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        filterStatus = uint.Parse(value);

                        foreach (FilterAPI.FilterStatus status in Enum.GetValues(typeof(FilterAPI.FilterStatus)))
                        {
                            string item = status.ToString();
                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = status;

                            if (((uint)status & filterStatus) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

                case OptionType.Return_Status:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Only One Status", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        returnStatus = uint.Parse(value);

                        foreach (NtStatus.Status status in Enum.GetValues(typeof(NtStatus.Status)))
                        {
                            string item = status.ToString();
                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = status;

                            if (((uint)status & filterStatus) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

                case OptionType.FilterType:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Filter Driver Type", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        filterType = byte.Parse(value);

                        foreach (FilterAPI.FilterType fltType in Enum.GetValues(typeof(FilterAPI.FilterType)))
                        {
                            string item = fltType.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = fltType;

                            if ((filterType & (byte)fltType) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

                case OptionType.ProcessControlFlag:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Process Control Flag", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        processControlFlag = uint.Parse(value);

                        foreach (FilterAPI.ProcessControlFlag controlFlag in Enum.GetValues(typeof(FilterAPI.ProcessControlFlag)))
                        {
                            string item = controlFlag.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = controlFlag;

                            if ((processControlFlag & (uint)controlFlag) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }
                case OptionType.FilterDesiredAccess:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Filter DesiredAccess Flag", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        filterDesiredAccess = uint.Parse(value);

                        foreach (WinData.DisiredAccess desiredAccess in Enum.GetValues(typeof(WinData.DisiredAccess)))
                        {
                            string item = "(0x" + ((uint)desiredAccess).ToString("X") + ")" +  desiredAccess.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = desiredAccess;

                            if ((filterDesiredAccess & (uint)desiredAccess) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }
                case OptionType.FilterDisposition:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Filter Disposition Flag", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        filterDisposition = uint.Parse(value);

                        foreach (WinData.Disposition dispostion in Enum.GetValues(typeof(WinData.Disposition)))
                        {
                            string item = "(0x" + ((uint)dispostion).ToString("X") + ")" + dispostion.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = dispostion;

                            if ((filterDisposition & (uint)dispostion) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }
                case OptionType.FilterCreateOptions:
                    {
                        listView1.Clear();		//clear control
                        //create column header for ListView
                        listView1.Columns.Add("Select Filter CreateOptions Flag", 400, System.Windows.Forms.HorizontalAlignment.Left);

                        filterCreateOptions = uint.Parse(value);

                        foreach (WinData.CreateOptions createOptions in Enum.GetValues(typeof(WinData.CreateOptions)))
                        {
                            string item = "(0x" + ((uint)createOptions).ToString("X") + ")" + createOptions.ToString();

                            ListViewItem lvItem = new ListViewItem(item, 0);
                            lvItem.Tag = createOptions;

                            if ((filterCreateOptions & (uint)createOptions) > 0)
                            {
                                lvItem.Checked = true;
                            }

                            listView1.Items.Add(lvItem);
                        }

                        break;
                    }

            }
        }

        private void button_Ok_Click(object sender, EventArgs e)
        {
            monitorFileEvents = 0;
            monitorIOEvents = 0;
            controlIOEvents = 0;

            processId = string.Empty;
            accessFlags = 0;
            debugModules = 0;            
            registryAccessFlags = 0;
            registryCallbackClass = 0;
            filterType = 0;
            processControlFlag = 0;
            filterDesiredAccess = 0;
            filterDisposition = 0;
            filterCreateOptions = 0;

            foreach (ListViewItem item in listView1.CheckedItems)
            {
                switch (optionType)
                {
                    case OptionType.MonitorFileEvents:
                        monitorFileEvents |= (uint)item.Tag;
                        break;
                    case OptionType.MonitorFileIOEvents:
                        monitorIOEvents |= (ulong)item.Tag;
                        break;
                    case OptionType.ControlFileIOEvents:
                        controlIOEvents |= (ulong)item.Tag;
                        break;
                    case OptionType.ProccessId:
                        int pid = (int)item.Tag;
                        processId += pid.ToString() + ";";
                        break;

                    case OptionType.Access_Flag:
                        accessFlags |= (uint)item.Tag;
                        break;

                    case OptionType.RegistryAccessFlag:
                        registryAccessFlags |= (uint)item.Tag;
                        break;

                    case OptionType.RegistryCallbackClass:
                        registryCallbackClass |= (ulong)item.Tag;
                        break;

                    case OptionType.ShareFileAccessFlag:
                        accessFlags |= (uint)item.Tag;
                        break;

                    case OptionType.Filter_Status:
                        filterStatus |= (uint)item.Tag;
                        break;

                    case OptionType.Return_Status:
                        returnStatus |= (uint)item.Tag;
                        return;

                    case OptionType.FilterType:
                        filterType |= (byte)item.Tag;
                        break;

                    case OptionType.ProcessControlFlag:
                        processControlFlag |= (uint)item.Tag;
                        break;
                    case OptionType.FilterDesiredAccess:
                        filterDesiredAccess |= (uint)item.Tag;
                        break;
                    case OptionType.FilterDisposition:
                        filterDisposition |= (uint)item.Tag;
                        break;
                    case OptionType.FilterCreateOptions:
                        filterCreateOptions |= (uint)item.Tag;
                        break;
                }
            }

        }

        private void button_SelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = true;
            }
        }

        private void button_ClearAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView1.Items)
            {
                item.Checked = false;
            }
        }


    }
}
