@echo off
cls
.\tools\NuGet\NuGet.exe Install FAKE -OutputDirectory packages -ExcludeVersion
.\tools\NuGet\NuGet.exe Install OpenCover -Version 4.6.519 -OutputDirectory packages
.\tools\NuGet\NuGet.exe Install coveralls.net -Version 0.412.0 -OutputDirectory packages

packages\FAKE\tools\Fake.exe build.fsx
pause