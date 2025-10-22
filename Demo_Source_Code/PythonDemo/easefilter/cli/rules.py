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

"""Rule definitions for the CLI demo."""

import hashlib
import logging
import operator
from argparse import Namespace
from functools import reduce
from getpass import getpass
from pathlib import Path

from easefilter.bitmask import str_flag
from easefilter.enums import (
    AccessFlag,
    FileEventType,
    IOCallbackClass,
    ProcessControlFlag,
    RegCallbackClass,
    RegControlFlag,
)
from easefilter.rules import EncryptRule, FileRule, ProcessRule, RegistryProcNameRule


def monitor_rule(args: Namespace, filter_mask: Path) -> FileRule:
    change_event_filter: FileEventType = args.change_mask or reduce(
        operator.ior, FileEventType
    )
    if args.change_sym:
        change_event_filter = str_flag(args.change_sym.split(","), FileEventType)

    monitor_io_filter: IOCallbackClass = IOCallbackClass.NONE
    if args.io_mask:
        monitor_io_filter = IOCallbackClass(args.io_mask)
    if args.io_sym:
        monitor_io_filter = str_flag(args.io_sym.split(","), IOCallbackClass)

    logging.info("Monitoring '%s'.", filter_mask)

    return FileRule(
        filter_mask,
        change_event_filter=change_event_filter,
        monitor_io_filter=monitor_io_filter,
    )


def control_rule(args: Namespace, filter_mask: Path) -> FileRule:
    access_flag: AccessFlag = AccessFlag.ALLOW_MAX_RIGHT_ACCESS
    if args.deny_mask:
        access_flag &= ~AccessFlag(args.deny_mask)
    elif args.deny_sym:
        access_flag &= ~str_flag(args.deny_sym.split(","), AccessFlag)

    control_io_filter: IOCallbackClass = IOCallbackClass.NONE
    if args.io_mask:
        control_io_filter = IOCallbackClass(args.io_mask)
    if args.io_sym:
        control_io_filter = str_flag(args.io_sym.split(","), IOCallbackClass)

    logging.info("Controlling '%s'.", filter_mask)

    return FileRule(
        filter_mask,
        control_io_filter=control_io_filter,
        access_flag=access_flag,
    )


def process_rule(args: Namespace) -> ProcessRule:
    control_flag: ProcessControlFlag = (
        ProcessControlFlag.PROCESS_CREATION_NOTIFICATION
        | ProcessControlFlag.PROCESS_TERMINATION_NOTIFICATION
    )
    if args.proc_control_mask:
        control_flag = ProcessControlFlag(args.proc_control_mask)
    elif args.proc_control_sym:
        control_flag = str_flag(args.proc_control_sym.split(","), ProcessControlFlag)

    return ProcessRule(args.proc_mask or Path("*"), control_flag=control_flag)


def encryption_rule(args: Namespace, filter_mask: Path) -> EncryptRule:
    salt: bytes = b"use-a-random-salt-in-production"
    password = args.password or getpass("Enter encryption password: ")
    key: bytes = hashlib.scrypt(
        password.encode(), salt=salt, n=16384, r=8, p=1, dklen=32
    )

    logging.info("Encrypting '%s'.", filter_mask)

    return EncryptRule(filter_mask, encryption_key=key)


def registry_rule(args: Namespace) -> RegistryProcNameRule:
    reg_mask: RegCallbackClass = reduce(operator.ior, RegCallbackClass)
    if args.reg_class_mask:
        reg_mask = RegCallbackClass(args.reg_class_mask)
    elif args.reg_class_sym:
        reg_mask = str_flag(args.reg_class_sym.split(","), RegCallbackClass)

    deny_mask = RegControlFlag(0)
    if args.reg_deny_mask:
        deny_mask = RegControlFlag(args.reg_deny_mask)
    if args.reg_deny_sym:
        deny_mask = str_flag(args.reg_deny_sym.split(","), RegControlFlag)

    logging.info("Monitoring Registry key '%s'.", args.keymask)

    return RegistryProcNameRule(
        key_mask=args.keymask,
        callback_class=reg_mask,
        access_flag=RegControlFlag.REG_MAX_ACCESS_FLAG & ~deny_mask,
        proc_name=args.proc_mask or Path("*"),
    )
