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

"""Demo CLI interface for the library.

This is a non-exhaustive overview of EaseFilter's features.
"""

from __future__ import annotations

import argparse
import ctypes
import logging
from argparse import Namespace
from pathlib import Path
from typing import TYPE_CHECKING

import easefilter
from easefilter import errors
from easefilter.cli.formatters import (
    fmt_encrypt_event,
    fmt_file_event,
    fmt_process_event,
    fmt_registry_event,
)
from easefilter.cli.rules import (
    control_rule,
    encryption_rule,
    monitor_rule,
    process_rule,
    registry_rule,
)
from easefilter.config import get_config
from easefilter.enums import (
    FilterType,
)
from easefilter.events import EncryptEvent, FileEvent, ProcessEvent, RegistryEvent

if TYPE_CHECKING:
    from easefilter.rules import (
        FilterRule,
    )
    from easefilter.types import BaseEvent, BaseReply

################################
################################
## main demo implementation
################################
################################


################################
## event handler
################################


def msg_handler(ev: BaseEvent) -> BaseReply | None:
    outp: str = ""
    ev_map = {
        FileEvent: fmt_file_event,
        ProcessEvent: fmt_process_event,
        EncryptEvent: fmt_encrypt_event,
        RegistryEvent: fmt_registry_event,
    }

    for (
        ev_type,
        fmt_func,
    ) in ev_map.items():
        if isinstance(ev, ev_type):
            outp = fmt_func(ev)
            break
    else:
        logging.error("Could not format event: %s", ev)

    print(outp, end="")


################################
## main function
################################


def main(args: Namespace) -> None:
    config = get_config(args.config)

    if args.subcommand in ("install", "uninstall"):
        ef = easefilter.EaseFilter(dll_path=args.dll_path)
        f_map = {"install": ef.install_driver, "uninstall": ef.uninstall_driver}
        f_map[args.subcommand]()

    with easefilter.EaseFilter(
        dll_path=args.dll_path,
        license_key=config.get("license_key"),
        reset_on_start=True,
    ) as ef:
        ef.install_driver()
        ef.message_callback = msg_handler

        filt_type = FilterType[args.subcommand.upper()]
        ef.set_filter_type(filt_type)

        test_dir: Path = Path("C:\\easefilter_demo")

        rule: FilterRule | None = None

        rule_map = {
            FilterType.MONITOR: monitor_rule,
            FilterType.CONTROL: control_rule,
            FilterType.ENCRYPTION: encryption_rule,
            FilterType.PROCESS: process_rule,
            FilterType.REGISTRY: registry_rule,
        }

        if filt_type in (
            FilterType.MONITOR | FilterType.CONTROL | FilterType.ENCRYPTION
        ):
            filter_mask = args.pathmask or test_dir / "*"
            rule = rule_map[filt_type](args, filter_mask)
        else:
            rule = rule_map[filt_type](args)

        if rule is None:
            msg = "`rule` is `None`."
            raise RuntimeError(msg)

        # Defer CONTROL rule installation to after we create the test directory,
        # in case writing is denied.
        if filt_type != FilterType.CONTROL:
            rule.install(ef)

        if filt_type in (
            FilterType.MONITOR | FilterType.CONTROL | FilterType.ENCRYPTION
        ):
            test_dir_exists = test_dir.exists()
            if not test_dir_exists:
                if input(f"Create test directory at {test_dir!s}? (Y/N): ").lower() in (
                    "yes",
                    "y",
                ):
                    logging.info("Creating test directory...")
                    test_dir.mkdir(exist_ok=True)
                    test_dir_exists = True
                else:
                    logging.info("Test directory was not created.")
            if test_dir_exists:
                readme = test_dir / "README.txt"
                readme.write_text(
                    "\n\n".join(
                        (
                            "This is a testing directory for the EaseFilter Python demo.",
                            "Try moving or editing this file."
                            if filt_type == FilterType.MONITOR
                            else "",
                        )
                    )
                )
            else:
                logging.info("Test directory was not created.")

        if filt_type == FilterType.CONTROL:
            rule.install(ef)

        logging.warning("Filter started. Press ENTER to stop the filter.")
        input()


################################
################################
## CLI argument parser code
################################
################################

################################
## general params
################################

parser = argparse.ArgumentParser(
    prog="easefilter-cli",
    description="A demo of EaseFilter's features.",
    epilog="For any questions, including obtaining a licence key, contact info@easefilter.com.",
)

parser.add_argument(
    "--log",
    choices=["DEBUG", "INFO", "WARNING", "ERROR"],
    default="INFO",
    help="Sets log level.",
)

parser.add_argument(
    "--dll-path",
    help="Override the default EaseFilter DLL path.",
)

parser.add_argument(
    "--config",
    type=Path,
    help="Override the default `config.yml` location.",
    default=None,
)

subparsers = parser.add_subparsers(
    title="Subcommands", dest="subcommand", required=True
)

################################
## install/uninstall
################################

