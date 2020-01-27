// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AxeWindowsCLI
{
    public class OutputGenerator : IOutputGenerator
    {
        private readonly TextWriter _writer;

        private bool _bannerHasBeenShown;

        public OutputGenerator(TextWriter writer)
        {
            if (writer == null) throw new ArgumentNullException(nameof(writer));

            _writer = writer;
        }

        public void ShowOutput(IOptions options, IErrorCollector errorCollector, ScanResults scanResults)
        {
            bool failedToComplete = errorCollector.ParameterErrors.Any() || errorCollector.Exceptions.Any() || (scanResults == null);

            ShowBanner(options, failedToComplete ? VerbosityLevel.Quiet : VerbosityLevel.Default);
            if (failedToComplete)
            {
                ShowExecutionErrors(errorCollector);
            }
            else
            {
                ShowScanResults(options, scanResults);
            }
        }

        public void ShowBanner(IOptions options)
        {
            ShowBanner(options, VerbosityLevel.Default);
        }

        private void ShowBanner(IOptions options, VerbosityLevel miniumVerbosity)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if(!_bannerHasBeenShown && options.VerbosityLevel >= miniumVerbosity)
            {
                string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
                _writer.WriteLine("Axe.Windows Accessibility Scanner CLI (version {0})", version);

                bool haveProcessName = options.ProcessName != IProcessHelper.InvalidProcessName;
                bool haveProcessId = options.ProcessId != IProcessHelper.InvalidProcessId; 

                if (haveProcessName || haveProcessId)
                {
                    _writer.Write("Scan Target:");

                    if (haveProcessName)
                    {
                        _writer.Write(" Process Name = {0}", options.ProcessName);
                    }
                    if (haveProcessName && haveProcessId)
                    {
                        _writer.Write(",");
                    }
                    if (haveProcessId)
                    {
                        _writer.Write(" Process ID = {0}", options.ProcessId);
                    }
                    _writer.WriteLine();
                }
                if (!string.IsNullOrEmpty(options.ScanId))
                {
                    _writer.WriteLine("Scan Id = {0}", options.ScanId);
                }

                _bannerHasBeenShown = true;
            }
        }

        private void ShowExecutionErrors(IErrorCollector errorCollector)
        {
            foreach (string parameterError in errorCollector.ParameterErrors)
            {
                _writer.WriteLine("Parameter Error: {0}", parameterError);
            }

            foreach (Exception exception in errorCollector.Exceptions)
            {
                _writer.WriteLine("The following exception was caught: {0}", exception);
            }
        }

        private void ShowScanResults(IOptions options, ScanResults scanResults)
        {
            if (options.VerbosityLevel == VerbosityLevel.Quiet)
            {
                return;
            }

            if (scanResults.ErrorCount == 1)
            {
                _writer.WriteLine("1 error was found");
            }
            else
            {
                _writer.WriteLine("{0} errors were found", scanResults.ErrorCount);
            }

            if (options.VerbosityLevel >= VerbosityLevel.Verbose)
            {
                ShowVerboseResults(scanResults);
            }

            if (!string.IsNullOrEmpty(scanResults.OutputFile.A11yTest))
            {
                _writer.WriteLine("Results were written to \"{0}\"", scanResults.OutputFile.A11yTest);
            }
        }

        private void ShowVerboseResults(ScanResults scanResults)
        {
            int errorCount = 0;
            foreach (ScanResult scanResult in scanResults.Errors)
            {
                _writer.WriteLine("Error {0}: {1}", ++errorCount, scanResult.Rule.Description);
                if (scanResult.Element.Properties != null && scanResult.Element.Properties.Any())
                {
                    _writer.WriteLine("  Element Properties:");
                    foreach (KeyValuePair<string, string> pair in scanResult.Element.Properties)
                    {
                        _writer.WriteLine("    {0} = {1}", pair.Key, pair.Value);
                    }
                }
                if (scanResult.Element.Patterns != null && scanResult.Element.Patterns.Any())
                {
                    _writer.WriteLine("  Element Patterns:");
                    foreach (string pattern in scanResult.Element.Patterns)
                    {
                        _writer.WriteLine("    {0}", pattern);
                    }
                }
                _writer.WriteLine("----------------------------------------------------------------------");
            }
        }
    }
}
