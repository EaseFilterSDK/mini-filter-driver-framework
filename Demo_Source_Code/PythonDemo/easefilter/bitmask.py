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

"""Utilities for managing bitmasks."""

from __future__ import annotations

from enum import IntFlag
from functools import reduce
from typing import TYPE_CHECKING, TypeVar

if TYPE_CHECKING:
    from collections.abc import Iterator

T = TypeVar("T", bound=IntFlag)


def str_flag(strs: list[str] | set[str], flags: type[T]) -> T:
    """Convert a series of string symbols to a bitmask.

    Args:
        strs: List of string symbols to convert.
        flags: IntFlag type to convert to.
    """
    return reduce(lambda x, y: x | flags[y], strs, flags(0))


T = TypeVar("T", bound=IntFlag)


def flag_extract(flags: T) -> Iterator[T]:
    """Convert a bitmask integer to individual bitflags.

    Args:
        flags: Bitmask (IntFlag type.)
    """
    for flag in type(flags):
        if flag & flags == flag and flag:
            yield flag
