<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Overview

Axe.Windows is a [NuGet](https://www.nuget.org/) package for running automated accessibility tests on Windows® applications.

- Use an automation test framework like [UI Automation](https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-overview) or [WinAppDriver](https://github.com/microsoft/WinAppDriver) to manipulate your application
- Scan your application as many times as you need to
- Axe.Windows returns results with each scan and can optionally save each scan's results to an a11ytest file you can open with [Accessibility Insights](https://accessibilityinsights.io/docs/en/windows/overview)

To get the latest version of the Axe.Windows NuGet package, visit
[Axe.Windows on NuGet.org](https://www.nuget.org/packages/Axe.Windows/).

## How to run automated accessibility tests
     
For information about how to use Axe.Windows to test a Windows® application, please see the [Automation Guide](./docs/Automation.md).
     
## Contributing
All contributions are welcome! Please read through our guidelines on [contributions](./Contributing.md) to this project.

## Building the code
You can find more information on how to set up your development environment [here](./docs/SetUpDevEnv.md).

### 1. Clone the repository
- Clone the repository using one of the following commands
  ``` bash
  git clone https://github.com/Microsoft/axe-windows.git
  ```
  or
  ``` bash
  git clone git@github.com:Microsoft/axe-windows.git
  ```
- Select the created directory
  ``` bash
  cd axe-windows
  ```

### 2. Open the solution in Visual Studio
- Use the `src/AxeWindows.sln` file to open the solution.

### 3. Build and run unit tests

## Testing
We use the unit test framework from Visual Studio. Find more information in our [FAQ section](./docs/FAQ.md).

### More information
Visit the [Overview of Axe.Windows](./docs/Overview.md) page.

## Data/Telemetry

Axe.Windows does not collect any telemetry on its own. However, the package does provide telemetric data for use by calling applications. 
Please see the [Telemetry](./docs/Telemetry.md) page for more details.

## Reporting security vulnerabilities
If you believe you have found a security vulnerability in this project, please follow [these steps](https://technet.microsoft.com/en-us/security/ff852094.aspx) to report it. For more information on how vulnerabilities are disclosed, see [Coordinated Vulnerability Disclosure](https://technet.microsoft.com/en-us/security/dn467923).

## FAQ
Please visit our [FAQ section](./docs/FAQ.md) to get answers to common questions.
