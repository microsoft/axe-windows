using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace AxeWindowsScanner
{
    public class CommandLineOptions
    {
        [Option('d', "directory", Required = false, HelpText = "Output file")]
        public string Directory { get; set; }

        [Option('f', "file", Required = false, HelpText = "Output file")]
        public string File { get; set; }

        [Option('i', "id", Required = false, HelpText = "Process Id")]
        public int Id { get; set; }

        [Option('n', "name", Required = false, HelpText = "Process name")]
        public string Name { get; set; }

        [Option('v', "verbose", Required = false, HelpText = "Use verbose output")]
        public bool Verbose { get; set; }

        [Option('q', "quiet", Required = false, HelpText = "Use quiet output")]
        public bool Quiet { get; set; }
    }
}
