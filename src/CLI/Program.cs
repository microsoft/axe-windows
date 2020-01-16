// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using CommandLine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AxeWindowsScanner
{
    class Program
    {
        static ErrorCollector ErrorCollector = new ErrorCollector();

        static int Main(string[] args)
        {
            int exitCode = (int)ExitCode.ScanDidNotComplete;
            try
            {
                Options.ErrorCollector = ErrorCollector;
                Options.ProcessHelper = new ProcessHelper(new ProcessAbstraction(), ErrorCollector);

                string[] t = { "--verbosity", "verbose", "--outputdirectory", "abc", "--processname", "notepad++" };
                exitCode = Parser.Default.ParseArguments<Options>(t)
                    .MapResult((opts) => HandleParsableInputs(opts),
                    errs => HandleNonParsableInputs(errs));
            }
            catch (Exception e)
            {
                ErrorCollector.AddException(e);
                exitCode = (int)ExitCode.ScanDidNotComplete;
            }
            Console.WriteLine("Exit code : {0} ({1})", exitCode, (ExitCode)exitCode);
            return exitCode;
        }

        static int HandleNonParsableInputs(IEnumerable<Error> errs)
        {
            return (int)ExitCode.InvalidCommandLine;
        }

        static int HandleParsableInputs(Options options)
        {
            if (ErrorCollector.ParameterErrors.Any())
            {
                return (int)ExitCode.InvalidCommandLine;
            }

            try
            {
                ScanRunner.RunScan(options, ErrorCollector);

                if (ErrorCollector.ScanErrors.Any())
                {
                    return (int)ExitCode.ScanFoundErrors;
                }

                return (int)ExitCode.ScanFoundNoErrors;
            }
            catch (Exception e)
            {
                ErrorCollector.AddException(e);
            }
            return (int)ExitCode.ScanDidNotComplete;
        }
    }
}
