// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions;
using Axe.Windows.Core.Bases;
using Axe.Windows.Desktop.Settings;
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Class to take a snapshot (via SnapshotCommand.Execute). 
    /// </summary>
    static class SnapshotCommand
    {
        /// <summary>
        /// Execute the Start command. Used by both .NET and by PowerShell entry points
        /// </summary>
        /// <param name="config">A set of configuration options</param>
        /// <param name="scanTools">A set of tools for writing output files,
        /// creating the expected results format, and finding the target element to scan</param>
        /// <returns>A SnapshotCommandResult that describes the result of the command</returns>
        public static ScanResults Execute(Config config, IScanTools scanTools)
        {
            return ExecutionWrapper.ExecuteCommand<ScanResults>(() =>
            {
                if (config == null) throw new ArgumentNullException(nameof(config));
                if (scanTools == null) throw new ArgumentNullException(nameof(scanTools));
                if (scanTools.TargetElementLocator == null) throw new ArgumentNullException(nameof(scanTools.TargetElementLocator));
                if (scanTools.InternalScanner == null) throw new ArgumentNullException(nameof(scanTools.InternalScanner));

                var rootElement = scanTools.TargetElementLocator.LocateRootElement(config.ProcessId);
                if (rootElement == null) throw new InvalidOperationException(nameof(rootElement));

                return scanTools.InternalScanner.Scan(rootElement,
                    (element, elementId) =>
                {
                    return ProcessResults(element, elementId, config, scanTools);
                });
            });
        }

        private static ScanResults ProcessResults(A11yElement element, Guid elementId, Config config, IScanTools scanTools)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (scanTools == null) throw new ArgumentNullException(nameof(scanTools));
            if (scanTools.ResultsAssembler == null) throw new ArgumentNullException(nameof(scanTools.ResultsAssembler));

            var results = scanTools.ResultsAssembler.AssembleScanResultsFromElement(element);

            if (results.ErrorCount > 0)
                results.OutputFile = WriteOutputFiles(config.OutputFileFormat, scanTools.OutputFileHelper, element, elementId);

            return results;
        }

        private static (string A11yTest, string Sarif) WriteOutputFiles(OutputFileFormat outputFileFormat, IOutputFileHelper outputFileHelper, A11yElement element, Guid elementId)
        {
            if (outputFileHelper == null) throw new ArgumentNullException(nameof(OutputFileHelper));
            if (element == null) throw new ArgumentNullException(nameof(element));

            string a11yTestOutputFile = null;

            if (outputFileFormat.HasFlag(OutputFileFormat.A11yTest))
            {
                ScreenShotAction.CaptureScreenShot(elementId);

                a11yTestOutputFile = outputFileHelper.GetNewA11yTestFilePath();
                SaveAction.SaveSnapshotZip(a11yTestOutputFile, elementId, element.UniqueId, A11yFileMode.Test);
            }

#if NOT_CURRENTLY_SUPPORTED
                                if (locationHelper.IsSarifExtension())
                                    // SaveAction.SaveSarifFile(outputFileHelper.GetNewSarifFilePath(), ec2.Id, !locationHelper.IsAllOption());
#endif

            return (a11yTestOutputFile, null);
        }
    } // class
} // namespace
