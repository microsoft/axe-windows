// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;
using Axe.Windows.Concretions;
using System;

namespace Axe.Windows.Automation
{
    static class Factory
    {
        public static IOutputFileHelper CreateOutputFileHelper(string outputDirectory)
        {
            return new OutputFileHelper(outputDirectory, CreateSystemFactory());
        }

        private static ISystemFactory CreateSystemFactory()
        {
            return new SystemFactory();
        }
    } // class
} // namespace
