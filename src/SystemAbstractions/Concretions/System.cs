// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Axe.Windows.SystemAbstractions
{
    class System : ISystem
    {
        private readonly Lazy<ISystemDateTime> _dateTime;
        private readonly Lazy<ISystemEnvironment> _environment;
        private readonly Lazy<ISystemIO> _io;

        public System(ISystemFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _dateTime = new Lazy<ISystemDateTime>(factory.CreateSystemDateTime);
            _environment = new Lazy<ISystemEnvironment>(factory.CreateSystemEnvironment);
            _io = new Lazy<ISystemIO>(factory.CreateSystemIO);
        }

        public ISystemDateTime DateTime => _dateTime.Value;

        public ISystemEnvironment Environment => _environment.Value;

        public ISystemIO IO => _io.Value;
    } // class
} // namespace
