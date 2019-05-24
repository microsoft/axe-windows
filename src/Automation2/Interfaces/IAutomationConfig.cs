// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Automation2
{
    /// <summary>
    /// Configuration options for creating an AxeWindows automation session
    /// </summary>
    public interface IAutomationConfig
    {
        /// <summary>
        /// The process Id of the application to test.
        /// If the value is invalid, the automation session will throw an <see cref="AxeWindowsAutomationException"/>.
        /// </summary>
        int ProcessId { get; set; }

        /// <summary>
        /// The directory to which any output files should be written.
        /// This value is ignored if <see cref="IAutomationConfig.OutputFileFormat"/> is set to <see cref="OutputFileFormat.None"/>.
        /// If this value is null, and if <see cref="IAutomationConfig.OutputFileFormat"/> is not set to <see cref="OutputFileFormat.None"/>,
        /// files will be written to a directory named "AxeWindowsScanResults" under the current directory.
        /// If the value is not null, and the given directory does not exist, the automation session will throw an <see cref="AxeWindowsAutomationException"/>.
        /// </summary>
        /// <remarks>
        /// No output files will be written if no errors were found during the automation session.
        /// </remarks>
        string OutputDirectory { get; set; }

        /// <summary>
        /// Flags specifying the type of output file to create.
        /// Multiple values may be specified using the '|' (or) operator
        /// </summary>
        /// <remarks>
        /// No output files will be written if no errors were found during the automation session.
        /// </remarks>
        OutputFileFormat OutputFileFormat { get; set; }

        /// <summary>
        /// Set to true when running an automation session in PowerShell.
        /// Necessary because PowerShell requires a particular set of dependency assemblies.
        /// </summary>
        bool IsRunningInPowerShell { get; set; }
    } // interface
} // namespace
