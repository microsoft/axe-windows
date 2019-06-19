// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Automation
{
    class ScanTools : IScanTools
    {
        public IOutputFileHelper OutputFileHelper { get; }
        public IScanResultsAssembler ResultsAssembler { get; }
        public ITargetElementLocator TargetElementLocator { get; }
        public IAxeWindowsActions Actions { get; }

        public ScanTools(IOutputFileHelper outputFileHelper, IScanResultsAssembler resultsAssembler, ITargetElementLocator targetElementLocator, IAxeWindowsActions actions)
        {
            if (outputFileHelper == null) throw new ArgumentNullException(nameof(outputFileHelper));
            if (resultsAssembler == null) throw new ArgumentNullException(nameof(resultsAssembler));
            if (targetElementLocator == null) throw new ArgumentNullException(nameof(targetElementLocator));
            if (actions == null) throw new ArgumentNullException(nameof(actions));

            OutputFileHelper = outputFileHelper;
            ResultsAssembler = resultsAssembler;
            TargetElementLocator = targetElementLocator;
            Actions = actions;
        }
    } // class
} // namespace
