<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Overview

Axe.Windows is a NuGet package for running automated accessibility tests on Windows® applications.

To get the latest version of the Axe.Windows NuGet package, visit
[Axe.Windows on NuGet.org](https://www.nuget.org/packages/Axe.Windows/).

### How to run automated accessibility tests

1. Create a `Config` object using `Config.Builder`.

        // Create config to specifically target a process
        var myConfigBuilder = Config.Builder.ForProcessId(1234);

        // Optional: configure to create an A11yTest file
        myConfigBuilder.WithOutputFileFormat(OutputFileFormat.A11yTest);

        // Optional: configure to output the file to a specific directory (otherwise, current directory will be used)
        myConfigBuilder.WithOutputDirectory(".\test-directory");

        // Ready to use config
        var myConfig = myConfigBuilder.build();

2. Create a `Scanner` object using the `ScannerFactory` object with the `Config`.

        // Create scanner using myConfig
        var scanner = ScannerFactory.CreateScanner(myConfig);

3. Call  the `Scan` method on the `Scanner` object.

        var scanResults;
        try
        {
            scanResults = scanner.Scan();
        }
        catch(AxeWindowsAutomationException e)
        {
            Console.WriteLine(e.ToString());
        }

4. Check the results.

        Console.WriteLine("Number of errors found in scan: " + scanResults.ErrorCount);


- Use an automation test framework like [UI Automation](https://docs.microsoft.com/en-us/dotnet/framework/ui-automation/ui-automation-overview) or [WinAppDriver](https://github.com/microsoft/WinAppDriver) to manipulate your application
- Scan your application as many times as you need to
- Axe.Windows returns results with each scan and can optionally save each scan's results to an a11ytest file you can open with [Accessibility Insights](https://accessibilityinsights.io/docs/en/windows/overview)


For more details and a complete code example, please visit the [automation reference page](./docs/AutomationReference.md)

## Command line interface

Axe.Windows also has a command line interface (CLI) to simplify automated testing in build pipelines. Please check out the
[command line interface readme](./src/CLI/README.MD)
for more information.

## Contributing

All contributions are welcome! Please read through our guidelines on [contributions](./Contributing.md) to this project.

For instructions on how to build the code, please visit [building the code](./docs/BuildingTheCode.md).

For an overview of the solution, please visit the [solution overview](./docs/solution.md).

### More information

Visit the [Overview of Axe.Windows](./docs/Overview.md) page.

## Data/Telemetry

Axe.Windows does not collect any telemetry on its own. However, the package does provide telemetric data for use by calling applications. 
Please see the [Telemetry](./docs/Telemetry.md) page for more details.

## Reporting security vulnerabilities
If you believe you have found a security vulnerability in this project, please follow [these steps](https://technet.microsoft.com/en-us/security/ff852094.aspx) to report it. For more information on how vulnerabilities are disclosed, see [Coordinated Vulnerability Disclosure](https://technet.microsoft.com/en-us/security/dn467923).

## FAQ
Please visit our [FAQ section](./docs/FAQ.md) to get answers to common questions.
