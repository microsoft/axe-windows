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
        public string A11yTest { get; }
        public string Sarif { get; }
    }

    public class ScanResult
    {
        public RuleInfo Rule { get; }
        public ElementInfo Element { get; }
    }

    public class ScanResults
    {
        public OutputFile OutputFile { get; }
        public int ErrorCount { get; }
        public IEnumerable<ScanResult> Errors { get; }
    }

    public interface IScanner
    {
        ScanResults Scan(string scanId);
    }
}
