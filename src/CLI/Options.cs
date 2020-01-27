// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;
using System;
using System.IO;

namespace AxeWindowsCLI
{
    public class Options : IOptions
    {
        [Option(Required = false, HelpText = "Output directory")]
        public string OutputDirectory => _outputDirectory;

        [Option(Required = false, HelpText = "Scan ID")]
        public string ScanId => _scanId;

        [Option(Group = "Target", Required = false, HelpText = "Process Id")]
        public int ProcessId => _processId;

        [Option(Group = "Target", Required = false, HelpText = "Process Name")]
        public string ProcessName => _processName;

        [Option(Required = false, HelpText = "Verbosity level (Quiet/Default/Verbose)")]
        public string Verbosity => string.Empty;  // Exists only for CommandLine purposes

        public VerbosityLevel VerbosityLevel { get; }

        // We don't control construction of this class, so we use static Dependency Injection
        public static IErrorCollector ErrorCollector { get; set; }
        public static IProcessHelper ProcessHelper { get; set; }

        readonly string _outputDirectory;
        readonly string _scanId;
        readonly int _processId;
        readonly string _processName;

        public Options(string outputDirectory, string scanId, int processId, string processName, string verbosity)
        {
            _outputDirectory = outputDirectory;
            _scanId = scanId;
            VerbosityLevel = VerbosityLevel.Default;  // Until proven otherwise

            if (!string.IsNullOrEmpty(verbosity))
            {
                if (Enum.TryParse<VerbosityLevel>(verbosity, true, out VerbosityLevel level))
                {
                    VerbosityLevel = level;
                }
                else
                {
                    ErrorCollector.AddParameterError("Invalid verbosity level: " + verbosity);
                }
            }

            if (processId != 0)
            {
                _processId = processId; 
                _processName = ProcessHelper.FindProcessById(processId);
            }
            else
            {
                string p = Path.GetFileNameWithoutExtension(processName);
                _processName = p;
                _processId = ProcessHelper.FindProcessByName(p);
            }
        }
    }
}
