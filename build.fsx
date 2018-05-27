#r "packages/FAKE.Core/tools/FakeLib.dll"
#r "packages/FAKE.Core.Trace/lib/netstandard2.0/Fake.Core.Trace.dll"
#r "packages/FAKE.Core.Process/lib/netstandard2.0/Fake.Core.Process.dll"
#r "packages/FAKE.Core.Environment/lib/netstandard2.0/Fake.Core.Environment.dll"
#r "packages/FAKE.Core.Context/lib/netstandard2.0/Fake.Core.Context.dll"
#r "packages/FAKE.Testing.ReportGenerator/lib/netstandard2.0/Fake.Testing.ReportGenerator.dll"
#r "packages/FAKE.IO.FileSystem/lib/netstandard2.0/FAKE.IO.FileSystem.dll"
#r "packages/NETStandard.Library/build/netstandard2.0/ref/netstandard.dll"

let execContext = Fake.Core.Context.FakeExecutionContext.Create false "path/to/script.fsx" []
Fake.Core.Context.setExecutionContext (Fake.Core.Context.RuntimeContext.Fake execContext)

open Fake
open Fake.Testing

let outputDir = "./output"
let deployDir = outputDir @@ "/deploy/"
let packageName = "CatSkald.Roguelike"
let mainProject = "./src/CatSkald.Roguelike.Host/CatSkald.Roguelike.Host.csproj"

let version = EnvironmentHelper.environVarOrDefault "APPVEYOR_BUILD_VERSION" "0.0.1"
let buildNumber = EnvironmentHelper.environVarOrDefault "BUILD_NUMBER" "0"
let alphaVersionSuffix = "alpha" + (if (buildNumber <> "0") then (buildNumber) else "")
let dotnetPath = EnvironmentHelper.environVarOrDefault "DOTNET_INSTALL_DIR" "C:/Program Files/dotnet/dotnet.exe"

Target "Clean" (fun _ ->
    CleanDir outputDir
)

open Fake.AssemblyInfoFile
Target "UpdateAssemblyInfo" (fun _ ->
    UpdateAttributes "./CommonAssemblyInfo.cs"
      [ Attribute.Version version
        Attribute.FileVersion version ]
)

Target "Build" (fun _ ->
    DotNetCli.Restore id

    !! "./src/**/*.csproj"
      ++ "./test/**/*.csproj"
    |> Seq.iter (fun file -> 
        DotNetCli.Build (fun p -> 
        { p with 
            Project = file
            Configuration = "Release"
        })
    )
)

Target "UnitTestWithCoverlet" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter (fun file -> 
        DotNetCli.Test (fun p -> 
        { p with 
            Project = file
            Configuration = "Release"
            AdditionalArgs = ["/p:CollectCoverage=true"; "/p:CoverletOutputFormat=opencover"]
        })
    )
)

Target "MergeCoverageReportsIntoSingleFile" (fun _ ->
    let coverageFiles = !! "./test/**/coverage.xml"
                        |> String.concat ";"
    
    ReportGenerator.generateReports(fun p -> 
        { p with 
            ExePath = "packages/ReportGenerator/tools/ReportGenerator.exe"
            ReportTypes = [ReportGenerator.ReportType.XmlSummary]
            TargetDir = "./coverage"
        })([coverageFiles])
)

open Fake.OpenCoverHelper
Target "UnitTestWithOpenCover" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter(fun file -> 
         let targetArguments = sprintf "test %O" (DirectoryName file)
         OpenCoverHelper.OpenCover (fun p -> 
            { p with 
                ExePath = "./packages/OpenCover/tools/OpenCover.Console.exe"
                TestRunnerExePath = dotnetPath
                Filter = "+[CatSkald.*]* -[*Tests]*"
                Output = "coverage/opencover.xml"
                Register = RegisterUser
                OptionalArguments = "-mergeoutput -oldstyle"
            })
            targetArguments)
)

Target "PublishTestCoverage" (fun _ ->
    let result = Shell.Exec("./packages/coveralls.net/tools/csmacnz.Coveralls.exe","--opencover -i coverage/Summary.xml") 
    if result <> 0 then failwithf "Error during sending coverage to coverall: %d" result
    ()

    let result = Shell.Exec("./packages/coveralls.net/tools/csmacnz.Coveralls.exe","--opencover -i coverage/opencover.xml") 
    if result <> 0 then failwithf "Error during sending coverage to coverall: %d" result
    ()

    // TODO add codecov coverage
    // ExecuteGetCommand null null "https://codecov.io/bash"
    // Shell.Exec("./CodecovUploader.sh", "-f coverage.xml -X gcov")
)

Target "Pack" (fun _ ->
    DotNetCli.Pack (fun p -> 
        { p with 
            Project = mainProject
            Configuration = "Release"
            OutputPath = outputDir
            VersionSuffix = alphaVersionSuffix
        })
)

Target "Deploy" DoNothing


"Clean"
  ==> "UpdateAssemblyInfo"
  ==> "Build"
  ==> "UnitTestWithCoverlet"
  =?> ("MergeCoverageReportsIntoSingleFile", (buildServer = BuildServer.AppVeyor))
  =?> ("UnitTestWithOpenCover", (buildServer = BuildServer.AppVeyor))
  =?> ("PublishTestCoverage", (buildServer = BuildServer.AppVeyor))
  ==> "Pack"
  ==> "Deploy"
  
  
RunTargetOrDefault "Deploy"
