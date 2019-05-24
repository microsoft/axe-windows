// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Automation2
{
    /// <summary>
    /// Contains information about an AxeWindows automated scan
    /// </summary>
    public interface IAutomationScanResults
    {
        /// <summary>
        /// A Collection of paths to any output files written as a result of a scan.
        /// </summary>
        /// <remarks>
        /// This property may be null if no output files were written.
        /// That may happen if no <see cref="IAutomationConfig.OutputFileFormat"/> was specified
        /// or was set to <see cref="OutputFileFormat.None"/>.
        /// The value will also be null if no errors were found.
        /// </remarks>
        IEnumerable<string> OutputFiles { get; }

        /// <summary>
        /// A count of all errors across all elements scanned.
        /// </summary>
        /// <remarks>
        /// This may be useful as a simple test to determine if the number of errors between scans of the same area
        /// of an application has increased or (hopefully) decreased.
        /// </remarks>
        int ErrorCount { get; }

        /// <summary>
        /// A collection of errors found during the scan.
        /// </summary>
        /// <remarks>
        /// Use this to get in-depth information about the rule + element combination for each error. 
        /// </remarks>
        IEnumerable<IAutomationScanResult> Errors { get; }

        /// <summary>
        /// A dictionary containing the elements scanned.
        /// The keys are the values of <see cref="IAutomationScanResult.ElementId"/>.
        /// The values are objects of type <see cref="IElementInfo"/>.
        /// </summary>
        /// <remarks>
        /// All elements are included, even if no errors were found.
        /// </remarks>
        IReadOnlyDictionary<int, IElementInfo> Elements { get; }
    } // interface
} // namespace
