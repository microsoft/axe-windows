# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
Builds a ZIP file that contains the Axe.Windows CLI. Output is named AxeWindowsCLI.zip

.PARAMETER SrcDir
The directory where the full install config was built

.PARAMETER OtherSignedSrcDir
The directory where signed external dependencies reside

.PARAMETER TargetDir
The directory where the .zip file will be dropped. Don't put this under SrcDir or the zip file will be "nested" in subsequent runs

.Example Usage
.\BuildZippedCLI.ps1 -SrcDir c:\myrepo\src\CLI_Full\bin\release\netcoreapp3.1\win7-x86 -OtherSignedSrcDir c:\myrepo\src\CLI\bin\release\netcoreapp3.1 -TargetDir c:\myrepo\src\CLI_Installer\bin\release
#>

param(
    [Parameter(Mandatory=$true)][string]$SrcDir,
    [Parameter(Mandatory=$true)][string]$OtherSignedSrcDir,
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

function Add-ZipfileContents([string]$srcDir, [string]$scratch, [string]$patternToInclude, [string[]]$patternsToRemove){
    Write-Verbose "srcDir = $srcDir"
    Write-Verbose "scratch = $scratch"
    Write-Verbose "patternToInclude = $patternToInclude"
    Write-Verbose "patternsToRemove = $patternsToRemove"

    Copy-Files $srcDir $patternToInclude $scratch
    foreach ($pattern in $patternsToRemove) {
        Remove-FilesByPattern $scratch $pattern
    }
}

function Prepare-Zipfile([string]$srcDir, [string]$otherSignedSrcDir, [string]$scratch, [string[]]$patternsToRemove, [string[]]$otherSignedAssemblies){
    Write-Verbose "srcDir = $srcDir"
    Write-Verbose "otherSignedSrcDir = $otherSignedSrcDir"
    Write-Verbose "scratch = $scratch"
    Write-Verbose "patternsToRemove = $patternsToRemove"
    Write-Verbose "otherSignedAssemblies = $otherSignedAssemblies"
    
    Add-ZipfileContents $srcDir $scratch '*.*' $patternsToRemove + $otherSignedAssemblies

    foreach ($assembly in $otherSignedAssemblies) {
        Add-ZipfileContents $otherSignedSrcDir $scratch $assembly $patternsToRemove
    }
}

function Create-Zipfile([string]$srcDir, [string]$otherSignedSrcDir, [string]$app, [string]$zipFile, [string[]]$patternsToRemove, [string[]]$otherSignedAssemblies){
    Write-Verbose "srcDir = $srcDir"
    Write-Verbose "app = $app"
    Write-Verbose "zipFile = $zipFile"
    Write-Verbose "patternsToRemove = $patternsToRemove"

    $scratch=(Get-UniqueTempFolder $app)[0]
    Write-Verbose "scratch = $scratch"

    Prepare-Zipfile $srcDir $otherSignedSrcDir $scratch $patternsToRemove $otherSignedAssemblies

    Create-Archive $scratch $zipFile

    Remove-Item $scratch -Force -Recurse
}

function CreateCLIZip([string]$srcDir, [string]$otherSignedSrcDir, [string]$targetDir, [string]$zipFileName){
    $zipFile=Join-Path $targetDir $zipFileName
    $patternsToRemove=@('*.dev.json', '*.pdb')
    $otherSignedAssemblies=@('Newtonsoft.Json.dll', 'CommandLine.dll', 'Interop.UIAutomationCore.dll')
    Write-Host "Creating $zipFile from files in $srcDir"
    Create-ZipFile $srcDir $otherSignedSrcDir 'AxeWindowsCLI' $zipFile $patternsToRemove $otherSignedAssemblies
    Write-Host 'Successfully Created' $zipFile
}

CreateCLIZip $SrcDir $OtherSignedSrcDir $TargetDir 'AxeWindowsCLI.zip'
exit 0