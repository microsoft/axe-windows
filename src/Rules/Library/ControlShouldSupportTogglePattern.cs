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
    [RuleInfo(ID = RuleId.ControlShouldSupportTogglePattern)]
    class ControlShouldSupportTogglePattern : Rule
    {
        public ControlShouldSupportTogglePattern()
        {
            Info.Description = Descriptions.ControlShouldSupportTogglePattern;
            Info.HowToFix = HowToFix.ControlShouldSupportTogglePattern;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Toggle.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            // Button is handled separately.
            return AppBar | CheckBox | SemanticZoom;
        }
    } // class
} // namespace
