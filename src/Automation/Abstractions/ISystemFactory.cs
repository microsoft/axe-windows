﻿// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;

namespace Axe.Windows.Abstractions
{
    internal interface ISystemFactory
    {
        ISystemDateTime CreateSystemDateTime();
        ISystemEnvironment CreateSystemEnvironment();
        ISystemIO CreateSystemIO();
        ISystemIODirectory CreateSystemIODirectory();
    } // interface
} // namespace
