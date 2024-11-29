###############################################################################
##
##    (C) Copyright 2022 EaseFilter Technologies
##    All Rights Reserved
##
##    This software is part of a licensed software product and may
##    only be used or copied in accordance with the terms of that license.
##
##    NOTE:  THIS MODULE IS UNSUPPORTED SAMPLE CODE
##
##    This module contains sample code provided for convenience and
##    demonstration purposes only
##    "AS-IS" BASIS
##     either express or implied.
##
###############################################################################

import enum
import logging
import platform
from collections.abc import Iterator
from enum import IntEnum, IntFlag, auto
from typing import Optional, TypeVar

from typing_extensions import Self

from easefilter import bitmask

"""Definitions for various hardcoded Enums."""


################################
################################
## helper functions
################################
################################

T = TypeVar("T", bound=IntEnum)


def enum_coerce(enum: type[T], val: int) -> Optional[T]:  # noqa: FA100 (import future annotations does weird things to typevars)
    """See if a value fits in an enum, and if so return the enum."""
    try:
        return enum(val)
    except ValueError:
        return None


T = TypeVar("T", bound=IntFlag)


def autoflag_member_iter(flags: T) -> Iterator[T]:
    """__iter__ implementation for instances of IntFlag in Python 3.11+.

    This won't work on the class itself.
    """
    yield from flags._iter_member_(flags.value)  # type: ignore reportAttributeAccessIssue


if [int(i) for i in platform.python_version_tuple()] < [3, 11, 0]:
    logging.debug("using intflag shim (python < 3.11)")
    autoflag_member_iter = bitmask.flag_extract


class AutoFlag(IntFlag):
    """IntFlag with extra compatibility shim."""

    def __iter__(self) -> Iterator[Self]:
        """Shim for __iter__ on an IntFlag pre-Python 3.11."""
        if isinstance(self, enum.EnumMeta):
            yield from self._member_map_.values()  # type: ignore generalTypeIssues
        else:
            yield from autoflag_member_iter(self)

    def new_iter(self) -> Iterator[Self]:
        """New iter for after Python 3.11."""
        if isinstance(self, enum.EnumMeta):
            yield from self._member_map_.values()  # type: ignore generalTypeIssues
        else:
            yield from self._iter_member_by_value_(self.value)  # type: ignore reportAttributeAccessIssue


class AutoEnum(IntEnum):
    @staticmethod
    def _generate_next_value_(name, start, count, last_values) -> int:  # noqa: ANN001
        """Emulate C++'s default enum behaviour for enum values."""
        del name
        del start

        if count > 0:
            return last_values[-1] + 1

        return 0

    def __iter__(self) -> Iterator[Self]:
        """Emulate bitflag's iter on its members.

        Iterating over the class should yield all members,
        and iterating over a single member should give itself.
        """
        if isinstance(self, enum.EnumMeta):
            yield from self._member_map_.values()  # type: ignore generalTypeIssues
        else:
            yield self


################################
################################
## easefilter enums
################################
################################


class FilterType(AutoFlag):
    """Enable specific Filter capabilities."""

    CONTROL = 0x01
    """File I/O control."""

    ENCRYPTION = 0x02
    """Transparent file encryption."""

    MONITOR = 0x04
    """File I/O monitoring."""

    REGISTRY = 0x08
    """Registry I/O monitoring & control."""

    PROCESS = 0x10
    """Process/thread monitor & control."""

    HSM = 0x40
    """Hierarchical storage management."""

    CLOUD = 0x80
    """Cloud storage."""


class FileEventType(AutoFlag):
    """Select specific file events."""

    CREATED = 0x00000020
    WRITTEN = 0x00000040
    RENAMED = 0x00000080
    DELETED = 0x00000100
    SECURITY_CHANGED = 0x00000200
    INFO_CHANGED = 0x00000400
    READ = 0x00000800
    COPIED = 0x00001000
    """File copy; only available on Windows 11."""


