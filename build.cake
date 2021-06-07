using System.Diagnostics;
using System.Text.RegularExpressions;

// Get runtime variables
var target              = Argument("target", "Default");
var branch              = Argument("branch", "undefined");
var build               = Argument("build", "0");
var pushToOctopus       = Argument("pushToOctopus", false);
var octopusUrl          = Argument("octopusUrl", string.Empty);
var octopusApiKey       = Argument("octopusApiKey", string.Empty);

// Settings
var applicationName     = "BjarteSanityDotNet";
var solutionFileName    = $"{applicationName}.sln";

var projectNames        =   new [] {
                                "Site"
                            };

var configuration       = "Release";
var framework           = "net6.0";
var runtime             = "win10-x64";

// Branch name might contain illegal characters, like "/". We also want to
// remove root branch names, like "refs" and "heads"
//
// Example: "refs/heads/feature/admin-login"
// This will be converted to "feature-admin-login"
branch = branch.Replace("refs/", "").Replace("heads/", "");
Regex regex = new Regex("[^a-zA-Z0-9-]");
branch = regex.Replace(branch, "-");

var version             = $"{DateTime.Now.Year}.{DateTime.Now.Month}.{DateTime.Now.Day}.{build}-{branch}";

Console.WriteLine("");
Console.WriteLine("### Variables used for build.cake:");
Console.WriteLine("");

Console.WriteLine($"Target:                 {target}");
Console.WriteLine($"Branch:                 {branch}");
Console.WriteLine($"Build:                  {build}");
Console.WriteLine($"Push to Octopus:        {pushToOctopus}");
Console.WriteLine($"Octopus url:            {octopusUrl}");
Console.WriteLine($"Octopus API key:        {octopusApiKey}");
Console.WriteLine("");

Console.WriteLine($"Application name:       {applicationName}");
Console.WriteLine($"Solution file name:     {solutionFileName}");
Console.WriteLine($"Project names:");
foreach(var projectName in projectNames) {
    Console.WriteLine($"                        - {projectName}");
}
Console.WriteLine("");

Console.WriteLine($"Configuration:          {configuration}");
Console.WriteLine($"Framework:              {framework}");
Console.WriteLine($"Runtime:                {runtime}");
Console.WriteLine("");
Console.WriteLine($"Version:                {version}");

Task("Clean")
    .Does(() =>
    {
        var dirsToClean = GetDirectories("./**/bin");
        dirsToClean.Add(GetDirectories("./**/obj"));

        foreach(var dir in dirsToClean) {
            Console.WriteLine(dir);
        }

        CleanDirectories(dirsToClean);
    });

Task("Test")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        GetFiles("./tests/**/*.csproj")
            .ToList()
            .ForEach(file => DotNetCoreTest(file.FullPath));
    });


Task("Publish")
    .IsDependentOn("Test")
    .Does(() =>
    {
        foreach(var projectName in projectNames) {

            DotNetCorePublish($"./src/{projectName}", new DotNetCorePublishSettings {
                Configuration = configuration,
                Framework = framework,
                Runtime = runtime,
                EnvironmentVariables = new Dictionary<string, string> {
                    { "version", version }
                }
            });

            Console.WriteLine("");
        }
    });


Task("OctoPack")
    .IsDependentOn("Publish")
    .Does(() =>
    {
        foreach(var projectName in projectNames) {

            var basePath = $"./src/{projectName}/bin/{configuration}/{framework}/{runtime}/publish";

            OctoPack($"{applicationName}.{projectName}", new OctopusPackSettings {
                BasePath = basePath,
                Format = OctopusPackFormat.Zip,
                Version = version,
                OutFolder = new DirectoryPath("./tools/")
            });

            Console.WriteLine("");
        }
    });

Task("OctoPush")
    .IsDependentOn("OctoPack")
    .WithCriteria(pushToOctopus)
    .WithCriteria(!string.IsNullOrEmpty(octopusUrl))
    .WithCriteria(!string.IsNullOrEmpty(octopusApiKey))
    .Does(() =>
    {
        foreach(var projectName in projectNames) {

            var packagePathCollection = new FilePathCollection(
                System.IO.Directory.GetFiles("./tools/", $"{applicationName}.{projectName}.*.zip").Select(filePath => new FilePath(filePath)),
                new PathComparer(false));

            OctoPush(
                octopusUrl,
                octopusApiKey,
                packagePathCollection,
                new OctopusPushSettings {
                    ReplaceExisting = true
                }
            );

            OctoCreateRelease(projectName, new CreateReleaseSettings {
                Server = octopusUrl,
                ApiKey = octopusApiKey,
                ReleaseNotes = $"Built with Azure DevOps: <a href=\"https://dev.azure.com/abc/xyz/_build/results?buildId={build}\">Build {build}</a>"
            });

            Console.WriteLine("");
        }
    });

Task("Default")
  .IsDependentOn("OctoPush");

RunTarget(target);
