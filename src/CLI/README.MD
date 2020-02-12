# Welcome to the Axe.Windows Command Line Interface (AxeWindowsCLI.exe)

This tool allows you to scan your application for accessibility issues from your currently existing scripting system.

## Running the tool
You invoke the scanner by running `AxeWindowsCLI.exe`. By default, this tool gets installed to `c:\users\Program Files (x86)\AxeWindowsCLI\1.0\AxeWindowsCLI.exe`, although you can install it into other locations if you wish.

If you run the tool without parameters, you'll be presented with the help screen:

```
AxeWindowsCLI.exe

  --processid                Process Id

  --processname              Process Name

  --outputdirectory          Output directory

  --scanid                   Scan ID

  --verbosity                Verbosity level (Quiet/Default/Verbose)

  --showthirdpartynotices    Display Third Party Notices (opens file in browser
                             without executing scan). If specified, all other
                             options will be ignored.

  --help                     Display this help screen.

  --version                  Display version information.
```

  To scan an application, you need run AxeWindowsCLI with the information to know _which_ process to scan via either the `--processId` or `--processName` parameters

### Command line parameters

Parameter|Description
|---|---|
|processId|Identifies the process ID of the application to be scanned. Must be specified as an integer value|
|processName|Identifies the name of the process to be scanned. Requires that the process to scan be the _only_ process of that name currently running on the system.|
|outputDirectory|Identifies the folder where output files will be created. If not specified, this will default to `.\AxeWindowsOutputFiles` (relative to the current working directory)|
|scanid|Identifies the specific ID of the scan. This allows you to preassign a name to the given scan (and output file). If omitted, an dynamic name in the format AxeWindows_YY-MM-DD_hh-mm-ss.fffffff will be used.|
|verbosity|Identifies the level of detail you want in the output. Valid values are `quiet` (minimal output), `default` (typical output), or `verbose` (maximum output).
|showThirdPartyNotices|If specified, displays the third party notices for components used by AxeWindowsCLI. This information is also available in the `thirdpartynotices.html` file that is installed with AxeWindowsCLI.

### Scan results
A summary of scan results will be displayed after the scan is run. In addition, an A11yTest file will be generated if 1 or more errors were detected. The location of this file will be reported in the tool output (see the documentation of the `--outputDirectory` and `--scanId` parameters for ways to alter the name or location of the output file). This file can then be opened with **Accessibility Insights for Windows**, which is freely available at http://accessibilityinsights.io

A detailed description of scan results will also be displayed if you specify verbose output (see the `verbosity` parameter).

## Troubleshooting

### Error message: A fatal error occurred. The required library hostfxr.dll could not be found.
AxeWindowsCLI requires the presence of the .NET Core runtime, version 3.0 or greater. You can download the framework from https://dotnet.microsoft.com/download/dotnet-core. This error should go away after installing an appropriate runtime of the .NET Core.

### Exception: Unable to locate root element
This can occur if you are trying to scan a process that is running in an elevated context. To get around it, try running your process non-elevated, or try launching the scan from an elevated process.
