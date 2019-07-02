## Axe.Windows - Automation

### Overview
To enable various automation scenarios, we have created an assembly
(`Axe.Windows.Automation.dll`) that exposes a subset of core
AxeWindows functionality to automation systems.

### General Characteristics

#### Fully Synchronous
Since these commands are all stateful, they are intentionally synchronous within
a process. If you attempt to call into the commands concurrently, the first one
to obtain the lock will execute, then another, then another. This is by design
and is not expected to change at any future time. If you have a scenario that
truly requires the command to execute in parallel, then you will need to create
a solution where you can make those calls from separate processes.

#### How To Use (.NET)

Consumers should look to follow the processs below:

1. Create a `Config` object using the `ConfigBuilder` class
2. Create a `Scanner` object using the `Config` object created in step 1.
3. Run the `Scan` method on the `Scanner` object.
4. Analyze the returned `ScanResults` object for your respective needs.

The details of these objects/methods are below.

### Implementation Details

#### Config.Builder Details/Methods

##### ForProcessId
Create the builder for the config for the specified process.

##### Parameters

The **ForProcessId** method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
processId | `int` | The process Id of the application to test. If the value is invalid, the automation session will throw an `AxeWindowsAutomationException`.

##### Return object

The **ForProcessId** method returns a **Config.Builder** object, which has the following methods:

**Name** | **Parameters** | **Output** | **Description**
---|---|---|---
WithOutputFileFormat | `(OutputFileFormat format)` | `Config.Builder` | Specify the type(s) of output files you wish AxeWindows to create. No output files will be created if this is left unspecified.
WithOutputDirectory | `(string directory)` | `Config.Builder` | Specify the directory where any output files should be written. Output files will be created in the current directory under folder **AxeWindowsOutputFiles** if left unspecified.
Build | `N/A` | `Config` |  Build an instance of `Config`; to be consumed by `ScannerFactory`.

#### ScannerFactory Details/Methods

##### CreateScanner
Create an object using config that implements `IScanner`.

##### Parameters

The **ScannerFactory.CreateScanner** method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
config | `Config` | The configuration used by the returned `IScanner` object.

##### Return object

The **ScannerFactory.CreateScanner** method returns an **IScanner** object.

### IScanner Details/Methods

#### Scan
The Scan runs AxeWindows automated tests using the config provided at the time of creation of the scanner.

##### Parameters

The **Scan** method accepts no parameters.

##### Return object

The **Scan** method returns a **ScanResults** object and has the following properties:

**Name** | **Type** | **Description**
---|---|---
ErrorCount | `int` | A count of all errors across all elements scanned.
Errors | `IEnumerable<ScanResult>` | A collection of errors found during the scan.
OutputFile | `(string A11yTest, string Sarif)` | A Tuple with paths to any output files written as a result of a scan. Tuple members are A11yTest and Sarif (**not implemented yet**).

The **Errors** property contains **ScanResult** objects which are the result of a single rule test on a single element and have the following properties:

**Name** | **Type** | **Description**
---|---|---
Rule | `RuleInfo` | Information about the rule (description, how to fix information, etc.) that was evaluated on the element.
Element | `ElementInfo` | The element which was tested against the rule.

`RuleInfo` contains the following properties:

**Name** | **Type** | **Description**
---|---|---
ID | `RuleId` | Contains a unique identifier for the rule from the RuleId enumeration.
Description | `string` | Contains a short description of the rule.
HowToFix | `string` | Detailed information on how to resolve a violation reported by the rule.
Standard | `A11yCriteriaId` | An enum which identifies the standards documentation from which the rule was derived.
PropertyID | `int` | In cases where the rule tests one specific UI Automation property, this contains the UI Automation property ID in question. This property is used to link elements with rule violations to relevant documentation.
Condition | 'string' | A description of the conditions under which a rule will be evaluated.

`ElementInfo` contains the following properties:

**Name** | **Type** | **Description**
---|---|---
Properties | `Dictionary<string, string>` | A string to string dictionary where the key is a UIAutomation property name and the value is the corresponding UI Automation property value.
Patterns | `IEnumerable<string>` | A list of names of supported patterns.

### Using the assembly
You can get the files via a NuGet package Configure NuGet to retrieve the
**Axe.Windows** package from
<https://api.nuget.org/v3/index.json>,
then use the classes in the Axe.Windows.Automation namespace (see
example below):

-   Prerequisite: Your project *must* use .NET 4.7.1 (this is required by Axe-Windows).
-   If you’re using NuGet, add the appropriate feed to your project.
-   Add **using Axe.Windows.Automation;** to your code.
-   Follow the steps in **How To Use**.

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
                string testAppPath = Path.GetFullPath("..\myApplication.exe");
                string outputDir = Path.GetFullPath(".\TestOutput");
                Process testProcess = Process.Start(testAppPath);
                var config = Config.Builder.ForProcessId(testProcess.Id)
                    .WithOutputDirectory(outputDir)
                    .WithOutputFileFormat(OutputFileFormat.A11yTest)
                    .Build();

                var scanner = ScannerFactory.CreateScanner(config);

                var output = scanner.Scan();

                Assert.IsTrue(output.ErrorCount == 0);
            }
        }
    }
```