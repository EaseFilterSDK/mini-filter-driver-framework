###############################################################################
##
##    (C) Copyright 2024 EaseFilter Technologies
##    All Rights Reserved
##
##    This software is part of a licensed software product and may
##    only be used or copied in accordance with the terms of that license.
##
##    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
##
##    This module contains sample code provided for convenience and
##    demonstration purposes only,this software is provided on an
##    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
##     either express or implied.
##
###############################################################################

"""Low-level EaseFilter DLL interface + Windows functions.

This API is faithful to FilterApi.h in the C++
demo, although some parts are unimplemented.
"""

from __future__ import annotations

import ctypes
import datetime
import logging
from ctypes import (
    POINTER,
    WINFUNCTYPE,
    Array,
    Structure,
    Union,
    addressof,
    byref,
    c_longlong,
    c_ubyte,
    c_uint,
    c_ulong,
    c_ulonglong,
    c_void_p,
    c_wchar_p,
    cast,
    create_string_buffer,
    create_unicode_buffer,
)
from ctypes.wintypes import (
    BOOL,
    DWORD,
    HANDLE,
    HMODULE,
    LPCWSTR,
    LPDWORD,
    LPWSTR,
    MAX_PATH,
    PCHAR,
    PULONG,
    PWCHAR,
    ULONG,
    WCHAR,
)
from dataclasses import dataclass
from datetime import timezone
from pathlib import Path
from typing import (
    TYPE_CHECKING,
    Annotated,
    Any,
    get_args,
    get_origin,
    get_type_hints,
)

if TYPE_CHECKING:
    from collections.abc import Callable

    from easefilter.enums import (
        AccessFlag,
        BooleanConfig,
        FilterType,
        IOCallbackClass,
        ProcessControlFlag,
        RegCallbackClass,
        RegControlFlag,
    )

################################################################
################################################################

# utility functions

################################################################
################################################################


def windows_timestamp(t: int) -> datetime.datetime:
    """Convert a Windows 'file time' to a Python datetime.

    A file time is a 64-bit int that represents the number of 100-nanosecond
    intervals elapsed since 12:00 AM on January 1, 1601 UTC.

    Args:
        t: File time (64-bit signed integer).
    """
    us = t / 10
    epoch = datetime.datetime(1601, 1, 1, tzinfo=timezone.utc)
    return epoch + datetime.timedelta(microseconds=us)


def windows_check_error(ret: int) -> None:
    """Ctypes error-check function on Windows API functions that return BOOL."""
    if ret == 0:
        raise ctypes.WinError(ctypes.get_last_error())


PSID = ctypes.c_void_p


def ConvertSidToStringSidW(sid: Array[c_ubyte]) -> str:
    fct: ctypes._FuncPointer = ctypes.windll.Advapi32.ConvertSidToStringSidW
    fct.argtypes = [PSID, POINTER(LPWSTR)]
    fct.restype = BOOL
    fct.restype = windows_check_error

    ptr = LPWSTR()
    fct(sid, byref(ptr))

    if ptr.value is None:
        msg = "Got unexpected null pointer."
        raise RuntimeError(msg)

    return ptr.value


@dataclass
class AccountData:
    """A user account (Python representation)."""

    username: str

    domain: str
    """User's domain name."""


def LookupAccountSidW(sid: Array[c_ubyte]) -> AccountData:
    """Look up account information based on an Sid struct.

    See https://learn.microsoft.com/en-us/windows/win32/api/winbase/nf-winbase-lookupaccountsidw
    """
    fct = ctypes.windll.Advapi32.LookupAccountSidW
    fct.argtypes = [
        LPCWSTR,
        PSID,
        LPWSTR,
        LPDWORD,
        LPWSTR,
        LPDWORD,
        POINTER(c_uint),
    ]
    fct.restype = windows_check_error

    # these have the sizes of our buffers. if they're too small they get updated to what the size should be
    cch_user = DWORD(MAX_PATH)
    cch_domain = DWORD(MAX_PATH)

    user_str = create_unicode_buffer("", size=MAX_PATH)
    domain_str = create_unicode_buffer("", size=MAX_PATH)

    # SID_NAME_USE enum, which is discarded
    peUse = c_uint()
    fct(
        cast(0, LPCWSTR),
        sid,
        user_str,
        byref(cch_user),
        domain_str,
        byref(cch_domain),
        byref(peUse),
    )

    if user_str.value is None:
        msg = "Got empty username."
        raise RuntimeError(msg)

    if domain_str.value is None:
        msg = "Got empty domain name."
        raise RuntimeError(msg)

    return AccountData(username=user_str.value, domain=domain_str.value)


