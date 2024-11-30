# [mini-filter-driver-framework](https://www.easefilter.com/Forums_Files/Comprehensive-file-security-sdk.htm)
A mini filter driver development framework allows you to develop minit filter driver with different features.

## Windows File System Filter Driver
A file system filter driver intercepts requests targeted at a file system or another file system filter driver. By intercepting the request before it reaches its intended target, the filter driver can extend or replace functionality provided by the original target of the request. File system filtering services are available through the filter manager in Windows. The Filter Manager provides a framework for developing File Systems and File System Filter Drivers without having to manage all the complexities of file I/O. The Filter Manager simplifies the development of third-party filter drivers and solves many of the problems with the existing legacy filter driver model, such as the ability to control load order through an assigned altitude. A filter driver developed to the Filter Manager model is called a minifilter. Every minifilter driver has an assigned altitude, which is a unique identifier that determines where the minifilter is loaded relative to other minifilters in the I/O stack. Altitudes are allocated and managed by Microsoft.

![Filter Model](https://www.easefilter.com/images/filter-manager-architecture.gif)

Even to an experienced developer, developing file system filter driver is certainly a challenge. To develop the filter driver, you can use the WDK, a software toolset from Microsoft that enables the development of device drivers for the Microsoft Windows platform. It includes documentation, samples, build environments, and tools for driver developers. To simplify your development and to provide you with a robust and well-tested file system filter driver that works with all versions and patch releases of the Windows operating systems supported by Microsoft, EaseFilter filter driver SDK will be your best choice, it provides a complete, modular environment for building active file system filters in your application.

## EaseFilter Mini Filter Driver Framework
One might think that writing a file system filter would be a much easier task, since there are lots of starter samples available on internet, but the truth is to write a professional commercial file system filters in the real world is very hard.Sometime writing a file system filter driver is harder than a file system. EaseFilter Mini Filter Driver Framework can provide you a complete, modular environment for building active file system filter driver in your application. 

## What Can You Do With EaseFilter Mini Filter Driver Framework

### [File Monitor/File Audit - Track file change on the fly](https://www.easefilter.com/Forums_Files/FileMonitor.htm)
File System Tiered Storage Filter Driver SDK, is a data storage technique which automatically moves data between high-cost and low-cost storage media. Tiered Storage Filter systems exist because high-speed storage devices, such as hard disk drive arrays, are more expensive (per byte stored) than slower devices, such as optical discs and magnetic tape drives. Tiered Storage Filter systems store the bulk of the enterprise’s data on slower devices. A stub is created for and replaces each migrated file in the fast disk drives. On the local system, a stub file looks and act like a regular file. When you or a Windows application accesses a migrated file stub, the Windows operating system transparently directs a file access request to the Tiered Storage Filter driver. This driver retrieves the full file from the repository to which it was migrated.

![Filter Monitor](https://www.easefilter.com/images/MonitorScreenshot.png)

### [File Protector - Prevent your sensitive data from being accessed by unauthorized users or processes](https://www.easefilter.com/Forums_Files/FileProtector.htm)
File system control filter can control the file activities, which you can intercept the file system call, modify its content before or after the request goes down to the file system, allow/deny/cancel its execution based on the filter rule. You can fully control file open/create/overwrite, read/write, query/set file attribute/size/time security information, rename/delete, directory browsing these Io requests. With file system control filter you can developer these kinds of software:

1.  Create your Data protection Software. Block accessing your data based on your security policy, prevent data modification without permission.
2.  Create your own encryption software via encrypt the write data and decrypt the read data.
3.  Create your own custom security policies to control the file access.
4.  Hide or replace the files in the directory. You can modify the directory buffer to hide some files or change file name.

![Filter Protector](https://www.easefilter.com/images/ControlFilter.png)

### [File Encryption Filter Driver Framework - Transparent on-access, per-file encryption](https://www.easefilter.com/Forums_Files/Transparent_Encryption_Filter_Driver.htm)
EaseFilter File system encryption filter driver SDK provides a comprehensive solution for transparent file level encryption. It allows developers to create transparent encryption products which it can encrypt or decrypt files on-the-fly, it can allow only authorized users or processes can access the encrypted files.

Supported strong cryptographic algorithm Rijndael is a high security algorithm which was chosen by the National Institute of Standards and Technology (NIST) as the new Advanced Encryption Standard (AES), it can support key length 128-bits,192-bits and 256-bits.

![Filter Encryption](https://www.easefilter.com/images/TransparentFileEncryption.png)

### [Secure File Sharing - Control share file access with digital rights embedded](https://www.easefilter.com/Forums_Files/AssureFiles_Secure_File_Sharing.htm)
Encrypt the file with 256-bits key,and embed with the digital rights management protection, only the authorized users, processes and computers can access the encrypted file.Share your files with fully control, you can expire or revoke the file access at any time, even after the file has been shared. Add or remove the authorized users, processes and computers at any time.

![secure file sharing](https://www.easefilter.com/images/SecureSharing.png)

### [Registry access monitoring and protection](https://www.easefilter.com/Forums_Files/RegMon.htm)
Monitoring registry calls to track the registry changes. When the registry key, value or security was modified, the callback routine will be invoked with a data structure that contains information that is specific to the type of registry operation.

Blocking registry calls to prevent your registry from being changed by unauthorized processes. When the registry key, value or security is going to be modified, the callback routine will be invoked with a data structure that contains information that is specific to the type of registry operation, If a RegistryCallback routine returns a status value "STATUS_ACCESS_DENIED" for a pre-notification, this registry operation will be blocked and the error code will be returned.

Modifying registry calls to create virtual registry key or value.

![registry filter](https://www.easefilter.com/images/registryScreenshot.png)

### [Process access monitoring and protection](https://www.easefilter.com/Forums_Files/Process-Monitor.htm)
Monitoring the process and thread creation or termination, get the notification of the process and thread operations when you register the events. Prevent the untrusted executable binaries ( malwares) from being launched, protect your data being damaged by the untrusted processes.

![process filter](https://www.easefilter.com/images/processScreenshot.png)

### [Secure Sandbox Solution](https://www.easefilter.com/Forums_Files/Secure-Sandbox.htm)
A sandbox is a secure, isolated and a tightly controlled environment where programs can be run and data can be protected. Sandboxes restrict what a piece of code can do, giving it just as many, permissions as it needs without adding additional permissions that could be abused. Prevent malicious or malfunctioning programs from running.Run untrusted Windows programs safely in Easefilter Secure Sandbox. Protect your confidential files in Easefilter Secure Sandbox.

![secure sandbox](https://www.easefilter.com/images/secureSandboxScreenshot.png)


## EaseFilter File System Filter Driver SDK Reference
| Product Name | Description |
| --- | --- |
| [File Monitor SDK](https://www.easefilter.com/kb/file-monitor-filter-driver-sdk.htm) | EaseFilter File Monitor Filter Driver SDK Introduction. |
| [File Control SDK](https://www.easefilter.com/kb/file-control-file-security-sdk.htm) | EaseFilter File Control Filter Driver SDK Introduction. |
| [File Encryption SDK](https://www.easefilter.com/kb/transparent-file-encryption-filter-driver-sdk.htm) | EaseFilter Transparent File Encryption Filter Driver SDK Introduction. |
| [Registry Filter SDK](https://www.easefilter.com/kb/registry-filter-drive-sdk.htm) | EaseFilter Registry Filter Driver SDK Introduction. |
| [Process Filter SDK](https://www.easefilter.com/kb/process-filter-driver-sdk.htm) | EaseFilter Process Filter Driver SDK Introduction. |
| [Storage Tiering SDK](https://www.easefilter.com/cloud/storage-tiering-sdk.htm) | EaseFilter Storage Tiering Filter Driver SDK Introduction. |
| [EaseFilter SDK Programming](https://www.easefilter.com/kb/programming.htm) | EaseFilter Filter Driver SDK Programming. |

## EaseFilter SDK Sample Projects
| Sample Project | Description |
| --- | --- |
| [Auto File DRM Encryption](https://www.easefilter.com/kb/auto-file-drm-encryption-tool.htm) | Auto file encryption with DRM data embedded. |
| [Transparent File Encrypt](https://www.easefilter.com/kb/AutoFileEncryption.htm) | Transparent on access file encryption. |
| [Secure File Sharing with DRM](https://www.easefilter.com/kb/DRM_Secure_File_Sharing.htm) | Secure encrypted file sharing with digital rights management. |
| [File Monitor Example](https://www.easefilter.com/kb/file-monitor-demo.htm) | Monitor file system I/O in real time, tracking file changes. |
| [File Protector Example](https://www.easefilter.com/kb/file-protector-demo.htm) | Prevent sensitive files from being accessed by unauthorized users or processes. |
| [FolderLocker Example](https://www.easefilter.com/kb/FolderLocker.htm) | Lock file automatically in a FolderLocker. |
| [Process Monitor](https://www.easefilter.com/kb/Process-Monitor.htm) | Monitor the process creation and termination, block unauthorized process running. |
| [Registry Monitor](https://www.easefilter.com/kb/RegMon.htm) | Monitor the Registry activities, block the modification of the Registry keys. |
| [Secure Sandbox Example](https://www.easefilter.com/kb/Secure-Sandbox.htm) |A secure sandbox example, block the processes accessing the files out of the box. |
| [FileSystemWatcher Example](https://www.easefilter.com/kb/FileSystemWatcher.htm) | File system watcher, logging the file I/O events. |

## Filter Driver Reference

* Understand MiniFilter Driver: https://www.easefilter.com/kb/understand-minifilter.htm
* Understand File I/O: https://www.easefilter.com/kb/File_IO.htm
* Understand I/O Request Packets(IRPs):https://www.easefilter.com/kb/understand-irps.htm
* Filter Driver Developer Guide: https://www.easefilter.com/kb/DeveloperGuide.htm
* MiniFilter Filter Driver Framework: https://www.easefilter.com/kb/minifilter-framework.htm
* Isolation Filter Driver: https://www.easefilter.com/kb/Isolation_Filter_Driver.htm

## Support
If you have questions or need help, please contact support@easefilter.com 

[Home](https://www.easefilter.com/) | [Solution](https://www.easefilter.com/solutions.htm) | [Download](https://www.easefilter.com/download.htm) | [Demos](https://www.easefilter.com/online-fileio-test.aspx) | [Blog](https://blog.easefilter.com/) | [Programming](https://www.easefilter.com/kb/programming.htm)


