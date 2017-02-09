#!/bin/bash

NUGET_PATH=./tools/NuGet/NuGet.exe
OUTPUT_DIRECTORY=./packages

if test "$OS" = "Windows_NT"
then
$NUGET_PATH install FAKE -OutputDirectory $OUTPUT_DIRECTORY -ExcludeVersion
$NUGET_PATH install OpenCover -Version 4.6.519 -OutputDirectory $OUTPUT_DIRECTORY
$NUGET_PATH install coveralls.net -Version 0.412.0 -OutputDirectory $OUTPUT_DIRECTORY
./packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
else
mono $NUGET_PATH install FAKE -OutputDirectory $OUTPUT_DIRECTORY -ExcludeVersion
mono $NUGET_PATH install System.Net.Http -OutputDirectory $OUTPUT_DIRECTORY
mono $NUGET_PATH install Microsoft.Net.Http -OutputDirectory $OUTPUT_DIRECTORY
mono $NUGET_PATH install OpenCover -Version 4.6.519 -OutputDirectory $OUTPUT_DIRECTORY
mono $NUGET_PATH install coveralls.net -Version 0.412.0 -OutputDirectory $OUTPUT_DIRECTORY
mono ./tools/FAKE.Core/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi