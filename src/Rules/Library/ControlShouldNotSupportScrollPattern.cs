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
    [RuleInfo(ID = RuleId.ControlShouldNotSupportScrollPattern)]
    class ControlShouldNotSupportScrollPattern : Rule
    {
        public ControlShouldNotSupportScrollPattern()
        {
            Info.Description = Descriptions.ControlShouldNotSupportScrollPattern;
            Info.HowToFix = HowToFix.ControlShouldNotSupportScrollPattern;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !Patterns.Scroll.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // ComboBox is handled in a separate rule
            return ScrollBar;
        }
    } // class
} // namespace
