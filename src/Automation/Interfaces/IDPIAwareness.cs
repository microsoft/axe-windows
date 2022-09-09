// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Automation
{
    /// <summary>
    /// UIA operates in physical screen coordinates, so DPI awareness must be enabled while scanning.
    /// Methods on this interface will be called before the first scan begins and after the last scan completes.
    /// </summary>
    public interface IDPIAwareness
    {
        /// <summary>
        /// Enable DPI awareness for the scan
        /// </summary>
        /// <returns>An object that will be passed as a parameter <see cref="IDPIAwareness.Restore"/></returns>
        object Enable();

        /// <summary>
        /// Restore DPI awareness to its original state
        /// </summary>
        /// <param name="dataFromEnable">The object returned from the call to <see cref="IDPIAwareness.Enable"/></param>
        void Restore(object dataFromEnable);
    }
}
