// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Abstractions;

namespace Axe.Windows.Concretions
{
    class SystemFactory : ISystemFactory
    {
        public ISystemIODirectory CreateSystemIODirectory()
        {
            return new SystemIODirectory();
        }

        public ISystemEnvironment CreateSystemEnvironment()
        {
            return new SystemEnvironment();
        }

        public ISystemDateTime CreateSystemDateTime()
        {
            return new SystemDateTime();
        }
    } // class
} // namespace
