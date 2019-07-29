// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Axe.Windows.SystemAbstractions
{
    class MicrosoftWin32 : IMicrosoftWin32
    {
        private readonly Lazy<IMicrosoftWin32Registry> _win32Registry;

        public MicrosoftWin32(IMicrosoftFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _win32Registry = new Lazy<IMicrosoftWin32Registry>(factory.CreateMicrosoftWin32Registry);
        }

        public IMicrosoftWin32Registry Registry => _win32Registry.Value;
    } // class
} // namespace
