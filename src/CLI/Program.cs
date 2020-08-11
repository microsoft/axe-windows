// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using CommandLine;
using System;
using System.IO;
using System.Reflection;

namespace AxeWindowsCLI
{
    class Program
    {
        private readonly string[] _args;
        private readonly TextWriter _writer;
        private readonly IProcessHelper _processHelper;
        private readonly IOutputGenerator _outputGenerator;
        private readonly IBrowserAbstraction _browserAbstraction;

        private ScanResults _scanResults;
        private IOptions _options;

        public Program(string[] args,
            TextWriter writer,
            IProcessHelper processHelper,
            IOutputGenerator outputGenerator,
            IBrowserAbstraction browserAbstraction)
        {
            _args = args;
            _writer = writer;
            _processHelper = processHelper;
            _outputGenerator = outputGenerator;
            _browserAbstraction = browserAbstraction;
        }

        static int Main(string[] args)
        {
            TextWriter writer = Console.Out;
            IProcessHelper processHelper = new ProcessHelper(new ProcessAbstraction());
            IOutputGenerator outputGenerator = new OutputGenerator(writer);
            IBrowserAbstraction browserAbstraction = new BrowserAbstraction();

            // We require some parameters, but don't have a clean way to tell CommandLineParser
            // about them. As a result, we don't get the default help text if the args list
            // is empty. This little trick forces help in this case
            string[] innerArgs = args.Length > 0 ? args : new string[] { "--help" };
            Program program = new Program(innerArgs, writer, processHelper, outputGenerator, browserAbstraction);
            return program.Run();
        }

        private int Run()
        {
            Exception caughtException = null;
            try
            {
                using (var parser = CaseInsensitiveParser())
                {
                parser.ParseArguments<Options>(_args)
                    .WithParsed<Options>(RunWithParsedInputs);
                }
            }
#pragma warning disable CA1031
            catch (Exception e)
#pragma warning restore CA1031
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
            // Quick exit if we display 3rd party stuff
            if (options.ShowThirdPartyNotices)
            {
                HandleThirdPartyNoticesAndExit();
            }

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

        private void HandleThirdPartyNoticesAndExit()
        {
            string currentFolder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string pathToFile = Path.Combine(currentFolder, "thirdpartynotices.html");
            _outputGenerator.WriteThirdPartyNoticeOutput(pathToFile);

            _browserAbstraction.Open(pathToFile);
            Environment.Exit(ReturnValueChooser.ThirdPartyNoticesDisplayed);
        }
    }
}
