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
    |> DotNetCli.Build id
)

open Fake.OpenCoverHelper
Target "Test" (fun _ ->
    !! "./test/**/*Tests/project.json"
    |> Seq.iter(fun file -> 
         let targetArguments = sprintf "test %O" (DirectoryName file)
         OpenCoverHelper.OpenCover 
            (fun p -> 
                { p with 
                    ExePath = "./packages/OpenCover.4.6.519/tools/opencover.console.exe"
                    TestRunnerExePath = "C:/Program Files/dotnet/dotnet.exe" 
                    Filter = "+[*]* -[*.Test.*]*"
                    Output = "coverage.xml"
                    Register = RegisterUser
                    OptionalArguments = "-mergeoutput -oldstyle"})
            targetArguments)


    let result = Shell.Exec("./packages/coveralls.net.0.412/tools/csmacnz.Coveralls.exe","--opencover -i coverage.xml") if result <> 0 then failwithf "%s exited with error %d" "batch.bat" result
)

Target "Package" (fun _ ->
    DotNetCli.Pack 
        (fun p -> { p with OutputPath = outputDir })
        [mainProject]
)

Target "Deploy" DoNothing


"Clean"
  ==> "UpdateAssemblyInfo"
  ==> "Build"
  ==> "Test"
  ==> "Package"
  ==> "Deploy"
  
  
RunTargetOrDefault "Deploy"
