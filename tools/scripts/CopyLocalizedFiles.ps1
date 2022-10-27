# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
Copies MicroBuild-generated localized files from a solution to their .NET equivalents in TargetDir.

.PARAMETER SrcDir
The directory of the .sln file. Localized projects will be auto-discovered

.PARAMETER TargetDir
The base directory where the localized files shoudld be copied. Locale folders will be create as children of this driectory.

.Example Usage
.\CopyLocalizedFiles.ps1 -SrcDir c:\myrepo\src -TargetDir c:\myrepo\src\CI\bin\release
#>

param(
    [Parameter(Mandatory=$true)][string]$SrcDir,
    [Parameter(Mandatory=$true)][string]$TargetDir
)

Set-StrictMode -Version Latest
$script:ErrorActionPreference = 'Stop'

# Uncomment the next line for debugging
#$VerbosePreference='continue'

# This suppresses the progress indicator while creating the zip file
$ProgressPreference='SilentlyContinue'

function EnsureDirectoryExists([string]$targetDir){
    Write-Verbose "entering EnsureDirectoryExists"
    Write-Verbose "  targetDir = $targetDir"

    if(Get-Item -Path $targetDir -ErrorAction Ignore){
        Write-Verbose "$targetDir already exists"
    } else {
        Write-Verbose "Creating $targetDir"
        New-Item $targetDir -ItemType Directory
    }
    Write-Verbose "exiting EnsureDirectoryExists"
}

function CopyResourceAssembliesForProjectLanguage([string]$srcDir, [string]$targetDir){
    Write-Verbose "entering CopyResourceAssembliesForProjectLanguage"
    Write-Verbose "  srcDir = $srcDir"
    Write-Verbose "  targetDir = $targetDir"

    $path = Join-Path $srcDir '*'
    Copy-Item -Path $path -Destination $targetDir -Filter '*.resources.dll'
    Write-Verbose "exiting CopyResourceAssembliesForProjectLanguage"
}

function CopyResourceAssembliesForProject([string]$srcDir, [string]$targetDir, [hashtable]$folderMap){
    Write-Verbose "entering CopyResourceAssembliesForProject"
    Write-Verbose "  srcDir = $srcDir"
    Write-Verbose "  targetDir = $targetDir"

    @($folderMap.GetEnumerator() |ForEach-Object {
        $localizedSrcDir = Join-Path -Path $srcDir -ChildPath $($_.Key)
        $localizedTargetDir = Join-Path -Path $targetDir -ChildPath $($_.Value)

        Write-Verbose "Copying $localizedSrcDir to $localizedTargetDir"
        EnsureDirectoryExists $localizedTargetDir
        CopyResourceAssembliesForProjectLanguage $localizedSrcDir $localizedTargetDir
        Write-Verbose "------ end of language ---------"
    })

    Write-Verbose "exiting CopyResourceAssembliesForProject"
}

function CopyResourceAssembliesForSolution([string]$srcDir, [string]$targetDir, [hashtable]$folderMap){
    Write-Verbose "entering CopyResourceAssembliesForSolution"
    Write-Verbose "  srcDir = $srcDir"
    Write-Verbose "  targetDir = $targetDir"

    Get-ChildItem -Path $srcDir -Directory |ForEach-Object {
        $localizedFilesFolder = Join-Path $_.FullName 'bin\Release\netstandard2.0\localize'
        if (Test-Path -Path $localizedFilesFolder) {
            CopyResourceAssembliesForProject $localizedFilesFolder $targetDir $folderMap
        }
    }

    Write-Verbose "exiting CopyResourceAssembliesForSolution"
}

# ENU is not in this map because it's the fallback language of the assemblies
$FolderMap = @{
    'CHS' = 'zh-Hans'
    'CHT' = 'zh-Hant'
    'CSY' = 'cs'
    'DEU' = 'de'
    'ESN' = 'es'
    'FRA' = 'fr'
    'ITA' = 'it'
    'JPN' = 'ja'
    'KOR' = 'ko'
    'PLK' = 'pl'
    'PTB' = 'pt-BR'
    'RUS' = 'ru'
    'TRK' = 'tr'
}

CopyResourceAssembliesForSolution $SrcDir $TargetDir $FolderMap

exit 0
