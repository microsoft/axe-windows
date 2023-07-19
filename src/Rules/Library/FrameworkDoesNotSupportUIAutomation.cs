// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using System.Text.RegularExpressions;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.FrameworkDoesNotSupportUIAutomation)]
    class FrameworkDoesNotSupportUIAutomation : Rule
    {
        private static readonly string[] KnownProblematicClasses =
        {
            @"^\s*SunAwt.*$",
        };

        // Testable constructor
        internal FrameworkDoesNotSupportUIAutomation(Condition excludedCondition) : base (excludedCondition)
        {
            Info.Description = Descriptions.FrameworkDoesNotSupportUIAutomation;
            Info.HowToFix = HowToFix.FrameworkDoesNotSupportUIAutomation;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.ErrorCode = EvaluationCode.Error;
            Info.FrameworkIssueLink = "https://go.microsoft.com/fwlink/?linkid=2214160";
        }

        public FrameworkDoesNotSupportUIAutomation() : this(excludedCondition: DefaultExcludedCondition)
        {
        }

        public override bool PassesTest(IA11yElement e)
        {
            string className = e.ClassName;

            foreach (var knownProblematicClass in KnownProblematicClasses)
            {
                if (Regex.IsMatch(className, knownProblematicClass, RegexOptions.CultureInvariant))
                    return false;
            }

            return true;
        }

        protected override Condition CreateCondition()
        {
            return Win32Framework & Window;
        }
    }
}
