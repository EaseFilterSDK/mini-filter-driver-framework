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

"""Test registry filter features."""

from __future__ import annotations

import functools
import logging
import operator
import os
import subprocess
import winreg
from pathlib import Path
from threading import Event, Lock
from typing import TYPE_CHECKING

import pytest

from easefilter.enums import FilterType, RegCallbackClass, RegControlFlag
from easefilter.events import DenyReply, RegistryEvent
from easefilter.rules import RegistryProcIdRule, RegistryProcNameRule

if TYPE_CHECKING:
    from easefilter import EaseFilter
    from easefilter.types import BaseEvent


def test_no_default():
    """Test that invalid rules can not be created."""
    with pytest.raises(TypeError):
        RegistryProcIdRule(key_mask="", callback_class=RegCallbackClass(0))
    with pytest.raises(TypeError):
        RegistryProcNameRule(key_mask="", callback_class=RegCallbackClass(0))


def test_proc_name_monitor(ef_handle: EaseFilter):
    """Test registry event monitoring, matching by process name."""
    app_name = "EasefilterPython"

    rule = RegistryProcNameRule(
        f"*\\{app_name}\\*",
        functools.reduce(operator.ior, RegCallbackClass),
        proc_name=Path(f"{os.environ['WINDIR']}\\system32\\reg.exe"),
    )

    test_keys = {"1", "test_name", "1234", "TEST"}

    lock = Lock()
    found_keys: set[str] = set()
    done = Event()
    bad = Event()

    def msg_hook(ev: BaseEvent) -> None:
        if not isinstance(ev, RegistryEvent):
            logging.warning("spurious event %s", ev)
            return

        with lock:
            found_keys.add(ev.key_name.split("\\")[-1].strip(":"))
            if len(found_keys) > len(test_keys):
                logging.info("too many keys: %s", found_keys)
                bad.set()
            if len(found_keys) == len(test_keys):
                done.set()

    with ef_handle as ef:
        ef.set_filter_type(FilterType.REGISTRY)
        ef.message_callback = msg_hook
        rule.install(ef)
        logging.debug("applied rule: %s", rule)

        with (
            winreg.OpenKeyEx(winreg.HKEY_CURRENT_USER, "Software\\") as soft_key,
            winreg.CreateKey(soft_key, app_name + "\\"),
        ):
            for k in test_keys:
                key_path = "HKCU\\Software\\" + app_name + "\\" + k

                c = ["reg", "add", key_path, "/f"]
                logging.debug("running: %s", c)
                subprocess.run(c, check=True, shell=True)

                c = ["reg", "delete", key_path, "/f"]
                logging.debug("running: %s", c)
                subprocess.run(c, check=True, shell=True)

        assert done.wait(timeout=0.2)
        assert not bad.wait(timeout=0.2)
        with lock:
            assert found_keys == test_keys


def key_exists(k: str) -> bool:
    try:
        subprocess.run(["reg", "query", k], shell=True, check=True)
    except subprocess.CalledProcessError:
        return False
    else:
        return True


@pytest.mark.parametrize("install", [False, True])
def test_proc_deny(*, ef_handle: EaseFilter, install: bool):
    """Test denying access to a key via rule."""
    app_name = "EasefilterPython"

    rule = RegistryProcNameRule(
        f"*\\{app_name}\\*",
        functools.reduce(operator.ior, RegCallbackClass),
        proc_name=Path(f"{os.environ['WINDIR']}\\system32\\reg.exe"),
        access_flag=RegControlFlag.REG_MAX_ACCESS_FLAG
        & ~RegControlFlag.REG_ALLOW_OPEN_KEY,
    )

    with ef_handle as ef:
        ef.message_callback = lambda _: None

        ef.set_filter_type(FilterType.REGISTRY)
        rule.install(ef)

        if not install:
            rule.uninstall(ef)

        with (
            winreg.OpenKeyEx(winreg.HKEY_CURRENT_USER, "Software\\") as soft_key,
            winreg.CreateKey(soft_key, app_name + "\\"),
        ):
            k = "static_deny_key"
            key_path = "HKCU\\Software\\" + app_name + "\\" + k

            c = ["reg", "add", key_path, "/f"]
            logging.debug("running: %s", c)
            subprocess.run(c, check=True, shell=True)
            exists = key_exists(key_path)
            assert exists != install
            if exists:
                c = ["reg", "delete", key_path, "/f"]
                logging.debug("running: %s", c)
                subprocess.run(c, check=True, shell=True)


def test_proc_dynamic_deny(*, ef_handle: EaseFilter):
    """Test denying access to a key via callback."""
    deny_list = {"deny_1", "deny_👍🐸"}
    key_list = {"allow_1", "allow_👍🐸", *deny_list}

    app_name = "EasefilterPython"
    rule = RegistryProcNameRule(
        f"*\\{app_name}\\*",
        functools.reduce(operator.ior, RegCallbackClass),
        proc_name=Path(f"{os.environ['WINDIR']}\\system32\\reg.exe"),
    )

    def msg_hook(ev: BaseEvent) -> DenyReply | None:
        if not isinstance(ev, RegistryEvent):
            logging.warning("spurious event %s", ev)
            return None

        if ev.key_name.strip(":").split("\\")[-1] in deny_list:
            return DenyReply()

        return None

    with ef_handle as ef:
        ef.message_callback = msg_hook
        ef.set_filter_type(FilterType.REGISTRY)
        rule.install(ef)

        with (
            winreg.OpenKeyEx(winreg.HKEY_CURRENT_USER, "Software\\") as soft_key,
            winreg.CreateKey(soft_key, app_name + "\\"),
        ):
            for k in key_list:
                key_path = "HKCU\\Software\\" + app_name + "\\" + k

                c = ["reg", "add", key_path, "/f"]
                logging.debug("running: %s", c)
                subprocess.run(c, check=True, shell=True)

                exists = key_exists(key_path)
                assert exists == (k not in deny_list)
                if exists:
                    c = ["reg", "delete", key_path, "/f"]
                    logging.debug("running: %s", c)
                    subprocess.run(c, check=True, shell=True)
