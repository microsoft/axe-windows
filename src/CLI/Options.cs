using CommandLine;
using System;
using System.IO;

namespace AxeWindowsScanner
{
    public class Options
    {
        [Option(Required = false, HelpText = "Output directory")]
        public string OutputDirectory => _outputDirectory;

        [Option(Required = false, HelpText = "Scan ID")]
        public string ScanId => _scanId;

        [Option(Group = "Target", Required = false, HelpText = "Process Id")]
        public int ProcessId => _processId;

        [Option(Group = "Target", Required = false, HelpText = "Process Name")]
        public string ProcessName => _processName;

        [Option(Required = false, HelpText = "Verbosity level (Quiet/Default/Verbose)")]
        public string Verbosity => _verbosity;

        public VerbosityLevel VerbosityLevel { get; }
        public bool VerbosityError { get; }

        readonly string _outputDirectory;
        readonly string _scanId;
        readonly int _processId;
        readonly string _processName;
        readonly string _verbosity;

        public Options(string outputDirectory, string scanId, int processId, string processName, string verbosity)
        {
            _outputDirectory = outputDirectory;
            _scanId = scanId;
            _verbosity = verbosity;

            bool verbosityExists = !string.IsNullOrEmpty(verbosity);

            if (verbosityExists
                && Enum.TryParse<VerbosityLevel>(verbosity, true, out VerbosityLevel level))
            {
                VerbosityLevel = level;
            }
            else
            {
                VerbosityLevel = VerbosityLevel.Default;
                VerbosityError = verbosityExists;
            }

            if (processId != 0)
            {
                _processId = processId; 
                _processName = ProcessHelper.FindProcessById(processId);
            }
            else
            {
                string p = Path.GetFileNameWithoutExtension(processName);
                _processName = p;
                _processId = ProcessHelper.FindProcessByName(p);
            }
        }
    }
}
