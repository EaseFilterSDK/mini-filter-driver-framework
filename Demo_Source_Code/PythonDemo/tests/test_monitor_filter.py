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

"""Test monitor filter features."""

import logging
import os
import subprocess
import threading
import time
from datetime import datetime, timedelta, timezone
from pathlib import Path
from threading import Event

import pytest

from easefilter import EaseFilter
from easefilter.bitmask import str_flag
from easefilter.enums import FileEventType, FilterType
from easefilter.events import FileEvent
from easefilter.rules import FileRule
from easefilter.types import BaseEvent


def test_events(ef_handle: EaseFilter, test_dir: Path) -> None:
    """Test the monitor filter on specific events, and test uninstalling rules."""
    found_good: dict[FileEventType, Event] = {k: Event() for k in FileEventType}
    found_wrong = Event()

    magic_file: Path = test_dir / "MAGIC_FILE_NAME"
    new_magic_file: Path = test_dir / "NEW_MAGIC_FILE_NAME"
    wrong_file: Path = test_dir / "WRONG_FILE_NAME"

    rule_id_set = Event()
    rule_id: int

    def msg_hook(event_data: BaseEvent):
        if not isinstance(event_data, FileEvent):
            return

        if not rule_id_set.wait(timeout=1):
            found_wrong.set()
            msg = "rule id not set when message callback called"
            raise RuntimeError(msg)

        if event_data.file_name in (magic_file, new_magic_file):
            t = event_data.io_event_type
            if isinstance(t, FileEventType):
                for sym in t:
                    found_good[sym].set()
            else:
                found_wrong.set()
                msg = f"spurious event {t} found"
                raise RuntimeError(msg)
        if event_data.file_name == wrong_file:
            found_wrong.set()
            msg = f"wrong file name {event_data.file_name} found"
            raise RuntimeError(msg)
        if event_data.filter_rule_id != rule_id:
            found_wrong.set()
            msg = f"wrong rule id {event_data.filter_rule_id} found"
            raise RuntimeError(msg)

    def reset_events():
        for k in found_good:
            found_good[k].clear()

    def get_events():
        assert not found_wrong.wait(timeout=0.1)
        return {k for k in FileEventType if found_good[k].wait(timeout=0.1)}

    with ef_handle as ef:
        ef.message_callback = msg_hook

        ef.set_filter_type(FilterType.MONITOR)
        rule = FileRule(
            file_path=test_dir / "*",
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
        )

        for rule_install in [False, True]:
            if rule_install:
                rule.install(ef)
                assert rule.rule_id is not None
                rule_id = rule.rule_id
                rule_id_set.set()
                logging.debug("Rule ID: %d", rule_id)
            else:
                rule.uninstall(ef)

            magic_file.touch()
            logging.debug("Touched %s.", magic_file)
            ev = get_events()
            if rule_install:
                assert ev == {FileEventType.CREATED}
            else:
                assert ev == set()
            reset_events()

            with magic_file.open("w") as f:
                f.write("test contents")
            logging.debug("Wrote to %s.", magic_file)
            ev = get_events()
            if rule_install:
                assert ev == {FileEventType.WRITTEN}
            else:
                assert ev == set()

            reset_events()

            magic_file.rename(new_magic_file)
            logging.debug("File moved to %s.", new_magic_file)
            ev = get_events()
            if rule_install:
                assert ev == {FileEventType.RENAMED}
            else:
                assert ev == set()
            reset_events()

            with new_magic_file.open("r") as f:
                _ = f.read()
            logging.debug("Read content from %s.", new_magic_file)
            ev = get_events()
            if rule_install:
                assert ev == {FileEventType.READ}
            else:
                assert ev == set()
            reset_events()

            new_magic_file.unlink()
            logging.debug("Deleted %s.", new_magic_file)
            ev = get_events()
            if rule_install:
                assert ev == {FileEventType.DELETED}
            else:
                assert ev == set()
            reset_events()


def test_rule_id(ef_handle: EaseFilter, test_dir: Path) -> None:
    """Test that monitor rule IDs make sense."""
    file1: Path = test_dir / "MAGIC_FILE_NAME"
    file2: Path = test_dir / "NEWER_FILE_NAME"

    rule1_id: int
    rule2_id: int

    found1 = Event()
    found2 = Event()

    wrong = Event()

    def msg_hook(event_data: BaseEvent):
        if not isinstance(event_data, FileEvent):
            return

        if event_data.file_name == file1:
            found1.set()
            if rule1_id != event_data.filter_rule_id:
                wrong.set()
        if event_data.file_name == file2:
            found2.set()
            if rule2_id != event_data.filter_rule_id:
                wrong.set()

    with ef_handle as ef:
        ef.message_callback = msg_hook

        ef.set_filter_type(FilterType.MONITOR)
        rule1 = FileRule(
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
        )
        rule1.install(ef)
        assert rule1.rule_id is not None
        rule1_id = rule1.rule_id

        rule2 = FileRule(
            file_path=test_dir / "NEWER_FILE_NAME",
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
        )
        rule2.install(ef)
        assert rule2.rule_id is not None
        rule2_id = rule2.rule_id
        assert rule2_id != rule1_id

        file1.touch()
        file2.touch()

        assert not wrong.wait(timeout=0.1)
        assert found1.wait(timeout=0.1)
        assert found2.wait(timeout=0.1)


