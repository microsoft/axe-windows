// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
{
    internal interface IScanToolsBuilder
    {
        IScanToolsBuilder WithOutputDirectory(string outputDirectory);
        IScanToolsBuilder WithDPIAwareness(IDPIAwareness dpiAwareness);
        IScanTools Build();
    } // interface
} // namespace
