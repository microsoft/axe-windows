// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Axe.Windows.Core.Bases
{
    /// <summary>
    /// Extend IA11yElement to expose the properties that we need for unit tests.
    /// (IA11yElement exposes only what is needed for rules)
    /// </summary>
    internal interface ICoreA11yElement : IA11yElement
    {
        /// <summary>
        /// The Accelerator key for this element (null if property does not exist)
        /// </summary>
        string AcceleratorKey { get; }

        /// <summary>
        /// The Access key for this element (null if property does not exist)
        /// </summary>
        string AccessKey { get; }

        /// <summary>
        /// The Culture for this element (null if property does not exist)
        /// </summary>
        string Culture { get; }
    }
}
