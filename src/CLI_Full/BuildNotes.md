# Build notes for CLI_Full project

The `CLI_Full` project exists only to package the CLI into a format where all dependent files of the assembly are gathered by the compiler. We compile the code from the `CLI` project. It can be used as-is for test purposes, but any released version should use the signed binaries (`AxeWindowsCLI.exe` and `AxeWindowsCLI.dll`) that are built from the CLI project. This substitution will be handled by the script that assembles the zip file for release.
