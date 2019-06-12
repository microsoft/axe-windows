// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;

namespace Axe.Windows.Abstractions
{
    internal interface ISystemFactory
    {
        ISystemIODirectory CreateSystemIODirectory();
        ISystemEnvironment CreateSystemEnvironment();
        ISystemDateTime CreateSystemDateTime();
    } // interface
} // namespace
