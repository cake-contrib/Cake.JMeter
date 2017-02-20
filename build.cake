#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=GitVersion.Commandline"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
string version = null;

Task("Restore")
    .Does(() =>
    {
        NuGetRestore("Cake.JMeter/Cake.JMeter.sln");
    });

Task("UpdateAssemblyInfo")
    .Does(() => 
{
    version = GitVersion(new GitVersionSettings { UpdateAssemblyInfo = true }).NuGetVersionV2;
    if(AppVeyor.IsRunningOnAppVeyor) AppVeyor.UpdateBuildVersion(version);
});

Task("Clean")
    .Does(() => 
{
    CleanDirectory("./nuget");
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
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
    // NUnit3("./**/bin/**/*.Tests.dll");
    // if(AppVeyor.IsRunningOnAppVeyor) AppVeyor.UploadTestResults("./TestResult.xml", AppVeyorTestResultsType.NUnit3);
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() =>
{
    var nuGetPackSettings   = new NuGetPackSettings 
    {
        Id           = "Cake.JMeter",
        Version      = version,
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
    var file = GetFiles("nuget/*.nupkg").First();
    if(AppVeyor.IsRunningOnAppVeyor) AppVeyor.UploadArtifact(file);
});

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);