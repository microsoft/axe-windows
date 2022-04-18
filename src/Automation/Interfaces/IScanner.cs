// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Runs AxeWindows automated tests
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Run AxeWindows automated tests on the first window found
        /// </summary>
        /// <remarks>
        /// If a value was provided in <see cref="Config.OutputDirectory"/>,
        /// output files may be written to the specified directory.
        /// (Note: no output files will be written if no errors were found.)
        /// An exception may be thrown if the value of <see cref="Config.ProcessId"/> is invalid
        /// or if the directory provided in <see cref="Config.OutputDirectory"/> cannot be created or accessed.
        /// All exceptions are wrapped in <see cref="AxeWindowsAutomationException"/>.
        /// If the exception was not thrown by AxeWindows automation, the <see cref="Exception.InnerException"/> property
        /// will contain the exception.
        /// Calling this function is equivalent to passing false to <see cref="IScanner.Scan(bool)"/> and passing false for <c>enableMultipleWindows</c>.
        /// </remarks>
        /// <returns>Information about the scan and any issues detected</returns>
        /// <exception cref="AxeWindowsAutomationException"/>
        ScanResults Scan();

        /// <summary>
        /// Run AxeWindows automated tests on all windows or the first window found.
        /// </summary>
        /// <remarks>
        /// If a value was provided in <see cref="Config.OutputDirectory"/>,
        /// output files may be written to the specified directory.
        /// (Note: no output files will be written if no errors were found.)
        /// An exception may be thrown if the value of <see cref="Config.ProcessId"/> is invalid
        /// or if the directory provided in <see cref="Config.OutputDirectory"/> cannot be created or accessed.
        /// All exceptions are wrapped in <see cref="AxeWindowsAutomationException"/>.
        /// If the exception was not thrown by AxeWindows automation, the <see cref="Exception.InnerException"/> property
        /// will contain the exception.
        /// If <paramref name="enableMultipleWindows"/> is set to true, all windows will be scanned. If it is set to false, only the first window will be scanned.
        /// </remarks>
        /// <returns>Information about the scan and any issues detected</returns>
        /// <exception cref="AxeWindowsAutomationException"/>
        IReadOnlyCollection<ScanResults> Scan(bool enableMultipleWindows);


        /// <summary>
        /// Run AxeWindows automated tests
        /// </summary>
        /// <param name="scanId">The ID of this scan. Must be null or meet the requirements for a file name.</param>
        /// <remarks>
        /// If a value was provided in <see cref="Config.OutputDirectory"/>,
        /// output files may be written to the specified directory.
        /// (Note: no output files will be written if no errors were found.)
        /// An exception may be thrown if the value of <see cref="Config.ProcessId"/> is invalid
        /// or if the directory provided in <see cref="Config.OutputDirectory"/> cannot be created or accessed.
        /// All exceptions are wrapped in <see cref="AxeWindowsAutomationException"/>.
        /// If the exception was not thrown by AxeWindows automation, the <see cref="Exception.InnerException"/> property
        /// will contain the exception.
        /// Calling this function is equivalent to passing false to <see cref="IScanner.Scan(string, bool)"/> and passing false for <c>enableMultipleWindows</c>.
        /// </remarks>
        /// <returns>Information about the scan and any issues detected</returns>
        /// <exception cref="AxeWindowsAutomationException"/>
        ScanResults Scan(string scanId);

        /// <summary>
        /// Run AxeWindows automated tests on all windows or the first window found.
        /// </summary>
        /// <remarks>
        /// If a value was provided in <see cref="Config.OutputDirectory"/>,
        /// output files may be written to the specified directory.
        /// (Note: no output files will be written if no errors were found.)
        /// An exception may be thrown if the value of <see cref="Config.ProcessId"/> is invalid
        /// or if the directory provided in <see cref="Config.OutputDirectory"/> cannot be created or accessed.
        /// All exceptions are wrapped in <see cref="AxeWindowsAutomationException"/>.
        /// If the exception was not thrown by AxeWindows automation, the <see cref="Exception.InnerException"/> property
        /// will contain the exception.
        /// If <paramref name="enableMultipleWindows"/> is set to true, all windows will be scanned. If it is set to false, only the first window will be scanned.
        /// </remarks>
        /// <returns>Information about the scan and any issues detected</returns>
        /// <exception cref="AxeWindowsAutomationException"/>
        IReadOnlyCollection<ScanResults> Scan(string scanId, bool enableMultipleWindows);
    } // interface
} // namespace
