// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using System.Collections.Generic;

namespace Automation2
{
    /// <summary>
    /// Contains identifying information about an element
    /// </summary>
    public class ElementInfo
    {
        /// <summary>
        /// This element's parent element.
        /// </summary>
        public ElementInfo Parent { get; private set;  }

        /// <summary>
        /// A string to string dictionary where the key is a UIAutomation property name
        /// and the value is the corresponding UI Automation property value.
        /// </summary>
        /// <remarks>
        /// The key strings are likely to be very stable, so it will be safe to index certain keys
        /// without fear of breaking changes.
        /// Only properties with values set will be included.
        /// </remarks>
        public IReadOnlyDictionary<string, string> Properties { get; }

        /// <summary>
        /// A list of names of supported patterns
        /// </summary>
        /// <remarks>
        /// The names are likely to be very stable.
        /// </remarks>
        public IEnumerable<string> Patterns { get; private set; }
    } // class
} // namespace
