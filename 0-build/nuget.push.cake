using NuGet.Packaging;

// https://github.com/linianhui/nuget.server
var nugetPushSetting = new DotNetCoreNuGetPushSettings
{
    Source           = "http://nuget-server.test/nuget",
    ApiKey           = "123456",
    IgnoreSymbols    = false
};

var nugetListSetting = new NuGetListSettings {
    AllVersions = true,
    Source      = new string[]{ nugetPushSetting.Source }
};

void NugetPacakge_Push(string packagesPath)
{
    var packageFilePaths  =  GetFiles(packagesPath + "*.symbols.nupkg");
    var completed         = new List<IPackageMetadata>();
    var exists            = new List<IPackageMetadata>();
    var failed            = new List<IPackageMetadata>();

    foreach(var packageFilePath in packageFilePaths){
        var packageMetadata = GetPackageMetadata(packageFilePath.FullPath);
        try
        {
            if(NugetPacakge_IsPublished(nugetListSetting, packageMetadata)){
                exists.Add(packageMetadata);
                continue;
            }
            DotNetCoreNuGetPush(packageFilePath.FullPath, nugetPushSetting);
            completed.Add(packageMetadata);
        }
        catch
        {
            failed.Add(packageMetadata);
        }
    }

    NugetPacakge_Push_PrintResult(nugetPushSetting, packageFilePaths.Count, completed, exists, failed);
}

IPackageMetadata GetPackageMetadata(string filePath)
{
    using (var fileStream = new PackageArchiveReader(filePath).GetNuspec())
    {
        return Manifest.ReadFrom(fileStream, false).Metadata;
    }
}

bool NugetPacakge_IsPublished(NuGetListSettings setting, IPackageMetadata packageMetadata)
{
    var packages = NuGetList(packageMetadata.Id, setting);
    return packages.Any(_=>_.Name == packageMetadata.Id && _.Version == packageMetadata.Version.ToString());
}

void NugetPacakge_Push_PrintResult(
    DotNetCoreNuGetPushSettings setting,
    int totalPackageCounts, 
    List<IPackageMetadata> completed, 
    List<IPackageMetadata> exists,
    List<IPackageMetadata> failed)
{
    Information("\n发布到{0}的结果：", setting.Source);
    foreach(var package in completed){
        Information($"{package.Id,-36} {package.Version,-10} 发布完成");
    }
    foreach(var package in exists){
        Warning($"{package.Id,-36} {package.Version,-10} 已经存在，忽略发布");
    }
    foreach(var package in failed){
        Error($"{package.Id,-36} {package.Version,-10} 发布失败");
    }
    Information("共{0}，成功{1},已存在{2},失败{3}。\n", totalPackageCounts, completed.Count, exists.Count, failed.Count);
}