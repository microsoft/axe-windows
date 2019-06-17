// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Actions.Misc;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Desktop.Settings;
using System;
using System.Collections.Generic;
using System.Globalization;

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
                if (scanTools == null) throw new ArgumentNullException(nameof(scanTools));
                if (scanTools.OutputFileHelper == null) throw new ArgumentNullException(nameof(scanTools.OutputFileHelper));
                if (scanTools.ResultsAssembler == null) throw new ArgumentNullException(nameof(scanTools.ResultsAssembler));
                if (scanTools.TargetElementLocator == null) throw new ArgumentNullException(nameof(scanTools.TargetElementLocator));

                using (var dataManager = DataManager.GetDefaultInstance())
                using (var sa = SelectAction.GetDefaultInstance())
                {
                    var rootElement = scanTools.TargetElementLocator.LocateRootElement(config.ProcessId);
                    if (rootElement == null) throw new ArgumentNullException(nameof(rootElement));

                    sa.SetCandidateElement(rootElement);

                    if (sa.Select())
                    {
                        using (ElementContext ec2 = sa.POIElementContext)
                        {
                            GetDataAction.GetProcessAndUIFrameworkOfElementContext(ec2.Id);
                            if (CaptureAction.SetTestModeDataContext(ec2.Id, DataContextMode.Test, TreeViewMode.Control))
                            {
                                // send telemetry of scan results. 
                                var dc = GetDataAction.GetElementDataContext(ec2.Id);
                                dc.PublishScanResults();

                                if (dc.ElementCounter.UpperBoundExceeded)
                                {
                                    throw new AxeWindowsAutomationException(string.Format(CultureInfo.InvariantCulture,
                                        DisplayStrings.ErrorTooManyElementsToSetDataContext,
                                        dc.ElementCounter.UpperBound));
                                }

                                var results = scanTools.ResultsAssembler.AssembleScanResultsFromElement(ec2.Element);

                                if (results.ErrorCount > 0)
                                    results.OutputFile = WriteOutputFiles(config.OutputFileFormat, scanTools.OutputFileHelper, ec2);

                                return results;
                            }
                        }
                    }

                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorUnableToSetDataContext);
                } // using
            });
        }

        private static (string A11yTest, string Sarif) WriteOutputFiles(OutputFileFormat outputFileFormat, IOutputFileHelper outputFileHelper, ElementContext elementContext)
        {
            string a11yTestOutputFile = null;

            if (outputFileFormat.HasFlag(OutputFileFormat.A11yTest))
            {
                ScreenShotAction.CaptureScreenShot(elementContext.Id);

                a11yTestOutputFile = outputFileHelper.GetNewA11yTestFilePath();
                SaveAction.SaveSnapshotZip(a11yTestOutputFile, elementContext.Id, elementContext.Element.UniqueId, A11yFileMode.Test);
            }

#if NOT_CURRENTLY_SUPPORTED
                                if (locationHelper.IsSarifExtension())
                                    // SaveAction.SaveSarifFile(outputFileHelper.GetNewSarifFilePath(), ec2.Id, !locationHelper.IsAllOption());
#endif

            return (a11yTestOutputFile, null);
        }
    } // class
} // namespace
