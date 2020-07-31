<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->
     
## Adding a new project

### All project types

1. Add your new project to the Axe.Windows Solution (`src\AxeWindows.sln`).
2. Before creating a pull request, verify that Visual Studio can successfully load and build the entire solution in both Debug and Release.
3. If your project requires NuGet dependencies which need to be installed alongside it, update the <dependencies> section of `./src/ci/axe.windows.nuspec`.

#### For *production (not test)* .NET Standard projects

1. Use src\Core\Core.csproj as your template. Remove all ItemGroup blocks except the one that includes the analyzers.
2. Update the `AssemblyName` and `RootNamespace` entries to match your new project.
3. Add the new project to the assembly.
4. Add your files in Visual Studio

#### For *test* .NET Standard projects

1. Use src\CoreTests\CoreTests.csproj as your template. Remove all ItemGroup blocks except the one that includes the test adapters, test framework, Moq, etc.
2. Add the new project to the assembly.
3. Add your files in Visual Studio
