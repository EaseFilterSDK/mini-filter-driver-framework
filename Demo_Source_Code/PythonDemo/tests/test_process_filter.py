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

"""Test process filter features."""

from __future__ import annotations

import inspect
import logging
import os
import shutil
import subprocess
import sys
import threading
import venv
from pathlib import Path
from threading import Event, Lock
from typing import TYPE_CHECKING

import pytest

if TYPE_CHECKING:
    from easefilter import EaseFilter
    from easefilter.types import BaseEvent

from easefilter.enums import FilterType, ProcessControlFlag, ProcessEventType
from easefilter.events import DenyReply, ProcessEvent
from easefilter.rules import ProcessRule


def process_test(n_threads: int):
    """Test script that spawns threads.

    Do not call directly; this is a helper for `test_process_thread_monitor`.
    It is intended to run in a separate script file, under another Python process, so that it may be isolated in the test.

    Args:
        n_threads: Total number of threads to spawn, including the initial one which already exists.
    """

    def thread_worker(_):
        import time

        return time.sleep(0.02)

    import os
    import threading

    if __name__ == "__main__":
        print(os.getpid())  # noqa: T201
        workers = []
        for i in range(n_threads - 1):
            t = threading.Thread(target=thread_worker, args=(i,))
            t.start()
            workers.append(t)

        _ = threading.current_thread()
        assert len(threading.enumerate()) == n_threads

        for t in workers:
            t.join()


@pytest.mark.parametrize("enable_rule", [False, True])
@pytest.mark.parametrize("n_processes", [1, 3, 5])
@pytest.mark.parametrize("n_threads", list(range(1, 3)))
def test_process_thread_monitor(
    *,
    ef_handle: EaseFilter,
    test_dir: Path,
    n_processes: int,
    enable_rule: bool,
    n_threads: int,
):
    """Monitor process/thread creation/termination.

    Currently, thread monitoring is not tested because Python venv threads are
    hard to manage consistently, and thus make for flaky tests.

    Args:
        ef_handle: EaseFilter handle.
        test_dir: Testing temp dir.
        n_processes: Number of processes to spawn.
        n_threads: Threads per process to spawn.
        enable_rule: If disabled, uninstalls filter rule immediately.
    """
    lock = Lock()
    process_create: int = 0
    process_del: int = 0
    thread_create: int = 0
    thread_del: int = 0
    good = Event()
    bad = Event()

    def msg_hook(data: BaseEvent):
        if not isinstance(data, ProcessEvent):
            return

        nonlocal process_create, process_del, thread_create, thread_del
        lock.acquire(timeout=5)
        if data.io_event_type == ProcessEventType.ProcessCreationInfo:
            process_create += 1
        elif data.io_event_type == ProcessEventType.ProcessTerminated:
            process_del += 1
        elif data.io_event_type == ProcessEventType.ThreadCreated:
            thread_create += 1
        elif data.io_event_type == ProcessEventType.ThreadTerminated:
            thread_del += 1

        if not enable_rule and (
            max(process_create, process_del, thread_create, thread_del) > 0
        ):
            bad.set()
            logging.error("process/thread detected when rule disabled")
        if max(process_create, process_del) > n_processes:
            bad.set()
            logging.error("too many processes %d / %d", process_create, process_del)
        if process_create == n_processes and process_del == n_processes:
            good.set()
        lock.release()

    venv_path = test_dir / Path("venv")
    exec_path = venv_path / Path("Scripts/python.exe")

    with ef_handle as ef:
        ef.message_callback = msg_hook
        ef.set_filter_type(FilterType.PROCESS)

        # we're going to run a script using a separate copy of Python
        venv.create(venv_path)
        script: str = (
            inspect.getsource(process_test) + f"""\nprocess_test({n_threads})"""
        )
        script_path = test_dir / Path("script.py")
        script_path.write_text(script)

        rule = ProcessRule(
            executable_mask=venv_path / Path("*"),
            control_flag=ProcessControlFlag.PROCESS_CREATION_NOTIFICATION
            | ProcessControlFlag.PROCESS_TERMINATION_NOTIFICATION
            | ProcessControlFlag.THREAD_CREATION_NOTIFICATION
            | ProcessControlFlag.THREAD_TERMINATION_NOTIFICATION,
        )
        rule.install(ef)
        if not enable_rule:
            rule.uninstall(ef)

        pids: set[str] = set()
        for _ in range(n_processes):
            pids.add(
                subprocess.check_output([str(exec_path), str(script_path)])
                .decode("utf-8")
                .strip()
            )
        assert len(pids) == n_processes
        logging.debug(pids)

        if enable_rule:
            assert good.wait(timeout=0.5)
        assert not bad.wait(timeout=0.1)

        lock.acquire(timeout=5)
        if enable_rule:
            assert process_create == n_processes
            assert process_del == n_processes
        else:
            assert process_create == 0
            assert process_del == 0
        lock.release()


def test_deny_process(*, ef_handle: EaseFilter, test_dir: Path):
    """Deny process start."""
    # hatch venv python won't work outside of venv
    # however it will run if it is not denied
    src_path = Path(sys.executable)
    exec_path = test_dir / "python.exe"
    shutil.copy(src_path, exec_path)

    def msg_hook(_): ...

    rule = ProcessRule(
        exec_path,
        ProcessControlFlag.DENY_NEW_PROCESS_CREATION,
    )

    with ef_handle as ef:
        ef.message_callback = msg_hook
        ef.set_filter_type(FilterType.PROCESS)

        rule.install(ef)
        with pytest.raises(PermissionError):
            subprocess.check_call(exec_path)

        rule.uninstall(ef)

        # this binary should be broken and return non-zero
        # still it should run
        with pytest.raises(subprocess.CalledProcessError):
            subprocess.check_call([exec_path, "garbage_arg"])


