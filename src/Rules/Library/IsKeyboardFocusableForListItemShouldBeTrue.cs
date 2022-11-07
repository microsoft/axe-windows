// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using System;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;
using static Axe.Windows.Rules.PropertyConditions.Relationships;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.IsKeyboardFocusableForListItemShouldBeTrue)]
    class IsKeyboardFocusableForListItemShouldBeTrue : Rule
    {
        public IsKeyboardFocusableForListItemShouldBeTrue()
        {
            Info.Description = Descriptions.IsKeyboardFocusableForListItemShouldBeTrue;
            Info.HowToFix = HowToFix.IsKeyboardFocusableForListItemShouldBeTrue;
            Info.Standard = A11yCriteriaId.Keyboard;
            Info.PropertyID = PropertyType.UIA_IsKeyboardFocusablePropertyId;
            Info.ErrorCode = EvaluationCode.Warning;
        }

        public override bool PassesTest(IA11yElement e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));

            return !AnyChild(IsKeyboardFocusable).Matches(e);
        }

        protected override Condition CreateCondition()
        {
            return ListItem & IsNotKeyboardFocusable & IsNotOffScreen & IsEnabled & BoundingRectangle.Valid;
        }
    } // class
} // namespace
