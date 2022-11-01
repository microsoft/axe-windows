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
        public EdgeBrowserHasBeenDeprecated()
        {
            this.Info.Description = Descriptions.EdgeBrowserHasBeenDeprecated;
            this.Info.HowToFix = HowToFix.EdgeBrowserHasBeenDeprecated;
            this.Info.Standard = A11yCriteriaId.ObjectInformation;
            this.Info.ErrorCode = EvaluationCode.Error;
            this.Info.FrameworkIssueLink = "https://go.microsoft.com/fwlink/?linkid=2214421";
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
