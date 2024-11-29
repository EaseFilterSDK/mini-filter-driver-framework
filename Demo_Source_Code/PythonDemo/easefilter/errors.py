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

"""Custom exceptions."""


class FilterError(Exception):
    """Generic error in the filter."""


class CorruptionError(FilterError):
    """Communication between driver and userspace got corrupted."""


class PrivilegeError(Exception):
    def __init__(self) -> None:
        super().__init__("Please run this program with administrator privileges.")