class IOName(AutoEnum):
    """Types of I/O filesystem events."""

    PRE_FILE_CREATE = 0x00020001
    POST_FILE_CREATE = auto()
    PRE_FILE_READ = auto()
    POST_FILE_READ = auto()
    PRE_FILE_WRITE = auto()
    POST_FILE_WRITE = auto()
    PRE_QUERY_FILE_SIZE = auto()
    POST_QUERY_FILE_SIZE = auto()
    PRE_QUERY_FILE_BASIC_INFO = auto()
    POST_QUERY_FILE_BASIC_INFO = auto()
    PRE_QUERY_FILE_STANDARD_INFO = auto()
    POST_QUERY_FILE_STANDARD_INFO = auto()
    PRE_QUERY_FILE_NETWORK_INFO = auto()
    POST_QUERY_FILE_NETWORK_INFO = auto()
    PRE_QUERY_FILE_ID = auto()
    POST_QUERY_FILE_ID = auto()
    PRE_QUERY_FILE_INFO = auto()
    POST_QUERY_FILE_INFO = auto()
    PRE_SET_FILE_SIZE = auto()
    POST_SET_FILE_SIZE = auto()
    PRE_SET_FILE_BASIC_INFO = auto()
    POST_SET_FILE_BASIC_INFO = auto()
    PRE_SET_FILE_STANDARD_INFO = auto()
    POST_SET_FILE_STANDARD_INFO = auto()
    PRE_SET_FILE_NETWORK_INFO = auto()
    POST_SET_FILE_NETWORK_INFO = auto()
    PRE_MOVE_OR_RENAME_FILE = auto()
    POST_MOVE_OR_RENAME_FILE = auto()
    PRE_DELETE_FILE = auto()
    POST_DELETE_FILE = auto()
    PRE_SET_FILE_INFO = auto()
    POST_SET_FILE_INFO = auto()
    PRE_QUERY_DIRECTORY_FILE = auto()
    POST_QUERY_DIRECTORY_FILE = auto()
    PRE_QUERY_FILE_SECURITY = auto()
    POST_QUERY_FILE_SECURITY = auto()
    PRE_SET_FILE_SECURITY = auto()
    POST_SET_FILE_SECURITY = auto()
    PRE_FILE_HANDLE_CLOSE = auto()
    POST_FILE_HANDLE_CLOSE = auto()
    PRE_FILE_CLOSE = auto()
    POST_FILE_CLOSE = auto()


