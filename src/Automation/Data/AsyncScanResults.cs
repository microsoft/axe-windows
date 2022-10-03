// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Axe.Windows.Automation.Data
{
    /// <summary>
    /// Contains information, such as ScanResults, related to a particular invocation of the ScanAsync method.
    /// This object currently contains ScanResults.
    /// However, additional fields may be added at a future time to add extra information without breaking existing users of the IAsyncScanner API.
    /// </summary>
    public class AsyncScanResults
    {
        /// <summary>
        /// The ScanResults object associated with this scan
        /// </summary>
        public IReadOnlyCollection<ScanResults> ScanResultsCollection { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanResultsCollection">The collection of ScanResults to associate with this object</param>
        public AsyncScanResults(IReadOnlyCollection<ScanResults> scanResultsCollection)
        {
            ScanResultsCollection = scanResultsCollection ?? throw new ArgumentNullException(nameof(scanResultsCollection));
        }
    }
}
