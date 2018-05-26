#!/bin/bash

NUGET_PATH=tools/nuget.exe

if test "$OS" = "Windows_NT"
then
$NUGET_PATH install packages.config -OutputDirectory packages
packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
else
mono $NUGET_PATH install packages.config -OutputDirectory packages
mono $NUGET_PATH install System.Net.Http -OutputDirectory packages
mono $NUGET_PATH install Microsoft.Net.Http -OutputDirectory packages
mono packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi
