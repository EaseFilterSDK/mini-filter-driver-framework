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

"""Object-oriented event types.

These provide a cleaner interface over the message send/reply structs.
"""

from __future__ import annotations

import ctypes
import logging
from dataclasses import dataclass
from pathlib import Path
from typing import TYPE_CHECKING, TypedDict

from typing_extensions import override

from easefilter.enums import (
    AccessFlag,
    EncryptEventType,
    FileStatus,
    FilterCommand,
    FilterStatus,
    InformationClass,
    ProcessEventType,
    RegCallbackClass,
    enum_coerce,
)
from easefilter.errors import CorruptionError

if TYPE_CHECKING:
    from easefilter.filter_api import MESSAGE_SEND_DATA
from easefilter.enums import FileEventType, IOCallbackClass
from easefilter.filter_api import (
    AES_MAX_TAG_DATA_SIZE,
    MESSAGE_REPLY_DATA,
    MESSAGE_SEND_VERIFICATION_NUMBER,
    PPROCESS_INFO,
    AccountData,
    ConvertSidToStringSidW,
    GetProcessPathByPid,
    LookupAccountSidW,
)
from easefilter.types import (
    BaseEvent,
    BaseReply,
    FileTimes,
    IOEvent,
    ProcessData,
)

################################
################################
## incoming event handlers
################################
################################


class BaseEventArgs(TypedDict):
    """Fields common to all filter events."""

    message_id: int
    io_event_type: IOEvent
    filter_rule_id: int
    sid: str
    can_reply: bool
    account: AccountData


def base_args(msg: MESSAGE_SEND_DATA, ev: IOEvent, *, can_reply: bool) -> BaseEventArgs:
    """Processes fields common to all filter events.

    Args:
        msg: Data to make arguments from.
        ev: Event type.
        can_reply: True if event can be replied to (message reply pointer not null).
    """
    return {
        "message_id": int(msg.MessageId),
        "io_event_type": ev,
        "filter_rule_id": int(msg.FilterRuleId),
        "sid": ConvertSidToStringSidW(msg.Sid),
        "can_reply": can_reply,
        "account": LookupAccountSidW(msg.Sid),
    }


@dataclass
class FileEvent(BaseEvent):
    """File I/O (monitor & control mode) event."""

    file_name: Path
    """File path affected by this event."""

    process: ProcessData
    """Information about the process that initiated this event."""

    time: FileTimes
    """File timestamps."""

    info_class: InformationClass | None
    """File information class, if event changes information. See `InformationClass`."""

    new_path: Path | None
    """New path for copy/rename operations.

    (Old path for copy operation.)
    """

    @override
    @classmethod
    def from_msg(
        cls: type[FileEvent], msg: MESSAGE_SEND_DATA, base_args: BaseEventArgs
    ) -> FileEvent:
        """Instantiate from MESSAGE_SEND data."""
        info_class: InformationClass | None = None
        ev: IOEvent = base_args["io_event_type"]
        if isinstance(ev, IOCallbackClass) and (
            IOCallbackClass.POST_SET_INFORMATION in ev
            or IOCallbackClass.PRE_SET_INFORMATION in ev
        ):
            info_class = InformationClass(msg.InfoClass)

        new_path: Path | None = None
        if (
            isinstance(ev, FileEventType)
            and (FileEventType.RENAMED in ev or FileEventType.COPIED in ev)
            and int(msg.DataBufferLength) > 0
        ):
            s = ctypes.cast(msg.DataBuffer, ctypes.c_wchar_p).value
            if s is not None:
                new_path = Path(s)

        return cls(
            **base_args,
            file_name=Path(str(msg.FileName)),
            process=ProcessData.from_msg(msg),
            time=FileTimes.from_msg(msg),
            info_class=info_class,
            new_path=new_path,
        )


