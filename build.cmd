@echo off

cls

SET NUGET_PATH=tools\NuGet\NuGet.exe

"%NUGET_PATH%" Install FAKE -OutputDirectory packages -ExcludeVersion
"%NUGET_PATH%" Install OpenCover -Version 4.6.519 -OutputDirectory packages
"%NUGET_PATH%" Install coveralls.net -Version 0.412.0 -OutputDirectory packages

packages\FAKE\tools\Fake.exe build.fsx
