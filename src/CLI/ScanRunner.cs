﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System.Collections.Generic;

namespace AxeWindowsCLI
{
    internal static class ScanRunner
    {
        public static IReadOnlyCollection<ScanResults> RunScan(IOptions options)
        {
            IScanner scanner = BuildScanner(options);
            return scanner.Scan(options.ScanId, options.ScanMultipleWindows);
        }

        private static IScanner BuildScanner(IOptions options)
        {
            Config.Builder builder = Config.Builder
                .ForProcessId(options.ProcessId)
                .WithOutputFileFormat(OutputFileFormat.A11yTest)
                .WithCustomUIAConfig(options.CustomUia);

            if (!string.IsNullOrEmpty(options.OutputDirectory))
                builder = builder.WithOutputDirectory(options.OutputDirectory);

            return ScannerFactory.CreateScanner(builder.Build());
        }
    }
}
