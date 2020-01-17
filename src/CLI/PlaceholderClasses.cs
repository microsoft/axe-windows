// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Collections.Generic;

namespace AxeWindowsScanner
{
    public class RuleInfo
    {
    }

    public class ElementInfo
    {
    }

    public class OutputFile
    {
        public string A11yTest { get; set; }
        public string Sarif { get; set; }
    }

    public class ScanResult
    {
        public RuleInfo Rule { get; set; }
        public ElementInfo Element { get; set; }
    }

    public class ScanResults
    {
        public OutputFile OutputFile { get; set; }
        public int ErrorCount { get; set; }
        public IEnumerable<ScanResult> Errors { get; set; }
    }

    public interface IScanner
    {
        ScanResults Scan(string scanId);
    }
}
