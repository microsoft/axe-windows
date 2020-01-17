// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AxeWindowsScanner
{
    class Program
    {
        static TextWriter Writer = Console.Out;
        static IErrorCollector ErrorCollector = new ErrorCollector();
        static IOutputGenerator OutputGenerator = new OutputGenerator(Writer);
        static ScanResults ScanResults = null;

        static int Main(string[] args)
        {
            int exitCode = (int)ExitCode.ScanDidNotComplete;
            Options options = null;
            try
            {
                Options.ErrorCollector = ErrorCollector;
                Options.ProcessHelper = new ProcessHelper(new ProcessAbstraction(), ErrorCollector);

                string[] t = { "--verbosity", "default", "--SCANID", "MyScan", "--outputdirectory", "abc", "--processname", "notepad++" };
                exitCode = CaseInsensitiveParser().ParseArguments<Options>(t)
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

            OutputGenerator.ShowOutput(exitCode, options, ErrorCollector, ScanResults);
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
                ScanResults = ScanRunner.RunScan(options);

                if (ScanResults.ErrorCount > 0)
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

        static Parser CaseInsensitiveParser()
        {
            // CommandLineParser is case-sensitive by default (intentional choice by the code
            // owners for better compatibility with *nix platforms). This removes the case
            // sensitivity and routes all output ot the same stream (Console.Out)
            return new Parser((settings) =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = Writer;
            });
        }
    }
}
