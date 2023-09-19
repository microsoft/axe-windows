// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using CommandLine;

namespace AxeWindowsCLI
{
    public class Options : IOptions
    {
        // The HelpText value specified is the name of the resource string in the OptionsHelpText class
        [Option(Required = false, HelpText = "ProcessId", ResourceType = typeof(Resources.OptionsHelpText))]
        public int ProcessId { get; set; }

        [Option(Required = false, HelpText = "ProcessName", ResourceType = typeof(Resources.OptionsHelpText))]
        public string ProcessName { get; set; }

        [Option(Required = false, HelpText = "OutputDirectory", ResourceType = typeof(Resources.OptionsHelpText))]
        public string OutputDirectory { get; set; }

        [Option(Required = false, HelpText = "ScanId", ResourceType = typeof(Resources.OptionsHelpText))]
        public string ScanId { get; set; }

        [Option(Required = false, HelpText = "Verbosity", ResourceType = typeof(Resources.OptionsHelpText))]
        public string Verbosity { get; set; }

        [Option(Required = false, HelpText = "ShowThirdPartyNotices", ResourceType = typeof(Resources.OptionsHelpText))]
        public bool ShowThirdPartyNotices { get; set; }

        [Option(Required = false, HelpText = "DelayInSeconds", ResourceType = typeof(Resources.OptionsHelpText))]
        public int DelayInSeconds { get; set; }

        [Option(Required = false, HelpText = "CustomUia", ResourceType = typeof(Resources.OptionsHelpText))]
        public string CustomUia { get; set; }

        [Option(Required = false, HelpText = "AlwaysSaveTestFile", ResourceType = typeof(Resources.OptionsHelpText))]
        public bool AlwaysSaveTestFile { get; set; }

        [Option(Required = false, HelpText = "TestAllChromiumContent", ResourceType = typeof(Resources.OptionsHelpText))]
        public bool TestAllChromiumContent { get; set; }

        // CommandLineParser will never set this value!
        public VerbosityLevel VerbosityLevel { get; set; } = VerbosityLevel.Default;
    }
}
