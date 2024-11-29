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

from ._ef_class import _EaseFilter

# this class is a shim over _EaseFilter, because Pyright does not display
# re-exported names in autocomplete


class EaseFilter(_EaseFilter):
    r"""High-level filter control handle.

    Example usage of this class::

        from pathlib import Path

        from easefilter import EaseFilter
        from easefilter.enums import FileEventType, FilterType
        from easefilter.events import FileEvent
        from easefilter.rules import FileRule
        from easefilter.types import BaseEvent, BaseReply


        def msg_hook(event_data: BaseEvent) -> BaseReply | None:
            if not isinstance(event_data, FileEvent):
                # only deal with file events here
                return

            print(event_data)


        ef_handle = EaseFilter(license_key="...")
        with ef_handle as ef:
            ef.set_filter_type(FilterType.MONITOR)
            ef.message_callback = msg_hook

            # monitor file creations
            rule = FileRule(file_path=Path("*"), change_event_filter=FileEventType.CREATED)
            rule.install(ef)
    """


__all__ = ["EaseFilter"]
