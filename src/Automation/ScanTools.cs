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
        public INativeMethods NativeMethods { get; }

        public ScanTools(IOutputFileHelper outputFileHelper, IScanResultsAssembler resultsAssembler, ITargetElementLocator targetElementLocator, IAxeWindowsActions actions, INativeMethods nativeMethods)
        {
            OutputFileHelper = outputFileHelper ?? throw new ArgumentNullException(nameof(outputFileHelper));
            ResultsAssembler = resultsAssembler ?? throw new ArgumentNullException(nameof(resultsAssembler));
            TargetElementLocator = targetElementLocator ?? throw new ArgumentNullException(nameof(targetElementLocator));
            Actions = actions ?? throw new ArgumentNullException(nameof(actions));
            NativeMethods = nativeMethods ?? throw new ArgumentNullException(nameof(nativeMethods));
        }
    } // class
} // namespace
