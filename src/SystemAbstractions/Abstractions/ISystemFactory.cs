// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SystemAbstractions
{
    internal interface ISystemFactory
    {
        ISystemDateTime CreateSystemDateTime();
        ISystemEnvironment CreateSystemEnvironment();
        ISystemIO CreateSystemIO();
        ISystemIODirectory CreateSystemIODirectory();
    } // interface
} // namespace
