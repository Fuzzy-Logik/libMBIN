@echo off
setlocal

REM ***************************************************************************
REM This is a post-build event script for the MBINCompiler project.
REM
REM Syntax:
REM     post-build.bat "$(SolutionName)" "$(TargetDir)" "$(ConfigurationName)"
REM
REM It will generate a release package using a generated name of the
REM format:
REM     "$(SolutionName)-GIT_TAG-$(ConfigurationName) (YYYY-MM-DD-HH-mm).zip"
REM in $(TargetDir).
REM 
REM The package will include
REM     - *.exe
REM     - *.dll
REM     - *.txt
REM     - *.md
REM
REM A VERSION.TXT file will also be generated that will contain:
REM     - The product name
REM     - Version: %VERSION_TAG% ($(ConfigurationName) Build)
REM     - Build ID: %COMMIT_SHA%
REM     - Timestamp: YYYY-MM-DD HH:mm
REM
REM Make sure the project has been committed before building and
REM distributing a release pkg or else the SHA will be off-by-one.
REM
REM This script requires 7z.exe and expects to find it the path .\bin
REM relative to this file.
REM ***************************************************************************


REM Get the command line arguments
set SOLUTION_NAME=%~1
set TARGET_DIR=%~2

REM remove trailing slash from TARGET_DIR
set TARGET_DIR=%TARGET_DIR:~0,-1%


REM ***************************************************************************
REM INIT

REM Get the path to this script
set SCRIPT_PATH=%~dp0

REM Get the last tag on or before this commit.
for /f "usebackq tokens=*" %%I in (`git describe --always --tags --candidates=1`) do set VERSION_TAG=%%I

REM Get the SHA commit hash 
for /f "usebackq tokens=*" %%I in (`git rev-parse HEAD`) do set COMMIT_SHA=%%I

REM format the timestamp: YYYY-MM-DD-HH-mm
set DATESTAMP=%date:~10,4%-%date:~4,2%-%date:~7,2%
set TIMESTAMP=%time:~0,2%.%time:~3,2%

REM Derive CONFIG_NAME from TARGET_DIR
for /f "usebackq tokens=*" %%I in ('%TARGET_DIR%') do set CONFIG_NAME=%%~nI

REM Get the config abbreviation
set CFG_NAME=dbg
if "%CONFIG_NAME%"=="Release" set CFG_NAME=rel

REM The completed pkg name. Can't use : in a filename so we replace with -
set RELEASE=%SOLUTION_NAME%-%VERSION_TAG%-%CFG_NAME:~0,3% (%DATESTAMP%.%TIMESTAMP%)

REM The package folder
set RELEASE_PATH=%TARGET_DIR%\%RELEASE%

REM This file will be generated
set VERSION_FILE=%TARGET_DIR%\VERSION.TXT


REM ***************************************************************************
REM EXECUTE

REM Generate the VERSION_FILE
echo "%SOLUTION_NAME%"                                   > "%VERSION_FILE%"
echo "Version: %VERSION_TAG:-= % (%CONFIG_NAME% build)" >> "%VERSION_FILE%"
echo "TimeStamp: %DATESTAMP% %TIMESTAMP:.=^:%"          >> "%VERSION_FILE%"
echo "SHA: %COMMIT_SHA%"                                >> "%VERSION_FILE%"

REM Assemble the package
mkdir "%RELEASE_PATH%"
copy "%TARGET_DIR%\*.exe" "%RELEASE_PATH%\"
copy "%TARGET_DIR%\*.dll" "%RELEASE_PATH%\"
copy "%TARGET_DIR%\*.txt" "%RELEASE_PATH%\"
copy "%TARGET_DIR%\*.md"  "%RELEASE_PATH%\"

REM Create a .zip archive
"%SCRIPT_PATH%\bin\7z.exe" a -bd -r -tzip "%RELEASE%.zip" "%RELEASE_PATH%"


REM ***************************************************************************
REM CLEANUP

REM Cleanup package assembly
REM rd /s /q  "%ARCHIVE_PATH%"