// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Contains information about an AxeWindows automated scan
    /// </summary>
    public class ScanResults
    {
        /// <summary>
        /// An OutputFile object containing paths to any output files written as
        /// a result of a scan.
        /// </summary>
        /// <remarks>
        /// This property's members may be null if no output files were written.
        /// That may happen if no <see cref="Config.OutputFileFormat"/> was specified
        /// or was set to <see cref="OutputFileFormat.None"/>.
        /// This property's members will also be null if no errors were found.
        /// </remarks>
        public OutputFile OutputFile { get; internal set; }

        /// <summary>
        /// A count of all errors across all elements scanned.
        /// </summary>
        /// <remarks>
        /// This may be useful as a simple test to determine if the number of errors between scans of the same area
        /// of an application has increased or (hopefully) decreased.
        /// </remarks>
        public int ErrorCount { get; internal set; }

        /// <summary>
        /// A collection of errors found during the scan.
        /// </summary>
        /// <remarks>
        /// Use this to get in-depth information about the rule + element combination for each error. 
        /// </remarks>
        public IEnumerable<ScanResult> Errors { get; internal set; }
    } // class 
} // namespace
