// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Encapsulates the set of tools used to scan, assemble results, and write output files 
    /// </summary>
    internal interface IScanTools
    {
        IOutputFileHelper OutputFileHelper { get; }
        IScanResultsAssembler ResultsAssembler { get; }
        ITargetElementLocator TargetElementLocator { get; }
    } // interface
} // namespace
