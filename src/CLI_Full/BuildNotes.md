# Build notes for CLI_Full project

The `CLI_Full` project exists to gather the CLI into a self-contained format (.zip) where all necessary .NET Core runtime files are gathered by the compiler. We compile the code from the `CLI` project is used, but specify extra flags to create the self-contained version. We end up with two versions of `AxeWindowsCLI.exe`, which are not interchangeable due to compiler-generated differences inside the binaries. The csproj file that builds CLI_Full actually _copies_ AxeWindowsCLI.dll as part of the build. This was added to ensure that resources are identical between the 2 flavors.

The _only_ changes to be made in the `CLI_Full` project are those change that impact the packaging of the zip file.
