#tool "nuget:?package=xunit.runner.console"

/// args
var target = Argument("target", "default");


/// build task
Task("build")
    .IsDependentOn("clean")
    .IsDependentOn("restore-nuget-packages")
    .Does(() =>
{
    MSBuild("./cake.demo.sln", new MSBuildSettings{
        Verbosity = Verbosity.Minimal
    });
});

Task("clean")
    .Does(() =>
{
    CleanDirectories("./src/*/bin");
    CleanDirectories("./test/*/bin");
});

/// nuget task
Task("restore-nuget-packages")
    .Does(() =>
{
    NuGetRestore("./cake.demo.sln");
});

/// unit-test task
Task("unit-test")
    .IsDependentOn("build")
    .Does(() =>
{
    XUnit2("./test/*/bin/*/*.Tests.dll");
});

Task("default")
    .IsDependentOn("unit-test");

/// run task
RunTarget(target);