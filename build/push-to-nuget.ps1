$root = Resolve-Path (Join-Path $PSScriptRoot "..")
$output = "$root/artifacts"
foreach ($package in Get-ChildItem -Path $output -Filter "*.nupkg") {
    nuget push $package.FullName -Source https://api.nuget.org/v3/index.json
}
