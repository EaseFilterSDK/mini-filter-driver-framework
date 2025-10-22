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

"""High-level rule interfaces."""

from __future__ import annotations

import warnings
from abc import ABC, abstractmethod
from dataclasses import dataclass, field
from pathlib import Path
from typing import TYPE_CHECKING

from typing_extensions import override

if TYPE_CHECKING:
    from easefilter import EaseFilter

from easefilter.enums import (
    AccessFlag,
    BooleanConfig,
    FileEventType,
    IOCallbackClass,
    ProcessControlFlag,
    RegCallbackClass,
    RegControlFlag,
)


@dataclass()
class FilterRule(ABC):
    """Interface for filter rules.

    Each type of rule has a different configuration interface in the C++ API,
    so each type of rule needs specific code to install it.

    This class should also include all configuration options for a given rule,
    so that `install` may apply them. Because of this, subclasses of this
    should also be dataclasses.
    """

    def install(self, ef: EaseFilter) -> None:
        """Apply this rule (i.e. send it to the driver).

        Args:
            ef: EaseFilter handle.
        """
        if not ef.has_set_type:
            warnings.warn(
                RuntimeWarning(
                    "Filter type is not set. Set with `ef.set_filter_type()`."
                ),
                stacklevel=3,
            )

    @abstractmethod
    def uninstall(self, ef: EaseFilter) -> None:
        """Remove this rule from the driver.

        Args:
            ef: EaseFilter handle.
        """

    _rule_id: int | None = field(init=False, repr=True, default=None)

    @property
    def rule_id(self) -> int | None:
        """Unique rule ID. Read only."""
        return self._rule_id


@dataclass
class _BaseFileRule(FilterRule):
    """Parent class for MONITOR/CONTROL/ENCRYPTION rules.

    Do not instantiate directly.
    """

    file_path: Path
    """File path to monitor (may be a glob, e.g. '*').

    This must be unique, otherwise it overwrites the older rule.
    """

    access_flag: AccessFlag = AccessFlag.ALLOW_MAX_RIGHT_ACCESS
    """Allow/disallow flags for file access (Control/encryption mode only)."""

    boolean_config: BooleanConfig = field(default=BooleanConfig(0))
    """Rule-specific configuration flags.

    This is in contrast to global configuration at `EaseFilter.boolean_config`.
    """

    def uninstall(self, ef: EaseFilter) -> None:
        ef.api.RemoveFilterRule(self.file_path)


@dataclass()
class FileRule(_BaseFileRule):
    """File rule, for either MONITOR or CONTROL mode.

    See `FilterRule` for more information.
    """

    change_event_filter: FileEventType = field(default=FileEventType(0))
    """List of file change events to monitor."""

    is_resident: bool = False
    """'Resident' CONTROL mode rules are always active, even at boot time."""

    monitor_io_filter: IOCallbackClass = IOCallbackClass.NONE
    """List of I/O events to monitor."""

    control_io_filter: IOCallbackClass = IOCallbackClass.NONE
    """List of I/O events to send to the control filter."""

    def install(self, ef: EaseFilter) -> None:
        super().install(ef)
        self._rule_id = ef.rule_id
        ef.handle_error(
            ef.api.AddFileFilterRule(
                self.access_flag,
                self.file_path,
                isResident=self.is_resident,
                ruleId=self._rule_id,
            ),
        )

        ef.api.AddBooleanConfigToFilterRule(self.file_path, self.boolean_config)

        ef.handle_error(
            ef.api.RegisterFileChangedEventsToFilterRule(
                self.file_path,
                self.change_event_filter,
            ),
        )
        ef.handle_error(
            ef.api.RegisterMonitorIOToFilterRule(
                self.file_path,
                self.monitor_io_filter,
            ),
        )
        ef.handle_error(
            ef.api.RegisterControlIOToFilterRule(
                self.file_path,
                self.control_io_filter,
            ),
        )


@dataclass
class ProcessRule(FilterRule):
    """Process rule. See `FilterRule` for more information."""

    executable_mask: Path
    """Path to an executable (may be a glob)."""

    control_flag: ProcessControlFlag
    """Sets process events to be notified for, and sets permissions."""

    def install(self, ef: EaseFilter) -> None:
        super().install(ef)
        self._rule_id = ef.rule_id
        ef.handle_error(
            ef.api.AddProcessFilterRule(
                len(str(self.executable_mask)) * 2,
                self.executable_mask,
                self.control_flag,
                self._rule_id,
            )
        )

    def uninstall(self, ef: EaseFilter) -> None:
        ef.handle_error(
            ef.api.RemoveProcessFilterRule(
                len(str(self.executable_mask)) * 2, self.executable_mask
            )
        )


