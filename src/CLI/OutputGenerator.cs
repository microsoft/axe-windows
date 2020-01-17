// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace AxeWindowsScanner
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

        public void ShowOutput(int exitCode, IOptions options, IErrorCollector errorCollector, ScanResults scanResults)
        {
            bool scanCompleted = (exitCode == (int)ExitCode.ScanFoundErrors) ||
                (exitCode == (int)ExitCode.ScanFoundNoErrors);

            ShowBanner(options, scanCompleted ? VerbosityLevel.Default : VerbosityLevel.Quiet);
            if (scanCompleted)
            {
                ShowScanResults(options, scanResults);
            }
            else
            {
                ShowExecutionErrors(options, errorCollector);
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
                _writer.WriteLine("Axe.Windows Accessibility Scanner (version {0})", version);

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

        private void ShowExecutionErrors(IOptions options, IErrorCollector errorCollector)
        {

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
            foreach (ScanResult scanResult in scanResults.Errors)
            {
                _writer.WriteLine("Placeholder for error details");
            }
        }
    }
}
