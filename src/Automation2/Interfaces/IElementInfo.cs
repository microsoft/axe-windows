// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Automation2
{
    /// <summary>
    /// Contains identifying information about an element
    /// </summary>
    public interface IElementInfo
    {
        /// <summary>
        /// The id of this element's parent element.
        /// </summary>
        /// <remarks>
        /// The parent can be found by using the given id as an index into <see cref="IAutomationScanResults.Elements"/>
        /// </remarks>
        int ParentId { get; }

        /// <summary>
        /// A string to string dictionary where the key is a UIAutomation property name
        /// and the value is the corresponding UI Automation property value.
        /// </summary>
        /// <remarks>
        /// The key strings are likely to be very stable, so it will be safe to index certain keys
        /// without fear of breaking changes.
        /// Only properties with values set will be included.
        /// </remarks>
        IReadOnlyDictionary<string, string> Properties { get; }
    } // interface
} // namespace
