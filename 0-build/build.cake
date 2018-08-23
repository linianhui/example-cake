/// args
var target = Argument("target", "default");

var rootPath     = "../";
var srcPath      = rootPath + "1-src/";
var testPath     = rootPath + "2-test/";
var distPath     = rootPath + "3-dist/";

var soluction    = rootPath + "cake.example.sln";
var srcProjects  = GetFiles(srcPath + "**/*.csproj");
var testProjects = GetFiles(testPath + "**/*.csproj");

Task("clean")
    .Description("清理项目缓存")
    .Does(() =>
{
    DotNetCoreClean(soluction);
    DeleteFiles(distPath + "*.nupkg");
});

Task("restore")
    .Description("还原项目依赖")
    .Does(() =>
{
    DotNetCoreRestore(soluction);
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
     
    DotNetCoreBuild(soluction, buildSetting);
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

Task("default")
    .Description("默认-运行测试(-target test)")
    .IsDependentOn("test");

RunTarget(target);