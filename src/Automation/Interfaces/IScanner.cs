﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Runs AxeWindows automated tests
    /// </summary>
    public interface IScanner
    {
        /// <summary>
        /// Run AxeWindows automated tests
        /// </summary>
        /// <remarks>
        /// If a value was provided in <see cref="Config.OutputDirectory"/>,
        /// output files may be written to the specified directory.
        /// (Note: no output files will be written if no errors were found.)
        /// An exception may be thrown if the value of <see cref="Config.ProcessId"/> is invalid
        /// or if the directory provided in <see cref="Config.OutputDirectory"/> does not exist.
        /// All exceptions are wrapped in <see cref="AxeWindowsAutomationException"/>.
        /// If the exception was not thrown by AxeWindows automation, the <see cref="Exception.InnerException"/> property
        /// will contain the exception.
        /// </remarks>
        /// <returns>Information about the scan and any issues detected</returns>
        /// <exception cref="AxeWindowsAutomationException"/>
        ScanResults Scan();
    } // interface
} // namespace
