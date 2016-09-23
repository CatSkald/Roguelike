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
    CreateCSharpAssemblyInfo "./CommonAssemblyInfo.cs"
      [ Attribute.Version version
        Attribute.FileVersion version ]
)

Target "Build" (fun _ ->
    DotNetCli.Restore id

    !! "./**/project.json"
    |> DotNetCli.Build id
)

Target "Test" (fun _ ->
    !! "./test/**/*Tests/project.json"
    |> DotNetCli.Test id
)

Target "Package" (fun _ ->
    DotNetCli.Pack 
        (fun p -> 
            { p with 
                OutputPath = outputDir })
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