class IOCallbackClass(AutoFlag):
    """I/O types that can be intercepted via monitor/control filter.

    Monitor can only register events after they occur (post I/O),
    while control can register all events (pre and post).

    See [Wikipedia](https://en.m.wikipedia.org/wiki/I/O_request_packet)
    for basic information about what an IRP is.
    """

    NONE = 0

    PRE_CREATE = 0x00000001
    """`IRP_MJ_CREATE`: open file handle."""

    POST_CREATE = 0x00000002
    """`IRP_MJ_CREATE`: open file handle."""

    PRE_NEW_FILE_CREATED = 0x0000000100000000
    POST_NEW_FILE_CREATED = 0x0000000200000000

    PRE_FASTIO_READ = 0x00000004
    """Fast I/O read. Returns true if data is cached. If not cached, a new cache read IRP will be generated."""
    POST_FASTIO_READ = 0x00000008
    """Fast I/O read. Returns true if data is cached. If not cached, a new cache read IRP will be generated."""

    PRE_CACHE_READ = 0x00000010
    """`IRP_MJ_READ`: read data from cache. If not cached, a paging read request is generated."""
    POST_CACHE_READ = 0x00000020
    """`IRP_MJ_READ`: read data from cache. If not cached, a paging read request is generated."""

    PRE_NOCACHE_READ = 0x00000040
    """`IRP_MJ_READ`: read data without cache, bypassing the cache manager."""
    POST_NOCACHE_READ = 0x00000080
    """`IRP_MJ_READ`: read data without cache, bypassing the cache manager."""

    PRE_PAGING_IO_READ = 0x00000100
    """`IRP_MJ_READ`: paging read that caches on-disk data."""
    POST_PAGING_IO_READ = 0x00000200
    """`IRP_MJ_READ`: paging read that caches on-disk data."""

    PRE_FASTIO_WRITE = 0x00000400
    """Fast I/O write.

    Data written to cache if the request is immediately satisfied, otherwise an IRP cache write will be generated.
    """
    POST_FASTIO_WRITE = 0x00000800
    """Fast I/O write.

    Data written to cache if the request is immediately satisfied, otherwise an IRP cache write will be generated.
    """

    PRE_CACHE_WRITE = 0x00001000
    """`IRP_MJ_WRITE` cache write.

    A paging write IRP will be generated after this.
    """
    POST_CACHE_WRITE = 0x00002000
    """`IRP_MJ_WRITE` cache write.

    A paging write IRP will be generated after this.
    """

    PRE_NOCACHE_WRITE = 0x00004000
    """`IRP_MJ_WRITE`: write directly to disk, bypassing cache manager."""
    POST_NOCACHE_WRITE = 0x00008000
    """`IRP_MJ_WRITE`: write directly to disk, bypassing cache manager."""

    PRE_PAGING_IO_WRITE = 0x00010000
    """`IRP_MJ_WRITE`: paging write that moves data from cache to disk."""
    POST_PAGING_IO_WRITE = 0x00020000
    """`IRP_MJ_WRITE`: paging write that moves data from cache to disk."""

    PRE_QUERY_INFORMATION = 0x00040000
    """`IRP_QUERY_INFORMATION`: file information query.

    This flag registers all queries; other `PRE_QUERY` flags can watch specific queries, e.g. only file size.
    """
    POST_QUERY_INFORMATION = 0x00080000
    """`IRP_QUERY_INFORMATION`: file information query.

    This flag registers all queries; other `QUERY` flags can watch specific queries, e.g. only file size.
    """

    PRE_QUERY_FILE_SIZE = 0x0000000400000000
    """`IRP_QUERY_INFORMATION`: file size."""
    POST_QUERY_FILE_SIZE = 0x0000000800000000
    """`IRP_QUERY_INFORMATION`: file size."""

    PRE_QUERY_FILE_BASIC_INFO = 0x0000001000000000
    """`IRP_QUERY_INFORMATION`: basic file information."""
    POST_QUERY_FILE_BASIC_INFO = 0x0000002000000000
    """`IRP_QUERY_INFORMATION`: basic file information."""

    PRE_QUERY_FILE_STANDARD_INFO = 0x0000004000000000
    """`IRP_QUERY_INFORMATION`: standard file information."""
    POST_QUERY_FILE_STANDARD_INFO = 0x0000008000000000
    """`IRP_QUERY_INFORMATION`: standard file information."""

    PRE_QUERY_FILE_NETWORK_INFO = 0x0000010000000000
    """`IRP_QUERY_INFORMATION`: file network information."""
    POST_QUERY_FILE_NETWORK_INFO = 0x0000020000000000
    """`IRP_QUERY_INFORMATION`: file network information."""

    PRE_QUERY_FILE_ID = 0x0000040000000000
    """`IRP_QUERY_INFORMATION`: file ID."""
    POST_QUERY_FILE_ID = 0x0000080000000000
    """`IRP_QUERY_INFORMATION`: file ID."""

    PRE_SET_INFORMATION = 0x00100000
    """`IRP_SET_INFORMATION`: set file information.

    This flag registers all requests; other `SET` flags can watch specific requests, e.g. only file size.
    """
    POST_SET_INFORMATION = 0x00200000
    """`IRP_SET_INFORMATION`: set file information.

    This flag registers all requests; other `SET` flags can watch specific requests, e.g. only file size.
    """

    PRE_SET_FILE_SIZE = 0x0000400000000000
    """`IRP_SET_INFORMATION`: file size."""
    POST_SET_FILE_SIZE = 0x0000800000000000
    """`IRP_SET_INFORMATION`: file size."""

    PRE_SET_FILE_BASIC_INFO = 0x0001000000000000
    """`IRP_SET_INFORMATION`: file basic information."""
    POST_SET_FILE_BASIC_INFO = 0x0002000000000000
    """`IRP_SET_INFORMATION`: file basic information."""

    PRE_SET_FILE_STANDARD_INFO = 0x0004000000000000
    """`IRP_SET_INFORMATION`: file standard information."""
    POST_SET_FILE_STANDARD_INFO = 0x0008000000000000
    """`IRP_SET_INFORMATION`: file standard information."""

    PRE_SET_FILE_NETWORK_INFO = 0x0010000000000000
    """`IRP_SET_INFORMATION`: file network information."""
    POST_SET_FILE_NETWORK_INFO = 0x0020000000000000
    """`IRP_SET_INFORMATION`: file network information."""

    PRE_RENAME_FILE = 0x0040000000000000
    """`IRP_SET_INFORMATION`: file rename."""
    POST_RENAME_FILE = 0x0080000000000000
    """`IRP_SET_INFORMATION`: file rename."""

    PRE_DELETE_FILE = 0x0100000000000000
    """`IRP_SET_INFORMATION`: file delete."""
    POST_DELETE_FILE = 0x0200000000000000
    """`IRP_SET_INFORMATION`: file delete."""

    PRE_DIRECTORY = 0x00400000
    """`IRP_MJ_DIRECTORY_CONTROL`: query directory information."""
    POST_DIRECTORY = 0x00800000
    """`IRP_MJ_DIRECTORY_CONTROL`: query directory information."""

    PRE_QUERY_SECURITY = 0x01000000
    """`IRP_MJ_QUERY_SECURITY`: query file security information."""
    POST_QUERY_SECURITY = 0x02000000
    """`IRP_MJ_QUERY_SECURITY`: query file security information."""

    PRE_SET_SECURITY = 0x04000000
    """`IRP_MJ_SET_SECURITY`: set file security information."""
    POST_SET_SECURITY = 0x08000000
    """`IRP_MJ_SET_SECURITY`: set file security information."""

    PRE_CLEANUP = 0x10000000
    """`IRP_MJ_CLEANUP`: close file handle."""
    POST_CLEANUP = 0x20000000
    """`IRP_MJ_CLEANUP`: close file handle."""

    PRE_CLOSE = 0x40000000
    """`IRP_MJ_CLOSE`: close file I/O."""
    POST_CLOSE = 0x80000000
    """`IRP_MJ_CLOSE`: close file I/O."""


