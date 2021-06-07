<#
	PS> ./build.ps1 -target Default -build 1 -branch "main" -octopusUrl https://bjarte.octopus.app/ -octopusApiKey API-PUAKS7R0AHTVPYO2PPKBSDDUO6J6VIG -pushToOctopus $True



    ### How to use

    # Build locally
    PS> ./build.ps1 -target Default -build 123 -branch "feature/my-nice-feature" -octopusUrl https://myoctopus.com -octopusApiKey xyz

    # Build with TeamCity PowerShell runner or Azure DevOps PowerShell task

    # Example setup for Azure DevOps PowerShell task with arguments

    Script Path:
    build.ps1

    Arguments:
    -target Default -build $(Build.BuildNumber) -branch "$(Build.SourceBranch)" -octopusUrl https://myoctopus.com -octopusApiKey xyz -pushToOctopus $True
#>

[CmdletBinding()]
Param(
    [string]$target = "Default",
    [string]$branch = "undefined",
    [string]$build = "0",
    [string]$octopusUrl = "x",
    [string]$octopusApiKey = "x",
    [boolean]$pushToOctopus = $False
)

# Get runtime variables
Write-Output("")
Write-Output("### Variables used for build.ps1:")
Write-Output("")

Write-Output("Target:                 $target")
Write-Output("Branch:                 $branch")
Write-Output("Build:                  $build")
Write-Output("Push to Octopus:        $pushToOctopus")
Write-Output("Octopus url:            $octopusUrl")
Write-Output("Octopus API key:        $octopusApiKey")
Write-Output("")

$ErrorActionPreference = 'Stop'

Set-Location -LiteralPath $PSScriptRoot

$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = '1'
$env:DOTNET_CLI_TELEMETRY_OPTOUT = '1'
$env:DOTNET_NOLOGO = '1'

dotnet new tool-manifest
dotnet tool install Cake.Tool --global --version 1.1.0
dotnet tool install Octopus.DotNet.Cli --global --version 4.39.1
dotnet tool restore

if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet cake --target="$target" --branch="$branch" --build="$build" --pushToOctopus="$pushToOctopus" --octopusUrl="$octopusUrl" --octopusApiKey="$octopusApiKey" --settings_skippackageversioncheck=true

if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
