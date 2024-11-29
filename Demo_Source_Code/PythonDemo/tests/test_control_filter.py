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

"""Test control filter features."""

from __future__ import annotations

import logging
import operator
from functools import reduce
from time import sleep
from typing import TYPE_CHECKING

import pytest

from easefilter.bitmask import str_flag
from easefilter.enums import (
    AccessFlag,
    FileEventType,
    FilterType,
    InformationClass,
    IOCallbackClass,
)
from easefilter.events import DenyReply, FileEvent
from easefilter.rules import FileRule

if TYPE_CHECKING:
    from collections.abc import Callable
    from pathlib import Path

    from easefilter import EaseFilter
    from easefilter.types import BaseEvent


def perm_tester(
    test_dir: Path, disable_rule: Callable[[Callable], None]
) -> set[AccessFlag]:
    """Test manipulations on files, reporting ones that fail.

    Args:
        test_dir: Directory to perform the test in.
        disable_rule: Wrapper closure that temporarily disables the filter rule.
    """
    fails: set[AccessFlag] = set()
    file: Path = test_dir / "MAGIC_FILE_NAME"
    file2: Path = test_dir / "NEW_FILE_NAME"

    disable_rule(file.touch)

    Err = (OSError, PermissionError)

    try:
        file.read_text()
    except Err:
        fails.add(AccessFlag.ALLOW_READ_ACCESS)

    try:
        file.write_text("test text")
    except Err:
        fails.add(AccessFlag.ALLOW_WRITE_ACCESS)

    try:
        file.rename(file2)
        if not file2.exists:
            raise OSError
    except Err:
        fails.add(AccessFlag.ALLOW_FILE_RENAME)
    else:
        disable_rule(file.touch)

    try:
        file.unlink()
        if file.exists():
            raise OSError
    except Err:
        fails.add(AccessFlag.ALLOW_FILE_DELETE)
    else:
        disable_rule(file.touch)

    try:
        f = file.open("r")
        f.close()
    except Err:
        fails.add(AccessFlag.ALLOW_OPEN_WITH_READ_ACCESS)

    try:
        f = file.open("w")
        f.close()
    except Err:
        fails.add(AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS)

    disable_rule(file.unlink)
    try:
        f = file.touch()
    except Err:
        fails.add(AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS)

    return fails


@pytest.mark.parametrize(
    ("deny_perms", "expected_fails"),
    [
        pytest.param((), set(), id="empty"),
        pytest.param(
            (AccessFlag.ALLOW_READ_ACCESS),
            {AccessFlag.ALLOW_READ_ACCESS},
            id="read",
        ),
        pytest.param(
            (AccessFlag.ALLOW_WRITE_ACCESS),
            {AccessFlag.ALLOW_WRITE_ACCESS},
            id="write",
        ),
        pytest.param(
            (AccessFlag.ALLOW_WRITE_ACCESS, AccessFlag.ALLOW_READ_ACCESS),
            {AccessFlag.ALLOW_WRITE_ACCESS, AccessFlag.ALLOW_READ_ACCESS},
            id="read-write",
        ),
        pytest.param(
            (AccessFlag.ALLOW_FILE_RENAME,),
            {AccessFlag.ALLOW_FILE_RENAME},
            id="rename",
        ),
        pytest.param(
            (AccessFlag.ALLOW_FILE_DELETE,),
            {AccessFlag.ALLOW_FILE_DELETE},
            id="delete",
        ),
        pytest.param(
            (AccessFlag.ALLOW_OPEN_WITH_READ_ACCESS,),
            {
                AccessFlag.ALLOW_READ_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_READ_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS,
                AccessFlag.ALLOW_WRITE_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS,
                AccessFlag.ALLOW_FILE_RENAME,
            },
            id="openread",
        ),
        pytest.param(
            (AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS,),
            {
                AccessFlag.ALLOW_WRITE_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS,
                AccessFlag.ALLOW_FILE_RENAME,
                AccessFlag.ALLOW_FILE_DELETE,
                AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS,
            },
            id="openwrite",
        ),
        pytest.param(
            (AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS,),
            {
                AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS,
                AccessFlag.ALLOW_WRITE_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS,
            },
            id="opencreate",
        ),
    ],
)
def test_permissions(
    ef_handle: EaseFilter,
    test_dir: Path,
    deny_perms: tuple[AccessFlag],
    expected_fails: set[AccessFlag],
) -> None:
    """Test denying certain permissions for files.

    This test is by no means exhaustive.
    """
    deny_mask: AccessFlag = reduce(operator.ior, deny_perms, AccessFlag(0))
    logging.debug("deny mask is %s", deny_mask)

    def msg_hook(_):
        pass

    with ef_handle as ef:
        ef.message_callback = msg_hook

        ef.set_filter_type(FilterType.CONTROL)
        rule = FileRule(
            file_path=test_dir / "MAGIC_FILE_NAME",
            change_event_filter=str_flag(
                [
                    "CREATED",
                    "WRITTEN",
                    "RENAMED",
                    "DELETED",
                    "READ",
                ],
                FileEventType,
            ),
            access_flag=AccessFlag.ALLOW_MAX_RIGHT_ACCESS & ~deny_mask,
        )

        rule.install(ef)

        def disable_rule(f: Callable):
            """Disable rule and run function."""
            rule.uninstall(ef)
            f()
            rule.install(ef)

        sleep(0.01)

        fails = perm_tester(test_dir, disable_rule)

        assert fails == expected_fails


