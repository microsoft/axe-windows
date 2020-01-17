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
            Options options = null;
            try
            {
                Options.ErrorCollector = ErrorCollector;
                Options.ProcessHelper = new ProcessHelper(new ProcessAbstraction(), ErrorCollector);

                string[] t = { "--verbosity", "default", "--SCANID", "MyScan", "--outputdirectory", "abc", "--processname", "notepad++" };
                CaseInsensitiveParser().ParseArguments<Options>(t)
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
            }

            OutputGenerator.ShowOutput(options, ErrorCollector, ScanResults);
            return ReturnValueChooser.GetReturnValue(ErrorCollector, ScanResults);
        }

        static int HandleNonParsableInputs(IEnumerable<Error> errors)
        {
            foreach (Error error in errors)
            {
                if (error.Tag != ErrorType.HelpRequestedError &&
                    error.Tag != ErrorType.HelpVerbRequestedError)
                {
                    ErrorCollector.AddParameterError("Command line error: " + error.Tag);
                }
            }
            return 0;  // Return value is ignored
        }

        static int HandleParsableInputs(IOptions options)
        {
            if (!ErrorCollector.ParameterErrors.Any())
            {
                try
                {
                    OutputGenerator.ShowBanner(options);
                    ScanResults = ScanRunner.RunScan(options);

                }
                catch (Exception e)
                {
                    ErrorCollector.AddException(e);
                }
            }
            return 0;  // Return value is ignored
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
