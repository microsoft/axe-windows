# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
Builds a ZIP file that contains the Axe.Windows CLI. Output is named AxeWindowsCLI.zip

.PARAMETER SrcDir
The directory where the full install config was built

.PARAMETER TargetDir
The directory where the .zip file will be dropped. Don't put this under SrcDir or the zip file will be "nested" in subsequent runs

.Example Usage
.\BuildZippedCLI.ps1 -SrcDir c:\myrepo\src\CLI_Full\bin\release\netcoreapp3.1\win7-x86 -TargetDir c:\myrepo\src\CLI_Installer\bin\release
#>

param(
    [Parameter(Mandatory=$true)][string]$SrcDir,
    [Parameter(Mandatory=$true)][string]$TargetDir
)

Set-StrictMode -Version Latest
$script:ErrorActionPreference = 'Stop'

# Uncomment the next line for debugging
$VerbosePreference='continue'

# This suppresses the progress indicator while creating the zip file
$ProgressPreference='SilentlyContinue'

function Get-UniqueTempFolder([string] $app){
    Write-Verbose "        entering Get-UniqueTempFolder"
    Write-Verbose "          app = $app"

    $guid=New-Guid
    $uniqueName=$app + '_' + $guid
    $tempFolder=Join-Path $env:temp $uniqueName
    New-Item -Path $tempFolder -ItemType Directory

    Write-Verbose "        exiting Get-UniqueTempFolder"

    return $tempFolder
}

function Copy-Files([string]$src, [string]$pattern, [string]$dst) {
    Write-Verbose "              entering Copy-Files"
    Write-Verbose "                src = $src"
    Write-Verbose "                pattern = $pattern"
    Write-Verbose "                dst = $dst"

    [string]$srcWithPattern = Join-Path $src $pattern
    Write-Verbose "                Copying $srcWithPattern to $dst"

    Copy-Item $srcWithPattern $dst -Force

    Write-Verbose "              exiting Copy-Files"
}

function Remove-FilesByPattern([string]$path, [string]$pattern){
    Write-Verbose "                entering Remove-FilesByPattern"
    Write-Verbose "                  path = $path"
    Write-Verbose "                  pattern = $pattern"

    Get-ChildItem $path -File -Recurse -Include $pattern | Remove-Item

    Write-Verbose "                exiting Remove-FilesByPattern"
}

function Create-Archive([string]$src, [string]$dst){
    Write-Verbose "        entering Create-Archive"
    Write-Verbose "          src = $src"
    Write-Verbose "          dst = $dst"

    $from=$src + '\*'
    Compress-Archive -Force -Path $from -DestinationPath $dst

    Write-Verbose "    exiting Create-Archive"
}

function Add-ZipfileContents([string]$srcDir, [string]$scratch, [string]$patternToInclude, [string[]]$patternsToRemove){
    Write-Verbose "            entering Add-ZipfileContents"
    Write-Verbose "              srcDir = $srcDir"
    Write-Verbose "              scratch = $scratch"
    Write-Verbose "              patternToInclude = $patternToInclude"
    Write-Verbose "              patternsToRemove = $patternsToRemove"

    Copy-Files $srcDir $patternToInclude $scratch
    foreach ($pattern in $patternsToRemove) {
        Remove-FilesByPattern $scratch $pattern
    }

    Write-Verbose "            exiting Add-ZipfileContents"
}

function Prepare-Zipfile([string]$srcDir, [string]$scratch, [string[]]$patternsToRemove){
    Write-Verbose "        entering Prepare-Zipfile"
    Write-Verbose "          srcDir = $srcDir"
    Write-Verbose "          scratch = $scratch"
    Write-Verbose "          patternsToRemove = $patternsToRemove"

    Add-ZipfileContents $srcDir $scratch '*.*' $patternsToRemove

    Write-Verbose "        exiting Prepare-Zipfile"
}

function Create-Zipfile([string]$srcDir, [string]$app, [string]$zipFile, [string[]]$patternsToRemove){
    Write-Verbose "    entering Create-Zipfile"
    Write-Verbose "      srcDir = $srcDir"
    Write-Verbose "      app = $app"
    Write-Verbose "      zipFile = $zipFile"
    Write-Verbose "      patternsToRemove = $patternsToRemove"

    $scratch=(Get-UniqueTempFolder $app)[0]
    Write-Verbose "scratch = $scratch"

    Prepare-Zipfile $srcDir $scratch $patternsToRemove

    Create-Archive $scratch $zipFile

    Remove-Item $scratch -Force -Recurse

    Write-Verbose "    exiting Create-Zipfile"
}

function Create-CLIZip([string]$srcDir, [string]$targetDir, [string]$zipFileName){
    Write-Verbose "entering Create-CLIZip"
    Write-Verbose "  srcDir = $srcDir"
    Write-Verbose "  targetDir = $targetDir"
    Write-Verbose "  zipFileName = $zipFileName"

    $zipFile=Join-Path $targetDir $zipFileName
    $patternsToRemove=@('*.dev.json', '*.pdb')
    Write-Host "Creating $zipFile from files in $srcDir"
    Create-ZipFile $srcDir 'AxeWindowsCLI' $zipFile $patternsToRemove
    Write-Host 'Successfully Created' $zipFile

    Write-Verbose "exiting Create-CLIZip"
}

Create-CLIZip $SrcDir $TargetDir 'AxeWindowsCLI.zip'
exit 0
