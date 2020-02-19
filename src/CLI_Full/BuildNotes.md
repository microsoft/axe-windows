# Build notes for CLI_Full project

The `CLI_Full` project exists to package the CLI into a format where all dependent files of the assembly are gathered by the compiler. We compile the code from the `CLI` project, but with additional project flags. This results in changes inside `AxeWindowsCLI.exe` that make the executables from the 2 projects non-interchangeable. Changes to code need to be made in the `CLI` project--changes in teh `CLI_Full` project should be limited to settings that are specific to building the self-contained version.
