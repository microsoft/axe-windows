// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Misc;
using System.Collections.Generic;
using System.Linq;

namespace Axe.Windows.Core.Results
{
    /// <summary>
    /// class ScanResults
    /// Container for all ScanResult(s) from Single Scanner
    /// it contains multiple instances of "ScanResult"
    /// </summary>
    public class ScanResults
    {
        private readonly object _itemsLock = new object();

        /// <summary>
        /// Items with ScanResult
        /// </summary>
        public IList<ScanResult> Items { get; private set; }

        /// <summary>
        /// Aggregated status
        /// </summary>
        public ScanStatus Status
        {
            get
            {
                ScanStatus status = ScanStatus.NoResult;

                if (Items != null)
                {
                    var tss = from i in Items
                              select i.Status;

                    status = tss.GetAggregatedScanStatus();
                }

                return status;
            }
        }

        /// <summary>
        /// Add Test report
        /// it supports multi thread.
        /// </summary>
        /// <param name="report"></param>
        public void AddScanResult(ScanResult report)
        {
            lock (_itemsLock)
            {
                Items.Add(report);
            }
        }

        /// <summary>
        /// Constructor for TestResult class
        /// </summary>
        /// <param name="desc"></param>
        public ScanResults()
        {
            Items = new List<ScanResult>();
        }

        /// <summary>
        /// clear internal data
        /// </summary>
        internal void Clear()
        {
            Items.Clear();
        }
    }
}
