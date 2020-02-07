# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
Builds a ZIP file that contains the Axe.Windows CLI. Output is named AxeWindowsCLI.zip

.PARAMETER Configuration
Debug or Release. Will retain PDB files in the ZIP file if DEBUG. Use $(ConfigurationName) if calling from a csproj file

.PARAMETER TargetDir
The directory where the files were built (and where the ZIP file will be dropped). Use $(TargetDir) if calling from a csproj file

.Example Usage
.\BuildZippedCLI.ps1 -Configuration release -TargetDir c:\myrepo\src\CLI\bin\release\netcoreapp3.0
#>

param(
    $Configuration,
    $TargetDir
)

# Uncomment the next line for debugging
#$VerbosePreference='continue'

# This suppresses the progress indicator while creating the zip file
$ProgressPreference='SilentlyContinue'

Clear-Host

function Get-UniqueTempFolder($baseName){
    $guid=New-Guid
    $uniqueName=$baseName + '_' + $guid
    $tempFolder=Join-Path $env:temp $uniqueName
    return $tempFolder
}

function Copy-SourceTree($src, $dst){
    Copy-Item $src $dst -force -recurse >$null
}

function Remove-FilesByExtension($path, $extension){
    $pattern='*.' + $extension
    Get-ChildItem $path -File -Recurse -Include $pattern | Remove-Item
}

function Remove-FoldersByName($path, $name){
    Get-ChildItem $path -Directory -Recurse -Include $name | Remove-Item -Force -Recurse
}

function Create-Archive($src, $dst){
    $from=$src + '\*'
    Compress-Archive -Force -Path $from -DestinationPath $dst
}

function Create-Zipfile($src, $app, $zipFile, [string[]]$extensionsToPrune, [string[]]$foldersToPrune){
    Write-Verbose "src = $src"
    Write-Verbose "app = $app"
    Write-Verbose "zipFile = $zipFile"
    Write-Verbose "extensionsToPrune = $extensionsToPrune"
    Write-Verbose "foldersToPrune = $foldersToPrune"

    $scratch=Get-UniqueTempFolder $app
    Write-Verbose "$scratch = $scratch"
    Copy-SourceTree $src $scratch
    foreach ($extension in $extensionsToPrune) {
        Remove-FilesByExtension $scratch $extension
    }
    foreach ($folder in $foldersToPrune) {
        Remove-FoldersByName $scratch $folder
    }
    Create-Archive $scratch $zipFile

    Remove-Item $scratch -Force -Recurse
}

function CreateCLIZip($confguration, $targetDir, $zipFileName){
    Write-Host "Creating a CLI zip file from files in $targetDir"
    if ($confguration -eq 'debug') {
        $extensionsToPrune=@('dev.json')
    } else {
        $extensionsToPrune=@('pdb', 'dev.json')
    }
    $foldersToPrune=@('unix')
    $zipFile=Join-Path $targetDir $zipFileName
    Create-ZipFile $targetDir 'CLI' $zipFile $extensionsToPrune $foldersToPrune
    Write-Host 'Successfully Created' $zipFile
}

if ($TargetDir -eq $null) {
    Write-Host 'TargetDir is a required parameter'
    exit 1
}

if ($Configuration -eq $null) {
    Write-Host 'Configuration is a required parameter'
    exit 1
}

CreateCLIZip $Configuration $TargetDir 'AxeWindowsCLI.zip'
exit 0
