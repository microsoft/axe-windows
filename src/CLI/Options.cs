// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;

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

        [Option(Required = false, HelpText = "Display Third Party Notices (Opens in browser, does not execute scan)")]
        public bool ShowThirdPartyNotices { get; set; }

        // CommandLineParser will never set this value!
        public VerbosityLevel VerbosityLevel { get; set; } = VerbosityLevel.Default;
    }
}
