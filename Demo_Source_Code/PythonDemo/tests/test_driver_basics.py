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

"""Test basic driver installation/activation, start/stop features."""

import subprocess
from pathlib import Path
from threading import Event

import pytest

from easefilter import EaseFilter
from easefilter.errors import FilterError
from easefilter.rules import FileRule


@pytest.mark.usefixtures("_clean_driver")
def test_install(ef_handle_unauth, check_installed):
    """Test driver installation."""
    ef_handle_unauth.install_driver()
    assert check_installed()

    # Ensure installing twice at once does not error
    ef_handle_unauth.install_driver()


def test_uninstall(ef_handle_unauth, check_installed):
    """Test driver uninstallation."""
    ef_handle_unauth.install_driver()
    ef_handle_unauth.uninstall_driver()
    assert not check_installed()


def test_license_check(ef_handle):
    """Test that the license key works."""
    with ef_handle as _:
        pass


def test_license_fail_check(ef_handle_unauth):
    """Test that an absent license key does not work."""
    with pytest.raises(FilterError), ef_handle_unauth as _:
        pass


def test_discon_callback(ef_handle: EaseFilter):
    """Test that disconnecting the driver fires the disconnect hook."""
    disconnected = Event()

    def discon_hook():
        nonlocal disconnected
        disconnected.set()

    with ef_handle as ef:
        ef.discon_callback = discon_hook

    proc = subprocess.run(
        ["C:\\Windows\\System32\\fltmc", "unload", "easefilter"],
        check=True,
    )
    assert proc.returncode == 0

    # disconnect *should* fire when the with block is exited but it doesn't so whatever

    assert disconnected.wait(timeout=0.5)


def test_filter_type_warning(ef_handle: EaseFilter):
    """Test that the filter will warn if a rule is applied without setting filter type."""
    with ef_handle as ef:
        ef.message_callback = lambda _: None
        rule = FileRule(Path(__file__))
        with pytest.warns(RuntimeWarning, match="Filter type"):
            rule.install(ef)