# https://learn.microsoft.com/en-us/windows/win32/api/psapi/nf-psapi-getmodulefilenameexw
GetModuleFileNameExW = ctypes.windll.psapi.GetModuleFileNameExW
GetModuleFileNameExW.restype = windows_check_error
GetModuleFileNameExW.argtypes = [HANDLE, HMODULE, LPWSTR, DWORD]

# https://learn.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-openprocess
OpenProcess = ctypes.windll.kernel32.OpenProcess
OpenProcess.restype = HANDLE
OpenProcess.argtypes = [DWORD, BOOL, DWORD]


def GetProcessPathByPid(pid: c_ulong) -> str | None:
    PROCESS_QUERY_INFORMATION = 0x0400
    PROCESS_VM_READ = 0x0010

    try:
        h = OpenProcess(PROCESS_VM_READ | PROCESS_QUERY_INFORMATION, False, pid)  # noqa: FBT003 (boolean positional arg)
        if h is None:
            logging.debug("Could not open process handle for PID %s.", pid)
            return None
        name = create_unicode_buffer("", MAX_PATH)
        GetModuleFileNameExW(h, 0, name, MAX_PATH)
    except OSError:
        logging.debug("Could not get process path for PID %d.", pid)
        return None
    else:
        return name.value


def _pretty_fields(dct: dict[str, Any]) -> None:
    """Convert a namespace's type annotations to _fields_ in ctypes structs/unions."""

    class TypeDummy:
        __annotations__ = dct.get("__annotations__", {})

    # this line helps with [PEP 563](https://peps.python.org/pep-0563/)
    # some type hints may be strings, and this will convert them back to real code
    fields = get_type_hints(TypeDummy, localns=dct, include_extras=True)

    old_fields: list[tuple[str, Any]] = dct.get("_fields_", [])
    field_list: list[tuple[str, Any]] = []

    # this relies on dictionaries preserving insertion order to preserve struct field order
    # see https://stackoverflow.com/a/39980744
    for k, v in fields.items():
        # special treatment for arrays
        # example syntax: Annotated[Array[c_ubyte], 16]
        if get_origin(v) is Annotated:
            args = get_args(v)
            arr_len = args[1]
            arr_type = get_args(args[0])[0]
            field_list.append((k, arr_type * arr_len))
        else:
            field_list.append((k, v))

    dct["_fields_"] = field_list + old_fields


class PrettyStruct(type(Structure)):
    """Metaclass to make struct definitions pretty in Python.

    (For unions, see `PrettyUnion`.)

    By default, struct definitions look like this::

    ```
    class Foo(Structure):
        _fields_ = [
            ("arr", c_int * 5),
            ("x", c_int),
        ]
    ```

    This metaclass gives both type hints, cleaner syntax, and the possibility for docstrings::

    ```
    class Foo(Structure, metaclass=PrettyStruct):
        arr: Annotated[Array[c_int], 5]
        '''This is an array type.'''

        y: c_int
        '''This is an integer.'''
    ```

    Remember to use `typing.Annotated` for array types, as type checkers don't understand the ctypes array syntax.
    You can use the ctypes `type * len` syntax, but you will have to ignore the type checker errors.

    In case some fields can not be expressed in pretty syntax, you can fall back to `_fields_` syntax.
    Fields defined in `_fields_` syntax will be invariably placed AFTER the pretty fields.
    It is not optimal to mix pretty and `_fields_` syntax, as field order is hard to define.
    """

    def __new__(
        cls, name: str, base: tuple[type], dct: dict[str, Any]
    ) -> type[Structure]:
        _pretty_fields(dct)
        return super().__new__(cls, name, base, dct)


class PrettyUnion(type(Union)):
    """Metaclass to make union definitions cleaner.

    See `PrettyStruct`.
    """

    def __new__(cls, name: str, base: tuple[type], dct: dict[str, Any]) -> type[Union]:
        _pretty_fields(dct)
        return super().__new__(cls, name, base, dct)


################################################################
################################################################

# struct definitions

################################################################
################################################################


################################
# filter-userspace communication
################################

MESSAGE_SEND_VERIFICATION_NUMBER = 0xFF000001

