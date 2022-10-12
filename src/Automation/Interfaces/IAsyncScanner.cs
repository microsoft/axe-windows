// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation.Data;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Runs AxeWindows automated tests asynchronously
    /// This is the recommended interface for new applications
    /// </summary>
    public interface IAsyncScanner
    {
        /// <summary>
        /// Asynchronously run AxeWindows automated tests
        /// </summary>
        /// <param name="scanOptions">An instance of <see cref="ScanOptions"/>. If null, default options will be used.</param>
        /// <param name="cancellationToken">A cancellation token.</param>
        /// <remarks>
        /// If a value was provided in <see cref="Config.OutputDirectory"/>,
        /// output files may be written to the specified directory.
        /// (Note: no output files will be written if no errors were found.)
        /// An exception may be thrown if the value of <see cref="Config.ProcessId"/> is invalid
        /// or if the directory provided in <see cref="Config.OutputDirectory"/> cannot be created or accessed.
        /// </remarks>
        /// <returns>Information about the scan and any issues detected</returns>
        Task<ScanOutput> ScanAsync(ScanOptions scanOptions, CancellationToken cancellationToken);
    }
}
