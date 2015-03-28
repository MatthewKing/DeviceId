$toolsDirectory = '.\tools'
if (!(Test-Path -path $toolsDirectory )) { New-Item $toolsDirectory -Type Directory }

$nuget = '.\tools\nuget.exe'
if ((Test-Path $nuget) -eq $false) {
  Invoke-WebRequest -Uri http://nuget.org/nuget.exe -OutFile $nuget
}
Set-Alias nuget $nuget

$cake = '.\tools\Cake\Cake.exe'
if ((Test-Path $cake) -eq $false) {
  nuget install Cake -OutputDirectory $toolsDirectory -ExcludeVersion
}
Set-Alias cake $cake

cake build.cake
