// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace AxeWindowsScanner
{
    static class ScanRunner
    {
        public static ScanResults RunScan(IOptions options)
        {
            return BuildSampleResults();
            //IScanner scanner = BuildScanner(options);
            //return scanner.Scan(options.ScanId);
        }

        private static IScanner BuildScanner(IOptions options)
        {
            return null;
        }

        private static ScanResults BuildSampleResults()
        {
            List<ScanResult> errors = new List<ScanResult>
            {
                new ScanResult(),
                new ScanResult(),
                new ScanResult(),
            };

            return new ScanResults
            {
                Errors = errors,
                ErrorCount = errors.Count,
                OutputFile = new OutputFile { A11yTest = @"c:\foo\bar.a11ytest" },
            };
        }
    }
}
