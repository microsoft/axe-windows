// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Automation.Data;
using Axe.Windows.Core.Bases;
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Class to take a snapshot (via SnapshotCommand.Execute). 
    /// </summary>
    static class SnapshotCommand
    {
        /// <summary>
        /// Execute the scan
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
                if (scanTools.Actions == null) throw new ArgumentNullException(nameof(scanTools.Actions));

                var rootElement = scanTools.TargetElementLocator.LocateRootElement(config.ProcessId);
                if (rootElement == null) throw new InvalidOperationException(nameof(rootElement));

                return scanTools.Actions.Scan(rootElement,
                    (element, elementId) =>
                {
                    return ProcessResults(element, elementId, config, scanTools);
                });
            });
        }

        private static ScanResults ProcessResults(A11yElement element, Guid elementId, Config config, IScanTools scanTools)
        {
            if (scanTools?.ResultsAssembler == null) throw new ArgumentNullException(nameof(scanTools.ResultsAssembler));

            var results = scanTools.ResultsAssembler.AssembleScanResultsFromElement(element);

            if (results.ErrorCount > 0)
                results.OutputFile = WriteOutputFiles(config.OutputFileFormat, scanTools, element, elementId);

            return results;
        }

        private static OutputFile WriteOutputFiles(OutputFileFormat outputFileFormat, IScanTools scanTools, A11yElement element, Guid elementId)
        {
            if (scanTools?.OutputFileHelper == null) throw new ArgumentNullException(nameof(scanTools.OutputFileHelper));

            string a11yTestOutputFile = null;

            if (outputFileFormat.HasFlag(OutputFileFormat.A11yTest))
            {
                scanTools.Actions.CaptureScreenshot(elementId);

                a11yTestOutputFile = scanTools.OutputFileHelper.GetNewA11yTestFilePath();
                if (a11yTestOutputFile == null) throw new InvalidOperationException(nameof(a11yTestOutputFile));
                
scanTools.Actions.SaveA11yTestFile(a11yTestOutputFile, element, elementId);
            }

#if NOT_CURRENTLY_SUPPORTED
                                if (locationHelper.IsSarifExtension())
                                    // SaveAction.SaveSarifFile(outputFileHelper.GetNewSarifFilePath(), ec2.Id, !locationHelper.IsAllOption());
#endif

            return OutputFile.BuildFromA11yTestFile(a11yTestOutputFile);
        }
    } // class
} // namespace
