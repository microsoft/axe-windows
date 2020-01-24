// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Drawing;
using System.Runtime.InteropServices;

namespace Axe.Windows.Win32
{
    /// <summary>
    /// Class NativeMethods for Win32 Apis pInvoke and Win32 specific code.
    /// Some of these definitions originated from https://pinvoke.net/
    /// </summary>
    internal static partial class NativeMethods
    {
        /// <summary>
        /// Clean up Variant
        /// </summary>
        /// <param name="pvarg"></param>
        /// <returns></returns>
        [DllImport("OleAut32.dll")]
        internal static extern uint VariantClear(ref dynamic pvarg);

        [DllImport("user32.dll")]
        internal static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        /// <summary>
        /// Turns on DPI awareness for the current process
        /// </summary>
        /// <returns>
        /// true if the operation was successful, otherwise false
        /// </returns>
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool SetProcessDPIAware();

        [DllImport("user32.dll")]
        internal static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo", SetLastError = true)]
        internal static extern bool SystemParametersInfoHighContrast(uint action, uint param, ref HighContrast vparam, uint init);
    }
}
