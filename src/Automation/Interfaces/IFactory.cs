// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
        IAxeWindowsActions CreateAxeWindowsActions(string bitmapCreatorAssemblyFullName);
        INativeMethods CreateNativeMethods();
    } // interface
} // namespace
