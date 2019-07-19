<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->
     
## Adding a new project

### All project types

1. Add your new project to the Axe.Windows Solution (`src\AxeWindows.sln`).
2. Before creating a pull request, verify that Visual Studio can successfully load and build the entire solution in both Debug and Release.
3. If your project requires NuGet dependencies which need to be installed along side it, update the <dependencies> section of `./src/ci/axe.windows.nuspec`.

### For all .Net Framework projects

1. Right-click on the project and select Properties.
2. In the Application tab, configure "Target Framework" to use the same .NET Framework version used by the `axe.windows.core` project, currently .NET Framework 4.7.1.
3. In the build tab, set the following for both Debug and Release configurations:
   1. "Warning level" to 4.
   2. "Treat warnings as errors" to "All".

#### For *production (not test)* .Net Framework projects

1. Add the following NuGet packages in Visual Studio to enable signing and code analysis:<br>
   ```
   Microsoft.VisualStudioEng.MicroBuild.Core
   Microsoft.CodeAnalysis.FxCopAnalyzers
   ```
2. Close the solution and use your text editor to make the following changes to your `.csproj` file to properly configure the version and signing options:
   1. Add the following line along side the existing `<Compile>` statements:<br>
   `<Compile Include="$(TEMP)\AxeWindowsVersionInfo.cs" />`
   2. Add the following below the last ItemGroup:<br>
   ```
   <ItemGroup>
      <DropSignedFile Include="$(TargetPath)" />
   </ItemGroup>
   <Import Project="..\..\build\settings.targets" />
   ```
3. Use your text editor to make the following changes to your project's `Properties\AssemblyInfo` file to set the correct version:
   1. Remove the following lines, as well as the commented lines above them: <br>
      ```
      [assembly: AssemblyVersion("1.0.*")]
      [assembly: AssemblyFileVersion("1.0.0.0")]
      ```

#### For *test* .Net Framework projects

1. Close the solution and use your text editor to add the following line to your `.csproj` file to properly configure [Strong Naming](https://docs.microsoft.com/en-us/dotnet/framework/app-domains/strong-named-assemblies) for your assembly:<br>
   `<Import Project="..\..\build\delaysign.targets" />`
2. Build the entire solution in both Debug and Release. Ensure that the tests are appropriately discovered in the test explorer, and that they all run successfully.

### .Net Standard and .Net Core projects

#### For *production (not test)* .Net Standard/Core projects

In a text editor, add the following line:<br>
`<Import Project="..\..\build\NetStandardRelease.targets" />`

#### For *test* .Net Standard/Core projects

In a text editor, add the following line:<br>
`<Import Project="..\..\build\NetStandardTest.targets" />`
