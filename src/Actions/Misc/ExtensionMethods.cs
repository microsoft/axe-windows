// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Resources;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Misc;
using Axe.Windows.Telemetry;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Axe.Windows.Actions.Misc
{
    /// <summary>
    /// Class for Extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Identifies elements whose bounding rectangles contain the given position
        /// and returns the element with the smallest bounding rectangle (by area)
        ///
        /// Does not translate the given position
        ///
        /// Throws ArgumentNullException if allElements is null
        /// Throws ArgumentException if allElements is empty
        /// </summary>
        /// <param name="position">Pixel location</param>
        /// <returns>element overlapping point with smallest area, or null if no elements overlap the given point</returns>
        internal static ICoreA11yElement GetSmallestElementFromPoint(Dictionary<int, ICoreA11yElement> allElements, System.Drawing.Point position)
        {
            if (allElements == null) throw new ArgumentNullException(nameof(allElements));
            if (allElements.Count == 0) throw new ArgumentException(ErrorMessages.ItemsExpected, nameof(allElements));

            // Identify elements whose bounding rectangle contain clicked pixel point
            var containingElements = allElements.
                Where(kv => !kv.Value.IsRootElement() && !kv.Value.IsOffScreen).
                ToDictionary(kv => kv.Key, kv =>
                {
                    System.Drawing.Rectangle rect = kv.Value.BoundingRectangle;
                    return rect;
                }).
                Where(kv => kv.Value.Contains(position));

            if (!containingElements.Any())
            {
                return null;
            }

            // Find smallest element area
            var smallestArea = containingElements.Aggregate((idToRectA, idToRectB) =>
                  idToRectA.Value.Size.Height * idToRectA.Value.Size.Width
                < idToRectB.Value.Size.Height * idToRectB.Value.Size.Width
                ? idToRectA : idToRectB);

            return allElements[smallestArea.Key];
        }

        /// <summary>
        /// Sends the results of the testing to telemetry
        ///
        /// Sends a statistics event of the following format:
        /// {
        ///     "ElementsInScan":593,
        ///     "UpperBoundExceeded":false,
        /// }
        ///
        /// Also sends one event per rule and groups by combination of
        /// framework ID / control type. For each group the
        /// statuses are aggregated / counted.
        ///
        /// example:
        /// {
        ///     "RuleId":"NameNonEmpty",
        ///     "SingleTestResults":[{"ControlType":"Window","UIFramework":"WPF","Pass":"1"},
        ///                          {"ControlType":"MenuBar","UIFramework":"WPF","Pass":"2", "Fail":"4"}]
        /// }
        ///
        /// </summary>
        /// <param name="dc"></param>
        public static void PublishScanResults(this ElementDataContext dc, TimeSpan scanDuration)
        {
            if (dc == null)
                return; // no op

            if (!Logger.IsEnabled)
                return; // No telemetry, so don't waste CPU cycles

            Logger.PublishTelemetryEvent(TelemetryAction.Scan_Statistics, new Dictionary<TelemetryProperty, string>
            {
                { TelemetryProperty.ElementsInScan, dc.ElementCounter.Attempts.ToString(CultureInfo.InvariantCulture) },
                { TelemetryProperty.ScanDuration, scanDuration.ToString("c", CultureInfo.InvariantCulture) },
                { TelemetryProperty.UpperBoundExceeded, dc.ElementCounter.UpperBoundExceeded.ToString(CultureInfo.InvariantCulture) }

            });

            var ruleResults = dc.Elements.Where(pair => pair.Value.ScanResults != null)
                .SelectMany(pair => pair.Value.ScanResults.Items)
                .SelectMany(scanResults => scanResults.Items);

            var flattenedRuleInfoTuples = ruleResults.Select(ruleResult =>
                (ruleId: ruleResult.Rule.ToString(),
                 status: ruleResult.Status.ToString(),
                 controlType: ruleResult.MetaInfo.ControlType.ToString(CultureInfo.InvariantCulture),
                 framework: ruleResult.MetaInfo.UIFramework)
            ).ToList();

            // Group by rules
            var ruleGroups = flattenedRuleInfoTuples.GroupBy(tuple => tuple.ruleId);
            foreach (var ruleGroup in ruleGroups)
            {
                var ruleId = ruleGroup.Key;

                var groupDicts = new List<Dictionary<string, string>>();

                // Group by control type and framework
                var controlFrameworkGroups = ruleGroup.GroupBy(tuple => (controlType: tuple.controlType, framework: tuple.framework));
                foreach (var secondGroup in controlFrameworkGroups)
                {
                    Dictionary<string, string> groupDict = new Dictionary<string, string>();
                    groupDict.Add(TelemetryProperty.ControlType.ToString(), secondGroup.Key.controlType);
                    groupDict.Add(TelemetryProperty.UIFramework.ToString(), secondGroup.Key.framework);
                    foreach (var statusGroup in secondGroup.GroupBy(group => group.status))
                    {
                        groupDict.Add(statusGroup.Key, statusGroup.Count().ToString(CultureInfo.InvariantCulture));
                    }
                    groupDicts.Add(groupDict);
                }

                // Create output dictionary and send to telemetry
                Dictionary<string, dynamic> output = new Dictionary<string, dynamic>
                {
                    { TelemetryProperty.RuleId.ToString(), ruleId },
                    { TelemetryProperty.Results.ToString(), groupDicts },
                };
                Logger.PublishTelemetryEvent(TelemetryAction.SingleRule_Tested_Results, TelemetryProperty.TestResults, Newtonsoft.Json.JsonConvert.SerializeObject(output));
            }
        }
    }
}
