﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace AxeWindowsCLI
{
    internal interface IProcessHelper
    {
        int ProcessIdFromName(string processName);
        string ProcessNameFromId(int processId);
    }
}
