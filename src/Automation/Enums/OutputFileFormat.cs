// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace Axe.Windows.Automation
{
#pragma warning disable CA1714 // We cannot change the enum name without breaking backward compatibility because the API has been released to the public
    /// <summary>
    /// Bit flags to specify which output file formats a <see cref="IScanner"/> should write
    /// </summary>
    [Flags]
    public enum OutputFileFormat
    {
        /// <summary>
        /// Create no output files
        /// </summary>
        None = 0,

        /// <summary>
        /// Create output files which can be opened using <see href="https://accessibilityinsights.io/docs/en/windows/overview">Accessibility Insights for Windows</see>.
        /// </summary>
        A11yTest = 1,
    } // enum
#pragma warning restore CA1714
} // namespace
