﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;

namespace AxeWindowsCLI
{
    internal interface IProcessAbstraction
    {
        Process[] GetProcessesByName(string name);
        Process GetProcessById(int id);
    }
}
