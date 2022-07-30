// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
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
        /// If the value is not null, and the given directory does not exist, the <see cref="IScanner"/> object will attempt to create it.
        /// If the directory cannot be created, an <see cref="AxeWindowsAutomationException"/> will be thrown.
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

        ///  <summary>The path to a file containing configuration instructing Axe Windows how to interpret custom UI Automation data.</summary>
        public string CustomUIAConfigPath { get; private set; }

        /// <summary>
        /// Determines whether multiple UIA roots will be scanned or just the first one.
        /// </summary>
        public bool AreMultipleScanRootsEnabled { get; private set; }

        private Config()
        { }

#pragma warning disable CA1034 // Do not nest type
        /// <summary>
        /// Builds an instance of the <see cref="Config"/> class
        /// </summary>
        public class Builder
        {
            private readonly Config _config;

            private Builder(int processId)
            {
                _config = new Config
                {
                    ProcessId = processId
                };
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
            /// Specify the path to a custom configuration file instructing Axe Windows how to interpret custom UIA data.
            /// </summary>
            public Builder WithCustomUIAConfig(string path)
            {

                _config.CustomUIAConfigPath = path;
                return this;
            }

            /// <summary>
            /// Enables the scanning of multiple top level UIA roots.
            /// </summary>
            /// <returns></returns>
            public Builder WithMultipleScanRootsEnabled()
            {
                _config.AreMultipleScanRootsEnabled = true;
                return this;
            }

            /// <summary>
            /// Build an instance of <see cref="Config"/>
            /// </summary>
            /// <returns></returns>
            public Config Build()
            {
                return new Config
                {
                    ProcessId = _config.ProcessId,
                    OutputFileFormat = _config.OutputFileFormat,
                    OutputDirectory = _config.OutputDirectory,
                    CustomUIAConfigPath = _config.CustomUIAConfigPath,
                    AreMultipleScanRootsEnabled = _config.AreMultipleScanRootsEnabled,
                };
            }
        } // Builder
#pragma warning restore CA1034
    } // class
} // namespace
