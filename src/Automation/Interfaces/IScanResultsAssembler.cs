// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Provides methods used to assemble <see cref="ScanResult"/> and <see cref="WindowScanOutput"/> objects
    /// </summary>
    internal interface IScanResultsAssembler
    {
        /// <summary>
        /// Assembles failed scans from the provided element
        /// </summary>
        /// <param name="element">Root element from which the scan output will be assembled</param>
        /// <returns>A WindowScanOutput object containing the relevant errors and error count</returns>
        WindowScanOutput AssembleWindowScanOutputFromElement(A11yElement element);
    }
}