INET_ADDR_STR_LEN = 22
MAX_FILE_NAME_LENGTH = 1024
MAX_SID_LENGTH = 256
MAX_BUFFER_SIZE = 16384
AES_MAX_TAG_DATA_SIZE = 914


class MESSAGE_SEND_DATA(Structure, metaclass=PrettyStruct):
    """Structure for driver to app communication."""

    VerificationNumber: ULONG
    """Field for data integrity checking (should be equal to `MESSAGE_SEND_VERIFICATION_NUMBER`)."""

    FilterCommand: ULONG
    """Command ID sent by the driver."""

    MessageId: ULONG
    """Sequential message ID."""

    FilterRuleId: ULONG
    """ID of the filter rule associated to this message."""

    RemoteIP: Annotated[Array[WCHAR], INET_ADDR_STR_LEN]
    """IP address of remote computer for files accessed over SMB (Windows 7+ only)."""

    FileObject: c_void_p
    """Equivalent to file handle. Unique per file stream opened."""

    FsContext: c_void_p
    """Context, unique per file."""

    MessageType: c_longlong
    """File I/O message type registry class."""

    ProcessId: ULONG
    """PID for the thread that originally requested the I/O operation."""

    ThreadId: ULONG
    """Thread ID that requested the I/O operation."""

    Offset: c_longlong
    """Read/write offset."""

    Length: ULONG
    """Read/write length."""

    FileSize: c_longlong
    """Size of the file affected by the I/O operation."""

    TransactionTime: c_longlong
    """Transaction time of this request, UTC."""

    CreationTime: c_longlong
    """Creation time of the file, UTC."""

    LastAccessTime: c_longlong
    """Last access time of the file, UTC."""

    LastWriteTime: c_longlong
    """Last write time of the file, UTC."""

    FileAttributes: ULONG
    """File attributes."""

    DesiredAccess: ULONG
    """See the CreateFile Windows API."""
    Disposition: ULONG
    """See the CreateFile Windows API."""
    ShareAccess: ULONG
    """See the CreateFile Windows API."""
    CreateOptions: ULONG
    """See the CreateFile Windows API."""
    CreateStatus: ULONG
    """See the CreateFile Windows API."""

    InfoClass: ULONG
    """Information class for security/directory/information I/O"""

    Status: ULONG
    """File system I/O status."""

    ReturnLength: ULONG
    """Return I/O (read/write) length."""

    FileNameLength: ULONG
    """File name length, in bytes."""

    FileName: Annotated[Array[WCHAR], MAX_FILE_NAME_LENGTH]
    """Name of the file affected by the I/O."""

    SidLength: ULONG
    """Security identifier length."""

    Sid: Annotated[Array[c_ubyte], MAX_SID_LENGTH]
    """Security identifier."""

    DataBufferLength: ULONG
    """Length of data buffer."""

    DataBuffer: Annotated[Array[c_ubyte], MAX_BUFFER_SIZE]
    """Data for read/write/query info/set info operations."""


class MESSAGE_REPLY_DATA(Structure, metaclass=PrettyStruct):
    """Data returned from user mode to the filter driver."""

    class _ReplyData(Union, metaclass=PrettyUnion):
        class _Data(Structure, metaclass=PrettyStruct):
            BLOCK_SIZE = 65536

            DataBufferLength: ULONG
            DataBuffer: Annotated[Array[c_ubyte], BLOCK_SIZE]

        class _AESData(Structure, metaclass=PrettyStruct):
            class _Data(Structure, metaclass=PrettyStruct):
                AccessFlag: ULONG
                IVLength: ULONG
                IV: Annotated[Array[c_ubyte], 16]
                EncryptionKeyLength: ULONG
                EncryptionKey: Annotated[Array[c_ubyte], 32]
                TagDataLength: ULONG
                TagData: Annotated[Array[c_ubyte], 1]

            SizeOfData: ULONG
            Data: _Data

        class _UserInfo(Structure, metaclass=PrettyStruct):
            UserNameLength: ULONG
            UserName: PWCHAR

        class _FileInfo(Structure, metaclass=PrettyStruct):
            FileNameLength: ULONG
            FileName: PWCHAR

        Data: _Data
        AESData: _AESData
        UserInfo: _UserInfo
        FileInfo: _FileInfo

    MessageId: ULONG
    MessageType: ULONG
    ReturnStatus: ULONG
    FilterStatus: ULONG
    ReplyData: _ReplyData


