# Localization Testing Sample App

This is a test app used to test axe-windows localization. It starts up a sample app to test (by default, the WildLifeManager app in `../WildlifeManager/WildlifeManager.exe`), creates a scanner, scans the app, and prints localized results to the console. 

## How to run

You can run this app from the command line via `dotnet run`. If testing a released version of the `axe-windows` nuget package, this project can be run with no changes. If you want to run this app against an unreleased version of the axe-windows package, you can do so as follows:

1. Create a `packages` directory within `LocTestingSampleApp` and place the package (`.nupkg`) inside it.

2. Update the package reference in `LocTestingSampleApp.csproj` to the proper version of the package (i.e. replace `2.1.0` in `<PackageReference Include="Axe.Windows" Version="2.1.0" />`).

3. Create a `nuget.config` to point nuget to the newly created `packages` directory and pick up the local copy of the package. See example `nuget.config` content below:
```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
    <packageSources>
    <clear />
    <add key="LocalPackages" value=".\packages" />
    <add key="NuGet" value="https://api.nuget.org/v3/index.json" />
    </packageSources>
    <disabledPackageSources />
</configuration>
```

## Customizations

By default, this project tests against the `WildlifeManager` sample app, sets the language to french, and shows only a subset of results. These defaults can all be changed by editing the following fields in Program.cs:

### langCode

This string determines the language that the scan results will be in. For versions 2.1.0 and higher, the axe-windows package supports the following language codes: `cs`, `de`, `es`, `fr`, `it`, `ja`, `ko`, `pl`, `pl-BR`, `ru`, `tr`, `zh-Hans`, and `zh-Hant`.

### exePath

This string is a path to any `.exe` file to run a scan against.

### showFullResults

This boolean determines what format the results will be printed to the console in. When this is `true`, the full results will be printed in the `JSON` format, however these results can be quite long and confusing, since they include both items that are localized and items that are intentionally not localized (such as field/property names). When this is `false` the app will only print a subset of the results, all of which should be localized according to the language provided via `langCode`. This setting is ideal for spot testing localization.
