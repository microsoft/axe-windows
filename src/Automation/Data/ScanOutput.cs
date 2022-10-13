// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Axe.Windows.Automation.Data
{
    /// <summary>
    /// Contains information, such as WindowScanOutput objects, related to a particular invocation of the IScanner interface.
    /// This object currently contains a collection of WindowScanOutput objects.
    /// However, additional fields may be added at a future time to add extra information without breaking existing users of the IScanner API.
    /// </summary>
    public class ScanOutput
    {
        /// <summary>
        /// A collection of WindowScanOutput objects produced from this scan
        /// </summary>
        public IReadOnlyCollection<WindowScanOutput> WindowScanOutputs { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="windowScanOutputs">A collection of WindowScanOutput objects produced from this scan</param>
        public ScanOutput(IReadOnlyCollection<WindowScanOutput> windowScanOutputs)
        {
            WindowScanOutputs = windowScanOutputs ?? throw new ArgumentNullException(nameof(windowScanOutputs));
        }
    }
}
