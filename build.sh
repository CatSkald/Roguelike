#!/bin/bash

NUGET_PATH=tools/NuGet/NuGet.exe

if test "$OS" = "Windows_NT"
then
$NUGET_PATH install FAKE -OutputDirectory packages -ExcludeVersion
$NUGET_PATH install OpenCover -Version 4.6.519 -OutputDirectory packages
$NUGET_PATH install coveralls.net -Version 0.412.0 -OutputDirectory packages
packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
else
mono $NUGET_PATH install FAKE.Core -OutputDirectory packages -ExcludeVersion
mono $NUGET_PATH install System.Net.Http -OutputDirectory packages
mono $NUGET_PATH install Microsoft.Net.Http -OutputDirectory packages
mono $NUGET_PATH install OpenCover -Version 4.6.519 -OutputDirectory packages
mono $NUGET_PATH install coveralls.net -Version 0.412.0 -OutputDirectory packages
mono packages/FAKE.Core/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi