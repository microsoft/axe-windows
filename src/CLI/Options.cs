// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;
using System;

namespace AxeWindowsCLI
{
    public class Options : IOptions
    {
        [Option(Required = false, HelpText = "Process Id")]
        public int ProcessId { get; set; }

        [Option(Required = false, HelpText = "Process Name")]
        public string ProcessName { get; set; }

        [Option(Required = false, HelpText = "Output directory")]
        public string OutputDirectory { get; set; }

        [Option(Required = false, HelpText = "Scan ID")]
        public string ScanId { get; set; }

        [Option(Required = false, HelpText = "Verbosity level (Quiet/Default/Verbose)")]
        public string Verbosity { get; set; }

        [Option(Required = false, HelpText = "Display Third Party Notices (opens file in browser without executing scan). If specified, all other options will be ignored.")]
        public bool ShowThirdPartyNotices { get; set; }

        [Option(Required = false, HelpText = "How many seconds to delay before triggering the scan. Valid range is 0 to 60 seconds, with a default of 0.")]
        public int DelayInSeconds { get; set; }

        [Option(Required = false, HelpText = "Full path to an assembly that exposes a public implementation of the IBitmapCreator interface for use with this scan.")]
        public string ScreenCaptureAssembly { get; set; }

        // CommandLineParser will never set this value!
        public VerbosityLevel VerbosityLevel { get; set; } = VerbosityLevel.Default;

        public Options Clone()
        {
            Options output = new Options();
            foreach (var property in typeof(Options).GetProperties())
            {
                property.SetValue(output, property.GetValue(this));
            }
            return output;
        }
    }
}
