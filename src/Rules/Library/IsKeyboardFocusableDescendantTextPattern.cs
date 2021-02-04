// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.Relationships;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.IsKeyboardFocusableDescendantTextPattern)]
    class IsKeyboardFocusableDescendantTextPattern : Rule
    {
        public IsKeyboardFocusableDescendantTextPattern()
        {
            this.Info.Description = Descriptions.IsKeyboardFocusableDescendantTextPattern;
            this.Info.HowToFix = HowToFix.IsKeyboardFocusableDescendantTextPattern;
            this.Info.Standard = A11yCriteriaId.Keyboard;
            this.Info.PropertyID = PropertyType.UIA_IsKeyboardFocusablePropertyId;
            this.Info.ErrorCode = EvaluationCode.Note;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            return IsContentOrControlElement & IsNotKeyboardFocusable & IsNotOffScreen & BoundingRectangle.Valid
                & Patterns.Text & AnyAncestor(Patterns.Text);
        }
    } // class
} // namespace
