// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;

namespace AxeWindowsScanner
{
    static class ScanRunner
    {
        public static ScanResults RunScan(IOptions options)
        {
            IScanner scanner = BuildScanner(options);
            return scanner.Scan(options.ScanId);
        }

        private static IScanner BuildScanner(IOptions options)
        {
            Config.Builder builder = Config.Builder
                .ForProcessId(options.ProcessId)
                .WithOutputFileFormat(OutputFileFormat.A11yTest);

            if (!string.IsNullOrEmpty(options.OutputDirectory))
                builder = builder.WithOutputDirectory(options.OutputDirectory);

            return ScannerFactory.CreateScanner(builder.Build());
        }
    }
}
