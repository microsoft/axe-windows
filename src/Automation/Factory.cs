// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Desktop.Drawing;
using Axe.Windows.SystemAbstractions;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Factory used to create objects for internal use
    /// </summary>
    internal class Factory : IFactory
    {
        private Factory()
        { }

        static public IScanToolsBuilder CreateScanToolsBuilder()
        {
            return new ScanToolsBuilder(new Factory());
        }

        public IOutputFileHelper CreateOutputFileHelper(string outputDirectory)
        {
            return new OutputFileHelper(outputDirectory, SystemFactory.CreateSystem());
        }

        public IScanResultsAssembler CreateResultsAssembler()
        {
            return new ScanResultsAssembler();
        }

        public ITargetElementLocator CreateTargetElementLocator()
        {
            return new TargetElementLocator();
        }

        public IAxeWindowsActions CreateAxeWindowsActions(string bitmapCreatorAssemblyFullName)
        {
            IBitmapCreator bitmapCreator = bitmapCreatorAssemblyFullName == null ?
                new FrameworkBitmapCreator() :
                BitmapCreatorLocator.GetBitmapCreator(bitmapCreatorAssemblyFullName, out _);
            return new AxeWindowsActions(bitmapCreator);
        }

        public INativeMethods CreateNativeMethods()
        {
            return new NativeMethods();
        }
    } // class
} // namespace
