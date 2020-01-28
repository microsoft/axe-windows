// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using CommandLine;
using System;
using System.IO;

namespace AxeWindowsCLI
{
    class Program
    {
        private readonly string[] _args;
        private readonly TextWriter _writer;
        private readonly IProcessHelper _processHelper;
        private readonly IOutputGenerator _outputGenerator;

        private ScanResults _scanResults = null;
        private IOptions _options = null;

        public Program(string[] args,
            TextWriter writer,
            IProcessHelper processHelper,
            IOutputGenerator outputGenerator)
        {
            _args = args;
            _writer = writer;
            _processHelper = processHelper;
            _outputGenerator = outputGenerator;
        }

        static int Main(string[] args)
        {
            TextWriter writer = Console.Out;
            IProcessHelper processHelper = new ProcessHelper(new ProcessAbstraction());
            IOutputGenerator outputGenerator = new OutputGenerator(writer);

            Program program = new Program(args, writer, processHelper, outputGenerator);
            return program.Run();
        }

        private int Run()
        {
            Exception caughtException = null;
            try
            {
                CaseInsensitiveParser().ParseArguments<Options>(_args)
                    .WithParsed<Options>(RunWithParsedInputs);
            }
            catch (Exception e)
            {
                caughtException = e;
            }

            if (_options != null)
            {
                _outputGenerator.WriteOutput(_options, _scanResults, caughtException);
            }
            return ReturnValueChooser.GetReturnValue(_scanResults, caughtException);
        }

        void RunWithParsedInputs(Options options)
        {
            // OptionsEvaluator will throw if the inputs are invalid, so save
            // them before validation, then save the updated values after validation
            _options = options;
            _options = OptionsEvaluator.ProcessInputs(options, _processHelper);
            _outputGenerator.WriteBanner(_options);
            _scanResults = ScanRunner.RunScan(_options);
        }

        private Parser CaseInsensitiveParser()
        {
            // CommandLineParser is case-sensitive by default (intentional choice by the code
            // owners for better compatibility with *nix platforms). This removes the case
            // sensitivity and routes all output ot the same stream (Console.Out)
            return new Parser((settings) =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = _writer;
            });
        }
    }
}
