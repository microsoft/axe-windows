// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System;

namespace Axe.Windows.CI
{
    /// <summary>
    /// This app exists as a way to gather the assemblies that will be packaged
    /// into the NuGet package. To learn how to use the NuGet Package, please
    /// refer either to the CLI project or AutomationReference.md in the docs
    /// folder.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IScanner = {0}", typeof(IScanner).FullName);
        }
    }
}
