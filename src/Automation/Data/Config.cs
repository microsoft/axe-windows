// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.SystemAbstractions;

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
        /// The assembly containing the IBitmapCreator implementation.
        /// If this value is null, the default implementation will be used.
        /// If this value is non-null, but does not expose an appropriate implementation,
        /// an <see cref="AxeWindowsAutomationException"/> will be thrown.
        /// </summary>
        public string ScreenCaptureAssembly { get; private set; }

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

        internal Config Clone()
        {
            Config output = new Config();
            foreach (var property in typeof(Config).GetProperties())
            {
                property.SetValue(output, property.GetValue(this));
            }
            return output;
        }

#pragma warning disable CA1034 // Do not nest type
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
            /// <param name="processId">The numeric process ID</param>
            public static Builder ForProcessId(int processId)
            {
                return new Builder(processId);
            }

            /// <summary>
            /// Specify the directory where any output files should be written
            /// </summary>
            /// <param name="directory">The output directory</param>
            public Builder WithOutputDirectory(string directory)
            {
                _config.OutputDirectory = directory;
                return this;
            }

            /// <summary>
            /// Specify the type(s) of output files you wish AxeWindows to create
            /// </summary>
            /// <param name="format">The output format</param>
            public Builder WithOutputFileFormat(OutputFileFormat format)
            {
                _config.OutputFileFormat = format;
                return this;
            }

            /// <summary>
            /// Specify the screen capture assembly override
            /// </summary>
            /// <param name="assemblyFullName">The full path and name of the assembly that exposes the <see cref="IBitmapCreator"></see> interface./></param>
            public Builder WithScreenCaptureAssembly(string assemblyFullName)
            {
                _config.ScreenCaptureAssembly = assemblyFullName;
                return this;
            }

            /// <summary>
            /// Build an instance of <see cref="Config"/>
            /// </summary>
            public Config Build()
            {
                return _config.Clone();
            }
        } // Builder
        #pragma warning restore CA1034
    } // class
} // namespace