parser_install = subparsers.add_parser(
    "install", help="Installs the EaseFilter system driver."
)
parser_uninstall = subparsers.add_parser(
    "uninstall", help="Uninstalls the EaseFilter system driver."
)

################################
## monitor filter
################################

parser_monitor = subparsers.add_parser("monitor", help="Monitor filter demo.")
mon_change_event = parser_monitor.add_mutually_exclusive_group()
mon_change_event.add_argument(
    "--change-sym",
    help="Comma-separated string symbols of `FileEventType`s to monitor.",
    type=str,
)
mon_change_event.add_argument(
    "--change-mask",
    type=int,
    help="Integer (decimal form) bitmask of `FileEventType`s to monitor.",
)
mon_io_event = parser_monitor.add_mutually_exclusive_group()
mon_io_event.add_argument(
    "--io-sym",
    help="Comma-separated string symbols of `IOCallbackClass` granular events to monitor.",
    type=str,
)
mon_io_event.add_argument(
    "--io-mask",
    type=int,
    help="Integer (decimal form) bitmask of `IOCallbackClass` granular events to monitor.",
)
parser_monitor.add_argument(
    "pathmask",
    type=Path,
    help="Path to monitor. This may contain `*` wildcards.",
    default=None,
    nargs="?",
)

################################
## control filter
################################

parser_control = subparsers.add_parser("control", help="Control filter demo.")
con_deny = parser_control.add_mutually_exclusive_group()
con_deny.add_argument(
    "--deny-sym",
    help="Comma-separated string symbols of `AccessFlag` events to deny.",
    type=str,
)
con_deny.add_argument(
    "--deny-mask",
    type=int,
    help="Integer (decimal form) bitmask of `AccessFlag` events to deny.",
)
con_io_event = parser_control.add_mutually_exclusive_group()
con_io_event.add_argument(
    "--io-sym",
    help="Comma-separated string symbols of `IOCallbackClass` granular events to monitor.",
    type=str,
)
con_io_event.add_argument(
    "--io-mask",
    type=int,
    help="Integer (decimal form) bitmask of `IOCallbackClass` granular events to monitor.",
)
parser_control.add_argument(
    "pathmask",
    type=Path,
    help="Path to control. This may contain `*` wildcards.",
    default=None,
    nargs="?",
)

################################
## encryption filter
################################

parser_encryption = subparsers.add_parser("encryption", help="Encryption filter demo.")
parser_encryption.add_argument(
    "--password",
    help="Password to use for encryption.\n\nPassing passwords via command-line argument is insecure.\nIf this argument is not used, you will be instead prompted for a password.",
    type=str,
)
parser_encryption.add_argument(
    "pathmask",
    type=Path,
    help="Path to encrypt. This can be a directory followed by `*`.",
    default=None,
    nargs="?",
)

################################
## process filter
################################

parser_process = subparsers.add_parser("process", help="Process filter demo.")
parser_process.add_argument(
    "--proc-control-sym",
    help="Comma-separated string symbols of `ProcessControlFlag` events to register / control flags. WARNING: Be careful setting DENY_NEW_PROCESS_CREATION.",
    type=str,
)
parser_process.add_argument(
    "--proc-control-mask",
    type=int,
    help="Integer (decimal form) bitmask of `ProcessControlFlag` events to register / control flags. WARNING: Be careful setting 0x1 (deny process creation).",
)
parser_process.add_argument(
    "--proc-mask",
    help="Executable paths to monitor (or directory with `*` at the end.)",
    type=Path,
)

################################
## registry filter
################################

parser_registry = subparsers.add_parser(
    "registry", help="Windows Registry filter demo."
)
parser_registry.add_argument(
    "keymask",
    help="Registry key to monitor, may include wildcards (for example `*KeyName*`).",
    type=str,
    default="*",
    nargs="?",
)
parser_registry.add_argument(
    "--reg-class-sym",
    help="Comma-separated string symbols of `RegCallbackClass` events to monitor.",
    type=str,
)
parser_registry.add_argument(
    "--reg-class-mask",
    help="Integer (decimal form) bitmask of `RegCallbackClass` events to monitor.",
    type=int,
)
parser_registry.add_argument(
    "--reg-deny-sym",
    help="Comma-separated string symbols of `RegControlFlag` events to deny. WARNING: Be careful with this.",
    type=str,
)
parser_registry.add_argument(
    "--reg-deny-mask",
    help="Integer (decimal form) bitmask of `RegControlFlag` events to deny. WARNING: Be careful with this.",
    type=int,
)
parser_registry.add_argument(
    "--proc-mask",
    help="Monitor a specific executable path's registry events. Accepts `*` wildcards.",
    type=Path,
)


def entrypoint() -> None:
    args = parser.parse_args()

    if not ctypes.windll.shell32.IsUserAnAdmin():
        raise errors.PrivilegeError

    logging.basicConfig(level=getattr(logging, str.upper(args.log)))
    logging.info("Log level is %s.", args.log)

    main(args)


if __name__ == "__main__":
    entrypoint()
