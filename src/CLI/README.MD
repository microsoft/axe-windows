# Welcome to the Axe.Windows Command Line Interface (AxeWindowsCLI.exe)

This tool allows you to scan your application for accessibility issues from your currently existing scripting system.

## Installing the CLI

If you prefer to install silently, open a command prompt as administrator, change to the directory containing the file (typically your downloads folder), and execute the MSI file directly with the /quiet flag. Be sure to use the appropriate version number:

```
msiexec.exe /i AxeWindowsCLI-X.Y.Z.msi /quiet
```

## Running the tool
You invoke the scanner by running `AxeWindowsCLI.exe`. By default, this tool gets installed to `c:\Program Files (x86)\AxeWindowsCLI\<version>\AxeWindowsCLI.exe`, although you can install it into other locations if you wish.

If you run the tool without parameters, you'll be presented with the help screen. An example follows:

```
AxeWindowsCLI 2.2.1
Copyright c 2020

  --processid                 Process Id

  --processname               Process Name

  --outputdirectory           Output directory

  --scanid                    Scan ID

  --verbosity                 Verbosity level (Quiet/Default/Verbose)

  --showthirdpartynotices     Display Third Party Notices (opens file in browser
                              without executing scan). If specified, all other
                              options will be ignored.

  --delayinseconds            How many seconds to delay before triggering the
                              scan. Valid range is 0 to 60 seconds, with a
                              default of 0.

  --customuia                 The path to a configuration file specifying custom
                              UI Automation attributes

  --alwayssavetestfile        If specified, always save the test file. By
                              default, the test file is saved only if errors are
                              found.

  --testallchromiumcontent    If specified, causes Chromium HTML content to be
                              scanned. Chromium HTML content is ignored without
                              this flag. This is intended primarily for browser
                              development teams and is discouraged for other
                              scenarios.
```

To scan an application, you need to specify the application's process via either the `--processId` or `--processName` parameters

### Command line parameters

Parameter|Description
---|---
processId|Identifies the process ID of the application to be scanned. Must be specified as an integer value
processName|Identifies the name of the process to be scanned. Requires that the process to scan be the _only_ process of that name currently running on the system.
outputDirectory|Identifies the folder where output files will be created. If not specified, this will default to `.\AxeWindowsOutputFiles` (relative to the current working directory)
scanId|Identifies the specific ID of the scan. This allows you to preassign a name to the given scan (and output file). If omitted, an dynamic name in the format AxeWindows_YY-MM-DD_hh-mm-ss.fffffff will be used.
verbosity|Identifies the level of detail you want in the output. Valid values are `quiet` (minimal output), `default` (typical output), or `verbose` (maximum output).
showThirdPartyNotices|If specified, displays the third party notices for components used by AxeWindowsCLI. This information is also available in the `thirdpartynotices.html` file that is installed with AxeWindowsCLI.
delayInSeconds|Optionally inserts a delay before triggering the scan. This allows transient controls (menus, drop-down-lists, etc.) to be scanned.
customUia|Optionally provides a path to a [custom UIA configuration file](../../docs/CustomUIA.md). By default, only system-defined UIA properties will be included in the scan.
alwayssavetestfile|Optionally causes the test file to always be saved. By default, the test file is saved only if errors are found.
testAllChromiumContent|Optionally causes Chromium HTML content to be scanned. Chromium HTML content is ignored without this flag. This is intended primarily for browser development teams and is discouraged for other scenarios.

### Scan results
A summary of scan results will be displayed after the scan is run. In addition, an `.a11ytest` file will be generated if 1 or more errors were detected or if the `alwayssavetestfile` option was specified. The location of this file will be reported in the tool output (see the documentation of the `--outputDirectory` and `--scanId` parameters for ways to alter the name or location of the output file). This file can then be opened with **Accessibility Insights for Windows**, which is freely available at http://accessibilityinsights.io. If you scan an application with multiple top-level windows, an `.a11ytest` file will be generated for _each_ top-level window, even if no errors are detected.

A detailed description of scan results will also be displayed if you specify verbose output (see the `verbosity` parameter).

## Troubleshooting

### Error message: A fatal error occurred. The required library hostfxr.dll could not be found.
AxeWindowsCLI requires the presence of version 3.0 (or greater) of the .NET Core runtime. You can download the framework from https://dotnet.microsoft.com/download/dotnet-core. This error should go away after installing an appropriate runtime of the .NET Core.

### Exception: Unable to locate root element
This can occur if you are trying to scan a process that is running in an elevated context. To get around it, try running your process non-elevated, or try launching the scan from an elevated process.
