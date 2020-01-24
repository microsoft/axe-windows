// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Runtime.InteropServices;

namespace Axe.Windows.Win32
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    internal struct HighContrast
    {
        public UInt32 Size { get; private set; }
        public Int32 Flags { get; private set; }
        public string DefaultScheme { get; private set; }
        public bool IsOn => (Flags & HighContrastOnFlag) == HighContrastOnFlag;

        private const uint SPI_GetHighContrast = 0x0042;
        private const UInt32 HighContrastOnFlag = 0x1;

        public static HighContrast Create()
        {
            var hc = new HighContrast { Size = (UInt32)Marshal.SizeOf(typeof(HighContrast)) };

            NativeMethods.SystemParametersInfoHighContrast(SPI_GetHighContrast, hc.Size, ref hc, 0);

            return hc;
        }
    } // struct
} // namespace
