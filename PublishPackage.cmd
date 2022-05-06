@ECHO OFF

ECHO Puslish on Nuget Version 2.0.1 of HelperAndToolsForUnitTest
dotnet nuget push .\HelperAndToolsForTest\bin\Debug\HelperAndToolsForUnitTest.2.0.1.nupkg --api-key oy2m5xejlr3b4wcght7q23zjfv776wxe7d5dpfck5mkvze --source https://api.nuget.org/v3/index.json

echo Completated
