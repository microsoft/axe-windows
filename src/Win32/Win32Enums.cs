// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Win32
{
    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646309(v=vs.85).aspx
    /// </summary>
    internal enum HotkeyModifier : int
    {
        MOD_ALT = 0x0001,
        MOD_CONTROL = 0x0002,
        MOD_NOREPEAT = 0x4000,
        MOD_SHIFT = 0x0004,
        MOD_WIN = 0x0008,
        MOD_NoModifier = -1,
    }

    /// <summary>
    /// Flag used when in OS comparison methods
    /// </summary>
    internal enum OsComparisonResult
    {
        /// <summary>
        /// OS is older than comparison point
        /// </summary>
        Older,

        /// <summary>
        /// OS is equal to comparison point
        /// </summary>
        Equal,

        /// <summary>
        /// OS is newer than comparison point
        /// </summary>
        Newer
    }
}
