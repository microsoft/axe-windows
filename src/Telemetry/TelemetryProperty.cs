// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//
namespace Axe.Windows.Telemetry
{
    /// <summary>
    /// Enums for Telemetry Properties
    /// </summary>
    internal enum TelemetryProperty
    {
        ControlType,
        ElementsInScan,
        ScanDurationInMilliseconds,
        Results,
        RuleId,
        TestResults, // parent container, has rule id and results
        UIFramework,
        UpperBoundExceeded,
    }
}