class AccessFlag(AutoFlag):
    """Access control for a file.

    Only for Control mode.

    IMPORTANT: Do not set this to 0 for least access.
    See `AccessFlag.LEAST_ACCESS_FLAG`.
    """

    EXCLUDE_FILTER_RULE = 0x00000000
    """Filter driver skips all I/O on this file.

    (i.e, no access protection.)
    """

    EXCLUDE_FILE_ACCESS = 0x00000001
    """Block file open."""

    ENABLE_REPARSE_FILE_OPEN = 0x00000002
    """Allow reparsing the file to another filename through a reparse mask."""

    ENABLE_HIDE_FILES_IN_DIRECTORY_BROWSING = 0x00000004
    """Allow hiding files in a directory through a hide file mask."""

    ENABLE_FILE_ENCRYPTION_RULE = 0x00000008
    """Enable transparent file encryption if an encryption key is added."""

    ALLOW_OPEN_WITH_ACCESS_SYSTEM_SECURITY = 0x00000010
    """Allow file open to access the file's security information."""

    ALLOW_OPEN_WITH_READ_ACCESS = 0x00000020
    """Allow file open with read access."""

    ALLOW_OPEN_WITH_WRITE_ACCESS = 0x00000040
    """Allow file open with write access."""

    ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS = 0x00000080
    """Allow file open with create/overwrite access."""

    ALLOW_OPEN_WITH_DELETE_ACCESS = 0x00000100
    """Allow file open with delete access."""

    ALLOW_READ_ACCESS = 0x00000200
    """Allow reading data from file."""

    ALLOW_WRITE_ACCESS = 0x00000400
    """Allow writing data from file."""

    ALLOW_QUERY_INFORMATION_ACCESS = 0x00000800
    """Allow querying the file's information."""

    ALLOW_SET_INFORMATION = 0x00001000
    """Allow changing the file's information:

    - File attributes
    - File size
    - File name
    - Delete file
    """

    ALLOW_FILE_RENAME = 0x00002000
    """Allow renaming the file."""

    ALLOW_FILE_DELETE = 0x00004000
    """Allow deleting the file."""

    ALLOW_FILE_SIZE_CHANGE = 0x00008000
    """Allow changing file size."""

    ALLOW_QUERY_SECURITY_ACCESS = 0x00010000
    """Allow querying the file's security information."""

    ALLOW_SET_SECURITY_ACCESS = 0x00020000
    """Allow changing the file's security information."""

    ALLOW_DIRECTORY_LIST_ACCESS = 0x00040000
    """Allow listing the directory's contents"""

    ALLOW_FILE_ACCESS_FROM_NETWORK = 0x00080000
    """Allow remote access via share folder."""

    ALLOW_ENCRYPT_NEW_FILE = 0x00100000
    """Allow encrypting a new file if the encryption filter rule is enabled."""

    ALLOW_READ_ENCRYPTED_FILES = 0x00200000
    """Allow applications to read encrypted files; if not set, return encrypted data."""

    ALLOW_ALL_SAVE_AS = 0x00400000
    """Allow applications to create a new file after opening the protected file."""

    ALLOW_COPY_PROTECTED_FILES_OUT = 0x00800000
    """Allow copying protected files out of the protected folder if `ALLOW_ALL_SAVE_AS` is enabled."""

    ALLOW_FILE_MEMORY_MAPPED = 0x01000000
    """Allow file to be memory mapped."""

    DISABLE_ENCRYPT_DATA_ON_READ = 0x02000000
    """Disable encrypting data on read when the encryption filter is enabled."""

    ALLOW_COPY_PROTECTED_FILES_TO_USB = 0x04000000
    """Allow copying protected files to USB storage."""

    LEAST_ACCESS_FLAG = 0xF0000000
    """Disable all access to this file.

    IMPORTANT: Do not set `AccessFlag` to 0.
    This is equivalent to disabling all protections
    (`EXCLUDE_FILTER_RULE`).
    """

    ALLOW_MAX_RIGHT_ACCESS = 0xFFFFFFF0
    """Allow all access to the file."""


