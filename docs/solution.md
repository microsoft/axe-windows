<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Solution overview

The code is organized into the following general areas:

#### Runtime components
These assemblies provide the interaction with UIA, as well as layers that allow the application to interact with that information in a more convenient way:

Assembly | Responsibility
--- | ---
Axe.Windows.Actions | Provide a high-level set of Actions that are the primary interface into the Runtime components.
Axe.Windows.Core | Provide classes, interfaces, and types used by projects throughout the Axe.Windows solution.
Axe.Windows.Desktop | Provide Windows-specific implementations of the data abstractions found in Axe.Windows.Core. The low-level interactions with UIA occur in this assembly.
Axe.Windows.SystemAbstractions | Provides abstract classes for accessing system components, making it possible to unit test code that consumes the abstractions by using mocks
Axe.Windows.Telemetry | Provides an interface which any caller can supply to capture telemetry from Axe.Windows
Axe.Windows.Win32 | Provide a wrapper around Win32-specific code that is needed by other assemblies.

#### Accessibility rules
These assemblies contain the automated tests used to evaluate the accessibility of an application. Please visit the [Rules Overview](./RulesOverview.md) for a detailed description of the automated accessibility tests.

Assembly | Responsibility
--- | ---
Axe.Windows.Rules | Provide a library of rules, each of which scans a given element (or elements) for issues that are likely to be problematic. For example, a button without an accessible label will be flagged as an error.
Axe.Windows.RuleSelection | Coordinate rule execution in a consistent and reproducible way.

#### Application entry points
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
- SystemAbstractionsTests
- UnitTestSharedLibrary
- TelemetryTests
- Win32Tests 
