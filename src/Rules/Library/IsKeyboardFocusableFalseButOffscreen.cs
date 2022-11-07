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
    [RuleInfo(ID = RuleId.IsKeyboardFocusableFalseButOffscreen)]
    class IsKeyboardFocusableFalseButOffscreen : Rule
    {
        public IsKeyboardFocusableFalseButOffscreen()
        {
            Info.Description = Descriptions.IsKeyboardFocusableFalseButOffscreen;
            Info.HowToFix = HowToFix.IsKeyboardFocusableFalseButOffscreen;
            Info.Standard = A11yCriteriaId.Keyboard;
            Info.PropertyID = PropertyType.UIA_IsKeyboardFocusablePropertyId;
            Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return IsNotKeyboardFocusable & IsEnabled & IsOffScreen
                & ElementGroups.ExpectedToBeFocusable;
        }
    } // class
} // namespace
