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

from __future__ import annotations

import logging
from pathlib import Path
from typing import Any

try:
    import tomllib
except ModuleNotFoundError:  # no cov
    import tomli as tomllib  # pyright: ignore reportMissingImports


def get_config(config_path: Path | None = None) -> dict[str, Any]:
    if config_path is None:
        config_path = Path(__file__).parents[1] / "config.toml"
    if not config_path.is_file():
        msg = f"Configuration file {config_path} is not a file."
        raise ValueError(msg)

    with config_path.open("rb") as f:
        logging.debug("Loading config file at %s.", config_path)
        return tomllib.load(f)