def test_dynamic_deny(*, ef_handle: EaseFilter, test_dir: Path):
    """Deny process start, programmatically."""
    # hatch venv python won't work outside of venv
    # however it will run if it is not denied
    src_path = Path(sys.executable)
    exec_path = test_dir / "python.exe"
    shutil.copy(src_path, exec_path)

    forbidden_word: str = "password123"

    def msg_hook(msg: BaseEvent) -> DenyReply | None:
        if not isinstance(msg, ProcessEvent):
            return None
        if msg.command_line and forbidden_word in msg.command_line:
            return DenyReply()
        return None

    rule = ProcessRule(
        exec_path,
        ProcessControlFlag.PROCESS_CREATION_NOTIFICATION,
    )

    with ef_handle as ef:
        ef.message_callback = msg_hook
        ef.set_filter_type(FilterType.PROCESS)

        rule.install(ef)
        subprocess.run(exec_path, check=False)
        subprocess.run([exec_path, "argument", "--flag"], check=False)

        with pytest.raises(PermissionError):
            subprocess.run(
                [exec_path, "--secret", forbidden_word, "--flag"], check=False
            )


def info_test():
    """Helper script that prints PID/TID.

    Do not call directly, as this is intended to run in a separate Python instance.
    """
    import os
    from threading import get_native_id

    # https://stackoverflow.com/a/64891362
    # this should be just getpid(), but windows venv breaks that
    print(os.getppid())  # noqa: T201
    print(get_native_id())  # noqa: T201


def test_process_info(*, ef_handle: EaseFilter, test_dir: Path):
    """Test PROCESS_INFO struct fields.

    Notably:
        - Parent process
        - Creating process/thread
        - Child process
        - Command line
    """
    venv_path = test_dir / Path("venv")
    exec_path = venv_path / Path("Scripts/python.exe")

    magic_param = "MAGIC_COOKIE1234👍🐸"
    """Argument to be passed to the child process to test the command line field.

    Includes some emoji for good measure.
    """

    par_pid = os.getpid()
    """Parent process ID (reference value)."""
    creating_tid = threading.get_native_id()
    """Creator thread ID (reference value)."""
    good_p_pid = Event()
    """Has par_pid been found in the callback thread yet."""
    good_cr_tid = Event()
    """Has creating_tid been found in the callback thread yet."""

    child_pid: int | None = None
    """Child PID, assigned in the callback thread and verified in the parent."""
    found_child_pid = Event()
    """Has `child_pid` been set by the callback thread yet."""

    bad = Event()
    """Has an error occured in the callback."""

    def msg_hook(msg: BaseEvent):
        if not isinstance(msg, ProcessEvent):
            logging.error("spurious event %s", msg)
            bad.set()
            return

        if msg.io_event_type == ProcessEventType.ProcessCreationInfo:
            nonlocal child_pid
            child_pid = msg.process.pid
            found_child_pid.set()
            if msg.creating_proc:
                if msg.creating_proc.tid != creating_tid:
                    bad.set()
                    logging.error(
                        "creating tid invalid: got %d instead of %d",
                        msg.creating_proc.tid,
                        creating_tid,
                    )
                else:
                    good_cr_tid.set()

        if msg.parent_proc:
            if msg.parent_proc.pid != par_pid:
                bad.set()
                logging.error(
                    "parent pid invalid: got %d instead of %d",
                    msg.parent_proc.pid,
                    par_pid,
                )
            else:
                good_p_pid.set()

        if msg.command_line:
            assert msg.command_line.endswith(magic_param)

    with ef_handle as ef:
        ef.message_callback = msg_hook
        ef.set_filter_type(FilterType.PROCESS)

        # we're going to run a script using a separate copy of Python
        venv.create(venv_path)
        script: str = inspect.getsource(info_test) + """\ninfo_test()"""
        script_path = test_dir / Path("script.py")
        script_path.write_text(script)

        rule = ProcessRule(
            executable_mask=exec_path,
            control_flag=ProcessControlFlag.PROCESS_CREATION_NOTIFICATION
            | ProcessControlFlag.PROCESS_TERMINATION_NOTIFICATION
            | ProcessControlFlag.THREAD_CREATION_NOTIFICATION,
        )
        rule.install(ef)

        child_proc = subprocess.Popen(
            [exec_path, script_path, magic_param], stdout=subprocess.PIPE, text=True
        )
        logging.debug("process opened.")
        stdout, _ = child_proc.communicate()
        logging.debug("communication finished.")
        outp = stdout.splitlines()
        logging.debug("child stdout: %s", outp)

        # child pid/tid as seen by itself
        p_child_pid, child_tid = (int(i) for i in outp)
        assert p_child_pid == child_proc.pid
        logging.debug("child pid / tid : %d / %d", p_child_pid, child_tid)

        assert found_child_pid.wait(timeout=0.5)
        assert child_pid == p_child_pid

        assert all([e.wait(timeout=0.5) for e in [good_p_pid, good_cr_tid]])  # noqa: C419 (unnecessary list comprehension; useful in pytest output)
        assert not bad.wait(timeout=0.5)
