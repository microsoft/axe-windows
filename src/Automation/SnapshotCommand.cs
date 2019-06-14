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
        /// <summary>
        /// Execute the Start command. Used by both .NET and by PowerShell entry points
        /// </summary>
        /// <param name="config">A set of configuration options</param>
        /// <param name="outputFileHelper"/>
        /// <param name="resultsAssembler"/>
        /// <returns>A SnapshotCommandResult that describes the result of the command</returns>
        public static ScanResults Execute(Config config, IOutputFileHelper outputFileHelper, IScanResultsAssembler resultsAssembler)
        {
            return ExecutionWrapper.ExecuteCommand<ScanResults>(() =>
            {
                if (outputFileHelper == null) throw new ArgumentNullException(nameof(outputFileHelper));
                if (resultsAssembler == null) throw new ArgumentNullException(nameof(resultsAssembler));

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

                                var results = resultsAssembler.AssembleScanResultsFromElement(ec2.Element);

                                string a11yTestOutputFile = config.OutputFileFormat.HasFlag(OutputFileFormat.A11yTest) ? outputFileHelper.GetNewA11yTestFilePath() : string.Empty;

                                if (results.ErrorCount > 0)
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

                                    results.OutputFile = (a11yTestOutputFile, null);
                                }

                                return results;
                            }
                        }
                    }

                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorUnableToSetDataContext);
                } // using
            });
        }
    }
}
