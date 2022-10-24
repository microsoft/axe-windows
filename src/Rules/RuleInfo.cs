// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System;
using System.Globalization;

namespace Axe.Windows.Rules
{
#pragma warning disable CA1710

    /// <summary>
    /// Provides metadata about a rule.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class RuleInfo : Attribute
    {
#pragma warning restore CA1710

        /// <summary>
        /// Contains a unique identifier for the rule from the RuleId enumeration.
        /// </summary>
        public RuleId ID { get; set; }

        /// <summary>
        /// Contains a short description of the rule.
        /// This is typically displayed in a list of rule results after a scan.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Detailed information on how to resolve a violation reported by the rule.
        /// </summary>
        public string HowToFix { get; set; }

        /// <summary>
        /// Indicates the standards documentation from which the rule was derived.
        /// </summary>
        public A11yCriteriaId Standard { get; set; }

        /// <summary>
        /// In cases where the rule tests one specific UI Automation property,
        /// this contains the UI Automation property ID in question.
        /// This property is used to link elements with rule violations to relevant documentation.
        /// </summary>
        public int PropertyID { get; set; }

        /// <summary>
        /// A description of the conditions under which a rule will be evaluated.
        /// This information is generated programatically when a rule inherits the Rule base class
        /// and is not meant to be changed.
        /// </summary>
        public string Condition { get; set; }

        /// <summary>
        /// The <see cref="EvaluationCode"/> to be returned
        /// in the case the test does not pass
        /// </summary>
        public EvaluationCode ErrorCode { get; set; } = EvaluationCode.NotSet;

        /// <summary>
        /// The link to a known framework issue that can cause this rule to fail.
        /// This field will be null if no known framework issue exists.
        /// </summary>
        public string FrameworkIssueLink { get; set; }

        /// <summary>
        /// Provides a string summary of the information contained in the RuleInfo object.
        /// </summary>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Descriptions.SummaryFormat, this.ID, this.Description, this.HowToFix, this.Condition);
        }
    } // class
} // namespace
