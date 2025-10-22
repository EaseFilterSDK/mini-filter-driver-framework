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

"""Miscellaneous high-level interface types."""

from __future__ import annotations

from abc import ABC, ABCMeta, abstractmethod
from dataclasses import dataclass
from typing import TYPE_CHECKING, Optional, Union

from typing_extensions import Self

from easefilter.filter_api import (
    MESSAGE_SEND_DATA,
    AccountData,
    GetProcessPathByPid,
    windows_timestamp,
)

if TYPE_CHECKING:
    import datetime

    from easefilter.events import BaseEventArgs
from collections.abc import Callable
from pathlib import Path

from easefilter.enums import (
    EncryptEventType,
    FileEventType,
    IOCallbackClass,
    ProcessEventType,
    RegCallbackClass,
)


@dataclass
class ProcessData:
    """Process/thread information.

    Fields may be unknown, depending on the
    situation, in which case they will be `None`.
    """

    pid: int | None
    """Process ID."""

    tid: int | None
    """Thread ID."""

    path: Path | None
    """Executable path."""

    @classmethod
    def from_msg(cls: type[Self], msg: MESSAGE_SEND_DATA) -> Self:
        """Instantiate from MESSAGE_SEND."""
        process_path = GetProcessPathByPid(msg.ProcessId)

        return cls(
            pid=int(msg.ProcessId),
            tid=int(msg.ThreadId),
            path=Path(process_path) if process_path else None,
        )


@dataclass
class FileTimes:
    """Timestamps for a file."""

    create: datetime.datetime
    """Creation time."""

    access: datetime.datetime
    """Last access time."""

    write: datetime.datetime
    """Last write time."""

    @classmethod
    def from_msg(cls: type[Self], msg: MESSAGE_SEND_DATA) -> Self:
        """Instantiate from MESSAGE_SEND."""
        return cls(
            create=windows_timestamp(int(msg.CreationTime)),
            access=windows_timestamp(int(msg.LastAccessTime)),
            write=windows_timestamp(int(msg.LastWriteTime)),
        )


IOEvent = Union[
    IOCallbackClass, FileEventType, ProcessEventType, EncryptEventType, RegCallbackClass
]


@dataclass
class BaseEvent(metaclass=ABCMeta):
    """Base class for data about a filter event.

    Do not instantiate directly.
    Subclasses should also be data-classes.
    """

    @classmethod
    @abstractmethod
    def from_msg(
        cls: type[BaseEvent], msg: MESSAGE_SEND_DATA, base_args: BaseEventArgs
    ) -> BaseEvent:
        """Instantiate class from a raw MESSAGE_SEND_DATA struct.

        Args:
            msg: Data to instantiate from.
            base_args: Pre-computed arguments in the BaseEvent dataclass. Subclasses should only deal with their own arguments, and splat these common arguments out.
        """

    io_event_type: IOEvent
    """I/O event type(s)."""

    filter_rule_id: int
    """ID of the filter rule that fired this event."""

    message_id: int
    """Sequential message ID."""

    sid: str
    """Windows Security Identifier (SID) string of the user that initiated this event."""

    account: AccountData
    """Data about the user that initiated this event."""

    can_reply: bool
    """Set to `True` if this event may be replied to with a `CallbackReplyData`."""


@dataclass
class BaseReply(ABC):
    """Base class that the Python callback code can send to the driver in a reply.

    Do not instantiate directly.

    This is useful in, e.g., Control mode, where userspace needs to tell the
    driver to deny/modify an I/O operation.
    """

    @abstractmethod
    def to_msg(self, reply_data_p) -> None:  # noqa: ANN001
        """Convert this class into a raw MESSAGE_REPLY_DATA struct.

        Args:
            reply_data_p: Pointer to the struct. Guaranteed non-null by caller.
        """


MsgCallback = Callable[[BaseEvent], Optional[BaseReply]]
DisconCallback = Callable[[], None]
