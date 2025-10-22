@echo off

:: Wrapper over .jar file. Works around this issue: https://stackoverflow.com/a/48954048

setlocal enabledelayedexpansion

set "ARGS="

:loop
if "%~1"=="" goto done
if defined ARGS (
    set "ARGS=!ARGS!;%~1"
) else (
    set "ARGS=%~1"
)
shift
goto loop

:done
set "ARGS=%ARGS%"

cd target
java -jar ef-demo.jar

endlocal