class FilterCommand(AutoEnum):
    MESSAGE_TYPE_RESTORE_BLOCK_OR_FILE = 0x00000001
    MESSAGE_TYPE_RESTORE_FILE_TO_ORIGINAL_FOLDER = 0x00000002
    MESSAGE_TYPE_GET_FILE_LIST = 0x00000004
    MESSAGE_TYPE_RESTORE_FILE_TO_CACHE = 0x00000008
    MESSAGE_TYPE_SEND_EVENT_NOTIFICATION = 0x00000010
    MESSAGE_TYPE_DELETE_FILE = 0x00000020
    MESSAGE_TYPE_RENAME_FILE = 0x00000040
    MESSAGE_TYPE_SEND_MESSAGE_FILENAME = 0x00000080
    FILTER_SEND_FILE_CHANGED_EVENT = 0x00010001
    FILTER_REQUEST_USER_PERMIT = 0x00010002
    FILTER_REQUEST_ENCRYPTION_KEY = 0x00010003
    FILTER_REQUEST_ENCRYPTION_IV_AND_KEY = 0x00010004
    FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_ACCESSFLAG = 0x00010005
    FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA = 0x00010006
    FILTER_SEND_REG_CALLBACK_INFO = 0x00010007
    FILTER_SEND_PROCESS_CREATION_INFO = 0x00010008
    FILTER_SEND_PROCESS_TERMINATION_INFO = 0x00010009
    FILTER_SEND_THREAD_CREATION_INFO = 0x0001000A
    FILTER_SEND_THREAD_TERMINATION_INFO = 0x0001000B
    FILTER_SEND_PROCESS_HANDLE_INFO = 0x0001000C
    FILTER_SEND_THREAD_HANDLE_INFO = 0x0001000D
    FILTER_SEND_ATTACHED_VOLUME_INFO = 0x0001000E
    FILTER_SEND_DETACHED_VOLUME_INFO = 0x0001000F
    FILTER_SEND_DENIED_FILE_IO_EVENT = 0x00010010
    FILTER_SEND_DENIED_VOLUME_DISMOUNT_EVENT = 0x00010011
    FILTER_SEND_DENIED_PROCESS_EVENT = 0x00010012
    FILTER_SEND_DENIED_REGISTRY_ACCESS_EVENT = 0x00010013
    FILTER_SEND_DENIED_PROCESS_TERMINATED_EVENT = 0x00010014
    FILTER_SEND_DENIED_USB_READ_EVENT = 0x00010015
    FILTER_SEND_DENIED_USB_WRITE_EVENT = 0x00010016
    FILTER_SEND_PRE_TERMINATE_PROCESS_INFO = 0x00010017


class FilterStatus(AutoFlag):
    """Return status for `CallbackReplyData`."""

    FILTER_MESSAGE_IS_DIRTY = 0x00000001
    """Message needs to be processed."""

    FILTER_COMPLETE_PRE_OPERATION = 0x00000002
    """Pre-operation completed by your code."""

    FILTER_DATA_BUFFER_IS_UPDATED = 0x00000004
    """Callback's reply data contains an update."""

    FILTER_BLOCK_DATA_WAS_RETURNED = 0x00000008
    """Read block databuffer is returned to filter."""

    FILTER_CACHE_FILE_WAS_RETURNED = 0x00000010
    """The whole cache file was downloaded."""

    FILTER_REHYDRATE_FILE_VIA_CACHE_FILE = 0x00000020
    """Whole cache file downloaded, and stub file needs to be rehydrated."""


class ProcessControlFlag(AutoFlag):
    DENY_NEW_PROCESS_CREATION = 0x00000001
    """Deny the new process creation if the flag is on."""

    PROCESS_PRE_TERMINATION_REQUEST = 0x00000002
    """Send the callback request before the process is going to be terminated.

    You can block the process termination in the callback function.
    """

    PROCESS_CREATION_NOTIFICATION = 0x00000100
    """Get a notification when a new process is being created."""

    PROCESS_TERMINATION_NOTIFICATION = 0x00000200
    """Get a notification when a process was terminated."""

    PROCESS_HANDLE_OP_NOTIFICATION = 0x00000400
    """Get a notification for process handle operations,
    when a handle for a process is being created or duplicated."""

    THREAD_CREATION_NOTIFICATION = 0x00000800
    """Get a notifcation when a new thread is being created."""

    THREAD_TERMINATION_NOTIFICATION = 0x00001000
    """Get a notification when a thread was terminated."""

    THREAD_HANDLE_OP_NOTIFICATION = 0x00002000
    """Get a notification for thread handle operations, when a handle for a process
    is being created or duplicated."""


