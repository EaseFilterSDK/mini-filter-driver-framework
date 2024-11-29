EaseFilter File System Filter Driver SDK ReadMe

Known Issues:
1. The transparent encryption doesn't support the earlier office 97-2003 version real time encryption.
2. The transparent encryption doesn't support the Database real time encryption.
3. To install the driver in Windows 7/8/8.1/2008R2/2012/2012R2, the KB3033929 is required.

What can yo do with EaseFilter SDK?
1. The File Monitor Filter Driver allows you to monitor the file I/O activities, get the notification when the file was changed.
2. The File Control Filter Driver allows you to control the file access, modify the file I/O data.
3. The File Encryption Filter Driver allows you to encrypt or decrypt the files transparently.
4. The Process Filter Driver allows you to get the notification when the process was created or terminated, prevent untrusted process from being launching.
5. The Registry Filter Driver allows you to monitor the registry access, block the registry change by the unauthorized processes.

How to use EaseFilter SDK?

The EaseFilter SDK includes two components (EaseFlt.sys and FilterAPI.dll), The EaseFlt.sys and FilterAPI.dll are different for 32bit and 64bit windows system. 
EaseFlt.sys is the file system filter driver which implements all the functionalities in the file system level. 
FilterAPI.dll is a wrapper DLL which exports the API to the user mode applications. 
The right platform(32bit or 64bit) EaseFlt.sys and FilterAPI.dll have to be copied to the same folder of your application..

For C++ application:
Copy FilterAPI.h and FilterAPI.lib to your application folder, add "FilterAPI.h" to your C++ project, include "FilterAPI.lib" to your project.
We provide a C++ example project "EaseFltCPPDemo" for your reference, it demostrates how to use EaseFilter SDK in C++.

For C# application:
You need to add the reference "FilterControl" to your project. Include the namespace "EaseFilter.FilterControl" in your project files to use the FilterAPI interface.

Below are some c# projects to demostrate the EaseFilter SDK used cases in different scenarios:

1.FileMonitorConsole
This is simple console application to demostrate how to use the File Monitor Filter Driver SDK. For more funtionalities you can reference the GUI demo project "FileMonitor".

2.FileProtectorConsole
This is simple console application to demostrate how to use the File Control Filter Driver SDK. For more funtionalities you can reference the GUI demo project "FileProtector".

3. EaseFltCSConsoleDemo
This is a simple C# console project to demostrate how to use File Monitor and Control Filter Driver SDK, how to run as Windows service.

4. AutoEncryptDemo.exe
This is a simple encryption demo project. You can create an auto encryption folder in computer A, all new created files in this folder will be encrypted. You can setup the authorized
processes or users who can decrypt the encrypted files in computer A. The encrypted files can be copied or transferred to the other computer B, to keep the files encrypted, 
make sure the copy or transfer application was not included in the authorized process list, or you will copy the decrypted files instead of the encrypted files. You can create
an auto decryption folder in computer B, you can copy the encrypted files to this folder, the files are still kept encrypted in the disk, you can setup the authorized processes 
or users who can decrypt the encrypted files in computer B.

5. AutoFileCryptTool.exe
This is another encryption demo project with more settings to create multiple encryption folders with auhorized or unauthorized processes or users setting.

6. FileMonitor.exe
This project demostrates how to monitor the file I/Os in real time, track the file access and file change.

7. FileProtector.exe
This project demostrates how to monitor the file I/Os, allow or deny the file access to the specific folders, authorized the users or processes to access the sensitive files in the folder.

8. SecureShare.exe
This project demostrates how to share the encrypted file with embedded digital rights, you can grant or revoke your shared files at anytime and anywhere, and track who and when your shared files were accessed.

9. FolderLocker.exe
This project demostrates how to prevent your files being read/written/deleted/renamed in protected folders , it also allow you to encrypt the files, hide the files in the folder, secure file sharing.

10. RegMon.exe
This project demostrates how to track the registry access activities, to know who query the registry key/value, to protect your registry key being modified or deleted.

11. ProcessMon.exe
This project demostrates how to track the process creation or termination, prevent the suspicious executable binaries( malware) from launching.

12. SecureSandbox.exe
This project demostrate how to create a sandbox folder, prevent the suspicious executable binaries( malware) from launching inside the sandbox, protect the files inside the sandbox, prevent the files from being read or written.


