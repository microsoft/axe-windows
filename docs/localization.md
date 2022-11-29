<!-- Copyright (c) Microsoft Corporation. All rights reserved. 
     Licensed under the MIT License. --> 

## axe-windows localization

The axe-windows package has been partially localized from version 2.1.0 on. Note that not all package content is localized, only the strings that are user facing (such as rules descriptions and conditions). Error messages and strings not contained in the axe-windows nuget package (such as those associated with the CLI) are not localized.

### Localization Process

Translations for localizable strings are checked into this repo in the `.LCL` files in `../src/loc/lcl/*`. These translations include many technical UI Automation terms which should be translated in accordance with the translated versions of the [UIA docs pages](https://learn.microsoft.com/dotnet/framework/ui-automation/). The translation files are used in our build process to create localized resource `.dll`s alongside each localized assembly, which are eventually signed and bundled into the axe-windows package.

### Testing Localized Packages

Localized versions of the axe-windows package can be tested using the localization testing app located in the [tools directory](../tools/LocTestingSampleApp). See the app's [README](../tools/LocTestingSampleApp/README.md) for details.