def test_dir_permissions(
    ef_handle: EaseFilter,
    test_dir: Path,
) -> None:
    """Test denying directory list access."""
    file: Path = test_dir / "MAGIC_FILE_NAME"

    def msg_hook(_):
        pass

    with ef_handle as ef:
        ef.message_callback = msg_hook

        file.touch()

        ef.set_filter_type(FilterType.CONTROL)
        rule = FileRule(
            file_path=test_dir / "*",
            change_event_filter=str_flag(
                [],
                FileEventType,
            ),
            access_flag=AccessFlag.ALLOW_MAX_RIGHT_ACCESS
            & ~AccessFlag.ALLOW_DIRECTORY_LIST_ACCESS,
        )

        assert list(test_dir.iterdir()) == [file]

        rule.install(ef)

        sleep(0.01)

        with pytest.raises(PermissionError):
            list(test_dir.iterdir())

        file.write_text("test text")
        assert file.read_text() == "test text"


@pytest.mark.parametrize(
    ("control_mask", "expected_fails"),
    [
        pytest.param((), set()),
        pytest.param(
            {
                IOCallbackClass.PRE_CREATE,
                IOCallbackClass.PRE_DELETE_FILE,
            },
            {
                AccessFlag.ALLOW_READ_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_READ_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS,
                AccessFlag.ALLOW_WRITE_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS,
                AccessFlag.ALLOW_FILE_RENAME,
                AccessFlag.ALLOW_FILE_DELETE,
            },
        ),
        pytest.param(
            {
                IOCallbackClass.PRE_CACHE_READ,
                IOCallbackClass.PRE_NOCACHE_READ,
                IOCallbackClass.PRE_FASTIO_READ,
                IOCallbackClass.PRE_PAGING_IO_READ,
            },
            {
                AccessFlag.ALLOW_READ_ACCESS,
            },
        ),
        pytest.param(
            {
                IOCallbackClass.PRE_CACHE_WRITE,
                IOCallbackClass.PRE_NOCACHE_WRITE,
                IOCallbackClass.PRE_FASTIO_WRITE,
                IOCallbackClass.PRE_PAGING_IO_WRITE,
            },
            {
                AccessFlag.ALLOW_WRITE_ACCESS,
            },
        ),
        pytest.param(
            {IOCallbackClass.PRE_DELETE_FILE, IOCallbackClass.PRE_SET_INFORMATION},
            {
                AccessFlag.ALLOW_FILE_DELETE,
            },
        ),
        pytest.param(
            {IOCallbackClass.PRE_RENAME_FILE, IOCallbackClass.PRE_SET_INFORMATION},
            {
                AccessFlag.ALLOW_FILE_RENAME,
            },
        ),
        pytest.param(
            set(IOCallbackClass),
            {
                AccessFlag.ALLOW_READ_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_READ_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS,
                AccessFlag.ALLOW_WRITE_ACCESS,
                AccessFlag.ALLOW_OPEN_WITH_WRITE_ACCESS,
                AccessFlag.ALLOW_FILE_RENAME,
                AccessFlag.ALLOW_FILE_DELETE,
            },
        ),
    ],
)
@pytest.mark.parametrize("control_all", [False, True])
def test_dynamic_deny(
    ef_handle: EaseFilter,
    test_dir: Path,
    control_mask: set[IOCallbackClass],
    expected_fails: set[AccessFlag],
    *,
    control_all: bool,
) -> None:
    """Test denying access programmatically."""

    def msg_hook(data: BaseEvent) -> DenyReply | None:
        if not isinstance(data, FileEvent):
            return None

        if isinstance(data.io_event_type, IOCallbackClass) and (
            any(
                i in data.io_event_type
                for i in control_mask
                if i != IOCallbackClass.PRE_SET_INFORMATION
            )
            or (
                data.info_class
                in (
                    InformationClass.FileRenameInformationEx,
                    InformationClass.FileRenameInformation,
                )
                and IOCallbackClass.PRE_RENAME_FILE in control_mask
            )
            or (
                data.info_class
                in (
                    InformationClass.FileDispositionInformation,
                    InformationClass.FileDispositionInformationEx,
                )
                and IOCallbackClass.PRE_DELETE_FILE in control_mask
            )
        ):
            return DenyReply()

        return None

    with ef_handle as ef:
        ef.message_callback = msg_hook

        ef.set_filter_type(FilterType.CONTROL)

        rule = FileRule(
            file_path=test_dir / "*",
            control_io_filter=reduce(
                operator.ior, IOCallbackClass if control_all else control_mask, 0
            ),
            access_flag=AccessFlag.ALLOW_MAX_RIGHT_ACCESS,
        )

        rule.install(ef)

        def disable_rule(f: Callable):
            """Disable rule and run function."""
            rule.uninstall(ef)
            f()
            rule.install(ef)

        sleep(0.01)

        fails = perm_tester(test_dir, disable_rule)

        assert fails == expected_fails