class BooleanConfig(AutoFlag):
    """Configuration and feature switches.

    Use `easefilter.EaseFilter.configure()` to apply this.
    """

    ENABLE_NO_RECALL_FLAG = 0x00000001
    """EaseTag: if true, after opening the reparse point file, won't restore data back for read/write."""

    DISABLE_FILTER_UNLOAD_FLAG = 0x00000002
    """If true, disables unloading the filter driver."""

    ENABLE_SET_OFFLINE_FLAG = 0x00000004
    """If true, sets the offline attribute for virtual files."""

    ENABLE_DEFAULT_IV_TAG = 0x00000008
    """If true, uses a default IV to encrypt files in encryption mode."""

    ENABLE_ADD_MESSAGE_TO_FILE = 0x00000010
    """If true, sends message data to a persistent file, otherwise send the event to the service immediately."""

    ENCRYPT_FILE_WITH_REPARSE_POINT_TAG = 0x00000020
    """(Version 5.0) If true, encrypted file metadata is embedded in the reparse point tag."""

    REQUEST_ENCRYPT_KEY_AND_IV_FROM_SERVICE = 0x00000040
    """Deprecated."""

    ENABLE_PROTECTION_IN_BOOT_TIME = 0x00000080
    """If true, enables control filter rules at boot time."""

    REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE = 0x00000100
    """
    If true, encryption rules get the encryption key and IV, plus optional tag
    data from your user-mode callback code instead of storing it in the driver.
    """

    ENABLE_SEND_DATA_BUFFER = 0x00000200
    """If enabled, will send read/write data to user-mode callback code."""

    ENABLE_REOPEN_FILE_ON_REHYDRATION = 0x00000400
    """If true, will re-open files when rehydrating the stub."""

    ENABLE_MONITOR_EVENT_BUFFER = 0x00000800
    """If true, queues monitor mode events in a buffer, then processes them asynchronously.

    This avoids blocking I/O requests, but may result in events being dropped when the buffer is full.
    """

    ENABLE_SEND_DENIED_EVENT = 0x00001000
    """If true, sends events even if they are blocked."""

    ENABLE_WRITE_WITH_ZERO_DATA_AND_SEND_DATA = 0x00002000
    """If true, and write access is disabled, writes return success and write zero data to the file, and sends write data to user mode."""

    DISABLE_REMOVABLE_MEDIA_AS_USB = 0x00004000
    """If true, treats portable massive storage as USB.

    This is for the volume control flag for `BLOCK_USB_READ`, `BLOCK_USB_WRITE`.
    """

    DISABLE_RENAME_ENCRYPTED_FILE = 0x00008000
    """If true, blocks moving an encrypted file to a different folder.

    By default, moving a file out of an encrypted folder results in the file
    being unable to be decrypted.
    """

    DISABLE_FILE_SYNCHRONIZATION = 0x00010000
    """If true, disables file synchronization for file reading in CloudTier."""

    ENABLE_PROTECTION_IF_SERVICE_STOPPED = 0x00020000
    """If true, data protection will continue after the service process is stopped."""

    ENABLE_SIGNAL_WRITE_ENCRYPT_INFO_EVENT = 0x00020000
    """If true and write encrypt info to cache is enabled, signals the system thread to write cached data to disk right away."""

    ENABLE_BLOCK_SAVE_AS_FLAG = 0x00040000
    """
    Enable this feature for the `ALLOW_SAVE_AS` or
    `ALLOW_COPY_PROTECTED_FILES_OUT` accessFlag values. By default this feature
    is disabled, because if these two flags are disabled this will block all new
    file creation of the process which has read the protected files.
    """


class RegCallbackClass(AutoFlag):
    """REGISTRY mode filter driver callback class.

    These are registry events that can be subscribed to.
    """

    Reg_Pre_Delete_Key = 0x00000001
    Reg_Pre_Set_Value_Key = 0x00000002
    Reg_Pre_Delete_Value_Key = 0x00000004
    Reg_Pre_SetInformation_Key = 0x00000008
    Reg_Pre_Rename_Key = 0x00000010
    Reg_Pre_Enumerate_Key = 0x00000020
    Reg_Pre_Enumerate_Value_Key = 0x00000040
    Reg_Pre_Query_Key = 0x00000080
    Reg_Pre_Query_Value_Key = 0x00000100
    Reg_Pre_Query_Multiple_Value_Key = 0x00000200
    Reg_Pre_Create_Key = 0x00000400
    Reg_Post_Create_Key = 0x00000800
    Reg_Pre_Open_Key = 0x00001000
    Reg_Post_Open_Key = 0x00002000
    Reg_Pre_Key_Handle_Close = 0x00004000

    # .NET only values
    Reg_Post_Delete_Key = 0x00008000
    Reg_Post_Set_Value_Key = 0x00010000
    Reg_Post_Delete_Value_Key = 0x00020000
    Reg_Post_SetInformation_Key = 0x00040000
    Reg_Post_Rename_Key = 0x00080000
    Reg_Post_Enumerate_Key = 0x00100000
    Reg_Post_Enumerate_Value_Key = 0x00200000
    Reg_Post_Query_Key = 0x00400000
    Reg_Post_Query_Value_Key = 0x00800000
    Reg_Post_Query_Multiple_Value_Key = 0x01000000
    Reg_Post_Key_Handle_Close = 0x02000000
    Reg_Pre_Create_KeyEx = 0x04000000
    Reg_Post_Create_KeyEx = 0x08000000
    Reg_Pre_Open_KeyEx = 0x10000000
    Reg_Post_Open_KeyEx = 0x20000000

    # Available in Windows Vista and later
    Reg_Pre_Flush_Key = 0x40000000
    Reg_Post_Flush_Key = 0x80000000

    Reg_Pre_Load_Key = 0x100000000
    Reg_Post_Load_Key = 0x200000000
    Reg_Pre_UnLoad_Key = 0x400000000
    Reg_Post_UnLoad_Key = 0x800000000
    Reg_Pre_Query_Key_Security = 0x1000000000
    Reg_Post_Query_Key_Security = 0x2000000000
    Reg_Pre_Set_Key_Security = 0x4000000000
    Reg_Post_Set_Key_Security = 0x8000000000

    Reg_Callback_Object_Context_Cleanup = 0x10000000000
    """Per-object context cleanup."""

    # Available in Windows Vista SP2 and later
    Reg_Pre_Restore_Key = 0x20000000000
    Reg_Post_Restore_Key = 0x40000000000
    Reg_Pre_Save_Key = 0x80000000000
    Reg_Post_Save_Key = 0x100000000000
    Reg_Pre_Replace_Key = 0x200000000000
    Reg_Post_Replace_Key = 0x400000000000

    # Available in Windows 10 and later
    Reg_Pre_Query_KeyName = 0x800000000000
    Reg_Post_Query_KeyName = 0x1000000000000

    Max_Reg_Callback_Class = 0xFFFFFFFFFFFFFFFF


