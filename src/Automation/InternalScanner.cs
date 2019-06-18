// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Actions;
using Axe.Windows.Actions.Contexts;
using Axe.Windows.Actions.Enums;
using Axe.Windows.Actions.Misc;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using System;
using System.Globalization;

namespace Axe.Windows.Automation
{
    class InternalScanner : IInternalScanner
    {
        public ResultsT Scan<ResultsT>(A11yElement element, InternalScannerCallback<ResultsT> resultsCallback)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));
            if (resultsCallback == null) throw new ArgumentNullException(nameof(resultsCallback));

            using (var dataManager = DataManager.GetDefaultInstance())
            using (var sa = SelectAction.GetDefaultInstance())
            {
                sa.SetCandidateElement(element);

                if (!sa.Select())
                    throw new AxeWindowsAutomationException(DisplayStrings.ErrorUnableToSetDataContext);

                using (ElementContext ec2 = sa.POIElementContext)
                {
                    GetDataAction.GetProcessAndUIFrameworkOfElementContext(ec2.Id);
                    if (!CaptureAction.SetTestModeDataContext(ec2.Id, DataContextMode.Test, TreeViewMode.Control))
                        throw new AxeWindowsAutomationException(DisplayStrings.ErrorUnableToSetDataContext);

                        // send telemetry of scan results. 
                        var dc = GetDataAction.GetElementDataContext(ec2.Id);
                        dc.PublishScanResults();

                        if (dc.ElementCounter.UpperBoundExceeded)
                        {
                            throw new AxeWindowsAutomationException(string.Format(CultureInfo.InvariantCulture,
                                DisplayStrings.ErrorTooManyElementsToSetDataContext,
                                dc.ElementCounter.UpperBound));
                        }

                    return resultsCallback(ec2.Element, ec2.Id);
                } // using
            } // using
        }
    } // class
} // namespace
