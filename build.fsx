#r "packages/FAKE/tools/FakeLib.dll"
open Fake
open Fake.DotNetCli
open Fake.AppVeyor
open Fake.EnvironmentHelper

let outputDir = "./output"
let deployDir = outputDir @@ "/deploy/"
let packageName = "CatSkald.Roguelike"
let mainProject = "./src/CatSkald.Roguelike.Host/project.json"

let version = EnvironmentHelper.environVarOrDefault "APPVEYOR_BUILD_VERSION" "0.0.1"
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

    !! "./**/project.json"
    |> Seq.iter (fun file -> 
        DotNetCli.Build (fun p -> 
        { p with 
            Project = file
            Configuration = "Release"
        })
    )
)

open Fake.OpenCoverHelper
Target "Test" (fun _ ->
    !! "./test/**/*Tests/project.json"
    |> Seq.iter(fun file -> 
         let targetArguments = sprintf "test %O" (DirectoryName file)
         OpenCoverHelper.OpenCover (fun p -> 
            { p with 
                ExePath = "./packages/OpenCover.4.6.519/tools/OpenCover.Console.exe"
                TestRunnerExePath = dotnetPath
                Filter = "+[*]* -[*.Test.*]*"
                Output = "coverage.xml"
                Register = RegisterUser
                OptionalArguments = "-mergeoutput -oldstyle"
            })
            targetArguments)


    let result = Shell.Exec("./packages/coveralls.net.0.412/tools/csmacnz.Coveralls.exe","--opencover -i coverage.xml") 
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
            VersionSuffix = version
        })
)

Target "Deploy" DoNothing


"Clean"
  ==> "UpdateAssemblyInfo"
  ==> "Build"
  ==> "Test"
  ==> "Package"
  ==> "Deploy"
  
  
RunTargetOrDefault "Deploy"
