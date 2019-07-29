// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Axe.Windows.SystemAbstractions
{
    class Microsoft : IMicrosoft
    {
        private readonly Lazy<IMicrosoftWin32> _win32;

        public Microsoft(IMicrosoftFactory factory)
        {
            if (factory == null) throw new ArgumentNullException(nameof(factory));

            _win32 = new Lazy<IMicrosoftWin32>(factory.CreateMicrosoftWin32);
        }

        public IMicrosoftWin32 Win32 => _win32.Value;
    } // class
} // namespace
