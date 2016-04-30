@echo off
cls
"tools\nuget\nuget.exe" "install" "FAKE" "-OutputDirectory" "tools" "-ExcludeVersion" "-Version" "2.18.0"
"tools\FAKE\tools\Fake.exe" build.fsx
pause