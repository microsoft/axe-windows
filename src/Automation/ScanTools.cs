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
        public IDPIAwareness DpiAwareness { get; }
        public IntPtr ScanRootWindowHandle { get; set; } = IntPtr.Zero;

        public ScanTools(IOutputFileHelper outputFileHelper, IScanResultsAssembler resultsAssembler, ITargetElementLocator targetElementLocator, IAxeWindowsActions actions, IDPIAwareness dpiAwareness)
        {
            OutputFileHelper = outputFileHelper ?? throw new ArgumentNullException(nameof(outputFileHelper));
            ResultsAssembler = resultsAssembler ?? throw new ArgumentNullException(nameof(resultsAssembler));
            TargetElementLocator = targetElementLocator ?? throw new ArgumentNullException(nameof(targetElementLocator));
            Actions = actions ?? throw new ArgumentNullException(nameof(actions));
            DpiAwareness = dpiAwareness ?? throw new ArgumentNullException(nameof(dpiAwareness));
        }
    } // class
} // namespace
