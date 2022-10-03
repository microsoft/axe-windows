// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;

namespace Axe.Windows.Automation.Data
{
    /// <summary>
    /// Represents options for <see cref="IAsyncScanner.ScanAsync(ScanOptions, CancellationToken)"/>.
    /// </summary>
    public class ScanOptions
    {
        /// <summary>
        /// The ID of this scan. Must be null or meet the requirements for a file name.
        /// </summary>
        public string ScanId { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanId">The ID of this scan. Must be null or meet the requirements for a file name.</param>
        public ScanOptions(string scanId = null)
        {
            ScanId = scanId;
        }
    }
}
