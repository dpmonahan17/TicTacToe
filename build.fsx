// include Fake libs
#r "./packages/FAKE/tools/FakeLib.dll"

open Fake
open Fake.Testing

// Directories
let buildDir  = "./build/"
let deployDir = "./deploy/"


// Filesets
let appReferences  =
    !! "/**/*.csproj"
    ++ "/**/*.fsproj"

// version info
let version = "0.1"  // or retrieve from CI server

// Targets
Target "Clean" (fun _ ->
    CleanDirs [buildDir; deployDir]
)

Target "Build" (fun _ ->
    // compile all projects below src/app/
    MSBuildDebug buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "Deploy" (fun _ ->
    !! (buildDir + "/**/*.*")
    -- "*.zip"
    |> Zip buildDir (deployDir + "TicTacToe." + version + ".zip")
)

let testDlls = !! (buildDir + "*Tests.exe")

Target "NUnitTest" (fun _ ->
  testDlls
    |> NUnit3 (fun p -> p)

)

// Build order
"Clean"
  ==> "Build"
  ==> "Deploy"
  ==> "NUnitTest"

// start build
RunTargetOrDefault "Build"
