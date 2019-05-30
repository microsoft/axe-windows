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
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true, ExactSpelling = true)]
        internal static extern uint GetDeviceCaps(IntPtr hDC, int nIndex);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dd145062(v=vs.85).aspx
        [DllImport("User32.dll")]
        internal static extern IntPtr MonitorFromPoint([In]Point pt, [In]uint dwFlags);

        //https://msdn.microsoft.com/en-us/library/windows/desktop/dn280510(v=vs.85).aspx
        [DllImport("Shcore.dll")]
        internal static extern IntPtr GetDpiForMonitor([In]IntPtr hmonitor, [In]DpiType dpiType, [Out]out uint dpiX, [Out]out uint dpiY);

        /// <summary>
        /// Clean up Variant
        /// </summary>
        /// <param name="pvarg"></param>
        /// <returns></returns>
        [DllImport("OleAut32.dll")]
        internal static extern uint VariantClear(ref dynamic pvarg);

        [DllImport("user32.dll")]
        internal static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}
