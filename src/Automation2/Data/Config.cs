// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Automation2
{
    /// <summary>
    /// Configuration options for creating an AxeWindows automation session
    /// </summary>
    public class Config
    {
        /// <summary>
        /// The process Id of the application to test.
        /// If the value is invalid, the automation session will throw an <see cref="AxeWindowsAutomationException"/>.
        /// </summary>
        public int ProcessId { get; private set; }

        /// <summary>
        /// The directory to which any output files should be written.
        /// This value is ignored if <see cref="Config.OutputFileFormat"/> is set to <see cref="OutputFileFormat.None"/>.
        /// If this value is null, and if <see cref="Config.OutputFileFormat"/> is not set to <see cref="OutputFileFormat.None"/>,
        /// files will be written to a directory named "AxeWindowsScanResults" under the current directory.
        /// If the value is not null, and the given directory does not exist, the automation session will throw an <see cref="AxeWindowsAutomationException"/>.
        /// </summary>
        /// <remarks>
        /// No output files will be written if no errors were found during the automation session.
        /// </remarks>
        public string OutputDirectory { get; private set; }

        /// <summary>
        /// Flags specifying the type of output file to create.
        /// Multiple values may be specified using the '|' (or) operator
        /// </summary>
        /// <remarks>
        /// No output files will be written if no errors were found during the automation session.
        /// </remarks>
        public OutputFileFormat OutputFileFormat { get; private set; }

        private Config()
        { }

        /// <summary>
        /// Builds an instance of the <see cref="Config"/> class
        /// </summary>
        public class Builder
        {
            private readonly Config _config;

            private Builder(int processId)
            {
                _config = new Config();
                _config.ProcessId = processId;
            }

            /// <summary>
            /// Create the builder for the specified process
            /// </summary>
            /// <param name="processId"></param>
            /// <returns></returns>
            public static Builder ForProcessId(int processId)
            {
                return new Builder(processId);
            }

            /// <summary>
            /// Specify the directory where any output files should be written
            /// </summary>
            /// <param name="directory"></param>
            /// <returns></returns>
            public Builder WithOutputDirectory(string directory)
            {
                _config.OutputDirectory = directory;
                return this;
            }

            /// <summary>
            /// Specify the type(s) of output files you wish AxeWindows to create
            /// </summary>
            /// <param name="format"></param>
            /// <returns></returns>
            public Builder WithOutputFileFormat(OutputFileFormat format)
            {
                _config.OutputFileFormat = format;
                return this;
            }

            /// <summary>
            /// Build an instance of <see cref="Config"/>
            /// </summary>
            /// <returns></returns>
            public Config Build()
            {
                return _config;
            }
        } // Builder
    } // class
} // namespace
