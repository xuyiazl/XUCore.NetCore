. ".\common.ps1"

Set-Location $packFolder

# Get the version
[xml]$commonPropsXml = Get-Content (Join-Path $slnPath "common.props")
$version = $commonPropsXml.Project.PropertyGroup.Version

# Publish all packages
foreach($project in $projects) {
    $projectName = $project.Substring($project.LastIndexOf("/") + 1)
    & dotnet nuget push ($projectName + "." + $version + ".nupkg") -s https://api.nuget.org/v3/index.json -k "API_KEY"
}

# Go back to the pack folder
Set-Location $slnPath
