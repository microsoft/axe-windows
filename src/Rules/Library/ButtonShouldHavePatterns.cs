// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ButtonShouldHavePatterns)]
    class ButtonShouldHavePatterns : Rule
    {
        public ButtonShouldHavePatterns()
        {
            Info.Description = Descriptions.ButtonShouldHavePatterns;
            Info.HowToFix = HowToFix.ButtonShouldHavePatterns;
            Info.Standard = A11yCriteriaId.NameRoleValue;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var condition = Relationships.Any(Patterns.ExpandCollapse, Patterns.Invoke, Patterns.Toggle);

            return condition.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Button;
        }
    } // class
} // namespace
