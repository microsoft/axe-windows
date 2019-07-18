// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;

namespace Axe.Windows.SystemAbstractions
{
    class SystemIO : ISystemIO
    {
        private readonly Lazy<ISystemIODirectory> _directory;

        public SystemIO(ISystemFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _directory = new Lazy<ISystemIODirectory>(factory.CreateSystemIODirectory);
        }

        public ISystemIODirectory Directory => _directory.Value;
    } // class
} // namespace
