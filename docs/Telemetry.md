<!-- Copyright (c) Microsoft Corporation. All rights reserved.
     Licensed under the MIT License. -->

## Telemetry

Axe.Windows does not collect any telemetry on its own. However, the package does provide telemetric data for use by calling applications. 

### Getting telemetry events using `IAxeWindowsTelemetry`

To receive telemetry from Axe.Windows, you must create an object which implements the `IAxeWindowsTelemetry` interface. The `IAxeWindowsTelemetry` interface has the following  methods and properties:

#### `void PublishEvent(string eventName, IReadOnlyDictionary<string, string> properties);`

This method is called for expected telemetry events (not errors).

##### Parameters

###### `string eventName`

A string representing the general type of telemetry event. The following are the possible values for this parameter:

*Note*: please see the [Telemetry properties](#telemetry-properties) section for details on each telemetry property.

Value | Expected telemetry properties
--- | ---
Scan_Statistics | ElementsInScan, UpperBoundExceeded
SingleRule_Tested_Results | RuleId, TestResults

###### `IReadOnlyDictionary<string, string> properties`

A dictionary where the key is a [telemetry property](#telemetry-properties)
and the value is the data associated with the property.
 
##### Returns
 
void

#### `void ReportException(Exception e);`

##### Parameters

###### `Exception e`

An exception representing the error.

##### Returns

void

#### `bool IsEnabled { get;  }`
 
 A boolean value indicateing if Axe.Windows should send telemetry events.
 
Even though telemetry is expected to be very efficient, this allows the internal code to avoid doing any potentially time consuming steps to package the telemetry data if it won't be published.
 
### Setting the telemetry sink

To set the object you created in the section above as the sink for Axe.Windows telemetry events, call the `SetTelemetrySink` function on the `Logger` class as follows:

`Axe.Windows.Telemetry.Logger.SetTelemetrySink(yourTelemetrySink);`

### Telemetry properties

Wehn "Scan_Statistics" is sent to `PublishEvent` as the event name, the following items are expected in the properties dictionary:

Property | Data
--- | ---
ElementsInScan | The number of UI Automation elements evaluated during the scan
UpperBoundExceeded | A boolean string indicating whether the upper bound for the number of elements allowed per scan was reached

Wehn "SingleRule_Tested_Results" is sent to `PublishEvent` as the event name, the following items are expected in the properties dictionary:

Property | data
--- | ---
RuleId | A unique string that identifies a rule
TestResults | JSON formatted string containing results

The JSON data for the TestResults property takes the following form:

```
{
    "RuleId": "BoundingRectangleNotAllZeros",
    "Results": [
        {
            "ControlType": "Window",
            "UIFramework": "WPF",
            "Pass": "1"
        },
        {
            "ControlType": "TitleBar",
            "UIFramework": "WPF",
            "Pass": "1"
        }
    ]
}
```

The objects in the `Results` array have the following fields

field | Value
--- | ---
ControlType | The value of the LocalizedControlType property for a given UI Automation element
UIFramework | The value of the FrameworkId property for a given UI Automation element
Pass/Fail/Uncertain | The count of the type of result
