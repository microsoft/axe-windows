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
using System.Diagnostics;
using System.Globalization;

namespace Axe.Windows.Automation
{
    class AxeWindowsActions : IAxeWindowsActions
    {
        public ResultsT Scan<ResultsT>(A11yElement element, ScanActionCallback<ResultsT> scanCallback)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (scanCallback == null) throw new ArgumentNullException(nameof(scanCallback));

            using (var dataManager = DataManager.GetDefaultInstance())
            using (var sa = SelectAction.GetDefaultInstance())
            {
                sa.SetCandidateElement(element);

                if (!sa.Select())
                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorUnableToSetDataContext);

                using (ElementContext ec2 = sa.POIElementContext)
                {
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    GetDataAction.GetProcessAndUIFrameworkOfElementContext(ec2.Id);
                    if (!CaptureAction.SetTestModeDataContext(ec2.Id, DataContextMode.Test, TreeViewMode.Control))
                        throw new AxeWindowsAutomationException(DisplayStrings.ErrorUnableToSetDataContext);
                    long scanDurationInMilliseconds = stopwatch.ElapsedMilliseconds;

                    // send telemetry of scan results.
                    var dc = GetDataAction.GetElementDataContext(ec2.Id);
                    dc.PublishScanResults(scanDurationInMilliseconds);

                    if (dc.ElementCounter.UpperBoundExceeded)
                    {
                        throw new AxeWindowsAutomationException(string.Format(CultureInfo.InvariantCulture,
                            DisplayStrings.ErrorTooManyElementsToSetDataContext,
                            dc.ElementCounter.UpperBound));
                    }

                    return scanCallback(ec2.Element, ec2.Id);
                } // using
            } // using
        }

        public void CaptureScreenshot(Guid elementId)
        {
            ScreenShotAction.CaptureScreenShot(elementId);
        }

        public void SaveA11yTestFile(string path, A11yElement element, Guid elementId)
        {
            SaveAction.SaveSnapshotZip(path, elementId, element.UniqueId, A11yFileMode.Test);
        }

        public void RegisterCustomUIAPropertiesFromConfig(string path)
        {
            Core.CustomObjects.Config conf = CustomUIAAction.ReadConfigFromFile(path);
            CustomUIAAction.RegisterCustomProperties(conf.Properties);
        }
    } // class
} // namespace
