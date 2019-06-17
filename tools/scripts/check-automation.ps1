# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

#environment variables
$VerbosePreference='continue'
Clear-Host

$flavor=$args[0]
switch ($flavor)
{
  ''        {$flavor='debug'; break}
  'debug'   {$flavor='debug'; break}
  'release' {$flavor='release'; break}
  default   {Write-Host -separator '' 'Argument "' $($flavor)'" is invalid. Please specify debug (default) or release'; exit}
}

$appPath='..\..\..\..\tools\WildlifeManager\WildlifeManager.exe'
$rootPath=Join-Path '..\..\src\Automation\bin\' $flavor
$fullPath = Resolve-Path $rootPath
$outputPath = Join-Path $fullPath.Path 'AutomationCheck'

if (-Not (Test-Path (Join-Path $rootPath Axe.Windows.Automation.dll)))
{
  Write-Host 'Please build the' $flavor 'version of Axe.Windows.Automation before running this script'
  exit 1
}

Remove-Item $outputPath -Recurse -Force -ErrorAction Ignore | Out-Null
New-Item $outputPath -ItemType Directory | Out-Null

Push-Location
Set-Location $rootPath
Write-Verbose '------------------------'
Write-Verbose 'Importing Axe.Windows.Automation assembly'
Add-Type -Path .\Axe.Windows.Automation.dll

Write-Verbose '------------------------'
Write-Verbose 'Launching WildlifeManager'
Start-Process -FilePath $($appPath)
Start-Sleep 5
$procId=get-process WildlifeManager | select -expand id
Write-Verbose "WildlifeManager has processId of $($procId)"

Write-Verbose '------------------------'
Write-Verbose 'Creating config'
$outputFormat = [Axe.Windows.Automation.OutputFileFormat]::A11yTest
$config = [Axe.Windows.Automation.Config+Builder]::ForProcessId($procId).WithOutputDirectory($outputPath).WithOutputFileFormat($outputFormat).Build()

Write-Verbose '------------------------'
Write-Verbose 'Creating scanner'
$scanner =  [Axe.Windows.Automation.ScannerFactory]::CreateScanner($config)

Write-Verbose '------------------------'
Write-Verbose 'Invoking scan'
$result = $scanner.Scan();
Write-Verbose $($result)
Stop-Process -Id $procId

Pop-Location

if ($result.ErrorCount -eq 0)
{
  Write-Host '*** AUTOMATION FAILED: SCAN FOUND NO FAILING RESULTS ***' -ForegroundColor Red
  exit 1
}

if ($result.ErrorCount -ne $result.Errors.Count)
{
    Write-Host '*** AUTOMATION FAILED: INCORRECT ERROR COUNT ***' -ForegroundColor Red
    exit 2 
}

if ((Get-ChildItem $result.OutputFile.Item1).Length -eq 0)
{
  Write-Host '*** AUTOMATION FAILED: NO OUTPUT FILE GENERATED ***' -ForegroundColor Red
  exit 3
}

#keep these lines at the end of the script
Write-Host '*** AUTOMATION SUCCEEDED ***' -ForegroundColor Green
exit 0