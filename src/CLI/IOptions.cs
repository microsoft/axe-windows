using System;
using System.Collections.Generic;
using System.Text;

namespace AxeWindowsScanner
{
    public interface IOptions
    {
        string OutputDirectory { get; }
        string ScanId { get; }
        int ProcessId { get; }
        string ProcessName { get; }
        VerbosityLevel VerbosityLevel { get; }
    }
}
