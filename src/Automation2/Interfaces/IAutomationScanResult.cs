// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Enums;
using System;

namespace Automation2
{
    /// <summary>
    /// The result of a single rule test on a single element
    /// </summary>
    public interface IAutomationScanResult
    {
        /// <summary>
        /// The id of the rule that evaluated the element specified by <see cref="ElementId"/>
        /// </summary>
        /// <remarks>
        /// Use this as an indexer to the Axe.Windows.Rules.Rules.All dictionary,
        /// which will provide details about the rule.
        /// </remarks>
        RuleId RuleId { get; }

        /// <summary>
        /// The id of the element which was tested against the rule specified in <see cref="RuleId"/>
        /// </summary>
        int ElementId { get; }
    } // interface
} // namespace
