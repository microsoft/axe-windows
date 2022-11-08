// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ButtonInvokeAndExpandCollapsePatterns)]
    class ButtonInvokeAndExpandCollapsePatterns : Rule
    {
        public ButtonInvokeAndExpandCollapsePatterns()
        {
            Info.Description = Descriptions.ButtonInvokeAndExpandCollapsePatterns;
            Info.HowToFix = HowToFix.ButtonInvokeAndExpandCollapsePatterns;
            Info.Standard = A11yCriteriaId.InfoAndRelationships;
            Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            var rule = Relationships.All(Patterns.Invoke, Patterns.ExpandCollapse);

            return !rule.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Button;
        }
    } // class
} // namespace
