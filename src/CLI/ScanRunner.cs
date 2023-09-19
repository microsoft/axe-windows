// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using Axe.Windows.Automation.Data;
using System.Collections.Generic;

namespace AxeWindowsCLI
{
    internal static class ScanRunner
    {
        public static IReadOnlyCollection<WindowScanOutput> RunScan(IOptions options)
        {
            IScanner scanner = BuildScanner(options);
            return scanner.Scan(new ScanOptions(options.ScanId)).WindowScanOutputs;
        }

        private static IScanner BuildScanner(IOptions options)
        {
            Config.Builder builder = Config.Builder
                .ForProcessId(options.ProcessId)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithCustomUIAConfig(options.CustomUia);

            if (!string.IsNullOrEmpty(options.OutputDirectory))
                builder = builder.WithOutputDirectory(options.OutputDirectory);

            if (options.AlwaysSaveTestFile)
                builder = builder.WithAlwaysSaveTestFile();

            if (options.TestAllChromiumContent)
                builder = builder.WithTestAllChromiumContent();

            return ScannerFactory.CreateScanner(builder.Build());
        }
    }
}
