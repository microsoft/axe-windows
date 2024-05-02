// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using AxeWindowsCLI.Resources;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace AxeWindowsCLI
{
    internal class OutputGenerator : IOutputGenerator
    {
        private readonly TextWriter _writer;

        private bool _bannerHasBeenShown;

        public OutputGenerator(TextWriter writer)
        {
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public void WriteOutput(IOptions options, WindowScanOutput scanOut, Exception caughtException)
        {
            bool failedToComplete = caughtException != null || scanOut == null;

            WriteBanner(options, failedToComplete ? VerbosityLevel.Quiet : VerbosityLevel.Default);
            if (failedToComplete)
            {
                WriteExecutionErrors(caughtException);
            }
            else
            {
                WriteWindowScanOutput(options, scanOut);
            }
        }

        public void WriteBanner(IOptions options)
        {
            WriteBanner(options, VerbosityLevel.Default);
        }

        public void WriteThirdPartyNoticeOutput(string pathToFile)
        {
            WriteAppBanner();
            _writer.WriteLine(DisplayStrings.ThirdPartyNoticeFormat, pathToFile);
        }

        private void WriteAppBanner()
        {
#pragma warning disable IL3000 // We don't use a single file installer
            string version = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).ProductVersion;
            _writer.WriteLine(DisplayStrings.VersionFormat, version);
#pragma warning restore IL3000 // We don't use a single file installer
        }

        private void WriteBanner(IOptions options, VerbosityLevel minimumVerbosity)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            if (!_bannerHasBeenShown && options.VerbosityLevel >= minimumVerbosity)
            {
                WriteAppBanner();

                bool haveProcessName = options.ProcessName != null;
                bool haveProcessId = options.ProcessId > 0;

                if (haveProcessName || haveProcessId)
                {
                    _writer.Write(DisplayStrings.ScanTargetIntro);

                    if (haveProcessName)
                    {
                        _writer.Write(DisplayStrings.ScanTargetProcessNameFormat, options.ProcessName);
                    }
                    if (haveProcessName && haveProcessId)
                    {
                        _writer.Write(DisplayStrings.ScanTargetSeparator);
                    }
                    if (haveProcessId)
                    {
                        _writer.Write(DisplayStrings.ScanTargetProcessIdFormat, options.ProcessId);
                    }

                    bool haveScanRootWindowHandle = options.ScanRootWindowHandle != IntPtr.Zero;
                    if (haveScanRootWindowHandle)
                    {
                        _writer.Write(DisplayStrings.ScanTargetSeparator);
                        _writer.Write(DisplayStrings.ScanTargetRootWindowHandleFormat, options.ScanRootWindowHandle);
                    }
                    _writer.WriteLine();
                }
                if (!string.IsNullOrEmpty(options.ScanId))
                {
                    _writer.WriteLine(DisplayStrings.ScanIdFormat, options.ScanId);
                }

                _bannerHasBeenShown = true;
            }
        }

        private void WriteExecutionErrors(Exception caughtException)
        {
            _writer.Write(DisplayStrings.ScanNotCompleteIntro);

            if (caughtException == null)
            {
                _writer.WriteLine(DisplayStrings.ScanNotCompleteNoException);
            }
            else if (caughtException is ParameterException)
            {
                _writer.WriteLine(caughtException.Message);
            }
            else
            {
                _writer.WriteLine(DisplayStrings.ScanNotCompleteExceptionFormat, caughtException);
            }
        }

        private void WriteWindowScanOutput(IOptions options, WindowScanOutput output)
        {
            if (options.VerbosityLevel == VerbosityLevel.Quiet)
            {
                return;
            }

            WriteErrorCount(output);

            if (options.VerbosityLevel >= VerbosityLevel.Verbose)
            {
                WriteVerboseWindowScanOutput(output);
            }

            if (!string.IsNullOrEmpty(output.OutputFile.A11yTest))
            {
                _writer.WriteLine(DisplayStrings.ScanResultsLocationFormat, output.OutputFile.A11yTest);
            }
        }

        private void WriteErrorCount(WindowScanOutput output)
        {
            if (output.ErrorCount == 1)
            {
                _writer.WriteLine(DisplayStrings.ScanResultsSingleError);
            }
            else
            {
                _writer.WriteLine(DisplayStrings.ScanResultsMultipleErrorsFormat, output.ErrorCount);
            }
        }

        private void WriteVerboseWindowScanOutput(WindowScanOutput output)
        {
            int errorCount = 0;
            foreach (ScanResult scanResult in output.Errors)
            {
                _writer.WriteLine(DisplayStrings.ScanResultDetailHeader, ++errorCount, scanResult.Rule.Description);
                WriteFrameworkLink(scanResult);
                WriteProperties(scanResult);
                WritePatterns(scanResult);
                _writer.WriteLine(DisplayStrings.ScanResultDetailFooter);
            }
        }

        private void WriteFrameworkLink(ScanResult scanResult)
        {
            if (scanResult.Rule.FrameworkIssueLink != null)
            {
                _writer.WriteLine(DisplayStrings.FrameworkIssueLink, scanResult.Rule.FrameworkIssueLink);
            }
        }

        private void WriteProperties(ScanResult scanResult)
        {
            if (scanResult.Element.Properties != null && scanResult.Element.Properties.Any())
            {
                _writer.WriteLine(DisplayStrings.ScanResultDetailElementPropertiesHeader);
                foreach (KeyValuePair<string, string> pair in scanResult.Element.Properties)
                {
                    _writer.WriteLine(DisplayStrings.ScanResultDetailElementPropertyFormat, pair.Key, pair.Value);
                }
            }
        }

        private void WritePatterns(ScanResult scanResult)
        {
            if (scanResult.Element.Patterns != null && scanResult.Element.Patterns.Any())
            {
                _writer.WriteLine(DisplayStrings.ScanResultDetailElementPatternsHeader);
                foreach (string pattern in scanResult.Element.Patterns)
                {
                    _writer.WriteLine(DisplayStrings.ScanResultDetailElementPatternFormat, pattern);
                }
            }
        }
    }
}
