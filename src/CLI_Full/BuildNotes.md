# Build notes for CLI_Full project

The `CLI_Full` project exists to gather the CLI into a self-contained format (.zip) where all necessary .NET Core runtime files are gathered by the compiler. We compile the code from the `CLI` project is used, but specify extra flags to create the self-contained version. We end up with two versions of `AxeWindowsCLI.exe`, which are not interchangeable due to compiler-generated differences inside the binaries.

Changes that impact _common functionality_ need to be made in the `CLI` project. changes that impact the generation of the self-contained version need to be made in the `CLI_Full` project.
