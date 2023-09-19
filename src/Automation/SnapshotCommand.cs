﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Actions.Contexts;
using Axe.Windows.Automation.Data;
using Axe.Windows.Automation.Resources;
using Axe.Windows.Core.Bases;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Class to take a snapshot (via SnapshotCommand.Execute*).
    /// </summary>
    static class SnapshotCommand
    {
        /// <summary>
        /// Execute the scan asynchronously
        /// </summary>
        /// <param name="config">A set of configuration options</param>
        /// <param name="scanTools">A set of tools for writing output files,
        /// creating the expected results format, and finding the target element to scan</param>
        /// <param name="cancellationToken">A cancellation token</param>
        /// <returns>A ScanOutput object that describes the result of the command</returns>
        public static async Task<ScanOutput> ExecuteAsync(Config config, IScanTools scanTools, CancellationToken cancellationToken)
        {
            ValidateScanParameters(config, scanTools);

            return await Task.Run<ScanOutput>(() => GetScanOutput(config, scanTools, cancellationToken)).ConfigureAwait(false);
        }

        /// <summary>
        /// Execute the scan synchronously
        /// </summary>
        /// <param name="config">A set of configuration options</param>
        /// <param name="scanTools">A set of tools for writing output files,
        /// creating the expected results format, and finding the target element to scan</param>
        /// <returns>A ScanOutput object that describes the result of the command</returns>
        public static ScanOutput Execute(Config config, IScanTools scanTools)
        {
            ValidateScanParameters(config, scanTools);

            return GetScanOutput(config, scanTools, CancellationToken.None);
        }

        private static void ValidateScanParameters(Config config, IScanTools scanTools)
        {
            if (config == null) throw new ArgumentNullException(nameof(config));
            if (scanTools == null) throw new ArgumentNullException(nameof(scanTools));
            if (scanTools.TargetElementLocator == null) throw new ArgumentException(ErrorMessages.ScanToolsTargetElementLocatorNull, nameof(scanTools));
            if (scanTools.Actions == null) throw new ArgumentException(ErrorMessages.ScanToolsActionsNull, nameof(scanTools));
            if (scanTools.DpiAwareness == null) throw new ArgumentException(ErrorMessages.ScanToolsDpiAwarenessNull, nameof(scanTools));
        }

        private static ScanOutput GetScanOutput(Config config, IScanTools scanTools, CancellationToken cancellationToken)
        {
            if (config.CustomUIAConfigPath != null)
                scanTools.Actions.RegisterCustomUIAPropertiesFromConfig(config.CustomUIAConfigPath);

            List<WindowScanOutput> resultList = new List<WindowScanOutput>();

            using (var actionContext = ScopedActionContext.CreateInstance(cancellationToken))
            {
                var rootElements = scanTools.TargetElementLocator.LocateRootElements(config.ProcessId, actionContext);

                if (rootElements is null || !rootElements.Any())
                {
                    return new ScanOutput(resultList);
                }

                int targetIndex = 1;

                IReadOnlyList<A11yElement> rootElementList = rootElements.ToList();
                bool multipleTopLevelWindowsExist = rootElementList.Count > 1;

                foreach (var rootElement in rootElementList)
                {
                    ScanAndProcessResults(config, scanTools, resultList, rootElements, multipleTopLevelWindowsExist, targetIndex, rootElement, actionContext);

                    targetIndex++;
                }
            }

            return new ScanOutput(resultList);
        }

        /// <summary>
        /// This method is our atomic scanner. It needs to run within the scope of a single thread.
        /// </summary>
        private static void ScanAndProcessResults(Config config, IScanTools scanTools, List<WindowScanOutput> resultList, IEnumerable<A11yElement> rootElements, bool multipleTopLevelWindowsExist, int targetIndex, A11yElement rootElement, IActionContext actionContext)
        {
            var dpiAwarenessObject = scanTools.DpiAwareness.Enable();
            try
            {
                scanTools.Actions.SetShouldTestAllChromiumContent(config.TestAllChromiumContent);
                resultList.Add(scanTools.Actions.Scan(rootElement, (element, elementId) =>
                {
                    return ProcessResults(element, elementId, config, scanTools, multipleTopLevelWindowsExist, targetIndex, rootElements.Count(), actionContext);
                }, actionContext));
            }
            finally
            {
                scanTools.DpiAwareness.Restore(dpiAwarenessObject);
            }
        }

        private static WindowScanOutput ProcessResults(A11yElement element, Guid elementId, Config config, IScanTools scanTools, bool multipleTopLevelWindowsExist, int targetIndex, int targetCount, IActionContext actionContext)
        {
            // We must turn on DPI awareness so we get physical, not logical, UIA element bounding rectangles
            object dpiAwarenessObject = scanTools.DpiAwareness.Enable();

            try
            {
                if (scanTools == null) throw new ArgumentNullException(nameof(scanTools));
                if (scanTools.ResultsAssembler == null) throw new ArgumentException(ErrorMessages.ScanToolsResultsAssemblerNull, nameof(scanTools));

                var results = scanTools.ResultsAssembler.AssembleWindowScanOutputFromElement(element);

                if (multipleTopLevelWindowsExist)
                {
                    results.OutputFile = WriteOutputFiles(config.OutputFileFormat, scanTools, element, elementId, (name) => $"{name}_{targetIndex}_of_{targetCount}", actionContext);
                }
                else
                {
                    if (results.ErrorCount > 0 || config.AlwaysSaveTestFile)
                    {
                        results.OutputFile = WriteOutputFiles(config.OutputFileFormat, scanTools, element, elementId, null, actionContext);
                    }
                }

                return results;
            }
            finally
            {
                scanTools.DpiAwareness.Restore(dpiAwarenessObject);
            }
        }

        private static OutputFile WriteOutputFiles(OutputFileFormat outputFileFormat, IScanTools scanTools, A11yElement element, Guid elementId, Func<string, string> decorator, IActionContext actionContext)
        {
            if (scanTools == null) throw new ArgumentNullException(nameof(scanTools));
            if (scanTools.OutputFileHelper == null) throw new ArgumentException(ErrorMessages.ScanToolsOutputFileHelperNull, nameof(scanTools));

            string a11yTestOutputFile = null;

            if (outputFileFormat.HasFlag(OutputFileFormat.A11yTest))
            {
                scanTools.Actions.CaptureScreenshot(elementId, actionContext);

                scanTools.OutputFileHelper.EnsureOutputDirectoryExists();

                a11yTestOutputFile = scanTools.OutputFileHelper.GetNewA11yTestFilePath(decorator);
                if (a11yTestOutputFile == null) throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ErrorMessages.VariableNull, nameof(a11yTestOutputFile)));

                scanTools.Actions.SaveA11yTestFile(a11yTestOutputFile, element, elementId, actionContext);
            }

            return OutputFile.BuildFromA11yTestFile(a11yTestOutputFile);
        }
    } // class
} // namespace
