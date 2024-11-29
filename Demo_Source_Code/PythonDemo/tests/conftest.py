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
##    demonstration purposes only,this software is provided on an
##    "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
##     either express or implied.
##
###############################################################################

import contextlib
import ctypes
import subprocess
from typing import TYPE_CHECKING

import pytest

import easefilter
from easefilter.config import get_config

if TYPE_CHECKING:
    from pathlib import Path

with contextlib.suppress(ModuleNotFoundError):
    pass


@pytest.fixture
def check_installed():
    """Check that EaseFilter is loaded and installed."""

    def _check_installed():
        sc_proc = subprocess.run(
            ["C:\\Windows\\System32\\sc", "query", "EaseFilter"],
            capture_output=True,
            check=False,
        )
        if sc_proc.returncode != 0:
            return False

        fltmc_proc = subprocess.run(
            ["C:\\Windows\\System32\\fltmc"],
            capture_output=True,
            text=True,
            check=False,
        )
        return any(word == "EaseFilter" for word in fltmc_proc.stdout.split())

    return _check_installed


@pytest.fixture
def win_sid():
    """The Windows Security Identifier for the current user."""
    return (
        subprocess.check_output("C:\\Windows\\System32\\whoami /User")
        .decode()
        .splitlines()[-1]
        .split()[-1]
    )


def _ef_handle(*, activate: bool = True):
    config = get_config()

    license_key = config.get("license_key") if activate else None

    if not ctypes.windll.shell32.IsUserAnAdmin():  # pragma: no cover
        pytest.fail("Please run the tests with administrator privileges.")
    return easefilter.EaseFilter(license_key=license_key)


@pytest.fixture
def ef_handle_unauth():
    """Get an EaseFilter handle, without activating with the license key."""
    return _ef_handle(activate=False)


@pytest.fixture(scope="session")
def ef_handle():
    """Get an EaseFilter handle, activated with the license key."""
    return _ef_handle()


@pytest.fixture
def _clean_driver():
    """Ensure EaseFilter is unloaded and uninstalled."""
    subprocess.run(
        ["C:\\Windows\\System32\\fltmc", "unload", "EaseFilter"],
        check=False,
    )
    subprocess.run(["C:\\Windows\\System32\\sc", "stop", "EaseFilter"], check=False)
    subprocess.run(["C:\\Windows\\System32\\sc", "delete", "EaseFilter"], check=False)


@pytest.fixture
def test_dir(tmp_path_factory):
    """Create a temp directory for testing."""
    test_dir: Path = tmp_path_factory.mktemp("easefilter_pytest")
    return test_dir
