// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Rules;

namespace Axe.Windows.Automation
{
    /// <summary>
    /// The result of a single rule test on a single element
    /// </summary>
    public class ScanResult
    {
        /// <summary>
        /// Information about the rule that evaluated the element specified by <see cref="Element"/>
        /// </summary>
        public RuleInfo Rule { get; internal set; }

        /// <summary>
        /// The element which was tested against the rule specified in <see cref="Rule"/>
        /// </summary>
        public ElementInfo Element { get; internal set; }
    } // class
} // namespace
