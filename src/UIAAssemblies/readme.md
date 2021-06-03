## Information on UIA Assemblies
Axe.Windows uses 2 interop assemblies, both of which get embedded into our binary files:

### Interop.UIAutomationClient.dll
This assembly contains features that may change with updated versions of Windows and will periodically need to be updated to take advantage of new UIA features. When this happens, do the following:

1. Obtain the updated version of UIAutomationCore.dll. This file lives in the windows\system32 folder of your windows installation.
2. From an administrative command prompt, execute the following command:
```
   tlbimp UIAutomationCore.dll /namespace:UIAutomationClient /out:Interop.UIAutomationClient.dll
```
3. Create a new folder under `src/UIAAssemblies` for the new Windows version
4. Copy Interop.UIAutomationClient.dll to the new folder
5. Do a global search & replace to update `UIAAssemblies\Win10.XXXXX\` to `UIAAssemblies\Win10.YYYYY\` in the csproj files. As of this writing, this will touch the following projects:

   * Actions.csproj
   * Desktop.csproj
   * Rules.csproj
   * RulesTests.csproj
  
6. Delete the folder containing the old interop file
7. Build Axe.Windows
8. Use the CLI to ensure that the new interop is working as expected.

### Interop.UIAutomationCore.dll
This assembly contains features used in conjunction with custom UIA support and will rarely change. If this happens, do the following:

1. Work with the owners of https://github.com/TestStack/uia-custom-pattern-managed to make the necessary update and create a new release.
2. Create a test project (outside of the Axe.Windows solution) that contains a NuGet reference to [TestStack.UiaCustomPattersManaged](https://www.nuget.org/packages/TestStack.UiaCustomPattersManaged/) (note that there's a typo in the package name).
3. Build your test project.
4. Copy `Interop.UIAUtomationCore.dll` from your test project and overwrite the committed assembly in the Axe.Windows repo.
5. Build Axe.Windows
6. Use the CLI (using custom properties when they're enabled) to ensure that the new interop is working as expected.

In an ideal world, we would just consume this assembly directly from the NuGet package, but this package includes several additional test assemblies (and several transitive dependencies) that we don't want to include in Axe.Windows.
