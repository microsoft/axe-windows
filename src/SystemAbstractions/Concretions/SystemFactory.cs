// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace SystemAbstractions
{
    public class SystemFactory : ISystemFactory
    {
        private readonly static ISystemFactory SystemFactoryInstance = new SystemFactory();

        private SystemFactory()
        { }

        public static ISystem CreateSystem()
        {
            return new System(SystemFactoryInstance);
        }

        public ISystemDateTime CreateSystemDateTime()
        {
            return new SystemDateTime();
        }

        public ISystemEnvironment CreateSystemEnvironment()
        {
            return new SystemEnvironment();
        }

        public ISystemIO CreateSystemIO()
        {
            return new SystemIO(this);
        }

        public ISystemIODirectory CreateSystemIODirectory()
        {
            return new SystemIODirectory();
        }
    } // class
} // namespace
