﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.SystemAbstractions
{
    public interface IMicrosoftWin32Registry
    {
        object GetValue(string keyName, string valueName, object defaultValue);
    } // Interface
} // namespace
