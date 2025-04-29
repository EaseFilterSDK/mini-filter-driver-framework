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
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace EaseFilter.FilterControl
{
    /// <summary>
    /// Fires the control events before the file IO goes down to the file system for pre-io,
    /// fires the control events after the file IO was returned from the file system for post-io.
    /// you can allow, modify or deny the IO with control events.
    /// </summary>
    public enum ControlFileIOEvents : ulong
    {
        /// <summary>
        /// Fires this event before the file create IO was going down to the file system.
        /// </summary>
        OnPreFileCreate = 0x00000001,
        /// <summary>
        /// Fires this event after the file create IO was returned from the file system.
        /// </summary>
        OnPostFileCreate = 0x00000002,
        /// <summary>
        /// Fires this event before the file read IO was going down to the file system.
        /// </summary>
        OnPreFileRead = 0x00000154,
        /// <summary>
        /// Fires this event after the file read IO was returned from the file system.
        /// </summary>
        OnPostFileRead = 0x000002a8,
        /// <summary>
        /// Fires this event before the file write IO was going down to the file system.
        /// </summary>
        OnPreFileWrite = 0x00015400,
        /// <summary>
        /// Fires this event after the file write IO was returned from the file system.
        /// </summary>
        OnPostFileWrite = 0x0002a800,
        /// <summary>
        /// Fires this event before the query file size IO was going down to the file system.
        /// </summary>
        OnPreQueryFileSize = 0x0000000400000000,
        /// <summary>
        /// Fires this event after the query file size IO was returned from the file system.
        /// </summary>
        OnPostQueryFileSize = 0x0000000800000000,
        /// <summary>
        /// Fires this event before the query file basic info IO was going down to the file system.
        /// </summary>
        OnPreQueryFileBasicInfo = 0x0000001000000000,
        /// <summary>
        /// Fires this event after the query file basic info IO was returned from the file system.
        /// </summary>
        OnPostQueryFileBasicInfo = 0x0000002000000000,
        /// <summary>
        /// Fires this event before the query file standard info IO was going to the file system.
        /// </summary>
        OnPreQueryFileStandardInfo = 0x0000004000000000,
        /// <summary>
        /// Fires this event after the query file standard info IO was returned from the file system.
        /// </summary>
        OnPostQueryFileStandardInfo = 0x0000008000000000,
        /// <summary>
        /// Fires this event before the query file network info IO was going down to the file system.
        /// </summary>
        OnPreQueryFileNetworkInfo = 0x0000010000000000,
        /// <summary>
        /// Fires this event after the query file network info IO was returned from the file system.
        /// </summary>
        OnPostQueryFileNetworkInfo = 0x0000020000000000,
        /// <summary>
        /// Fires this event before the query file Id IO was going down to the file system.
        /// </summary>
        OnPreQueryFileId = 0x0000040000000000,
        /// <summary>
        /// Fires this event after the query file Id IO was returned from the file system.
        /// </summary>
        OnPostQueryFileId = 0x0000080000000000,
        /// <summary>
        /// Fires this event before the query file info IO was going down to the file system
        /// if the query file information class is not 'QueryFileSize','QueryFileBasicInfo'
        /// ,'QueryFileStandardInfo','QueryFileNetworkInfo' or 'QueryFileId'.
        /// </summary>
        OnPreQueryFileInfo = 0x00040000,
        /// <summary>
        /// Fires this event after the query file info IO was returned from the file system.
        /// </summary>
        OnPostQueryFileInfo = 0x00080000,
        /// <summary>
        /// Fires this event before the set file size IO was going down to the file system.
        /// </summary>         
        OnPreSetFileSize = 0x0000400000000000,
        /// <summary>
        /// Fires this event after the set file size IO was returned from the file system.
        /// </summary>
        OnPostSetFileSize = 0x0000800000000000,
        /// <summary>
        /// Fires this event before the set file basic info IO was going down to the file system.
        /// </summary>
        OnPreSetFileBasicInfo = 0x0001000000000000,
        /// <summary>
        /// Fires this event after the set file basic info IO was returned from the file system.
        /// </summary>
        OnPostSetFileBasicInfo = 0x0002000000000000,
        /// <summary>
        /// Fires this event before the set file standard info IO was going down to the file system.
        /// </summary>
        OnPreSetFileStandardInfo = 0x0004000000000000,
        /// <summary>
        /// Fires this event after the set file standard info IO was returned from the file system.
        /// </summary>
        OnPostSetFileStandardInfo = 0x0008000000000000,
        /// <summary>
        /// Fires this event before the set file network info was going down to the file system.
        /// </summary>
        OnPreSetFileNetworkInfo = 0x0010000000000000,
        /// <summary>
        /// Fires this event after the set file network info was returned from the file system.
        /// </summary>
        OnPostSetFileNetworkInfo = 0x0020000000000000,
        /// <summary>
        /// Fires this event before the file move or rename IO was going down to the file system.
        /// </summary>
        OnPreMoveOrRenameFile = 0x0040000000000000,
        /// <summary>
        /// Fires this event after the file move or rename IO was returned from the file system.
        /// </summary>
        OnPostMoveOrRenameFile = 0x0080000000000000,
        /// <summary>
        /// Fires this event before the file delete IO was going down to the file system.
        /// </summary>
        OnPreDeleteFile = 0x0100000000000000,
        /// <summary>
        /// Fires this event after the file delete IO was returned from the file system.
        /// </summary>
        OnPostDeleteFile = 0x0200000000000000,
        /// <summary>
        /// Fires this event before the set file info IO was going down to the file system
        /// if the information class is not 'SetFileSize','SetFileBasicInfo'
        /// ,'SetFileStandardInfo','SetFileNetworkInfo'.
        /// </summary>
        OnPreSetFileInfo = 0x00100000,
        /// <summary>
        /// Fires this event after the set file info IO was returned from the file system.
        /// </summary>
        OnPostSetFileInfo = 0x00200000,
        /// <summary>
        /// Fires this event before the query directory file info was going down to the file system.
        /// </summary>
        OnPreQueryDirectoryFile = 0x00400000,
        /// <summary>
        /// Fires this event after the query directory file info was returned from the file system.
        /// </summary>
        OnPostQueryDirectoryFile = 0x00800000,
        /// <summary>
        /// Fires this event before the query file security IO was going down to the file system.
        /// </summary>
        OnPreQueryFileSecurity = 0x01000000,
        /// <summary>
        /// Fires this event after the query file security IO was returned from the file system.
        /// </summary>
        OnPostQueryFileSecurity = 0x02000000,
        /// <summary>
        /// Fires this event before the set file security IO was going down to the file system.
        /// </summary>
        OnPreSetFileSecurity = 0x04000000,
        /// <summary>
        /// Fires thiis event after the set file security IO was returned from the file system.
        /// </summary>
        OnPostSetFileSecurity = 0x08000000,
        /// <summary>
        /// Fire this event before the file handle close IO was going down to the file system.
        /// </summary>
        OnPreFileHandleClose = 0x10000000,
        /// <summary>
        /// Fires this event after the file handle close IO was returned from the file system.
        /// </summary>
        OnPostFileHandleClose = 0x20000000,
        /// <summary>
        /// Fires this event before the file close IO was going down to the file system.
        /// </summary>
        OnPreFileClose = 0x40000000,
        /// <summary>
        /// Fires this event after the file close IO was returned from the file system.
        /// </summary>
        OnPostFileClose = 0x80000000,               

    }

    public class ProcessRightInfo
    {
        /// <summary>
        /// the process name filter mask
        /// </summary>
        public string processNameFilterMask;
        /// <summary>
        /// the certificate name of the signed process
        /// if it is not empty, only the process signed with this trusted certificate will have the access rights.
        /// </summary>
        public string certificateName;
        /// <summary>
        /// the sha256 hash of the binary image
        /// if it is not empty, only the process with the same sha256 hash will have the access rights.
        /// </summary>
        public string imageSha256Hash;
        /// <summary>
        /// The process's access right to the files.
        /// </summary>
        public uint accessFlags;

        public ProcessRightInfo(uint accessFlags, string processName, string certificateName, string imageSha256)
        {
            this.processNameFilterMask = processName;
            this.certificateName = certificateName;
            this.imageSha256Hash = imageSha256;
            this.accessFlags = accessFlags;
        }
    }

    partial class FileFilter
    {
        /// <summary>
        /// register the callback control IO
        /// </summary>
        ControlFileIOEvents registerControlFileIOEvents = 0;

        /// <summary>
        /// the control flag of the filter
        /// reference FilterAPI.AccessFlag enumeration
        /// </summary>
        uint accessFlags = FilterAPI.ALLOW_MAX_RIGHT_ACCESS;

        /// <summary>
        /// the access right of the process 
        /// </summary>
        Dictionary<uint, uint> processIdAccessRightList = new Dictionary<uint, uint>();
        Dictionary<string, ProcessRightInfo> processNameRightList = new Dictionary<string, ProcessRightInfo>();

        /// <summary>
        /// the access right of the users
        /// </summary>
        Dictionary<string, uint> userNameAccessRightList = new Dictionary<string, uint>();

        #region control filter property

        /// <summary>
        /// Enable the control file filter rule in boot time.
        /// </summary>
        public bool IsResident
        {
            get { return isResident; }
            set { isResident = value; }
        }

        /// <summary>
        /// Below is the control filter properties to check if the associated control event was registered.
        /// </summary>
        public bool IsPreFileCreateRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreFileCreate) > 0; } }
        public bool IsPostFileCreateRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostFileCreate) > 0; } }

        public bool IsPreQueryFileSizeRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreQueryFileSize) > 0; } }
        public bool IsPostQueryFileSizeRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostQueryFileSize) > 0; } }

        public bool IsPreQueryBasicInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreQueryFileBasicInfo) > 0; } }
        public bool IsPostQueryBasicInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostQueryFileBasicInfo) > 0; } }

        public bool IsPreQueryFileStandardInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreQueryFileStandardInfo) > 0; } }
        public bool IsPostQueryFileStandardInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostQueryFileStandardInfo) > 0; } }

        public bool IsPreQueryFileNetworkInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreQueryFileNetworkInfo) > 0; } }
        public bool IsPostQueryFileNetworkInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostQueryFileNetworkInfo) > 0; } }

        public bool IsPreQueryFileIdRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreQueryFileId) > 0; } }
        public bool IsPostQueryFileIdRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostQueryFileId) > 0; } }

        public bool IsPreDeleteFileRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreDeleteFile) > 0; } }
        public bool IsPostDeleteFileRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostDeleteFile) > 0; } }

        public bool IsPreMoveOrRenameFileRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreMoveOrRenameFile) > 0; } }
        public bool IsPostMoveOrRenameFileRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostMoveOrRenameFile) > 0; } }

        public bool IsPreSetFileBasicInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreSetFileBasicInfo) > 0; } }
        public bool IsPostSetFileBasicInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostSetFileBasicInfo) > 0; } }

        public bool IsPreSetFileNetworkInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreSetFileNetworkInfo) > 0; } }
        public bool IsPostSetFileNetworkInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostSetFileNetworkInfo) > 0; } }

        public bool IsPreSetFileSizeRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreSetFileSize) > 0; } }
        public bool IsPostSetFileSizeRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostSetFileSize) > 0; } }

        public bool IsPreSetFileStandardInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPreSetFileStandardInfo) > 0; } }
        public bool IsPostSetFileStandardInfoRegister { get { return (registerControlFileIOEvents & ControlFileIOEvents.OnPostSetFileStandardInfo) > 0; } }

        /// <summary>
        /// Register the file control IOs callback notification for control filter
        /// to allow/modify/deny the file I/O
        /// </summary>
        public ControlFileIOEvents ControlFileIOEventFilter
        {
            get { return registerControlFileIOEvents; }
            set { registerControlFileIOEvents = value; }
        }

        /// <summary>
        /// Add the access right to the process with the process name filter mask.
        /// </summary>
        /// <param name="processNameFilterMask"></param>
        /// <param name="accessFlags"></param>
        public void AddProcessNameAccessRight(string processNameFilterMask, uint accessFlags)
        {
            ProcessRightInfo processRightInfo =  new ProcessRightInfo(accessFlags, processNameFilterMask, "", "");
            processNameRightList.Add(processNameFilterMask, processRightInfo);
        }

        /// <summary>
        /// Add the access rights to the process which was signed or has the same sha256 hash
        /// </summary>
        /// <param name="accessFlags">the access rights for the process</param>
        /// <param name="processNameFilterMask">the process name filter mask</param>
        /// <param name="certificateName">the certificate name to sign the process, it is optional</param>
        /// <param name="ImageSha256Hash">the sha256 hash of the process, it is optional</param>
        public void AddTrustedProcessRight(uint accessFlags, string processNameFilterMask, string certificateName, string imageSha256Hash)
        {
            processNameRightList.Add(processNameFilterMask, new ProcessRightInfo(accessFlags, processNameFilterMask, certificateName, imageSha256Hash));
        }

        /// <summary>
        /// The process access right list, you can define the trusted process which was signed with specific certificate name, or with the same sha256 hash.
        /// You can set the access right to the sepecific processes.
        /// </summary>
        public Dictionary<string, ProcessRightInfo> TrustedProcessAccessRightList
        {
            get { return processNameRightList; }
            set { processNameRightList = value; }
        }

        /// <summary>
        /// Get or set the process name access right list in string
        /// the process access rights string format "accessFlags|processName|certificateName|image256Hash"
        /// for example: "123456|notepad.exe|EaseFilter Technologies|A123234234BADSFSFASDFSFASFASDF"
        /// seperate the multiple items with ';'
        /// </summary>
        public string ProcessNameAccessRightString
        {
            get
            {
                string ProcessNameRights = string.Empty;
                foreach (ProcessRightInfo processRightInfo in TrustedProcessAccessRightList.Values)
                {
                    ProcessNameRights += processRightInfo.accessFlags.ToString() + "|" + processRightInfo.processNameFilterMask
                        + "|" + processRightInfo.certificateName + "|" + processRightInfo.imageSha256Hash + ";";
                }

                return ProcessNameRights;
            }
            set
            {
                this.processNameRightList.Clear();

                string[] processNameRightList = value.Split(new char[] { ';' });
                if (processNameRightList.Length > 0)
                {
                    foreach (string ProcessNameRight in processNameRightList)
                    {
                        string[] entries = ProcessNameRight.Split(new char[] { '|' });
                        if (entries.Length > 3)
                        {
                            uint accessFlags = uint.Parse(entries[0].Trim());
                            string processName = entries[1].Trim();                            
                            string certName = entries[2].Trim();
                            string sha256Hash = entries[3].Trim();

                            AddTrustedProcessRight(accessFlags, processName, certName, sha256Hash);
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Remove the process's access right from the file filter rule.
        /// </summary>
        /// <param name="processNameFilterMask"></param>
        /// <returns></returns>
        public bool RemoveProcessRights(string processNameFilterMask)
        {
            return FilterAPI.RemoveProcessRightsFromFilterRule(this.fileFilterMask, processNameFilterMask);
        }

        /// <summary>
        /// set the access rights of the files to the process in the list
        /// </summary>
        public Dictionary<uint, uint> ProcessIdAccessRightList
        {
            get { return processIdAccessRightList; }
            set { processIdAccessRightList = value; }
        }

        /// <summary>
        /// get or set the access rights of the files to the process list in string
        /// the process Id access right string format: pid|accessRight;pid2|accessright
        /// </summary>
        public string ProcessIdAccessRightString
        {
            get
            {
                string processIdAccessRights = string.Empty;
                foreach (KeyValuePair<uint,uint> processIdAccessRight in processIdAccessRightList)
                {
                    processIdAccessRights += processIdAccessRight.Key.ToString() + "|" + processIdAccessRight.Value.ToString() + ";";
                }

                return processIdAccessRights;
            }
            set
            {
                processIdAccessRightList.Clear();
                string[] processIdAccessRights = value.Split(new char[] { ';' });
                if (processIdAccessRights.Length > 0)
                {
                    foreach (string processIdAccessRight in processIdAccessRights)
                    {
                        string[] entries = processIdAccessRight.Split(new char[] { '|' });
                        if (entries.Length > 1)
                        {
                            uint processId = uint.Parse(entries[0]);
                            uint accessFlags = uint.Parse(entries[1]);
                            ProcessIdAccessRightList.Add(processId, accessFlags);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// set the access rights of the files to the users in the list
        /// </summary>
        public Dictionary<string, uint> UserAccessRightList
        {
            get { return userNameAccessRightList; }
            set { userNameAccessRightList = value; }
        }

        /// <summary>
        /// get or set the access rights of the files to the user list in string
        /// the user access right string format: userName|accessright;userName2|accessRight
        /// </summary>
        public string UserAccessRightString
        {
            get
            {
                string userRights = string.Empty;
                foreach (KeyValuePair<string, uint> userRight in UserAccessRightList)
                {
                    userRights += userRight.Key + "|" + userRight.Value.ToString() + ";";
                }

                return userRights;
            }
            set
            {
                userNameAccessRightList.Clear();
                string[] userRights = value.Split(new char[] { ';' });
                if (userRights.Length > 0)
                {
                    foreach (string userRight in userRights)
                    {
                        string[] entries = userRight.Split(new char[] { '|' });
                        if (entries.Length > 1)
                        {
                            string userName = entries[0];
                            uint accessFlags = uint.Parse(entries[1]);
                            UserAccessRightList.Add(userName, accessFlags);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get or set the hidden file filter mask list,
        /// hide the files if the file name matches the filter mask in directory browsing.
        /// </summary>
        public List<string> HiddenFileFilterMaskList
        {
            get { return hidenFileFilterMaskList; }
            set { hidenFileFilterMaskList = value; }
        }

        /// <summary>
        /// Get or set the hidden file filter mask list in string
        /// </summary>
        public string HiddenFileFilterMaskString
        {
            get
            {
                string hiddenFileFilterMaskString = string.Empty;
                foreach (string hidenFileFilterMask in hidenFileFilterMaskList)
                {
                    if (hidenFileFilterMask.Trim().Length > 0)
                    {
                        hiddenFileFilterMaskString += hidenFileFilterMask + ";";
                    }
                }

                return hiddenFileFilterMaskString;
            }
            set
            {
                hidenFileFilterMaskList.Clear();
                string[] hidenFileFilterMasks = value.Split(new char[] { ';' });
                if (hidenFileFilterMasks.Length > 0)
                {
                    foreach (string hidenFileFilterMask in hidenFileFilterMasks)
                    {
                        if (hidenFileFilterMask.Trim().Length > 0)
                        {
                            hidenFileFilterMaskList.Add(hidenFileFilterMask);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///Get or set the reparse file filter mask list, 
        ///reparse the file open to the new file which the file name will be replaced with the reparse filter mask.
        /// 
        ///For example:
        ///FilterMask = c:\test\*txt
        ///ReparseFilterMask = d:\reparse\*doc
        ///If you open file c:\test\MyTest.txt, it will reparse to the file d:\reparse\MyTest.doc.
        /// </summary>
        public string ReparseFileFilterMask
        {
            get { return reparseFileFilterMask; }
            set { reparseFileFilterMask = value; }
        }

        /// <summary>
        /// Control the file access rights of the filter
        /// </summary>
        public FilterAPI.AccessFlag AccessFlags
        {
            get { return (FilterAPI.AccessFlag)accessFlags; }
            set { accessFlags = (uint)value; }
        }

        /// <summary>
        /// allow the process to read the file when it is true, or the file can't be read.
        /// </summary>
        public bool EnableReadFileData
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ACCESS;
                }
            }
        }

        /// <summary>
        /// allow the process to read the encrypted file when it is true, or the file can't be read.
        /// </summary>
        public bool EnableReadEncryptedFileData
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_READ_ENCRYPTED_FILES;
                }
            }
        }

        /// <summary>
        /// allow the process to write the file when it is true, or the file can't be written.
        /// </summary>
        public bool EnableWriteToFile
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_WRITE_ACCESS;
                }

            }
        }

        /// <summary>
        /// allow the process to delete the file when it is true, or the file can't be deleted.
        /// </summary>
        public bool EnableDeleteFile
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_DELETE;
                }
            }
        }

        /// <summary>
        /// allow the process to rename or move the file when it is true, or the file can't be moved or renamed.
        /// </summary>
        public bool EnableRenameOrMoveFile
        {
            get { return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME) > 0); }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_RENAME;
                }

            }
        }


        /// <summary>
        /// allow the process to create the new file when it is true, or file creation will be denied.
        /// </summary>
        public bool EnableFileCreation
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS;
                }
            }
        }

        /// <summary>
        /// allow the process to browse the directory when it is true, or the directory file list can't be enumerated.
        /// </summary>
        public bool EnableDirectoryBrowsing
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS;
                }
            }
        }

        /// <summary>
        /// allow the files being accessed by the processes from network when it is true, or the file access will be blocked.
        /// </summary>
        public bool EnableFileBeingAccessedViaNetwork
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_FILE_ACCESS_FROM_NETWORK;
                }
            }
        }

        /// <summary>
        /// allow the file's security being changed when it is true, or the file security change will be blocked.
        /// </summary>
        public bool EnableChangeFileSecurity
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_SECURITY_ACCESS;
                }
            }
        }

        /// <summary>
        /// allow the file's information(file size,attributes,file time) being changed when it is true, or the file information change will be blocked.
        /// </summary>
        public bool EnableChangeFileInfo
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_SET_INFORMATION;
                }
            }
        }

        /// <summary>
        /// allow the file being copied when it is true, or the file copy will be blocked.
        /// </summary>
        public bool EnableFileBeingCopied
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_ALL_SAVE_AS;
                }
            }
        }

        /// <summary>
        /// allow the file being copied out from the current folder when it is true, or the file copy will be blocked.
        /// </summary>
        public bool EnableFileBeingCopiedOutOfFolder
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ALLOW_COPY_PROTECTED_FILES_OUT;
                }

            }
        }

        /// <summary>
        /// Enable to hide the files in directory browsing.
        /// </summary>
        public bool EnableHiddenFile
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING;
                }
            }
        }


        /// <summary>
        /// Enable reparse file open to the other file.
        /// </summary>
        public bool EnableReparseFile
        {
            get
            {
                return ((accessFlags & (uint)FilterAPI.AccessFlag.ENABLE_REPARSE_FILE_OPEN) > 0);
            }

            set
            {
                if (value)
                {
                    accessFlags |= (uint)FilterAPI.AccessFlag.ENABLE_REPARSE_FILE_OPEN;
                }
                else
                {
                    accessFlags &= ~(uint)FilterAPI.AccessFlag.ENABLE_REPARSE_FILE_OPEN;
                }
            }
        }      

        #endregion //control property

        #region file control events you can register

        /// <summary>
        /// Fires this event before the file is opened/created,
        /// you can block the file open/create or reparse the file open to other file in this event.
        /// </summary>
        public event EventHandler<FileCreateEventArgs> OnPreCreateFile;
        /// <summary>
        /// Fires this event after the file is opened/created,
        /// you will get the status of the create to know if the file was created or opened sucessfully.
        /// </summary>
        public event EventHandler<FileCreateEventArgs> OnPostCreateFile;
        /// <summary>
        /// Fires this event before the data of the file was read,
        /// you can block the read request, or return your own read data here.
        /// </summary>
        public event EventHandler<FileReadEventArgs> OnPreFileRead;
        /// <summary>
        /// Fires this event after the data of the file was read,
        /// you can modify the return read data here.
        /// </summary>
        public event EventHandler<FileReadEventArgs> OnPostFileRead;
        /// <summary>
        /// Fires this event before the data was written to the file,
        /// you can block the write request, or modify the write data.
        /// </summary>
        public event EventHandler<FileWriteEventArgs> OnPreFileWrite;
        /// <summary>
        /// Fires this event after the data was written to the file.
        /// </summary>
        public event EventHandler<FileWriteEventArgs> OnPostFileWrite;
        /// <summary>
        /// Fires this event before the file size query goes down to the file system.
        /// you can block the query request or return your own file size data here.
        /// </summary>
        public event EventHandler<FileSizeEventArgs> OnPreQueryFileSize;
        /// <summary>
        /// Fires this event after the file size query was returned from the file system.
        /// you can modify the file size data here.
        /// </summary>
        public event EventHandler<FileSizeEventArgs> OnPostQueryFileSize;
        /// <summary>
        /// Fires this event before the file basic info query goes down to the file system.
        /// you can block the query request or return your own file basic information.
        /// </summary>
        public event EventHandler<FileBasicInfoEventArgs> OnPreQueryFileBasicInfo;
        /// <summary>
        /// Fires this event after the file basic info query was returned from the file system.
        /// you can modify the file basic info data here.
        /// </summary>
        public event EventHandler<FileBasicInfoEventArgs> OnPostQueryFileBasicInfo;
        /// <summary>
        /// Fires this event before the file standard info query goes down to the file system.
        /// you can block the query request or return your own file standard information.
        /// </summary>
        public event EventHandler<FileStandardInfoEventArgs> OnPreQueryFileStandardInfo;
        /// <summary>
        /// Fires this event after the file standard info query was returned from the file system.
        /// you can modify the file standard info data here.
        public event EventHandler<FileStandardInfoEventArgs> OnPostQueryFileStandardInfo;
        /// <summary>
        /// Fires this event before the file network info query goes down to the file system.
        /// you can block the query request or return your own file network information.
        /// </summary>
        public event EventHandler<FileNetworkInfoEventArgs> OnPreQueryFileNetworkInfo;
        /// <summary>
        /// Fires this event after the file network info query was returned from the file system.
        /// you can modify the file network info data here.
        public event EventHandler<FileNetworkInfoEventArgs> OnPostQueryFileNetworkInfo;
        /// <summary>
        /// Fires this event before the file Id query goes down to the file system.
        /// you can block the query request.
        public event EventHandler<FileIdEventArgs> OnPreQueryFileId;
        /// <summary>
        /// Fires this event after the file Id query was returned from the file system.
        public event EventHandler<FileIdEventArgs> OnPostQueryFileId;
        /// <summary>
        /// Fires this event before the file query info goes down to the file system.
        /// you can block the query request.
        public event EventHandler<FileInfoArgs> OnPreQueryFileInfo;
        /// <summary>
        /// Fires this event after the file query info was returned from the file system.
        public event EventHandler<FileInfoArgs> OnPostQueryFileInfo;
        /// <summary>
        /// Fires this event before the set file size request goes down to the file system.
        /// you can block this event or modify the file size data here.
        /// </summary>
        public event EventHandler<FileSizeEventArgs> OnPreSetFileSize;
        /// <summary>
        /// Fires this event after the set file size request was returned from the file system.
        /// </summary>
        public event EventHandler<FileSizeEventArgs> OnPostSetFileSize;
        /// <summary>
        /// Fires this event before the set file basic info request goes down to the file system.
        /// you can block this request or modify the basic info data here.
        /// </summary>
        public event EventHandler<FileBasicInfoEventArgs> OnPreSetFileBasicInfo;
        /// <summary>
        /// Fires this event after the set file basic info request was returned from the file system.
        /// </summary>
        public event EventHandler<FileBasicInfoEventArgs> OnPostSetFileBasicInfo;
        /// <summary>
        /// Fires this event before the set file standard info request goes down to the file system.
        /// you can block this request or modify the standard info data here.
        /// </summary>
        public event EventHandler<FileStandardInfoEventArgs> OnPreSetFileStandardInfo;
        /// <summary>
        /// Fires this event after the set file standard info request was returned from the file system.
        /// </summary>
        public event EventHandler<FileStandardInfoEventArgs> OnPostSetFileStandardInfo;
        /// <summary>
        /// Fires this event before the set file network info request goes down to the file system.
        /// you can block this request or modify the network info data here.
        /// </summary>
        public event EventHandler<FileNetworkInfoEventArgs> OnPreSetFileNetworkInfo;
        /// <summary>
        /// Fires this event after the set file network info request was returned from the file system.
        /// </summary>
        public event EventHandler<FileNetworkInfoEventArgs> OnPostSetFileNetworkInfo;
        /// <summary>
        /// Fires this event before the file was moved or renamed.
        /// you can block this request here.
        /// </summary>
        public event EventHandler<FileMoveOrRenameEventArgs> OnPreMoveOrRenameFile;
        /// <summary>
        /// Fires this event after the file was moved or renamed.
        /// </summary>
        public event EventHandler<FileMoveOrRenameEventArgs> OnPostMoveOrRenameFile;
        /// <summary>
        /// Fires this event before the file is going to be deleted.
        /// you can block this request here.
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnPreDeleteFile;
        /// <summary>
        /// Fires this event after the file was marked as deleted 
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnPostDeleteFile;
        /// <summary>
        /// Fires this event before the set file info goes down to the file system. 
        /// you can block this request here.
        /// </summary>
        public event EventHandler<FileInfoArgs> OnPreSetFileInfo;
        /// <summary>
        /// Fires this event after the set file info was returned from file system. 
        /// </summary>
        public event EventHandler<FileInfoArgs> OnPostSetFileInfo;
        /// <summary>
        /// Fires this event before the file security query goes down to the file system.
        /// you can block this request or return your own security data here.
        /// </summary>
        public event EventHandler<FileSecurityEventArgs> OnPreQueryFileSecurity;
        /// <summary>
        /// Fires this event after the file security query was returned from the file system.
        /// you can modify the file security data here.
        /// </summary>
        public event EventHandler<FileSecurityEventArgs> OnPostQueryFileSecurity;
        /// <summary>
        /// Fires this event before the set file security request goes down to the file system.
        /// you can block this request or modify the file security 
        /// </summary>
        public event EventHandler<FileSecurityEventArgs> OnPreSetFileSecurity;
        /// <summary>
        /// Fires this event after the set file security request was returned from the file system.
        /// </summary>
        public event EventHandler<FileSecurityEventArgs> OnPostSetFileSecurity;
        /// <summary>
        /// Fires this event before the directory enumeration query goes down to the file system.
        /// you can block this request or return your own data here.
        /// </summary>
        public event EventHandler<FileQueryDirectoryEventArgs> OnPreQueryDirectoryFile;
        /// <summary>
        /// Fires this event after the directory enumeration query was returned from the file system.
        /// you can modify the return file data here, i.e. hide the file, change the file name.
        /// </summary>
        public event EventHandler<FileQueryDirectoryEventArgs> OnPostQueryDirectoryFile;
        /// <summary>
        /// Fires this event before the file handle was closed.
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnPreFileHandleClose;
        /// <summary>
        /// Fires this event after the file handle was closed.
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnPostFileHandleClose;
        /// <summary>
        /// Fires this event before all system references to fileObject were closed 
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnPreFileClose;
        /// <summary>
        /// Fires this event after all system references to fileObject were closed
        /// </summary>
        public event EventHandler<FileIOEventArgs> OnPostFileClose;

        #endregion //file control events you can register

        public virtual bool FireControlEvents(FilterAPI.MessageSendData messageSend, ref FilterAPI.MessageReplyData messageReply)
        {
            bool retVal = true;

            try
            {
                if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CREATE)
                {
                    FileCreateEventArgs fileCreateEventArgs = new FileCreateEventArgs(messageSend);
                    fileCreateEventArgs.Description = fileCreateEventArgs.ToString();

                    if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CREATE
                        && null != OnPreCreateFile && IsPreFileCreateRegister)
                    {

                        fileCreateEventArgs.EventName = "OnPreCreateFile";

                        //invoke the create event delegate
                        OnPreCreateFile(this, fileCreateEventArgs);

                        //you can passthrough, modify or deny the create file IO based on the return status.
                        messageReply.ReturnStatus = (uint)fileCreateEventArgs.ReturnStatus;
                        if (fileCreateEventArgs.IsDataModified || fileCreateEventArgs.ReturnStatus != NtStatus.Status.Success)
                        {
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                            if (fileCreateEventArgs.ReturnStatus == NtStatus.Status.Reparse && fileCreateEventArgs.reparseFileName.Length > 0)
                            {
                                //reparse file open to the new file name
                                byte[] returnData = Encoding.Unicode.GetBytes(fileCreateEventArgs.reparseFileName);
                                Array.Copy(returnData, messageReply.DataBuffer, returnData.Length);
                                messageReply.DataBufferLength = (uint)returnData.Length;
                            }
                        }

                    }

                    if (fileCreateEventArgs.isDeleteOnClose && null != OnPreDeleteFile && IsPreDeleteFileRegister)
                    {
                        fileCreateEventArgs = new FileCreateEventArgs(messageSend);
                        fileCreateEventArgs.EventName = "OnPreDeleteFile";
                        OnPreDeleteFile(this, fileCreateEventArgs);

                        //you can deny the create file IO based on the return status.
                        messageReply.ReturnStatus = (uint)fileCreateEventArgs.ReturnStatus;
                        if (fileCreateEventArgs.ReturnStatus != NtStatus.Status.Success)
                        {
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }

                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CREATE)
                {
                    FileCreateEventArgs fileCreateEventArgs = new FileCreateEventArgs(messageSend);
                    fileCreateEventArgs.Description = fileCreateEventArgs.ToString();

                    if (null != OnPostCreateFile && IsPostFileCreateRegister)
                    {
                        fileCreateEventArgs.EventName = "OnPostCreateFile";

                        //invoke the create event delegate
                        OnPostCreateFile(this, fileCreateEventArgs);
                    }

                    if (fileCreateEventArgs.isDeleteOnClose && null != OnPostDeleteFile && IsPostDeleteFileRegister)
                    {
                        fileCreateEventArgs = new FileCreateEventArgs(messageSend);
                        fileCreateEventArgs.EventName = "OnPostDeleteFile";

                        OnPostDeleteFile(this, fileCreateEventArgs);
                    }

                    messageReply.ReturnStatus = (uint)fileCreateEventArgs.ReturnStatus;
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CACHE_READ
                      || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_FASTIO_READ
                      || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_NOCACHE_READ
                      || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_PAGING_IO_READ)
                {
                    if (null != OnPreFileRead)
                    {
                        FileReadEventArgs fileReadEventArgs = new FileReadEventArgs(messageSend);
                        fileReadEventArgs.EventName = "OnPreFileRead-" + fileReadEventArgs.readType;

                        //invoke the event delegate
                        OnPreFileRead(this, fileReadEventArgs);
                        messageReply.ReturnStatus = (uint)fileReadEventArgs.ReturnStatus;

                        if (NtStatus.Status.Success != fileReadEventArgs.ReturnStatus)
                        {
                            //block the io, and complete the pre-io.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }
                        else if (fileReadEventArgs.IsDataModified)
                        {
                            //complete the pre-read IO,won't go down to the file system, replace the read data with your own data. 
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION
                                | (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                            Array.Copy(fileReadEventArgs.buffer, messageReply.DataBuffer, fileReadEventArgs.buffer.Length);
                            messageReply.DataBufferLength = (uint)fileReadEventArgs.buffer.Length;
                        }

                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CACHE_READ
                    || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_FASTIO_READ
                    || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_NOCACHE_READ
                    || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_PAGING_IO_READ)
                {
                    if (null != OnPostFileRead)
                    {
                        FileReadEventArgs fileReadEventArgs = new FileReadEventArgs(messageSend);
                        fileReadEventArgs.EventName = "OnPostFileRead-" + fileReadEventArgs.readType;

                        //invoke the event delegate
                        OnPostFileRead(this, fileReadEventArgs);
                        messageReply.ReturnStatus = (uint)fileReadEventArgs.ReturnStatus;
                        if (fileReadEventArgs.IsDataModified)
                        {
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                            //replace the read data with your own data.
                            Array.Copy(fileReadEventArgs.buffer, messageReply.DataBuffer, fileReadEventArgs.buffer.Length);
                            messageReply.DataBufferLength = (uint)fileReadEventArgs.buffer.Length;
                        }
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CACHE_WRITE
                     || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_FASTIO_WRITE
                     || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_NOCACHE_WRITE
                     || messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_PAGING_IO_WRITE)
                {
                    //if the event was subscribed 
                    if (null != OnPreFileWrite)
                    {
                        FileWriteEventArgs fileWriteEventArgs = new FileWriteEventArgs(messageSend);
                        fileWriteEventArgs.EventName = "OnPreFileWrite-" + fileWriteEventArgs.writeType;

                        //invoke the event delegate
                        OnPreFileWrite(this, fileWriteEventArgs);

                        //change the IO status, and modify the write data here.
                        messageReply.ReturnStatus = (uint)fileWriteEventArgs.ReturnStatus;

                        if (NtStatus.Status.Success != fileWriteEventArgs.ReturnStatus)
                        {
                            //block the io, and complete the pre-io.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }
                        else if (fileWriteEventArgs.IsDataModified)
                        {
                            //replace the write data with your own data and write it to the file.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                            Array.Copy(fileWriteEventArgs.buffer, messageReply.DataBuffer, fileWriteEventArgs.buffer.Length);
                            messageReply.DataBufferLength = (uint)fileWriteEventArgs.buffer.Length;
                        }
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CACHE_WRITE
                || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_FASTIO_WRITE
                || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_NOCACHE_WRITE
                || messageSend.MessageType == (uint)FilterAPI.MessageType.POST_PAGING_IO_WRITE)
                {
                    //if the event was subscribed 
                    if (null != OnPostFileWrite)
                    {
                        FileWriteEventArgs fileWriteEventArgs = new FileWriteEventArgs(messageSend);
                        fileWriteEventArgs.EventName = "OnPostFileWrite;" + fileWriteEventArgs.writeType;

                        //invoke the event delegate
                        OnPostFileWrite(this, fileWriteEventArgs);

                        messageReply.ReturnStatus = (uint)fileWriteEventArgs.ReturnStatus;
                    }

                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_QUERY_INFORMATION)
                {
                    if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileEndOfFileInformation && IsPreQueryFileSizeRegister)
                    {
                        //if the event was subscribed 
                        if (null != OnPreQueryFileSize)
                        {
                            FileSizeEventArgs fileSizeArgs = new FileSizeEventArgs(messageSend);
                            fileSizeArgs.EventName = "OnPreQueryFileSize";

                            //invoke the event delegate
                            OnPreQueryFileSize(this, fileSizeArgs);

                            messageReply.ReturnStatus = (uint)fileSizeArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileSizeArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileSizeArgs.IsDataModified)
                            {
                                //replace the file size with your own data and complete pre-io. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED | (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                                byte[] buffer = BitConverter.GetBytes(fileSizeArgs.fileSizeToQueryOrSet);
                                Array.Copy(buffer, messageReply.DataBuffer, buffer.Length);
                                messageReply.DataBufferLength = (uint)buffer.Length;
                            }

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileInternalInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreQueryFileId && IsPreQueryFileIdRegister)
                        {
                            FileIdEventArgs fileIdArgs = new FileIdEventArgs(messageSend);
                            fileIdArgs.EventName = "OnPreQueryFileId";

                            //invoke the event delegate
                            OnPreQueryFileId(this, fileIdArgs);

                            messageReply.ReturnStatus = (uint)fileIdArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileIdArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileBasicInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreQueryFileBasicInfo && IsPreQueryBasicInfoRegister)
                        {
                            FileBasicInfoEventArgs fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
                            fileBasicInfoArgs.EventName = "OnPreQueryFileBasicInfo";

                            //invoke the event delegate
                            OnPreQueryFileBasicInfo(this, fileBasicInfoArgs);

                            messageReply.ReturnStatus = (uint)fileBasicInfoArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileBasicInfoArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileBasicInfoArgs.IsDataModified)
                            {
                                //replace the file basic info data with your own data,complete pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED | (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                                int len = Marshal.SizeOf(fileBasicInfoArgs.basicInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileBasicInfoArgs.basicInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileStandardInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreQueryFileStandardInfo && IsPreQueryFileStandardInfoRegister)
                        {
                            FileStandardInfoEventArgs fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
                            fileStandardInfoArgs.EventName = "OnPreQueryFileStandardInfo";

                            //invoke the event delegate
                            OnPreQueryFileStandardInfo(this, fileStandardInfoArgs);

                            messageReply.ReturnStatus = (uint)fileStandardInfoArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileStandardInfoArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileStandardInfoArgs.IsDataModified)
                            {
                                //replace the file standardInfo data with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED | (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                                int len = Marshal.SizeOf(fileStandardInfoArgs.standardInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileStandardInfoArgs.standardInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileNetworkOpenInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreQueryFileNetworkInfo && IsPreQueryFileNetworkInfoRegister)
                        {
                            FileNetworkInfoEventArgs fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
                            fileNetworkInfoArgs.EventName = "OnPreQueryFileNetworkInfo";

                            //invoke the event delegate
                            OnPreQueryFileNetworkInfo(this, fileNetworkInfoArgs);

                            messageReply.ReturnStatus = (uint)fileNetworkInfoArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileNetworkInfoArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileNetworkInfoArgs.IsDataModified)
                            {
                                //replace the file standardInfo data with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED | (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                                int len = Marshal.SizeOf(fileNetworkInfoArgs.networkInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileNetworkInfoArgs.networkInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }

                        }
                    }
                    else if (null != OnPreQueryFileInfo)
                    {
                        FileInfoArgs fileInfoArgs = new FileInfoArgs(messageSend);
                        fileInfoArgs.EventName = "OnPreQueryFileInfo:" + fileInfoArgs.FileInfoClass.ToString();

                        //invoke the event delegate
                        OnPreQueryFileInfo(this, fileInfoArgs);

                        messageReply.ReturnStatus = (uint)fileInfoArgs.ReturnStatus;

                        if (NtStatus.Status.Success != fileInfoArgs.ReturnStatus)
                        {
                            //block the io, and complete the pre-io.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }
                        else if (fileInfoArgs.IsDataModified)
                        {
                            //replace the file standardInfo data with your own data. 
                        }

                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_QUERY_INFORMATION)
                {
                    if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileEndOfFileInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostQueryFileSize && IsPostQueryFileSizeRegister)
                        {
                            FileSizeEventArgs fileSizeArgs = new FileSizeEventArgs(messageSend);
                            fileSizeArgs.EventName = "OnPostQueryFileSize";

                            //invoke the event delegate
                            OnPostQueryFileSize(this, fileSizeArgs);

                            messageReply.ReturnStatus = (uint)fileSizeArgs.ReturnStatus;
                            if (fileSizeArgs.IsDataModified)
                            {
                                //replace the file size with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                byte[] buffer = BitConverter.GetBytes(fileSizeArgs.fileSizeToQueryOrSet);
                                Array.Copy(buffer, messageReply.DataBuffer, buffer.Length);
                                messageReply.DataBufferLength = (uint)buffer.Length;
                            }

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileInternalInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostQueryFileId && IsPostQueryFileIdRegister)
                        {
                            FileIdEventArgs fileIdArgs = new FileIdEventArgs(messageSend);
                            fileIdArgs.EventName = "OnPostQueryFileId";

                            //invoke the event delegate
                            OnPostQueryFileId(this, fileIdArgs);
                            messageReply.ReturnStatus = (uint)fileIdArgs.ReturnStatus;

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileBasicInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostQueryFileBasicInfo && IsPostQueryBasicInfoRegister)
                        {
                            FileBasicInfoEventArgs fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
                            fileBasicInfoArgs.EventName = "OnPostQueryFileBasicInfo";

                            //invoke the event delegate
                            OnPostQueryFileBasicInfo(this, fileBasicInfoArgs);

                            messageReply.ReturnStatus = (uint)fileBasicInfoArgs.ReturnStatus;
                            if (fileBasicInfoArgs.IsDataModified)
                            {
                                //replace the file basic info data with your own data.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                int len = Marshal.SizeOf(fileBasicInfoArgs.basicInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileBasicInfoArgs.basicInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileStandardInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostQueryFileStandardInfo && IsPostQueryFileStandardInfoRegister)
                        {
                            FileStandardInfoEventArgs fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
                            fileStandardInfoArgs.EventName = "OnPostQueryFileStandardInfo";

                            //invoke the event delegate
                            OnPostQueryFileStandardInfo(this, fileStandardInfoArgs);

                            messageReply.ReturnStatus = (uint)fileStandardInfoArgs.ReturnStatus;
                            if (fileStandardInfoArgs.IsDataModified)
                            {
                                //replace the file standardInfo data with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                int len = Marshal.SizeOf(fileStandardInfoArgs.standardInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileStandardInfoArgs.standardInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileNetworkOpenInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostQueryFileNetworkInfo && IsPostQueryFileNetworkInfoRegister)
                        {
                            FileNetworkInfoEventArgs fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
                            fileNetworkInfoArgs.EventName = "OnPostQueryFileNetworkInfo";

                            //invoke the event delegate
                            OnPostQueryFileNetworkInfo(this, fileNetworkInfoArgs);

                            messageReply.ReturnStatus = (uint)fileNetworkInfoArgs.ReturnStatus;
                            if (fileNetworkInfoArgs.IsDataModified)
                            {
                                //replace the file standardInfo data with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                int len = Marshal.SizeOf(fileNetworkInfoArgs.networkInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileNetworkInfoArgs.networkInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }
                        }
                    }
                    else if (null != OnPostQueryFileInfo)
                    {
                        FileInfoArgs fileInfoArgs = new FileInfoArgs(messageSend);
                        fileInfoArgs.EventName = "OnPostQueryFileInfo:" + fileInfoArgs.FileInfoClass.ToString();

                        //invoke the event delegate
                        OnPostQueryFileInfo(this, fileInfoArgs);

                        messageReply.ReturnStatus = (uint)fileInfoArgs.ReturnStatus;
                        if (fileInfoArgs.IsDataModified)
                        {
                            //replace the file standardInfo data with your own data. 
                        }

                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_SET_INFORMATION)
                {
                    if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileEndOfFileInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreSetFileSize && IsPreSetFileSizeRegister)
                        {
                            FileSizeEventArgs fileSizeArgs = new FileSizeEventArgs(messageSend);
                            fileSizeArgs.EventName = "OnPreSetFileSize";

                            //invoke the event delegate
                            OnPreSetFileSize(this, fileSizeArgs);

                            messageReply.ReturnStatus = (uint)fileSizeArgs.ReturnStatus;


                            if (NtStatus.Status.Success != fileSizeArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileSizeArgs.IsDataModified)
                            {
                                //replace the file size with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                byte[] buffer = BitConverter.GetBytes(fileSizeArgs.fileSizeToQueryOrSet);
                                Array.Copy(buffer, messageReply.DataBuffer, buffer.Length);
                                messageReply.DataBufferLength = (uint)buffer.Length;
                            }

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileBasicInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreSetFileBasicInfo && IsPreSetFileBasicInfoRegister)
                        {
                            FileBasicInfoEventArgs fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
                            fileBasicInfoArgs.EventName = "OnPreSetFileBasicInfo";

                            //invoke the event delegate
                            OnPreSetFileBasicInfo(this, fileBasicInfoArgs);

                            messageReply.ReturnStatus = (uint)fileBasicInfoArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileBasicInfoArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileBasicInfoArgs.IsDataModified)
                            {
                                //replace the file basic info data with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                int len = Marshal.SizeOf(fileBasicInfoArgs.basicInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileBasicInfoArgs.basicInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileStandardInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreSetFileStandardInfo && IsPreSetFileStandardInfoRegister)
                        {
                            FileStandardInfoEventArgs fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
                            fileStandardInfoArgs.EventName = "OnPreSetFileStandardInfo";

                            //invoke the event delegate
                            OnPreSetFileStandardInfo(this, fileStandardInfoArgs);

                            messageReply.ReturnStatus = (uint)fileStandardInfoArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileStandardInfoArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileStandardInfoArgs.IsDataModified)
                            {
                                //replace the file standardInfo data with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                int len = Marshal.SizeOf(fileStandardInfoArgs.standardInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileStandardInfoArgs.standardInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileNetworkOpenInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPreSetFileNetworkInfo && IsPreSetFileNetworkInfoRegister)
                        {
                            FileNetworkInfoEventArgs fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
                            fileNetworkInfoArgs.EventName = "OnPreSetFileNetworkInfo";

                            //invoke the event delegate
                            OnPreSetFileNetworkInfo(this, fileNetworkInfoArgs);

                            messageReply.ReturnStatus = (uint)fileNetworkInfoArgs.ReturnStatus;

                            if (NtStatus.Status.Success != fileNetworkInfoArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                            else if (fileNetworkInfoArgs.IsDataModified)
                            {
                                //replace the file standardInfo data with your own data. 
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                                int len = Marshal.SizeOf(fileNetworkInfoArgs.networkInfo);
                                IntPtr ptr = Marshal.AllocHGlobal(len);
                                Marshal.StructureToPtr(fileNetworkInfoArgs.networkInfo, ptr, true);
                                Marshal.Copy(ptr, messageReply.DataBuffer, 0, len);
                                Marshal.FreeHGlobal(ptr);
                                messageReply.DataBufferLength = (uint)len;

                            }

                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileRenameInformation
                            || messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileRenameInformationEx)
                    {
                        //if the event was subscribed 
                        if (null != OnPreMoveOrRenameFile && IsPreMoveOrRenameFileRegister)
                        {
                            FileMoveOrRenameEventArgs fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
                            fileRenameArgs.EventName = "OnPreMoveOrRenameFile";

                            //invoke the event delegate
                            OnPreMoveOrRenameFile(this, fileRenameArgs);

                            messageReply.ReturnStatus = (uint)fileRenameArgs.ReturnStatus;
                            if (NtStatus.Status.Success != fileRenameArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileDispositionInformation
                       || messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileDispositionInformationEx)
                    {
                        //if the event was subscribed 
                        if (null != OnPreDeleteFile && IsPreDeleteFileRegister)
                        {
                            FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                            fileIOArgs.EventName = "OnPreDeleteFile";
                            fileIOArgs.Description = "The file is going to be deleted.";

                            //invoke the event delegate
                            OnPreDeleteFile(this, fileIOArgs);

                            messageReply.ReturnStatus = (uint)fileIOArgs.ReturnStatus;
                            if (NtStatus.Status.Success != fileIOArgs.ReturnStatus)
                            {
                                //block the io, and complete the pre-io.
                                messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                            }
                        }
                    }
                    else if (null != OnPreSetFileInfo)
                    {
                        FileInfoArgs fileInfoArgs = new FileInfoArgs(messageSend);
                        fileInfoArgs.EventName = "OnPreSetFileInfo:" + fileInfoArgs.FileInfoClass.ToString();

                        //invoke the event delegate
                        OnPreSetFileInfo(this, fileInfoArgs);

                        messageReply.ReturnStatus = (uint)fileInfoArgs.ReturnStatus;
                        if (NtStatus.Status.Success != fileInfoArgs.ReturnStatus)
                        {
                            //block the io, and complete the pre-io.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }

                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_SET_INFORMATION)
                {
                    if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileEndOfFileInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostSetFileSize && IsPostSetFileSizeRegister)
                        {
                            FileSizeEventArgs fileSizeArgs = new FileSizeEventArgs(messageSend);
                            fileSizeArgs.EventName = "OnPostSetFileSize";

                            //invoke the event delegate
                            OnPostSetFileSize(this, fileSizeArgs);
                            messageReply.ReturnStatus = (uint)fileSizeArgs.ReturnStatus;
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileBasicInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostSetFileBasicInfo && IsPostSetFileBasicInfoRegister)
                        {
                            FileBasicInfoEventArgs fileBasicInfoArgs = new FileBasicInfoEventArgs(messageSend);
                            fileBasicInfoArgs.EventName = "OnPostSetFileBasicInfo";

                            //invoke the event delegate
                            OnPostSetFileBasicInfo(this, fileBasicInfoArgs);
                            messageReply.ReturnStatus = (uint)fileBasicInfoArgs.ReturnStatus;
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileStandardInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostSetFileStandardInfo && IsPostSetFileStandardInfoRegister)
                        {
                            FileStandardInfoEventArgs fileStandardInfoArgs = new FileStandardInfoEventArgs(messageSend);
                            fileStandardInfoArgs.EventName = "OnPostSetFileStandardInfo";

                            //invoke the event delegate
                            OnPostSetFileStandardInfo(this, fileStandardInfoArgs);
                            messageReply.ReturnStatus = (uint)fileStandardInfoArgs.ReturnStatus;
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileNetworkOpenInformation)
                    {
                        //if the event was subscribed 
                        if (null != OnPostSetFileNetworkInfo && IsPostSetFileNetworkInfoRegister)
                        {
                            FileNetworkInfoEventArgs fileNetworkInfoArgs = new FileNetworkInfoEventArgs(messageSend);
                            fileNetworkInfoArgs.EventName = "OnPostSetFileNetworkInfo";

                            //invoke the event delegate
                            OnPostSetFileNetworkInfo(this, fileNetworkInfoArgs);
                            messageReply.ReturnStatus = (uint)fileNetworkInfoArgs.ReturnStatus;
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileRenameInformation
                            || messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileRenameInformationEx)
                    {
                        //if the event was subscribed 
                        if (null != OnPostMoveOrRenameFile && IsPostMoveOrRenameFileRegister)
                        {
                            FileMoveOrRenameEventArgs fileRenameArgs = new FileMoveOrRenameEventArgs(messageSend);
                            fileRenameArgs.EventName = "OnPostMoveOrRenameFile";

                            //invoke the event delegate
                            OnPostMoveOrRenameFile(this, fileRenameArgs);
                            messageReply.ReturnStatus = (uint)fileRenameArgs.ReturnStatus;
                        }
                    }
                    else if (messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileDispositionInformation
                       || messageSend.InfoClass == (uint)WinData.FileInfomationClass.FileDispositionInformationEx)
                    {
                        //if the event was subscribed 
                        if (null != OnPostDeleteFile && IsPostDeleteFileRegister)
                        {
                            FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                            fileIOArgs.EventName = "OnPostDeleteFile";
                            fileIOArgs.Description = "The file was deleted.";

                            //invoke the event delegate
                            OnPostDeleteFile(this, fileIOArgs);
                            messageReply.ReturnStatus = (uint)fileIOArgs.ReturnStatus;

                        }
                    }
                    else if (null != OnPostSetFileInfo)
                    {
                        FileInfoArgs fileInfoArgs = new FileInfoArgs(messageSend);
                        fileInfoArgs.EventName = "OnPostSetFileInfo:" + fileInfoArgs.FileInfoClass.ToString();

                        //invoke the event delegate
                        OnPostSetFileInfo(this, fileInfoArgs);
                        messageReply.ReturnStatus = (uint)fileInfoArgs.ReturnStatus;
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_QUERY_SECURITY)
                {
                    //if the event was subscribed 
                    if (null != OnPreQueryFileSecurity)
                    {
                        FileSecurityEventArgs fileSecurityArgs = new FileSecurityEventArgs(messageSend);
                        fileSecurityArgs.EventName = "OnPreQueryFileSecurity";

                        //invoke the event delegate
                        OnPreQueryFileSecurity(this, fileSecurityArgs);

                        messageReply.ReturnStatus = (uint)fileSecurityArgs.ReturnStatus;

                        if (NtStatus.Status.Success != fileSecurityArgs.ReturnStatus)
                        {
                            //block the io, and complete the pre-io.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }
                        else if (fileSecurityArgs.IsDataModified)
                        {
                            //replace the file security data with your own data. 
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED | (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                            Array.Copy(fileSecurityArgs.securityBuffer, messageReply.DataBuffer, fileSecurityArgs.securityBuffer.Length);
                            messageReply.DataBufferLength = (uint)fileSecurityArgs.securityBuffer.Length;

                        }
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_QUERY_SECURITY)
                {
                    //if the event was subscribed 
                    if (null != OnPostQueryFileSecurity)
                    {
                        FileSecurityEventArgs fileSecurityArgs = new FileSecurityEventArgs(messageSend);
                        fileSecurityArgs.EventName = "OnPostQueryFileSecurity";

                        //invoke the event delegate
                        OnPostQueryFileSecurity(this, fileSecurityArgs);
                        messageReply.ReturnStatus = (uint)fileSecurityArgs.ReturnStatus;
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_SET_SECURITY)
                {
                    //if the event was subscribed 
                    if (null != OnPreSetFileSecurity)
                    {
                        FileSecurityEventArgs fileSecurityArgs = new FileSecurityEventArgs(messageSend);
                        fileSecurityArgs.EventName = "OnPreSetFileSecurity";

                        //invoke the event delegate
                        OnPreSetFileSecurity(this, fileSecurityArgs);
                        if (NtStatus.Status.Success != fileSecurityArgs.ReturnStatus)
                        {
                            //block the io, and complete the pre-io.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }
                        else if (fileSecurityArgs.IsDataModified)
                        {
                            //replace the file security data with your own data. 
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED | (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                            Array.Copy(fileSecurityArgs.securityBuffer, messageReply.DataBuffer, fileSecurityArgs.securityBuffer.Length);
                            messageReply.DataBufferLength = (uint)fileSecurityArgs.securityBuffer.Length;

                        }

                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_SET_SECURITY)
                {
                    //if the event was subscribed 
                    if (null != OnPostSetFileSecurity)
                    {
                        FileSecurityEventArgs fileSecurityArgs = new FileSecurityEventArgs(messageSend);
                        fileSecurityArgs.EventName = "OnPostSetFileSecurity";

                        //invoke the event delegate
                        OnPostSetFileSecurity(this, fileSecurityArgs);
                        messageReply.ReturnStatus = (uint)fileSecurityArgs.ReturnStatus;
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_DIRECTORY)
                {
                    //if the event was subscribed 
                    if (null != OnPreQueryDirectoryFile)
                    {
                        FileQueryDirectoryEventArgs directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
                        directoryArgs.EventName = "OnPreQueryDirectoryFile";

                        //invoke the event delegate
                        OnPreQueryDirectoryFile(this, directoryArgs);

                        messageReply.ReturnStatus = (uint)directoryArgs.ReturnStatus;

                        if (NtStatus.Status.Success != directoryArgs.ReturnStatus)
                        {
                            //block the io, and complete the pre-io.
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;
                        }
                        else if (directoryArgs.IsDataModified)
                        {
                            //replace the directory data with your own data. 
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED | (uint)FilterAPI.FilterStatus.FILTER_COMPLETE_PRE_OPERATION;

                            Array.Copy(directoryArgs.directoryBuffer, messageReply.DataBuffer, directoryArgs.directoryBuffer.Length);
                            messageReply.DataBufferLength = (uint)directoryArgs.directoryBuffer.Length;

                        }
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_DIRECTORY)
                {
                    //if the event was subscribed 
                    if (null != OnPostQueryDirectoryFile)
                    {
                        FileQueryDirectoryEventArgs directoryArgs = new FileQueryDirectoryEventArgs(messageSend);
                        directoryArgs.EventName = "OnPostQueryDirectoryFile";

                        //invoke the event delegate
                        OnPostQueryDirectoryFile(this, directoryArgs);

                        messageReply.ReturnStatus = (uint)directoryArgs.ReturnStatus;

                        if (directoryArgs.IsDataModified)
                        {
                            //replace the directory data with your own data. 
                            messageReply.FilterStatus = (uint)FilterAPI.FilterStatus.FILTER_DATA_BUFFER_IS_UPDATED;

                            Array.Copy(directoryArgs.directoryBuffer, messageReply.DataBuffer, directoryArgs.directoryBuffer.Length);
                            messageReply.DataBufferLength = (uint)directoryArgs.directoryBuffer.Length;

                        }
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CLEANUP)
                {
                    //if the event was subscribed 
                    if (null != OnPreFileHandleClose)
                    {
                        FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                        fileIOArgs.EventName = "OnPreFileHandleClose";
                        fileIOArgs.Description = "The opened file handle is going to close.";

                        //invoke the event delegate
                        OnPreFileHandleClose(this, fileIOArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CLEANUP)
                {
                    //if the event was subscribed 
                    if (null != OnPostFileHandleClose)
                    {
                        FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                        fileIOArgs.EventName = "OnPostFileHandleClose";
                        fileIOArgs.Description = "The opened file handle was closed.";

                        //invoke the event delegate
                        OnPostFileHandleClose(this, fileIOArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.PRE_CLOSE)
                {
                    //if the event was subscribed 
                    if (null != OnPreFileClose)
                    {
                        FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                        fileIOArgs.EventName = "OnPreFileClose";
                        fileIOArgs.Description = "All references to the fileObject " + messageSend.FileObject.ToString("X") + " are going to close.";

                        //invoke the event delegate
                        OnPreFileClose(this, fileIOArgs);
                    }
                }
                else if (messageSend.MessageType == (uint)FilterAPI.MessageType.POST_CLOSE)
                {
                    //if the event was subscribed 
                    if (null != OnPostFileClose)
                    {
                        FileIOEventArgs fileIOArgs = new FileIOEventArgs(messageSend);
                        fileIOArgs.EventName = "OnPostFileClose";
                        fileIOArgs.Description = "All references to the fileObject " + messageSend.FileObject.ToString("X") + " were closed.";

                        //invoke the event delegate
                        OnPostFileClose(this, fileIOArgs);
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("FileControlEvents exception:" + ex.Message);
            }


            return retVal;
        }
    }
}
