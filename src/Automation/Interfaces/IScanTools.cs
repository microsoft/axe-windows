// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
{
    internal interface IScanTools
    {
        IOutputFileHelper OutputFileHelper { get; }
        IScanResultsAssembler ResultsAssembler { get; }
        ITargetElementLocator TargetElementLocator { get; }
    } // interface
} // namespace