def test_ids(ef_handle: EaseFilter, test_dir: Path, win_sid) -> None:
    """Test that PID/thread IDs, SID and user/domain info come back correctly."""
    file: Path = test_dir / "MAGIC_FILE_NAME"

    pid: int
    tid: int
    sid: str
    whoami: str

    found = Event()
    wrong = Event()

    def msg_hook(event_data: BaseEvent):
        if not isinstance(event_data, FileEvent):
            return
        if event_data.file_name == file:
            found.set()
        if event_data.process.tid != tid:
            logging.debug(
                "Thread ID wrong, got: '%d'; expected '%d'",
                event_data.process.tid,
                tid,
            )
            wrong.set()
        if event_data.process.pid != pid:
            logging.debug(
                "PID wrong, got: '%d'; expected '%d'",
                event_data.process.pid,
                pid,
            )
            wrong.set()
        if event_data.sid != sid:
            logging.debug("SID wrong, got: '%s'; expected '%s'", event_data.sid, sid)
            wrong.set()

        got_whoami: str = (
            f"{event_data.account.domain}\\{event_data.account.username}".lower()
        )
        if got_whoami != whoami:
            logging.debug(
                "User info wrong, got '%s'; expected '%s'",
                got_whoami,
                whoami,
            )
            wrong.set()

    with ef_handle as ef:
        ef.message_callback = msg_hook

        pid = os.getpid()
        tid = threading.get_ident()
        sid = win_sid
        whoami = (
            subprocess.check_output("C:\\Windows\\System32\\whoami")
            .decode()
            .lower()
            .strip()
        )

        ef.set_filter_type(FilterType.MONITOR)
        rule1 = FileRule(
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
        )
        rule1.install(ef)

        file.touch()

        assert not wrong.wait(timeout=0.1)
        assert found.wait(timeout=0.1)


def test_process_path(ef_handle: EaseFilter, test_dir: Path) -> None:
    """Test process file path."""
    file: Path = test_dir / "MAGIC_FILE_NAME"
    with file.open("w") as f:
        f.write("file contents")

    found = Event()
    wrong = Event()

    def msg_hook(event_data: BaseEvent):
        if not isinstance(event_data, FileEvent):
            return
        if event_data.file_name == file:
            found.set()
        if (path := event_data.process.path) != (
            cmd_path := Path("C:\\") / "Windows" / "System32" / "cmd.exe"
        ):
            logging.debug("Process path wrong, got '%s'; expected '%s'", path, cmd_path)
            wrong.set()

    with ef_handle as ef:
        ef.message_callback = msg_hook

        ef.set_filter_type(FilterType.MONITOR)
        rule1 = FileRule(
            file_path=file,
            change_event_filter=str_flag(
                [
                    "READ",
                ],
                FileEventType,
            ),
        )
        rule1.install(ef)

        # ping as delay
        cmd_line = [
            "cmd.exe",
            "/C",
            "type",
            str(file),
            "&&",
            "ping",
            "localhost",
            "-n",
            "2",
            ">nul",
        ]
        logging.debug("Running '%s'", " ".join(cmd_line))
        outp: str = subprocess.check_output(cmd_line).decode()
        assert outp == "file contents"

        assert not wrong.wait(timeout=0.1)
        assert found.wait(timeout=0.1)


