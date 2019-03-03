#r "paket:
nuget Fake.IO.FileSystem
nuget Fake.DotNet.AssemblyInfoFile
nuget Fake.DotNet.Cli
nuget Fake.DotNet.MSBuild
nuget Fake.DotNet.Testing.OpenCover
nuget Fake.Core.Target 
nuget Fake.Testing.ReportGenerator
//"

#load ".fake/build.fsx/intellisense.fsx"

open Fake.Core
open Fake.Core.TargetOperators
open Fake.DotNet
open Fake.DotNet.Testing
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing.Operators
open Fake.Testing

let outputDir = "./output"
let deployDir = outputDir @@ "/deploy/"
let packageName = "CatSkald.Roguelike"
let mainProject = "./src/CatSkald.Roguelike.Host/CatSkald.Roguelike.Host.csproj"
let solution = "CatSkald.Roguelike.sln"

let version = Environment.environVarOrDefault "APPVEYOR_BUILD_VERSION" "0.0.1"
let buildNumber = Environment.environVarOrDefault "BUILD_NUMBER" "0"
let alphaVersionSuffix = if (buildNumber <> "0") then Some("alpha" + buildNumber) else None
let dotnetPath = Environment.environVarOrDefault "DOTNET_INSTALL_DIR" "C:/Program Files/dotnet/dotnet.exe"

Target.create "Clean" (fun _ ->
    !! "src/**/bin"
    ++ "src/**/obj"
    ++ outputDir
    |> Shell.cleanDirs 
)

Target.create "UpdateAssemblyInfo" (fun _ ->
    AssemblyInfoFile.createCSharp "./CommonAssemblyInfo.cs"
      [ AssemblyInfo.Copyright "Copyright © CatSkald 2016 - 2019"
        AssemblyInfo.Version version
        AssemblyInfo.FileVersion version ]
)

Target.create "Build" (fun _ ->
    DotNet.restore (fun p ->
    { p with
        NoCache = true 
    }) solution

    !! "./src/**/*.csproj"
    ++ "./test/**/*.csproj"
    |> Seq.iter (fun file -> 
        DotNet.build (fun p -> 
        { p with 
            Configuration = DotNet.BuildConfiguration.Release
        }) file
    )
)

Target.create "UnitTestWithCoverlet" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter (fun file -> 
        DotNet.test (fun p -> 
        { p with 
            Configuration = DotNet.BuildConfiguration.Release
            RunSettingsArguments = Some("/p:CollectCoverage=true /p:CoverletOutputFormat=opencover")
        }) file
    )
)

Target.create "MergeCoverageReportsIntoSingleFile" (fun _ ->
    let coverageFiles = !! "./test/**/coverage.xml"
                        |> String.concat ";"
    
    ReportGenerator.generateReports(fun p -> 
        { p with 
            ExePath = "packages/ReportGenerator/tools/ReportGenerator.exe"
            ReportTypes = [ReportGenerator.ReportType.XmlSummary]
            TargetDir = "./coverage"
        })([coverageFiles])
)

Target.create "UnitTestWithOpenCover" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter(fun file -> 
         let targetArguments = sprintf "test %O" file
         OpenCover.run (fun p -> 
            { p with 
                ExePath = "./packages/OpenCover/tools/OpenCover.Console.exe"
                TestRunnerExePath = dotnetPath
                Filter = "+[CatSkald.*]* -[*Tests]*"
                Output = "coverage/opencover.xml"
                //Register = RegisterUser
                OptionalArguments = "-mergeoutput -oldstyle"
            })
            targetArguments)
)

Target.create "PublishTestCoverage" (fun _ ->
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

Target.create "Pack" (fun _ ->
    DotNet.pack (fun p -> 
        { p with 
            Configuration = DotNet.BuildConfiguration.Release
            OutputPath = Some(outputDir)
            VersionSuffix = alphaVersionSuffix
        }) mainProject
)

Target.create "Deploy" (fun _ ->
    Trace.trace "Skipping deployment."
)

"Clean"
  ==> "UpdateAssemblyInfo"
  ==> "Build"
  ==> "UnitTestWithCoverlet"
  =?> ("MergeCoverageReportsIntoSingleFile", (BuildServer.buildServer = BuildServer.AppVeyor))
  =?> ("UnitTestWithOpenCover", (BuildServer.buildServer = BuildServer.AppVeyor))
  =?> ("PublishTestCoverage", (BuildServer.buildServer = BuildServer.AppVeyor))
  ==> "Pack"
  ==> "Deploy"

Target.runOrDefault "Deploy"
