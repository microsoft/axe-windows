﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AxeWindowsScanner
{
    public interface IOptions
    {
        string OutputDirectory { get; }
        string ScanId { get; }
        int ProcessId { get; }
        string ProcessName { get; }
        VerbosityLevel VerbosityLevel { get; }
    }
}