@dataclass
class ProcessEvent(BaseEvent):
    """Process mode event. Includes process and thread events."""

    process: ProcessData
    """Information about the relevant process/thread for this event."""

    parent_proc: ProcessData | None
    """Information about the parent process/thread.

    Only available on a process start event.
    """

    creating_proc: ProcessData | None
    """Information about the creating process/thread.

    This may be different from the parent process in some cases.

    Only available on a process start event.
    """

    command_line: str | None
    """Full command line used to start this process.

    Only available on a process start event.
    """

    @override
    @classmethod
    def from_msg(
        cls: type[ProcessEvent], msg: MESSAGE_SEND_DATA, base_args: BaseEventArgs
    ) -> ProcessEvent:
        """Instantiate from MESSAGE_SEND data."""
        proc_info_p = ctypes.cast(msg.DataBuffer, PPROCESS_INFO)
        proc_info = proc_info_p.contents

        cmd_line: str | None = str(proc_info.CommandLine)
        if cmd_line == "":
            cmd_line = None

        is_create_ev: bool = (
            base_args["io_event_type"] == ProcessEventType.ProcessCreationInfo
        )

        par_proc: ProcessData | None = None
        create_proc: ProcessData | None = None
        if is_create_ev:
            par_path = GetProcessPathByPid(proc_info.ParentProcessId)
            par_proc = ProcessData(
                int(proc_info.ParentProcessId),
                None,
                Path(par_path) if par_path else None,
            )
            create_path = GetProcessPathByPid(proc_info.ParentProcessId)
            create_proc = ProcessData(
                int(proc_info.CreatingProcessId),
                int(proc_info.CreatingThreadId),
                Path(create_path) if create_path else None,
            )

        return cls(
            **base_args,
            process=ProcessData(
                int(msg.ProcessId),
                int(msg.ThreadId),
                Path(str(msg.FileName)),
            ),
            creating_proc=create_proc,
            parent_proc=par_proc,
            command_line=cmd_line,
        )


@dataclass
class EncryptEvent(BaseEvent):
    """Encryption mode event.

    This is only used when callback code is enabled through
    `BooleanConfig.REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE`. Reply to
    this event either with a `DenyReply` or an `EncryptReply`.
    """

    file_name: Path
    """File path to decrypt/encrypt."""

    process: ProcessData
    """Information about the process that initiated this event."""

    tag_data: bytes | None

    io_status: FileStatus
    """Operation status."""

    @override
    @classmethod
    def from_msg(
        cls: type[EncryptEvent], msg: MESSAGE_SEND_DATA, base_args: BaseEventArgs
    ) -> EncryptEvent:
        """Instantiate from MESSAGE_SEND data."""
        tag_data: bytes | None = None
        if base_args["io_event_type"] == EncryptEventType.RequestIvAndKey:
            data_len = int(msg.DataBufferLength)
            logging.debug("tag data length: %d", data_len)
            if data_len > 0:
                tag_data = bytes(msg.DataBuffer)[:data_len]

        return cls(
            **base_args,
            file_name=Path(str(msg.FileName)),
            process=ProcessData.from_msg(msg),
            tag_data=tag_data,
            io_status=FileStatus(msg.Status),
        )


@dataclass
class RegistryEvent(BaseEvent):
    """Registry mode event."""

    new_path: str | None
    """New key name for rename operations."""

    key_name: str
    """Registry key being affected by this operation."""

    process: ProcessData
    """Process triggering this operation."""

    @classmethod
    def from_msg(
        cls: type[RegistryEvent], msg: MESSAGE_SEND_DATA, base_args: BaseEventArgs
    ) -> RegistryEvent:
        new_path: str | None = None
        if base_args["io_event_type"] in (
            RegCallbackClass.Reg_Pre_Rename_Key,
            RegCallbackClass.Reg_Post_Rename_Key,
        ):
            new_path = ctypes.cast(msg.DataBuffer, ctypes.c_wchar_p).value
        return cls(
            **base_args,
            new_path=new_path,
            key_name=str(msg.FileName),
            process=ProcessData.from_msg(msg),
        )


def handle_msg(msg: MESSAGE_SEND_DATA, *, can_reply: bool) -> BaseEvent:
    """Takes a MESSAGE_SEND struct and returns an equivalent Event.

    Args:
        msg: Struct to parse.
        can_reply: True if event can be replied to (message struct pointer non-NULL).
    """
    if msg.VerificationNumber != MESSAGE_SEND_VERIFICATION_NUMBER:
        m = "Driver-to-userspace communication was corrupted. This may be a struct layout issue."
        raise CorruptionError(m)

    if ev := enum_coerce(ProcessEventType, int(msg.FilterCommand)):
        args = base_args(msg, ev, can_reply=can_reply)
        return ProcessEvent.from_msg(msg, args)
    if ev := enum_coerce(EncryptEventType, int(msg.FilterCommand)):
        args = base_args(msg, ev, can_reply=can_reply)
        return EncryptEvent.from_msg(msg, args)
    if msg.FilterCommand == FilterCommand.FILTER_SEND_FILE_CHANGED_EVENT:
        args = base_args(msg, FileEventType(msg.InfoClass), can_reply=can_reply)
        return FileEvent.from_msg(msg, args)
    if msg.FilterCommand == FilterCommand.FILTER_SEND_REG_CALLBACK_INFO:
        args = base_args(msg, RegCallbackClass(msg.MessageType), can_reply=can_reply)
        return RegistryEvent.from_msg(msg, args)

    args = base_args(msg, IOCallbackClass(msg.MessageType), can_reply=can_reply)
    return FileEvent.from_msg(msg, args)


