// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;

namespace AxeWindowsCLI
{
    internal static class ScanRunner
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
                .WithScreenCaptureAssembly(options.ScreenCaptureAssembly)
                .WithOutputDirectory(options.OutputDirectory)
                .WithOutputFileFormat(OutputFileFormat.A11yTest);

            return ScannerFactory.CreateScanner(builder.Build());
        }
    }
}
