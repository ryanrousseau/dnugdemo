// include Fake lib
#r @"tools/FAKE/tools/FakeLib.dll"
open System
open System.IO
open Fake
open Fake.Testing.XUnit2
open TeamCityHelper
open OctoTools

// Properties
let buildDir = "./build/"
let testDir = "./test/"
let webBuildDir = buildDir + "_PublishedWebsites/octopusdemo/"
let toolsDir = "./tools/"
let nuspecFile = "./octopusdemo.nuspec"
let packageDir = "./package/"
let webPackageDir = packageDir + "Web/"

let version =
    match buildServer with
    | TeamCity -> buildVersion
    | _ -> "0.0.1-dev"

// Targets
Target "Clean" (fun _ ->
    CleanDir buildDir
    CleanDir testDir
    CleanDir packageDir
)

Target "Build" (fun _ ->
    !! "src/*.sln"
        |> MSBuild buildDir "Build" ["Configuration", "Release"; "VisualStudioVersion", "11.0"]
        |> Log "AppBuild-Output: "
)

Target "Test" (fun _ ->
    !! (buildDir + @"\*.Test.dll")
        |> xUnit2 (fun p -> { p with HtmlOutputPath = Some (testDir @@ "xunit.html") })
)

Target "Package" (fun _ ->
    CreateDir webPackageDir

    CopyFile webPackageDir nuspecFile
    CopyDir webPackageDir webBuildDir allFiles
)

Target "CreateNuGetPackage" (fun _ ->
    // Copy all the package files into a package folder

    NuGet (fun p -> 
        {p with
            Authors = ["ryanrousseau"]
            Project = "octopusdemo"
            Description = "octopusdemo"
            OutputPath = packageDir
            Summary = "octopusdemo"
            WorkingDir = webPackageDir
            Version = version
            Publish = false })
        "octopusdemo.nuspec"

    PublishArtifact (packageDir + "octopusdemo." + version + ".nupkg")
)

// Dependencies
"Clean"
    ==> "Build"
    ==> "Test"
    ==> "Package"
    ==> "CreateNuGetPackage"

// Start build
RunTargetOrDefault "CreateNuGetPackage"