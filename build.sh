#!/bin/bash

NUGET_PATH=tools/NuGet/nuget.exe

if test "$OS" = "Windows_NT"
then
$NUGET_PATH install FAKE -OutputDirectory packages -ExcludeVersion
$NUGET_PATH install OpenCover -Version 4.6.519 -OutputDirectory packages
$NUGET_PATH install coveralls.net -Version 0.7.0 -OutputDirectory packages
packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
else
$NUGET_PATH install FAKE -OutputDirectory packages -ExcludeVersion
$NUGET_PATH install System.Net.Http -OutputDirectory packages
$NUGET_PATH install Microsoft.Net.Http -OutputDirectory packages
$NUGET_PATH install OpenCover -Version 4.6.519 -OutputDirectory packages
$NUGET_PATH install coveralls.net -Version 0.7.0 -OutputDirectory packages
packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi