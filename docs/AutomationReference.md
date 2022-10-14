## Axe.Windows - automation reference

Note: The following documentation describes the supported API provided by Axe.Windows. There are other, public interfaces and classes in various assemblies throughout the package; however, please be aware that interfaces and classes not described in this document are not supported and may change at any time.

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

The `ForProcessId` method returns a new instance of `Config.Builder`.

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

The `WithOutputFileFormat` method returns the `Config.Builder` configured with the specified format.

##### `WithOutputDirectory`

Specify the directory where any output files should be written.

###### Parameters

The `WithOutputDirectory` method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
directory | `string` | The directory where any output files should be written; is not used if output file format is `None`. Output files will be created in the current directory under folder **AxeWindowsOutputFiles** if left unspecified.

###### Return object

The `WithOutputDirectory` method returns the `Config.Builder` configured with the specified output directory.

##### `WithDPIAwareness`

Override the default implementation of [`IDPIAwareness`](#idpiawareness) with a caller-specified implementation. The default implementation calls the Win32 [`SetDPIProcessAware`](https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setprocessdpiaware) function on `Enable`, and does nothing on `Restore`.

###### Parameters

The `WithDPIAwareness` method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
dpiAwareness | `IDPIAwareness` | The caller-provided implementation of [`IDPIAwareness`](#idpiawareness).

###### Return object

The `WithDPIAwareness` method returns the `Config.Builder` configured to use the specified implementation of IDPIAwareness.

##### `WithCustomUIAConfig`

Specify the path to a [custom UIA configuration file](./CustomUIA.md).

###### Parameters

The `WithCustomUIAConfig` method accepts the following parameters:

**Name** | **Type** | **Description**
---|---|---
path | `string` | The path to the configuration file.

###### Return object

The `WithCustomUIAConfig` method returns the  `Config.Builder` configured with the specified custom UIA configuration file.

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

##### `ScanAsync`
The `ScanAsync` method asynchronously runs AxeWindows automated tests using the config provided at the time of creation of the scanner. This method should be `await`ed.

##### `Scan`
The `Scan` method synchronously runs AxeWindows automated tests using the config provided at the time of creation of the scanner, and blocks until the scan is complete.

##### Parameters
Methods of `IScanner` accept one parameter: an instance of `ScanOptions` containing custom settings for this scan, or `null` for default options.

##### Return object
Methods of `IScanner` return a `ScanOutput` object with the following properties:

**Name** | **Type** | **Description**
---|---|---
`WindowScanOutputs` | `IReadOnlyCollection<WindowScanOutput>` | The set of `WindowScanOutput` objects produced by this scan, one per top-level application window.

#### `ScanOptions`
The `ScanOptions` constructor accepts the following arguments:

**Name** | **Type** | **Description**
---|---|---
scanId | `string` | A string identifier for the scan. If the scan produces output files based on the `Config` object used to create the scanner, the output files will be given the name of the scan id (e.g., MyScanId.a11ytest).

#### `WindowScanOutput`
A `WindowScanOutput` object contains the results of scanning one top level window and has the following properties:

**Name** | **Type** | **Description**
---|---|---
ErrorCount | `int` | A count of all errors across all elements scanned.
Errors | `IEnumerable<ScanResult>` | A collection of errors found during the scan.
OutputFile | `OutputFile` | Represents the output file(s), if any, associated with a `WindowScanOutput` object.

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

#### IDPIAwareness

UIA operates in physical screen coordinates, so DPI awareness _must_ be enabled while scanning. Methods on this interface will be called in pairs on each call to `IScanner`'s methods. Each successful call to `Enable` will have a corresponding call to `Restore`.

##### `Enable`
Enable DPI awareness for the scan.

###### Parameters

The `Enable` method accepts no parameters

###### Return object

The return object from `Enable` is passed as a parameter to the `Restore` method. It allows calls to `Enable` and `Restore` to be easily associated and is not used by Axe.Windows.

###### Implementation Note
If `Enable` throws an Exception, then Axe.Windows will _not_ call the `Restore` method. Any required cleanup becomes the responsibility of the Exception handler inside the implementation of `Enable`.

##### `Restore`
Restore DPI awareness to its non-scanning state.

###### Parameters

The `Restore` method accepts 1 parameter

**Name** | **Type** | **Description**
---|---|---
dataFromEnable | `object` | This is the data returned from the `Enable` method. It allows calls to `Enable` and `Restore` to be easily associated.

###### Return object

The return object from `Enable` is passed as a parameter to the `Restore` method. It is not used by Axe.Windows.

### Using the assembly
You can get the files via a [NuGet package](https://www.nuget.org/packages/Axe.Windows); configure NuGet to retrieve the
**Axe.Windows** package from
<https://api.nuget.org/v3/index.json>,
then use the classes in the Axe.Windows.Automation namespace (see
example below):

-   Prerequisite: Your project *must* be able to use .NET Standard 2.0 libraries.
-   If you’re using NuGet, add the appropriate feed to your project.
-   Add **using Axe.Windows.Automation;** to your code.
-   Follow the steps in [How To Run An Automated Scan](#how-to-run-an-automated-scan).

#### Example
```C#
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Axe.Windows.Automation;

namespace AxeWindowsDemo
{
    class Program
    {
        /// <summary>
        /// This is a quick and easy demo of the automation code
        /// </summary>
        static async Task Main(string[] args)
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
                var output = await scanner.ScanAsync(null);
                Assert.AreEqual(0, output.WindowScanOutputs.First().ErrorCount);
            }
            catch(Exception e)
            {
                var errorMessage = e.toString();
                Assert.Fail(errorMessage);
            }
        }
    }
}
```

#### Debugging with symbols

There may be occasions where you want to debug assemblies within the Axe.Windows [NuGet package](https://www.nuget.org/packages/Axe.Windows) that your process is consuming. The specific steps depend on the version of Axe.Windows that you are using.

##### Axe.Windows 1.1.5 and earlier
These versions require you to access the symbols from the NuGet symbol server for debugging. The steps to enable debugging are:
1. In Visual Studio, go to **Tools** > **Options** > **Debugging** > **General** and uncheck "Enable Just My Code".
2. In Visual Studio, go to **Tools** > **Options** > **Debugging** > **Symbols** and check "NuGet.org Symbol Server". This tells Visual Studio to attempt to download symbols for NuGet packages. (Note that this download will occur on the first app boot after the setting is enabled or after a version changes, so you may experience a delay in loading the app after making this change).
3. You will need to have a local copy of the sources at the same commit as the release (one easy to do this is to download the source code zip file from the corresponding [release](https://github.com/microsoft/axe-windows/releases)). When Visual Studio needs the sources, it will prompt for the location of the source code.
4. Run and debug as usual.

##### Axe.Windows 1.1.6 and newer
These versions embed the symbols into the assemblies and support [SourceLink](https://github.com/dotnet/sourcelink) to easily obtain the source code at the correct commit. The steps to enable debugging are:
1. In Visual Studio, go to **Tools** > **Options** > **Debugging** > **General** and uncheck "Enable Just My Code".
2. In Visual Studio, go to **Tools** > **Options** > **Debugging** > **General** and check "Enable Source Link support". When Visual Studio needs the sources, it will ask for permission to retrieve them, then it will cache a local copy for future use.
3. Run and debug as usual.

#### Error handling

`AxeWindowsAutomationException` is thrown for errors in `Axe.Windows.Automation`'s logic where meaningful error reporting can be generated. In some situations, the `Exception.InnerException` property may contain a corresponding system-level exception for errors encountered by Axe.Windows.

#### Migrating from Axe.Windows 1.x
Migration from Axe.Windows 1x to Axe.Windows 2.0 should require minimal code changes for most projects.

##### `IScanner.ScanAll` removed
The `ScanAll` method for scanning multiple top-level windows has been removed. Use `ScanAsync` or `Scan` instead.

##### Calling `IScanner.Scan`

In place of this | Do this
---|---
`IScanner.Scan()` | `IScanner.Scan(null)`
`IScanner.Scan("scanId")` | `IScanner.Scan(new ScanOptions(scanId: "scanId"))`

##### `ScanResults` replaced with `ScanOutput`
The new return type of `IScanner`'s methods is `ScanOutput`. This object contains a `WindowScanOutputs` field, a `IReadOnlyCollection` of `WindowScanOutput` objects. These objects contain the same fields as the previous `ScanResults` class.

In place of this | Do this
---|---
`IScanner.Scan().ErrorCount` | `IScanner.Scan(null).WindowScanOutputs.First().ErrorCount`
`foreach (ScanResult item in IScanner.ScanAll())` | `foreach (ScanResult item in IScanner.Scan(null).WindowScanOutputs)`
