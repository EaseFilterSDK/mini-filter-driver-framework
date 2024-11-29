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

"""Test encryption filter features."""

from __future__ import annotations

import logging
from threading import Event
from typing import TYPE_CHECKING

import pytest

from easefilter.enums import AccessFlag, BooleanConfig, EncryptEventType, FilterType
from easefilter.events import DenyReply, EncryptEvent, EncryptReply
from easefilter.rules import EncryptRule

if TYPE_CHECKING:
    from pathlib import Path

    from easefilter import EaseFilter
    from easefilter.types import BaseEvent


def test_encryption(
    *,
    ef_handle: EaseFilter,
    test_dir: Path,
):
    """Test encryption with pre-set key (i.e. no callback)."""
    test_file = test_dir / "test_file"
    with ef_handle as ef:
        ef.set_filter_type(FilterType.ENCRYPTION)

        test_text = b"lorem ipsum dolor sit amet"

        encryption_key = b"0123456789abcdef"

        rule = EncryptRule(
            file_path=test_file,
            encryption_key=encryption_key,
        )
        rule.install(ef)
        test_file.write_bytes(test_text)
        assert test_file.read_bytes() == test_text
        rule.uninstall(ef)
        encrypted: bytes = test_file.read_bytes()
        assert encrypted != test_text

        rule2 = EncryptRule(
            file_path=test_file,
            encryption_key=encryption_key,
            access_flag=AccessFlag.ALLOW_MAX_RIGHT_ACCESS
            & ~(AccessFlag.ALLOW_READ_ENCRYPTED_FILES),
        )
        rule2.install(ef)
        assert test_file.read_bytes() == encrypted
        rule2.uninstall(ef)
        assert test_file.read_bytes() == encrypted


def test_callback_encryption(
    *,
    ef_handle: EaseFilter,
    test_dir: Path,
):
    """Test dynamic encryption (i.e. with callback)."""
    enc_folder = test_dir / "encrypted"
    enc_folder.mkdir()
    test_file1 = enc_folder / "test_file1"
    test_file2 = enc_folder / "test_file2"

    enc_key: bytes = b"1234567890abcdef" * 2
    iv = b"1234567890abcdef"

    def msg_hook(msg: BaseEvent) -> EncryptReply | DenyReply | None:
        """Deny processes from decrypting one file."""
        if not isinstance(msg, EncryptEvent):
            logging.warning("spurious event %s", msg)
            return None

        if msg.file_name == test_file2:
            return DenyReply()

        return EncryptReply(encryption_key=enc_key, iv=iv)

    test_data: bytes = b"lorem ipsum dolor sit amet"

    with ef_handle as ef:
        ef.message_callback = msg_hook
        ef.set_filter_type(FilterType.ENCRYPTION)

        rule = EncryptRule(
            file_path=enc_folder / "*",
            encryption_key=None,
            boolean_config=BooleanConfig.REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE,
        )
        rule.install(ef)

        test_file1.write_bytes(test_data)
        assert test_file1.read_bytes() == test_data

        with pytest.raises(OSError, match="Permission denied"):
            test_file2.write_bytes(test_data)


def test_tag_data(*, ef_handle: EaseFilter, test_dir: Path):
    """Test storing tag data in files."""
    enc_folder = test_dir / "encrypted"
    enc_folder.mkdir()

    enc_key: bytes = b"1234567890abcdef"

    found = Event()
    """Tag data was found."""

    bad = Event()
    """Tag data found was incorrect."""

    def msg_hook(msg: BaseEvent) -> EncryptReply | None:
        """Tag files with their file path."""
        if not isinstance(msg, EncryptEvent):
            logging.warning("spurious event %s", msg)
            return None

        if msg.io_event_type == EncryptEventType.RequestIvAndKey and msg.tag_data:
            found.set()
            if msg.tag_data.decode() != str(msg.file_name):
                bad.set()

        return EncryptReply(
            encryption_key=enc_key, tag_data=str(msg.file_name).encode()
        )

    with ef_handle as ef:
        ef.message_callback = msg_hook
        ef.set_filter_type(FilterType.ENCRYPTION)
        rule = EncryptRule(
            file_path=test_dir / "*",
            boolean_config=BooleanConfig.REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE,
        )
        rule.install(ef)

        name = "file.txt"
        f = test_dir / name
        test_text = "lorem ipsum"
        f.write_text(test_text)
        assert f.read_text() == test_text

    assert found.wait(timeout=0.2)
    assert not bad.wait(timeout=0.2)
