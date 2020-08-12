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
}
