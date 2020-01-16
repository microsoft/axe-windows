// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AxeWindowsScanner
{
    static class ScanRunner
    {
        public static void RunScan(IOptions options, IErrorCollector errorCollector)
        {
            //IScanner scanner = BuildScanner(options);
            //ScanResults results = scanner.Scan(options.ScanId);
            //foreach (ScanResult error in results.Errors)
            //{
            //    errorCollector.AddScanError("*** Need to decide what format we display ***");
            //}
        }

        private static IScanner BuildScanner(IOptions options)
        {
            return null;
        }
    }
}
