@echo off

cls

SET NUGET_PATH=tools\NuGet.exe

"%NUGET_PATH%" Install packages.config -OutputDirectory packages

packages\FAKE\tools\Fake.exe build.fsx