class PROCESS_INFO(Structure, metaclass=PrettyStruct):
    """Supplementary data for a PROCESS mode event.

    This data can be found under `MESSAGE_SEND_DATA.DataBuffer`.

    The executable file path (image name) can be found in `MESSAGE_SEND_DATA.FileName`.
    """

    ParentProcessId: ULONG
    """Parent PID. May be different from CreatingProcessId!"""

    CreatingProcessId: ULONG
    """PID of the creator of this process."""

    CreatingThreadId: ULONG
    """TID of the creator of this process."""

    DesiredAccess: ULONG
    """An `ACCESS_MASK` value that specifies the access rights to grant for the handle for `OB_PRE_CREATE_HANDLE_INFORMATION`."""

    Operation: ULONG
    """The type of handle operation.

    May be: `OB_OPERATION_HANDLE_CREATE`, `OB_OPERATION_HANDLE_DUPLICATE`.
    """

    FileOpenNameAvailable: ULONG
    """Boolean, specifies whether `ImageFileName` contains the exact file name that is used to open the process executable file."""

    CommandLineLength: ULONG
    """Length of the CommandLine field."""

    CommandLine: Annotated[Array[WCHAR], MAX_FILE_NAME_LENGTH]
    """Command used to start a process, including arguments."""


PPROCESS_INFO = POINTER(PROCESS_INFO)


################################################################
################################################################

# Filter API class

################################################################
################################################################


