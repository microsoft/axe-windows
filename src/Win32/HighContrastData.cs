// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Runtime.InteropServices;

namespace Axe.Windows.Win32
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct HighContrastData
    {
        public UInt32 Size;
        public Int32 Flags;

        // changing the following type to string will cause .NET Core to crash during the unit tests
        public IntPtr DefaultScheme;
    } // struct
} // namespace
