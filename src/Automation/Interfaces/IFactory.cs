// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.SystemAbstractions;
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Factory used to create objects for internal use
    /// </summary>
    interface IFactory
    {
        IOutputFileHelper CreateOutputFileHelper(string outputDirectory);
        IScanResultsAssembler CreateResultsAssembler();
        ITargetElementLocator CreateTargetElementLocator();
        IAxeWindowsActions CreateAxeWindowsActions();
        INativeMethods CreateNativeMethods();
    } // interface
} // namespace
