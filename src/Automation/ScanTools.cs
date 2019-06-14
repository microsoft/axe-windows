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
        public IInternalScanner InternalScanner { get; }

        public ScanTools(IOutputFileHelper outputFileHelper, IScanResultsAssembler resultsAssembler, ITargetElementLocator targetElementLocator, IInternalScanner internalScanner)
        {
            if (outputFileHelper == null) throw new ArgumentNullException(nameof(outputFileHelper));
            if (resultsAssembler == null) throw new ArgumentNullException(nameof(resultsAssembler));
            if (targetElementLocator == null) throw new ArgumentNullException(nameof(targetElementLocator));
            if (internalScanner == null) throw new ArgumentNullException(nameof(internalScanner));

            OutputFileHelper = outputFileHelper;
            ResultsAssembler = resultsAssembler;
            TargetElementLocator = targetElementLocator;
            InternalScanner = internalScanner;
        }
    } // class
} // namespace
