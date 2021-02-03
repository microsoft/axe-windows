// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using System;
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldNotSupportTogglePattern)]
    class ControlShouldNotSupportTogglePattern : Rule
    {
        public ControlShouldNotSupportTogglePattern()
        {
            this.Info.Description = Descriptions.ControlShouldNotSupportTogglePattern;
            this.Info.HowToFix = HowToFix.ControlShouldNotSupportTogglePattern;
            this.Info.Standard = A11yCriteriaId.AvailableActions;
            this.Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !Patterns.Toggle.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return RadioButton;
        }
    } // class
} // namespace
