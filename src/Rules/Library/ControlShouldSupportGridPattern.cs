// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.ControlShouldSupportGridPattern)]
    class ControlShouldSupportGridPattern : Rule
    {
        public ControlShouldSupportGridPattern()
        {
            Info.Description = Descriptions.ControlShouldSupportGridPattern;
            Info.HowToFix = HowToFix.ControlShouldSupportGridPattern;
            Info.Standard = A11yCriteriaId.AvailableActions;
            Info.PropertyID = PropertyType.UIA_IsGridPatternAvailablePropertyId;
            Info.ErrorCode = EvaluationCode.Error;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return Patterns.Grid.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return Calendar | DataGrid | Table;
        }
    } // class
} // namespace
