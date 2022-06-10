// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using CommandLine;
using CommandLine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace AxeWindowsCLI
{
    class Program
    {
        private readonly string[] _args;
        private readonly TextWriter _writer;
        private readonly IProcessHelper _processHelper;
        private readonly IOutputGenerator _outputGenerator;
        private readonly IBrowserAbstraction _browserAbstraction;
        private readonly IScanDelay _scanDelay;

        private IReadOnlyCollection<ScanResults> _scanResultsCollection;
        private IOptions _options;

        public Program(string[] args,
            TextWriter writer,
            IProcessHelper processHelper,
            IOutputGenerator outputGenerator,
            IBrowserAbstraction browserAbstraction,
            IScanDelay scanDelay)
        {
            _args = args;
            _writer = writer;
            _processHelper = processHelper;
            _outputGenerator = outputGenerator;
            _browserAbstraction = browserAbstraction;
            _scanDelay = scanDelay;
        }

        static int Main(string[] args)
        {
            TextWriter writer = Console.Out;
            IProcessHelper processHelper = new ProcessHelper(new ProcessAbstraction());
            IScanDelay scanDelay = new ScanDelay(writer, () => Thread.Sleep(TimeSpan.FromSeconds(1)));
            IOutputGenerator outputGenerator = new OutputGenerator(writer);
            IBrowserAbstraction browserAbstraction = new BrowserAbstraction();

            // We require some parameters, but don't have a clean way to tell CommandLineParser
            // about them. As a result, we don't get the default help text if the args list
            // is empty. This little trick forces help in this case
            string[] innerArgs = args.Length > 0 ? args : new string[] { "--help" };
            Program program = new Program(innerArgs, writer, processHelper, outputGenerator, browserAbstraction, scanDelay);
            return program.Run();
        }

        private int Run()
        {
            Exception caughtException = null;
            try
            {
                using (var parser = CaseInsensitiveParser())
                {
                    ParserResult<Options> parserResult = parser.ParseArguments<Options>(_args);
                    parserResult.WithParsed(RunWithParsedInputs)
                        .WithNotParsed(_ =>
                        {
                            HelpText helpText = HelpText.AutoBuild(parserResult, h =>
                            {
                                h.AutoHelp = false;     // hides --help
                                h.AutoVersion = false;  // hides --version
                                return HelpText.DefaultParsingErrorsHandler(parserResult, h);
                            }, e => e);
                            _writer.WriteLine(helpText);
                        });
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
                if (_scanResultsCollection == null)
                {
                    _outputGenerator.WriteOutput(_options, null, caughtException);
                }
                else
                {
                    foreach (var scanResult in _scanResultsCollection)
                    {
                        _outputGenerator.WriteOutput(_options, scanResult, caughtException);
                    }
                }
            }
            return ReturnValueChooser.GetReturnValue(_scanResultsCollection, caughtException);
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
            _scanDelay.DelayWithCountdown(_options);
            _scanResultsCollection = ScanRunner.RunScan(_options);
        }

        private static Parser CaseInsensitiveParser()
        {
            // CommandLineParser is case-sensitive by default (intentional choice by the code
            // owners for better compatibility with *nix platforms). This removes the case
            // sensitivity and disables default output so we can override it
            return new Parser((settings) =>
            {
                settings.CaseSensitive = false;
                settings.HelpWriter = null;
            });
        }

        private void HandleThirdPartyNoticesAndExit()
        {
            string pathToFile = Path.Combine(AppContext.BaseDirectory, "thirdpartynotices.html");
            _outputGenerator.WriteThirdPartyNoticeOutput(pathToFile);

            _browserAbstraction.Open(pathToFile);
            Environment.Exit(ReturnValueChooser.ThirdPartyNoticesDisplayed);
        }
    }
}
