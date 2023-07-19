// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.Framework;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.EdgeBrowserHasBeenDeprecated)]
    class EdgeBrowserHasBeenDeprecated : Rule
    {
        // Testable constructor
        internal EdgeBrowserHasBeenDeprecated(Condition excludedCondition) : base (excludedCondition)
        {
            Info.Description = Descriptions.EdgeBrowserHasBeenDeprecated;
            Info.HowToFix = HowToFix.EdgeBrowserHasBeenDeprecated;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.ErrorCode = EvaluationCode.Error;
            Info.FrameworkIssueLink = "https://go.microsoft.com/fwlink/?linkid=2214421";
        }

        public EdgeBrowserHasBeenDeprecated() : this (excludedCondition: DefaultExcludedCondition)
        {
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return Edge;
        }
    }
}
