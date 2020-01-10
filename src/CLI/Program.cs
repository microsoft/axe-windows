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
            string[] t = { "-verbose", "-quiet", "-d", "abc", "-f", "def", "-i", "111", "-n", "excel.exe" };
			var result = Parser.Default.ParseArguments<CommandLineOptions>(t).MapResult((opts) => RunOptionsAndReturnExitCode(opts), //in case parser sucess
				errs => HandleParseError(errs)); //in case parser fails
			Console.WriteLine("Return code= {0}", result);
			return -1;
		}

		static int RunOptionsAndReturnExitCode(CommandLineOptions o)
		{
			var exitCode = 0;

			Console.WriteLine("Verbose = {0}", o.Verbose);
			Console.WriteLine("Quiet = {0}", o.Quiet);
			Console.WriteLine("OutputFile = {0}", o.File);
			Console.WriteLine("Directory = {0}", o.Directory);
			Console.WriteLine("Id = {0}", o.Id);
			Console.WriteLine("Name = {0}", o.Name);

			return exitCode;
		}

		//in case of errors or --help or --version
		static int HandleParseError(IEnumerable<Error> errs)
		{
			var result = -2;
			Console.WriteLine("errors {0}", errs.Count());
			if (errs.Any(x => x is HelpRequestedError || x is VersionRequestedError))
				result = -1;
			Console.WriteLine("Exit code {0}", result);
			return result;
		}
    }
}
