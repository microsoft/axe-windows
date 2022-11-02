// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.IsKeyboardFocusableShouldBeFalse)]
    class IsKeyboardFocusableShouldBeFalse : Rule
    {
        public IsKeyboardFocusableShouldBeFalse()
        {
            Info.Description = Descriptions.IsKeyboardFocusableShouldBeFalse;
            Info.HowToFix = HowToFix.IsKeyboardFocusableShouldBeFalse;
            Info.Standard = A11yCriteriaId.Keyboard;
            Info.PropertyID = PropertyType.UIA_IsKeyboardFocusablePropertyId;
            Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return IsNotKeyboardFocusable.Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return ElementGroups.ExpectedNotToBeFocusable;
        }
    } // class
} // namespace
