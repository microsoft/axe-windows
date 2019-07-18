// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Microsoft.Win32;
using System;

namespace Axe.Windows.SystemAbstractions
{
    class MicrosoftWin32Registry : IMicrosoftWin32Registry
    {
        public object GetValue(string keyName, string valueName, object defaultValue)
        {
            return Registry.GetValue(keyName, valueName, defaultValue);
        }
    } // class
} // namespace