class RegControlFlag(AutoFlag):
    """Permission flags for the REGISTRY mode filter."""

    REG_ALLOW_OPEN_KEY = 0x00000001
    REG_ALLOW_CREATE_KEY = 0x00000002
    REG_ALLOW_QUERY_KEY = 0x00000004
    REG_ALLOW_RENAME_KEY = 0x00000008
    REG_ALLOW_DELETE_KEY = 0x00000010
    REG_ALLOW_SET_VALUE_KEY_INFORMATION = 0x00000020
    REG_ALLOW_SET_INFORMATION_KEY = 0x00000040
    REG_ALLOW_ENUMERATE_KEY = 0x00000080
    REG_ALLOW_QUERY_VALUE_KEY = 0x00000100
    REG_ALLOW_ENUMERATE_VALUE_KEY = 0x00000200
    REG_ALLOW_QUERY_MULTIPLE_VALUE_KEY = 0x00000400
    REG_ALLOW_DELETE_VALUE_KEY = 0x00000800
    REG_ALLOW_QUERY_KEY_SECURITY = 0x00001000
    REG_ALLOW_SET_KEY_SECRUITY = 0x00002000
    REG_ALLOW_RESTORE_KEY = 0x00004000
    REG_ALLOW_REPLACE_KEY = 0x00008000
    REG_ALLOW_SAVE_KEY = 0x00010000
    REG_ALLOW_FLUSH_KEY = 0x00020000
    REG_ALLOW_LOAD_KEY = 0x00040000
    REG_ALLOW_UNLOAD_KEY = 0x00080000
    REG_ALLOW_KEY_CLOSE = 0x00100000
    REG_ALLOW_QUERY_KEYNAME = 0x00200000
    REG_MAX_ACCESS_FLAG = 0xFFFFFFFF


################################
################################
## Windows data structures
################################
################################


class FileStatus(IntEnum):
    SUCCESS = 0
    ACCESS_DENIED = 0xC0000022
    REPARSE = 0x00000104
    NO_MORE_FILES = 0x80000006
    WARNING = 0x80000000
    ERROR = 0xC0000000


