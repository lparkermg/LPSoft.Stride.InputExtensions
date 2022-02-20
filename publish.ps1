param(
    [string]$version,
    [string]$nugetKey
)

if(-not $version){
    $host.ui.WriteErrorLine('Version not provided, aborting!');
    exit 1;
}

if (-not $nugetKey){
    $host.ui.WriteErrorLine('NugetKey not provided, aborting!');
    exit 2;
}

.\build.ps1 $version


&dotnet nuget push .\dist\LPSoft.Stride.InputExtensions.BuildInputConfig.$version.nupkg --api-key $nugetKey --source https://api.nuget.org/v3/index.json