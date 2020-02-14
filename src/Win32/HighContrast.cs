// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Runtime.InteropServices;

namespace Axe.Windows.Win32
{
    internal class HighContrast
    {
        private const uint SPI_GetHighContrast = 0x0042;
        private const UInt32 HighContrastOnFlag = 0x1;

        private HighContrastData _data;

        public bool IsOn => (_data.Flags & HighContrastOnFlag) == HighContrastOnFlag;

        private HighContrast(HighContrastData data)
        {
            _data = data;
        }

        public static HighContrast Create()
        {
            var data = new HighContrastData();
            data.Size = (UInt32)Marshal.SizeOf(data);

            NativeMethods.SystemParametersInfoHighContrast(SPI_GetHighContrast, data.Size, ref data, 0);

            return new HighContrast(data);
        }
    } // struct
} // namespace