class InformationClass(AutoEnum):
    """The specific class a generic POST_SET_INFORMATION event.

    For more information, see https://learn.microsoft.com/en-us/openspecs/windows_protocols/ms-fscc/4718fc40-e539-4014-8e33-b675af74e3e1
    """

    FileDirectoryInformation = 1
    FileFullDirectoryInformation = auto()
    FileBothDirectoryInformation = auto()
    FileBasicInformation = auto()
    FileStandardInformation = auto()
    FileInternalInformation = auto()
    FileEaInformation = auto()
    FileAccessInformation = auto()
    FileNameInformation = auto()
    FileRenameInformation = auto()
    FileLinkInformation = auto()
    FileNamesInformation = auto()
    FileDispositionInformation = auto()
    FilePositionInformation = auto()
    FileFullEaInformation = auto()
    FileModeInformation = auto()
    FileAlignmentInformation = auto()
    FileAllInformation = auto()
    FileAllocationInformation = auto()
    FileEndOfFileInformation = auto()
    FileAlternateNameInformation = auto()
    FileStreamInformation = auto()
    FilePipeInformation = auto()
    FilePipeLocalInformation = auto()
    FilePipeRemoteInformation = auto()
    FileMailslotQueryInformation = auto()
    FileMailslotSetInformation = auto()
    FileCompressionInformation = auto()
    FileObjectIdInformation = auto()
    FileCompletionInformation = auto()
    FileMoveClusterInformation = auto()
    FileQuotaInformation = auto()
    FileReparsePointInformation = auto()
    FileNetworkOpenInformation = auto()
    FileAttributeTagInformation = auto()
    FileTrackingInformation = auto()
    FileIdBothDirectoryInformation = auto()
    FileIdFullDirectoryInformation = auto()
    FileValidDataLengthInformation = auto()
    FileShortNameInformation = auto()
    FileIoCompletionNotificationInformation = auto()
    FileIoStatusBlockRangeInformation = auto()
    FileIoPriorityHintInformation = auto()
    FileSfioReserveInformation = auto()
    FileSfioVolumeInformation = auto()
    FileHardLinkInformation = auto()
    FileProcessIdsUsingFileInformation = auto()
    FileNormalizedNameInformation = auto()
    FileNetworkPhysicalNameInformation = auto()
    FileIdGlobalTxDirectoryInformation = auto()
    FileIsRemoteDeviceInformation = auto()
    FileUnusedInformation = auto()
    FileNumaNodeInformation = auto()
    FileStandardLinkInformation = auto()
    FileRemoteProtocolInformation = auto()

    #
    #  These are special versions of these operations (defined earlier)
    #  which can be used by kernel mode drivers only to bypass security
    #  access checks for Rename and HardLink operations.  These operations
    #  are only recognized by the IOManager, a file system should never
    #  receive these.
    #

    FileRenameInformationBypassAccessCheck = auto()
    FileLinkInformationBypassAccessCheck = auto()

    #
    # End of special information classes reserved for IOManager.
    #

    FileVolumeNameInformation = auto()
    FileIdInformation = auto()
    FileIdExtdDirectoryInformation = auto()
    FileReplaceCompletionInformation = auto()
    FileHardLinkFullIdInformation = auto()
    FileIdExtdBothDirectoryInformation = auto()
    FileDispositionInformationEx = auto()
    FileRenameInformationEx = auto()
    FileRenameInformationExBypassAccessCheck = auto()
    FileDesiredStorageClassInformation = auto()
    FileStatInformation = auto()
    FileMemoryPartitionInformation = auto()
    FileStatLxInformation = auto()
    FileCaseSensitiveInformation = auto()
    FileLinkInformationEx = auto()
    FileLinkInformationExBypassAccessCheck = auto()
    FileStorageReserveIdInformation = auto()
    FileCaseSensitiveInformationForceAccessCheck = auto()
    FileKnownFolderInformation = auto()

    FileMaximumInformation = auto()


################################
################################
## python library enums
################################
################################


class ProcessEventType(AutoEnum):
    """Namespace for specific process-related FilterCommand members.

    FilterCommand should be able to be directly casted to this.
    """

    ProcessCreationBlocked = FilterCommand.FILTER_SEND_DENIED_PROCESS_EVENT
    """Tried to run an executable; blocked by filter."""

    ProcessTerminated = FilterCommand.FILTER_SEND_PROCESS_TERMINATION_INFO
    """Process stopped."""

    ThreadCreated = FilterCommand.FILTER_SEND_THREAD_CREATION_INFO
    """New thread created."""

    ThreadTerminated = FilterCommand.FILTER_SEND_THREAD_TERMINATION_INFO
    """Thread stopped."""

    ProcessHandleInfo = FilterCommand.FILTER_SEND_PROCESS_HANDLE_INFO
    """Notification for opening a new Windows process handle."""

    ThreadHandleInfo = FilterCommand.FILTER_SEND_THREAD_HANDLE_INFO
    """Notification for opening a new Windows thread handle."""

    # these two are separate because they can be denied
    ProcessCreationInfo = FilterCommand.FILTER_SEND_PROCESS_CREATION_INFO
    """Process is being created; can be blocked by your filter code."""
    ProcessPreTerminateInfo = FilterCommand.FILTER_SEND_PRE_TERMINATE_PROCESS_INFO
    """Process is being terminated forcefully; can be blocked by your filter code."""


class EncryptEventType(AutoEnum):
    """Namespace for encryption-related FilterCommand members.

    FilterCommand should be able to be directly casted to this.
    """

    RequestKey = FilterCommand.FILTER_REQUEST_ENCRYPTION_KEY
    """Your code has to provide an encryption key."""

    RequestIvAndKey = FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY
    """Your code has to provide an encryption key and IV.

    This event may also contain existing tag data for the file.
    """

    RequestIvAndKeyAndTagData = (
        FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_TAGDATA
    )
    """Your code has to provide an encryption key and IV, and optionally set tag data for the file."""

    RequestIvAndKeyAndAccessFlag = (
        FilterCommand.FILTER_REQUEST_ENCRYPTION_IV_AND_KEY_AND_ACCESSFLAG
    )
    """Your code has to provide an encryption key, IV, and the `AccessFlag`."""