################################
################################
## outgoing reply handlers
################################
################################


def handle_msg_reply(reply: BaseReply | None, reply_data_p) -> None:  # noqa: ANN001
    """Convert a reply object to a MESSAGE_REPLY struct.

    Args:
        reply: Data to convert.
        reply_data_p: Output struct pointer.
    """
    if reply:
        if reply_data_p:
            reply.to_msg(reply_data_p)
            logging.debug("replying with: %s", reply)
        else:
            logging.warning("Callback reply discarded, because it can not be used.")


@dataclass
class DenyReply(BaseReply):
    """Deny a filter event, if possible.

    This has different effects in different modes:
    CONTROL:     I/O request is denied.
    REGISTRY:    I/O request is denied.
    PROCESS:     Process creation/termination is denied.
    ENCRYPTION:  File data is returned encrypted to the reading process.
    """

    def to_msg(self, reply_data_p) -> None:  # noqa: ANN001
        reply_data: MESSAGE_REPLY_DATA = reply_data_p.contents
        reply_data.ReturnStatus = ctypes.c_ulong(FileStatus.ACCESS_DENIED)
        reply_data.FilterStatus = ctypes.c_ulong(
            FilterStatus.FILTER_MESSAGE_IS_DIRTY
            | FilterStatus.FILTER_COMPLETE_PRE_OPERATION
        )


@dataclass
class EncryptReply(BaseReply):
    """Reply to a request for encryption key, IV, and other data.

    To deny a decryption request, use `DenyReply` instead.
    """

    encryption_key: bytes
    """Encryption key to use.

    Must be 16, 24, or 32 bytes in length.
    """

    iv: bytes | None = None
    """Specific 16-byte initialization vector to use for encryption.

    If not provided, a unique auto-generated IV will be used for each file.
    """

    tag_data: bytes | None = None
    """Optional metadata to attach to the file header.

    Only set this for a EncryptEventType.RequestIvAndKeyAndTagData reply.
    """

    def to_msg(self, reply_data_p) -> None:  # noqa: ANN001
        ln = len(self.encryption_key)
        if ln not in (16, 24, 32):
            msg = f"Encryption key must be 16, 24, or 32 bytes (got {ln})."
            raise ValueError(msg)
        reply_data: MESSAGE_REPLY_DATA = reply_data_p.contents
        reply_data.ReplyData.AESData.Data.EncryptionKeyLength = ctypes.c_ulong(ln)
        buf = (ctypes.c_byte * ln).from_buffer_copy(self.encryption_key)
        ctypes.memmove(
            reply_data.ReplyData.AESData.Data.EncryptionKey,
            buf,
            ln,
        )

        if self.iv:
            IV_LENGTH = 16
            ln = len(self.iv)
            if ln != IV_LENGTH:
                msg = f"IV must be {IV_LENGTH} bytes (got {ln})."
                raise ValueError(msg)
            reply_data_p[0].ReplyData.AESData.Data.IVLength = ln
            buf = (ctypes.c_ubyte * ln).from_buffer(bytearray(self.iv))
            ctypes.memmove(
                reply_data_p[0].ReplyData.AESData.Data.IV,
                buf,
                ln,
            )
        else:
            reply_data_p[0].ReplyData.AESData.IVLength = 0

        data_size = ctypes.sizeof(reply_data_p[0].ReplyData.AESData.Data)

        if self.tag_data is not None:
            ln: int = len(self.tag_data)
            if ln > AES_MAX_TAG_DATA_SIZE:
                msg = f"Tag data too large: {ln} > {AES_MAX_TAG_DATA_SIZE} bytes."
                raise ValueError(msg)
            data_size += ln
            reply_data_p[0].ReplyData.AESData.Data.TagDataLength = ln
            ctypes.memmove(
                reply_data_p[0].ReplyData.AESData.Data.TagData,
                self.tag_data,
                ln,
            )

        reply_data_p[0].ReplyData.AESData.SizeOfData = data_size
        reply_data_p[
            0
        ].ReplyData.AESData.Data.AccessFlag = AccessFlag.ALLOW_MAX_RIGHT_ACCESS
        reply_data_p[0].ReturnStatus = FileStatus.SUCCESS

        self.encryption_key = b"<redacted>"
        self.iv = b"<redacted>"
