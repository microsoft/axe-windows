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
    [RuleInfo(ID = RuleId.ControlShouldNotSupportInvokePattern)]
    class ControlShouldNotSupportInvokePattern : Rule
    {
        public ControlShouldNotSupportInvokePattern()
        {
            Info.Description = Descriptions.ControlShouldNotSupportInvokePattern;
            Info.HowToFix = HowToFix.ControlShouldNotSupportInvokePattern;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !Patterns.Invoke.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return AppBar | TabItem;
        }
    } // class
} // namespace
