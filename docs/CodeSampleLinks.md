<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Code Sample Links

We provide web-hosted documentation containing additional guidance on how to create a more accessible application. This documentation is indexed in [Axe.Windows.CodeSampleLinks.json](../Desktop/Resources/Axe.Windows.CodeSampleLinks.json), which is a JSON-serialized dictionary. This file is included in the NuGet package that provides the Axe.Windows binaries.

### Keys
Each key is formatted in 3 parts:
1. The Framework identifier
2. The Control type
3. The name of a value from the [`PropertyType` class](https://github.com/microsoft/axe-windows/blob/main/src/Core/Types/PropertyType.cs#L16), without the "UIA_" prefix or the "PropertyId" suffix.

The 3 parts are then contatenated using spear casing. For example, if the framework is `WinForm`, the control type is `ToolBar`, and the property is `UIA_NamePropertyId`, the key would be `WinForm-ToolBar-Name`. Not all properties are documented for all controls or frameworks.

### Values
Each value is a redirectable link that resolves to the current documentation for that topic. The redirectable links should remain unchanged, even if the resolved links may change as the documentation structure evolves and matures.
