// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System.Globalization;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// Result from a Stop-AxeWindows command
    /// </summary>
    public class StopCommandResult : SharedCommandResult
    {
        /// <summary>
        /// If true, AxeWindows stopped successfully and has been cleaned up
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// Convert contents to a user-friendly description
        /// </summary>
        /// <returns>The user-friendly description</returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "StopCommandResult: Succeeded={0}, {1}",
                Succeeded, ToStringProtected());
        }
    }
}
