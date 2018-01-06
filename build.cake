/// args
var target = Argument("target", "default");

var soluctionFile    = "./cake.example.sln";
var srcProjectFiles  = GetFiles("./1-src/**/*.csproj");
var testProjectFiles = GetFiles("./2-test/**/*.csproj");
var distPath         = "./3-dist/";

/// build task
Task("build")
    .IsDependentOn("clean")
    .IsDependentOn("restore")
    .Does(() =>
{
	var dotNetCoreBuildSettings = new DotNetCoreBuildSettings
    {
        ArgumentCustomization = _=>_.Append("--no-restore")
    };
     
	DotNetCoreBuild(soluctionFile, dotNetCoreBuildSettings);
});

Task("clean")
    .Does(() =>
{
	DotNetCoreClean(soluctionFile);
});

/// restore task
Task("restore")
    .Does(() =>
{
	DotNetCoreRestore(soluctionFile);
});

/// test task
Task("test")
    .IsDependentOn("build")
    .Does(() =>
{
	var dotNetCoreTestSettings = new DotNetCoreTestSettings
    {
        ArgumentCustomization = _=>_.Append("--no-build").Append("--no-restore")
    };
    foreach(var testProjectFile in testProjectFiles)
    {
        DotNetCoreTest(testProjectFile.FullPath, dotNetCoreTestSettings);
    }
});

/// pack task
Task("pack")
    .IsDependentOn("test")
    .Does(() =>
{
    DeleteFiles(distPath + "*.nupkg");
    
    var dotNetCorePackSetting = new DotNetCorePackSettings {
        Configuration = "Release",
        OutputDirectory = distPath,
        IncludeSource = true,
        IncludeSymbols = true,
        NoBuild = false
    };

    foreach(var project in srcProjectFiles){
        DotNetCorePack(project.FullPath, dotNetCorePackSetting);
    }
});

Task("default")
    .IsDependentOn("test");

/// run task
RunTarget(target);