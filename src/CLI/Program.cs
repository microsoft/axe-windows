// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using CommandLine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AxeWindowsCLI
{
    class Program
    {
        const int IgnoredValue = 0;

        static TextWriter Writer = Console.Out;
        static IProcessHelper processHelper = new ProcessHelper(new ProcessAbstraction());
        static IOutputGenerator OutputGenerator = new OutputGenerator(Writer);
        static ScanResults ScanResults = null;

        static int Main(string[] args)
        {
            Exception caughtException = null;
            Options options = null;
            try
            {
                CaseInsensitiveParser().ParseArguments<Options>(args)
                    .WithParsed<Options>(Run);
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            if (options != null)
            {
                OutputGenerator.WriteOutput(options, ScanResults, caughtException);
            }
            return ReturnValueChooser.GetReturnValue(ScanResults, caughtException);
        }

        static void Run(IOptions options)
        {
            OutputGenerator.WriteBanner(options);
            ScanResults = ScanRunner.RunScan(options);
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
