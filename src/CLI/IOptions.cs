// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AxeWindowsCLI
{
    internal interface IOptions
    {
        string OutputDirectory { get; }
        string ScanId { get; }
        int ProcessId { get; }
        string ProcessName { get; }
        VerbosityLevel VerbosityLevel { get; }
        int DelayInSeconds { get; }
        string CustomUia { get; }
        bool AreMultipleScanRootsEnabled { get; }
    }
}
