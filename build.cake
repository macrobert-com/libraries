using System.Text.RegularExpressions;

// Define constants
var solutionFile = "./libraries.sln"; // Replace with your solution name
var artifactsPath = "./artifacts";
var configuration = Argument("configuration", "Release");
var testResultsFile = $"C:/temp/test_results_{DateTime.UtcNow:yyyyMMdd_HHmmss}.trx";

// Read API keys and BaGet server URL from environment variables
var nugetApiKey = EnvironmentVariable("NUGET_API_KEY");
var bagetApiKey = EnvironmentVariable("BAGET_API_KEY");
var bagetServerUrl = EnvironmentVariable("BAGET_SERVER_URL");

if (string.IsNullOrWhiteSpace(nugetApiKey))
{
    throw new CakeException("Environment variable NUGET_API_KEY is not set. Please define it.");
}

if (string.IsNullOrWhiteSpace(bagetApiKey))
{
    throw new CakeException("Environment variable BAGET_API_KEY is not set. Please define it.");
}

if (string.IsNullOrWhiteSpace(bagetServerUrl))
{
    throw new CakeException("Environment variable BAGET_SERVER_URL is not set. Please define it.");
}

// Argument for selecting the NuGet server
var targetServerRegex = new Regex("none|nuget|baget", RegexOptions.IgnoreCase);
var targetServer = Argument("target-server", "none"); // Options: "nuget", "baget"

if (!targetServerRegex.IsMatch(targetServer))
{
    throw new CakeException("target-server must be one of nuget|baget");
}

// Define targets
Task("Clean")
    .Does(() =>
{
// Use GetFiles to match the files/directories you want to clean
var directoriesToClean = 
    GetDirectories("./src/**/bin")
    .Concat(GetDirectories("./src/**/obj"))
    .Concat(GetDirectories("./test/**/bin"))
    .Concat(GetDirectories("./test/**/obj"));

    foreach (var dir in directoriesToClean)
    {
        CleanDirectory(dir);
    }

    // Ensure the artifacts folder is clean also
    CleanDirectory(artifactsPath);
});


Task("Restore")
    .Does(() =>
{
    DotNetRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
{
    DotNetBuild(solutionFile, new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest(solutionFile, new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

Task("Pack")
    .IsDependentOn("Test")
    .Does(() => 
{
    // Pack all projects in the src folder
    var projects = GetFiles("./src/**/*.csproj");
    foreach (var project in projects)
    {
        DotNetPack(project.FullPath, new DotNetPackSettings 
        {
            Configuration = configuration,
            OutputDirectory = artifactsPath,
            IncludeSymbols = true,
            SymbolPackageFormat = "snupkg" // Generate .snupkg files
        });
    }
});

Task("Push")
    .IsDependentOn("Pack")
    .Does(() => 
{
    // Ensure a server is selected
    if (targetServer == "none")
    {
        throw new ArgumentException("You must specify a target server using '--target-server=[nuget|baget]'");
    }

    var apiKey = targetServer == "nuget" ? nugetApiKey : bagetApiKey;
    var source = targetServer == "nuget" ? "https://api.nuget.org/v3/index.json" : bagetServerUrl;

    var pushSettings = new DotNetNuGetPushSettings 
    {
        ApiKey = apiKey,
        Source = source,
        SymbolSource = source,
        SkipDuplicate = true
    };

    var packages = GetFiles($"{artifactsPath}/*.nupkg");
    foreach (var package in packages)
    {
       DotNetNuGetPush(package.FullPath, pushSettings);
    }    
    var symbols = GetFiles($"{artifactsPath}/*.snupkg");
    foreach (var symbolPackage in symbols)
    {
       DotNetNuGetPush(symbolPackage.FullPath, pushSettings);
    }
});

// Default target
Task("Default")
    .IsDependentOn("Test");

// Define the target chain
//Task("BuildAndPush")
//    .IsDependentOn("Push");

// Run the default target
RunTarget(Argument("target", "Default"));
