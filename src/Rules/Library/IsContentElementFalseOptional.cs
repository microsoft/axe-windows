// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Axe.Windows.Core.Bases;
using Axe.Windows.Core.Enums;
using Axe.Windows.Core.Types;
using Axe.Windows.Rules.PropertyConditions;
using Axe.Windows.Rules.Resources;
using static Axe.Windows.Rules.PropertyConditions.BoolProperties;
using static Axe.Windows.Rules.PropertyConditions.ControlType;

namespace Axe.Windows.Rules.Library
{
    [RuleInfo(ID = RuleId.IsContentElementFalseOptional)]
    class IsContentElementFalseOptional : Rule
    {
        public IsContentElementFalseOptional()
        {
            Info.Description = Descriptions.IsContentElementFalseOptional;
            Info.HowToFix = HowToFix.IsContentElementFalseOptional;
            Info.Standard = A11yCriteriaId.ObjectInformation;
            Info.PropertyID = PropertyType.UIA_IsContentElementPropertyId;
            Info.ErrorCode = EvaluationCode.NeedsReview;
        }

        public override bool PassesTest(IA11yElement e)
        {
            return false;
        }

        protected override Condition CreateCondition()
        {
            var controls = AppBar | Header | HeaderItem | MenuBar
                | ScrollBar | Separator | Thumb | TitleBar;

            return IsContentElement & controls & BoundingRectangle.Valid;
        }
    } // class
} // namespace
