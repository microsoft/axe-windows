// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace SystemAbstractions
{
    public interface ISystem
    {
        ISystemDateTime DateTime { get; }
        ISystemEnvironment Environment { get; }
        ISystemIO IO { get; }
    } // interface
} // namespace
