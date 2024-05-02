// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation.Data
{
    /// <summary>
    /// Represents options for methods of <see cref="IScanner"/>.
    /// Passing null in place of a ScanOptions object to <see cref="IScanner"/> methods will use default values for all options.
    /// </summary>
    public class ScanOptions
    {
        /// <summary>
        /// The ID of this scan. Must be null or meet the requirements for a file name.
        /// </summary>
        public string ScanId { get; }

        /// <summary>
        /// The window handle for the root of the UIA subtree to scan.
        /// </summary>
        public System.IntPtr ScanRootWindowHandle { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="scanId">The ID of this scan. Must be null or meet the requirements for a file name.</param>
        /// <param name="scanRootWindowHandle">The window handle for the root of the UIA subtree to scan.</param>
        public ScanOptions(string scanId = null, System.IntPtr? scanRootWindowHandle = null)
        {
            ScanId = scanId;
            ScanRootWindowHandle = scanRootWindowHandle.GetValueOrDefault(System.IntPtr.Zero);
        }
    }
}