def test_timestamps(ef_handle: EaseFilter, test_dir: Path) -> None:
    """Test file timestamps."""
    file: Path = test_dir / "MAGIC_FILE_NAME"
    file.touch()

    def ns_ts_to_date(t: int) -> datetime:
        """Round Unix timestamp nanoseconds to the nearest 100 nanosecond tick.

        This makes it correspond to the resolution of Windows' timestamps.
        """
        # hundred-nanosecond tick
        t = t // 100

        return datetime.fromtimestamp(0, tz=timezone.utc) + timedelta(
            microseconds=t / 10,
        )

    time_create = ns_ts_to_date(file.stat().st_ctime_ns)

    # ensure we have different timestamps for create/write
    time.sleep(0.1)
    with file.open("w") as f:
        f.write("file contents")
    time_write = ns_ts_to_date(file.stat().st_mtime_ns)

    # lock to ensure we read the access time through python before verifying
    # filter's time against it
    ready = Event()

    time_access = None

    found = Event()
    wrong = Event()

    def msg_hook(event_data: BaseEvent):
        if not isinstance(event_data, FileEvent):
            return
        if event_data.file_name == file:
            assert ready.wait(timeout=0.1)
            found.set()

            def fuzzy_eq(
                t1: datetime,
                t2: datetime,
                epsilon: timedelta = timedelta(microseconds=5),
            ) -> bool:
                """Fuzzy equality for timestamps.

                Arguments:
                    t1: Timestamp.
                    t2: Timestamp.
                    epsilon: Maximum allowed difference between timestamps.

                Returns:
                    If the timestamps are similar enough to be considered qual.
                """
                if t2 > t1:
                    t1, t2 = t2, t1

                return t2 - t1 <= epsilon

            assert time_access is not None

            # access time may not be always updated for performance reasons
            # it could be equal to write time
            if not fuzzy_eq(event_data.time.access, time_access) and not fuzzy_eq(
                event_data.time.access,
                time_write,
            ):
                logging.debug(
                    "Got atime %s, expected %s",
                    event_data.time.access,
                    time_access,
                )
                wrong.set()
            if not fuzzy_eq(event_data.time.write, time_write):
                logging.debug(
                    "Got mtime %s, expected %s",
                    event_data.time.write,
                    time_write,
                )
                wrong.set()
            if not fuzzy_eq(event_data.time.create, time_create):
                logging.debug(
                    "Got ctime %s, expected %s",
                    event_data.time.create,
                    time_create,
                )
                wrong.set()

    with ef_handle as ef:
        ef.message_callback = msg_hook

        ef.set_filter_type(FilterType.MONITOR)
        rule1 = FileRule(
            file_path=file,
            change_event_filter=str_flag(
                [
                    "READ",
                ],
                FileEventType,
            ),
        )
        rule1.install(ef)

        time.sleep(0.05)

        with file.open("r") as f:
            content = f.read()
            assert content == "file contents"
            time_access = ns_ts_to_date(file.stat().st_atime_ns)
        ready.set()

        assert not wrong.wait(timeout=0.1)
        assert found.wait(timeout=0.1)


@pytest.mark.parametrize("event_type", ["RENAMED", "COPIED"])
def test_rename_copy(ef_handle: EaseFilter, test_dir: Path, event_type: str) -> None:
    """Test that rename/copied events have the correct new file name."""
    if event_type == "COPIED":
        pytest.skip()  # COPIED broken in the test environment

    file: Path = test_dir / "MAGIC_FILE_NAME"
    file2: Path = test_dir / "NEW_MAGIC_FILE_NAME"
    file3: Path = test_dir / "NEWER_MAGIC_FILE_NAME"

    found = Event()
    wrong = Event()

    def msg_hook(event_data: BaseEvent):
        if not isinstance(event_data, FileEvent):
            return
        target: Path = file2

        if event_type == "RENAMED":
            if event_data.file_name == file:
                found.set()
                target = file2
            elif event_data.file_name == file2:
                found.set()
                target = file3
        elif event_type == "COPIED":
            if event_data.file_name == file2:
                found.set()
                target = file
            elif event_data.file_name == file3:
                found.set()
                target = file2

        if event_data.new_path is None:
            wrong.set()
            msg = "New path was empty."
            raise ValueError(msg)
        if event_data.new_path != target:
            logging.debug(
                "New path wrong, got: '%s'; expected '%s'",
                event_data.new_path,
                target,
            )
            wrong.set()

    with ef_handle as ef:
        ef.message_callback = msg_hook

        ef.set_filter_type(FilterType.MONITOR)
        rule1 = FileRule(
            file_path=test_dir / "*",
            change_event_filter=str_flag(
                [
                    event_type,
                ],
                FileEventType,
            ),
        )
        logging.debug("applying rule: %s", rule1)
        rule1.install(ef)

        def copy(f1: Path, f2: Path):
            cmd = ["copy", str(f1), str(f2)]
            logging.debug("performing: %s", cmd)
            subprocess.run(cmd, shell=True, check=True)

        file.touch()
        if event_type == "RENAMED":
            file.rename(file2)
            file2.rename(file3)
        elif event_type == "COPIED":
            assert not file2.exists()
            copy(file, file2)
            assert file2.exists()

            assert not file3.exists()
            copy(file2, file3)
            assert file3.exists()

        assert not wrong.wait(timeout=0.1)
        assert found.wait(timeout=0.1)
