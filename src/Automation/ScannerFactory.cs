// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Create objects that implement IScanner
    /// </summary>
    public static class ScannerFactory
    {
        /// <summary>
        /// Create an object that implements IScanner
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IScanner CreateScanner(Config config)
        {
            // Factory.CreateOutputFileHelper(config.OutputDirectory)
            var scanTools = Factory.CreateScanTools();
            return new Scanner(config, scanTools);
        }
    } // class
} // namespace
