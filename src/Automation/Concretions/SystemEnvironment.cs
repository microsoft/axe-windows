// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;
using System;

namespace Axe.Windows.Concretions
{
    class SystemEnvironment : ISystemEnvironment
    {
        public string CurrentDirectory => Environment.CurrentDirectory;
    } // class
} // namespace
