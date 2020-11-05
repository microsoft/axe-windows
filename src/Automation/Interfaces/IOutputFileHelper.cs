// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.Automation
{
    internal interface IOutputFileHelper
    {
        void EnsureOutputDirectoryExists();
        string GetNewA11yTestFilePath();
        void SetScanId(string scanId);
    } // interface
} // namespace
