// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;
using Axe.Windows.Concretions;
using System;

namespace Axe.Windows.Automation
{
    static class Factory
    {
        static public IScanTools CreateScanTools()
        {
            return new ScanTools(
                CreateOutputFileHelper(null),
                CreateResultsAssembler(),
                CreateTargetElementLocator());
        }

        public static IOutputFileHelper CreateOutputFileHelper(string outputDirectory)
        {
            return new OutputFileHelper(outputDirectory, CreateSystemFactory());
        }

        private static ISystemFactory CreateSystemFactory()
        {
            return new SystemFactory();
        }

        private static IScanResultsAssembler CreateResultsAssembler()
        {
            return new ScanResultsAssembler();
        }

        private static ITargetElementLocator CreateTargetElementLocator()
        {
            return new TargetElementLocator();
        }
    } // class
} // namespace
