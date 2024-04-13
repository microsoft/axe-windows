// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Win32
{
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

    /// <summary>
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/dn280511(v=vs.85).aspx
    /// </summary>
    public enum DpiType
    {
        Effective = 0,
        Angular = 1,
        Raw = 2,
    }

    /// <summary>
    /// Device Cap
    /// https://docs.microsoft.com/en-us/windows/desktop/api/wingdi/nf-wingdi-getdevicecaps
    /// </summary>
    public enum DeviceCap
    {
        /// <summary>
        /// Logical pixels inch in X
        /// </summary>
        LOGPIXELSX = 88,
        /// <summary>
        /// Logical pixels inch in Y
        /// </summary>
        LOGPIXELSY = 90
    }
}
