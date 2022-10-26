# Copyright (c) Microsoft. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.
<#
.SYNOPSIS
Verifies that all .resx files in the target folder have comments for resource strings.

.PARAMETER Target
The target folder to search recursively.

.Example Usage 
.\ResourceStringCommentVerification.ps1 -Target 'C:\code\axe-windows\src'
#>

param(
    $Target
)

$NewLine=([Environment]::NewLine)
$FailedStrings = @()
# Exclude the projects that are not yet 
$excludeList="(Actions|Automation|CLI|Desktop|tools)"

function Get-FileText($pathToFile){
   return Get-Content $pathToFile -Raw -Encoding UTF8
}

# Remove resource file header/footers
function Get-ResourceContent($fileContent){
    $fileLines = $fileContent.Split($NewLine, [StringSplitOptions]::RemoveEmptyEntries)
    $headerIndexes = (0..($fileLines.Count-1)) | where {$fileLines[$_] -Match '</resheader>'}
    $startIndex = $headerIndexes[$headerIndexes.Count - 1] + 1
    $endIndex = $fileLines.Count - 2
    $resourceStrings = $fileLines[$startIndex..$endIndex]
    return $resourceStrings
}

(Get-ChildItem $Target\* -Include *.resx -Recurse) | Where {$_.FullName -notmatch $excludeList} | Foreach-Object {
    $path = $_.FullName
    $fileContent=Get-FileText $path
    $resourceStrings = Get-ResourceContent($fileContent)

    for($i=0; $i -lt $resourceStrings.Length; $i++) {
        if ($resourceStrings[$i] -Match "<data name=") {
            # Account for any multi-line resource values
            $j = $i + 1;
            while ($resourceStrings[$j] -NotMatch "</value>") {
                $j = $j + 1
            }

            # There should be a comment directly after the resource value
            if ($resourceStrings[$j + 1] -NotMatch "<comment>") {
                $match = $resourceStrings[$i] -Match "<data name=`"(?<content>[^;]+)`" xml:space=`"preserve`""
                $resourceName = $matches["content"]
                $FailedStrings += "$resourceName ($path)" 
            }
        }
    }

    echo "Finished scanning $path"
}

echo "$NewLine"
if($FailedStrings.Count -gt 0){
    $message = "The following resources do not have translator comments $NewLine" + ($FailedStrings -join $NewLine) + "$NewLine"
    throw ($message)
} else {
    echo "All string resources have translator comments"
}
