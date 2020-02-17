# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
Builds a ZIP file that contains the Axe.Windows CLI. Output is named AxeWindowsCLI.zip

.PARAMETER Configuration
Debug or Release. Will retain PDB files in the ZIP file if DEBUG. Use $(ConfigurationName) if calling from a csproj file

.PARAMETER FullTargetDir
The directory where the full files were built.

.PARAMETER SignedTargetDir
The directory where the signed CLI files were built.

.PARAMETER OutputDir
The directory where the .zip file will be dropped. Don't put this under FullTargetDir or the zip file will be "nested" in subsequent runs

.Example Usage
.\BuildZippedCLI.ps1 -Configuration release -FullTargetDir c:\myrepo\src\CLI_Full\bin\release\netcoreapp3.0\win7-x86 -SignedTargetDir c:\myrepo\src\CLI\bin\release\netcoreapp3.0 -OutputDir c:\myrepo\src\CLI_Installer\bin\release
#>

param(
    [Parameter(Mandatory=$true)][string]$Configuration,
    [Parameter(Mandatory=$true)][string]$FullTargetDir,
    [Parameter(Mandatory=$true)][string]$SignedTargetDir,
    [Parameter(Mandatory=$true)][string]$OutputDir
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

function Create-Zipfile([string]$fullTargetSrc, [string]$signedTargetSrc, [string]$app, [string]$zipFile, [string[]]$patternsToPrune){
    Write-Verbose "fullTargetSrc = $fullTargetSrc"
    Write-Verbose "signedTargetSrc = $signedTargetSrc"
    Write-Verbose "app = $app"
    Write-Verbose "zipFile = $zipFile"
    Write-Verbose "patternsToPrune = $patternsToPrune"

    $scratch=(Get-UniqueTempFolder $app)[0]
    Write-Verbose "scratch = $scratch"

    Copy-Files $fullTargetSrc '*.*' $scratch
    Copy-Files $signedTargetSrc ($app + '.*') $scratch
    foreach ($pattern in $patternsToPrune) {
        Remove-FilesByPattern $scratch $pattern
    }
    Create-Archive $scratch $zipFile

    Remove-Item $scratch -Force -Recurse
}

function CreateCLIZip([string]$configuration, [string]$fullTargetDir, [string]$signedTargetDir, [string]$outputDir, [string]$zipFileName){
    Write-Host "Creating an AxeWindowsCLI zip file from files in $fullTargetDir and $signedTargetDir"
	$patternsToPrune=@('AxeWindowsCLIPlaceholder.*', '*.dev.json', '*.pdb')
    $zipFile=Join-Path $outputDir $zipFileName
    Create-ZipFile $fullTargetDir $signedTargetDir 'AxeWindowsCLI' $zipFile $patternsToPrune
    Write-Host 'Successfully Created' $zipFile
}

CreateCLIZip $Configuration $FullTargetDir $SignedTargetDir $OutputDir 'AxeWindowsCLI.zip'
exit 0
