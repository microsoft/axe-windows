﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.SystemAbstractions;
using System;

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

        public IAxeWindowsActions CreateAxeWindowsActions()
        {
            return new AxeWindowsActions();
        }
    } // class
} // namespace
