<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->
     
## Overview of Axe.Windows

### Tool Overview

Windows UI Automation (UIA) is the platform-provided way for accessibility tools to interact with programs. A fully accessible program is a program whose full functionality can be accessed exclusively through the UIA-provided mechanism. Axe.Windows uses the UIA-provided information to scan programs for accessibility issues. The basic units of UIA are:

- Hierarchy: The entire surface of all interactive applications is represented as a tree of Elements.
- Elements: Each Element may optionally have any combination of Properties, Patterns, and child Elements.
- Properties: Each Property is a Key/Value pair that identifies some aspect of the Element. The key is the property type (for example, IsVisible) and the value is the state of that property (for example, true or false).
- Patterns: There are several patterns that map to functionality that is exposed via the UI. For example, a list element will typically support a pattern that exposes all of the items in the list, as well as the ability to learn which item is currently selected, and the ability to select a different element, if desired.

Axe.Windows takes the data provided by UIA and compares it to a set of rules which identify cases where the given data would create issues for users of the assistive technologies which consume UIA data. The results of these rules are then returned to the caller in a selection of formats.

### Code Organization
The code is organized into the following general areas:

#### Runtime components
These assemblies provide the interaction with UIA, as well as layers that allow the application to interact with that information in a more structured manner:

Assembly | Responsibility
--- | ---
Axe.Windows.Actions | Provide a high-level set of Actions that are the primary interface into the Runtime components.
Axe.Windows.Core | Provide classes, interfaces, and types used by projects throughout the Axe.Windows solution.
Axe.Windows.Desktop | Provide Windows-specific implementations of the data abstractions found in Axe.Windows.Core. The low-level interactions with UIA occur in this assembly.
Axe.Windows.Telemetry | Provides an interface which any caller can supply to capture telemetry from Axe.Windows
Axe.Windows.Win32 | Provide a wrapper around Win32-specific code that is needed by other assemblies.

#### Accessibility Rules
These assemblies contain the automated tests used to evaluate the accessibility of an application. Please visit the [Rules Overview](./RulesOverview.md) for a detailed description of the automated accessibility tests.

Assembly | Responsibility
--- | ---
Axe.Windows.Rules | Provide a library of rules, each of which scans a given element (or elements) for issues that are likely to be problematic. For example, a button without an accessible label will be flagged as an error.
Axe.Windows.RuleSelection | Coordinate rule execution in a consistent and reproducible way.

#### Application Entry Points
These assemblies allow user interaction with the Runtime components and the Accessibility Rules.

Assembly | Responsibility
--- | ---
Axe.Windows.Automation | Provide a layer that wraps key actions behind a simplified interface. This layer can be used to run Axe.Windows automated accessibility tests programmatically.

#### Packaging
The packaging project exists to gather assemblies into their shipping vehicle:

Project | Responsibility
--- | ---
CI | Builds the NuGet package that will be referenced by code that uses the library.

#### Tests
Unit tests are built using a combination of Moq and Microsoft Fakes. The folllowing projects exist for testing purposes:
- Fakes.Prebuild
- ActionsTests
- AutomationTests
- CoreTests
- DesktopTests
- RuleSelectionTests
- RulesTest
- UnitTestSharedLibrary
- TelemetryTests
- Win32Tests 
