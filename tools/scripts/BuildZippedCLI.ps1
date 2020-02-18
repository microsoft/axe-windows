# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
Builds a ZIP file that contains the Axe.Windows CLI. Output is named AxeWindowsCLI.zip

.PARAMETER Configuration
Debug or Release. Use $(ConfigurationName) if calling from a csproj file

.PARAMETER SrcDir
The directory where the full install config was built

.PARAMETER SignedSrcDir
The directory where the signed versions of AxeWindowsCLI.exe and AxeWindowsCLI.dll come from

.PARAMETER TargetDir
The directory where the .zip file will be dropped. Don't put this under SrcDir or the zip file will be "nested" in subsequent runs

.Example Usage
.\BuildZippedCLI.ps1 -Configuration release -SrcDir c:\myrepo\src\CLI_Full\bin\release\netcoreapp3.0\win7-x86 -SignedSrcDir c:\myrepo\src\CLI\bin\release\netcoreapp3.0 -TargetDir c:\myrepo\src\CLI_Installer\bin\release
#>

param(
    [Parameter(Mandatory=$true)][string]$Configuration,
    [Parameter(Mandatory=$true)][string]$SrcDir,
    [Parameter(Mandatory=$true)][string]$SignedSrcDir,
    [Parameter(Mandatory=$true)][string]$TargetDir
)

Set-StrictMode -Version Latest
$script:ErrorActionPreference = 'Stop'

# Uncomment the next line for debugging
#$VerbosePreference='continue'

# This suppresses the progress indicator while creating the zip file
$ProgressPreference='SilentlyContinue'

function Get-UniqueTempFolder([string] $app){
    $guid=New-Guid
    $uniqueName=$app + '_' + $guid
    $tempFolder=Join-Path $env:temp $uniqueName
	New-Item -Path $tempFolder -ItemType Directory
    return $tempFolder
}

function Copy-Files([string]$src, [string]$pattern, [string]$dst) {
    [string]$srcWithPattern = Join-Path $src $pattern
    Write-Verbose "Copying $srcWithPattern to $dst"

    Copy-Item $srcWithPattern $dst -Force
}

function Remove-FilesByPattern([string]$path, [string]$pattern){
    Get-ChildItem $path -File -Recurse -Include $pattern | Remove-Item
}

function Create-Archive([string]$src, [string]$dst){
    $from=$src + '\*'
    Compress-Archive -Force -Path $from -DestinationPath $dst
}

function Create-Zipfile([string]$srcDir, [string]$signedSrcDir, [string]$app, [string]$zipFile, [string[]]$patternsToRemove){
    Write-Verbose "srcDir = $srcDir"
    Write-Verbose "app = $app"
    Write-Verbose "zipFile = $zipFile"
    Write-Verbose "patternsToRemove = $patternsToRemove"

    $scratch=(Get-UniqueTempFolder $app)[0]
    Write-Verbose "scratch = $scratch"

    Copy-Files $srcDir '*.*' $scratch
    Copy-Files $signedSrcDir 'AxeWindowsCLI.exe' $scratch
    Copy-Files $signedSrcDir 'AxeWindowsCLI.dll' $scratch
    foreach ($pattern in $patternsToRemove) {
        Remove-FilesByPattern $scratch $pattern
    }
    Create-Archive $scratch $zipFile

    Remove-Item $scratch -Force -Recurse
}

function CreateCLIZip([string]$configuration, [string]$srcDir, [string]$signedSrcDir, [string]$targetDir, [string]$zipFileName){
    Write-Host "Creating an AxeWindowsCLI zip file from files in $srcDir, signed files from $signedSrcDir"
	$patternsToRemove=@('*.dev.json', '*.pdb')
    $zipFile=Join-Path $targetDir $zipFileName
    Create-ZipFile $srcDir $signedSrcDir 'AxeWindowsCLI' $zipFile $patternsToRemove
    Write-Host 'Successfully Created' $zipFile
}

CreateCLIZip $Configuration $SrcDir $SignedSrcDir $TargetDir 'AxeWindowsCLI.zip'
exit 0
