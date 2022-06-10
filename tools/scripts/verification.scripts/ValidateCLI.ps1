# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

<#
.SYNOPSIS
Automates some CLI checks to streamline release validation. Run from its location in the file system.

.Example Usage
.\ValidateCLI.ps1
#>

function ValidateExitCode($actualCode, $expectedCode)
{
	if ($expectedCode -ne $actualCode)
	{
		Write-Host 'WRONG ERROR CODE! Received' $actualCode 'instead of' $expectedCode
		Exit 255
	}
}

function PromptAndWait($prompt)
{
    Write-Host '--------------------------------------------'
    Write-Host $prompt
    Write-Host 'Press ENTER to continue'
    Write-Host '--------------------------------------------'
    Read-Host
}

# Const values
$ScanCompletedAndFoundNoErrors = 0
$ScanCompletedAndFoundErrors = 1
$ScanFailedToComplete = 2
$ThirdPartyNoticesDisplayed = 3
$BadInputParameters = 255

Clear
PromptAndWait 'This script assumes that you are running from the scripts folder and that you have built a release version of the CLI.'

# Note: I tried unsuccessfully to find a way to parameterize the command line and still get the exit code.
# If you can find a way to make this work, then please refactor this code!

Clear
../../../src/cli/bin/Release/netcoreapp3.1/AxeWindowsCLI.exe
ValidateExitCode $lastExitCode $BadInputParameters
PromptAndWait 'The scenario should have displayed help text and thrown no exceptions'

Clear
../../../src/cli/bin/Release/netcoreapp3.1/AxeWindowsCLI.exe --UndefinedParameter
ValidateExitCode $lastExitCode $BadInputParameters
PromptAndWait "The scenario should have identified 'UndefinedParameter' as an unknown `nparameter, shown the help text, and thrown no exceptions"

Clear
../../../src/cli/bin/Release/netcoreapp3.1/AxeWindowsCLI.exe --ProcessName
ValidateExitCode $lastExitCode $BadInputParameters
PromptAndWait "The scenario should have indicated that the user needs to specify either `nprocessId or processName, and thrown no exceptions"

Clear
../../../src/cli/bin/Release/netcoreapp3.1/AxeWindowsCLI.exe --ProcessName ThisProcessDoesNotExist
ValidateExitCode $lastExitCode $BadInputParameters
PromptAndWait "The scenario should have indicated that it could not find a process named `nThisProcessDoesNotExist, and thrown no exceptions"

Clear
../../../src/cli/bin/Release/netcoreapp3.1/AxeWindowsCLI.exe --showthirdpartynotices
ValidateExitCode $lastExitCode $ThirdPartyNoticesDisplayed
PromptAndWait "The scenario should have opened the placeholder ThirdPartyNotices.html file `nin the default browser, and thrown no exceptions."

