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
open Fake.DotNet.Testing.OpenCover
open Fake.IO
open Fake.IO.FileSystemOperators
open Fake.IO.Globbing
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

let failOnBadExitAndPrint (p : ProcessResult) =
    if p.ExitCode <> 0 then
        p.Errors |> Seq.iter Trace.traceError
        failwithf "failed with exitcode %d" p.ExitCode

let tool optionConfig command args =
    DotNet.exec (fun p -> { p with WorkingDirectory = "."} |> optionConfig ) (sprintf "%s" command) args
    |> failOnBadExitAndPrint

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

Target.create "TestWithCoverlet" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter (fun file -> 
        DotNet.test (fun p -> 
        { p with 
            Configuration = DotNet.BuildConfiguration.Release
            RunSettingsArguments = Some("/p:CollectCoverage=true /p:CoverletOutputFormat=opencover")
        }) file
    )
)

Target.create "TestWithAltCover" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter (fun file -> 
        DotNet.test (fun p -> 
        { p with 
            Configuration = DotNet.BuildConfiguration.Release
            RunSettingsArguments = Some("/p:AltCover=true")
        }) file
    )
)

Target.create "GenerateTestCoverageReport" (fun _ ->
    let reportGenerator optionConfig args =
        tool optionConfig "reportgenerator" args

    let coverageReports =
        !!"./test/**/coverage.xml"
        |> String.concat ";"
    let sourceDirs =
        !! "./src/**/*.??proj"
        |> Seq.map Path.getDirectory
        |> String.concat ";"
    let independentArgs =
            [
                sprintf "-reports:%s" coverageReports
                sprintf "-targetdir:%s" "coverage"
                sprintf "-sourcedirs:%s" sourceDirs
                sprintf "-Reporttypes:%s" "XMLSummary"
            ]
    let args = independentArgs
               |> String.concat " "

    reportGenerator id args
)

Target.create "TestWithOpenCover" (fun _ ->
    let independentArgs =
            [
                sprintf "-target:%s" dotnetPath
                sprintf "-register:%s" "user"
                sprintf "-filter:%s" "+[CatSkald.*]* -[*Tests]*"
                sprintf "-output:%s" "coverage/opencover.xml"
                "-mergeoutput"
                "-oldstyle"
            ]
    let args = independentArgs
               |> String.concat " "

    let openCover optionConfig args =
        tool optionConfig "OpenCover.Console" args

    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter(fun file -> 
         openCover id (sprintf "%s test %O" args file)
        //  let targetArguments = sprintf "test %O" file
        //  OpenCover.run (fun p -> 
        //     { p with 
        //         ExePath = "./packages/OpenCover/tools/OpenCover.Console.exe"
        //         TestRunnerExePath = dotnetPath
        //         Filter = "+[CatSkald.*]* -[*Tests]*"
        //         Output = "coverage/opencover.xml"
        //         //Register = RegisterUser
        //         OptionalArguments = "-mergeoutput -oldstyle"
        //     })
        //     targetArguments)
    )
)

Target.create "PublishTestCoverage" (fun _ ->
    let coveralls optionConfig args =
        tool optionConfig "coveralls.net" args

    coveralls id "--opencover -i coverage/Summary.xml"

    // let result = Shell.Exec("./packages/coveralls.net/tools/csmacnz.Coveralls.exe","--opencover -i coverage/Summary.xml") 
    // if result <> 0 then failwithf "Error during sending coverage to coverall: %d" result
    // ()

    // let result = Shell.Exec("./packages/coveralls.net/tools/csmacnz.Coveralls.exe","--opencover -i coverage/opencover.xml") 
    // if result <> 0 then failwithf "Error during sending coverage to coverall: %d" result
    // ()

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
  ==> "TestWithCoverlet"
//  ==> "TestWithAltCover"
//  =?> ("GenerateTestCoverageReport", (BuildServer.buildServer = BuildServer.AppVeyor))
//  =?> ("TestWithOpenCover", (BuildServer.buildServer = BuildServer.AppVeyor))
//  =?> ("PublishTestCoverage", (BuildServer.buildServer = BuildServer.AppVeyor))
  ==> "Pack"
  ==> "Deploy"

Target.runOrDefault "Deploy"
