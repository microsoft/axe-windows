// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Automation2
{
    /// <summary>
    /// Runs AxeWindows automated tests
    /// </summary>
    public interface IAutomationSession : IDisposable
    {
        /// <summary>
        /// Runs a single AxeWindows automated scan.
        /// Multiple scans may be run in a single automation session.
        /// If a value was provided in <see cref="IAutomationConfig.OutputDirectory"/>,
        /// output files may be written to the specified directory.
        /// (Note: no output files will be written if no errors were found.)
        /// An exception may be thrown if the value of <see cref="IAutomationConfig.ProcessId"/> is invalid
        /// or if the directory provided in <see cref="IAutomationConfig.OutputDirectory"/> does not exist.
        /// All exceptions are wrapped in <see cref="AxeWindowsAutomationException"/>.
        /// If the exception was not thrown by AxeWindows automation, the <see cref="Exception.InnerException"/> property
        /// will contain the exception.
        /// </summary>
        /// <returns>Information about the scan and any issues detected</returns>
        /// <exception cref="AxeWindowsAutomationException"/>
        IAutomationScanResults Scan();
    } // interface
} // namespace
