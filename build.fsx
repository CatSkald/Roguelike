#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.DotNetCli
open Fake.AppVeyor
open Fake.EnvironmentHelper
open Fake.BuildServerHelper

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

open Fake.Testing.NUnit3
Target "UnitTest" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> NUnit3 (fun p ->
        {p with
            ShadowCopy = false
            ToolPath = "./packages/NUnit.ConsoleRunner.3.7.0/tools/nunit3-console.exe" 
        })
)

open Fake.OpenCoverHelper
Target "UnitTestWithCoverageReport" (fun _ ->
    !! "./test/**/*UnitTests.csproj"
    |> Seq.iter(fun file -> 
         let targetArguments = sprintf "test %O" (DirectoryName file)
         OpenCoverHelper.OpenCover (fun p -> 
            { p with 
                ExePath = "./packages/OpenCover.4.6.519/tools/OpenCover.Console.exe"
                TestRunnerExePath = dotnetPath
                Filter = "+[*]* -[*.Test.*]* -[*.*Tests]*"
                Output = "coverage.xml"
                Register = RegisterUser
                OptionalArguments = "-mergeoutput -oldstyle"
            })
            targetArguments)


    let result = Shell.Exec("./packages/coveralls.io.1.4.2/tools/coveralls.net.exe","--opencover -i coverage.xml") 
    if result <> 0 then failwithf "Error during sending coverage to coverall: %d" result
    ()
    // TODO add codecov coverage
    // ExecuteGetCommand null null "https://codecov.io/bash"
    // Shell.Exec("./CodecovUploader.sh", "-f coverage.xml -X gcov")
)

Target "Package" (fun _ ->
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
  //Generate test coverage only on AppVeyor as OpenCover does not work with Mono (used on Travis), 
  //and there is no need to generate multiple coverage reports for same sources
  =?> ("UnitTest", not (buildServer = BuildServer.AppVeyor))
  =?> ("UnitTestWithCoverageReport", (buildServer = BuildServer.AppVeyor))
  ==> "Package"
  ==> "Deploy"
  
  
RunTargetOrDefault "Deploy"
