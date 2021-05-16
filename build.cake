#tool "nuget:?package=xunit.runner.console&version=2.4.1"
#tool "nuget:?package=GitVersion.CommandLine&version=3.6.5"
#tool "nuget:?package=OpenCover&version=4.6.519"
#tool "nuget:?package=coveralls.io&version=1.4.2"
#addin "nuget:?package=Cake.Coveralls&version=0.9.0"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var nugetApiToken = EnvironmentVariable("nuget_api_token");
var coverallsToken = EnvironmentVariable("coveralls_token");
var slnPath = File("./src/Cake.JMeter.sln");

var version = GitVersion(new GitVersionSettings { UpdateAssemblyInfo = true });
if (AppVeyor.IsRunningOnAppVeyor) {
    AppVeyor.UpdateBuildVersion(version.NuGetVersionV2);
}

Task("Clean")
    .Does(() => 
{
    CleanDirectory("./nuget");
    CleanDirectories("./**/bin");
    CleanDirectories("./**/obj");
    if (FileExists("./Cake.JMeter.Tests.dll.xml"))
        DeleteFile("./Cake.JMeter.Tests.dll.xml");
    if (FileExists("./Coverage.xml"))
        DeleteFile("./Coverage.xml");
});

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetCoreRestore(slnPath);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() => 
{
    var settings = new DotNetCoreBuildSettings {
        Configuration = configuration
    };
    DotNetCoreBuild(slnPath, settings);
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() => 
{
    var testSettings = new DotNetCoreTestSettings {
        Configuration = configuration,
        NoBuild = true,
        Logger = "xunit;LogFilePath=../../Cake.JMeter.Tests.dll.xml",
        //Verbosity = DotNetCoreVerbosity.Normal,
        ArgumentCustomization = args=>args.Append("-v=normal"), // Workaround until the Verbosity works correctly
    };
    OpenCover(tool => 
        tool.DotNetCoreTest("src/Cake.JMeter.Tests/Cake.JMeter.Tests.csproj", testSettings),
        "Coverage.xml",
        new OpenCoverSettings {
            ReturnTargetCodeOffset = 0,
            OldStyle = true // See https://github.com/OpenCover/opencover/issues/789
        }.WithFilter("+[*]* -[xunit.*]* -[*.Tests]*")
    );

    if (AppVeyor.IsRunningOnAppVeyor) 
    {
        AppVeyor.UploadTestResults("./Cake.JMeter.Tests.dll.xml", AppVeyorTestResultsType.XUnit);
        CoverallsIo("Coverage.xml", new CoverallsIoSettings()
        {
            RepoToken = coverallsToken
        });
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
        Authors      = new[] { "pitermarx", "augustoproiete" },
        Description  = "Cake aliases for JMeter. To be used in conjunction with the JMeter tool.",
        ProjectUrl   = new Uri("https://github.com/cake-contrib/Cake.JMeter"),
        LicenseUrl   = new Uri("https://github.com/cake-contrib/Cake.JMeter/blob/master/LICENSE"),
        Tags         = new [] { "cake", "jmeter", "cake-addin", "cake-build", "cake-contrib", "addin", "script", "build" },
        IconUrl      = new Uri("https://cdn.jsdelivr.net/gh/cake-contrib/graphics/png/addin/cake-contrib-addin-medium.png"),
        Files        = new [] { 
            new NuSpecContent { Source = "src/Cake.JMeter/bin/Release/netstandard2.0/Cake.JMeter.dll", Target = "lib\\netstandard2.0" },
            new NuSpecContent { Source = "src/Cake.JMeter/bin/Release/netstandard2.0/Cake.JMeter.xml", Target = "lib\\netstandard2.0" },
            new NuSpecContent { Source = "src/Cake.JMeter/bin/Release/netstandard2.0/Cake.JMeter.pdb", Target = "lib\\netstandard2.0" }
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
