# EaseFilter Java Demo

This is a collection of simple CLI demos for [EaseFilter](https://easefilter.com).
The source code is thoroughly documented, and serves as an example for using EaseFilter.

Java 17 and above is supported.
**A license key is required** to use this program.
Contact [info@easefilter.com](mailto:info@easefilter.com)
for a trial key.

The provided demos include:

* Monitoring of file/folder access

* Control of file/folder access

* Filesystem-level encryption of specific folders and files

* Monitoring of the Windows Registry

* Monitoring of specific processes

## Installation

> [!NOTE]
> Administrator permissions are required to run EaseFilter.

- [Install Java.](https://learn.microsoft.com/en-us/java/openjdk/install)

- [Install Maven.](https://maven.apache.org/install.html)

- Edit the configuration file at `src\main\resources\config.properties` and add the license key.

- Compile the demo CLI:

  ```
  mvn package
  ```
  
- You can now run the demo:

  ```
  .\ef-demo --help
  ```

## Examples

Here are some examples of using the demo CLI.
These commands assume you are using `cmd` or `powershell`;
on a different shell, quote the arguments appropriately.

To get help with a specific subcommand, you may pass `--help` to it, for example

```
.\ef-demo monitor --help
```

to get help for using the monitor filter demo.

### Monitor filter

- Monitor all events in the filesystem:
  ```
  .\ef-demo monitor *
  ```

- Monitor all events in the `C:\` drive:
  ```
  .\ef-demo monitor C:\*
  ```
  
- Monitor all events in a specific directory:
  ```
  .\ef-demo monitor C:\path\to\directory\*
  ```
  
### Control filter

Deny all file writes in a directory:

  ```
  .\ef-demo control --block-perms="ALLOW_WRITE_ACCESS,ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS" C:\path\to\directory\*
  ```
  
Deny all reads in the directory:

  ```
  .\ef-demo control --block-perms="ALLOW_READ_ACCESS" C:\path\to\directory\*
  ```

Deny file listing in a directory (makes directory seem empty):

  ```
  .\ef-demo control --block-perms="ALLOW_DIRECTORY_LIST_ACCESS" C:\path\to\directory\*
  ```

### Process filter

Monitor all process creations/deletions (try starting a PowerShell to test this):

  ```
  .\ef-demo process *
  ```

Monitor specifically `cmd.exe`:

  ```
  .\ef-demo process C:\Windows\System32\cmd.exe
  ```

Monitor all System32 processes:

  ```
  .\ef-demo process C:\Windows\System32\*
  ```

(BE CAREFUL with this flag) Prevent `cmd.exe` from starting:

  ```
  .\ef-demo process --deny C:\Windows\System32\cmd.exe
  ```

### Encryption filter

Transparently encrypt a directory:

  ```
  .\ef-demo encrypt C:\path\to\directory\*
  ```

This will prompt for a password to encrypt the directory with.
New files written to the directory will be encrypted.
Once the filter is stopped, files contents will be scrambled and unreadable.

> [!WARNING]
> In this simplified demo program, the encryption key is not generated securely.
> Use a proper key derivation method with a randomly-generated salt.

Encrypt a directory, and only allow Notepad to decrypt its contents:

  ```
  .\ef-demo encrypt --allow-proc notepad.exe C:\path\to\directory\*
  ```
  
### Registry filter

Monitor all registry events:

  ```
  .\ef-demo registry *
  ```

Monitor all registry events for keys with a matching name:

  ```
  .\ef-demo registry *KeyName*
  ```

Prevent deletion of keys with a matching name:

  ```
  .\ef-demo registry --block-perms=DELETE_KEY *KeyName*
  ```
  
Run `.\ef-demo registry --block-perms=HELP` to see a list of permissions that can be managed by EaseFilter.
