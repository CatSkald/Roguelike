#!/bin/bash
if test "$OS" = "Windows_NT"
then
./tools/NuGet/NuGet.exe install FAKE -OutputDirectory packages -ExcludeVersion
./tools/NuGet/NuGet.exe install OpenCover -Version 4.6.519 -OutputDirectory packages
./tools/NuGet/NuGet.exe install coveralls.net -Version 0.412.0 -OutputDirectory packages
./packages/FAKE/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
else
mono ./tools/NuGet/NuGet.exe install FAKE -OutputDirectory packages -ExcludeVersion
mono ./tools/NuGet/NuGet.exe install System.Net.Http -OutputDirectory packages
mono ./tools/NuGet/NuGet.exe install Microsoft.Net.Http -OutputDirectory packages
mono ./tools/NuGet/NuGet.exe install OpenCover -Version 4.6.519 -OutputDirectory packages
mono ./tools/NuGet/NuGet.exe install coveralls.net -Version 0.412.0 -OutputDirectory packages
mono ./tools/FAKE.Core/tools/FAKE.exe $@ --fsiargs -d:MONO build.fsx
fi