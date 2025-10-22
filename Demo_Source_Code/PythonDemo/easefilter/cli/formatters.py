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

"""Basic functions that format filter events."""

import textwrap

from easefilter.enums import FileEventType, RegCallbackClass
from easefilter.events import EncryptEvent, FileEvent, ProcessEvent, RegistryEvent


def str_join(strs: list[str]) -> str:
    return "\n" + "".join(textwrap.dedent(s).lstrip() for s in strs).strip("\n") + "\n"


def fmt_file_event(ev: FileEvent) -> str:
    return str_join(
        [
            f"""
        I/O type:           {ev.io_event_type!r}
        User:               {ev.account.username}\\{ev.account.domain}
        File name:          {ev.file_name}
        Process name:       {ev.process.path}
        Process ID:         {ev.process.pid}
        Thread ID:          {ev.process.tid}
        """,
            f"""
        Information class:  {ev.info_class}
        """
            if ev.info_class
            else "",
            f"""
        Filter message ID:  {ev.message_id}
        """,
            f"""
        ---
        Renamed to '{ev.new_path}'.
        """
            if isinstance(ev.io_event_type, FileEventType)
            and FileEventType.RENAMED in ev.io_event_type
            else "",
            f"""
        ---
        Copied from '{ev.new_path}'.
        """
            if isinstance(ev.io_event_type, FileEventType)
            and FileEventType.COPIED in ev.io_event_type
            else "",
        ]
    )


def fmt_process_event(ev: ProcessEvent) -> str:
    return str_join(
        [
            f"""
        I/O type:           {ev.io_event_type!r}
        User:               {ev.account.username}\\{ev.account.domain}
        Executable path:    {ev.process.path}
        Process ID:         {ev.process.pid}
        Thread ID:          {ev.process.tid}
        """,
            f"""
        Parent name:        {ev.parent_proc.path}
        Parent PID:         {ev.parent_proc.pid}
        """
            if ev.parent_proc is not None
            else "",
            f"""
        Creator name:       {ev.creating_proc.path}
        Creator PID:        {ev.creating_proc.pid}
        """
            if ev.creating_proc is not None
            else "",
            f"""
        Command line:       {ev.command_line}
        """
            if ev.command_line is not None
            else "",
            f"""
        Filter message ID:  {ev.message_id}
        """,
        ]
    )


def fmt_encrypt_event(ev: EncryptEvent) -> str:
    TRUNCATE_LEN = 32
    """Truncate tag data to this many bytes."""

    return str_join(
        [
            f"""
        I/O type:           {ev.io_event_type!r}
        User:               {ev.account.username}\\{ev.account.domain}
        File name:          {ev.file_name}
        """,
            f"""
        Tag data:           {ev.tag_data if len(ev.tag_data) <= TRUNCATE_LEN else ev.tag_data[:TRUNCATE_LEN]}
        """
            if ev.tag_data
            else "",
            f"""
        Process name:       {ev.process.path}
        Process ID:         {ev.process.pid}
        Thread ID:          {ev.process.tid}
        Filter message ID:  {ev.message_id}
        """,
        ]
    )


def fmt_registry_event(ev: RegistryEvent) -> str:
    return str_join(
        [
            f"""
        I/O type:           {ev.io_event_type!r}
        User:               {ev.account.username}\\{ev.account.domain}
        Key name:           {ev.key_name}
        Process name:       {ev.process.path}
        Process ID:         {ev.process.pid}
        Thread ID:          {ev.process.tid}
        Filter message ID:  {ev.message_id}
        """,
            f"""
        ---
        Renamed to '{ev.new_path}'.
        """
            if ev.io_event_type
            in (
                RegCallbackClass.Reg_Pre_Rename_Key,
                RegCallbackClass.Reg_Post_Rename_Key,
            )
            else "",
        ]
    )
