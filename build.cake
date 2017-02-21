#tool "nuget:?package=xunit.runner.console"
#tool "nuget:?package=GitVersion.Commandline"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetApiToken = EnvironmentVariable("nuget_api_token");

Cake.Common.Tools.GitVersion.GitVersion version = null;

Task("Restore")
    .Does(() =>
{
    NuGetRestore("Cake.JMeter/Cake.JMeter.sln");
});

Task("UpdateAssemblyInfo")
    .Does(() => 
{
    version = GitVersion(new GitVersionSettings { UpdateAssemblyInfo = true });
    if (AppVeyor.IsRunningOnAppVeyor) AppVeyor.UpdateBuildVersion(version.NuGetVersionV2);
});

Task("Clean")
    .Does(() => 
{
    CleanDirectory("./nuget");
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
    DeleteFile("./Cake.JMeter.Tests.dll.xml");
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateAssemblyInfo")
    .Does(() => 
{
    MSBuild("Cake.JMeter/Cake.JMeter.sln", configurator =>
        configurator
            .SetConfiguration(configuration)
            .SetVerbosity(Verbosity.Minimal));
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    XUnit2("./**/bin/**/*.Tests.dll", new XUnit2Settings {
        XmlReport = true,
        OutputDirectory = "."
    });
    if (AppVeyor.IsRunningOnAppVeyor) 
    {
        AppVeyor.UploadTestResults("./Cake.JMeter.Tests.dll.xml", AppVeyorTestResultsType.XUnit);
    }
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
{
    var nuGetPackSettings   = new NuGetPackSettings 
    {
        Id           = "Cake.JMeter",
        Version      = version.NuGetVersionV2,
        Authors      = new[] {"pitermarx"},
        Description  = "Cake aliases for JMeter. To be used in conjunction with the JMeter tool.",
        ProjectUrl   = new Uri("https://github.com/pitermarx/Cake.JMeter"),
        LicenseUrl   = new Uri("https://github.com/pitermarx/Cake.JMeter/blob/master/LICENSE"),
        Tags         = new [] {"cake","jmeter"},
        Files        = new [] { 
            new NuSpecContent { Source = "Cake.JMeter/bin/Release/Cake.JMeter.dll", Target = "lib/net45" },
            new NuSpecContent { Source = "Cake.JMeter/bin/Release/Cake.JMeter.xml", Target = "lib/net45" },
            new NuSpecContent { Source = "Cake.JMeter/bin/Release/Cake.JMeter.pdb", Target = "lib/net45" }
        },
        BasePath        = "./",
        OutputDirectory = "./nuget"
    };

    NuGetPack(nuGetPackSettings);
});

Task("Publish")
    .WithCriteria(AppVeyor.IsRunningOnAppVeyor)
    .IsDependentOn("Pack")
    .Does(() =>
{
    var file = GetFiles("nuget/*.nupkg").First();
    AppVeyor.UploadArtifact(file);

    var tagged = AppVeyor.Environment.Repository.Tag.IsTag && 
        !string.IsNullOrWhiteSpace(AppVeyor.Environment.Repository.Tag.Name);

    if (tagged)
    { 
        // Push the package.
        NuGetPush(file, new NuGetPushSettings 
        {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = nugetApiToken
        });
    }
});

Task("Default")
    .IsDependentOn("Publish");

RunTarget(target);