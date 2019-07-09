## Axe.Windows - Automation

### Overview
To provide automated accessibility testing for Windows applications, we have created the `Axe.Windows.Automation` .NET assembly which exposes a subset of core
AxeWindows functionality to automation systems.

### How to run an automated scan

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
            // Get the message from an exception, if one is thrown.
            var errorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
        }

4. Check the results.

        Console.WriteLine("Number of errors found in scan: " + scanResults.ErrorCount);


A [complete code example](#example) can be found below.

### Class details

#### Config.Builder

##### *`static`* `ForProcessId`
Create the builder for the config for the specified process.

###### Parameters

The `ForProcessId` method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
processId | `int` | The process Id of the application to test. If the value is invalid, the automation session will throw an [`AxeWindowsAutomationException`](#error-handling).

###### Return object

The `ForProcessId` method returns an instance of `Config.Builder`.

##### `WithOutputFileFormat`

Specify the type(s) of output files you wish AxeWindows to create.

The [`OutputFileFormat` enum](../src/Automation/enums/OutputFileFormat.cs) currently defines the following values:

**Name** | **Description**
---|---
None | Create no output files.
A11yTest | Create output files which can be opened using [Accessibility Insights for Windows](https://accessibilityinsights.io/docs/en/windows/overview).

###### Parameters

The `WithOutputFileFormat` method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
format | `OutputFileFormat` | The type(s) of output files you wish AxeWindows to create. No output files will be created if this is left unspecified or 0 errors are found. The default value is `None`.

###### Return object

The `WithOutputFileFormat` method returns the  `Config.Builder` configured with the specified format.

##### `WithOutputDirectory`

Specify the directory where any output files should be written.

###### Parameters

The `WithOutputDirectory` method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
directory | `string` | The directory where any output files should be written; is not used if output file format is `None`. Output files will be created in the current directory under folder **AxeWindowsOutputFiles** if left unspecified.

###### Return object

The `WithOutputDirectory` method returns the  `Config.Builder` configured with the specified output directory.

##### Build

Build an instance of `Config`.

###### Parameters

The `Build` method accepts no parameters.

###### Return object

The `Build` method returns an instance of `Config` with any modifications made through the `Config.Builder`.

#### ScannerFactory

##### `CreateScanner`
Create an object that implements `IScanner` using an instance of `Config`.

###### Parameters

The `ScannerFactory.CreateScanner` method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
config | `Config` | The configuration used by the returned `IScanner` object.

###### Return object

The `ScannerFactory.CreateScanner` method returns an `IScanner` object.

#### IScanner

##### `Scan`
The Scan runs AxeWindows automated tests using the config provided at the time of creation of the scanner.

###### Parameters

The `Scan` method accepts no parameters.

###### Return object

The `Scan` method returns a `ScanResults` object and has the following properties:

**Name** | **Type** | **Description**
---|---|---
ErrorCount | `int` | A count of all errors across all elements scanned.
Errors | `IEnumerable<ScanResult>` | A collection of errors found during the scan.
OutputFile | `OutputFile` | Represents the output file(s), if any, associated with a ScanResults object.

The `OutputFile` property is a struct with the following properties:

**Name** | **Type** | **Description**
---|---|---
A11yTest | `string` | The path to the A11yTest file that was generated (or null if no file was generated).
Sarif | `string` | The path to the Sarif file that was generated (or null if no file was generated).

The `Errors` property contains `ScanResult` objects which are the result of a single rule test on a single element and have the following properties:

**Name** | **Type** | **Description**
---|---|---
Rule | `RuleInfo` | Information about the rule (description, how to fix information, etc.) that was evaluated on the element.
Element | `ElementInfo` | The element which was tested against the rule.

`RuleInfo` contains the following properties:

**Name** | **Type** | **Description**
---|---|---
ID | `RuleId` | Contains a unique identifier for the rule from the [RuleId enumeration](../src/Core/Enums/RuleId.cs).
Description | `string` | Contains a short description of the rule.
HowToFix | `string` | Detailed information on how to resolve a violation reported by the rule.
Standard | `A11yCriteriaId` | The [A11yCriteriaId enumeration](../src/Rules/A11yCriteriaId.cs) identifies the standards documentation from which the rule was derived.
PropertyID | `int` | In cases where the rule tests one specific UI Automation property, this contains the UI Automation property ID in question. This property is used to link elements with rule violations to relevant documentation.
Condition | `string` | A description of the conditions under which a rule will be evaluated.

`ElementInfo` contains the following properties:

**Name** | **Type** | **Description**
---|---|---
Properties | `Dictionary<string, string>` | A string to string dictionary where the key is a UI Automation property name and the value is the corresponding UI Automation property value.
Patterns | `IEnumerable<string>` | A list of names of supported patterns.

### Using the assembly
You can get the files via a NuGet package; configure NuGet to retrieve the
**Axe.Windows** package from
<https://api.nuget.org/v3/index.json>,
then use the classes in the Axe.Windows.Automation namespace (see
example below):

-   Prerequisite: Your project *must* use .NET 4.7.1 (this is required by Axe-Windows).
-   If you’re using NuGet, add the appropriate feed to your project.
-   Add **using Axe.Windows.Automation;** to your code.
-   Follow the steps in [How To Run An Automated Scan](#how-to-run-an-automated-scan).

#### Example
```
    using System;
    using System.Collections.Generic;
    using Axe.Windows.Automation;

    namespace AxeWindowsDemo
    {
        class Program
        {
            /// <summary>
            /// This is a quick and easy demo of the automation code
            /// </summary>
            static void Main(string[] args)
            {
                string testAppPath = Path.GetFullPath(@"..\myApplication.exe");
                string outputDir = Path.GetFullPath(@".\TestOutput");
                Process testProcess = Process.Start(testAppPath);
                var config = Config.Builder.ForProcessId(testProcess.Id)
                    .WithOutputFileFormat(OutputFileFormat.A11yTest)
                    .WithOutputDirectory(outputDir)
                    .Build();

                var scanner = ScannerFactory.CreateScanner(config);

                try
                {
                    var output = scanner.Scan();
                    Assert.AreEqual(0, output.ErrorCount);
                }
                catch(AxeWindowsAutomationException e)
                {
                    var errorMessage = e.InnerException != null ? e.InnerException.Message : e.Message;
                    Assert.Fail(errorMessage);
                }
            }
        }
    }
```

### Miscellaneous

#### Error handling

`AxeWindowsAutomationException` is thrown for all unhandled errors in Axe.Windows.Automation. If an exception was thrown from code not owned by Axe.Windows.Automation, that exception will be wrapped in the `Exception.InnerException` property.

#### Fully synchronous
Because  automated scans are stateful, they are intentionally synchronous within
a process. If you attempt to initiate multiple scans concurrently, the first one
to obtain the lock will execute, then another, then another. This is by design
and is not expected to change at any future time. If you have a scenario that
truly requires the command to execute in parallel, then you will need to create
a solution where you can make those calls from separate processes.