class FilterApi:
    """Low-level interface for the Filter API.

    Functions that return `int` should return `1` for success and `0` for failure.

    Args:
        dll_path: Path to Filter API DLL file.
    """

    def __init__(self, dll_path: Path | None = None) -> None:
        """Loads the DLL and stores the handle."""
        path: Path = (
            dll_path if dll_path else Path(__file__).parent / "bin" / "FilterApi.dll"
        )
        self._dll: ctypes.CDLL = ctypes.cdll.LoadLibrary(str(path))

    def GetLastErrorMessage(self) -> str:
        """Get an error message from EaseFilter.

        Returns:
            A string with the error message. Empty if there is none.

        Raises:
            OSError: EaseFilter could not retrieve the error message.
        """
        length = ULONG(0)

        # Retrieve message length
        self._dll.GetLastErrorMessage(None, cast(addressof(length), PULONG))

        buf = create_unicode_buffer(length.value)

        # Retrieve message contents
        self._dll.GetLastErrorMessage(buf, cast(addressof(length), PULONG))

        return buf.value

    ################################
    ## driver and service control
    ################################

    def InstallDriver(self) -> int:
        """Installs the EaseFilter Windows driver."""
        return self._dll.InstallDriver()

    def UnInstallDriver(self) -> int:
        """Uninstalls the EaseFilter Windows driver."""
        return self._dll.UnInstallDriver()

    def IsDriverServiceRunning(self) -> bool:
        """Check if driver service is running."""
        return self._dll.IsDriverServiceRunning()

    def SetRegistrationKey(self, key: str) -> int:
        """Activate the service using a licence key."""
        self._dll.SetRegistrationKey.argtypes = [PCHAR]
        buf = create_string_buffer(key.encode())
        return self._dll.SetRegistrationKey(buf)

    def Disconnect(self) -> None:
        """Stop the filter service."""
        self._dll.Disconnect()

    ################################
    ################################
    ## filter configuration
    ################################
    ################################

    def SetBooleanConfig(self, booleanConfig: BooleanConfig) -> int:
        """Apply global boolean configuration settings.

        Args:
            booleanConfig: Configuration flags.
        """
        self._dll.SetBooleanConfig.argtypes = [ULONG]
        return self._dll.SetBooleanConfig(booleanConfig)

    def AddBooleanConfigToFilterRule(
        self, filterMask: Path, booleanConfig: BooleanConfig
    ) -> int:
        """Apply boolean configuration settings for a rule."""
        self._dll.AddBooleanConfigToFilterRule.argtypes = [PWCHAR, ULONG]
        return self._dll.AddBooleanConfigToFilterRule(str(filterMask), booleanConfig)

    def ResetConfigData(self) -> bool:
        """Resets all filter driver configuration settings to default."""
        return self._dll.ResetConfigData()

    def SetConnectionTimeout(self, connectionTimeout: int) -> int:
        """Set timeout for the driver sending messages to the service.

        Args:
            connectionTimeout: Timeout in seconds.
        """
        return self._dll.SetConnectionTimeout(connectionTimeout)

    def SetFilterType(self, filterType: FilterType) -> int:
        """Set filter type.

        Args:
            filterType: Bitflags of filter type.
        """
        self._dll.SetFilterType.argtypes = [ULONG]
        return self._dll.SetFilterType(filterType)

    Proto_Message_Callback = WINFUNCTYPE(
        BOOL,
        POINTER(MESSAGE_SEND_DATA),
        POINTER(MESSAGE_REPLY_DATA),
    )
    Proto_Disconnect_Callback = WINFUNCTYPE(None)

    def RegisterMessageCallback(
        self,
        ThreadCount: int,
        MessageCallback: Callable,
        DisconnectCallback: Callable,
    ) -> int:
        """Create the filter driver connection and set up callbacks.

        Args:
            ThreadCount: Number of threads waiting for the callback.
            MessageCallback: Callback function.
            DisconnectCallback: Callback run on disconnect.
        """
        # assigning as member variable to prevent these functions from going out of scope
        # if that happens the program silently crashes when the callback is called (it disappeared)
        self.message_callback = self.Proto_Message_Callback(MessageCallback)
        self.disconnect_callback = self.Proto_Disconnect_Callback(DisconnectCallback)

        self._dll.RegisterMessageCallback.argtypes = [
            ULONG,
            self.Proto_Message_Callback,
            self.Proto_Disconnect_Callback,
        ]
        return self._dll.RegisterMessageCallback(
            ThreadCount,
            self.message_callback,
            self.disconnect_callback,
        )

    ################################
    ## file filters
    ################################

    def AddFileFilterRule(
        self,
        accessFlag: AccessFlag,
        filterMask: Path,
        isResident: bool = False,
        ruleId: int = 0,
    ) -> int:
        """Add a new file filter rule to the driver.

        Args:
            accessFlag: Bitmask for access control rights to match.
            filterMask: File path for this rule (must be unique).
            isResident: Determines if rule is active at boot time (it will be added to registry if true).
            ruleId: ID for this rule. Will show up in `messageSend.FilterRuleId` if there is a callback.
        """
        self._dll.AddFileFilterRule.argtypes = [ULONG, PWCHAR, BOOL, ULONG]
        return self._dll.AddFileFilterRule(
            accessFlag, str(filterMask), isResident, ruleId
        )

    def RemoveFilterRule(self, FilterMask: Path) -> None:
        """Remove a file filter rule from the driver.

        Args:
            FilterMask: File path of the rule.
        """
        self._dll.RemoveFilterRule.argtypes = [PWCHAR]
        return self._dll.RemoveFilterRule(c_wchar_p(str(FilterMask)))

    def RegisterFileChangedEventsToFilterRule(
        self,
        filterMask: Path,
        eventType: int,
    ) -> bool:
        """Set events to get notified for in a filter rule.

        Args:
            filterMask: File path of the filter rule.
            eventType: Bitmask of events to register to (see `easefilter.enums.FileEventType`).
        """
        self._dll.RegisterFileChangedEventsToFilterRule.argtypes = [PWCHAR, ULONG]
        return self._dll.RegisterFileChangedEventsToFilterRule(
            str(filterMask), eventType
        )

    def RegisterMonitorIOToFilterRule(
        self, filterMask: Path, registerIO: IOCallbackClass
    ) -> bool:
        """Set I/O events to monitor in a filter rule.

        Args:
            filterMask: File path of the filter rule.
            registerIO: Bitmask of events to register to.
        """
        self._dll.RegisterMonitorIOToFilterRule.argtypes = [PWCHAR, c_ulonglong]
        return self._dll.RegisterMonitorIOToFilterRule(str(filterMask), registerIO)

    def RegisterControlIOToFilterRule(
        self, filterMask: Path, registerIO: IOCallbackClass
    ) -> bool:
        """Set I/O events to control in a filter rule.

        Args:
            filterMask: File path of the filter rule.
            registerIO: Bitmask of events to register to.
        """
        self._dll.RegisterControlIOToFilterRule.argtypes = [PWCHAR, c_ulonglong]
        return self._dll.RegisterControlIOToFilterRule(str(filterMask), registerIO)

    def AddEncryptionKeyToFilterRule(
        self, filterMask: Path, encryptionKeyLength: int, encryptionKey: bytes
    ) -> bool:
        """Encrypt a folder, giving each file its own IV.

        Encryption information will be prepended to the file in a header.

        Args:
            filterMask: File path of the filter rule.
            encryptionKeyLength: Byte length of the key.
            encryptionKey: Key data.
        """
        self._dll.AddEncryptionKeyToFilterRule.argtypes = [
            PWCHAR,
            ULONG,
            POINTER(c_ubyte),
        ]
        self._dll.AddEncryptionKeyToFilterRule.restype = BOOL

        key_buf = (c_ubyte * len(encryptionKey)).from_buffer(bytearray(encryptionKey))

        return self._dll.AddEncryptionKeyToFilterRule(
            str(filterMask), encryptionKeyLength, key_buf
        )

    ################################
    ## process filters
    ################################

    def AddProcessFilterRule(
        self,
        processNameMaskLength: int,
        processNameMask: Path,
        controlFlag: ProcessControlFlag,
        filterRuleId: int = 0,
    ) -> int:
        """Apply a new rule on a process.

        Args:
            processNameMaskLength: Length (in BYTES!) of the executable file path mask.
            processNameMask: Executable file path (can be a glob).
            controlFlag: Specific rules to apply.
            filterRuleId: Rule ID, which appears in `messageSend.FilterRuleId` as sent to the callback.
        """
        self._dll.AddProcessFilterRule.argtypes = [ULONG, c_wchar_p, ULONG, ULONG]
        return self._dll.AddProcessFilterRule(
            processNameMaskLength, str(processNameMask), controlFlag, filterRuleId
        )

    def RemoveProcessFilterRule(
        self, processNameMaskLength: int, processNameMask: Path
    ) -> int:
        self._dll.RemoveProcessFilterRule.argtypes = [ULONG, c_wchar_p]
        return self._dll.RemoveProcessFilterRule(
            processNameMaskLength, str(processNameMask)
        )

    ################################
    ## registry filters
    ################################

    def AddRegistryFilterRule(
        self,
        processNameLength: int,
        processName: Path | None,
        processId: int,
        userNameLength: int,
        userName: str,
        keyNameLength: int,
        keyNameFilterMask: str,
        accessFlag: RegControlFlag,
        regCallbackClass: RegCallbackClass,
        isExcludeFilter: bool,
        filterRuleId: int,
    ) -> int:
        """Create a new registry filter rule.

        Args:
            processNameLength: Length of process name IN BYTES (`wchar` = 2 bytes).
            processName: Process to filter, or `*` for all processes.
            processId: Process ID to filter, instead of `processName`.
            userNameLength: Length of user name.
            userName: User mask to filter.
            keyNameLength: Length of registry key filter.
            keyNameFilterMask: Registry key mask to filter.
            accessFlag: Access control flag for the registry.
            regCallbackClass: Registry events to listen for.
            isExcludeFilter: If true, does not filter events that match this rule.
            filterRuleId: Rule ID.
        """
        self._dll.AddRegistryFilterRule.argtypes = [
            ULONG,
            c_wchar_p,
            ULONG,
            ULONG,
            c_wchar_p,
            ULONG,
            c_wchar_p,
            ULONG,
            c_ulonglong,
            BOOL,
            ULONG,
        ]
        return self._dll.AddRegistryFilterRule(
            processNameLength,
            c_wchar_p(str(processName)),
            processId,
            userNameLength,
            c_wchar_p(str(userName)),
            keyNameLength,
            c_wchar_p(str(keyNameFilterMask)),
            accessFlag,
            regCallbackClass,
            isExcludeFilter,
            filterRuleId,
        )

    def RemoveRegistryFilterRuleByProcessId(self, processId: int) -> int:
        """Remove a registry filter rule by the PID it matches."""
        self._dll.RemoveRegistryFilterRuleByProcessId.argtypes = [ULONG]
        return self._dll.RemoveRegistryFilterRuleByProcessId(processId)

    def RemoveRegistryFilterRuleByProcessName(
        self, processNameLength: int, processName: str
    ) -> int:
        """Remove a registry filter rule by the process name it matches.

        Args:
            processNameLength: Length IN BYTES (`wchar` = 2 bytes) of the process name.
            processName: Process name.
        """
        self._dll.RemoveRegistryFilterRuleByProcessName.argtypes = [ULONG, c_wchar_p]
        return self._dll.RemoveRegistryFilterRuleByProcessName(
            processNameLength, processName
        )
