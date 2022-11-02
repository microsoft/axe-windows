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
    [RuleInfo(ID = RuleId.SplitButtonInvokeAndTogglePatterns)]
    class SplitButtonInvokeAndTogglePatterns : Rule
    {
        public SplitButtonInvokeAndTogglePatterns()
        {
            Info.Description = Descriptions.SplitButtonInvokeAndTogglePatterns;
            Info.HowToFix = HowToFix.SplitButtonInvokeAndTogglePatterns;
            Info.Standard = A11yCriteriaId.NameRoleValue;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            var condition = (Patterns.Invoke | Patterns.Toggle)
                & ~(Patterns.Invoke & Patterns.Toggle);

            return condition.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return SplitButton;
        }
    }
}
