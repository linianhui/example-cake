#reference "NuGet.Packaging"

#load nuget.push.cake

var target = Argument("target", "default");

var rootPath     = "../";
var srcPath      = rootPath + "1-src/";
var testPath     = rootPath + "2-test/";
var distPath     = rootPath + "3-dist/";

var solution    = rootPath + "cake.example.sln";
var srcProjects  = GetFiles(srcPath + "**/*.csproj");
var testProjects = GetFiles(testPath + "**/*.csproj");

Task("clean")
    .Description("清理项目缓存")
    .Does(() =>
{
    DotNetCoreClean(solution);
    DeleteFiles(distPath + "*.nupkg");
});

Task("restore")
    .Description("还原项目依赖")
    .Does(() =>
{
    DotNetCoreRestore(solution);
});

Task("build")
    .Description("编译项目")
    .IsDependentOn("clean")
    .IsDependentOn("restore")
    .Does(() =>
{
    var buildSetting = new DotNetCoreBuildSettings{
        NoRestore = true
    };
     
    DotNetCoreBuild(solution, buildSetting);
});


Task("test")
    .Description("运行测试")
    .IsDependentOn("build")
    .Does(() =>
{
    var testSetting = new DotNetCoreTestSettings{
        NoRestore = true,
        NoBuild = true
    };

    foreach(var testProject in testProjects)
    {
        DotNetCoreTest(testProject.FullPath, testSetting);
    }
});

Task("pack")
    .Description("nuget打包")
    .IsDependentOn("test")
    .Does(() =>
{
    var packSetting = new DotNetCorePackSettings {
        Configuration   = "Release",
        OutputDirectory = distPath,
        IncludeSource   = true,
        IncludeSymbols  = true,
        NoBuild         = false
    };

    foreach(var srcProject in srcProjects){
        DotNetCorePack(srcProject.FullPath, packSetting);
    }
});

Task("push")
    .Description("nuget发布")
    .IsDependentOn("pack")
    .Does(() =>
{
    NugetPacakge_Push(distPath);
});

Task("default")
    .Description("默认-运行测试(-target test)")
    .IsDependentOn("test");

RunTarget(target);