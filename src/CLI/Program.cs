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
        static IErrorCollector ErrorCollector = new ErrorCollector();
        static IOutputGenerator OutputGenerator = new OutputGenerator(Console.Out);
        static int Main(string[] args)
        {
            int exitCode = (int)ExitCode.ScanDidNotComplete;
            Options options = null;
            try
            {
                Options.ErrorCollector = ErrorCollector;
                Options.ProcessHelper = new ProcessHelper(new ProcessAbstraction(), ErrorCollector);

                string[] t = { "--verbosity", "default", "--scanid", "MyScan", "--outputdirectory", "abc", "--processname", "notepad++" };
                exitCode = Parser.Default.ParseArguments<Options>(t)
                    .MapResult((opts) =>
                    {
                        options = opts;
                        return HandleParsableInputs(opts);
                    },
                    errs => HandleNonParsableInputs(errs));
            }
            catch (Exception e)
            {
                ErrorCollector.AddException(e);
                exitCode = (int)ExitCode.ScanDidNotComplete;
            }

            OutputGenerator.ShowOutput(exitCode, options, ErrorCollector);
            return exitCode;
        }

        static int HandleNonParsableInputs(IEnumerable<Error> errs)
        {
            return (int)ExitCode.InvalidCommandLine;
        }

        static int HandleParsableInputs(IOptions options)
        {
            if (ErrorCollector.ParameterErrors.Any())
            {
                return (int)ExitCode.InvalidCommandLine;
            }

            try
            {
                OutputGenerator.ShowBanner(options);
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
