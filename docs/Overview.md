<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->
     
## Overview of Axe.Windows

### Windows UI Automation

Windows UI Automation (UIA) is the platform-provided way for accessibility tools to interact with programs. A fully accessible program is a program whose full functionality can be accessed exclusively through the UIA-provided mechanism. Axe.Windows uses the UIA-provided information to scan programs for accessibility issues. The basic units of UIA are:

- Hierarchy: The entire surface of all interactive applications is represented as a tree of Elements.
- Elements: Each Element may optionally have any combination of Properties, Patterns, and child Elements.
- Properties: Each Property is a Key/Value pair that identifies some aspect of the Element. The key is the property type (for example, IsVisible) and the value is the state of that property (for example, true or false).
- Patterns: There are several patterns that map to functionality that is exposed via the UI. For example, a list element will typically support a pattern that exposes all of the items in the list, as well as the ability to learn which item is currently selected, and the ability to select a different element, if desired.

Axe.Windows takes the data provided by UIA and compares it to a set of rules which identify cases where the given data would create issues for users of the assistive technologies which consume UIA data. The results of these rules are then returned to the caller in a selection of formats.

For information about the code, please visit the [solution overview](./solution.md).

For information about the rules used for accessibility testing, please visit the [rules overview](./RulesOverview.md).
