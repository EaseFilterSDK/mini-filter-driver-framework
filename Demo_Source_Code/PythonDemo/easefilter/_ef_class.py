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

"""Main high-level, Pythonic, EaseFilter class implementation."""

from __future__ import annotations

import logging
from typing import TYPE_CHECKING

from typing_extensions import Self

from easefilter.enums import BooleanConfig
from easefilter.errors import FilterError
from easefilter.events import handle_msg, handle_msg_reply
from easefilter.filter_api import (
    MESSAGE_SEND_DATA,
    FilterApi,
)

if TYPE_CHECKING:
    from collections.abc import Callable
    from pathlib import Path

    from easefilter.enums import (
        FilterType,
    )
    from easefilter.types import (
        DisconCallback,
        MsgCallback,
    )


class _EaseFilter:
    """Main class implementation.

    See `__init__.py` for the public docstring.
    """

    def __init__(
        self,
        dll_path: Path | None = None,
        license_key: str | None = None,
        *,
        reset_on_start: bool = True,
    ) -> None:
        r"""Inits the low-level API handle.

        Args:
            license_key: Activation licence key.
            dll_path: Absolute path or relative path prefixed with .\ to the Filter API DLL.
            reset_on_start: Automatically reset configuration on filter start.
        """
        self.api = FilterApi(dll_path=dll_path)
        self.__reset_on_start: bool = reset_on_start
        self.__license_key = license_key

        self.custom_msg_cb: Callable | None = None
        """Custom low-level handler, used instead of the Pythonic `message_callback`.

        Using this handler is almost exactly like writing C++ callback code::

            def handler(send_data_p, reply_data_p):
                # raw struct
                send_data: MESSAGE_SEND_DATA = send_data_p.contents

                # do stuff with the data...

                # modify the reply data struct...

                # the `[0]` is a pointer dereference
                reply_data_p[0].ReturnStatus = FileStatus.SUCCESS

                return 1

            ef_handle = EaseFilter(license_key="...")
            with ef_handle as ef:
                ef.custom_msg_cb = handler
                ...  # further setup code...
        """

    def handle_error(self, ret: int) -> None:
        """Handle the Filter API's error conditions if needed.

        Args:
            ret: Return code of a FilterAPI function.

        Raises:
            FilterError: The return code signals an error.
        """
        if ret == 0:
            err = self.api.GetLastErrorMessage()
            raise FilterError(err)

    def uninstall_driver(self) -> None:
        """Uninstalls the EaseFilter driver."""
        ret = self.api.UnInstallDriver()
        self.handle_error(ret)
        logging.debug("Uninstalled driver.")

    def install_driver(self) -> None:
        """Installs the EaseFilter driver."""
        if not self.api.IsDriverServiceRunning():
            logging.debug("Driver is not installed.")
            ret = self.api.InstallDriver()
            self.handle_error(ret)
            logging.debug("Installed driver.")
        else:
            logging.debug("Driver is installed.")

    __filter_started = False

    @property
    def filter_started(self) -> bool:
        return self.__filter_started

    def start_filter(self, thread_count: int = 20) -> None:
        """Start the filter, and register callbacks.

        Args:
            thread_count: threads to use for callbacks.
        """

        def message_callback(send_data_p, reply_data_p) -> int:  # noqa: ANN001
            # raw struct
            send_data: MESSAGE_SEND_DATA = send_data_p.contents

            logging.debug("Message callback called. Processing...")

            msg_data = handle_msg(send_data, can_reply=bool(reply_data_p))

            logging.debug("Message callback data:\n%s", msg_data)

            if self.message_callback is None:
                self._default_msg_cb()
                return 1

            ret = self.message_callback(msg_data)

            handle_msg_reply(ret, reply_data_p)

            return 1

        def disconnect_callback() -> None:  # no cov
            logging.debug("Disconnect callback called.")
            if self.discon_callback is not None:
                self.discon_callback()

        if self.filter_started:
            logging.debug("Filter was already started; not starting again.")
            return

        logging.info("Starting filter.")

        if not self.api.IsDriverServiceRunning():
            self.install_driver()

        self.handle_error(self.api.SetRegistrationKey(self.__license_key or ""))

        if self.__reset_on_start:
            self.reset_config()

        self.handle_error(
            self.api.RegisterMessageCallback(
                thread_count,
                self.custom_msg_cb or message_callback,
                disconnect_callback,
            ),
        )

        self.__filter_started = True

    def stop_filter(self) -> None:
        """Stop the filter."""
        if not self.filter_started:
            logging.debug("Filter was not started; not stopping.")
            return
        logging.debug("Calling disconnect on service.")
        self.api.Disconnect()
        self.__filter_started = False

    def __enter__(self) -> Self:
        self.start_filter()
        return self

    def __exit__(self, e_type, value, traceback) -> None:  # noqa: ANN001
        self.stop_filter()
        logging.debug(
            "Leaving context manager. type: %s, value: %s, traceback: %s",
            e_type,
            value,
            traceback,
        )

    warned_msg_cb: bool = False

    def _default_msg_cb(self) -> None:  # no cov
        if not self.warned_msg_cb:
            self.warned_msg_cb = True
            logging.warning(
                "message_callback was fired, but no handler function was assigned.",
            )

    message_callback: MsgCallback | None = None
    """Handler for filter events (e.g. I/O). This may return a `BaseReply` subclass to the filter."""

    discon_callback: DisconCallback | None = None
    """Hook to be run when EaseFilter is unloaded."""

    _rule_id: int = 0

    @property
    def rule_id(self) -> int:
        """Current filter rule ID. Automatically increments on read; keep a copy of it."""
        ret = self._rule_id
        self._rule_id += 1
        return ret

    __has_set_type: bool = False

    @property
    def has_set_type(self) -> bool:
        """Is filter type set yet?"""
        return self.__has_set_type

    def set_filter_type(self, filter_type: FilterType) -> None:
        """Set the filter types active in the driver.

        Args:
            filter_type: Filter types bitmask. See `easefilter.enums.FilterType`.
        """
        self.__has_set_type = True
        self.handle_error(self.api.SetFilterType(filter_type))

    __boolean_config: BooleanConfig = BooleanConfig(0)

    @property
    def boolean_config(self) -> BooleanConfig:
        """Set global configuration flags.

        This is in contrast to rule-specific flags, that can be set in the rule class.
        See `easefilter.enums.BooleanConfig` for more information.
        """
        return self.__boolean_config

    @boolean_config.setter
    def boolean_config(self, config: BooleanConfig) -> None:
        self.handle_error(self.api.SetBooleanConfig(config))
        self.__boolean_config = config

    def reset_config(self) -> None:
        """Reset all configuration data in the filter driver."""
        self.__has_set_type = False
        self.handle_error(self.api.ResetConfigData())
