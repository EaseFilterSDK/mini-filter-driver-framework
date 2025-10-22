# EaseFilter Python

This is a demo library that provides a Pythonic API over the EaseFilter driver.
The source code is throughly documented, and intended to serve as an example
for using EaseFilter.

A CLI is included, based on the C++ demo.

*Python version 3.9 or above is required* to use this program.
Also, **a license key is required**. Contact
[info@easefilter.com](mailto:info@easefilter.com) for a trial key.

The provided interfaces include:

* Monitoring of the Windows Registry

* Monitoring of specific processes

* Monitoring of file/folder access

* Control of file/folder access

* Filesystem-level encryption of specific folders and files

## Installation

NOTE: Administrator permissions are required to run EaseFilter.

### Install with Hatch

The recommended installation method uses [Hatch](https://hatch.pypa.io/latest/),
which takes care of the project dependencies.

First, [install Python](https://www.python.org/downloads/).

Then, install hatch:

    pip install hatch

Edit the license key field in `config.toml`:

    license_key = "..."

Change to this directory, and to run the included CLI, use:

    hatch run cli [args]

Otherwise, to start a Python REPL (command line), run:

    hatch run py

In this environment, you can import the `easefilter` module to interface with
the filter. The CLI's code is available at `easefilter/cli/main.py`, and can
serve as an example for using this library.

### Manual installation

If you do not wish to use Hatch, you can run this project directly through Python.

First, [install Python](https://www.python.org/downloads/).

Then, install the dependencies:

    pip install -r requirements.txt

Edit the license key field in `config.toml`:

    license_key = "..."

To run the CLI, use:

    python -m easefilter.cli [args]

And to start a Python REPL, change directory here, and run Python normally:

    python

## Visual Studio

Visual Studio can be used to navigate this codebase easily. Open
`ef-python.sln` to open this project.

There are no integrations for debugging/running the code directly in VS. Use
the terminal to install and run this project.

It may be helpful to set up a virtual environment; to do this, select `Tools /
Python / Python Environments` and click `Add environment`.

## CLI examples

Here are a few example CLI commands to try.

To get detailled help, run

    hatch run cli --help

or, for help on a specific subcommand,

    hatch run cli monitor --help

For documentation on event types you can register, see the file `easefilter/enums.py`.

### Monitor filter

Default settings, creates a test directory (`C:\easefilter_demo`) and monitors its contents:

    hatch run cli monitor

Monitor events for all files on your device:

    hatch run cli monitor *

Monitor a specific directory:

    hatch run cli monitor "C:\directory\*"

Enable monitoring all specific events (the `--io-mask` is shorthand for all the bitflags combined):

    hatch run cli monitor --io-mask 192118399729052330

Monitor all file handle creation events:

    hatch run cli monitor --io-sym POST_CREATE

### Control filter

Deny all file writes in a directory:

    hatch run cli control --deny-sym "ALLOW_WRITE_ACCESS,ALLOW_OPEN_WITH_CREATE_OR_OVERWRITE_ACCESS" C:\a_directory\*

Deny all reads in the directory (also denies `cd`-ing into directory):

    hatch run cli control --deny-sym "ALLOW_READ_ACCESS,ALLOW_OPEN_WITH_READ_ACCESS" C:\a_directory\*

Deny file listing in a directory (makes directory seem empty):

    hatch run cli control --deny-sym ALLOW_DIRECTORY_LIST_ACCESS C:\a_directory\*

### Process filter

Monitor all process creations/deletions (try starting a PowerShell to test this):

    hatch run cli process

Monitor specifically `cmd.exe`:

    hatch run cli process --proc-mask C:\Windows\System32\cmd.exe

Monitor all System32 processes:

    hatch run cli process --proc-mask C:\Windows\System32\*

(**BE CAREFUL** with this flag) Prevent `cmd.exe` from running:

    hatch run cli process --proc-control-sym DENY_NEW_PROCESS_CREATION --proc-mask C:\Windows\System32\cmd.exe

### Encryption filter

Transparently encrypt a directory:

    hatch run cli encryption 'C:\my_encrypted_dir\*'

This will prompt for a password. The password prompt does not show the
characters you type in.

Files will only be decrypted while the filter is running. Once stopped, files
will be encrypted and unreadable.

### Registry filter

Monitor all registry events:

    hatch run cli registry

Monitor all registry events for keys with a matching name:

    hatch run cli registry *KeyName*

Monitor all registry events coming from regedit:

    hatch run cli registry --proc-mask C:\Windows\regedit.exe

Monitor all registry key rename events coming from regedit:

    hatch run cli registry --reg-class-sym Reg_Pre_Rename_Key --proc-mask C:\Windows\regedit.exe

Prevent regedit from deleting keys:

    hatch run cli registry --reg-deny-sym REG_ALLOW_DELETE_KEY --proc-mask C:\Windows\regedit.exe

## Development

To format/lint after changing any code, run

    hatch fmt

or, to do a dry-run and avoid making changes,

    hatch fmt --check

These will run [Ruff](https://astral.sh/ruff) and [Pyright](https://github.com/microsoft/pyright).

Tests can be performed with [pytest](https://pytest.org/) like this:

    hatch test

You may specify a test to run by adding its path (e.g. `tests/test_something.py`) as an argument.
Consult [pytest's documentation](https://docs.pytest.org/en/stable/) for more information.