@dataclass
class EncryptRule(_BaseFileRule):
    """Encryption rule. See `FilterRule` for more information."""

    encryption_key: bytes | None = None
    """Encryption key to use.

    If `None`, requires your callback code to provide a key every time a file is opened.
    In this case, you must enable `BooleanConfig.REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE`.

    Must be 16, 24, or 32 bytes in length.
    """

    access_flag: AccessFlag = (
        AccessFlag.ALLOW_MAX_RIGHT_ACCESS | AccessFlag.ENABLE_FILE_ENCRYPTION_RULE
    )
    """Allow/disallow flags for file access."""

    @override
    def install(self, ef: EaseFilter) -> None:
        super().install(ef)
        self._rule_id = ef.rule_id
        ef.handle_error(
            ef.api.AddFileFilterRule(
                self.access_flag,
                self.file_path,
                isResident=False,
                ruleId=self._rule_id,
            ),
        )

        ef.api.AddBooleanConfigToFilterRule(self.file_path, self.boolean_config)

        if self.encryption_key:
            if ln := len(self.encryption_key) not in (16, 24, 32):
                msg = f"Encryption key must be 16, 24, or 32 bytes (got {ln})."
                raise ValueError(msg)
            ef.handle_error(
                ef.api.AddEncryptionKeyToFilterRule(
                    self.file_path, len(self.encryption_key), self.encryption_key
                )
            )
        elif (
            BooleanConfig.REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE
            not in self.boolean_config
        ):
            msg = "BooleanConfig.REQUEST_ENCRYPT_KEY_IV_AND_TAGDATA_FROM_SERVICE is required when encryption_key is None."
            raise ValueError(msg)


@dataclass
class _BaseRegistryRule(FilterRule):
    """Registry rule base class."""

    key_mask: str
    """Registry key filter that this rule will apply to.

    May contain a wildcard (`*`).
    """

    callback_class: RegCallbackClass

    access_flag: RegControlFlag = field(default=RegControlFlag.REG_MAX_ACCESS_FLAG)
    """Permission flags."""

    exclude_filter: bool = field(default=False)
    """If true, excludes all events matching this rule from being filtered."""

    def install(self, ef: EaseFilter) -> None:
        super().install(ef)
        self._rule_id = ef.rule_id

        proc_name: Path | None = getattr(self, "proc_name", None) or Path("*")
        proc_id: int = getattr(self, "proc_id", None) or 0
        username: str | None = getattr(self, "username", None) or "*"

        ef.api.AddRegistryFilterRule(
            len(str(proc_name or "")) * 2,
            proc_name,
            proc_id,
            len(username or "") * 2,
            username or "",
            len(self.key_mask) * 2,
            self.key_mask,
            self.access_flag,
            self.callback_class,
            self.exclude_filter,
            self._rule_id,
        )


class NoDefault:
    """Sentinel value type for `NO_DEFAULT`."""

    def __bool__(self) -> bool:
        return False


NO_DEFAULT = NoDefault()
"""Sentinel to be used with `NoDefaultMixin`."""


class NoDefaultMixin:
    """Mixin to have non-default dataclass values in Python 3.9.

    See https://stackoverflow.com/a/54653255.
    """

    def __post_init__(self) -> None:
        for k, v in self.__dict__.items():
            if v is NO_DEFAULT:
                msg = f"missing required argument: '{k}'"
                raise TypeError(msg)


@dataclass
class RegistryProcNameRule(_BaseRegistryRule, NoDefaultMixin):
    """Registry rule that matches by process path."""

    proc_name: Path | NoDefault = NO_DEFAULT
    """Process name (may contain wildcard `*` for all processes.)"""

    def uninstall(self, ef: EaseFilter) -> None:
        if isinstance(self.proc_name, NoDefault):
            msg = "Sentinel value still exists."
            raise TypeError(msg)
        s = str(self.proc_name)
        ef.api.RemoveRegistryFilterRuleByProcessName(len(s) * 2, s)


@dataclass
class RegistryProcIdRule(_BaseRegistryRule, NoDefaultMixin):
    """Registry rule that matches by process ID."""

    proc_id: int | NoDefault = NO_DEFAULT

    def uninstall(self, ef: EaseFilter) -> None:
        if isinstance(self.proc_id, NoDefault):
            msg = "Sentinel value still exists."
            raise TypeError(msg)
        ef.api.RemoveRegistryFilterRuleByProcessId(self.proc_id)
