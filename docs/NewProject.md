<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->
     
## Adding a new project
Do the following when adding a new project:

### For all projects
1. Add a project to the Axe.Windows Solution (`src\axe.windows.sln`).
2. Right-click on the project and select Properties.
3. In the Application tab, configure "Target Framework" to use the same .NET Framework version used by the `axe.windows.core` project.
   - Currently .NET Framework 4.7.1 is the target. 
4. In the build tab, set the following for both Debug and Release configurations:
   1. "Warning level" to 4.
   2. "Treat warnings as errors" to "All".
5. Close the solution and use your text editor to add the following line to your `.csproj` file to properly configure [Strong Naming](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/strong-named-assemblies) for your assembly:<br>
   `<Import Project="..\..\build\delaysign.targets" />`

### For *non-test* projects only
1. Add the following NuGet package in Visual Studio to enable code analysis:
   ```
   Microsoft.CodeAnalysis.FxCopAnalyzers
   ```
2. Close the solution and use your text editor to make the following changes to your `.csproj` file to properly configure the version and signing options:
   1. Add the following line with the other `.cs` files):<br>
   `<Compile Include="$(TEMP)\AxeWindowsVersionInfo.cs" />`
2. Use your text editor to make the following changes to your project's `Properties\AssemblyInfo` file to set the correct version:
   1. Remove the following lines from the file, as well as the commented lines above them: <br>
      ```
      [assembly: AssemblyVersion("1.0.*")]
      [assembly: AssemblyFileVersion("1.0.0.0")]
      ```
3. Verify that Visual Studio can successfully load and build the entire solution in both Debug and Release.
4. If your project requires NuGet dependencies which need to be installed along side it, update the <dependencies> section of `./src/ci/axe.windows.nuspec`.

### For *test* projects only
1. Verify that Visual Studio can successfully load and build the entire solution in both Debug and Release. Ensure that the tests are appropriately discovered in the test explorer, and that they all run successfully.
