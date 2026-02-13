@echo off
setlocal EnableExtensions EnableDelayedExpansion

set "PROJECT=VukCPU.csproj"
if not defined CONFIG set "CONFIG=Release"

set "TARGET=%~1"
if not defined TARGET set "TARGET=all"

rem Collect any arguments after the target name.
set "EXTRA_ARGS="
shift
:collect_args
if "%~1"=="" goto args_done
if defined EXTRA_ARGS (
    set "EXTRA_ARGS=!EXTRA_ARGS! %~1"
) else (
    set "EXTRA_ARGS=%~1"
)
shift
goto collect_args
:args_done

if not defined ARGS set "ARGS="
if defined EXTRA_ARGS (
    if defined ARGS (
        set "ARGS=%ARGS% %EXTRA_ARGS%"
    ) else (
        set "ARGS=%EXTRA_ARGS%"
    )
)

if /I "%TARGET%"=="all" goto all
if /I "%TARGET%"=="build" goto build
if /I "%TARGET%"=="run" goto run
if /I "%TARGET%"=="clean" goto clean

echo Unknown target: %TARGET%
echo Usage: Make.bat [all^|build^|run^|clean] [run args...]
exit /b 1

:all
echo "Defaulting to build, args: build, run, clean"
call "%~f0" build
exit /b %errorlevel%

:build
dotnet build "%PROJECT%" -c "%CONFIG%"
exit /b %errorlevel%

:run
dotnet run --project "%PROJECT%" -c "%CONFIG%" -- %ARGS%
exit /b %errorlevel%

:clean
dotnet clean "%PROJECT%" -c "%CONFIG%"
exit /b %errorlevel%
