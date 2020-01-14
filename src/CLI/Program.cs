using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxeWindowsScanner
{
    class Program
    {
        static int Main(string[] args)
        {
            try
            {
                string[] t = { "--verbosity", "verbose", "--outputdirectory", "abc", "--scanid", "def", "--processid", "27604" };
                var result = Parser.Default.ParseArguments<Options>(t)
                    .MapResult((opts) => HandleParsableInputs(opts),
                    errs => HandleNonParsableInputs(errs));
                Console.WriteLine("Return code= {0}", result);
                return -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return 255;
        }

        static int HandleNonParsableInputs(IEnumerable<Error> errs)
        {
            var result = -2;
            Console.WriteLine("errors {0}", errs.Count());
            if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
                result = -1;
            foreach (Error error in errs)
            {
                Console.WriteLine(error);
            }
            Console.WriteLine("Exit code {0}", result);
            return result;
        }

        static int HandleParsableInputs(Options o)
        {
            var exitCode = 0;

            Console.WriteLine("Verbosity = {0}", o.Verbosity);
            Console.WriteLine("VerbosityLevel = {0}", o.VerbosityLevel);
            Console.WriteLine("VerbosityError = {0}", o.VerbosityError);
            Console.WriteLine("OutputFile = {0}", o.ScanId);
            Console.WriteLine("OutputDirectory = {0}", o.OutputDirectory);
            Console.WriteLine("ProcessId = {0}", o.ProcessId);
            Console.WriteLine("ProcessName = {0}", o.ProcessName);

            return exitCode;
        }
    }
}
