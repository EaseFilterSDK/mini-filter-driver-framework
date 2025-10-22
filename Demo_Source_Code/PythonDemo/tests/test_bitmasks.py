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

import random
from platform import python_version_tuple

import easefilter.bitmask
import easefilter.enums
from easefilter.enums import AutoEnum, IOCallbackClass


def test_flag_extract() -> None:
    """Test pre-Python 3.11 shim on __iter__."""
    rand = random.Random(0)  # noqa: S311 (PRNG not suitable for crypto)
    for _ in range(2500):
        num = rand.randint(0, (easefilter.enums.IOCallbackClass.POST_CLOSE << 1) - 1)
        assert set(easefilter.enums.IOCallbackClass(num)) == set(
            easefilter.bitmask.flag_extract(easefilter.enums.IOCallbackClass(num))
        )

    s = set(easefilter.enums.IOCallbackClass)
    # behaviour changed in 3.11. zero will not appear
    if [int(i) for i in python_version_tuple()] < [3, 11, 0]:
        s.remove(IOCallbackClass(0))

    assert s == {
        IOCallbackClass.PRE_CREATE,
        IOCallbackClass.POST_CREATE,
        IOCallbackClass.PRE_NEW_FILE_CREATED,
        IOCallbackClass.POST_NEW_FILE_CREATED,
        IOCallbackClass.PRE_FASTIO_READ,
        IOCallbackClass.POST_FASTIO_READ,
        IOCallbackClass.PRE_CACHE_READ,
        IOCallbackClass.POST_CACHE_READ,
        IOCallbackClass.PRE_NOCACHE_READ,
        IOCallbackClass.POST_NOCACHE_READ,
        IOCallbackClass.PRE_PAGING_IO_READ,
        IOCallbackClass.POST_PAGING_IO_READ,
        IOCallbackClass.PRE_FASTIO_WRITE,
        IOCallbackClass.POST_FASTIO_WRITE,
        IOCallbackClass.PRE_CACHE_WRITE,
        IOCallbackClass.POST_CACHE_WRITE,
        IOCallbackClass.PRE_NOCACHE_WRITE,
        IOCallbackClass.POST_NOCACHE_WRITE,
        IOCallbackClass.PRE_PAGING_IO_WRITE,
        IOCallbackClass.POST_PAGING_IO_WRITE,
        IOCallbackClass.PRE_QUERY_INFORMATION,
        IOCallbackClass.POST_QUERY_INFORMATION,
        IOCallbackClass.PRE_QUERY_FILE_SIZE,
        IOCallbackClass.POST_QUERY_FILE_SIZE,
        IOCallbackClass.PRE_QUERY_FILE_BASIC_INFO,
        IOCallbackClass.POST_QUERY_FILE_BASIC_INFO,
        IOCallbackClass.PRE_QUERY_FILE_STANDARD_INFO,
        IOCallbackClass.POST_QUERY_FILE_STANDARD_INFO,
        IOCallbackClass.PRE_QUERY_FILE_NETWORK_INFO,
        IOCallbackClass.POST_QUERY_FILE_NETWORK_INFO,
        IOCallbackClass.PRE_QUERY_FILE_ID,
        IOCallbackClass.POST_QUERY_FILE_ID,
        IOCallbackClass.PRE_SET_INFORMATION,
        IOCallbackClass.POST_SET_INFORMATION,
        IOCallbackClass.PRE_SET_FILE_SIZE,
        IOCallbackClass.POST_SET_FILE_SIZE,
        IOCallbackClass.PRE_SET_FILE_BASIC_INFO,
        IOCallbackClass.POST_SET_FILE_BASIC_INFO,
        IOCallbackClass.PRE_SET_FILE_STANDARD_INFO,
        IOCallbackClass.POST_SET_FILE_STANDARD_INFO,
        IOCallbackClass.PRE_SET_FILE_NETWORK_INFO,
        IOCallbackClass.POST_SET_FILE_NETWORK_INFO,
        IOCallbackClass.PRE_RENAME_FILE,
        IOCallbackClass.POST_RENAME_FILE,
        IOCallbackClass.PRE_DELETE_FILE,
        IOCallbackClass.POST_DELETE_FILE,
        IOCallbackClass.PRE_DIRECTORY,
        IOCallbackClass.POST_DIRECTORY,
        IOCallbackClass.PRE_QUERY_SECURITY,
        IOCallbackClass.POST_QUERY_SECURITY,
        IOCallbackClass.PRE_SET_SECURITY,
        IOCallbackClass.POST_SET_SECURITY,
        IOCallbackClass.PRE_CLEANUP,
        IOCallbackClass.POST_CLEANUP,
        IOCallbackClass.PRE_CLOSE,
        IOCallbackClass.POST_CLOSE,
    }


def test_str_flag() -> None:
    """Test str_flag (convert strings to int-flags) and converting int-flags back.

    Using an int-flag as an iterable is a feature introduced in Python 3.11.
    """
    sym_strs = [
        "POST_CREATE",
        "POST_FASTIO_READ",
        "POST_CACHE_READ",
        "POST_NOCACHE_READ",
        "POST_PAGING_IO_READ",
        "POST_FASTIO_WRITE",
        "POST_CACHE_WRITE",
        "POST_NOCACHE_WRITE",
        "POST_PAGING_IO_WRITE",
        "POST_QUERY_INFORMATION",
        "POST_SET_INFORMATION",
        "POST_DIRECTORY",
        "POST_QUERY_SECURITY",
        "POST_SET_SECURITY",
        "POST_CLEANUP",
        "POST_CLOSE",
    ]

    num = 2863311530

    assert (
        easefilter.bitmask.str_flag(
            sym_strs,
            easefilter.enums.IOCallbackClass,
        )
        == num
    )

    syms: easefilter.enums.IOCallbackClass = easefilter.enums.IOCallbackClass(num)
    assert {sym.name for sym in syms} == set(sym_strs)

    l2 = [
        "POST_CREATE",
        "POST_FASTIO_READ",
    ]

    assert (
        easefilter.bitmask.str_flag(
            l2,
            easefilter.enums.IOCallbackClass,
        )
        == 10
    )

    assert {sym.name for sym in easefilter.enums.IOCallbackClass(10)} == set(l2)


def test_enums() -> None:
    """Test enum iteration behaviour.

    Iterating over an enum instance should yield only itself.
    """

    class e(AutoEnum):
        a = 1
        b = 2
        c = 3

    assert set(e) == {e.a, e.b, e.c}
    assert tuple(set(i) for i in e) == tuple({i} for i in e)
