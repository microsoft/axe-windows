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
        public string OutputDirectory { get; set; }

        [Option(Required = false, HelpText = "Scan ID")]
        public string ScanId { get; set; }

        [Option(Group = "Target", Required = false, HelpText = "Process Id")]
        public int ProcessId { get; set; }

        [Option(Group = "Target", Required = false, HelpText = "Process Name")]
        public string ProcessName { get; set; }

        [Option(Required = false, HelpText = "Verbosity level (Quiet/Default/Verbose)")]
        public string Verbosity { get; set; }

        // CommandLineParser will never set this value!
        public VerbosityLevel VerbosityLevel { get; set;  }
    }
}
