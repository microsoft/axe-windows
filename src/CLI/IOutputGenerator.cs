// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Automation;
using System;

namespace AxeWindowsCLI
{
    internal interface IOutputGenerator
    {
        void WriteBanner(IOptions options);
        void WriteOutput(IOptions options, ScanResults scanResults, Exception caughtException);
    }
}
