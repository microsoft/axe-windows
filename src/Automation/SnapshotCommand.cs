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
    /// Class to take a snapshot (via ShapshotCommand.Execute). Can only be successfully called after
    /// a successful call to StartCommand.Execute
    /// </summary>
    static class SnapshotCommand
    {
        private class ScanResultAccumulator
        {
            public int Passed { get; private set; }
            public int Failed { get; private set; }
            public int Inconclusive { get; private set;}
            public int Unsupported { get; private set; }
            public int Total { get => Passed + Failed + Inconclusive + Unsupported; }

            public bool MayHaveErrors => (Failed > 0) || (Inconclusive > 0);

            public void AddPass() => Passed++;
            public void AddFail() => Failed++;
            public void AddInconclusive() => Inconclusive++;
            public void AddUnsupported() => Unsupported++;
        }

        /// <summary>
        /// Execute the Start command. Used by both .NET and by PowerShell entry points
        /// </summary>
        /// <param name="config">A set of configuration options</param>
        /// <param name="outputFileHelper"/>
        /// <returns>A SnapshotCommandResult that describes the result of the command</returns>
        public static SnapshotCommandResult Execute(Config config, IOutputFileHelper outputFileHelper)
        {
            return ExecutionWrapper.ExecuteCommand<SnapshotCommandResult>(() =>
            {
                if (outputFileHelper == null) throw new ArgumentNullException(nameof(outputFileHelper));

                using (var dataManager = DataManager.GetDefaultInstance())
                using (var sa = SelectAction.GetDefaultInstance())
                {
                    ElementContext ec = TargetElementLocator.LocateRootElement(config.ProcessId);
                    sa.SetCandidateElement(ec.Element);

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

                                ScanResultAccumulator accumulator = new ScanResultAccumulator();
                                AccumulateScanResults(accumulator, ec2.Element);

                                string a11yTestOutputFile = config.OutputFileFormat.HasFlag(OutputFileFormat.A11yTest) ? outputFileHelper.GetNewA11yTestFilePath() : string.Empty;

                                if (accumulator.MayHaveErrors)
                                {
                                    if (config.OutputFileFormat.HasFlag(OutputFileFormat.A11yTest))
                                    {
                                        ScreenShotAction.CaptureScreenShot(ec2.Id);
                                        SaveAction.SaveSnapshotZip(a11yTestOutputFile, ec2.Id, ec2.Element.UniqueId, A11yFileMode.Test);
                                    }

#if NOT_CURRENTLY_SUPPORTED
                                if (locationHelper.IsSarifExtension())
                                    // SaveAction.SaveSarifFile(outputFileHelper.GetNewSarifFilePath(), ec2.Id, !locationHelper.IsAllOption());
#endif
                                }

                                string summaryMessage;

                                if (accumulator.MayHaveErrors)
                                {
                                    summaryMessage = string.Format(CultureInfo.InvariantCulture, DisplayStrings.SnapshotDetailViolationsFormat, accumulator.Failed, accumulator.Inconclusive, a11yTestOutputFile);
                                }
                                else
                                {
                                    summaryMessage = DisplayStrings.SnapshotDetailNoViolationsDataDiscarded;
                                }

                                return new SnapshotCommandResult
                                {
                                    Completed = true,
                                    SummaryMessage = summaryMessage,
                                    ScanResultsPassedCount = accumulator.Passed,
                                    ScanResultsFailedCount = accumulator.Failed,
                                    ScanResultsInconclusiveCount = accumulator.Inconclusive,
                                    ScanResultsUnsupportedCount = accumulator.Unsupported,
                                    ScanResultsTotalCount = accumulator.Total,
                                };
                            }
                        }
                    }

                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorUnableToSetDataContext);
                } // using
            });
        }

        /// <summary>
        /// How many violations were found (starting at the target element)
        /// </summary>
        /// <param name="accumulator">Where the results will be accumulated</param>
        /// <param name="element">The root element to check</param>
        private static void AccumulateScanResults(ScanResultAccumulator accumulator, A11yElement element)
        {
            if (element.ScanResults == null || element.ScanResults.Items == null)
                throw new AxeWindowsAutomationException(DisplayStrings.ErrorMissingScanResults);

            foreach (var scanResult in element.ScanResults.Items)
            {
                // This intentionally ignores NoResult values
                switch (scanResult.Status)
                {
                    case Core.Results.ScanStatus.Fail:
                        accumulator.AddFail();
                        break;

                    case Core.Results.ScanStatus.Pass:
                        accumulator.AddPass();
                        break;

                    case Core.Results.ScanStatus.Uncertain:
                        accumulator.AddInconclusive();
                        break;

                    case Core.Results.ScanStatus.ScanNotSupported:
                        accumulator.AddUnsupported();
                        break;
                }
            }

            if (element.Children != null)
            {
                foreach (var child in element.Children)
                {
                    AccumulateScanResults(accumulator, child);
                }
            }
        }
    }
}
